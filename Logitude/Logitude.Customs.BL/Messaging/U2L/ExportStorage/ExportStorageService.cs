using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.StimulReport.Mapping;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Contracts;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.U2L.Entry
{
    public class ExportStorageService : UnifreightGenericService
    {
        private LOGIEXPORTSTORAGE _LOGIEXPORTSTORAGE;
        private LogitudeExportStorage _LogitudeExportStorage;
        private ICustomContext _context;
        private GTRTRANQueryService _GTRTRANQueryService;
        private AmitalContext _AmitalContext;
        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.ExportStorage.ExportStorageService.Upsert()";
        private Stopwatch _Stopwatch;
        private ExportStoragePM _MyExportStoragePM;
        public Boolean suppressNewTrans;

        public ExportStorageService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGIEXPORTSTORAGE,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            try
            {

                _Stopwatch = Stopwatch.StartNew();
                MyCommunicationsParams.Subject = "ExportStorageService ";

                DeserilazeObject(xmlLOGIEXPORTSTORAGE);
                AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                CheckIntegrity();
                AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.Stage = "GetContext";
                _context = CustomContext.GetContext(ResolvedTenant());
                var myQueryService = new ExportStorageQueryService(_context);
                if (!String.IsNullOrWhiteSpace(this._LogitudeExportStorage.Id))
                {
                    MyGenericResponseObj.Stage = "GetSingle";
                    this._MyExportStoragePM = myQueryService.GetSingle(this._LogitudeExportStorage.Id, true, false);
                    if (this._MyExportStoragePM == null)
                    {
                        throw new BusinessErrorException("Id is " + this._LogitudeExportStorage.Id + " but not found");
                    }
                    AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                }
                ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
                MyCommunicationsParams.Tenant = ResolvedTenant();
                MyGenericResponseObj.Stage = "Upsert";
                Upsert(suppressNewTrans);
                MyGenericResponseObj.Stage = "Done";
                AppendLogLine("Upsert Storage Data:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.ApplicationId = _MyExportStoragePM.Id;
                //MyGenericResponseObj.ResponseXml = xml;
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
                MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;
            }
            catch (DbEntityValidationException ex)
            {
                var formatedException = ExceptionFormatUtil.GetFormated(ex);

                InsertLogLine(0, "ProccessRequest():Exception " + formatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                Debug.WriteLine("ProccessRequest():Exception " + formatedException.ToString(), true);
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                MyGenericResponseObj.Message = "Error while ExportStorageUpdateService.Update " + formatedException.Message;
                MyGenericResponseObj.ErrorDescription = formatedException.ToString();
                if (formatedException.InnerException != null)
                {
                    MyGenericResponseObj.InnerException = formatedException.InnerException.ToString();
                }

                MessageOut = MyGenericResponseObj.ErrorDescription;
            }
            catch (Exception e)
            {
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.TecinicalFailure;
                MyGenericResponseObj.Message = "Exception: " + e.Message;
                MyGenericResponseObj.ErrorDescription = e.ToString();
                if (e.InnerException != null)
                {
                    MyGenericResponseObj.InnerException = e.InnerException.ToString();
                }
                MessageOut = MyGenericResponseObj.ErrorDescription;
            }
            finally
            {
                try
                {
                    if (!String.IsNullOrWhiteSpace(MyCommunicationsParams.LoggingEntityId))
                    {
                        MyCommunicationsParams.LoggingObjectTableId = GetLoggingObjectTableId("Customs.ExportStorage");
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        protected override int ResolvedTenant() 
        {
            return int.Parse(_LOGIEXPORTSTORAGE.LogitudeExportStorage[0].Tenant);
        }

        private string GetExportStorageData(ExportStoragePM exportStoragePM)
        {
            int quantity = 0;
            var myLOGIEXPORTSTORAGE = new LOGIEXPORTSTORAGE();
            myLOGIEXPORTSTORAGE.LogitudeExportStorage = new LogitudeExportStorage[] { new LogitudeExportStorage() };
            if (declarationPM.Consignments != null && declarationPM.Consignments.Count() > 0)
            {
                myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].country = new country[] { new country() };
                myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].country[0].countryid = declarationPM.Consignments.FirstOrDefault().OriginCountryCode;
                foreach (var Consignment in declarationPM.Consignments)
                {
                    if (Consignment.ConsignmentPackages != null)
                    {
                        foreach (var Package in Consignment.ConsignmentPackages)
                        {
                            if (!string.IsNullOrWhiteSpace(declarationPM.ProcedureCurrentCode) && declarationPM.ProcedureCurrentCode.Substring(0, 1) == "7") // moran 24.3.16 - AMI-56387
                            {
                                if (Package.PackageMeasureQualifierCode == "1" && Package.PackageQuantity.HasValue) quantity += Package.PackageQuantity.Value;
                            }
                            else
                            {
                                if (Package.PackageQuantity.HasValue) quantity += Package.PackageQuantity.Value;
                            }
                        }
                    }
                }
            }
            myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].reshimon_num = declarationPM.DeclarationNumber;
            
            if (declarationPM.TaxationDateTime.HasValue)
            {
                myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].TaxationDateTime = declarationPM.TaxationDateTime.Value.Date.ToString("dd.MM.yy");
            }
            myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].quantity = quantity.ToString();
            SupplierInvoicePM primaryInvoice = new SupplierInvoicePM();

            if (declarationPM.SupplierInvoices != null && declarationPM.SupplierInvoices.Count() > 0)
            {
                primaryInvoice = declarationPM.SupplierInvoices.Where(d => d.IsPrimarySupplierInvoice).FirstOrDefault();
                if (primaryInvoice != null)
                {
                    if (primaryInvoice.TotalFreightInFreightCurrency.HasValue) myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].freight_rate = primaryInvoice.TotalFreightInFreightCurrency.Value.ToString();
                    myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].freight_currency = new freight_currency[] { new freight_currency() };
                    myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].freight_currency[0].freight_currencyid = primaryInvoice.FreightCurrencyTypeCode;
                    if (primaryInvoice.InsruancePercentage.HasValue)
                    {
                        myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].insurance_type = "1";
                        myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].insurance_percent = primaryInvoice.InsruancePercentage.Value.ToString();
                    }
                    else if (primaryInvoice.InsuranceAmount.HasValue)
                    {
                        myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].insurance_type = "2";
                        myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].insurance_amount = primaryInvoice.InsuranceAmount.Value.ToString();
                    }
                    myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].insurance_amount_currency = new insurance_amount_currency[] { new insurance_amount_currency() };
                    myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].insurance_amount_currency[0].insurance_amount_currencyid = primaryInvoice.InsruanceCurrencyTypeCode;

                    if (primaryInvoice.SupplierInvoiceModifications != null && primaryInvoice.SupplierInvoiceModifications.Count() > 0) // moran 25.5.16 - AMI-56711
                    {
                        /*
                        SupplierInvoiceModificationPM agent_fee = primaryInvoice.SupplierInvoiceModifications.Where(d => d.TypeCode == "160").FirstOrDefault();
                        if (agent_fee != null && agent_fee.Amount.HasValue)
                        {
                            myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].agent_fee = agent_fee.Amount.Value.ToString();
                            myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].agent_fee_currency = agent_fee.CurrencyTypeCode;
                        }*/
                    }

                }
                var invoices = new List<SupplierInvoice>();
                foreach (var supplierInvoice in declarationPM.SupplierInvoices)
                {
                    var mySupplierInvoice = new SupplierInvoice();
                    mySupplierInvoice.InvoiceNumber = supplierInvoice.InvoiceNumber;
                    mySupplierInvoice.AccountTypeCode = supplierInvoice.AccountTypeCode;
                    if (supplierInvoice.IssueDate.HasValue)
                    {
                        mySupplierInvoice.IssueDate = supplierInvoice.IssueDate.Value.Date.ToString("dd.MM.yy");
                    }
                    mySupplierInvoice.VendorId = supplierInvoice.VendorId;
                    CustomsVendorQueryService vendorQuery = new CustomsVendorQueryService(_context);
                    CustomsVendorPM vendor = vendorQuery.GetSingle(supplierInvoice.VendorId, true, false);
                    if (vendor != null)
                    {
                        mySupplierInvoice.VendorId = vendor.VendorNumber;
                    }
                    mySupplierInvoice.InvoiceCurrencyTypeCode = supplierInvoice.InvoiceCurrencyTypeCode;
                    if (supplierInvoice.InvoiceAmount.HasValue)
                    {
                        mySupplierInvoice.InvoiceAmount = supplierInvoice.InvoiceAmount.Value.ToString();
                    }
                    mySupplierInvoice.IncotermCode = supplierInvoice.IncotermCode;
                    mySupplierInvoice.IssueCountryCode = supplierInvoice.IssueCountryCode;
                    mySupplierInvoice.IsPreference = supplierInvoice.IsPreference.ToString();
                    mySupplierInvoice.PreferenceDocumentTypeCode = supplierInvoice.PreferenceDocumentTypeCode;
                    if (declarationPM.Consignments != null && declarationPM.Consignments.Count() > 0 && declarationPM.Consignments[0].ConsignmentPackages != null && declarationPM.Consignments[0].ConsignmentPackages.Where(r => r.PackageMeasureQualifierCode == "1").Count() > 0)
                    {
                        var packageTypeCode = declarationPM.Consignments[0].ConsignmentPackages.Where(r => r.PackageMeasureQualifierCode == "1").FirstOrDefault().PackageTypeCode;
                        if (!string.IsNullOrWhiteSpace(packageTypeCode))
                        {
                            //mySupplierInvoice.PackageTypeCode = GetTranslationP2L("IIGC", "CTBPACKTYPE", packageTypeCode);
                            mySupplierInvoice.PackageTypeCode = packageTypeCode;
                        }
                    }
                    if (supplierInvoice.SupplierInvoiceItems != null && supplierInvoice.SupplierInvoiceItems.Count() > 0)
                    {
                        var items = new List<LogitudeSupplierAccount>();
                        foreach (var item in supplierInvoice.SupplierInvoiceItems)
                        {
                            var mySupplierInvoiceItem = new LogitudeSupplierAccount();

                            mySupplierInvoiceItem.Document = item.PreferenceDocumentNumber;
                            if(item.ItemCode != null && item.ItemCode.Length < 31) mySupplierInvoiceItem.ItemNo = item.ItemCode;
                            mySupplierInvoiceItem.OriginCcountryId = item.OriginCountryCode;
                            mySupplierInvoiceItem.PratMehes = item.ClassificationCode;
                            mySupplierInvoiceItem.RateGroup = item.TradeAgreementCode;
                            mySupplierInvoiceItem.SupplierAccountLine = supplierInvoice.SequenceNumeric.ToString(); // item.CounterKey.ToString();
                            mySupplierInvoiceItem.SupplierItemLine = item.LineNumber.ToString();
                            mySupplierInvoiceItem.Unit = item.InvoiceQuantityType;
                            mySupplierInvoiceItem.StatisticQuantityUnit = item.StatisticQuantityType;
                            mySupplierInvoiceItem.MarksAndNumbers = item.MarksAndNumbers;
                            if (item.Weight.HasValue) mySupplierInvoiceItem.PackageWeight = item.Weight.ToString();
                            if (item.PackageQuantity.HasValue && item.PackageQuantity > 0)
                            {
                                mySupplierInvoiceItem.PackageQuantity = item.PackageQuantity.ToString();
                                if (item.ItemPrice.HasValue)mySupplierInvoiceItem.ItemPrice = (item.ItemPrice / item.PackageQuantity).ToString();
                                if (item.WholeSaleItemPrice.HasValue) mySupplierInvoiceItem.WholeSaleItemPrice = (item.WholeSaleItemPrice / item.PackageQuantity).ToString();
                                if (item.AdditionalQuantity.HasValue) mySupplierInvoiceItem.AdditionalQuantity = (item.AdditionalQuantity / item.PackageQuantity).ToString();
                                if (item.InvoiceQuantity.HasValue) mySupplierInvoiceItem.AccountQuantity = (item.InvoiceQuantity / item.PackageQuantity).ToString();
                                if (item.StatisticQuantity.HasValue) mySupplierInvoiceItem.StatisticQuantity = (item.StatisticQuantity / item.PackageQuantity).ToString();
                            }
                            else
                            {
                                if (item.ItemPrice.HasValue) mySupplierInvoiceItem.ItemPrice = item.ItemPrice.ToString();
                                if (item.WholeSaleItemPrice.HasValue) mySupplierInvoiceItem.WholeSaleItemPrice = item.WholeSaleItemPrice.ToString();
                                if (item.AdditionalQuantity.HasValue) mySupplierInvoiceItem.AdditionalQuantity = item.AdditionalQuantity.ToString();
                                if (item.InvoiceQuantity.HasValue) mySupplierInvoiceItem.AccountQuantity = item.InvoiceQuantity.ToString();
                                if (item.StatisticQuantity.HasValue) mySupplierInvoiceItem.StatisticQuantity = item.StatisticQuantity.ToString();
                            }
                            mySupplierInvoiceItem.WholeSaleItemPriceCurrencyCode = item.WholeSaleItemPriceCurrencyCode;
                            mySupplierInvoiceItem.AdditionalQuantityType = item.AdditionalQuantityType;
                            items.Add(mySupplierInvoiceItem);
                        }
                        mySupplierInvoice.LogitudeSupplierAccount = items.ToArray();
                    }

                    if (supplierInvoice.SupplierInvoiceModifications != null && supplierInvoice.SupplierInvoiceModifications.Count() > 0) 
                    {
                        var expenses = new List<EXPENSES>();
                        foreach (var modification in supplierInvoice.SupplierInvoiceModifications)
                        {
                            if (!string.IsNullOrWhiteSpace(modification.TypeCode))
                            {
                                var myExpenses = new EXPENSES();
                                myExpenses.TypeCode = modification.TypeCode;
                                myExpenses.CurrencyTypeCode = modification.CurrencyTypeCode;
                                myExpenses.Amount = modification.Amount.ToString();
                                expenses.Add(myExpenses);
                            }
                        }
                        mySupplierInvoice.EXPENSES = expenses.ToArray();
                    }
                    invoices.Add(mySupplierInvoice);
                }
                myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].SupplierInvoice = invoices.ToArray();
                // moran 25.5.16 - AMI-56624 -->

                var customsDocumentQueryService = new CustomsDocumentQueryService(_context);
                var customsDocumentPMList = customsDocumentQueryService.GetDeclarationDocumentList(declarationPM.Id, "Declaration", declarationPM.Tenant);

                if (customsDocumentPMList != null && customsDocumentPMList.Count() > 0)
                {
                    var customsDocuments = new List<CustomsDocuments>();
                    foreach (var customsDocumentPM in customsDocumentPMList)
                    {
                        if (!string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                        {
                            var myCustomsDocument = new CustomsDocuments();
                            var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_context);
                            List<CustomsDocumentPointerPM> customsDocumentPointerList = myCustomsDocumentPointerQueryService.GetCustomDocumentPointersForCustomDocumentId(customsDocumentPM.DocumentsFilingId, declarationPM.Tenant);
                            if (customsDocumentPointerList != null && customsDocumentPointerList.Count() > 0)
                            {
                                if (customsDocumentPointerList.Where(d => d.Child2EntityCode != null).Count() > 0)
                                {
                                    myCustomsDocument.Entname = "CI";
                                }
                                else if (customsDocumentPointerList.Where(d => d.Child1EntityCode != null).Count() > 0)
                                {
                                    myCustomsDocument.Entname = "SI";
                                }
                                else
                                {
                                    myCustomsDocument.Entname = "DE";
                                }
                            }

                            myCustomsDocument.DocumentTypeCode = customsDocumentPM.DocumentTypeCode;
                            myCustomsDocument.COM_ID = customsDocumentPM.DocumentsFilingId;
                            myCustomsDocument.FromDeclartion = "1";
                            //myCustomsDocument.CustomsDocId = customsDocumentPM.CustomsDocId;

                            customsDocuments.Add(myCustomsDocument);
                        }
                    }
                    myLOGIEXPORTSTORAGE.LogitudeExportStorage[0].CustomsDocuments = customsDocuments.ToArray();
                }

                // moran 25.5.16 - AMI-56624 <--
            }
            
            var xml = XmlGenericUtil<LOGIEXPORTSTORAGE>.SerializeObject(myLOGIEXPORTSTORAGE);
            return xml;
        }

        private string GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {

            if (partnerID == null || tableID == null || partnerCode == null)
            {
                return ("");
            }

            using (_AmitalContext = AmitalContext.GetContext(_MyExportStoragePM.Tenant))
            {
                _GTRTRANQueryService = new GTRTRANQueryService(_AmitalContext);
                var myGTRTRANPM = _GTRTRANQueryService.GetSingle(partnerID, tableID, partnerCode, null, true);

                if (myGTRTRANPM == null)
                {
                    return ("");
                }
                return (myGTRTRANPM.LOCALCODE);
            }

        }

        private void DeserilazeObject(string xmlLOGIEXPORTSTORAGE)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("ExportStorageService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGIEXPORTSTORAGE))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGIEXPORTSTORAGE.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGIEXPORTSTORAGE.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGIEXPORTSTORAGE);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGIEXPORTSTORAGE = XmlGenericUtil<LOGIEXPORTSTORAGE>.DeSerializeObject(xmlLOGIEXPORTSTORAGE);

            if (_LOGIEXPORTSTORAGE.LogitudeExportStorage == null || _LOGIEXPORTSTORAGE.LogitudeExportStorage.Length != 1)
            {
                throw new BusinessErrorException("_LOGIEXPORTSTORAGE.LogitudeExportStorage.Length != 1");
            }
            this._LogitudeExportStorage = _LOGIEXPORTSTORAGE.LogitudeExportStorage[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudeExportStorage.StorageNo))
            {
                throw new BusinessErrorException("StorageNo is missing");
            }
            AppendLogLine("StorageNo = " + this._LogitudeExportStorage.StorageNo);
        }

        public override string GetAssemblyQualifiedName()
        {
            return this.GetType().Name;
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGIEXPORTSTORAGE();
            var myAmitalExportStorage = new LogitudeExportStorage();
            myAmitalExportStorage.FirstCargoId = "11202A23";
            myAmitalExportStorage.CargoType = "1";
            myAmitalExportStorage.CargoTypeCode = "1";
            myAmitalExportStorage.ExportDealIdentification = "A123";
            myAmitalExportStorage.ExporterFileNumber = "123456";
            myAmitalExportStorage.ExporterNumber = "10011837";
            myAmitalExportStorage.SecondCargoId = "132256";
            myAmitalExportStorage.ShipCode = "10001142";
            myAmitalExportStorage.StorageNo = "555";
            myAmitalExportStorage.Tenant = "1";
            myAmitalExportStorage.ThirdCargoId = "22334";

            amitalObjExample.LogitudeExportStorage = new LogitudeExportStorage[] { myAmitalExportStorage };

            xml = XmlGenericUtil<LOGIEXPORTSTORAGE>.SerializeObject(amitalObjExample);

            return xml;
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

    }
}

