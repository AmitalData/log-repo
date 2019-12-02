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

namespace Logitude.Customs.BL.Messaging.U2L.Entry
{
    public class EntryService : UnifreightGenericService
    {
        private LOGIENTRY _LOGIENTRY;
        private LogitudeEntry _LogitudeEntry;
        private Def.EntityPMs.SupplierInvoicePM _MySupplierInvoicePM;
        private ICustomContext _context;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Entry.EntryService.Upsert()";
        private DeclarationPM _MyDeclarationPM;
        private Stopwatch _Stopwatch;

        public EntryService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGIENTRY,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "EntryService ";

            DeserilazeObject(xmlLOGIENTRY);
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";
            _context = CustomContext.GetContext(ResolvedTenant());
            var myQueryService = new DeclarationQueryService(_context);

            MyGenericResponseObj.Stage = "GetSingle";
            this._MyDeclarationPM = myQueryService.GetSingle(this._LogitudeEntry.logitude_file, true, false);
            if (this._MyDeclarationPM == null)
            {
                throw new BusinessErrorException("Id is " + this._LogitudeEntry.logitude_file + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            MyGenericResponseObj.Stage = "Get Entry Data for file " + this._MyDeclarationPM.CustomFileNo;
            DeclarationSRMapping declarationSRMapping = new DeclarationSRMapping();
            var xml = GetEntryData(this._MyDeclarationPM);
            if (String.IsNullOrWhiteSpace(xml))
            {
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                MyGenericResponseObj.Message = "Get Entry Data returned null";
                return;
            }
            MyGenericResponseObj.Stage = "Get Entry Data Done ";
            AppendLogLine("Get Entry Data:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
            MyGenericResponseObj.ResponseXml = xml;
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;

        }

        private string GetEntryData(DeclarationPM declarationPM)
        {
            int quantity = 0;
            var myLOGIENTRY = new LOGIENTRY();
            myLOGIENTRY.LogitudeEntry = new LogitudeEntry[] { new LogitudeEntry() };
            if (declarationPM.Consignments != null && declarationPM.Consignments.Count() > 0)
            {
                myLOGIENTRY.LogitudeEntry[0].country = new country[] { new country() };
                myLOGIENTRY.LogitudeEntry[0].country[0].countryid = declarationPM.Consignments.FirstOrDefault().OriginCountryCode;
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
            myLOGIENTRY.LogitudeEntry[0].reshimon_num = declarationPM.DeclarationNumber;
            myLOGIENTRY.LogitudeEntry[0].quantity = quantity.ToString();
            SupplierInvoicePM primaryInvoice = new SupplierInvoicePM();

            if (declarationPM.SupplierInvoices != null && declarationPM.SupplierInvoices.Count() > 0)
            {
                primaryInvoice = declarationPM.SupplierInvoices.Where(d => d.IsPrimarySupplierInvoice).FirstOrDefault();
                if (primaryInvoice != null)
                {
                    if (primaryInvoice.TotalFreightInFreightCurrency.HasValue) myLOGIENTRY.LogitudeEntry[0].freight_rate = primaryInvoice.TotalFreightInFreightCurrency.Value.ToString();
                    myLOGIENTRY.LogitudeEntry[0].freight_currency = new freight_currency[] { new freight_currency() };
                    myLOGIENTRY.LogitudeEntry[0].freight_currency[0].freight_currencyid = primaryInvoice.FreightCurrencyTypeCode;
                    if (primaryInvoice.InsruancePercentage.HasValue)
                    {
                        myLOGIENTRY.LogitudeEntry[0].insurance_type = "1";
                        myLOGIENTRY.LogitudeEntry[0].insurance_percent = primaryInvoice.InsruancePercentage.Value.ToString();
                    }
                    else if (primaryInvoice.InsuranceAmount.HasValue)
                    {
                        myLOGIENTRY.LogitudeEntry[0].insurance_type = "2";
                        myLOGIENTRY.LogitudeEntry[0].insurance_amount = primaryInvoice.InsuranceAmount.Value.ToString();
                    }
                    myLOGIENTRY.LogitudeEntry[0].insurance_amount_currency = new insurance_amount_currency[] { new insurance_amount_currency() };
                    myLOGIENTRY.LogitudeEntry[0].insurance_amount_currency[0].insurance_amount_currencyid = primaryInvoice.InsruanceCurrencyTypeCode;

                    if (primaryInvoice.SupplierInvoiceModifications != null && primaryInvoice.SupplierInvoiceModifications.Count() > 0) // moran 25.5.16 - AMI-56711
                    {
                        /*
                        SupplierInvoiceModificationPM agent_fee = primaryInvoice.SupplierInvoiceModifications.Where(d => d.TypeCode == "160").FirstOrDefault();
                        if (agent_fee != null && agent_fee.Amount.HasValue)
                        {
                            myLOGIENTRY.LogitudeEntry[0].agent_fee = agent_fee.Amount.Value.ToString();
                            myLOGIENTRY.LogitudeEntry[0].agent_fee_currency = agent_fee.CurrencyTypeCode;
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
                    if (supplierInvoice.SupplierInvoiceItems != null && supplierInvoice.SupplierInvoiceItems.Count() > 0)
                    {
                        var items = new List<LogitudeSupplierAccount>();
                        foreach (var item in supplierInvoice.SupplierInvoiceItems)
                        {
                            var mySupplierInvoiceItem = new LogitudeSupplierAccount();

                            mySupplierInvoiceItem.Document = item.PreferenceDocumentNumber;
                            if(item.ItemCode != null && item.ItemCode.Length < 11) mySupplierInvoiceItem.ItemNo = item.ItemCode;
                            mySupplierInvoiceItem.OriginCcountryId = item.OriginCountryCode;
                            mySupplierInvoiceItem.PratMehes = item.ClassificationCode;
                            if (item.InvoiceQuantity.HasValue) mySupplierInvoiceItem.AccountQuantity = item.InvoiceQuantity.ToString();
                            if (item.StatisticQuantity.HasValue) mySupplierInvoiceItem.StatisticQuantity = item.StatisticQuantity.ToString();
                            mySupplierInvoiceItem.RateGroup = item.TradeAgreementCode;
                            mySupplierInvoiceItem.SupplierAccountLine = item.CounterKey.ToString();
                            mySupplierInvoiceItem.SupplierItemLine = item.LineNumber.ToString();
                            mySupplierInvoiceItem.Unit = item.InvoiceQuantityType;
                            mySupplierInvoiceItem.StatisticQuantityUnit = item.StatisticQuantityType;

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
                myLOGIENTRY.LogitudeEntry[0].SupplierInvoice = invoices.ToArray();
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
                    myLOGIENTRY.LogitudeEntry[0].CustomsDocuments = customsDocuments.ToArray();
                }

                // moran 25.5.16 - AMI-56624 <--
            }
            var xml = XmlGenericUtil<LOGIENTRY>.SerializeObject(myLOGIENTRY);
            return xml;
        }

        private void DeserilazeObject(string xmlLOGIENTRY)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("EntryService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGIENTRY))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGIENTRY.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGIENTRY.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGIENTRY);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGIENTRY = XmlGenericUtil<LOGIENTRY>.DeSerializeObject(xmlLOGIENTRY);

            if (_LOGIENTRY.LogitudeEntry == null || _LOGIENTRY.LogitudeEntry.Length != 1)
            {
                throw new BusinessErrorException("_LOGIENTRY.Entry.Length != 1");
            }
            this._LogitudeEntry = _LOGIENTRY.LogitudeEntry[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudeEntry.logitude_file))
            {
                throw new BusinessErrorException("Id is missing");
            }
            AppendLogLine("Id = " + this._LogitudeEntry.logitude_file);
        }

        public override string GetAssemblyQualifiedName()
        {
            var xml = "";
            var amitalObjExample = new LOGIENTRY();
            var myAmitalEntry = new LogitudeEntry();


            myAmitalEntry.logitude_file = "1-1";
            myAmitalEntry.tenant = "1";


            amitalObjExample.LogitudeEntry = new LogitudeEntry[] { myAmitalEntry };

            xml = XmlGenericUtil<LOGIENTRY>.SerializeObject(amitalObjExample);

            return xml;
        }

        public override string GetExampleDataIn1()
        {
            throw new NotImplementedException();
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

