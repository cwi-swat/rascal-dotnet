using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
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
		internal string TestInternal()
		{
			return "TestInternal";
		}
		protected virtual string TestProtected()
		{
			Result = 129;
			return "Test protected";
		}
		private Int16 Result;
		
		public String Testing {
			get;
			private set;
		}
		
		public IDictionary<C<String>, T> TestCollection() {
			return null;	
		}
	}
	
	public enum TestEnum {
		FirstValue = 0,
		SecondValue = 4
	}

	internal class CON<T> where T : StringComparer, new()
	{
		protected string TestCall(COMP<T> firstParam, CL<List<String>> secondParam)
		{
			return secondParam.Equals(null).ToString();	
		}
	}
	
	internal static class ListExt{
		public static T Average<T>(this CON<T> source) where T: StringComparer, new()
		{
			return default(T);
		}
		
	}


	class Program
	{
		static void Main(string[] args)
		{
			var types = Landman.Rascal.CLRInfo.IPCServer.Program.PartitionTypes(new String[] { Assembly.GetExecutingAssembly().Location }, 100);
			var result = Landman.Rascal.CLRInfo.IPCServer.Program.GenerateResponseFor(types.First());
			Console.WriteLine(result.ToString());
		}
	}
}
