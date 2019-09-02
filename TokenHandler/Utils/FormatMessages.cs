using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenHandler.Utils
{
    /// <summary>
    /// Tool to user friendly format log messages.
    /// </summary>
    public static class FormatMessages
    {      
        /// <summary>
        /// Helper to format logs
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="methodParams"></param>
        /// <param name="exceptionMessage"></param>
        /// <returns></returns>
        public static string getLogMessage(string className, string methodName, string methodParams, string exceptionMessage="" ) { 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine().Append(" . Class name: ").Append(className);
            sb.AppendLine().Append(" . MetodName name: ").Append(methodName);
            sb.AppendLine().Append(" . Metod params: ").Append(methodParams);
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                sb.AppendLine().Append(" . Exception: ").Append(exceptionMessage);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Helper to format reflection
        /// </summary>
        /// <param name="title"></param>
        /// <param name="reflectedInfo"></param>
        /// <returns></returns>
        public static string GetFormatedReflection(string title, string reflectedInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine().Append("------------------------------------------>").Append(title).Append("");
            sb.AppendLine().Append(reflectedInfo);
            sb.AppendLine().Append("<-----------------------------------------");
            sb.AppendLine().AppendLine();
            return sb.ToString();        
        }
    }
}
