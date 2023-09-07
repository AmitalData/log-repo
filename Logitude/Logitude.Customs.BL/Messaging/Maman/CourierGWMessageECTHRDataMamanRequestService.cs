//https://docs.google.com/document/d/1cjjeORaFsWMS32LhIxmEqNza3q7s7ZAr_PQVQOlwupw/edit#bookmark=id.aszvu5fuahta

using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class CourierGWMessageECTHRDataMamanRequestService
    {
        //private DeclarationPM _DeclarationPM;
        //private CourierMasterPM _CourierMasterPM;
        public CourierGWMessageECTHRDataMamanRequestService()
        {

        }
        public string BuildQueueSendWebAPI(string declarationId, int tenant, DeclarationPM declarationPM=null, CourierMasterPM courierMasterPM = null)
        {
            string messageToMaman = GetMessage2Maman(declarationId, tenant, declarationPM, courierMasterPM);
            List<string> requiredField = GetRequiredField(messageToMaman);
            if (requiredField.Count > 0)
            {
                return $"חסרים שדות חובה :{String.Join(",", requiredField)}";
            }
            return BuildComm2Maman(declarationId, tenant, messageToMaman);
        }

        public  List<string> GetRequiredField(string messageToMaman)
        {
            return ProxyUtil.GetRequiredFieldInArrayJson(messageToMaman,
                            new List<string>()
                            {
                    "BaldarCode",
                    "BaldarAwb",
                    "AirlineAwbPref",
                    "Master",
                    "DecNoOfPackags",
                    "DecWeight",
                    "DolarValue",
                    "StoreTypeReq",
                    "Description",
                    "CustomerName",
                    "CustomerAddress",
                    "DestLineDesc",
                    "DestLineCode",
                    ///במסר שטר מטען בלדר תאורטי שדה מס' הצהרה יהפוך להיות O במקום M יש להוריד את הבדיקה של השדה משדות החובה- אפיון  "DeclarationId",
                    "BaldarHp",
                    "OpenBaldarAwbDate"
                            }
                            );
        }

        public string BuildComm2Maman(string declarationId, int tenant, string messageToMaman)
        {
            //using (var scop = TransactionFactory.GetTransaction())
            {
                byte[] bytearray = Encoding.UTF8.GetBytes(messageToMaman);


                //var myWebAPICourierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanResponseService();
                //myWebAPICourierGWMessageECTHRDataMamanService.BuildCommunicationLog(bytearray, tenant, declarationId);

                var webAPISendMessage2MamanService = new WebAPISendMessage2MasofService();
                webAPISendMessage2MamanService.BuildCommunicationLog(bytearray, tenant, declarationId, CustomsPartnerFtpDetails.InterfaceName_ECMMNTHR_REQUEST, CustomsPartnerFtpDetails.PartnerCode_Mamam);

                //scop.Complete();
                //output  ftp://192.168.10.88/FTP_MAMAN/  
            }
            return "המסר לממן נבנה בהצלחה וישלח בתהליך רקע ";
        }


        public string GetMessage2Maman(string declarationId, int tenant, DeclarationPM paramDeclarationPM, CourierMasterPM courierMasterPM, DeclarationCourierStatusPM declarationCourierStatusPM = null,bool ignoreIfCourierMasterNull=false)
        {
            var context = CustomContext.GetContext(tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var myCourierMasterQueryService = new CourierMasterQueryService(context);
            var myDeclarationPM = paramDeclarationPM ?? myDeclarationQueryService.GetSingle(declarationId, true, false);
            if (myDeclarationPM == null)
            {
                throw new Exception($"Declaration not in DB declarationId={declarationId}");
            }
            if (!myDeclarationPM.IsCourierDeclaration)
            {
                throw new Exception($"Declaration Is not CourierDeclaration  declarationId={declarationId}");
            }
            //if (myDeclarationPM.AcceptanceStatusCode != null)
            //{
            //    throw new Exception("לא ניתן לשדר מסר ש.מ.ב לממן לאחר קליטת זמינות");
            //}
            //CourierDeclarations
            //myCourierMasterQueryService.GetNotConnectedDeclaratins
            var myCourierMasterPM = courierMasterPM ?? myCourierMasterQueryService.GetByDeclarationId(declarationId, tenant);
            myCourierMasterPM = myCourierMasterPM ?? paramDeclarationPM?.MyEcomInsert?.MyCourierMasterPM;
            if (myCourierMasterPM == null)
            {if (ignoreIfCourierMasterNull) return null;
                //throw new Exception("Declaration is null:" + _CustomFileCreditModel.AppicationId);
                throw new Exception($"CourierMaster Is null  .GetByDeclarationId({declarationId}, tenant)");
            }

            GWMessageECTHRData myGWMessageECTHRData = CreateCourierHawbMamanMessage(myDeclarationPM, myCourierMasterPM, declarationCourierStatusPM);
            string messageToMaman = "";
            messageToMaman = ProxyUtil.JsonConvertSerialize(myGWMessageECTHRData);
            return messageToMaman;
        }

        private GWMessageECTHRData CreateCourierHawbMamanMessage(
            DeclarationPM myDeclarationPM, CourierMasterPM myCourierMasterPM, DeclarationCourierStatusPM declarationCourierStatusPM = null)
        {
            var context = CustomContext.GetContext(myDeclarationPM.Tenant);

            bool isDelay = false;
            var declarationMamanSpecialActionQueryService = new DeclarationMamanSpecialActionQueryService(context);
            var pmDeclarationMamanSpecialAction = declarationMamanSpecialActionQueryService.GetSingle(myDeclarationPM.Id, ((int)MamanSpecialCode.ReceivingDelayCertificate_DelayIt).ToString(), false, false);

            if(pmDeclarationMamanSpecialAction!= null && pmDeclarationMamanSpecialAction.MamanSpecialActionStatusCode=="1")
            {
                isDelay = true;
            }


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
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(myDeclarationPM.Tenant);


            string defBaldarCodeValue =
                //GetDefault("ISRAEL", "CGO_CUST_FORW", "NON", "NON", _DeclarationPM.Tenant);
                defaultValueQueryService.GetDefault("ISRAEL", "CGO_MMN_FORW", "NON", "NON", myDeclarationPM.Tenant);
            var rep = new CustomsAirlineRepository(myCourierMasterPM.Tenant);
            var customsAirline = rep.GetSingle(myCourierMasterPM.AirlineId, myCourierMasterPM.Tenant);

            //Get Trucker details - Task 49270
            string distributorHP = "";
            string distributorName = "";
            if(declarationCourierStatusPM == null)
            {
                DeclarationCourierStatusQueryService declarationCourierQueryService = new DeclarationCourierStatusQueryService(myDeclarationPM.Tenant);
                declarationCourierStatusPM = declarationCourierQueryService.GetSingle(myDeclarationPM.Id, false, false);
            }
            if (!string.IsNullOrWhiteSpace(declarationCourierStatusPM.TruckerId))
            {
                CardRepository cardRep = new CardRepository(myDeclarationPM.Tenant);
                Card card = cardRep.GetSingleCardCache(declarationCourierStatusPM.TruckerId, myDeclarationPM.Tenant);
                if (card != null)
                {
                    distributorHP = card.VatNumber;
                    distributorName = card.EnglishName;
                }
            }
            string MamanSuspendedCode = "";
            var courierPendingReasonRepository = new CourierPendingReasonRepository(myDeclarationPM.Tenant);
            var courierPendingListWithMamanSuspendedCode = courierPendingReasonRepository.GetPendingReasonsWithMamanSuspendedCode(myDeclarationPM.Tenant);

            var sb = new StringBuilder();
            sb.AppendLine($"DeclarationId:{declarationCourierStatusPM.DeclarationId};CourierPendingReasonList:{declarationCourierStatusPM.CourierPendingReasonList}");
            if (!string.IsNullOrEmpty(declarationCourierStatusPM.CourierPendingReasonList))
            {
                var arrPendings = declarationCourierStatusPM.CourierPendingReasonList.Split(',');

                var pendingCounted = courierPendingListWithMamanSuspendedCode.Where(x => arrPendings.Contains(x.Code)).Count();   //MamanSuspendedCode=declarationPendingWithMamanSuspendCode = declarationCourierStatus.CourierPendingReasonList;
                if (pendingCounted == 1)
                {
                    MamanSuspendedCode = courierPendingListWithMamanSuspendedCode.Where(x => arrPendings.Contains(x.Code)).FirstOrDefault().MamanSuspendedCode;
                    sb.AppendLine($"pendingCounted == 1;MamanSuspendedCode:{MamanSuspendedCode}");
                }
                if (pendingCounted > 1)
                {
                    sb.AppendLine($"pendingCounted > 1;MamanSuspendedCode:9999");
                    MamanSuspendedCode = "9999";
                }
            }
            if (Send2MasofIfNeededService.GetStopLogAt() > DateTime.Now)
            {
                Logger.LogMe(sb.ToString(), false, "GWMessageECTHRDataMaman");
            }
            
            string aw8 = null;
            if (!string.IsNullOrWhiteSpace(myCourierMasterPM.ShortHAWB) && CInt(myCourierMasterPM.ShortHAWB)!=0)
            {
                aw8 = CInt(myCourierMasterPM.ShortHAWB).ToString();
            }
            var courierHawbMamanModel = new GWMessageECTHRData()
            {
                BaldarCode = defBaldarCodeValue,//"לקחת מדיפולט קוד משלח בלדר",
                BaldarAwb = myDeclarationPM.CourierHAWB ?? "",
                //AirlineAwbPref = _CourierMasterPM.AirlineId,//יש לשלוח את Airline PRFIX)- 114
                AirlineAwbPref = myCourierMasterPM.AirlinePrefix ?? "",//יש לשלוח את Airline PRFIX)- 114

                Master = CInt(myCourierMasterPM.MAWB),
                Awb8 = aw8,
                HawbExtnd = myCourierMasterPM.HAWB ?? "",
                AirlineCode = customsAirline.AirlineCode ?? "",
                FltNo = CInt(myCourierMasterPM.FlightNumber),
                //FltDate = _CourierMasterPM.DepartureDate.GetValueOrDefault().Date,// fltdate is not nullable ??
                LandTime = myCourierMasterPM.EstimatedArrivalDate,// LandTime is not nullable ??
                DecNoOfPackags = DecNoOfPackags,
                DecWeight = DecWeight,
                DolarValue = DolarValue,
                StoreTypeReq = "67",//לפי טבלה B1                יש לשלוח תמיד 67
 
                Description =  myDeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().CargoDescription != null ? Regex.Replace(myDeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().CargoDescription, @"(\-)|(\%)|(\()|(\))|(\.)", "") : "" ,
                CustomerName = myDeclarationPM.ImporterName != null ? Regex.Replace(myDeclarationPM.ImporterName, @"(\-)|(\%)|(\()|(\))|(\.)|(\$)|(\{)|(\})", "") : "",
                CustomerAddress = myDeclarationPM.ImporterAddress != null ? Regex.Replace(myDeclarationPM.ImporterAddress, @"(\-)|(\%)|(\()|(\))|(\.)|(\$)|(\{)|(\})", ""): "",
                 CustomerPhone = myDeclarationPM.CasualImporterTel ?? "",
                //                DestLineDesc = "1",//יש לנהל קו הפרדה פר לקוח                יעד הפצה של חברת ההפצה לצורך בניית ממשקים
                DestLineDesc = declarationCourierStatusPM.DistributionArea ?? "כללי",// " - שינוי בשדה יעד המטען שליחה של "כללי" כברירת מחדל במקום 1
                BaldarMessageTime = DateTime.Now,
                BaldarHp = myDeclarationPM.AgentId??"",
                OpenBaldarAwbDate = GetOpenBaldarAwbDate(myDeclarationPM),// _DeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().ThirdCargoID.GetValueOrDefault(),///ThirdCargoID.Consignment

                //Task 46455:
                DestLineCode = "9999999999",
                DeclarationId = myDeclarationPM.DeclarationNumber,
                CustomIkuv = myDeclarationPM.CourierCustomStatusCode=="1"?"3": (!string.IsNullOrEmpty(myDeclarationPM.CourierCustomStatusCode) ? myDeclarationPM.CourierSuspentionCode: MamanSuspendedCode),//task 49300
                //CustomIkuv = myDeclarationPM.CourierSuspentionReasonCode,
                //Task 46455
                DistributorHP = distributorHP,
                DistributorName = distributorName,
                IsDelay = isDelay,
                //StorageSite = myDeclarationPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().StorageSiteCode ?? ""

            };

            if (myCourierMasterPM.DepartureDate.HasValue)
            {
                courierHawbMamanModel.FltDate = myCourierMasterPM.DepartureDate.GetValueOrDefault().Date;// fltdate is not nullable ??
            }
            return courierHawbMamanModel;
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
        public /*int*/ string Awb8 { get; set; }

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
        public bool IsDelay { get; set; }

        //public string StorageSite { get; set; }

    }
}