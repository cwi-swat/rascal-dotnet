#define ignorefailures
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using ProtoBuf;
using Landman.Rascal.CLRInfo.Protobuf;
using MiscUtil.IO;
using MiscUtil.Conversion;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;



namespace Landman.Rascal.CLRInfo.IPCServer
{
	public class Program
	{



		static void Main(string[] args)
		{
			var server = new TcpListener(IPAddress.Loopback, 5555);
			server.Start();
			while (true)
			{
				var newClient = server.AcceptTcpClient();
				Console.WriteLine("Got new client: " + newClient.Client.ToString());
				using (var clientStream = newClient.GetStream())
				{
					var streamReader = new EndianBinaryReader(new BigEndianBitConverter(), clientStream);
					var size = streamReader.ReadInt32();
					if (size == 0)
						return;
					var message = streamReader.ReadBytes(size);
					var request = Serializer.Deserialize<InformationRequest>(new MemoryStream(message));
					var result = HandleRequest(request);
					var resultStream = new MemoryStream();
					Serializer.Serialize(resultStream, result);
					var streamWriter = new EndianBinaryWriter(new BigEndianBitConverter(), clientStream);
					streamWriter.Write((int)resultStream.Length);
					resultStream.WriteTo(clientStream);
				}

			}
		}

	

		public static InformationResponse HandleRequest(InformationRequest request)
		{
			var result = new InformationResponse();
			
			var allTypes = request.Assemblies.Select(a => ModuleDefinition.ReadModule(a))
				.SelectMany(a => a.GetAllTypes()).Where(t => t.IsClass || t.IsEnum || t.IsInterface)
					.Where(t => !t.Name.StartsWith("<>c__DisplayClass")) // remove delegate compiler hack classes
					.Where(t => t.Name != "<Module>").ToList();
			try {
			result.Types.AddRange(allTypes.Select(t => GenerateEntity(t)));
			} catch {
#if !ignorefailures
				throw;
#endif
			}
			try {
			result.TypesInheritance.AddRange(allTypes.Where(t => t.BaseType != null).Select(t => GenerateEntityRel(t)));
			} catch {
#if !ignorefailures
				throw;
#endif
			}
			try {
			result.TypesImplementing.AddRange(allTypes.Where(t => t.Interfaces.Any())
				.SelectMany(t => t.Interfaces.Select(i => 
					new EntityRel { 
						From = GenerateEntity(t),
						To = GenerateEntity(i.Resolve())
					}))
				);
			} catch {
#if !ignorefailures
				throw;
#endif
			}
			try {
			result.Methods.AddRange(allTypes.SelectMany(t => t.Methods).Select(m => GenerateEntity(m)));
			} catch {
#if !ignorefailures
				throw;
#endif
			}
			try {               
			result.MethodCalls.AddRange(allTypes.Where(t => t.Methods.Any()).SelectMany(t => t.Methods)
				.Where(m => m.HasBody)
				.SelectMany(m => GenerateMethodCalls(m)));
			} catch {
#if !ignorefailures
				throw;
#endif
			}
			return result;
		}
	
		public static List<EntityRel> GenerateMethodCalls(MethodDefinition m)
		{
			var currentMethod = GenerateEntity(m);
			var result = new List<EntityRel>();
			try {
				ILParser.Parse(m, new MethodExtractor(result, currentMethod));
			}
			catch {
#if !ignorefailures
				throw;
#endif				
			}
			return result;
		}
		
		private class MethodExtractor : IILVisitor {
			private List<EntityRel> destination;
			private Entity callingMethod;
			public MethodExtractor(List<EntityRel> destination, Entity callingMethod)
			{
				this.destination = destination;
				this.callingMethod = callingMethod;
			}	
		
			public void OnInlineMethod (Mono.Cecil.Cil.OpCode opcode, MethodReference method)
			{
				destination.Add(new EntityRel { From = callingMethod, To = GenerateEntity(method.Resolve()) });
			}


			#region IILVisitor implementation
			public  void OnInlineNone (Mono.Cecil.Cil.OpCode opcode)
			{
				
			}

			public  void OnInlineSByte (Mono.Cecil.Cil.OpCode opcode, sbyte value)
			{
				
			}

			public  void OnInlineByte (Mono.Cecil.Cil.OpCode opcode, byte value)
			{
			}

			public  void OnInlineInt32 (Mono.Cecil.Cil.OpCode opcode, int value)
			{
			}

			public  void OnInlineInt64 (Mono.Cecil.Cil.OpCode opcode, long value)
			{
			}

			public  void OnInlineSingle (Mono.Cecil.Cil.OpCode opcode, float value)
			{
			}

			public  void OnInlineDouble (Mono.Cecil.Cil.OpCode opcode, double value)
			{
			}

			public  void OnInlineString (Mono.Cecil.Cil.OpCode opcode, string value)
			{
			}

			public  void OnInlineBranch (Mono.Cecil.Cil.OpCode opcode, int offset)
			{
			}

			public  void OnInlineSwitch (Mono.Cecil.Cil.OpCode opcode, int[] offsets)
			{
			}

			public  void OnInlineVariable (Mono.Cecil.Cil.OpCode opcode, VariableDefinition variable)
			{
				
			}

			public  void OnInlineArgument (Mono.Cecil.Cil.OpCode opcode, ParameterDefinition parameter)
			{
				
			}

			public  void OnInlineSignature (Mono.Cecil.Cil.OpCode opcode, CallSite callSite)
			{
				
			}

			public  void OnInlineType (Mono.Cecil.Cil.OpCode opcode, TypeReference type)
			{
				
			}

			public  void OnInlineField (Mono.Cecil.Cil.OpCode opcode, FieldReference field)
			{
				
			}
			#endregion
		}
		
		private static EntityRel GenerateEntityRel(TypeDefinition t){
			return new EntityRel { From = GenerateEntity(t), To = GenerateEntity(t.BaseType.Resolve()) };	
		}
		
		private static Entity GenerateEntity(MethodDefinition m){
			var result = new Entity();
			result.Ids.AddRange(GenerateEntity(m.DeclaringType).Ids); // class ref
			Id methodId = null;
			if (m.IsConstructor)
				methodId = new Id { Kind = Id.IdKind.Constructor };
			else 
				methodId = new Id { Kind = Id.IdKind.Method, Name = m.Name };
			if (m.ReturnType.IsGenericParameter)
				methodId.ReturnType = GenerateEntity((GenericParameter)(m.ReturnType));
			else 
				methodId.ReturnType = GenerateEntity(m.ReturnType.Resolve());
			methodId.Params.AddRange(m.Parameters.Select(p => GenerateParameter(p)));
			result.Ids.Add(methodId);
			return result;
		}
		
		private static Entity GenerateParameter(ParameterDefinition p) {
			var result = new Entity();
			var param = new Id { Name = p.Name };
			var paramType = p.ParameterType;
			if (p.ParameterType.IsByReference)
			{
				paramType = ((ByReferenceType)p.ParameterType).ElementType;
			}
			if (paramType.IsGenericParameter) {
				param.Kind = Id.IdKind.TypeParameter;
				AddConstrainsToParam(param, (GenericParameter)paramType);
			}
			else
			{
				param.Kind = Id.IdKind.Parameter;
				param.ElementType = GenerateEntity(paramType.Resolve());
			}
			result.Ids.Add(param);
			return result;
		}
		
		private static Entity GenerateEntity(TypeDefinition t){
			var result = new Entity();
			var currentNamespace = t.IsNested ? t.DeclaringType.Namespace : t.Namespace;
			result.Ids.AddRange(currentNamespace.Split('.').Select(n => new Id { Kind = Id.IdKind.Namespace, Name = n }));
			var entityList = new List<Id>();
			var currentType = t;
			while (currentType.IsNested) {
				entityList.Insert(0, GetEntityType(currentType));
				currentType = currentType.DeclaringType;
			}
			entityList.Insert(0, GetEntityType(currentType));
			result.Ids.AddRange(entityList);
			return result;
		}
		private static Id GetEntityType(TypeDefinition currentType)
		{
			Id entityId = new Id { Name = currentType.Name };
			int genericNameTickIndex = entityId.Name.IndexOf('`');
			if (genericNameTickIndex != -1)
			{
				entityId.Name = entityId.Name.Substring(0, genericNameTickIndex);
			}
			if (currentType.IsClass)
			{
				entityId.Kind = currentType.HasGenericParameters ? Id.IdKind.GenericClass : Id.IdKind.Class;
			}
			else if (currentType.IsInterface)
			{
				entityId.Kind = currentType.HasGenericParameters ? Id.IdKind.GenericInterface : Id.IdKind.Interface;
			}
			else if (currentType.IsEnum)
			{
				entityId.Kind = Id.IdKind.Enumeration;
			}
			else
			{
				entityId.Kind = Id.IdKind.Unkown;	
			}

			if (currentType.HasGenericParameters)
			{
				entityId.Params.AddRange(currentType.GenericParameters.Select(p => GenerateEntity(p)));
			}
			return entityId;
		}

		private static Entity GenerateEntity(GenericParameter p)
		{
			var result = new Entity();
			if (p.IsGenericParameter)
			{
				var param = new Id { Kind = Id.IdKind.TypeParameter };
				param.Name = p.Name;
				AddConstrainsToParam(param, p);
				result.Ids.Add(param);
			}
			else
			{
				result = result;
			}
			return result;
		}
		       
		private static void AddConstrainsToParam(Id param, GenericParameter p)
		{
		foreach (var con in p.Constraints)
				{
					if (con.GetElementType().FullName == typeof(ValueType).FullName)
					{
						param.Constrains.Add(new Constrain { Kind = Constrain.ConstrainKind.IsStruct });
					}
					else {
						var entity = GenerateEntity(con.GetElementType().Resolve());
						if (con.IsGenericInstance)
						{
							var actualRef = (GenericInstanceType)con;
							for (int paramIndex = 0; paramIndex < actualRef.GenericArguments.Count; paramIndex++)
							{
								entity.Ids.Last().Params[paramIndex] = GenerateEntity(actualRef.GenericArguments[paramIndex].GetElementType().Resolve());
							}
						}
						param.Constrains.Add(new Constrain { Kind = Constrain.ConstrainKind.Entity, ConstrainEntity = entity });
						
					}
				}
				if (p.HasDefaultConstructorConstraint)
				{
					param.Constrains.Add(new Constrain { Kind = Constrain.ConstrainKind.HasConstructor });
				}
				if (param.Constrains.Count == 0)
				{
					param.Constrains.Add(new Constrain { Kind = Constrain.ConstrainKind.IsClass });
				}
	
		}
				                 
	}
}
