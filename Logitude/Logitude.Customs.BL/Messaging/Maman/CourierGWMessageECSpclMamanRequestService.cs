#define waitTillMiritWillCreateDBAndScreen
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

namespace Logitude.Customs.BL.Messaging.Maman
{
    
   


    public class CourierGWMessageECSpclMamanRequestService : ICourierGWMessageECSpcRequestService
    {
        private DeclarationPM _DeclarationPM;
        private CourierMasterPM _CourierMasterPM;
        

        public static string TestSend()
        {
            var courierGWMessageECSpclMamanRequestService =new CourierGWMessageECSpclMamanRequestService();
            string actionResultString =courierGWMessageECSpclMamanRequestService.BuildQueueSendWebAPI("1-115843", 1, MamanActionCodeUpdateOrCancel.Upsert, MamanSpecialCode.StickerPrinting );
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

#if waitTillMiritWillCreateDBAndScreen

            //בעת שליחת המסר תבוצע שליפה של טבלת DeclarationMamanSpecialAction לפי מפתח הצהרה + קוד פעולה מיוחדת, והנתונים יישלחו לפי קוד פעולה שהמשתמש בחר + נתונים מ DB של הצהרה + DeclarationMamanSpecialAction
            var declarationMamanSpecialActionQueryService = new DeclarationMamanSpecialActionQueryService(context);
            var pmDeclarationMamanSpecialAction =declarationMamanSpecialActionQueryService.GetSingle(declarationId,  ((int)mamanSpecialCode).ToString(),false,false);

#endif

            ECSpclMamanMessage myECSpclMamanData = CreateCourierECSpclMamanMessage(pmDeclarationMamanSpecialAction, mamanActionCode, mamanSpecialCode);
            string messageToMaman = "";
            messageToMaman = ProxyUtil.JsonConvertSerialize(myECSpclMamanData);
            using (var scop = TransactionFactory.GetTransaction())
            {
                byte[] bytearray = Encoding.UTF8.GetBytes(messageToMaman);


                //var myWebAPICourierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanResponseService();
                //myWebAPICourierGWMessageECTHRDataMamanService.BuildCommunicationLog(bytearray, tenant, declarationId);

                var webAPISendMessage2MamanService = new WebAPISendMessage2MasofService();
                webAPISendMessage2MamanService.BuildCommunicationLog(bytearray, tenant, declarationId, CustomsPartnerFtpDetails.InterfaceName_ECSPCL, CustomsPartnerFtpDetails.PartnerCode_Mamam);

                scop.Complete();
                //output  ftp://192.168.10.88/FTP_MAMAN/  
            }
            return "המסר נבנה בהצלחה וישלח בתהליך רקע";
        }

        private ECSpclMamanMessage CreateCourierECSpclMamanMessage(
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
                case MamanSpecialCode.StickerPrinting :
                    mamanSpecialActionCode = "4";
                    break;
                case MamanSpecialCode.PrintDocuments:
                    mamanSpecialActionCode = "5";
                    break;
                case MamanSpecialCode.Sban:
                    mamanSpecialActionCode = "6";
                    break;

            }
            return new ECSpclMamanMessage()
            {
                ActionCode = mamanActionCodeUpdateOrCancel,
                BaldarAwb = _DeclarationPM.CourierHAWB ?? "",
                BaldarHp = _DeclarationPM.AgentId ?? "",
                OpenBaldarAwbDate = CourierGWMessageECTHRDataMamanRequestService.GetOpenBaldarAwbDate(this._DeclarationPM),
                SpSpclCode = mamanSpecialActionCode ?? "",
                SpLabel1 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText1 ?? "" : "",
                SpLabel2 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText2 ?? "" : "",
                SpLabel3 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText3 ?? "" : "",
                SpLabel4 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText4 ?? "" : "",
                SpLabel5 = mamanSpecialActionCode == "4" ? pmDeclarationMamanSpecialAction.MamanLabelText5 ?? "" : "",


            };
        }
    }
    public class ECSpclMamanMessage
    {
        public string ActionCode { get; set; }
        public string BaldarAwb { get; set; }
        public string BaldarHp { get; set; }
        public DateTime OpenBaldarAwbDate { get; set; }
        public string SpSpclCode { get; set; }
        
        public string SpLabel1 { get; set; }
        public string SpLabel2 { get; set; }
        public string SpLabel3 { get; set; }
        public string SpLabel4 { get; set; }
        public string SpLabel5 { get; set; }



        public int ResponseStatusCode { get; set; }
        public string ResponseStatusMsg { get; set; }
    }
}
