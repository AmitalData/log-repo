using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class CourierGWMessageECTHRDataMamanRequestService
    {
        private DeclarationPM _DeclarationPM;
        private CourierMasterPM _CourierMasterPM;

        public string BuildQueueSendWebAPI(string declarationId, int tenant, CourierMasterPM courierMasterPM = null)
        {
            
            var context = CustomContext.GetContext(tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            _DeclarationPM = myDeclarationQueryService.GetSingle(declarationId, true, false);
            if (_DeclarationPM == null)
            {
                throw new Exception($"Declaration not in DB declarationId={declarationId}");
            }
            if (!_DeclarationPM.IsCourierDeclaration)
            {
                throw new Exception($"Declaration Is not CourierDeclaration  declarationId={declarationId}");
            }
            //CourierDeclarations
            //myCourierMasterQueryService.GetNotConnectedDeclaratins

            _CourierMasterPM = courierMasterPM ?? myCourierMasterQueryService.GetByDeclarationId(declarationId, tenant);
            if (_CourierMasterPM == null)
            {
                //throw new Exception("Declaration is null:" + _CustomFileCreditModel.AppicationId);
                throw new Exception($"CourierMaster Is null  .GetByDeclarationId({declarationId}, tenant)");
            }

            GWMessageECTHRData myGWMessageECTHRData = CreateCourierHawbMamanMessage();
            string messageToMaman = "";
            messageToMaman = ProxyUtil.JsonConvertSerialize(myGWMessageECTHRData);
            using (var scop = TransactionFactory.GetTransaction())
            {
                byte[] bytearray = Encoding.UTF8.GetBytes(messageToMaman);


                //var myWebAPICourierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanResponseService();
                //myWebAPICourierGWMessageECTHRDataMamanService.BuildCommunicationLog(bytearray, tenant, declarationId);

                var webAPISendMessage2MamanService = new WebAPISendMessage2MamanService();
                webAPISendMessage2MamanService.BuildCommunicationLog(bytearray, tenant, declarationId, CustomsPartnerFtpDetails.InterfaceName_ECTHR);

                scop.Complete();
                //output  ftp://192.168.10.88/FTP_MAMAN/  
            }
            return "המסר נבנה בהצלחה וישלח בתהליך רקע";
        }

        private GWMessageECTHRData CreateCourierHawbMamanMessage()
        {
            var ConsignmentPackageQualifierCode2 = _DeclarationPM.Consignments.SelectMany(r => r.ConsignmentPackages)
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
            if (_DeclarationPM.SupplierInvoices.Count > 0)
            {
                DolarValue = _DeclarationPM.SupplierInvoices.Sum(r => r.InvoiceAmountInUSD.GetValueOrDefault());
            }

            string defBaldarCodeValue = GetDefault("ISRAEL", "CGO_CUST_FORW", "NON", "NON", _DeclarationPM.Tenant);
            var rep = new CustomsAirlineRepository(_CourierMasterPM.Tenant);
            var customsAirline = rep.GetSingle(_CourierMasterPM.AirlineId, _CourierMasterPM.Tenant);

            var courierHawbMamanModel = new GWMessageECTHRData()
            {
                BaldarCode = defBaldarCodeValue,//"לקחת מדיפולט קוד משלח בלדר",
                BaldarAwb = _DeclarationPM.CourierHAWB??"",
                //AirlineAwbPref = _CourierMasterPM.AirlineId,//יש לשלוח את Airline PRFIX)- 114
                AirlineAwbPref = _CourierMasterPM.AirlinePrefix??"",//יש לשלוח את Airline PRFIX)- 114

                Master = CInt(_CourierMasterPM.MAWB),
                Awb8 = CInt(_CourierMasterPM.ShortHAWB),
                HawbExtnd = _CourierMasterPM.HAWB??"",
                AirlineCode = customsAirline.AirlineCode??"",
                FltNo = CInt(_CourierMasterPM.FlightNumber),
                //FltDate = _CourierMasterPM.DepartureDate.GetValueOrDefault().Date,// fltdate is not nullable ??
                LandTime = _CourierMasterPM.EstimatedArrivalDate,// LandTime is not nullable ??
                DecNoOfPackags = DecNoOfPackags,
                DecWeight = DecWeight,
                DolarValue = DolarValue,
                StoreTypeReq = "67",//לפי טבלה B1                יש לשלוח תמיד 67
                Description = _DeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().CargoDescription??"",
                CustomerName = _DeclarationPM.ImporterName??"",
                CustomerAddress = _DeclarationPM.ImporterAddress??"",
                CustomerPhone = _DeclarationPM.CasualImporterTel??"",
//                DestLineDesc = "1",//יש לנהל קו הפרדה פר לקוח                יעד הפצה של חברת ההפצה לצורך בניית ממשקים
                DestLineDesc = "כללי",// - שינוי בשדה יעד המטען שליחה של "כללי" כברירת מחדל במקום 1
                BaldarMessageTime = DateTime.Now,
                BaldarHp = _DeclarationPM.AgentId??"",
                OpenBaldarAwbDate = GetOpenBaldarAwbDate(this._DeclarationPM),// _DeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().ThirdCargoID.GetValueOrDefault(),///ThirdCargoID.Consignment

                //Task 46455:
                DestLineCode = "9999999999",
                DeclarationId = this._DeclarationPM.DeclarationNumber,
                CustomIkuv = this._DeclarationPM.CourierSuspentionReasonCode,
                //Task 46455





            };

            if (_CourierMasterPM.DepartureDate.HasValue)
            {
                courierHawbMamanModel.FltDate = _CourierMasterPM.DepartureDate.GetValueOrDefault().Date;// fltdate is not nullable ??
            }
            return courierHawbMamanModel;
        }
        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            var myGDFDATAQueryService = new GDFDATAQueryService(AmitalContext.GetContext(tenant));

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            var myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }

        public static DateTime GetOpenBaldarAwbDate(DeclarationPM _DeclarationPM)
        {
            var myConsignmentPM=_DeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First();
            if (myConsignmentPM == null)
            {
                return DateTime.MinValue;
            }
            if (string.IsNullOrWhiteSpace(myConsignmentPM.ThirdCargoID)){
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
            string_Maybe_mAWB = list.LastOrDefault()??"";

            int res = 0;
            int.TryParse(string_Maybe_mAWB, out res);
            return res;
        }



        
    }
    public class GWMessageECTHRData
    {

        // not from  https://docs.google.com/document/d/1cjjeORaFsWMS32LhIxmEqNza3q7s7ZAr_PQVQOlwupw/edit#
        //from https://maman.wsfreeze.co.il/WebAPIExt/Help/Api/POST-api-baldar-CreateECTHRMessgae
        public string BaldarCode { get; set; }
        public string BaldarAwb { get; set; }

        public string AirlineAwbPref { get; set; }

        
        public int Master { get; set; }
        public int Awb8 { get; set; }

        public string HawbExtnd { get; set; }
        public string AirlineCode { get; set; }
        public int FltNo { get; set; }
        public DateTime? FltDate { get; set; }
        public DateTime? LandTime { get; set; }
        public int DecNoOfPackags { get; set; }
        public decimal DecWeight { get; set; }
        public decimal DolarValue { get; set; }
        public string StoreTypeReq { get; set; }
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string DestLineDesc { get; set; }
        //task 46455:
        public string DestLineCode { get; set; }
        public string DistributorHP { get; set; }
        public string DistributorName { get; set; }
        public string DeclarationId { get; set; }
        public string CustomIkuv { get; set; }
        //TAsk 46455.

        public DateTime BaldarMessageTime { get; set; }
        public string BaldarHp { get; set; }
        public DateTime OpenBaldarAwbDate { get; set; }

        public int ResponseStatusCode { get; set; }
        public string ResponseStatusMsg { get; set; }
        


    }
}