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
    
    public enum MamanActionCode
    {
        Upsert,
        Cancel
    }
    public enum MamanSpecialCode
    {
        /// <summary>
        /// קליטת עיכוב (ללא ששודרה קודם השהיה)
        /// </summary>
        DelayIt = 2,
        PrintLabels=4,
        PrintDocuments=5



    }
    class CourierGWMessageECSpclMamanRequestService
    {
        private DeclarationPM _DeclarationPM;
        private CourierMasterPM _CourierMasterPM;
        

        public static string TestSend()
        {
            var courierGWMessageECSpclMamanRequestService =new CourierGWMessageECSpclMamanRequestService();
            string actionResultString =courierGWMessageECSpclMamanRequestService.BuildQueueSendWebAPI("1-222", 1, MamanActionCode.Upsert, MamanSpecialCode.DelayIt);
            return actionResultString;
        }

        public string BuildQueueSendWebAPI(
            string declarationId, int tenant,
            MamanActionCode mamanActionCode,
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
            var pmDeclarationMamanSpecialAction =declarationMamanSpecialActionQueryService.GetSingle(tenant, declarationId,  ((int)mamanSpecialCode).ToString());

#endif

            ECSpclMamanMessage myECSpclMamanData = CreateCourierECSpclMamanMessage(mamanActionCode,mamanSpecialCode);
            string messageToMaman = "";
            messageToMaman = ProxyUtil.JsonConvertSerialize(myECSpclMamanData);
            using (var scop = TransactionFactory.GetTransaction())
            {
                byte[] bytearray = Encoding.UTF8.GetBytes(messageToMaman);


                //var myWebAPICourierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanResponseService();
                //myWebAPICourierGWMessageECTHRDataMamanService.BuildCommunicationLog(bytearray, tenant, declarationId);

                var webAPISendMessage2MamanService = new WebAPISendMessage2MamanService();
                webAPISendMessage2MamanService.BuildCommunicationLog(bytearray, tenant, declarationId, CustomsPartnerFtpDetails.InterfaceName_ECSPCL);

                scop.Complete();
                //output  ftp://192.168.10.88/FTP_MAMAN/  
            }
            return "המסר נבנה בהצלחה וישלח בתהליך רקע";
        }

        private ECSpclMamanMessage CreateCourierECSpclMamanMessage(
            MamanActionCode mamanActionCode,
            MamanSpecialCode mamanSpecialCode)
        {
            string actionCode = "";
            switch (mamanActionCode)
            {
                case MamanActionCode.Upsert:
                    actionCode = "C";
                    break;
                case MamanActionCode.Cancel:
                    actionCode = "U";
                    break;
                
            }
            string SpecialCode = "";
            switch (mamanSpecialCode)
            {
                case MamanSpecialCode.DelayIt:
                    SpecialCode = "2";
                    break;
                case MamanSpecialCode.PrintLabels:
                    SpecialCode = "4";
                    break;
                case MamanSpecialCode.PrintDocuments:
                    SpecialCode = "5";
                    break;
             
            }
            return new ECSpclMamanMessage()
            {
                ActionCode = actionCode,
                BaldarAwb = _DeclarationPM.CourierHAWB,
                BaldarHp = _DeclarationPM.AgentId,
                OpenBaldarAwbDate = CourierGWMessageECTHRDataMamanRequestService.GetOpenBaldarAwbDate(this._DeclarationPM),
                SpSpclCode= SpecialCode,


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
