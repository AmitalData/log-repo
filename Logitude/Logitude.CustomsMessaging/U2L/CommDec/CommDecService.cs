using Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.CustomsMessaging;
using Logitude.CustomsMessaging.MessagingServices;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Customs.Data.EntityKeys;


namespace Logitude.CustomsMessaging.U2L.CommDec
{
    public class CommDecService : UnifreightGenericService
    {
        private LOGICOMMDEC _LOGICOMMDEC;
        private LogitudeCommDecFile _LogitudeCommDecFile;
        private SupplierInvoicePM _MySupplierInvoicePM;
        private CourierMasterPM _CourierMasterPM;
        private CourierDeclarationPM _CourierDeclarationPM;
        private ICustomContext _context;
        private LOGICUSTFILE _LOGICUSTFILE;
        private LogitudeCustomsFile _AmitalCustomsFile;
        private DeclarationCourierStatusPM currentDeclarationCourierStatusPM;

        private AmitalContext amitalContext;

        public string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.CommDec.CommDecService.Upsert()";
        private Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICE _INVOICE;
        private DeclarationPM _MyDeclarationPM;



        //private DeclarationPM _MyEntryDeclarationPM;
        private Stopwatch _Stopwatch;
        private bool _IsBuildItemsUnit = false;
        private bool _IsNewDeclaration = false;
        public bool IsAutonomy = false;
        private decimal _SupplierInvoiceAmount;
        private string mode;
        private bool IsProcedureCurrentCodeChanged = false; 

        public CommDecService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        int _tenant = 0;

        public override void ProccessGenericRequest(
              string xmlLOGICOMMDEC,
              ref string MoreParams,
              out string MessageOut)

        {
            string customFileNo = "";
            string decId = "";
            string courierMasterID = "";
            MessageOut = "";
            _tenant = ResolvedTenant();
            var user = AuthenticationUtil.ResolveUserId(_tenant);
            string defValue = GetDefault("ISRAEL", "CGG_OPN_DEC_MET", "NON", "NON", _tenant);

         //   if (!string.IsNullOrEmpty(defValue) && defValue == "B")
          //  {
                AppendLogLine("!string.IsNullOrEmpty(defValue) && defValue=='B'");

                var messagingService = new DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceMessagingService();
                DCAInUCUW2LRequestParams requestParams = new DCAInUCUW2LRequestParams()
                {
                    LOGICOMMDEC = xmlLOGICOMMDEC,
                    MoreParams = MoreParams,
                    LoggingUserId = user,
                    Tenant = _tenant
                };

                string message = messagingService.CreateCRS(_tenant, user, requestParams);
                AppendLogLine("message : " + message);

                if (message == "SUCCESS")
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
                else
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.TecinicalFailure;


           // }
           // else
          //  {
          //      AppendLogLine("Default= WS'");

           //     ProccessGenericRequestReal(xmlLOGICOMMDEC, _tenant, user, ref MoreParams, out MessageOut, out customFileNo, out decId, out courierMasterID);
          //  }


        }
        //protected override int ResolvedTenant()
        //{
        //    return _tenant;
        //}
        public string _PBId;

     


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

     


       

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            return
@"<?xml version=""1.0"" encoding=""windows-1255""?>
<LOGICOMMDEC>
 <LogitudeCommDecFile>
  <LoadingPortCode>USBOS</LoadingPortCode>
  <MAWB>20261079</MAWB>
  <HAWB>09878498</HAWB>
  <CarrierPrefix>114</CarrierPrefix>
  <IsAutonomy>No</IsAutonomy>
  <INVOICE>
   <CURRENCYCODE>USD</CURRENCYCODE>
   <INVOICEAMOUNT>12.00</INVOICEAMOUNT>
   <ISSUECOUNTRYCODE>CN</ISSUECOUNTRYCODE>
   <ORIGIN_COUNTRY>CN</ORIGIN_COUNTRY>
   <Amount>0</Amount>
   <CurrencyTypeCode>USD</CurrencyTypeCode>
   <INVOICEITEMS>
    <ITEMPRICE>12.0000</ITEMPRICE>
    <QUANTITY_STS>1</QUANTITY_STS>
    <ITEMORIGINCOUNTRY>CN</ITEMORIGINCOUNTRY>
   </INVOICEITEMS>
   <INCOTERM_ID>CIF</INCOTERM_ID>
   <ACCOUNTTYPE>380</ACCOUNTTYPE>
   <TRANSP_VALUE_LIST>
    <TRANSP_VALUE_L>0</TRANSP_VALUE_L>
    <TRANSP_VALUE_CURR_L>USD</TRANSP_VALUE_CURR_L>
   </TRANSP_VALUE_LIST>
  </INVOICE>
  <OriginCountryCode>US</OriginCountryCode>
  <CustomFileNo>60390074</CustomFileNo>
  <Id/>
  <DeclarationOfficeCode>4</DeclarationOfficeCode>
  <FileState>P</FileState>
  <AgentId>514193408</AgentId>
  <CustomerId>10015236</CustomerId>
  <TransportModeId>A</TransportModeId>
  <CreatedByUserId>AMITAL.COURIER</CreatedByUserId>
  <ReferentUserId/>
  <DepartmentId>MSC</DepartmentId>
  <MAWB>20261079</MAWB>
  <DealId/>
  <HAWB>09878498</HAWB>
  <ManifestNumber>99999560337</ManifestNumber>
  <LoadingPortCode/>
  <OriginCountryCode>US</OriginCountryCode>
  <CargoDescription>IBOX 233</CargoDescription>
  <PackageTypeCode>PP</PackageTypeCode>
  <PackageMeasureQualifierCode>2</PackageMeasureQualifierCode>
  <PackageQuantity>1</PackageQuantity>
  <GrossMassMeasure>0.30</GrossMassMeasure>
  <VendorId/>
  <ImporterId/>
  <Tenant>1</Tenant>
  <GrantDate/>
  <ManifestDate/>
  <ArrivalDateTime/>
  <Mode>NEW</Mode>
  <EnglishName>Kobi Cohen</EnglishName>
  <HebrewName/>
  <WarehouseId>ILMMN</WarehouseId>
  <UnloadportId/>
  <ProcedureCurrentCode>4000507</ProcedureCurrentCode>
  <ImporterAddress>Dekel 27 2nd avenu 13 ddk Tel Aviv</ImporterAddress>
  <CargoTypeCode>17</CargoTypeCode>
  <SecondCargoID>514193408</SecondCargoID>
  <ThirdCargoID>25.10.21</ThirdCargoID>
  <UnloadDate/>
  <IsCourierDeclaration>true</IsCourierDeclaration>
  <CasualSupplierName>Yaron Toys</CasualSupplierName>
  <CasualSupplierAddress>Yaron Toys-6546465 China</CasualSupplierAddress>
  <CourierHawb>99999560337</CourierHawb>
  <HAWBDATE/>
  <COUWTVAL/>
  <CasualImporterAddress1>Dekel 27 2nd avenu 13 ddk</CasualImporterAddress1>
  <CasualImporterAddress2/>
  <CasualImporterCity>Tel Aviv</CasualImporterCity>
  <CasualImporterZipCode>6546465</CasualImporterZipCode>
  <CasualImporterFax/>
  <CasualImporterEmail>ven@vendor.com</CasualImporterEmail>
  <CasualImportelTel>972089230879</CasualImportelTel>
  <CasualImporterContact/>
  <CasualImporterCountry/>
  <IsDiamondsDeclaration/>
  <EstimatedTimeOfArrival/>
  <OrderNumber>65161</OrderNumber>
  <WithPaper/>
  <NewFile>true</NewFile>
  <ImporterFile>BC32878</ImporterFile>
  <Team/>
  <FileOpenDate>20201026</FileOpenDate>
  <SiteCode>139514</SiteCode>
 </LogitudeCommDecFile>
</LOGICOMMDEC>"
                ;

            /*
"<?xml version="1.0" encoding="utf-8" ?>
<ArrayOfEntry xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
 <Entry>
  <Key>MODE</Key>
  <Value></Value>
 </Entry>
 <Entry>
  <Key>TENANT</Key>
  <Value>1</Value>
 </Entry>
 <Entry>
  <Key>UNIFREIGHT_USER_ID</Key>
  <Value>ITZIK</Value>
 </Entry>
</ArrayOfEntry>
"
             */
        }
        public string GetExampleDataIn1_()
        {

            var xml = "";
            var amitalObjExample = new LOGICOMMDEC();
            var myAmitalCommDec = new LogitudeCommDecFile();
            var myAmitalCommDecInvoice = new List<Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICE>();
            var myAmitalCommDecInvoiceItem = new List<Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile.INVOICEITEMS>();

            myAmitalCommDec.Id = "1-1";
            myAmitalCommDecInvoice[1].INVOICELINENO = "1";
            myAmitalCommDecInvoice[1].INVOICELINENO = "1";
            myAmitalCommDecInvoice[1].ACCOUNTTYPE = "380";
            myAmitalCommDecInvoice[1].INVOICENUMBER = "999";
            myAmitalCommDecInvoice[1].VENDORNUMBER = "2000475";
            myAmitalCommDecInvoice[1].CURRENCYCODE = "18";
            myAmitalCommDecInvoice[1].INVOICEAMOUNT = "2";
            myAmitalCommDec.INVOICE = myAmitalCommDecInvoice.ToArray();

            myAmitalCommDecInvoiceItem[1].CLASSIFICATIONCODE = "260300009";
            myAmitalCommDecInvoiceItem[1].TRADEAGREEMENTCODE = "BGR";
            myAmitalCommDecInvoiceItem[1].QUANTITY = "1";
            myAmitalCommDecInvoiceItem[1].ITEMPRICE = "1";
            myAmitalCommDecInvoiceItem[1].ITEMORIGINCOUNTRY = "AD";
            myAmitalCommDecInvoiceItem[1].ITEMCODE = "DFDF";
            myAmitalCommDec.INVOICE[1].INVOICEITEMS = myAmitalCommDecInvoiceItem.ToArray();

            amitalObjExample.LogitudeCommDecFile = new LogitudeCommDecFile[] { myAmitalCommDec };

            xml = XmlGenericUtil<LOGICOMMDEC>.SerializeObject(amitalObjExample);

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

