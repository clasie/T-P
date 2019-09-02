using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TokenHandler.Constants;

namespace TokenHandler.Utils
{
    /// <summary>
    /// To help tracing In/Out movements trough the Web service we need to analyse
    /// and store this trafic made of list of business objects. Here we will detect
    /// which instance is going in/out in a dynamic reflection way without knowing  the type 
    /// by advance.
    /// </summary>
    public class Reflection
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace); 
        #endregion

        #region Methods
        public static Reflection GetReflection() {
            return new Reflection();
        }
        /// <summary>
        /// Will parse properties of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectPut"></param>
        /// <returns></returns>
        public string GetValues<T>(List<T> objectPut)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var elementSup in objectPut)
                {
                    Type type = elementSup.GetType();
                    List<string> listPropNameValues = new List<string>();
                    foreach (PropertyInfo propertyInfo in type.GetProperties())
                    {
                        if (propertyInfo.CanRead)
                        {
                            try
                            {
                                var propName = propertyInfo.Name;
                                var propValue = propertyInfo.GetValue(elementSup, null);
                                listPropNameValues.Add(string.Format(" {0} : {1} ", propName, propValue));
                            }
                            catch (Exception ex)
                            {
                                log.Error(FormatMessages.getLogMessage(
                                    this.GetType().Name,
                                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                                    string.Concat("Reflection.GetValues : ", elementSup.ToString()),
                                    ex.ToString()));
                            }
                        }
                    }
                    sb.Append(FormatProperties(type.Name, listPropNameValues));
                }
                return sb.ToString();
            }
            catch (Exception ex) {
                log.Error(FormatMessages.getLogMessage(
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    string.Concat("objectPut : ", objectPut.ToString()),
                    ex.ToString()));
                return string.Empty;
            }
        }
        /// <summary>
        /// Helper to format string.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="listNameValue"></param>
        /// <returns></returns>
        public string FormatProperties(String className, List<string> listNameValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.Append(" Class name: ").Append(className);
                sb.AppendLine();
                foreach (var element in listNameValue)
                {
                    sb.AppendLine(element);
                }
                return sb.ToString(); ;
            }
            catch (Exception ex) {
                log.Error(FormatMessages.getLogMessage(
                    this.GetType().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    string.Concat("className : ", className, "  listNameValue: ", listNameValue.ToString()),
                    ex.ToString()));
                return string.Empty;
            }
        }
        #endregion
    }
}
