using PriorityQueue.Constant;
using PriorityQueue.Enum;
using SideWsComptaPlus.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using TokenHandler.Constants;
using TokenHandler.Utils;

namespace PriorityQueue.Utils 
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeDetector
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        private static readonly log4net.ILog logInOut = log4net.LogManager.GetLogger(TokenKey.WebInOutLogsNameSpace);
        private static readonly log4net.ILog logQueue = log4net.LogManager.GetLogger(TokenKey.QueueLogsNameSpace);
        #endregion

        #region
        /// <summary>
        /// Extracts the Type name from contract list.
        /// </summary>
        /// <param name="listOfInterfaceObjects"></param>
        /// <returns></returns>
        public static string GetInterfaceTypeNameFromList(object listOfInterfaceObjects) {
            try { 
                return GetInterfaceTypeFromList(listOfInterfaceObjects).Name;
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "TypeDetector",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                throw ex;
            }
        }
        /// <summary>
        /// Extracts the Type from contract list.
        /// </summary>
        /// <param name="listOfInterfaceObjects"></param>
        /// <returns></returns>
        public static Type GetInterfaceTypeFromList(object listOfInterfaceObjects)
        {
            try {
                if (listOfInterfaceObjects != null) {
                    log.Info("List not NULL");
                    return listOfInterfaceObjects.GetType().GetGenericArguments().Single();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "TypeDetector 3.0",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                throw ex;
            }
        }
        /// <summary>
        /// Extracts the Type name from xml string.
        /// </summary>
        /// <param name="serializedAsXmlListOfInterfaces"></param>
        /// <returns></returns>
        public static string GetInterfaceTypeNameFromXml(string serializedAsXmlListOfInterfaces) {
            try { 
                var xDocument = XDocument.Parse(serializedAsXmlListOfInterfaces);
                return xDocument.Root.Name.ToString().Substring(ContractConst.XmlStartString.Length);
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "TypeDetector",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                return ContractEnum.Unknown.ToString();
            }
        }
        #endregion
    }
}