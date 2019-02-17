

//http://81.218.57.34:9094/
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.ILOVS
{

    public class CourierOVSSpecialActionRequestService: ICourierGWMessageECSpcRequestService
    {
        private DeclarationPM _DeclarationPM;
        private CourierMasterPM _CourierMasterPM;


        public static string TestSend()
        {
            var OVSSpecialActionRequestService = new CourierOVSSpecialActionRequestService();
            string actionResultString = OVSSpecialActionRequestService.BuildQueueSendWebAPI("1-115843", 1, MamanActionCodeUpdateOrCancel.Upsert, MamanSpecialCode.StickerPrinting);
            return actionResultString;
        }

        public string BuildQueueSendWebAPI(
            string declarationId, int tenant,
            MamanActionCodeUpdateOrCancel mamanActionCode,
            MamanSpecialCode mamanSpecialCode
            /*CourierMasterPM courierMasterPM = null*/)
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


            //בעת שליחת המסר תבוצע שליפה של טבלת DeclarationMamanSpecialAction לפי מפתח הצהרה + קוד פעולה מיוחדת, והנתונים יישלחו לפי קוד פעולה שהמשתמש בחר + נתונים מ DB של הצהרה + DeclarationMamanSpecialAction
            var declarationMamanSpecialActionQueryService = new DeclarationMamanSpecialActionQueryService(context);
            var pmDeclarationMamanSpecialAction = declarationMamanSpecialActionQueryService.GetSingle(declarationId, ((int)mamanSpecialCode).ToString(), false, false);


            OVSECSpclRequest myECSpclMamanData = CreateCourierECSpclMamanMessage(pmDeclarationMamanSpecialAction, mamanActionCode, mamanSpecialCode);
            string messageToMaman = "";
            messageToMaman = ProxyUtil.JsonConvertSerialize(myECSpclMamanData);
            using (var scop = TransactionFactory.GetTransaction())
            {
                byte[] bytearray = Encoding.UTF8.GetBytes(messageToMaman);


                //var myWebAPICourierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanResponseService();
                //myWebAPICourierGWMessageECTHRDataMamanService.BuildCommunicationLog(bytearray, tenant, declarationId);

                var webAPISendMessage2MamanService = new WebAPISendMessage2MasofService();
                webAPISendMessage2MamanService.BuildCommunicationLog(bytearray, tenant, declarationId, CustomsPartnerFtpDetails.InterfaceName_ECOVSSPCL_REQUEST, CustomsPartnerFtpDetails.PartnerCode_ILOVS);

                scop.Complete();
                //output  ftp://192.168.10.88/FTP_MAMAN/  
            }
            return "המסר נבנה בהצלחה וישלח בתהליך רקע";
        }

        private OVSECSpclRequest CreateCourierECSpclMamanMessage(
            DeclarationMamanSpecialActionPM pmDeclarationMamanSpecialAction,
            MamanActionCodeUpdateOrCancel mamanActionCode,
            MamanSpecialCode mamanSpecialCode)
        {
            string mamanActionCodeUpdateOrCancel = "";
            switch (mamanActionCode)
            {
                case MamanActionCodeUpdateOrCancel.Upsert:
                    mamanActionCodeUpdateOrCancel = "U";
                    break;
                case MamanActionCodeUpdateOrCancel.Cancel:
                    mamanActionCodeUpdateOrCancel = "C";
                    break;

            }
            string mamanSpecialActionCode = "";
            switch (mamanSpecialCode)
            {
                case MamanSpecialCode.ReceivingDelayCertificate_DelayIt:
                    mamanSpecialActionCode = "2";
                    break;
                case MamanSpecialCode.StickerPrinting:
                    mamanSpecialActionCode = "4";
                    break;
                case MamanSpecialCode.PrintDocuments:
                    mamanSpecialActionCode = "5";
                    break;

            }
            return new OVSECSpclRequest()
            {
                MessageType = mamanActionCodeUpdateOrCancel,
                CourierCompanyVat = _DeclarationPM.AgentId?? "",
                CourierHawbNumber = _DeclarationPM.CourierHAWB ?? "",
                CourierHawbDate = Maman.CourierGWMessageECTHRDataMamanRequestService.GetOpenBaldarAwbDate(this._DeclarationPM),
                SpecialActionCode = mamanSpecialActionCode ?? "",
                LabelText1 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText1 ?? "" : "",
                LabelText2 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText2 ?? "" : "",
                LabelText3 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText3 ?? "" : "",
                LabelText4 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText4 ?? "" : "",
                LabelText5 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText5 ?? "" : "",


            };
        }
    }
    public class OVSECSpclRequest
    {
        public string MessageType { get; set; }
        public string CourierCompanyVat { get; set; }
        public string CourierHawbNumber { get; set; }
        public DateTime CourierHawbDate { get; set; }
        public string SpecialActionCode { get; set; }

        public string LabelText1 { get; set; }
        public string LabelText2 { get; set; }
        public string LabelText3 { get; set; }
        public string LabelText4 { get; set; }
        public string LabelText5 { get; set; }

    }
}
