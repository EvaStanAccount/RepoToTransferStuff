using System;
using System.DirectoryServices;
using System.Collections.Generic;
namespace Ldap
{
    public class Ldap
    {
        static int tableWidth = 100;
        
        public static void Main(string[] args)
        {
            var arguments = new Dictionary<string, string>();
            foreach (var argument in args)
            {
                var idx = argument.IndexOf(':');
                if (idx > 0)
                {
                    arguments[argument.Substring(0, idx)] = argument.Substring(idx + 1);
                }
                else
                {
                    idx = argument.IndexOf('=');
                    if (idx > 0)
                    {
                        arguments[argument.Substring(0, idx)] = argument.Substring(idx + 1);
                    }
                    else
                    {
                        arguments[argument] = string.Empty;
                    }
                }
            }
            string command = args[0];
            
            string domain = "";
            string query = "";
            string uname = "";
            string password = "";
            if (arguments.ContainsKey("/domain"))
            {
                domain = arguments["/domain"];
            }
            if (arguments.ContainsKey("/query"))
            {
                query = arguments["/query"];
            }
            if (arguments.ContainsKey("/user"))
            {
                uname = arguments["/user"];
            }
            if (arguments.ContainsKey("/password"))
            {
                password = arguments["/password"];
            }

            if (command == "query")
            {
                Query(domain,query,uname,password);
            }
            else if (command == "GetDomainUsers")
            {
                Query(domain,"(objectclass=user)",uname,password);
            }
            else if (command == "GetDomainComputers")
            {
                Query(domain, "(objectclass=computer)",uname,password);
            }
            else
            {
                Console.WriteLine("[-] Unrecognized command provided...");
            }
            
            
                
            
        }
        
        static void Query(string domain, string query, string uname, string password)
        {
            DirectoryEntry entry;
            if (uname != "" && password != "")
            {
                 entry = new DirectoryEntry(String.Format("LDAP://{0}", domain),uname,password);
            }
            else 
            {
                 if (domain == "")
                 {
                    domain = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[0];
                 }
                 entry = new DirectoryEntry(String.Format("LDAP://{0}", domain));
                 
            }
            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = (query);
            searcher.SizeLimit = int.MaxValue;
            searcher.PageSize = int.MaxValue;
            
            foreach(SearchResult res in searcher.FindAll())
            {
                if (res.Properties.Contains("samaccountname"))
                {
                    PrintRow(res.Properties["samaccountname"][0].ToString(),"Properties:");
                }
                else 
                {
                    PrintRow("PropertyName:","Value:");
                }
                
                PrintLine();
                foreach (string property in res.Properties.PropertyNames)
                {
                    foreach (Object collection in res.Properties[property])
                    {
                        PrintRow(property,collection.ToString());
                    }
                }
		PrintLine();
            }
            
            searcher.Dispose();
            entry.Dispose();
        }
        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            
            foreach (string column in columns)
            {

                row += AlignCentre(column, width) + "|";
    
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width) + "|\n" + text.Substring((width)): text;
            
            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
