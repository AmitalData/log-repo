//http://81.218.57.34:9094/Help/Api/POST-api-Courier-UpdateHawbStatus
//https://docs.google.com/document/d/1bFMdrDnByDpvLcvE9H5eOfCAzbVdeoUypbzhwxbr0Po/edit#

using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.ILSWS
{
    public class CourierECSWSTHRMessageRequestService
    {
        //private DeclarationPM _DeclarationPM;
        //private CourierMasterPM _CourierMasterPM;

        public string BuildQueueSendWebAPI(string declarationId, int tenant,  DeclarationPM declarationPM = null, CourierMasterPM courierMasterPM = null)
        {
            string messageToSWS = GetMessageUpdateHawbStatus(declarationId, tenant, declarationPM, courierMasterPM);
            List<string> requiredField = GetRequiredField(messageToSWS);
            if (requiredField.Count > 0)
            {
                return $"חסרים שדות חובה :{String.Join(",", requiredField)}";
            }
            XmlDocument XMLmessageToSWS = DeserializeXmlNode(messageToSWS);
            return BuildUpdateHawbStatus(declarationId, tenant, XMLmessageToSWS);
        }

        public XmlDocument DeserializeXmlNode(string messagetoILSWS)
        {
            return ProxyUtil.DeserializeXmlNode(messagetoILSWS); // can remove once they start accepting json format
        }
        public List<string> GetRequiredField(string messagetoILSWS)
        {
            return ProxyUtil.GetRequiredFieldInArrayJson(messagetoILSWS,
                      new List<string>()
                      {
                    "CourierHawbDate",
                    "MawbPrefix",
                    "Mawb",
                    "PackageQuantity",
                    //"DecNoOfPackgs",
                    "Weight",
                    "GoodValueInUSD",
                    "ImporterName",
                    "ImporterAddress",
                      }
                      );
        }

        public  string BuildUpdateHawbStatus(string declarationId, int tenant, XmlDocument messageToSWS)
        {
            //using (var scop = TransactionFactory.GetTransaction())
            {
                byte[] bytearray = Encoding.UTF8.GetBytes(messageToSWS.OuterXml);



                bool haveDefinition = true;
                FTPOutMawbSWSService fTPOutMawbSWSServie = new FTPOutMawbSWSService();
                var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var defDefaultJSON = customsPartnerFtpDetails.GetAllInterfaceName().First(r => r.Key == CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_REQUEST).Value;
                var defDefault = ProxyUtil.JsonConvertDeserializeTyped<InterfaceDetails>(defDefaultJSON);
                ConsignmentRepository consignmentRepository = new ConsignmentRepository(tenant);
                string manifestnumber = consignmentRepository.GetManfiestNumberByDecId(declarationId, tenant);
                Guid g = Guid.NewGuid();
                string filename = manifestnumber + "_" + g;
                if (haveDefinition)
                {
                    fTPOutMawbSWSServie.BuildCommunicationLog(bytearray, tenant, declarationId, CustomsPartnerFtpDetails.InterfaceName_ECSWSTHR_REQUEST, filename);
                }
                ///scop.Complete();
                //output  ftp://192.168.10.88/FTP_MAMAN/  
            }
            return "המסר לסוויספורט נבנה בהצלחה וישלח בתהליך רקע ";
        }

        public string GetMessageUpdateHawbStatus(string declarationId, int tenant,  DeclarationPM declarationPM , CourierMasterPM courierMasterPM)
        {
            var context = CustomContext.GetContext(tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            var myDeclarationPM = declarationPM??myDeclarationQueryService.GetSingle(declarationId, true, false);
            if (myDeclarationPM == null)
            {
                throw new Exception($"Declaration not in DB declarationId={declarationId}");
            }
            if (!myDeclarationPM.IsCourierDeclaration)
            {
                throw new Exception($"Declaration Is not CourierDeclaration  declarationId={declarationId}");
            }
            //CourierDeclarations
            //myCourierMasterQueryService.GetNotConnectedDeclaratins

            var myCourierMasterPM = courierMasterPM ?? myCourierMasterQueryService.GetByDeclarationId(declarationId, tenant);
            if (myCourierMasterPM == null)
            {
                //throw new Exception("Declaration is null:" + _CustomFileCreditModel.AppicationId);
                throw new Exception($"CourierMaster Is null  .GetByDeclarationId({declarationId}, tenant)");
            }

            CourierSWSHAWBRequest myCourierSWSHAWBRequest = CreateCourierSWSHawbMessage(myDeclarationPM, myCourierMasterPM);
            string messageToSWS = "";
            messageToSWS = ProxyUtil.JsonConvertSerialize(myCourierSWSHAWBRequest);
            return messageToSWS;
        }

        private CourierSWSHAWBRequest CreateCourierSWSHawbMessage(DeclarationPM myDeclarationPM, CourierMasterPM myCourierMasterPM)
        {
            var ConsignmentPackageQualifierCode2 = myDeclarationPM.Consignments.SelectMany(r => r.ConsignmentPackages)
                .Where(r1 => r1.PackageMeasureQualifierCode == "2")
                .ToList();
            decimal DecWeight = 0;
            int DecNoOfPackags = 0;
            decimal DolarValue = 0;
            if (ConsignmentPackageQualifierCode2 != null && ConsignmentPackageQualifierCode2.Count > 0)
            {

                DecWeight = ConsignmentPackageQualifierCode2.Sum(r => r.GrossMassMeasure.GetValueOrDefault());
                DecNoOfPackags = ConsignmentPackageQualifierCode2.Sum(r => r.PackageQuantity.GetValueOrDefault());

            }
            if (myDeclarationPM.SupplierInvoices.Count > 0)
            {
                DolarValue = myDeclarationPM.SupplierInvoices.Sum(r => r.InvoiceAmountInUSD.GetValueOrDefault());
            }

            //string defBaldarCodeValue = GetDefault("ISRAEL", "CGO_CUST_FORW", "NON", "NON", _DeclarationPM.Tenant);
            var rep = new CustomsAirlineRepository(myCourierMasterPM.Tenant);
            var customsAirline = rep.GetSingle(myCourierMasterPM.AirlineId, myCourierMasterPM.Tenant);

            string crateNumber = "";
            var context = CustomContext.GetContext(myDeclarationPM.Tenant);
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(myDeclarationPM.Id, true, false);
            if(currentDeclarationCourierStatusPM != null && !String.IsNullOrWhiteSpace(currentDeclarationCourierStatusPM.CrateNumber))crateNumber = currentDeclarationCourierStatusPM.CrateNumber;

            string importerVat = "";
            if (!String.IsNullOrWhiteSpace(myDeclarationPM.ImporterId))
            {
                ClientQueryService clientQueryService = new ClientQueryService(myCourierMasterPM.Tenant);

                var clientPM = clientQueryService.GetSingle(myDeclarationPM.ImporterId, false, true); 
                if (clientPM != null && !String.IsNullOrWhiteSpace(clientPM.Code)) importerVat = clientPM.Code;
            }
            else if(!String.IsNullOrWhiteSpace(myDeclarationPM.ImporterCode))
            {
                importerVat = myDeclarationPM.ImporterCode;
            }


            var courierHawbMamanModel = new CourierSWSHAWBRequest()
            {

                //BaldarCode = defBaldarCodeValue,//"לקחת מדיפולט קוד משלח בלדר",
                CourierCompanyVat = myDeclarationPM.AgentId ?? "",
                CourierHawbNumber = myDeclarationPM.CourierHAWB ?? "",
                CourierHawbDate = GetOpenBaldarAwbDate(myDeclarationPM),
                MawbPrefix = myCourierMasterPM.AirlinePrefix ?? "",//יש לשלוח את Airline PRFIX)- 114
                Mawb = myCourierMasterPM.MAWB,//Mawb = CInt(myCourierMasterPM.MAWB),

                Hawb = myCourierMasterPM.HAWB ?? "",
                
                FlightNumber = CInt(myCourierMasterPM.FlightNumber),
                DepartureDate= myCourierMasterPM.DepartureDate,// LandTime is not nullable ??
                EstimatedArrivalDate = myCourierMasterPM.EstimatedArrivalDate,// LandTime is not nullable ??
                PackageQuantity = DecNoOfPackags,
                Weight = DecWeight,
                GoodValueInUSD = DolarValue,

                
                Description = myDeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().CargoDescription ?? "",
                ImporterName = myDeclarationPM.ImporterName ?? "",
                ImporterAddress = myDeclarationPM.ImporterAddress ?? "",
                DistributionLine = string.IsNullOrEmpty( currentDeclarationCourierStatusPM.DistributionArea)?"כללי" : currentDeclarationCourierStatusPM.DistributionArea,



                DistributionCompanyVat = "",


                DeclarationNumber = myDeclarationPM.DeclarationNumber??"",
                CustomsSuspention = myDeclarationPM.CourierSuspentionCode??"",
                Preclearence = myDeclarationPM.CourierCustomStatusCode== "1"  /*released*/,

                ImporterVat = importerVat,
                BoxBarcode = crateNumber,


            };


            if (!string.IsNullOrWhiteSpace(currentDeclarationCourierStatusPM.TruckerId))
            {
                CardRepository cardRep = new CardRepository(myDeclarationPM.Tenant);
                Card card = cardRep.GetSingleCardCache(currentDeclarationCourierStatusPM.TruckerId, myDeclarationPM.Tenant);
                if (card != null)
                {
                    courierHawbMamanModel.DistributionCompanyVat = card.VatNumber;
                }
            }


            return courierHawbMamanModel;
        }

        public static DateTime GetOpenBaldarAwbDate(DeclarationPM _DeclarationPM)
        {
            var myConsignmentPM = _DeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First();
            if (myConsignmentPM == null)
            {
                return DateTime.MinValue;
            }
            if (string.IsNullOrWhiteSpace(myConsignmentPM.ThirdCargoID))
            {
                return DateTime.MinValue;
            }
            DateTime d = DateTime.MinValue;
            DateTime.TryParse(myConsignmentPM.ThirdCargoID, out d);
            if (d == DateTime.MinValue)
            {
                DateTime.TryParseExact(myConsignmentPM.ThirdCargoID, "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
            }
            return d;
        }

        private int CInt(string string_Maybe_mAWB)
        {
            string_Maybe_mAWB = string_Maybe_mAWB ?? "";
            var list = string_Maybe_mAWB.Split('-').ToList();
            string_Maybe_mAWB = list.LastOrDefault() ?? "";

            int res = 0;
            int.TryParse(string_Maybe_mAWB, out res);
            return res;
        }




    }
    public class CourierSWSHAWBRequest
    {

        //https://docs.google.com/document/d/1bFMdrDnByDpvLcvE9H5eOfCAzbVdeoUypbzhwxbr0Po/edit#
        
        public string CourierCompanyVat { get; set; }
        public string CourierHawbNumber { get; set; }
        public DateTime CourierHawbDate { get; set; }
        public string MawbPrefix { get; set; }


        public string Mawb { get; set; }
        

        public string Hawb { get; set; }
        
        public int FlightNumber { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
        public int PackageQuantity { get; set; }
        public decimal Weight { get; set; }
        public decimal GoodValueInUSD { get; set; }
        //public string StoreTypeReq { get; set; }
        public string Description { get; set; }
        public string ImporterName { get; set; }
        public string ImporterAddress { get; set; }
        //public string CustomerPhone { get; set; }
        public string DistributionLine { get; set; }
        public string DistributionCompanyVat { get; set; }

        //public string DestLineDesc { get; set; }
        //task 46455:
        //public string DestLineCode { get; set; }
        //public string DistributorHP { get; set; }
        //public string DistributorName { get; set; }
        public string DeclarationNumber { get; set; }
        public string CustomsSuspention { get; set; }
        public bool Preclearence { get; set; }

        //public string CustomIkuv { get; set; }
        //TAsk 46455.

        //public DateTime BaldarMessageTime { get; set; }



        //public int ResponseStatusCode { get; set; }
        //public string ResponseStatusMsg { get; set; }

        public string ImporterVat { get; set; }

        public string BoxBarcode { get; set; }

    }

    public class CourierSWSHAWBResponse
    {
        public string CourierCompanyVat { get; set; }
        public string CourierHawbNumber { get; set; }
        public string StatusCode { get; set; }//1-    תקין//2 -    שגוי
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }

    }
}