using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;
using System.Data.Entity.Validation;
using System.Transactions;
using Logitude.Server.Tools.ExternalServices;
using Logitude.Server.Tools;
using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;

namespace CustomsWorkerRole.DCA
{
#if false
    

    public /*Test outside from C:\Users\itzik\Documents\Visual Studio 2012\Projects\CustomsWorkerRoleWindowsFormsApplication\CustomsWorkerRoleWindowsFormsApplication */
        class DcaService
    {
        //private readonly string _PartnerID;
        //private readonly string _UnifreightEnvironmentID;
        //private readonly string _Tenant;
        //private readonly string _DownloadMoreParams;
        private readonly string _AppendToDownloadFolderName;
        private readonly bool _EnableLog;

        public DcaService()
        {
            //_PartnerID = "IIG";---
                
            
            //_Tenant = "1";
            //<add key="tenant_1_alias_vault" value="dev64bit|amitestm53|amitestm53"/>
            //_UnifreightEnvironmentID = "tenant_";
            //_DownloadMoreParams="";


            _EnableLog = true;
        }
        

        public void DownloadAll(string debugIIGMessageId)
        {

            var messageListDCA = (new IIGMessageQueryService()).GetAll().Where(mess => mess.Interactive.HasFlag(IIGMessagePM.InteractiveMode.DCA)); ;
            if (!string.IsNullOrWhiteSpace(debugIIGMessageId))
            {

                messageListDCA = messageListDCA.Where(mess => mess.Id == debugIIGMessageId).ToList();
            }
            foreach (var messageDCA in messageListDCA)
            {
                if (!DoDcaMessageInAllEnviroment(messageDCA))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }




        }
        private bool Valid(IIGMessagePM messageDCA)
        {
            if (String.IsNullOrWhiteSpace(messageDCA.MalamClass))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(messageDCA.MalamClass))
            {
                return false;
            }
            return true;

        }
        private bool DoDcaMessageInAllEnviroment(IIGMessagePM messageDCA)
        {

            if (!Valid(messageDCA))
            {
                if (_EnableLog)
                {
                    //LogMessage(TRequestParams requestParams, string subject, string InOut, out string communicationLogId)
                }
                return false;
            }


            var customsDeploymentStage = Simplog.Server.Infrastructure.Helpers.SettingUtil.GetCustomsDeploymentStage();
            //foreach (IIGMessagePM.EnvironmentType environmentValue in Enum.GetValues(messageDCA.Environment.GetType()))
            IIGMessagePM.EnvironmentType environmentValue = IIGMessagePM.EnvironmentType.none;
            switch (customsDeploymentStage)
            {
                case Simplog.Server.Infrastructure.Helpers.SettingUtil.CustomsDeploymentStage.None:
                    break;
                case Simplog.Server.Infrastructure.Helpers.SettingUtil.CustomsDeploymentStage.Test:
                    environmentValue = IIGMessagePM.EnvironmentType.Test;
                    break;
                case Simplog.Server.Infrastructure.Helpers.SettingUtil.CustomsDeploymentStage.Pilot:
                    environmentValue = IIGMessagePM.EnvironmentType.Pilot;
                    break;
                case Simplog.Server.Infrastructure.Helpers.SettingUtil.CustomsDeploymentStage.Production:
                    environmentValue = IIGMessagePM.EnvironmentType.Production;
                    break;
                default:
                    break;
            }



            if (!messageDCA.Environment.HasFlag(environmentValue))
            {
                return true; //continue;
            }

            return DoDCAEnviroment(messageDCA, environmentValue);



            return true;
        }

        private bool DoDCAEnviroment(IIGMessagePM messageDCA, IIGMessagePM.EnvironmentType environmentValue)
        {

            bool myErrorOccurred;
            string myMoreParams = "";
            string myMessageOut;
            if (String.IsNullOrWhiteSpace(messageDCA.PrefixFileName ))
            {
                messageDCA.PrefixFileName = messageDCA.MalamClass;
                if (String.IsNullOrWhiteSpace(messageDCA.PrefixFileName))
                {
                    throw new Exception("PrefixFileName is must");
                }
            }
            var searchPattren = messageDCA.PrefixFileName + "*" + messageDCA.ToSufix(environmentValue);
            //searchPattren = "";
            var myDcaManager = new DcaManager(messageDCA.DCAServiceAddress,"IIG",messageDCA.Tenant );
            myMoreParams = "";// _DownloadMoreParams;
            var myFileListing = myDcaManager.FileListing(
                //this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant), 
                searchPattren, _AppendToDownloadFolderName,
                ref myMoreParams,
                out myErrorOccurred,
                out myMessageOut);
            if (myErrorOccurred)
            {
                if (_EnableLog)
                {
                    //LogMessage(TRequestParams requestParams, string subject, string InOut, out string communicationLogId)
                }
                return false;
            }
            foreach (var selectedFile in myFileListing)
            {

                DoDcaMessageFile(messageDCA, myDcaManager, selectedFile);


            }
            return true;
        }
        private bool DoDcaMessageFile(IIGMessagePM messageDCA, DcaManager myDcaManager, string selectedFile)
        {
            bool myErrorOccurred;
            string myMoreParams = "";
            string myMessageOut;

            string fileContentsBASE64 = "";
            string fileContents = "";
            myMoreParams = "";// _DownloadMoreParams;
            fileContentsBASE64 = myDcaManager.GetContentsBASE64OfDownloadIncomeFile(
                //this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant),
                selectedFile, this._AppendToDownloadFolderName,
                //out FileName, out FileContentsBASE64,
                ref myMoreParams,
                out myErrorOccurred, out myMessageOut);
            if (myErrorOccurred)
            {
                if (_EnableLog)
                {
                    //LogMessage(TRequestParams requestParams, string subject, string InOut, out string communicationLogId)
                }
                return false;
            }
            fileContents = UnifreightIIG.Common.Utils.Base64Util.FromBase64_Decode(fileContentsBASE64, false);
            var myXDocument = System.Xml.Linq.XDocument.Parse(fileContents);

#if false
            var myImporterService = _UnityContainer.Resolve<IImporterService>(messageDCA.ImporterService);
            myImporterService.LoadXML(fileContents);
            myImporterService.Validate();
            
#endif
            try
            {

#if true
                byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(fileContents);
                SaveMessageToAnalyzeQueue(messageDCA, selectedFile, messageBytes);
#else                    
                BrokeredMessage currentMessage = new BrokeredMessage(
                    //new MemoryStream(data)
                    new MemoryStream(UTF8Encoding.Default.GetBytes(fileContents))
                    , true);
                //currentMessage.Properties["Tenant"] = _Tenant;
                //currentMessage.Properties["FileName"] = selectedFile;
                //currentMessage.Properties["IIGMessagePM "] = messageDCA;
                string queueName = WebFreightEntryPoint.GetQueueByEnviroment(CreateAnalyzeQueueWR.QueueName);
                QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(queueName);
                client.Send(currentMessage);
#endif

            }
            catch (DbEntityValidationException ex)
            {
                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                //_sbGatewayLog.Insert(0, "ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                //Debug.WriteLine("ProccessRequest():Exception " + FormatedException.ToString(), true);
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role DbEntityValidationException", null);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role", null,null);
                return false;
            }

            myMoreParams = "";// _DownloadMoreParams;
            myDcaManager.DeleteIncomeFile(//this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant),
                selectedFile, this._AppendToDownloadFolderName,
                  ref myMoreParams,
                out myErrorOccurred, out myMessageOut);

            if (myErrorOccurred)
            {
                if (_EnableLog)
                {
                    //LogMessage(TRequestParams requestParams, string subject, string InOut, out string communicationLogId)
                }
                return false;
            }
            //PushToQueue();
            return true;
        }

  

        private void OpenNewComm()
        {
#if false
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Connecting To Entity  : " + analyzeQueue.ObjectTableName + analyzeQueue.EntityReference);
            var curCommunicationsParams = new Logitude.Server.Tools.CommunicationsParams()
            {
                Tenant = messageAnalyzerService.ResolveTenant(),

                LoggingObjectTableId = messageAnalyzerService.GetLoggingObjectTableId(),// objectTable.Id,
                LoggingEntityId = messageAnalyzerService.GetLoggingEntityId(),//_PhysicalCheckPM.Id,
                LoggingEntityReference = messageAnalyzerService.GetLoggingEntityReference(),//_PhysicalCheckPM.CheckId,

                Subject = messageAnalyzerService.GetSubject(),// "FU Status",

                //LoggingUserId = ResolveLoggingUserId(),//loggingUserId,
                Logs = Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(7950),
                ByteData = analyzeQueue.MessageBody,
                Status = "D",
                To = "Logitude",
                CommunicationLogTypeCode = "T",
                FolderName = "IIGDCA",
                From = "IIGDCA",
                InOut = "I",
            };
            analyzeQueue.CommunicationLogId = Logitude.Server.Tools.Communications.AddCommunicationLog(curCommunicationsParams);
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("Communication opened : " + analyzeQueue.CommunicationLogId);
            
#endif
        }

        private string GetExternalId(string selectedFile)
        {
            /*
        In 

        Customs Push
        \\GK-UNISVC1\CyberArk_DCA\amital_shipping\Download\ranar\UDCAServerHistory\SendMN_MSG1171_SendManifestFeedBack_Message_Out.IL941079089.2014-06-15_12-46-40-871.a60c718f-3d68-4d15-9b9a-6043dabb7574.PRD.xml.zip


        Return after our Req
        "\\GK-UNISVC1\CyberArk_DCA\amital_shipping\Download\ranar\UDCAServerHistory\SaveMN_MSG1170_1171_MANIFESTRequest_Out.IL941079089.2014-06-15_09-22-58-890.20140615083441612924803021008.PRD.xml.zip"

         */ 


            //\\dev2008\CyberArk_DCA\dev64bit_amitestm53\Download\IIG\GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST.xml
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(selectedFile);//GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST.xml

            fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);///GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60.TST
            ///GetSYSTBL_MSG9000_9001_SystemTableRequest_Out.IL941079089.2014-06-15_17-51-26-314.653bc69d-31e4-4e47-b973-28bd2cd8fe60
            var extension = Path.GetExtension(fileNameWithoutExtension);
            //.653bc69d-31e4-4e47-b973-28bd2cd8fe60
            extension = extension.Substring(1);//remove dot 
            return extension;
        }
        private void SaveMessageToAnalyzeQueue(IIGMessagePM messageDCA, string selectedFile, byte[] messageBytes)
        {
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            //byte[] messageBytes = Encoding.ASCII.GetBytes(messageData);
            //string mmm = Encoding.ASCII.GetString(messageBytes);
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "Customs",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = messageBytes.Length,

            };
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
            Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("DcaService  analyzeQueue add " + analyzeQueue.Id);
        }
    }
#endif
}
