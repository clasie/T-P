using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenHandler.Models
{
    public class Xml 
    {
        /*
        /// <summary>
        /// E:\WebServiceComptaPlus-Int\Log
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="serizalizedRequest"></param>
        public static void Insert(Guid gid, string serizalizedRequest) { 
            //using (StreamWriter file = new StreamWriter($"C:\\inetpub\\WebServiceComptaPlus\\Log\\{gid}.txt"))
            using (StreamWriter file = new StreamWriter($"E:\\WebServiceComptaPlus-Int\\Log\\{gid}.txt"))
            {
                file.WriteLine(serizalizedRequest);
                file.Close();
            }
        }
        public static string Select(Guid gid)
        {
            string line;
            //using (StreamReader reader = new StreamReader($"C:\\inetpub\\WebServiceComptaPlus\\Log\\{gid}.txt"))
            using (StreamReader reader = new StreamReader($"E:\\WebServiceComptaPlus-Int\\Log\\{gid}.txt"))
            {
                line = reader.ReadToEnd();
                reader.Close();
                return line;
            }
        }
        */
    }
}
