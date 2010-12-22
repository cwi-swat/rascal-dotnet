using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using ProtoBuf;
using Landman.Rascal.CLRInfo.Protobuf;

namespace TestProject
{
	public class S<T> where T : struct
	{
	}
	public class C<T> where T : class
	{
	}
	public class CL<T> where T : ICloneable, IEnumerable<String>
	{
	}
	public class COMP<T> where T : StringComparer
	{
	}

	public class CON<T> where T : new()
	{
	}
	
	public static class ListExt{
		public static T Average<T>(this CON<T> source) where T: new()
		{
			return default(T);
		}
		
	}


	class Program
	{
		static void Main(string[] args)
		{
		/*	var result = Landman.Rascal.CLRInfo.IPCServer.Program.HandleRequest(
				new InformationRequest { Assembly = Assembly.GetExecutingAssembly().Location });*/
			var result = Serializer.Deserialize<InformationResponse>(new FileStream("message.prot", FileMode.Open));
			Console.WriteLine(result.ToString());
		}
	}
}
