using Logitude.AmitalMessaging.Customs.CustomFile.Sivug;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
using Logitude.CustomsMessaging;
using Logitude.BL.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.Data.Repsitories;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityPMs.UGenerated;

namespace Logitude.CustomsMessaging.U2L.Sivug
{
    public class SivugUpsertService : UnifreightGenericService
    {
        private LOGISIVUG _LOGISIVUG;
        private SIVUG _SIVUG;
        private Logitude.Customs.Def.EntityPMs.SupplierInvoicePM _MySupplierInvoicePM;
        private ICustomContext _context;
        private AmitalContext amitalContext;
        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.Sivug.SivugUpsertService.Upsert()";
        private INVOICE _INVOICE;
        private DeclarationPM _MyDeclarationPM;
        private Stopwatch _Stopwatch;
        private string mode;
        private string messageType;
        List<LineToSequenceNumeric> lineToSequence;
        private int? lastSequenceNumeric = 0;

        private bool _IsBuildItemsUnit = false;

        protected override int ResolvedTenant()
        {
            if(this._MyDeclarationPM != null && this._MyDeclarationPM.Tenant > 0)
            {
                return this._MyDeclarationPM.Tenant;
            }
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
                if (mode == "UMS2L")
                {
                    mode = "INSERT_UPDATE_DELETE";
                    messageType = "UMS2L";
                }
                else
                {
                    messageType = "US2L";
                }
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
            this._MyDeclarationPM.MarkAsChanged = true;
            int minute = DateTime.Now.Minute;
            if (this.RequestParams != null)
            {
                this.RequestParams.CFIFILEMFileNo = this._MyDeclarationPM.CustomFileNo;
                this.RequestParams.DeclarationId = this._MyDeclarationPM.Id;
            }

            if (mode != "UPDATE_ONLY" && mode != "INSERT_UPDATE_DELETE")
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
            else
            {
                if (_MyDeclarationPM.SupplierInvoices != null && _MyDeclarationPM.SupplierInvoices.Count() > 0)lastSequenceNumeric = _MyDeclarationPM.SupplierInvoices.Max(m => m.SequenceNumeric);
            }

            if (this._SIVUG.INVOICE != null) //Yuval Chalup 07.10.2015 AMI-54402 (ADD the IF only)
            {
                if (this._SIVUG.INVOICE.Count() > 0) //Yuval Chalup 07.10.2015 AMI-54402 (ADD the IF only)
                {
                    if ((mode != "UPDATE_ONLY" && mode != "INSERT_UPDATE_DELETE") || this._MyDeclarationPM.SupplierInvoices == null || this._MyDeclarationPM.SupplierInvoices.Count() < 1)
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

                    string defValue = GetDefault("ISRAEL", "CGG_BUILD_UNIT", "NON", "NON", ResolvedTenant());
                    if (defValue == "Y")
                    {
                        _IsBuildItemsUnit = true;
                    }

                    foreach (var itemINVOICE in this._SIVUG.INVOICE)
                    {
                        this._INVOICE = itemINVOICE;
                        MyCommunicationsParams.Tenant = ResolvedTenant();
                        MyGenericResponseObj.Stage = "Upsert " + this._INVOICE.INVOICENUMBER;
                        InvoiceInsert();

                        MyGenericResponseObj.Stage = "Done " + this._INVOICE.INVOICENUMBER;
                    }
                    if (mode == "INSERT_UPDATE_DELETE")
                    {
                        foreach (var supplierInvoice in this._MyDeclarationPM.SupplierInvoices)
                        {
                            if(supplierInvoice.ChangeSetOp != ChangeSetOperation.Insert && supplierInvoice.ChangeSetOp != ChangeSetOperation.Update && String.IsNullOrWhiteSpace(supplierInvoice.ChangeInSupplierInvoice))
                            {
                                supplierInvoice.ChangeSetOp = ChangeSetOperation.Delete;
                            }
                        }
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
                        DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(_MyDeclarationPM.Tenant);
                        DocumentsFilingPM documentIn = documentsFilingQuery.GetSinglePM(customsDocument.COM_ID, _MyDeclarationPM.Tenant);
                        if (documentIn != null)
                        {
                            var myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_MyDeclarationPM.Tenant);
                            List<CustomsDocumentsTicketPM> customsDocumentsTicketListPM = new List<CustomsDocumentsTicketPM>();
                            customsDocumentsTicketListPM = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketsByDocumentsFilingId(customsDocument.COM_ID, _MyDeclarationPM.Tenant);
                            CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();
                            if (customsDocumentsTicketListPM == null || customsDocumentsTicketListPM.Count() < 1)
                            {
                                customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Insert;
                                customsDocumentsTicketPM.Tenant = _MyDeclarationPM.Tenant;
                                customsDocumentsTicketPM.DocumentsFilingId = customsDocument.COM_ID;
                                customsDocumentsTicketPM.DocumentTypeCode = customsDocument.DocumentTypeCode;
                                myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);
                            }
                            else
                            {
                                customsDocumentsTicketPM = customsDocumentsTicketListPM.FirstOrDefault();
                            }

                            var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_MyDeclarationPM.Tenant);
                            List<CustomsDocumentPointerPM> customsDocumentPointerListPM = new List<CustomsDocumentPointerPM>();
                            customsDocumentPointerListPM = myCustomsDocumentPointerQueryService.GetCustomDocumentPointersForCustomDocumentId(customsDocument.COM_ID, _MyDeclarationPM.Tenant);
                            CustomsDocumentPointerPM customsDocumentPointerPM = new CustomsDocumentPointerPM();
                            if (customsDocumentPointerListPM == null || customsDocumentPointerListPM.Count() < 1)
                            {
                                customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Insert;
                                // Create Document Pointer (every document pointer has a ticket)
                                customsDocumentPointerPM.Tenant = _MyDeclarationPM.Tenant;
                                customsDocumentPointerPM.CustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                                customsDocumentPointerPM.ParentEntityCode = "Declaration";
                                customsDocumentPointerPM.ParentEntityId = _MyDeclarationPM.Id;
                                if (customsDocument.Entname == "SI")
                                {
                                    if (!string.IsNullOrWhiteSpace(customsDocument.SerialNum))
                                    {
                                        if (!string.IsNullOrWhiteSpace(customsDocument.SerialNum))
                                        {
                                            customsDocumentPointerPM.Child1EntityCode = "SupplierInvoice";
                                            customsDocumentPointerPM.Child1EntityId = customsDocument.SerialNum;
                                            if (mode == "INSERT_UPDATE_DELETE")
                                            {
                                                int int1,int2;
                                                if (int.TryParse(customsDocument.SerialNum, out int1))
                                                {
                                                    int2 = lineToSequence.Where(d => d.line == int1).FirstOrDefault().sequenceNumeric;
                                                    if(int2 > 0)customsDocumentPointerPM.Child1EntityId = int2.ToString();
                                                }
                                            }
                                        }
                                    }
                                    if (string.IsNullOrWhiteSpace(customsDocumentPointerPM.Child1EntityId))
                                    {
                                        customsDocumentPointerPM.Child1EntityCode = "SupplierInvoice";
                                        customsDocumentPointerPM.Child1EntityId = "1";
                                    }
                                }
                                if (customsDocument.Entname == "CI")
                                {
                                    customsDocumentPointerPM.Child2EntityCode = "SupplierInvoiceItem";
                                    customsDocumentPointerPM.Child2EntityId = "1"; 
                                }
                                //customsDocumentPointerPM.DocumentStatusCode = "3";
                                customsDocumentPointerPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                                myCustomsDocumentPointerUpdateService.Update(customsDocumentPointerPM, true);
                            }
                            else
                            {
                                if (mode == "INSERT_UPDATE_DELETE")
                                {
                                    if (customsDocument.Entname == "SI")
                                    {
                                        int int1, int2;
                                        if (int.TryParse(customsDocument.SerialNum, out int1))
                                        {
                                            int2 = lineToSequence.Where(d => d.line == int1).FirstOrDefault().sequenceNumeric;
                                            if (int2 > 0)
                                            {
                                                customsDocumentPointerPM = customsDocumentPointerListPM.FirstOrDefault();
                                                if (customsDocumentPointerPM.Child1EntityCode == "SupplierInvoice" && customsDocumentPointerPM.Child1EntityId != int2.ToString())
                                                {
                                                    customsDocumentPointerPM.Child1EntityId = int2.ToString();
                                                    customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Update;
                                                    myCustomsDocumentPointerUpdateService.Update(customsDocumentPointerPM, true);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                            var myDocumentId = myCustomsDocumentQueryService.GetSingle(customsDocument.COM_ID, true, false);
                            if (myDocumentId == null)
                            {
                                var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                                CustomsDocumentPM customsDocumentPM = new CustomsDocumentPM();
                                customsDocumentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                                customsDocumentPM.DocumentsFilingId = customsDocument.COM_ID;
                                customsDocumentPM.DocumentTypeCode = customsDocument.DocumentTypeCode;
                                customsDocumentPM.CurrentCustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                                customsDocumentPM.Tenant = _MyDeclarationPM.Tenant;
                                foreach (CustomsDocumentMetaDataValuePM value in customsDocumentPM.CustomsDocumentMetaDataValues)
                                {
                                    value.ChangeSetOp = ChangeSetOperation.Insert;
                                }

                                myCustomsDocumentUpdateService.Update(customsDocumentPM, true);
                            }
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
            if (mode != "UPDATE_ONLY" && mode != "INSERT_UPDATE_DELETE")
            {
                this._MySupplierInvoicePM = new Logitude.Customs.Def.EntityPMs.SupplierInvoicePM();
                this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;
            }
            else
            {
                if (_MyDeclarationPM.SupplierInvoices.Where(si => si.UnfInvoiceCounterKey == this._INVOICE.SI_COUNTER).FirstOrDefault() == null)
                {
                    this._MySupplierInvoicePM = new Logitude.Customs.Def.EntityPMs.SupplierInvoicePM();
                    this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else if (mode == "INSERT_UPDATE_DELETE" && !String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.ChangeInSupplierInvoice))
                {
                    if (int.TryParse(this._INVOICE.INVOICELINENO, out int1))
                    {
                        if (lineToSequence == null) lineToSequence = new List<LineToSequenceNumeric>();
                        var lineToSequenceNumeric = new LineToSequenceNumeric();
                        lineToSequenceNumeric.line = int1;
                        lineToSequenceNumeric.sequenceNumeric = _MyDeclarationPM.SupplierInvoices.Where(si => si.UnfInvoiceCounterKey == this._INVOICE.SI_COUNTER).FirstOrDefault().SequenceNumeric.Value;
                        lineToSequence.Add(lineToSequenceNumeric);
                    }
                    return;
                }
                else
                {
                    this._MySupplierInvoicePM = _MyDeclarationPM.SupplierInvoices.Where(si => si.UnfInvoiceCounterKey == this._INVOICE.SI_COUNTER).FirstOrDefault();
                    this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Update;
                }
            }

            MyGenericResponseObj.Stage = "Mapping";
            /*
            if (this._MySupplierInvoicePM.SupplierInvoiceItems == null)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
            }
            if (this._MySupplierInvoicePM.SupplierInvoiceItems.Count == 0)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems.Add(new Logitude.Customs.BL.EntityPMs.SupplierInvoiceItemPM() { ChangeSetOp = ChangeSetOperation.Insert });
            }
            */
            if (String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.DeclarationId)) this._MySupplierInvoicePM.DeclarationId = this._MyDeclarationPM.Id;
            if ((mode != "UPDATE_ONLY" && mode != "INSERT_UPDATE_DELETE") || lastSequenceNumeric < 1)
            {
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
            }
            else
            {
                if (!this._MySupplierInvoicePM.SequenceNumeric.HasValue)
                {
                    lastSequenceNumeric += 1;
                    this._MySupplierInvoicePM.SequenceNumeric = lastSequenceNumeric;
                }
            }
            if (mode == "INSERT_UPDATE_DELETE")
            {
                if (int.TryParse(this._INVOICE.INVOICELINENO, out int1))
                {
                    if (lineToSequence == null) lineToSequence = new List<LineToSequenceNumeric>();
                    var lineToSequenceNumeric = new LineToSequenceNumeric();
                    lineToSequenceNumeric.line = int1;
                    lineToSequenceNumeric.sequenceNumeric = this._MySupplierInvoicePM.SequenceNumeric.Value;
                    lineToSequence.Add(lineToSequenceNumeric);
                }
            }
            if (!(!String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.AccountTypeCode) && String.IsNullOrWhiteSpace(this._INVOICE.ACCOUNTTYPE)))
            {
                this._MySupplierInvoicePM.AccountTypeCode = this._INVOICE.ACCOUNTTYPE;
            }
            if (!(!String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.InvoiceNumber) && String.IsNullOrWhiteSpace(this._INVOICE.INVOICENUMBER)))
            {
                this._MySupplierInvoicePM.InvoiceNumber = this._INVOICE.INVOICENUMBER;
            }
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
            if (!(!String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.InvoiceCurrencyTypeCode) && String.IsNullOrWhiteSpace(this._INVOICE.CURRENCYCODE)))
            {
                this._MySupplierInvoicePM.InvoiceCurrencyTypeCode = this._INVOICE.CURRENCYCODE;
            }
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
            if (!(!String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.IssueDate.ToString()) && String.IsNullOrWhiteSpace(this._INVOICE.ISSUEDATE)))
            {
                this._MySupplierInvoicePM.IssueDate = AmitalConvertUtil.GetUnifreightFormatedDate(this._INVOICE.ISSUEDATE, "INVOICE.ISSUEDATE"); // moran 3.12.15 - AMI-55320
            }
            if (this._INVOICE.INCOTERM_ID != null && !String.IsNullOrWhiteSpace(this._INVOICE.INCOTERM_ID))
            {
                this._MySupplierInvoicePM.IncotermCode = TranslateTermsOfSaleType(this._INVOICE.INCOTERM_ID);
            }
                
            if (this._INVOICE.SI_COUNTER != null) this._MySupplierInvoicePM.UnfInvoiceCounterKey = this._INVOICE.SI_COUNTER;
            VendorCommissionQueryService vendorCommissionQuery = new VendorCommissionQueryService(CustomContext.GetContext(ResolvedTenant()));
            VendorCommissionPM commisionPM = vendorCommissionQuery.GetSingleCommisionByVendorAndCustomer(this._MySupplierInvoicePM.VendorId, this._MyDeclarationPM.CustomerId, ResolvedTenant());
            if (commisionPM != null) this._MySupplierInvoicePM.VendorComissionPercentage = commisionPM.CommisionPercentage;
            if (this._INVOICE.ChangeInSupplierInvoice == "2" && !String.IsNullOrWhiteSpace(this._MySupplierInvoicePM.ChangeInSupplierInvoice)) this._MySupplierInvoicePM.ChangeInSupplierInvoice = "2";
            if (this._INVOICE.INVOICEITEMS != null && this._INVOICE.INVOICEITEMS.Count() > 0)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItemPM(this._INVOICE);
            }
            this._MySupplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModificationsPM(this._INVOICE);
            if (this._MySupplierInvoicePM.ChangeSetOp != ChangeSetOperation.Update)
            {
                this._MySupplierInvoicePM.Tenant = ResolvedTenant();
                this._MyDeclarationPM.SupplierInvoices.Add(this._MySupplierInvoicePM);
            }
            /*
            else
            {
                ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
                SupplierInvoiceUpdateService supplierInvoiceUpdateService = new SupplierInvoiceUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
                supplierInvoiceUpdateService.Update(this._MySupplierInvoicePM, true);
            }*/
        }

        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItemPM(INVOICE invoice)
        {
            var SupplierInvoiceItemPMList = new List<SupplierInvoiceItemPM>();

            Dictionary<string, string> ClasificationQtyTypes = new Dictionary<string, string>() { };
            CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(ResolvedTenant());

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


                if (String.IsNullOrWhiteSpace(SupplierInvoiceItemPM.DeclarationId)) SupplierInvoiceItemPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
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

                if (invoiceItem.StatisticQuantity != null && !String.IsNullOrWhiteSpace(invoiceItem.StatisticQuantity))
                {
                    if (decimal.TryParse(invoiceItem.StatisticQuantity, out decimal1))
                    {
                        SupplierInvoiceItemPM.StatisticQuantity = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing Statistic Quantity (" + invoiceItem.StatisticQuantity + ") into integer");
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
                if (!string.IsNullOrWhiteSpace(invoiceItem.CLASIFIEDREMARKS))
                {
                    SupplierInvoiceItemPM.ClasifiedRemarks = invoiceItem.CLASIFIEDREMARKS;
                }

                if (_IsBuildItemsUnit && !string.IsNullOrEmpty(SupplierInvoiceItemPM.ClassificationCode))
                {
                    if (ClasificationQtyTypes.Keys.Contains(SupplierInvoiceItemPM.ClassificationCode))
                    {
                        SupplierInvoiceItemPM.InvoiceQuantityType = ClasificationQtyTypes[SupplierInvoiceItemPM.ClassificationCode];
                    }
                    else
                    {
                        SupplierInvoiceItemPM.InvoiceQuantityType = customsItemQueryService.GetQuantityTypeByClassificationCode(SupplierInvoiceItemPM.ClassificationCode, ResolvedTenant());
                        ClasificationQtyTypes.Add(SupplierInvoiceItemPM.ClassificationCode, SupplierInvoiceItemPM.InvoiceQuantityType);
                    }
                }
                if(string.IsNullOrWhiteSpace(SupplierInvoiceItemPM.InvoiceQuantityType) && !string.IsNullOrWhiteSpace(invoiceItem.UNIT_ID))
                {
                    SupplierInvoiceItemPM.InvoiceQuantityType = TranslateMeasurmentUnit(invoiceItem.UNIT_ID);
                }
                if (string.IsNullOrWhiteSpace(SupplierInvoiceItemPM.StatisticQuantityType) && !string.IsNullOrWhiteSpace(invoiceItem.StatisticQuantityType))
                {
                    SupplierInvoiceItemPM.StatisticQuantityType = TranslateMeasurmentUnit(invoiceItem.StatisticQuantityType);
                }
                
                if (string.IsNullOrWhiteSpace(SupplierInvoiceItemPM.TaxExemptCode) && !string.IsNullOrWhiteSpace(invoiceItem.TAXEXEMPTCODE))
                {
                    SupplierInvoiceItemPM.TaxExemptCode = invoiceItem.TAXEXEMPTCODE; //TranslateTaxExemptCode(invoiceItem.TAXEXEMPTCODE);
                }

                if (invoiceItem.CERTIFICATES != null && invoiceItem.CERTIFICATES.Count() > 0)
                {
                    try
                    {
                        int tenant = ResolvedTenant();
                        string email = AuthenticationUtil.ResolveUserIdentityName(tenant);
                        string id= RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            ContactRepository contactrep = new ContactRepository(tenant);
                            var contact = contactrep.GetSingleContact(id, tenant);
                            email = contact.Email;

                        }
                        InjectionUtil.Instance.CheckContactFeature("Customs.Declaration", "IKEA", tenant, email);
                        SupplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvoiceItemCertificatePM(invoiceItem, SupplierInvoiceItemPM);
                    }
                    catch (Exception ex)
                    {
                        AppendLogLine("Check for IKEA Feature Failed, CERTIFICATES will not be built, Message: " + ex.Message);
                    }
                }

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
                }
                    SupplierInvoiceItemPMList.Add(SupplierInvoiceItemPM);
                /*
                }
                else
                {
                    ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
                    SupplierInvoiceItemUpdateService supplierInvoiceItemUpdateService = new SupplierInvoiceItemUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
                    supplierInvoiceItemUpdateService.Update(SupplierInvoiceItemPM, true);
                }*/
            }

            return SupplierInvoiceItemPMList;
        }

        private string TranslateTaxExemptCode(string amitalTaxExemptCode)
        {
            if (String.IsNullOrWhiteSpace(amitalTaxExemptCode))
            {
                AppendLogLine("amitalTaxExemptCode is null");
                return null;
            }
            var taxExemptCode = new ValidCustomsItemQueryService(ResolvedTenant());
            var myTaxExemptCode = taxExemptCode.GetSingle(amitalTaxExemptCode,false,true);
            if (myTaxExemptCode == null)
            {
                AppendLogLine("amitalTaxExemptCode = " + amitalTaxExemptCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalTaxExemptCode = " + amitalTaxExemptCode + " Translated to " + myTaxExemptCode.Code);
            return myTaxExemptCode.Code;
        }

        public CustomsMessaging.Common.RequestParams.Unifreight_L2US01RequestParam RequestParams { get; set; }

        private List<SupplierInvioceItemCertificatPM> GetSupplierInvoiceItemCertificatePM(AmitalMessaging.Customs.CustomFile.Sivug.INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
           
            var supplierInvoiceItemCertificatePMList = new List<SupplierInvioceItemCertificatPM>();

            foreach (var cert in invoiceItem.CERTIFICATES)
            {

                SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM;

                if (supplierInvoiceItemPM.SupplierInvioceItemCertificats != null && supplierInvoiceItemPM.SupplierInvioceItemCertificats.Where(sic => sic.ExternalRequestTypeCode == cert.REQ_CERT_ID).FirstOrDefault() != null)
                {
                    supplierInvioceItemCertificatPM = supplierInvoiceItemPM.SupplierInvioceItemCertificats.Where(sic => sic.ExternalRequestTypeCode == cert.REQ_CERT_ID).FirstOrDefault();
                    if (supplierInvioceItemCertificatPM == null)
                    {
                        supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                    }
                    else
                    {
                        supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
                else
                {
                    supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                }

                supplierInvioceItemCertificatPM.DeclarationId = supplierInvoiceItemPM.DeclarationId;

                supplierInvioceItemCertificatPM.InvoiceCounterKey = supplierInvoiceItemPM.CounterKey;
                supplierInvioceItemCertificatPM.LineNumber = supplierInvoiceItemPM.LineNumber;

                supplierInvioceItemCertificatPM.ExternalRequestTypeCode = cert.REQ_CERT_ID;
                supplierInvioceItemCertificatPM.ApprovalRequestNumber = cert.REQUEST_NO;

                if (supplierInvioceItemCertificatPM.ChangeSetOp != ChangeSetOperation.Update)
                {
                    supplierInvioceItemCertificatPM.Tenant = ResolvedTenant();
                    supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Insert;

                    supplierInvoiceItemCertificatePMList.Add(supplierInvioceItemCertificatPM);
                }
                else
                {
                    ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
                    SupplierInvioceItemCertificatUpdateService supplierInvioceItemCertificatUpdateService = new SupplierInvioceItemCertificatUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
                    supplierInvioceItemCertificatUpdateService.Update(supplierInvioceItemCertificatPM, true);
                }
            }

            return supplierInvoiceItemCertificatePMList;
        }


        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModificationsPM(INVOICE iNVOICE) 
        {
            var SupplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>();
            if (this._MySupplierInvoicePM.SupplierInvoiceModifications != null && this._MySupplierInvoicePM.SupplierInvoiceModifications.Count() > 0)
            {
                SupplierInvoiceModificationPMList = this._MySupplierInvoicePM.SupplierInvoiceModifications;
            }
            
            decimal decimal1 = 0;

            {

                if (iNVOICE.EXPENSES != null)
                {

                    foreach (var expense in iNVOICE.EXPENSES)
                    {
                        if (!String.IsNullOrWhiteSpace(expense.TypeCode))
                        {
                            if (SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault() != null)
                            {
                                if (!String.IsNullOrWhiteSpace(expense.CurrencyTypeCode))
                                {
                                    if (SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().CurrencyTypeCode != expense.CurrencyTypeCode)
                                    {
                                        var expenseCurrency = new CurrencyTypeRepository(ResolvedTenant());
                                        var myexpenseCurrency = expenseCurrency.GetSingle(expense.CurrencyTypeCode);
                                        if (myexpenseCurrency == null)
                                        {
                                            string expenseCurrencyCode = "";
                                            expenseCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", this._INVOICE.CURRENCYCODE);

                                            if (!string.IsNullOrWhiteSpace(expenseCurrencyCode) && SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().CurrencyTypeCode != expenseCurrencyCode)
                                            {
                                                SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().CurrencyTypeCode = expenseCurrencyCode;
                                                SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Update;
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrWhiteSpace(myexpenseCurrency.Code) && SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().CurrencyTypeCode != myexpenseCurrency.Code)
                                            {
                                                SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().CurrencyTypeCode = myexpenseCurrency.Code;
                                                SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Update;
                                            }
                                        }
                                    }
                                }
                                if (decimal.TryParse(expense.Amount, out decimal1))
                                {
                                    if (SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().Amount != decimal1)
                                    {
                                        SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().Amount = decimal1;
                                        SupplierInvoiceModificationPMList.Where(d => d.TypeCode != expense.TypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Update;
                                    }
                                }
                                else
                                {
                                    throw new BusinessErrorException("Error in parsing expense.Amount (" + expense.Amount + ") into decimal");
                                }
                            }
                            else
                            {
                                var SupplierInvoiceModificationPM = new SupplierInvoiceModificationPM();
                                if (decimal.TryParse(expense.Amount, out decimal1))
                                {
                                    SupplierInvoiceModificationPM.Amount = decimal1;
                                }
                                else
                                {
                                    throw new BusinessErrorException("Error in parsing expense.Amount (" + expense.Amount + ") into decimal");
                                }
                                if (!String.IsNullOrWhiteSpace(expense.CurrencyTypeCode))
                                {
                                    var expenseCurrency = new CurrencyTypeRepository(ResolvedTenant());
                                    var myexpenseCurrency = expenseCurrency.GetSingle(expense.CurrencyTypeCode);
                                    if (myexpenseCurrency == null)
                                    {
                                        string expenseCurrencyCode = "";
                                        expenseCurrencyCode = GetTranslationL2P("IIGC", "CTBCURRENCY", this._INVOICE.CURRENCYCODE);

                                        if (!string.IsNullOrWhiteSpace(expenseCurrencyCode)) SupplierInvoiceModificationPM.CurrencyTypeCode = expenseCurrencyCode;
                                    }
                                    else
                                    {
                                        SupplierInvoiceModificationPM.CurrencyTypeCode = myexpenseCurrency.Code.ToString();
                                    }
                                }
                                if (!String.IsNullOrWhiteSpace(expense.TypeCode))
                                {
                                    var modificationAndDiscountType = new ModificationAndDiscountTypeRepository(ResolvedTenant());
                                    var mymodificationAndDiscountType = modificationAndDiscountType.GetSingle(expense.TypeCode);
                                    if (mymodificationAndDiscountType != null && !String.IsNullOrWhiteSpace(mymodificationAndDiscountType.Code))
                                    {
                                        SupplierInvoiceModificationPM.TypeCode = mymodificationAndDiscountType.Code;
                                    }
                                }
                                SupplierInvoiceModificationPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                                if (this._MySupplierInvoicePM.InvoiceCounterKey > 0) SupplierInvoiceModificationPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                                SupplierInvoiceModificationPM.Tenant = (this._MyDeclarationPM.Tenant > 0) ? this._MyDeclarationPM.Tenant : ResolvedTenant();
                                SupplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Insert;

                                SupplierInvoiceModificationPMList.Add(SupplierInvoiceModificationPM);
                            }
                        }
                    }
                }

                return SupplierInvoiceModificationPMList;
            }
        }


        public string GetTranslationL2P(string partnerID, string tableID, string localCode)
        {
            var rec = (from a in amitalContext.GTRTRANs
                       where a.PARTNERID == partnerID && a.TABLEID == tableID && a.LOCALCODE == localCode
                       select a).FirstOrDefault();
            if (rec == null)
            {
                return null;
            }
            return rec.PARTNERCODE;
        }

        private string TranslateTermsOfSaleType(string amitalTermsOfSaleTypeCode)
        {
            if (String.IsNullOrWhiteSpace(amitalTermsOfSaleTypeCode))
            {
                AppendLogLine("amitalTermsOfSaleTypeCode is null");
                return null;
            }
            var termsOfSaleType = new TermsOfSaleTypeRepository(ResolvedTenant());
            var myTermsOfSaleType = termsOfSaleType.GetSingle(amitalTermsOfSaleTypeCode);
            if (myTermsOfSaleType == null)
            {
                AppendLogLine("amitalTermsOfSaleTypeCode = " + amitalTermsOfSaleTypeCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalTermsOfSaleTypeCode = " + amitalTermsOfSaleTypeCode + " Translated to " + myTermsOfSaleType.Code);
            return myTermsOfSaleType.Code;
        }

        private string TranslateMeasurmentUnit(string amitalMeasurmentUnit)
        {
            if (String.IsNullOrWhiteSpace(amitalMeasurmentUnit))
            {
                AppendLogLine("amitalMeasurmentUnit is null");
                return null;
            }
            var measurmentUnit = new MeasurmentUnitRepository(ResolvedTenant());
            var myMeasurmentUnit = measurmentUnit.GetSingle(amitalMeasurmentUnit);
            if (myMeasurmentUnit == null)
            {
                AppendLogLine("amitalMeasurmentUnit = " + amitalMeasurmentUnit + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalMeasurmentUnit = " + amitalMeasurmentUnit + " Translated to " + myMeasurmentUnit.Code);
            return myMeasurmentUnit.Code;
        }

        internal class LineToSequenceNumeric
        {
            public int line { get; set; }
            public int sequenceNumeric { get; set; }
        }

        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            AmitalContext amitalContext = AmitalContext.GetContext(tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }
    }
}

