using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLConfing
{
    class Program
    {
        private static readonly bool IsAspnetApp = string.IsNullOrEmpty(System.Web.HttpRuntime.AppDomainAppId) == false;
        static void Main(string[] args)
        {
            string folder = ConfigurationManager.AppSettings["Ext:XmlCommandFolder"];
            if (false == string.IsNullOrEmpty(folder))
            {
                string[] folders = folder.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < folders.Length; i++)
                {
                    folders[i] = System.IO.Path.Combine(RuntimeFolder, folders[i]);
                }

                XmlCommandManager.LoadCommnads(folders);
            }
            string StrSQL= XmlCommandManager.GetCommand("DeskTop:GetConfig").CommandText;
            string strSQL1 = XmlCommandManager.GetCommand("GetUser:Query").CommandText;
            Console.WriteLine(StrSQL);
            Console.WriteLine(strSQL1);
            Console.ReadKey();
        }
        private static string RuntimeFolder
        {
            get
            {
                if (IsAspnetApp)
                    return System.Web.HttpRuntime.AppDomainAppPath;

                else
                    return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
