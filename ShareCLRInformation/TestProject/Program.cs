using System;
using System.Collections;
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
	public class CL<T> where T : ICollection, IEnumerable<String>
	{
	}
	public class COMP<T> where T : StringComparer
	{
	}

	public class CON<T> where T : StringComparer, new()
	{
		public string TestCall(COMP<T> firstParam, CL<List<String>> secondParam)
		{
			return secondParam.Equals(null).ToString();	
		}
	}
	
	public static class ListExt{
		public static T Average<T>(this CON<T> source) where T: StringComparer, new()
		{
			return default(T);
		}
		
	}


	class Program
	{
		static void Main(string[] args)
		{
			var req = new InformationRequest();
			req.Assemblies.Add(Assembly.GetExecutingAssembly().Location);
			var result = Landman.Rascal.CLRInfo.IPCServer.Program.HandleRequest(req);
			Console.WriteLine(result.ToString());
		}
	}
}
