using Unifreight.BL;


using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;


namespace Logitude.CustomsMessaging.UnifreightGateway
{
    public class UGEnvironmentService: UnifreightGatewayProxy
    {
       
        //public enum MethodsEnum
        //{
        //    //None,
        //    InsertImportDeclaration
        //}


        public UGEnvironmentService()
            :base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "ImportDeclarationService";
            true 
            )

        {
            //base.UniVersion = "1.000.000001";
            //base.UniDescription = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;// "ImportDeclarationService";
            //UniProduction = false;
          
        }

   

        
        public override void ProccessRequest(
            string DataIn1,
            string DataIn2,
            out string DataOut1,
            out string DataOut2,
            out string SUCCESS,
            ref string MoreParams,
            out string MessageOut
            )
        {
            DataOut1 = DataOut2 = MessageOut = "";
            SUCCESS = false.ToString();
            try
            {
                AppendLogLine("UGEnvironmentService.ProccessRequest");
                AppendLogLine("Deserialize(DataIn1) ..");
                //Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO.DBTester.test();
                //Logitude.CustomsMessaging.Testers.SendUnifreightCustomInterfaceTester.Tester();
                ///new Logitude.CustomsMessaging.Helpers.SystemTables().GetTableData(TableID: "2009", sendMehesTable2Amital: true);
                ///
                if (true
                    )
                {
                    YuvalTester();
                    DataOut1 = "done";
                    return;
                }

                switch (DataIn1)
                {
                    case   "Logitude.CustomsMessaging.Utils.LogMessagingUtil.ToggleLogMessaging()" :
                        DataOut1 = Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToggleLogMessaging().ToString(); 
                        break;
                    case "Logitude.CustomsMessaging.Utils.LogMessagingUtil.LastMessagingLog.ToString()":
                        DataOut1 = Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(); 
                        break;

                    default:
                        DataOut1 = GetExampleDataIn1();
                        break;
                }
                

                
                //DF_NG_2754_MSG10004_ImportDeclarationResponse

               

            }
            catch (Exception e)
            {

                throw;
            }
        }

        private void YuvalTester()
        {
            //var MN_MSG1_MANIFESTRequestService = new MN_MSG1_MANIFESTRequestService();
        }

        public static void ItzikTester()
        {
            try
            {

                using (var amitalContext = Unifreight.Data.AmitalModel.AmitalContext.GetContext(208))
                {
                    
                    var dualQueryService = new DualQueryService(amitalContext);
                    dualQueryService.GetServerDateTime();
                    dualQueryService.GetServerDateTime();
                    var myGGGQUpdateService = new GGGQUpdateService(amitalContext);
                    var newQpm = new GGGQPM()
                    {
                         ChangeSetOp= Simplog.Server.Infrastructure.ChangeSetOperation.Insert ,
                        ORIGINQUE = "LR",//LugitudeRequest
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        TRY = 3,
                        PRIORITY = 8,
                        ENTNAME = "ISPSPED",
                        PRIMARYNUM = "A00000995",
                        FORMID = "LR_UPDATE_CCU",
                        DEBUG = "F"
                    };
                    myGGGQUpdateService.Update(newQpm, false);

                    var taskPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "1",
                        REQUESTDATA = new string('x', 3000),
                        ENTNAME = newQpm.ENTNAME,
                        PRIMARYNUM = newQpm.PRIMARYNUM,
                        PRIORITY = 1,
                        TYPE = "S",
                        ARCHIVE = "F" // moran 28.6.16 - AMI-57170
                    };
                    var myYCULTASKus = new YCULTASKUpdateService(amitalContext);
                    myYCULTASKus.Update(taskPM, true);

                    //var myGGGQRepository = new GGGQRepository(amitalContext);
                    //var newQ = new GGGQ()
                    //{
                    //    QUEID = CommCounterUtil.GetUnique30(),

                    //    CREATEDATE = DateTime.Now,
                    //    ORIGINQUE = "LR",//LugitudeRequest
                    //    USERID = "SYSTEM",
                    //    DONEOPERATION = "D",
                    //    STATUS = "1",
                    //    EXPTASKTIME = 5,
                    //    TRY = 3,
                    //    PRIORITY = 8,
                    //    ENTNAME = "ISPSPED",
                    //    PRIMARYNUM = "A00000995",
                    //    PROCESSID = System.Diagnostics.Process.GetCurrentProcess().Id,
                    //    FORMID = "LR_UPDATE_CCU",
                    //    COMPUTERID = Environment.MachineName,
                    //    QUEUEMANAGEMENT = false,
                    //    OTHERASNFILE = false,
                    //    STOPPEDBYSM = false,
                    //    DEBUG = "F",
                    //    WEAKREF = "F",
                    //    HUGERECORD = "F",

                    //};
                    
                    //var myYCULTASK = new YCULTASK();
                    //var newQPM = new Unifreight.BL.EntityPMs.YCULTASKPM()
                    //{

                    //    TASKID = myYCULTASK.TASKID,
                    //    ENTNAME = myYCULTASK.ENTNAME,
                    //    PRIMARYNUM = myYCULTASK.PRIMARYNUM,
                    //    TYPE = myYCULTASK.TYPE,
                    //    LOGTIME = myYCULTASK.LOGTIME,
                    //    PRIORITY = myYCULTASK.PRIORITY,
                    //    PROCESSSTARTTIME = myYCULTASK.PROCESSSTARTTIME,
                    //    PROCESSENDTIME = myYCULTASK.PROCESSENDTIME,
                    //    ARCHIVE = myYCULTASK.ARCHIVE,
                    //    STATUS = myYCULTASK.STATUS,
                    //    REQUESTDATA = myYCULTASK.REQUESTDATA,
                    //    RESPONSE = myYCULTASK.RESPONSE,

                    //};
                    //myGGGQRepository.Add(newQ);


                    

                    ///amitalContext.Attach(file);  

                    
                    //amitalContext.SaveChanges(); 
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MoranTester()
        {
            var messageService = new D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService();
            messageService.Send(new Logitude.CustomsMessaging.Common.RequestParams.D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam() { DocumentsFilingId = "1-1" });
        }

        
 
 

        public override string GetAssemblyQualifiedName()
        {
            return this.GetType().Name;
            //return this.GetType().AssemblyQualifiedName;
            //"UnifreightGatewayServer.BL.TaskYam.LogIn.TYLoginService, UnifreightGatewayServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        public override string GetExampleDataIn1()
        {

            return @"use ...
Logitude.CustomsMessaging.Utils.LogMessagingUtil.ToggleLogMessaging()
or 
Logitude.CustomsMessaging.Utils.LogMessagingUtil.LastMessagingLog.ToString()

and To Be Continue"; 
        }

        public override string GetExampleDataIn2()
        {

            

            
            return "";
        }

        public override string GetExampleDataout1()
        {

            return "1-9"; //new entity  ...import dec.
        }
        public override string GetExampleDataout2()
        {

            return "";
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Members

        //public   void Dispose()
        //{


        //}
        public override void Dispose()
        {
            
        }
        #endregion

        public static string ENTNAME { get; set; }

        public static string PRIMARYNUM { get; set; }
    }
  
 
}
