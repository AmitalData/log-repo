/* commented and moved to C:\LW\Customs\Logitude.CustomsMessaging\U2L\Sivug\SivugUpsertService.cs in order to use DocumentsFilings that is not referenced from current location
using Logitude.AmitalMessaging.Customs.CustomFile.Sivug;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Contracts;
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

namespace Logitude.Customs.BL.Messaging.U2L.Sivug
{
    public class SivugUpsertService : UnifreightGenericService
    {
        private LOGISIVUG _LOGISIVUG;
        private SIVUG _SIVUG;
        private EntityPMs.SupplierInvoicePM _MySupplierInvoicePM;
        private ICustomContext _context;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Sivug.SivugUpsertService.Upsert()";
        private INVOICE _INVOICE;
        private DeclarationPM _MyDeclarationPM;
        private Stopwatch _Stopwatch;
        private string mode;


        protected override int ResolvedTenant()
        {
            if (RequestParams == null)
            {
                return base.ResolvedTenant();
            }
            return RequestParams.Tenant; 
        }

        public SivugUpsertService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }
        public void ProccessGenericRequestObj(
                LOGISIVUG UnifreightResponse,
              ref string MoreParams,
              out string MessageOut
            )
        {
            if (UnifreightResponse != null)
            {
                this._LOGISIVUG = UnifreightResponse;

                if (_LOGISIVUG.SIVUG == null || _LOGISIVUG.SIVUG.Length != 1)
                {
                    throw new BusinessErrorException("_LOGISIVUG.SIVUG.Length != 1");
                }
                this._SIVUG = _LOGISIVUG.SIVUG[0];
            }
            ProccessGenericRequest(
              null,
              ref MoreParams,
               out MessageOut
            );
        }
        public override void ProccessGenericRequest(
              string xmlLOGISIVUG,
              ref string MoreParams,
              out string MessageOut
            )
        {
            MessageOut = "";

            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "SivugUpsertService ";
            if (this._SIVUG != null)
            {

            }
            else
            {
                DeserilazeObject(xmlLOGISIVUG);
            }
            if (!String.IsNullOrWhiteSpace(MoreParams))
            {
                AppendLogLine("MoreParams: " + MoreParams);
                var unifreightListsParams = UnifreightListsUtil.Deserialize(MoreParams);
                AppendLogLine("MoreParams after Deserialize: " + unifreightListsParams);
                mode = UnifreightListsUtil.GetValue(ref unifreightListsParams, "MODE");
                AppendLogLine("mode: " + mode);
            }

            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";
            _context = CustomContext.GetContext(ResolvedTenant());
            var myQueryService = new DeclarationQueryService(_context);

            //Delete Supplier Invoice
            MyGenericResponseObj.Stage = "GetSingle - To delete";
            this._MyDeclarationPM = myQueryService.GetSingle(this._SIVUG.LOGITUDEFILE, true, false);
            if (this._MyDeclarationPM == null)
            {
                throw new BusinessErrorException("LOGITUDEFILE is " + this._SIVUG.LOGITUDEFILE + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            DeclarationUpdateService DeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            int minute = DateTime.Now.Minute;
            if (this.RequestParams != null)
            {
                this.RequestParams.CFIFILEMFileNo = this._MyDeclarationPM.CustomFileNo;
                this.RequestParams.DeclarationId = this._MyDeclarationPM.Id;
            }

            if (mode != "UPDATE_ONLY")
            {
                var fast = true;
                //if (minute % 2 == 0)
                //{
                //    fast = true;
                //}
                if (fast)
                {
                    DeclarationUpdateService.DeclarationSupplierInvoicesFastDelete(_MyDeclarationPM, dbContext);
                    dbContext = CustomContext.GetContext(ResolvedTenant());
                    DeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());

                }
                else
                {
                    DeclarationUpdateService.MarkToDeleteSupplierInvoice(_MyDeclarationPM);
                }
                // LogMessagingUtil.Instance.AppendLine("IsFastDelete:" + fast.ToString() + ",Took :" + _Stopwatch.ElapsedMilliseconds);

                AppendLogLine("MarkToDeleteSupplierInvoice:Took:" + " (IsFastDelete:" + fast.ToString() + "), " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                _MyDeclarationPM.CurrentContextTag = UpsertActionConst; // moran 28.7.16 - Task 22249
                DeclarationUpdateService.Update(this._MyDeclarationPM, true);
                AppendLogLine("Update:MarkToDeleteSupplierInvoice:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            }
            if (this._SIVUG.INVOICE != null) //Yuval Chalup 07.10.2015 AMI-54402 (ADD the IF only)
            {
                if (this._SIVUG.INVOICE.Count() > 0) //Yuval Chalup 07.10.2015 AMI-54402 (ADD the IF only)
                {
                    if (mode != "UPDATE_ONLY" || this._MyDeclarationPM.SupplierInvoices == null || this._MyDeclarationPM.SupplierInvoices.Count() < 1)
                    {
                        this._MyDeclarationPM.SupplierInvoices = new List<SupplierInvoicePM>(); // moran 18.3.15 - Task 11540
                    }
                    //Add Supplier Invoice
                    /* // moran 18.3.15 - Task 11540 - commented -->
                    MyGenericResponseObj.Stage = "GetSingle - To add";
                    this._MyDeclarationPM = myQueryService.GetSingle(this._SIVUG.LOGITUDEFILE, true, false);
                    if (this._MyDeclarationPM == null)
                    {
                        throw new BusinessErrorException("LOGITUDEFILE is " + this._SIVUG.LOGITUDEFILE + " but not found");
                    }
                    AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
                    this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                    // moran 18.3.15 - Task 11540 - commented <-- */
                    /*
                    foreach (var itemINVOICE in this._SIVUG.INVOICE)
                    {
                        this._INVOICE = itemINVOICE;
                        MyCommunicationsParams.Tenant = ResolvedTenant();
                        MyGenericResponseObj.Stage = "Upsert " + this._INVOICE.INVOICENUMBER;
                        InvoiceInsert();

                        MyGenericResponseObj.Stage = "Done " + this._INVOICE.INVOICENUMBER;
                    }
                    AppendLogLine("InvoiceInsert:All:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    if (this._MyDeclarationPM.SupplierInvoices != null && this._MyDeclarationPM.SupplierInvoices.Count() > 0) // moran 8.10.15 - Task 16452
                    {
                        this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().IsPrimarySupplierInvoice = true;
                        //this._MyDeclarationPM.PrimaryInvoiceCounterKey = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().InvoiceCounterKey.ToString();
                    }
                    _MyDeclarationPM.CurrentContextTag = UpsertActionConst; // moran 28.7.16 - Task 22249
                    DeclarationUpdateService.Update(this._MyDeclarationPM, true);
                    AppendLogLine("Update:InvoiceInsert:All:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    MyGenericResponseObj.Stage = "Done All ";

                }
            }


            if (this._SIVUG.CustomsDocuments != null && this._SIVUG.CustomsDocuments.Where(d => d.Blocked != "1").Count() > 0) // moran 2.6.16 - AMI-56624
            {
                var myCustomsDocumentQueryService = new CustomsDocumentQueryService(dbContext);
                //var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(dbContext);
                var myCustomsDocumentPointerUpdateService = new CustomsDocumentPointerUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                //var myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(dbContext);
                var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);

                //CustomsDocumentPointerPM customsDocumentPointerPM = new CustomsDocumentPointerPM();
                //CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();


                foreach (var customsDocument in this._SIVUG.CustomsDocuments)
                {
                    if (customsDocument.Blocked != "1")
                    {
                        

                        CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();
                        customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Insert;
                        customsDocumentsTicketPM.Tenant = _MyDeclarationPM.Tenant;
                        customsDocumentsTicketPM.DocumentsFilingId = customsDocument.COM_ID;
                        customsDocumentsTicketPM.DocumentTypeCode = customsDocument.DocumentTypeCode;
                        myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);

                        CustomsDocumentPointerPM customsDocumentPointerPM = new CustomsDocumentPointerPM();
                        customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Insert;

                        // Create Document Pointer (every document pointer has a ticket)
                        customsDocumentPointerPM.Tenant = _MyDeclarationPM.Tenant;
                        customsDocumentPointerPM.CustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                        customsDocumentPointerPM.ParentEntityCode = "Declaration";
                        customsDocumentPointerPM.ParentEntityId = _MyDeclarationPM.Id;
                        if (customsDocument.Entname == "SI")
                        {
                            customsDocumentPointerPM.Child1EntityCode = "SupplierInvoice";
                            customsDocumentPointerPM.Child1EntityId = "1"; // customsDocument.Key_2;
                        }
                        if (customsDocument.Entname == "CI")
                        {

                            customsDocumentPointerPM.Child2EntityCode = "SupplierInvoiceItem";
                            customsDocumentPointerPM.Child2EntityId = "1"; //customsDocument.Key_3;
                        }
                        //customsDocumentPointerPM.DocumentStatusCode = "3";
                        customsDocumentPointerPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                        myCustomsDocumentPointerUpdateService.Update(customsDocumentPointerPM, true);

                        var myDocumentId = myCustomsDocumentQueryService.GetSingle(customsDocument.COM_ID, true, false);
                        if (myDocumentId == null)
                        {
                            var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                            CustomsDocumentPM customsDocumentPM = new CustomsDocumentPM();
                            customsDocumentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                            customsDocumentPM.DocumentsFilingId = customsDocument.COM_ID;
                            customsDocumentPM.DocumentTypeCode = customsDocument.DocumentTypeCode;
                            customsDocumentPM.CurrentCustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                            foreach (CustomsDocumentMetaDataValuePM value in customsDocumentPM.CustomsDocumentMetaDataValues)
                            {
                                value.ChangeSetOp = ChangeSetOperation.Insert;
                            }

                            myCustomsDocumentUpdateService.Update(customsDocumentPM, true);
                        }

                    }
                }
            }


            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            MyGenericResponseObj.ApplicationId = this._MyDeclarationPM.CustomFileNo;

        }

        private void DeserilazeObject(string xmlLOGISIVUG)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("SivugUpsertService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGISIVUG))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGISIVUG.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGISIVUG.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGISIVUG);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGISIVUG = XmlGenericUtil<LOGISIVUG>.DeSerializeObject(xmlLOGISIVUG);

            if (_LOGISIVUG.SIVUG == null || _LOGISIVUG.SIVUG.Length != 1)
            {
                throw new BusinessErrorException("_LOGISIVUG.SIVUG.Length != 1");
            }
            this._SIVUG = _LOGISIVUG.SIVUG[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._SIVUG.LOGITUDEFILE))
            {
                throw new BusinessErrorException("LOGITUDEFILE is missing");
            }
            AppendLogLine("LOGITUDEFILE = " + this._SIVUG.LOGITUDEFILE);
        }

        

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGISIVUG();
            var myAmitalSivug = new SIVUG();
            var myAmitalSivugInvoiceList = new List<INVOICE>();
            var myAmitalSivugInvoiceItemList = new List<INVOICEITEMS>();
            var myAmitalSivugInvoice = new INVOICE();
            var myAmitalSivugInvoiceItem = new INVOICEITEMS();
            myAmitalSivug.LOGITUDEFILE = "1-104368";
            myAmitalSivug.TENANT = "1";
            myAmitalSivugInvoice.INVOICELINENO = "1";
            //myAmitalSivugInvoice.INVOICELINENO = "1";
            myAmitalSivugInvoice.ACCOUNTTYPE = "380";
            myAmitalSivugInvoice.INVOICENUMBER = "M6I0323";
            myAmitalSivugInvoice.VENDORNUMBER = "2015002";
            myAmitalSivugInvoice.CURRENCYCODE = "USD";
            myAmitalSivugInvoice.INVOICEAMOUNT = "77919.56";
            myAmitalSivugInvoice.ISSUEDATE = "04.10.16";
            myAmitalSivugInvoice.ISSUECOUNTRYCODE = "US";
            myAmitalSivugInvoice.SI_COUNTER = "4494";
            myAmitalSivugInvoiceList.Add(myAmitalSivugInvoice);
            myAmitalSivug.INVOICE = myAmitalSivugInvoiceList.ToArray();

            myAmitalSivugInvoiceItem.CLASSIFICATIONCODE = "90229000006";
            myAmitalSivugInvoiceItem.TRADEAGREEMENTCODE = "2";
            myAmitalSivugInvoiceItem.ITEMLINENO = "1";
            myAmitalSivugInvoiceItem.QUANTITY = "2";
            myAmitalSivugInvoiceItem.ITEMPRICE = "79.08";
            myAmitalSivugInvoiceItem.ITEMORIGINCOUNTRY = "KR";
            myAmitalSivugInvoiceItem.ITEMCODE = "0K08A13451";
            myAmitalSivugInvoiceItem.ACTUALINVOICELINES = "0K08A13451C";
            myAmitalSivugInvoiceItem.LINE_ID = "4";
            myAmitalSivugInvoiceItemList.Add(myAmitalSivugInvoiceItem);
            myAmitalSivug.INVOICE[0].INVOICEITEMS = myAmitalSivugInvoiceItemList.ToArray();

            amitalObjExample.SIVUG = new SIVUG[] { myAmitalSivug };

            xml = XmlGenericUtil<LOGISIVUG>.SerializeObject(amitalObjExample);

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

        private void InvoiceInsert()
        {
            //CheckExist();

            var myQueryService = new CustomsVendorQueryService(this._context);

            int int1 = 0;
            decimal decimal1 = 0; 
            
            AppendLogLine("Insert Invoice..");

            MyGenericResponseObj.Stage = "GetContext";
            if (mode != "UPDATE_ONLY")
            {
                this._MySupplierInvoicePM = new EntityPMs.SupplierInvoicePM();
                this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;
            }
            else
            {
                if(_MyDeclarationPM.SupplierInvoices.Where(si => si.UnfInvoiceCounterKey == this._INVOICE.SI_COUNTER).FirstOrDefault() == null)
                {
                    this._MySupplierInvoicePM = new EntityPMs.SupplierInvoicePM();
                    this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    this._MySupplierInvoicePM = _MyDeclarationPM.SupplierInvoices.Where(si => si.UnfInvoiceCounterKey == this._INVOICE.SI_COUNTER).FirstOrDefault();
                    this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Update;
                }
            }

            MyGenericResponseObj.Stage = "Mapping";
            
            if (this._MySupplierInvoicePM.SupplierInvoiceItems == null)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
            }
            if (this._MySupplierInvoicePM.SupplierInvoiceItems.Count == 0)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems.Add(new EntityPMs.SupplierInvoiceItemPM() { ChangeSetOp = ChangeSetOperation.Insert });
            }

            if(String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.DeclarationId))this._MySupplierInvoicePM.DeclarationId = this._MyDeclarationPM.Id;

            if (!this._MySupplierInvoicePM.SequenceNumeric.HasValue)
            {
                if (int.TryParse(this._INVOICE.INVOICELINENO, out int1))
                {
                    this._MySupplierInvoicePM.SequenceNumeric = int1;
                    //this._MySupplierInvoicePM.InvoiceCounterKey = int1; // moran 8.10.15 - Task 16452 - commented - initiated automatically on creating
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing INVOICELINENO (" + this._INVOICE.INVOICELINENO + ") into integer");
                }
            }
            this._MySupplierInvoicePM.AccountTypeCode = this._INVOICE.ACCOUNTTYPE;
            this._MySupplierInvoicePM.InvoiceNumber = this._INVOICE.INVOICENUMBER;
            if (!string.IsNullOrWhiteSpace(this._INVOICE.VENDORNUMBER))
            {
                this._MySupplierInvoicePM.VendorId = myQueryService.GetIdByVendorNumber(this._INVOICE.VENDORNUMBER, ResolvedTenant());
                if (!string.IsNullOrWhiteSpace(this._MySupplierInvoicePM.VendorId))
                {
                    CustomsVendorPM customsVendorPM = myQueryService.GetSingle(this._MySupplierInvoicePM.VendorId, true, false);
                    if (customsVendorPM == null)
                    {
                        throw new BusinessErrorException("customsVendorPM '" + this._MySupplierInvoicePM.VendorId + "' is not found");
                    }
                    this._MySupplierInvoicePM.IssueCountryCode = customsVendorPM.CountryCode;
                }
            }
            this._MySupplierInvoicePM.InvoiceCurrencyTypeCode = this._INVOICE.CURRENCYCODE;
            if (this._INVOICE.INVOICEAMOUNT != null && !String.IsNullOrWhiteSpace(this._INVOICE.INVOICEAMOUNT))
            {
                if (decimal.TryParse(this._INVOICE.INVOICEAMOUNT, out decimal1))
                {
                    this._MySupplierInvoicePM.InvoiceAmount = decimal1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing INVOICEAMOUNT (" + this._INVOICE.INVOICEAMOUNT + ") into integer");
                }
            }
            this._MySupplierInvoicePM.IssueDate = AmitalConvertUtil.GetUnifreightFormatedDate(this._INVOICE.ISSUEDATE, "INVOICE.ISSUEDATE"); // moran 3.12.15 - AMI-55320
            if(this._INVOICE.SI_COUNTER != null)this._MySupplierInvoicePM.UnfInvoiceCounterKey = this._INVOICE.SI_COUNTER;
            
            this._MySupplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItemPM(this._INVOICE);
            if (this._MySupplierInvoicePM.ChangeSetOp != ChangeSetOperation.Update)
            {
                this._MySupplierInvoicePM.Tenant = ResolvedTenant();
                this._MyDeclarationPM.SupplierInvoices.Add(this._MySupplierInvoicePM);
            }
            else
            {
                ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
                SupplierInvoiceUpdateService supplierInvoiceUpdateService = new SupplierInvoiceUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
                supplierInvoiceUpdateService.Update(this._MySupplierInvoicePM, true);
            }
        }

        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItemPM(INVOICE invoice)
        {
            var SupplierInvoiceItemPMList = new List<SupplierInvoiceItemPM>();
            foreach (var invoiceItem in invoice.INVOICEITEMS)
            {
                int int1 = 0;
                decimal decimal1 = 0;
                SupplierInvoiceItemPM SupplierInvoiceItemPM;

                if (mode != "UPDATE_ONLY")
                {
                    SupplierInvoiceItemPM = new SupplierInvoiceItemPM();
                }
                else
                {
                    if (int.TryParse(invoiceItem.LINE_ID, out int1))
                    {

                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing LINE_ID (" + invoiceItem.LINE_ID + ") into integer");
                    }
                    if (_MyDeclarationPM.SupplierInvoices.Where(si => si.UnfInvoiceCounterKey == this._INVOICE.SI_COUNTER).FirstOrDefault() != null &&
                        _MyDeclarationPM.SupplierInvoices.Where(si => si.UnfInvoiceCounterKey == this._INVOICE.SI_COUNTER).FirstOrDefault().SupplierInvoiceItems != null)
                    {
                        SupplierInvoiceItemPM = _MyDeclarationPM.SupplierInvoices
                            .Where(si => si.UnfInvoiceCounterKey == this._INVOICE.SI_COUNTER)
                            .First()
                            .SupplierInvoiceItems.Where(sii => sii.UnfInvoiceLine == int1)
                            .FirstOrDefault();
                        if (SupplierInvoiceItemPM == null)
                        {
                            SupplierInvoiceItemPM = new SupplierInvoiceItemPM();
                        }
                        else
                        {
                            SupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                    else
                    {
                        SupplierInvoiceItemPM = new SupplierInvoiceItemPM();
                    }
                }

                
                if(String.IsNullOrWhiteSpace(SupplierInvoiceItemPM.DeclarationId)) SupplierInvoiceItemPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                if (SupplierInvoiceItemPM.CounterKey != 0 && SupplierInvoiceItemPM.CounterKey < 1) SupplierInvoiceItemPM.CounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                if (!SupplierInvoiceItemPM.SequenceNumeric.HasValue)
                {
                    if (int.TryParse(invoiceItem.ITEMLINENO, out int1))
                    {
                        SupplierInvoiceItemPM.SequenceNumeric = int1;
                        SupplierInvoiceItemPM.LineNumber = int1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing ITEMLINENO (" + invoiceItem.ITEMLINENO + ") into integer");
                    }
                }
                SupplierInvoiceItemPM.ClassificationCode = invoiceItem.CLASSIFICATIONCODE;
                // moran 18.3.15 - Task 11540 -->
                //SupplierInvoiceItemPM.TradeAgreementCode = invoiceItem.TRADEAGREEMENTCODE;
                if (!String.IsNullOrWhiteSpace(invoiceItem.TRADEAGREEMENTCODE))
                {
                    SupplierInvoiceItemPM.TradeAgreementCode = invoiceItem.TRADEAGREEMENTCODE;
                }
                // moran 18.3.15 - Task 11540 <--
                if (invoiceItem.QUANTITY != null && !String.IsNullOrWhiteSpace(invoiceItem.QUANTITY))
                {
                    if (decimal.TryParse(invoiceItem.QUANTITY, out decimal1))
                    {
                        SupplierInvoiceItemPM.InvoiceQuantity = decimal1;
                        //SupplierInvoiceItemPM.StatisticQuantity = decimal1; // moran 18.4.16 - AMI-56381 - commented
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing QUANTITY (" + invoiceItem.QUANTITY + ") into integer");
                    }
                }

                if (invoiceItem.ITEMPRICE != null && !String.IsNullOrWhiteSpace(invoiceItem.ITEMPRICE))
                {
                    if (decimal.TryParse(invoiceItem.ITEMPRICE, out decimal1))
                    {
                        SupplierInvoiceItemPM.ItemPrice = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing ITEMPRICE (" + invoiceItem.ITEMPRICE + ") into decimal");
                    }
                }
                if (!string.IsNullOrWhiteSpace(invoiceItem.ITEMORIGINCOUNTRY))
                {
                    SupplierInvoiceItemPM.OriginCountryCode = invoiceItem.ITEMORIGINCOUNTRY;
                }

                SupplierInvoiceItemPM.ItemCode = invoiceItem.ITEMCODE;
             //   SupplierInvoiceItemPM.UnfInvoiceCounterKey = invoiceItem.ITEM_SI_COUNTER;
                
                if (int.TryParse(invoiceItem.LINE_ID, out int1))
                {
                    SupplierInvoiceItemPM.UnfInvoiceLine = int1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing LINE_ID (" + invoiceItem.LINE_ID + ") into integer");
                }
                if (SupplierInvoiceItemPM.ChangeSetOp != ChangeSetOperation.Update)
                {
                    SupplierInvoiceItemPM.Tenant = ResolvedTenant();
                    SupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                    SupplierInvoiceItemPMList.Add(SupplierInvoiceItemPM);
                }
                else
                {
                    ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
                    SupplierInvoiceItemUpdateService supplierInvoiceItemUpdateService = new SupplierInvoiceItemUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
                    supplierInvoiceItemUpdateService.Update(SupplierInvoiceItemPM, true);
                }
            }

            return SupplierInvoiceItemPMList;
        }




        public CustomsMessaging.Common.RequestParams.Unifreight_L2US01RequestParam RequestParams { get; set; }
    }
}

*/