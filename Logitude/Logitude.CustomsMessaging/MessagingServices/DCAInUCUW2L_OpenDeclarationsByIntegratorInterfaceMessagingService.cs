using Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.MessageLib.Ransom;
 using UnifreightIIG.Common.SystemTableServiceReference;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCUW2LResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService, RequestHeader>
    {

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCUW2LResponseContentHeader customsResponse)
        {
            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument"),
                  LoggingEntityId = customsResponse.CustomFileNo
            };
            return myGenericRequestParams;

        }

        protected override DCAInUCUW2LResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "UCUW2L"; }
        }


        public string CreateCRS(int tenant, string LoggingUserId, GenericRequestParams mySendALLStorageSiteRequestParams)
        {

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var customsRequestsSheetQS = new CustomsRequestsSheetQueryService(tenant);
            //var RequestInProgressList = customsRequestsSheetQS.GetRequestInProgress(tenant, this.MainInterfaceCode, objectTableId, mySendALLStorageSiteRequestParams.CourierMasterId, null, null, null, true);
            //if (RequestInProgressList != null && RequestInProgressList.Count > 0)
            //{
            //    return "קיים מסר זהה בתהליך";
            //}
            LogMessagingUtil.Instance.AppendLine("Build !!!Requestsheet  with Interface Type  = UCUW2L  !!!");

            string uniComm = null;
            string fileName = null;
            var transmitionDateTime = DateTime.Now;
            string xmlESBResponseXmlClass = null;

            var myDCAInUCBCMSSWithResponseContentHeader = new DCAInUCUW2LResponseContentHeader()
            {
                LoggingUserId = "1-9",
                CustomFileNo = "122198",
                LOGICOMMDEC = @"<LOGICOMMDEC>

<LogitudeCommDecFile>

  <ImporterCode>200400820</ImporterCode>

  <LoadingPortCode>USBOS</LoadingPortCode>

  <MAWB>1111223</MAWB>

  <CarrierPrefix>114</CarrierPrefix>

  <IsAutonomy/>

  <INVOICE>

   <CURRENCYCODE>USD</CURRENCYCODE>

   <INVOICEAMOUNT>300.00</INVOICEAMOUNT>

   <ISSUECOUNTRYCODE>CN</ISSUECOUNTRYCODE>

   <ORIGIN_COUNTRY/>

   <Amount/>

   <CurrencyTypeCode/>

   <INVOICEITEMS>

    <ITEMPRICE>300.0000</ITEMPRICE>

    <QUANTITY_STS>1</QUANTITY_STS>

    <ITEMORIGINCOUNTRY>CN</ITEMORIGINCOUNTRY>

   </INVOICEITEMS>

   <INCOTERM_ID>CIF</INCOTERM_ID>

   <ACCOUNTTYPE>380</ACCOUNTTYPE>

   <TRANSP_VALUE_LIST>

    <TRANSP_VALUE_L>103.25</TRANSP_VALUE_L>

    <TRANSP_VALUE_CURR_L>USD</TRANSP_VALUE_CURR_L>

   </TRANSP_VALUE_LIST>

  </INVOICE>

  <OriginCountryCode>US</OriginCountryCode>

  <CustomFileNo>206207</CustomFileNo>

  <Id/>

  <DeclarationOfficeCode>4</DeclarationOfficeCode>

  <FileState>P</FileState>

  <AgentId>514193408</AgentId>

  <CustomerId>10015236</CustomerId>

  <TransportModeId>A</TransportModeId>

  <CreatedByUserId>AMITAL.COURIER</CreatedByUserId>

  <ReferentUserId/>

  <DepartmentId>MSC</DepartmentId>

  <MAWB>1111223</MAWB>

  <DealId/>

  <HAWB/>

  <ManifestNumber>680680</ManifestNumber>

  <LoadingPortCode/>

  <OriginCountryCode>US</OriginCountryCode>

  <CargoDescription>Electric Screwdriver125mm long angle grinder</CargoDescription>

  <PackageTypeCode>PP</PackageTypeCode>

  <PackageMeasureQualifierCode>2</PackageMeasureQualifierCode>

  <PackageQuantity>1</PackageQuantity>

  <GrossMassMeasure>4.13</GrossMassMeasure>

  <VendorId/>

  <ImporterId>200400820</ImporterId>

  <Tenant>1</Tenant>

  <GrantDate/>

  <ManifestDate/>

  <ArrivalDateTime/>

  <Mode>NEW</Mode>

  <EnglishName>sraya zeevi 1</EnglishName>

  <HebrewName/>

  <WarehouseId>ILOVL</WarehouseId>

  <UnloadportId/>

  <ProcedureCurrentCode>4000507</ProcedureCurrentCode>

  <ImporterAddress>Sokolov 77,Apt 26 Herzliya ISRAEL</ImporterAddress>

  <CargoTypeCode>17</CargoTypeCode>

  <SecondCargoID>514193408</SecondCargoID>

  <ThirdCargoID>21.02.21</ThirdCargoID>

  <UnloadDate/>

  <IsCourierDeclaration>true</IsCourierDeclaration>

  <CasualSupplierName>Ningbo GI Power Imp.&amp;Exp.Co.,LTD</CasualSupplierName>

  <CasualSupplierAddress>Jishigang Fengtai Road no. 99 Shenz China</CasualSupplierAddress>

  <CourierHawb>680680</CourierHawb>

  <HAWBDATE/>

  <COUWTVAL/>

  <CasualImporterAddress1>Sokolov 77,Apt 26</CasualImporterAddress1>

  <CasualImporterAddress2/>

  <CasualImporterCity>Herzliya</CasualImporterCity>

  <CasualImporterZipCode>4600916</CasualImporterZipCode>

  <CasualImporterFax/>

  <CasualImporterEmail/>

  <CasualImportelTel>03 9243399</CasualImportelTel>

  <CasualImporterContact/>

  <CasualImporterCountry>IL</CasualImporterCountry>

  <IsDiamondsDeclaration/>

  <EstimatedTimeOfArrival/>

  <OrderNumber/>

  <WithPaper/>

  <FileStatus/>

  <NewFile>true</NewFile>

  <ImporterFile/>

  <Team/>

  <FileOpenDate>21.02.21</FileOpenDate>

  <TruckerId/>

  <DistributionArea/>

  <FclLcl/>

  <ForwarderId/>

</LogitudeCommDecFile>

</LOGICOMMDEC>",
                MoreParams="",
                tenant=1,

                ResponseContentHeader = new DefaultResponseContentHeader()
                {
                    TransmitionDateTime = transmitionDateTime
                },
            };

            var body = XmlGenericUtil<DCAInUCUW2LResponseContentHeader>.SerializeObject(myDCAInUCBCMSSWithResponseContentHeader);
            body = body.Substring(body.IndexOf(Environment.NewLine));
            var myESBResponseXmlClass = new ESBResponseXmlClass();
            var extrenalId = "62833ff7-1cd3-4faa-85a6-a4312ae4797a";
            extrenalId = uniComm ?? Guid.NewGuid().ToString();
            xmlESBResponseXmlClass = myESBResponseXmlClass.Get(Guid.NewGuid().ToString(), extrenalId, body);
            var transTime = "2016-04-19_13-35-13-481";

            transTime = transmitionDateTime.ToString("s").Replace("T", "_").Replace(":", "-");
            transTime += "-";
            transTime += transmitionDateTime.Millisecond.ToString();

            fileName = "DcaPrefixName.IL941079089." + transTime + "." + extrenalId + ".PLT.xml";
            //var messService = new Logitude.CustomsMessaging.MessagingServices.DF_MSG10000_ImportDeclarationMessagingService();
            //var responseData = messService.SendSheet(genericRequestParams);

            var ourRef = "";
            using (var trans = TransactionFactory.GetNewTransaction())
            {
                try
                {
                    var InterfaceManagementQS = new InterfaceManagementQueryService(tenant);
                    var InterfaceManagementPM = InterfaceManagementQS.GetSingleInterfaceManagementwithDefinition(
                        this.MainInterfaceCode, tenant);
                    fileName = fileName.Replace("DcaPrefixName.", InterfaceManagementPM.DcaPrefixName);
                    ourRef = this.DcaReceivedCustomResponseCorrelation(InterfaceManagementPM, tenant, new Customs.BL.Utils.DCAFileModel()
                    {
                        SelectedFileDownload = fileName,
                        TimStamp = transmitionDateTime

                    }, xmlESBResponseXmlClass);

                    trans.Complete();
                    return "המסר נבנה בהצלחה וישלח בתהליך רקע";
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine(" UCBCMSS SameRequestInProgress!! " + myCustomsRequestsSheetServiceException.Message);
                    }
                    else if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.NoAvailableSignServer)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("UCBCTML SameRequestInProgress!!  " + myCustomsRequestsSheetServiceException.Message);
                    }
                    return "קיים מסר זהה בתהליך";
                }
            }
        }



    }

    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCUW2LResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCUW2LResponseContentHeader")]
    public class DCAInUCUW2LResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
         public string CustomFileNo { get; set; }
        public string LOGICOMMDEC { get; set; }
        public string MoreParams { get; set; }
        //public string master { get; set; }

        //public LOGIDOCS MyLOGIDOCS { get; set; }

        //public string MyMoreParams { get; set; }
        //public string DocumentsFilingCode { get; set; }
        //public string DocumentsFilingId { get; set; }
        //public string DOCUMENTTYPEID { get; set; }
        //public string DocumentTypeCode { get; set; }

        //public string DocumentTypeId { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }

}
