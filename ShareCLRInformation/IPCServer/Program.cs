//#define ignorefailures
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using Landman.Rascal.CLRInfo.Protobuf;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Google.ProtocolBuffers;



namespace Landman.Rascal.CLRInfo.IPCServer
{
	static class IListExtension{
		public static void AddRange<T>(this IList<T> dst, IEnumerable<T> range){
			foreach (T item in range) {
				dst.Add(item);
			}	
		}
	}
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
					var request = InformationRequest.ParseDelimitedFrom(clientStream);
					var typesPartitioned = PartitionTypes(request.AssembliesList, 25);
					SendAmountOfPartitionMessage((uint)typesPartitioned.Count, clientStream);
					foreach (var typedGroup in typesPartitioned)
					{
						var groupResponse = GenerateResponseFor(typedGroup);
						groupResponse.WriteDelimitedTo(clientStream);
					}
				}

			}
		}

		private static void SendAmountOfPartitionMessage(uint sizeToSend, NetworkStream clientStream)
		{
			var outputStream = CodedOutputStream.CreateInstance(clientStream);
			outputStream.WriteRawVarint32(sizeToSend);
			outputStream.Flush();
		}

		public static List<List<TypeDefinition>> PartitionTypes(IEnumerable<String> assemblies, int partitionSize)
		{
			// create groups of partitionSize large
			return assemblies.Select(a => ModuleDefinition.ReadModule(a))
				.SelectMany(a => a.GetAllTypes())
				.Where(t => t.IsClass || t.IsEnum || t.IsInterface)
				.Where(t => t.Name != "<Module>")
				.Select((t, i) => new { Group = i / partitionSize, Type = t })
				.GroupBy(g => g.Group)
				.Select(grouped => grouped.Select(g => g.Type).ToList()).ToList();
		}
	

		public static InformationResponse GenerateResponseFor(IEnumerable<TypeDefinition> allTypes)
		{
			var result = new InformationResponse();
			
			var allMethods = allTypes.SelectMany(t => t.Methods).ToList();
#if ignorefailures
			try {
#endif
			result.TypesList.AddRange(allTypes.Select(t => GenerateEntity(t)));
#if ignorefailures
			} catch { }
			try {
#endif
			result.TypesInheritanceList.AddRange(allTypes.Where(t => t.BaseType != null).Select(t => GenerateEntityRel(t, t.BaseType)));
#if ignorefailures
			} catch { }
			try {
#endif
			result.TypesImplementingList.AddRange(allTypes.Where(t => t.Interfaces.Any())
				.SelectMany(t => t.Interfaces.Select(i => GenerateEntityRel(t, i))));
#if ignorefailures
			} catch { }
			try {
#endif
			result.MethodsList.AddRange(allMethods.Select(m => GenerateEntity(m)));
#if ignorefailures
			} catch { }
			try {
#endif
			result.MethodCallsList.AddRange(allMethods.Where(m => m.HasBody)
				.SelectMany(m => GenerateMethodCalls(m)));
#if ignorefailures
			} catch { }
			try {
#endif
			result.GenericConstrainsList.AddRange(allTypes.Where(t => t.HasGenericParameters)
				.SelectMany(t => t.GenericParameters.SelectMany(p => GenerateConstrainsRels(t, p))));
			result.GenericConstrainsList.AddRange(allMethods.Where(m => m.HasParameters)
				.SelectMany(m => m.Parameters.SelectMany(p => GenerateConstrainsRels(m, p))));
#if ignorefailures
			} catch { }
#endif

			return result;
		}


		private static IEnumerable<ConstrainRel> GenerateConstrainsRels(TypeReference entity, GenericParameter param)
		{
			return GenerateConstrainsRels(GenerateEntity(entity), GenerateConstrains(param));
		}
		private static IEnumerable<ConstrainRel> GenerateConstrainsRels(MethodDefinition method, ParameterDefinition param)
		{
			var currentEntity = GenerateEntity(method);
			var actualType = GetActualType(param);
			IEnumerable<Constrain> constrains = null;
			if (actualType.IsGenericParameter)
				constrains = GenerateConstrains((GenericParameter)actualType);
			else if (actualType.IsArray && ((TypeSpecification)actualType).ElementType.IsGenericParameter)
				constrains = GenerateConstrains((GenericParameter)(((TypeSpecification)actualType).ElementType));
			else
				return new List<ConstrainRel>();
			// create a new method entity with only one parameter
			var newEntity = currentEntity.ToBuilder();
			var newMethodId = newEntity.IdsList.Last().ToBuilder();
			newEntity.IdsList.Remove(newEntity.IdsList.Last());
			newMethodId.ClearParams();
			newMethodId.AddParams(GenerateParameter(param));
			newEntity.AddIds(newMethodId.Build());
			return GenerateConstrainsRels(newEntity.Build(), constrains);
		}

		private static IEnumerable<ConstrainRel> GenerateConstrainsRels(Entity entity, IEnumerable<Constrain> constrains)
		{
			return constrains.Select(c =>
				ConstrainRel.CreateBuilder()
					.SetEntity(entity)
					.SetConstrain(c)
					.Build());
		}

		private static EntityRel GenerateEntityRel(TypeDefinition @from, TypeReference to)
		{
			return GenerateEntityRel(GenerateEntity(@from), GenerateEntity(to));
		}

		private static EntityRel GenerateEntityRel(Entity @from, Entity to)
		{
			return EntityRel.CreateBuilder()
				.SetFrom(@from)
				.SetTo(to)
				.Build();
		}
		public static List<EntityRel> GenerateMethodCalls(MethodDefinition m)
		{
			var currentMethod = GenerateEntity(m);
			return m.Body.Instructions.Where(i => i.OpCode == OpCodes.Call || i.OpCode == OpCodes.Newobj || i.OpCode == OpCodes.Callvirt)
				.Select(i => i.Operand as MethodReference)
				.Where(mr => mr != null)
				.Select(mr => GenerateEntityRel(currentMethod, GenerateEntity(mr)))
				.ToList();
		}

		private static Entity GenerateEntity(MethodReference m)
		{
			if (m.DeclaringType.IsArray)
			{
				var result = Entity.CreateBuilder();
				
				var resultArray = Id.CreateBuilder();
				resultArray.Kind = Id.Types.IdKind.Array;
				resultArray.ElementType = GenerateEntity(m.DeclaringType);
				
				var returnType = Entity.CreateBuilder();
				returnType.IdsList.Add(resultArray.Build());
				
				var methodId = Id.CreateBuilder();
				methodId.Kind = Id.Types.IdKind.Constructor;
				methodId.ReturnType = returnType.Build();
				methodId.ParamsList.AddRange(m.Parameters.Select(p => GenerateParameter(p)));
				
				result.IdsList.Add(methodId.Build());
				return result.Build();
			}
			return GenerateEntity(m.Resolve());
		}


		private static Entity GenerateEntity(MethodDefinition m){
			var result = Entity.CreateBuilder();
			result.IdsList.AddRange(GenerateEntity(m.DeclaringType).IdsList); // class ref
			var methodId = Id.CreateBuilder();
			if (m.IsConstructor) {
				methodId.Kind = Id.Types.IdKind.Constructor;
			} else {
				methodId.Kind = Id.Types.IdKind.Method;
				methodId.Name = m.Name;
			}

			methodId.ReturnType = GenerateEntity(m.ReturnType);
			methodId.ParamsList.AddRange(m.Parameters.Select(p => GenerateParameter(p)));
			result.IdsList.Add(methodId.Build());
			return result.Build();
		}
		
		private static Entity GenerateParameter(ParameterDefinition p) {
			var result = Entity.CreateBuilder();
			var param = Id.CreateBuilder();
			param.Name = p.Name;
			var paramType = GetActualType(p);
			if (paramType.IsGenericParameter) {
				param.Kind = Id.Types.IdKind.TypeParameter;
			}
			else if (paramType.IsArray) {
				param.Kind = Id.Types.IdKind.Array;
				if (((TypeSpecification)paramType).ElementType.IsGenericParameter)
				{
					var elementType = Entity.CreateBuilder();
					var typeParamId = Id.CreateBuilder();
					typeParamId.Kind = Id.Types.IdKind.TypeParameter;
					typeParamId.Name = ((TypeSpecification)paramType).ElementType.Name;
					elementType.IdsList.Add(typeParamId.Build());					
					param.ElementType = elementType.Build();
				}
				else
				{
					param.ElementType = GenerateEntity(((TypeSpecification)paramType).ElementType.Resolve());
				}
			}
			else
			{
				return GenerateEntity(paramType);
			}
			result.IdsList.Add(param.Build());
			return result.Build();
		}

		private static TypeReference GetActualType(ParameterDefinition p)
		{
			var paramType = p.ParameterType;
			if (p.ParameterType.IsByReference)
			{
				paramType = ((ByReferenceType)p.ParameterType).ElementType;
			}
			return paramType;
		}
		
		private static Landman.Rascal.CLRInfo.Protobuf.Entity GenerateEntity (TypeReference t)
		{
			if (t.IsGenericParameter)
			{
				return GenerateEntity((GenericParameter)t);
			}
			if (t.IsArray)
			{
				var result = Entity.CreateBuilder();
				var arrayId = Id.CreateBuilder();
				arrayId.Kind = Id.Types.IdKind.Array;
				arrayId.ElementType = GenerateEntity(((TypeSpecification)t).ElementType);
				result.IdsList.Add(arrayId.Build());
				return result.Build();
			}
			if (t.IsByReference)
			{
				return GenerateEntity(((ByReferenceType)t).ElementType);
			}
			return GenerateEntity(t.Resolve());
		}

		private static Entity GenerateEntity(TypeDefinition t){
			var result = Entity.CreateBuilder();
			AddNamespacesToEntity(t, result);
			var entityList = new List<Id>();
			var currentType = t;
			while (currentType.IsNested)
			{
				entityList.Insert(0, GetEntityType(currentType));
				currentType = currentType.DeclaringType;
			}
			entityList.Insert(0, GetEntityType(currentType));
			result.IdsList.AddRange(entityList);

			return result.Build();
		}

		private static void AddNamespacesToEntity(TypeDefinition t, Entity.Builder result)
		{
			var currentNamespace = t.IsNested ? t.DeclaringType.Namespace : t.Namespace;
			result.IdsList.AddRange(currentNamespace.Split('.').Select(n => Id.CreateBuilder()
																   .SetKind(Id.Types.IdKind.Namespace).SetName(n).Build()));
		}
		
		private static Id GetEntityType(TypeDefinition currentType)
		{
			var entityId = Id.CreateBuilder();
			entityId.Name = currentType.Name;
			int genericNameTickIndex = entityId.Name.IndexOf('`');
			if (genericNameTickIndex != -1)
			{
				entityId.Name = entityId.Name.Substring(0, genericNameTickIndex);
			}
			if (currentType.IsClass)
			{
				if (entityId.Name.StartsWith("<>c__DisplayClass"))
				{
					entityId.Kind = Id.Types.IdKind.DisplayClass;
					entityId.Id_ = Int32.Parse(entityId.Name.Substring("<>c__DisplayClass".Length), System.Globalization.NumberStyles.HexNumber);
				}
				else if (entityId.Name.StartsWith("<>__AnonType"))
				{
					entityId.Kind = Id.Types.IdKind.AnonymousClass;
					entityId.Id_ = Int32.Parse(entityId.Name.Substring("<>__AnonType".Length), System.Globalization.NumberStyles.HexNumber);
				}
				else{
					entityId.Kind = currentType.HasGenericParameters ? Id.Types.IdKind.GenericClass : Id.Types.IdKind.Class;
				}
			}
			else if (currentType.IsInterface)
			{
				entityId.Kind = currentType.HasGenericParameters ? Id.Types.IdKind.GenericInterface : Id.Types.IdKind.Interface;
			}
			else if (currentType.IsEnum)
			{
				entityId.Kind = Id.Types.IdKind.Enumeration;
			}
			else
			{
				entityId.Kind = Id.Types.IdKind.Unkown;	
			}

			if (currentType.HasGenericParameters)
			{
				entityId.ParamsList.AddRange(currentType.GenericParameters.Select(p => GenerateEntity(p)));
			}
			return entityId.Build();
		}

		private static Entity GenerateEntity(GenericParameter p)
		{
			var result = Entity.CreateBuilder();
			if (p.IsGenericParameter)
			{
				var param = Id.CreateBuilder();
				param.Kind = Id.Types.IdKind.TypeParameter;
				param.Name = p.Name;
				result.IdsList.Add(param.Build());
			}
			return result.Build();
		}
		       
		private static IEnumerable<Constrain> GenerateConstrains(GenericParameter p)
		{
			var result = new List<Constrain>();
			foreach (var con in p.Constraints)
			{
				if (con.GetElementType().FullName == typeof(ValueType).FullName)
				{
					result.Add(Constrain.CreateBuilder().SetKind(Constrain.Types.ConstrainKind.IsStruct).Build());
				}
				else {
					var entity = GenerateEntity(con.GetElementType().Resolve()).ToBuilder();
					if (con.IsGenericInstance)
					{
						var actualRef = (GenericInstanceType)con;
						var actualType = entity.IdsList.Last().ToBuilder();
						entity.IdsList.RemoveAt(entity.IdsList.Count - 1);
						for (int paramIndex = 0; paramIndex < actualRef.GenericArguments.Count; paramIndex++)
						{
							actualType.ParamsList[paramIndex] = GenerateEntity(actualRef.GenericArguments[paramIndex].GetElementType().Resolve());
						}
						entity.IdsList.Add(actualType.Build());
					}
					result.Add(Constrain.CreateBuilder()
					    .SetKind(Constrain.Types.ConstrainKind.Entity)
					    .SetConstrainEntity(entity.Build()).Build());
						
				}
			}
			if (p.HasDefaultConstructorConstraint)
			{
				result.Add(Constrain.CreateBuilder().SetKind(Constrain.Types.ConstrainKind.HasConstructor).Build());
			}
			if (result.Count == 0)
			{
				result.Add(Constrain.CreateBuilder().SetKind(Constrain.Types.ConstrainKind.IsClass).Build());
			}
			return result;
		}
				                 
	}
}
