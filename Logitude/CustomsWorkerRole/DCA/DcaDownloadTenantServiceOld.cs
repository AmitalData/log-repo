
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
using Logitude.Customs.Def.EntityPMs;
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
using Logitude.Customs.Data.EntityLists;
using System.Diagnostics;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.BL.Utils;
using Simplog.Server.Infrastructure.Helpers;


namespace CustomsWorkerRole.DCA
{
    public /*Test outside from C:\Users\itzik\Documents\Visual Studio 2012\Projects\CustomsWorkerRoleWindowsFormsApplication\CustomsWorkerRoleWindowsFormsApplication */
        class DcaDownloadTenantServiceOld
    {
        //private readonly string _PartnerID;
        //private readonly string _UnifreightEnvironmentID;

        //private readonly string _DownloadMoreParams;
        private readonly string _AppendToDownloadFolderName;
        private readonly bool _EnableLog;
        private List<InterfaceTenantDefinitionManagementPM> _AllInterface;
        private CustomsSettingPM _CustomsSettingPM;

        static List<LastAccessFileInDCADirM> _LastAccessFileInDCADirList = new List<LastAccessFileInDCADirM>();

        private LastAccessFileInDCADirM _MyLastAccessFileInDCADirM;
        private IEnumerable<InterfaceTenantDefinitionManagementPM> _InterfaceListDCA;


        public DcaDownloadTenantServiceOld(CustomsSettingPM customsSettingPM)
        {
            // TODO: Complete member initialization
            this._CustomsSettingPM = customsSettingPM;
            //_CustomsDeploymentStage = CustomsSettingUtil.GetCustomsDeploymentStage(customsSettingPM.Tenant);
            _MyLastAccessFileInDCADirM = _LastAccessFileInDCADirList.FirstOrDefault(rec => rec.Tenant == _CustomsSettingPM.Tenant);
            if (_MyLastAccessFileInDCADirM == null)
            {
                _MyLastAccessFileInDCADirM = new LastAccessFileInDCADirM(_CustomsSettingPM.Tenant);
                _LastAccessFileInDCADirList.Add(_MyLastAccessFileInDCADirM);
            }
            else
            {
                if (DateTime.Now.Subtract(_MyLastAccessFileInDCADirM.ObjectCreatedAt) > TimeSpan.FromHours(1))
                {
                    _LastAccessFileInDCADirList.Remove(_MyLastAccessFileInDCADirM);
                    _MyLastAccessFileInDCADirM = new LastAccessFileInDCADirM(_CustomsSettingPM.Tenant);
                    _LastAccessFileInDCADirList.Add(_MyLastAccessFileInDCADirM);
                }
            }
            _EnableLog = true;
            var interfaceTypeQueryService = new InterfaceTenantDefinitionQueryService(_CustomsSettingPM.Tenant);
            //interfaceTypeQueryService.GetInterfaceManagementwithDefinition(_CustomsSettingPM.Tenant);
            ////var interfaceTenantDefinitionQueryService = new InterfaceTenantDefinitionQueryService(_CustomsSettingPM.Tenant);

            _AllInterface = interfaceTypeQueryService.GetWithInterfaceManagementDefinition(_CustomsSettingPM.Tenant);

            _InterfaceListDCA = //(new IIGMessageQueryService()).GetAll().Where(mess => mess.Interactive.HasFlag(InterfaceType.InteractiveMode.DCA)); ;
                   _AllInterface.Where(rec =>
                       //rec.InterfaceManagement.INOUT ==  Logitude.Customs.BL.ClosedTable.InOutType.In  &&
                       //!string.IsNullOrWhiteSpace(rec.InterfaceManagement.DcaPrefixName) && 
                       //!rec.OverrideInActive &&
                       ///////rec.Interactive == Logitude.Customs.BL.ClosedTable.InteractiveMode.DCABatchIn &&
                       !String.IsNullOrWhiteSpace(
                       rec.InterfaceManagement.DcaPrefixName +
                       rec.InterfaceManagement.DcaPrefixName2 +
                       rec.InterfaceManagement.DcaPrefixName3 +
                       rec.InterfaceManagement.DcaPrefixName4)
                       );
        }


        public void DownloadAll(string debugIIGMessageId)
        {


            


                var suppressFeature = false;
                if (!suppressFeature)
                {
                    if (IsFolderNotChange())
                    {
                        return;
                    }    
                }
                
                

                
                //interfaceTenantDefinitionQueryService.GetAll();
                //            interfaceList = interfaceList.Where(rec => rec.SendOptionsCode == "DI" && rec.InOut = "I");


               
                if (!string.IsNullOrWhiteSpace(debugIIGMessageId))
                {

                    _InterfaceListDCA = _InterfaceListDCA.Where(mess => mess.Code == debugIIGMessageId).ToList();
                }


                foreach (var messageDCA in _InterfaceListDCA)
                {
                    //if (!DoDCAEnviroment(messageDCA))
                    //{
                    //    Thread.Sleep(TimeSpan.FromSeconds(10));
                    //}

                    var currMessagingService = GetMainMessagingService(messageDCA);
                    if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(currMessagingService))
                    {

                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug("currMessagingService : " + currMessagingService + " Is not Registered in ContainerAccessor.Container,    Due infinite errors i cancel writing log"); 
                        continue;
                        //ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                        throw new Exception("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                        //return;
                    }
                    bool featureSuffixOutCanBeIn = true;
                    var allDcaPrefixNameInCurrentInterfaceManagement = new HashSet<string>();


                    AddDacPreFixName(allDcaPrefixNameInCurrentInterfaceManagement, messageDCA.InterfaceManagement.DcaPrefixName, featureSuffixOutCanBeIn);
                    AddDacPreFixName(allDcaPrefixNameInCurrentInterfaceManagement, messageDCA.InterfaceManagement.DcaPrefixName2, featureSuffixOutCanBeIn);
                    AddDacPreFixName(allDcaPrefixNameInCurrentInterfaceManagement, messageDCA.InterfaceManagement.DcaPrefixName3, featureSuffixOutCanBeIn);
                    AddDacPreFixName(allDcaPrefixNameInCurrentInterfaceManagement, messageDCA.InterfaceManagement.DcaPrefixName4, featureSuffixOutCanBeIn);
                    if (allDcaPrefixNameInCurrentInterfaceManagement.Count() < 1)
                    {
                        continue;
                    }

                    foreach (var dcaPrefixName in allDcaPrefixNameInCurrentInterfaceManagement)
                    {
                        if (!DownloadCurrentInterface(messageDCA, dcaPrefixName))
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(3));
                        }    
                    }
                    
                }


            

        }

        private bool IsFolderNotChange()
        {
            DateTime? ServerDirLastChangeAt = null;
            bool myErrorOccurred = false;
            try
            {

                string myMessageOut = "";
                var myDcaManager = new DcaManager(_CustomsSettingPM.DCAServiceAddress, _CustomsSettingPM.DCAPartnerVault, _CustomsSettingPM.Tenant);
                var myMoreParams = "";// _DownloadMoreParams;

                ServerDirLastChangeAt = myDcaManager.LogitudeCalcDirLastChangeAt(
                    //this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant), 
                    _AppendToDownloadFolderName,
                    ref myMoreParams,
                    out myErrorOccurred,
                    out myMessageOut);

                if (!myErrorOccurred)
                {

                    if (_MyLastAccessFileInDCADirM.LastAccessFileInDCADir == ServerDirLastChangeAt)
                    {
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug("_MyLastAccessFileInDCADirM.LastAccessFileInDCADir == ServerDirLastChangeAt ==> Nothing to do FolderNotChange !! for this tenant ;");
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        return true;
                    }

                }
                return false;
            }
            finally
            {
                _MyLastAccessFileInDCADirM.ErrorOccurred = myErrorOccurred;
                _MyLastAccessFileInDCADirM.LastAccessFileInDCADir = ServerDirLastChangeAt;
            }
        }

        private string GetMainMessagingService(InterfaceTenantDefinitionManagementPM messageDCA)
        {
            var currMessagingService = messageDCA.Code;


            var mainOutInterface = _AllInterface.FirstOrDefault(rec =>
                rec.InterfaceManagement.ResponseInterfaceCode == messageDCA.InterfaceManagement.Code);
            if (mainOutInterface != null)
            {
                currMessagingService = mainOutInterface.Code;
            }
            else
            {

            }
            return currMessagingService;
        }



        private bool DownloadCurrentInterface(InterfaceTenantDefinitionManagementPM messageDCA, string dcaPrefixName)
        {



            bool myErrorOccurred;
            string myMoreParams = "";
            string myMessageOut;







            var searchPattren = dcaPrefixName + "*" + CustomsSettingUtil.GetSufix(_CustomsSettingPM.Tenant);
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(string.Format("searchPattren = {0} _CustomsSettingPM.Tenant = {1} ", searchPattren, _CustomsSettingPM.Tenant));
            //searchPattren = "";
            var myDcaManager = new DcaManager(_CustomsSettingPM.DCAServiceAddress, _CustomsSettingPM.DCAPartnerVault, _CustomsSettingPM.Tenant);
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
                LogMessagingUtil.Instance.Clear();


            }
            return true;
        }

        private static void AddDacPreFixName(HashSet<string> allDcaPrefixName, string dcaPrefixName, bool featureSuffixOutCanBeIn)
        {
            if (!String.IsNullOrWhiteSpace(dcaPrefixName))
            {
                if (featureSuffixOutCanBeIn)
                {
                    dcaPrefixName = SufixRemove_Out(dcaPrefixName);
                }
                if (!string.IsNullOrWhiteSpace(dcaPrefixName))
                {
                    allDcaPrefixName.Add(dcaPrefixName);
                }

            }
            
        }

        private static string SufixRemove_Out(string dcaPrefixName)
        {
            if (dcaPrefixName.EndsWith("_out.", StringComparison.OrdinalIgnoreCase))
            {

                dcaPrefixName = dcaPrefixName.Substring(0, dcaPrefixName.Length - 5);
            }
            
            return dcaPrefixName;
        }

   
        private bool DoDcaMessageFile(InterfaceTenantDefinitionManagementPM messageDCA, DcaManager myDcaManager, string selectedFile)
        {
            bool myErrorOccurred;
            string myMoreParams = "";
            string myMessageOut;

            string fileContentsBASE64 = "";
            string fileContents = "";
            myMoreParams = "";// _DownloadMoreParams;


            try
            {
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
                

                SaveRequestSheet(messageDCA, selectedFile, fileContentsBASE64
                    //messageBytes
                );

            }
            catch (DbEntityValidationException ex)
            {
                var log = LogMessagingUtil.Instance.ToString();
                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                //_sbGatewayLog.Insert(0, "ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                //Debug.WriteLine("ProccessRequest():Exception " + FormatedException.ToString(), true);
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role DbEntityValidationException", null, null);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role", null, null);
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

        private void SaveRequestSheet(InterfaceTenantDefinitionManagementPM messageDCA, string selectedFile, string fileContentsBASE64
            //byte[] messageBytes
                )
        {



            string currMessagingService = GetMainMessagingService(messageDCA);
            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(currMessagingService))
            {

               NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Due infinite errors i cancel writing log"); return;
                //ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                throw new Exception("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                //return;
            }

            
            AmitalDebuggerUtil.Break();

            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(currMessagingService);
            var fileContents = UnifreightIIG.Common.Utils.Base64Util.FromBase64_Decode(fileContentsBASE64, false);

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                //Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.Clear();
                var selectedDcaFile =DCAFilePraser.GetDCAFileModel( selectedFile);
                anaO.DcaReceivedCustomResponseCorrelation(messageDCA.InterfaceManagement, _CustomsSettingPM.Tenant, selectedDcaFile, fileContents);
                scope.Complete();
            }
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




        internal void TestDownloadFile(string dcaFile, 
            //byte[] bytsDcaFile
            string base64StringInnerUTF8
            )
        {
            InterfaceTenantDefinitionManagementPM messageDCA = _InterfaceListDCA.FirstOrDefault(
                rec => IsStart(rec.InterfaceManagement.DcaPrefixName, dcaFile) ||
                    IsStart(rec.InterfaceManagement.DcaPrefixName2, dcaFile) ||
                    IsStart(rec.InterfaceManagement.DcaPrefixName3, dcaFile) ||
                    IsStart(rec.InterfaceManagement.DcaPrefixName4, dcaFile)
                    );
            if (messageDCA==null)
            {
                throw new Exception("dcaFile=" + dcaFile +" Not found in InterfaceManagement.DcaPrefixName* ");
            }
        //    var base64String =
        //System.Convert.ToBase64String(bytsDcaFile,
        //                       0,
        //                       bytsDcaFile.Length);
             this.SaveRequestSheet(messageDCA, dcaFile, 
                 //base64String
                 base64StringInnerUTF8
                 );
        }

        private static bool IsStart(string curDcaPrefixName, string dcaFile)
        {
            if (!String.IsNullOrWhiteSpace(curDcaPrefixName))
            {
                if (dcaFile.StartsWith(curDcaPrefixName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
                         
        }
    }
}
