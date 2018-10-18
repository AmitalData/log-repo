using Logitude.AmitalMessaging.Customs.CustomFile.CellFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
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
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.U2L.CellFile
{
    public class CellFileUpsertService : UnifreightGenericService
    {
        private LOGICELLFILE _LOGICELLFILE;
        private LogitudeCellularFile _LogitudeCellularFile;
        private Def.EntityPMs.SupplierInvoicePM _MySupplierInvoicePM;
        private ICustomContext _context;

        private AmitalContext amitalContext;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.CellFile.CellFileUpsertService.Upsert()";
        private INVOICE _INVOICE;
        private DeclarationPM _MyDeclarationPM;
        private Stopwatch _Stopwatch;

        public CellFileUpsertService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGICELLFILE,
              ref string MoreParams,
              out string MessageOut
            )
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();  
            MyCommunicationsParams.Subject = "CellFileUpsertService ";
            if (this._LogitudeCellularFile != null)
            {
                
            }
            else
            {
                DeserilazeObject(xmlLOGICELLFILE);
            }
            
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
            
            CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString());_Stopwatch.Restart(); 
            MyGenericResponseObj.Stage = "GetContext";
            _context = CustomContext.GetContext(ResolvedTenant());
            var myQueryService = new DeclarationQueryService(_context);

            //Delete Supplier Invoice
            MyGenericResponseObj.Stage = "GetSingle - To delete";
            this._MyDeclarationPM = myQueryService.GetSingle(this._LogitudeCellularFile.LOGITUDEFILE, true, false);
            if (this._MyDeclarationPM == null)
            {
                throw new BusinessErrorException("LOGITUDEFILE is " + this._LogitudeCellularFile.LOGITUDEFILE + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            DeclarationUpdateService DeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
            this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            this._MyDeclarationPM.MarkAsChanged = true;
            int minute = DateTime.Now.Minute;

            var fast = true;
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
            
            AppendLogLine("MarkToDeleteSupplierInvoice:Took:" + " (IsFastDelete:" + fast.ToString() + "), " + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            _MyDeclarationPM.CurrentContextTag = UpsertActionConst; // moran 28.7.16 - Task 22249
            DeclarationUpdateService.Update(this._MyDeclarationPM, true);
            if (this._LogitudeCellularFile.INVOICE != null) 
            {
                if (this._LogitudeCellularFile.INVOICE.Count() > 0) 
                {
                    AppendLogLine("Update:MarkToDeleteSupplierInvoice:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                    this._MyDeclarationPM.SupplierInvoices = new List<SupplierInvoicePM>(); 
                    //Add Supplier Invoice
                    foreach (var itemINVOICE in this._LogitudeCellularFile.INVOICE)
                    {
                        this._INVOICE = itemINVOICE;
                        MyCommunicationsParams.Tenant = ResolvedTenant();
                        MyGenericResponseObj.Stage = "Upsert " + this._INVOICE.INVOICENUMBER;
                        InvoiceInsert();

                        MyGenericResponseObj.Stage = "Done " + this._INVOICE.INVOICENUMBER;
                    }
                    AppendLogLine("InvoiceInsert:All:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    if (this._MyDeclarationPM.SupplierInvoices != null && this._MyDeclarationPM.SupplierInvoices.Count() > 0) 
                    {
                        this._MyDeclarationPM.SupplierInvoices.FirstOrDefault().IsPrimarySupplierInvoice=true;
                    }
                    _MyDeclarationPM.CurrentContextTag = UpsertActionConst; // moran 28.7.16 - Task 22249
                    DeclarationUpdateService.Update(this._MyDeclarationPM, true);
                    AppendLogLine("Update:InvoiceInsert:All:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                    MyGenericResponseObj.Stage = "Done All ";

                }
            }
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            MyGenericResponseObj.ApplicationId = this._MyDeclarationPM.CustomFileNo;
            
        }

        private void DeserilazeObject(string xmlLOGICELLFILE)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("CellFileUpsertService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGICELLFILE))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGICELLFILE.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGICELLFILE.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGICELLFILE);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGICELLFILE = XmlGenericUtil<LOGICELLFILE>.DeSerializeObject(xmlLOGICELLFILE);

            if (_LOGICELLFILE.LogitudeCellularFile == null || _LOGICELLFILE.LogitudeCellularFile.Length != 1)
            {
                throw new BusinessErrorException("_LOGICELLFILE.LogitudeCellularFile.Length != 1");
            }
            this._LogitudeCellularFile = _LOGICELLFILE.LogitudeCellularFile[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudeCellularFile.LOGITUDEFILE))
            {
                throw new BusinessErrorException("LOGITUDEFILE is missing");
            }
            AppendLogLine("LOGITUDEFILE = " + this._LogitudeCellularFile.LOGITUDEFILE);
        }

        

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGICELLFILE();
            var myAmitalCellFile = new LogitudeCellularFile();
            var myAmitalCellFileInvoice = new List<INVOICE>();
            var myAmitalCellFileInvoiceItem = new List<INVOICEITEMS>();

            myAmitalCellFile.LOGITUDEFILE = "1-1";
            myAmitalCellFile.TENANT = "1";
            myAmitalCellFileInvoice[1].INVOICELINENO = "1";
            myAmitalCellFileInvoice[1].INVOICELINENO = "1";
            myAmitalCellFileInvoice[1].ACCOUNTTYPE = "380";
            myAmitalCellFileInvoice[1].INVOICENUMBER = "999";
            myAmitalCellFileInvoice[1].VENDORNUMBER = "2000475";
            myAmitalCellFileInvoice[1].CURRENCYCODE = "18";
            myAmitalCellFileInvoice[1].INVOICEAMOUNT = "2";
            myAmitalCellFile.INVOICE = myAmitalCellFileInvoice.ToArray();

            myAmitalCellFileInvoiceItem[1].CLASSIFICATIONCODE = "260300009";
            myAmitalCellFileInvoiceItem[1].TRADEAGREEMENTCODE = "BGR";
            myAmitalCellFileInvoiceItem[1].QUANTITY = "1";
            myAmitalCellFileInvoiceItem[1].ITEMPRICE = "1";
            myAmitalCellFileInvoiceItem[1].ITEMORIGINCOUNTRY = "AD";
            myAmitalCellFileInvoiceItem[1].ITEMCODE = "DFDF";
            myAmitalCellFile.INVOICE[1].INVOICEITEMS = myAmitalCellFileInvoiceItem.ToArray();

            amitalObjExample.LogitudeCellularFile = new LogitudeCellularFile[] { myAmitalCellFile };

            xml = XmlGenericUtil<LOGICELLFILE>.SerializeObject(amitalObjExample);

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

            var myQueryService = new CustomsVendorQueryService(this._context);

            int int1 = 0;
            decimal decimal1 = 0; 
            
            AppendLogLine("Insert Invoice..");

            MyGenericResponseObj.Stage = "GetContext";

            this._MySupplierInvoicePM = new Def.EntityPMs.SupplierInvoicePM();
            this._MySupplierInvoicePM.ChangeSetOp = ChangeSetOperation.Insert;

            MyGenericResponseObj.Stage = "Mapping";
            
            if (this._MySupplierInvoicePM.SupplierInvoiceItems == null)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems = new List<SupplierInvoiceItemPM>();
            }
            if (this._MySupplierInvoicePM.SupplierInvoiceItems.Count == 0)
            {
                this._MySupplierInvoicePM.SupplierInvoiceItems.Add(new Def.EntityPMs.SupplierInvoiceItemPM() { ChangeSetOp = ChangeSetOperation.Insert });
            }

            this._MySupplierInvoicePM.DeclarationId = this._MyDeclarationPM.Id;

            if (int.TryParse(this._INVOICE.INVOICELINENO, out int1))
            {
                this._MySupplierInvoicePM.SequenceNumeric = int1; 
            }
            else
            {
                throw new BusinessErrorException("Error in parsing INVOICELINENO (" + this._INVOICE.INVOICELINENO + ") into integer");
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
            if (this._INVOICE.INVOICEAMOUNT != null)
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
            this._MySupplierInvoicePM.IssueDate = AmitalConvertUtil.GetUnifreightFormatedDate(this._INVOICE.ISSUEDATE, "INVOICE.ISSUEDATE"); 
            this._MySupplierInvoicePM.Tenant = ResolvedTenant();
            this._MySupplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItemPM(this._INVOICE);
            this._MyDeclarationPM.SupplierInvoices.Add(this._MySupplierInvoicePM);

            return;

        }

        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItemPM(Logitude.AmitalMessaging.Customs.CustomFile.CellFile.INVOICE invoice)
        {
            var SupplierInvoiceItemPMList = new List<SupplierInvoiceItemPM>();
            foreach (var invoiceItem in invoice.INVOICEITEMS)
            {
                int int1 = 0;
                decimal decimal1 = 0;
                var SupplierInvoiceItemPM = new SupplierInvoiceItemPM();

                SupplierInvoiceItemPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;
                SupplierInvoiceItemPM.CounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                if (int.TryParse(invoiceItem.ITEMLINENO, out int1))
                {
                    SupplierInvoiceItemPM.SequenceNumeric = int1;
                    SupplierInvoiceItemPM.LineNumber = int1;
                }
                else
                {
                    throw new BusinessErrorException("Error in parsing ITEMLINENO (" + invoiceItem.ITEMLINENO + ") into integer");
                }
                
                SupplierInvoiceItemPM.ClassificationCode = invoiceItem.CLASSIFICATIONCODE;
                if (!String.IsNullOrWhiteSpace(invoiceItem.TRADEAGREEMENTCODE))
                {

                    var tradeAgreement = new TradeAgreementRepository(ResolvedTenant());
                    var myTradeAgreement = tradeAgreement.GetSingle(invoiceItem.TRADEAGREEMENTCODE);
                    if (myTradeAgreement == null)
                    {
                        string TradeAgreementCode = "";
                        TradeAgreementCode = GetTranslationL2P("IIGC", "CTBTARIFF", invoiceItem.TRADEAGREEMENTCODE);

                        if (!string.IsNullOrWhiteSpace(TradeAgreementCode)) SupplierInvoiceItemPM.TradeAgreementCode = TradeAgreementCode;
                    }
                    else
                    {
                        SupplierInvoiceItemPM.TradeAgreementCode = myTradeAgreement.Code.ToString();
                    }
                }
                if (invoiceItem.QUANTITY != null)
                {
                    if (decimal.TryParse(invoiceItem.QUANTITY, out decimal1))
                    {
                        SupplierInvoiceItemPM.InvoiceQuantity = decimal1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing QUANTITY (" + invoiceItem.QUANTITY + ") into integer");
                    }
                }
                SupplierInvoiceItemPM.InvoiceQuantityType = TranslateMeasurmentUnit(invoiceItem.UNIT);
                if (invoiceItem.ITEMPRICE != null)
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
                    string countryCode = "";
                    if (invoiceItem.ITEMORIGINCOUNTRY.Length > 2)
                    {
                        countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", invoiceItem.ITEMORIGINCOUNTRY);
                    }
                    else
                    {
                        countryCode = invoiceItem.ITEMORIGINCOUNTRY;
                    }
                    if (!string.IsNullOrWhiteSpace(countryCode)) SupplierInvoiceItemPM.OriginCountryCode = countryCode;
                }
                if (invoiceItem.CARS != null && invoiceItem.CARS.Count() > 0)
                {
                    // moran 4.7.16 - AMI-57259 -->
                    //SupplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemVehiclesPM(invoiceItem, SupplierInvoiceItemPM);
                    SupplierInvoiceItemPM.SupplierInvoiceItemsSerialNums = GetSupplierInvoiceItemsSerialNumsPM(invoiceItem, SupplierInvoiceItemPM);
                    // moran 4.7.16 - AMI-57259 <--
                }
                
                SupplierInvoiceItemPM.ItemCode = invoiceItem.ITEMCODE;
                SupplierInvoiceItemPM.Tenant = ResolvedTenant();
                SupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                SupplierInvoiceItemPMList.Add(SupplierInvoiceItemPM);
            }

            return SupplierInvoiceItemPMList;
        }

        private List<SupplierInvoiceItemsSerialNumPM> GetSupplierInvoiceItemsSerialNumsPM(INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        { // moran 4.7.16 - AMI-57259
            var SupplierInvoiceItemsSerialNumPMList = new List<SupplierInvoiceItemsSerialNumPM>();
            foreach (var invoiceItemSerialNum in invoiceItem.CARS)
            {
                int int1 = 0;
                var supplierInvoiceItemsSerialNumPM = new SupplierInvoiceItemsSerialNumPM();

                supplierInvoiceItemsSerialNumPM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;

                supplierInvoiceItemsSerialNumPM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                supplierInvoiceItemsSerialNumPM.InvoiceItemLineNumber = supplierInvoiceItemPM.LineNumber;
                supplierInvoiceItemsSerialNumPM.SerialNumber = invoiceItemSerialNum.CHASSIS;
                if (!string.IsNullOrWhiteSpace(invoiceItemSerialNum.COUNTER) && invoiceItemSerialNum.COUNTER != "0")
                {
                    if (int.TryParse(invoiceItemSerialNum.COUNTER, out int1))
                    {
                        supplierInvoiceItemsSerialNumPM.LineNumber = int1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing COUNTER (" + invoiceItemSerialNum.COUNTER + ") into integer");
                    }
                }
                if (invoiceItemSerialNum.CHASSIS != null)
                {
                    supplierInvoiceItemsSerialNumPM.TypeCode = "BN";
                }

                supplierInvoiceItemsSerialNumPM.Tenant = ResolvedTenant();
                supplierInvoiceItemsSerialNumPM.ChangeSetOp = ChangeSetOperation.Insert;

                SupplierInvoiceItemsSerialNumPMList.Add(supplierInvoiceItemsSerialNumPM);
            }

            return SupplierInvoiceItemsSerialNumPMList;
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

        private string TranslateMeasurmentUnit(string amitalMeasurmentUnitCode)
        {
            if (String.IsNullOrWhiteSpace(amitalMeasurmentUnitCode))
            {
                AppendLogLine("amitalDepartmentCode is null");
                return null;
            }
            var measurmentUnit = new MeasurmentUnitRepository(ResolvedTenant());
            var myMeasurmentUnit = measurmentUnit.GetSingle(amitalMeasurmentUnitCode);
            if (myMeasurmentUnit == null)
            {
                AppendLogLine("amitalMeasurmentUnitCode = " + amitalMeasurmentUnitCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalMeasurmentUnitCode = " + amitalMeasurmentUnitCode + " Translated to " + myMeasurmentUnit.Code);
            return myMeasurmentUnit.Code;
        }

        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemVehiclesPM(INVOICEITEMS invoiceItem, SupplierInvoiceItemPM supplierInvoiceItemPM)
        { 

            var SupplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();
            foreach (var invoiceItemCar in invoiceItem.CARS)
            {
                int int1 = 0;
                var SupplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM();

                SupplierInvoiceItemVehiclePM.DeclarationId = this._MySupplierInvoicePM.DeclarationId;

                SupplierInvoiceItemVehiclePM.InvoiceCounterKey = this._MySupplierInvoicePM.InvoiceCounterKey;
                SupplierInvoiceItemVehiclePM.InvoiceItemLineNumber = supplierInvoiceItemPM.LineNumber;
                //SupplierInvoiceItemVehiclePM.LineNumber = supplierInvoiceItemPM.LineNumber;
                SupplierInvoiceItemVehiclePM.VehicleChassisNumber = invoiceItemCar.CHASSIS;
                if (!string.IsNullOrWhiteSpace(invoiceItemCar.COUNTER) && invoiceItemCar.COUNTER != "0")
                {
                    if (int.TryParse(invoiceItemCar.COUNTER, out int1))
                    {
                        SupplierInvoiceItemVehiclePM.LineNumber = int1;
                    }
                    else
                    {
                        throw new BusinessErrorException("Error in parsing COUNTER (" + invoiceItemCar.COUNTER + ") into integer");
                    }
                }
                if (invoiceItemCar.CHASSIS != null)
                {
                    SupplierInvoiceItemVehiclePM.VehicleTypeCode = "CN";
                }
                
                SupplierInvoiceItemVehiclePM.Tenant = ResolvedTenant();
                SupplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Insert;

                SupplierInvoiceItemVehiclePMList.Add(SupplierInvoiceItemVehiclePM);
            }

            return SupplierInvoiceItemVehiclePMList;
        }

        public CustomsMessaging.Common.RequestParams.Unifreight_L2US01RequestParam RequestParams { get; set; }
    }
}

