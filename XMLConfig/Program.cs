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
      
        static void Main(string[] args)
        {
            string folder = ConfigurationManager.AppSettings["Ext:XmlCommandFolder"];
            XmlCommandManager.InitXML(folder);
            string StrSQL= XmlCommandManager.GetCommand("DeskTop:GetConfig").CommandText;
            string strSQL1 = XmlCommandManager.GetCommand("GetUser:Query").CommandText;
            Console.WriteLine(StrSQL);
            Console.WriteLine(strSQL1);
            Console.ReadKey();
        }

    }
}
