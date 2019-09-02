using PriorityQueue.Enum;
using SideWsComptaPlus.Contracts;
using SideWsComptaPlus.ModelBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TokenHandler.Constants;
using TokenHandler.Utils;

namespace PriorityQueue.Utils 
{
    /// <summary>
    /// This class manages the serialization/deserializations Contracts<->Xml
    /// </summary>
    public class Serialization
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(TokenKey.NormalLogsNameSpace);
        #endregion
        public static void SerializationInOrOut() {
        }
        #region Methods
        /// <summary>
        /// Convert contracts to xml string.
        /// </summary>
        /// <param name="listToSerialize"></param>
        /// <returns></returns>
        public static string ListToString(object listToSerialize)
        {
            XmlSerializer serializer = null;
            string serializedValue = string.Empty;
            try { 
                switch (TypeDetector.GetInterfaceTypeNameFromList(listToSerialize)){
                    //Response
                    case nameof(ContractEnum.Response):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(Response) };
                            serializer = new XmlSerializer(typeof(List<Response>), personTypes);
                            serializer.Serialize(textWriter, (List<Response>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //PcmnIn01
                    case nameof(ContractEnum.PcmnIn01):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(PcmnIn01) };
                            serializer = new XmlSerializer(typeof(List<PcmnIn01>), personTypes);
                            serializer.Serialize(textWriter, (List<PcmnIn01>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //TaxGroupIn361
                    case nameof(ContractEnum.TaxGroupIn361):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxGroupIn361) };
                            serializer = new XmlSerializer(typeof(List<TaxGroupIn361>), personTypes);
                            serializer.Serialize(textWriter, (List<TaxGroupIn361>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //TaxItemGroupIn362
                    case nameof(ContractEnum.TaxItemGroupIn362):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxItemGroupIn362) };
                            serializer = new XmlSerializer(typeof(List<TaxItemGroupIn362>), personTypes);
                            serializer.Serialize(textWriter, (List<TaxItemGroupIn362>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //TaxCodeIn363
                    case nameof(ContractEnum.TaxCodeIn363):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeIn363) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeIn363>), personTypes);
                            serializer.Serialize(textWriter, (List<TaxCodeIn363>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //TaxCodeTaxGroupIn364
                    case nameof(ContractEnum.TaxCodeTaxGroupIn364):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeTaxGroupIn364) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeTaxGroupIn364>), personTypes);
                            serializer.Serialize(textWriter, (List<TaxCodeTaxGroupIn364>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //TaxCodeTaxItemGroupIN365
                    case nameof(ContractEnum.TaxCodeTaxItemGroupIN365):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeTaxItemGroupIN365) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeTaxItemGroupIN365>), personTypes);
                            serializer.Serialize(textWriter, (List<TaxCodeTaxItemGroupIN365>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //TaxCodeValueIN366
                    case nameof(ContractEnum.TaxCodeValueIN366):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeValueIN366) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeValueIN366>), personTypes);
                            serializer.Serialize(textWriter, (List<TaxCodeValueIN366>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //TaxCodeLanguageTxtIN367
                    case nameof(ContractEnum.TaxCodeLanguageTxtIN367):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeLanguageTxtIN367) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeLanguageTxtIN367>), personTypes);
                            serializer.Serialize(textWriter, (List<TaxCodeLanguageTxtIN367>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //DimensionAttributeSetupIn03
                    case nameof(ContractEnum.DimensionAttributeSetupIn03):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(DimensionAttributeSetupIn03) };
                            serializer = new XmlSerializer(typeof(List<DimensionAttributeSetupIn03>), personTypes);
                            serializer.Serialize(textWriter, (List<DimensionAttributeSetupIn03>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //DimensionAttributeValueIn03
                    case nameof(ContractEnum.DimensionAttributeValueIn03):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(DimensionAttributeValueIn03) };
                            serializer = new XmlSerializer(typeof(List<DimensionAttributeValueIn03>), personTypes);
                            serializer.Serialize(textWriter, (List<DimensionAttributeValueIn03>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //LedgerJournalNameIn43
                    case nameof(ContractEnum.LedgerJournalNameIn43):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(LedgerJournalNameIn43) };
                            serializer = new XmlSerializer(typeof(List<LedgerJournalNameIn43>), personTypes);
                            serializer.Serialize(textWriter, (List<LedgerJournalNameIn43>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    //FiscalCalendarPeriodIN22
                    case nameof(ContractEnum.FiscalCalendarPeriodIN22):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(FiscalCalendarPeriodIN22) };
                            serializer = new XmlSerializer(typeof(List<FiscalCalendarPeriodIN22>), personTypes);
                            serializer.Serialize(textWriter, (List<FiscalCalendarPeriodIN22>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    case nameof(ContractEnum.AssetService):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(AssetService) };
                            serializer = new XmlSerializer(typeof(List<AssetService>), personTypes);
                            serializer.Serialize(textWriter, (List<AssetService>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    case nameof(ContractEnum.AssetBook):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(AssetBook) };
                            serializer = new XmlSerializer(typeof(List<AssetBook>), personTypes);
                            serializer.Serialize(textWriter, (List<AssetBook>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                    case nameof(ContractEnum.AssetDepreciation):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(AssetDepreciation) };
                            serializer = new XmlSerializer(typeof(List<AssetDepreciation>), personTypes);
                            serializer.Serialize(textWriter, (List<AssetDepreciation>)listToSerialize);
                            serializedValue = textWriter.ToString();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "Serialization",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
            }
            return serializedValue;
        }
        /// <summary>
        /// Convert Xml string to Contracts.
        /// </summary>
        /// <param name="serializedListOfObjects"></param>
        /// <returns></returns>
        public static object StringToList(string serializedListOfObjects) {
            XmlSerializer serializer = null;
            try { 
                switch (TypeDetector.GetInterfaceTypeNameFromXml(serializedListOfObjects))
                {
                    //Response
                    case nameof(ContractEnum.Response):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(Response) };
                            serializer = new XmlSerializer(typeof(List<Response>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<Response>)serializer.Deserialize(sr);
                            }
                        }
                    //PcmnIn01
                    case nameof(ContractEnum.PcmnIn01):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(PcmnIn01) };
                            serializer = new XmlSerializer(typeof(List<PcmnIn01>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List< PcmnIn01 >) serializer.Deserialize(sr);
                            }
                        }
                    //TaxGroupIn361
                    case nameof(ContractEnum.TaxGroupIn361):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxGroupIn361) };
                            serializer = new XmlSerializer(typeof(List<TaxGroupIn361>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<TaxGroupIn361>)serializer.Deserialize(sr);
                            }
                        }
                    //TaxItemGroupIn362
                    case nameof(ContractEnum.TaxItemGroupIn362):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxItemGroupIn362) };
                            serializer = new XmlSerializer(typeof(List<TaxItemGroupIn362>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<TaxItemGroupIn362>)serializer.Deserialize(sr);
                            }
                        }
                    //TaxCodeIn363
                    case nameof(ContractEnum.TaxCodeIn363):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeIn363) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeIn363>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<TaxCodeIn363>)serializer.Deserialize(sr);
                            }
                        }
                    //TaxCodeTaxGroupIn364
                    case nameof(ContractEnum.TaxCodeTaxGroupIn364):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeTaxGroupIn364) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeTaxGroupIn364>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<TaxCodeTaxGroupIn364>)serializer.Deserialize(sr);
                            }
                        }
                    //TaxCodeTaxItemGroupIN365
                    case nameof(ContractEnum.TaxCodeTaxItemGroupIN365):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeTaxItemGroupIN365) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeTaxItemGroupIN365>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<TaxCodeTaxItemGroupIN365>)serializer.Deserialize(sr);
                            }
                        }
                    //TaxCodeValueIN366
                    case nameof(ContractEnum.TaxCodeValueIN366):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeValueIN366) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeValueIN366>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<TaxCodeValueIN366>)serializer.Deserialize(sr);
                            }
                        }
                    //TaxCodeLanguageTxtIN367
                    case nameof(ContractEnum.TaxCodeLanguageTxtIN367):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(TaxCodeLanguageTxtIN367) };
                            serializer = new XmlSerializer(typeof(List<TaxCodeLanguageTxtIN367>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<TaxCodeLanguageTxtIN367>)serializer.Deserialize(sr);
                            }
                        }
                    //DimensionAttributeSetupIn03
                    case nameof(ContractEnum.DimensionAttributeSetupIn03):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(DimensionAttributeSetupIn03) };
                            serializer = new XmlSerializer(typeof(List<DimensionAttributeSetupIn03>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<DimensionAttributeSetupIn03>)serializer.Deserialize(sr);
                            }
                        }
                    //DimensionAttributeValueIn03
                    case nameof(ContractEnum.DimensionAttributeValueIn03):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(DimensionAttributeValueIn03) };
                            serializer = new XmlSerializer(typeof(List<DimensionAttributeValueIn03>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<DimensionAttributeValueIn03>)serializer.Deserialize(sr);
                            }
                        }
                    //LedgerJournalNameIn43
                    case nameof(ContractEnum.LedgerJournalNameIn43):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(LedgerJournalNameIn43) };
                            serializer = new XmlSerializer(typeof(List<LedgerJournalNameIn43>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<LedgerJournalNameIn43>)serializer.Deserialize(sr);
                            }
                        }
                    //FiscalCalendarPeriodIN22
                    case nameof(ContractEnum.FiscalCalendarPeriodIN22):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(FiscalCalendarPeriodIN22) };
                            serializer = new XmlSerializer(typeof(List<FiscalCalendarPeriodIN22>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<FiscalCalendarPeriodIN22>)serializer.Deserialize(sr);
                            }
                        }
                    //AssetService
                    case nameof(ContractEnum.AssetService):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(AssetService) };
                            serializer = new XmlSerializer(typeof(List<AssetService>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<AssetService>)serializer.Deserialize(sr);
                            }
                        }
                    //AssetBook
                    case nameof(ContractEnum.AssetBook):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(AssetBook) };
                            serializer = new XmlSerializer(typeof(List<AssetBook>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<AssetBook>)serializer.Deserialize(sr);
                            }
                        }
                    //AssetDepreciation
                    case nameof(ContractEnum.AssetDepreciation):
                        using (StringWriter textWriter = new StringWriter())
                        {
                            Type[] personTypes = { typeof(AssetDepreciation) };
                            serializer = new XmlSerializer(typeof(List<AssetDepreciation>), personTypes);
                            using (TextReader sr = new StringReader(serializedListOfObjects))
                            {
                                return (List<AssetBook>)serializer.Deserialize(sr);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                log.Error(FormatMessages.getLogMessage(
                    "Serialization 2.0",
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    TokenKey.NoMethodParams,
                    ex.ToString()));
                throw ex;
            }
            return null;
        }
        #endregion
    }
}
