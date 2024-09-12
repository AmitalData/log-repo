
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
//using Simplog.Global.Data.GlobalModel.Repositories;
//using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Logitude.CustomsMessaging.Common.DCAParams;
using System.Configuration;
using System.Collections.Concurrent;
using Logitude.Server.Tools.Utils;
using Logitude.BL.Security;
using System.Globalization;

namespace Logitude.CustomsMessaging.Dca
{
    public /*Test outside from C:\Users\itzik\Documents\Visual Studio 2012\Projects\CustomsWorkerRoleWindowsFormsApplication\CustomsWorkerRoleWindowsFormsApplication */
        class DcaDownloadTenantService
    {
        //private readonly string _PartnerID;
        //private readonly string _UnifreightEnvironmentID;

        //private readonly string _DownloadMoreParams;
        private readonly string _AppendToDownloadFolderName;
        private readonly bool _EnableLog;
        //private readonly CustomsDeploymentStage _CustomsDeploymentStage;

        private List<InterfaceTenantDefinitionManagementPM> _AllInterface;
        private CustomsSettingPM _CustomsSettingPM;

        static List<DCAIncomeDirStateM> _LastAccessFileInDCADirList = new List<DCAIncomeDirStateM>();

        private DCAIncomeDirStateM _MyDCAIncomeDirStateM;
        private List<InterfaceTenantDefinitionManagementPM> _InterfaceListDCA;


        public DcaDownloadTenantService(CustomsSettingPM customsSettingPM)
        {
            // TODO: Complete member initialization
            this._CustomsSettingPM = customsSettingPM;
            //_CustomsDeploymentStage = CustomsSettingUtil.GetCustomsDeploymentStage(customsSettingPM.Tenant);
            _MyDCAIncomeDirStateM = _LastAccessFileInDCADirList.FirstOrDefault(rec => rec.Tenant == _CustomsSettingPM.Tenant);
            if (_MyDCAIncomeDirStateM == null)
            {
                _MyDCAIncomeDirStateM = new DCAIncomeDirStateM(_CustomsSettingPM.Tenant);
                _LastAccessFileInDCADirList.Add(_MyDCAIncomeDirStateM);
            }
            else
            {
                if (DateTime.Now.Subtract(_MyDCAIncomeDirStateM.ObjectCreatedAt) > TimeSpan.FromHours(1))
                {
                    _LastAccessFileInDCADirList.Remove(_MyDCAIncomeDirStateM);
                    _MyDCAIncomeDirStateM = new DCAIncomeDirStateM(_CustomsSettingPM.Tenant);
                    _LastAccessFileInDCADirList.Add(_MyDCAIncomeDirStateM);
                }
            }
            _EnableLog = true;
            var interfaceTypeQueryService = new InterfaceTenantDefinitionQueryService(_CustomsSettingPM.Tenant);
            //interfaceTypeQueryService.GetInterfaceManagementwithDefinition(_CustomsSettingPM.Tenant);
            ////var interfaceTenantDefinitionQueryService = new InterfaceTenantDefinitionQueryService(_CustomsSettingPM.Tenant);

            _AllInterface = interfaceTypeQueryService.GetWithInterfaceManagementDefinition(_CustomsSettingPM.Tenant);

#if true
            _InterfaceListDCA=interfaceTypeQueryService.GetInterfaceListDCA(_AllInterface, customsSettingPM.CompanyType);
#else

            _InterfaceListDCA = //(new IIGMessageQueryService()).GetAll().Where(mess => mess.Interactive.HasFlag(InterfaceType.InteractiveMode.DCA)); ;
                   _AllInterface
                   .Where(r => r.OverrideActive == true)

                    //להתייחס לשדה Active מרמת ניהול מסרים
                    .Where(r => r.InterfaceManagement.Active == true)



                    .Where(
                       r =>
                    //INTERFACETYPE
                    //ערכים NULL== הכל, C == רק עמילות, B == רק בלדרות
                    string.IsNullOrWhiteSpace(r.InterfaceManagement.InterfaceType)//All

                    ||
                    (
                    !string.IsNullOrWhiteSpace(r.InterfaceManagement.InterfaceType)
                    &&
                     //COMPANYTYPE שם שדה ערכים C -דיפולטיבי(בסקריפט), או B == בלדרות - אסור ריק יאותחל עם הפצה ראשונה + DEFAULT == C
                     r.InterfaceManagement.InterfaceType == customsSettingPM.CompanyType
                     )
                     )

                   .Where(rec =>
                       //rec.InterfaceManagement.INOUT ==  Logitude.Customs.BL.ClosedTable.InOutType.In  &&
                       //!string.IsNullOrWhiteSpace(rec.InterfaceManagement.DcaPrefixName) && 
                       //!rec.OverrideInActive &&
                       ///////rec.Interactive == Logitude.Customs.BL.ClosedTable.InteractiveMode.DCABatchIn &&
                       !String.IsNullOrWhiteSpace(
                       rec.InterfaceManagement.DcaPrefixName +
                       rec.InterfaceManagement.DcaPrefixName2 +
                       rec.InterfaceManagement.DcaPrefixName3 +
                       rec.InterfaceManagement.DcaPrefixName4)
                       ).ToList();
#endif
            _AllDcaPreFixWithoutInOutUpper = new List<string>();




            if (AddUnifreightTester == true)
            {
                _InterfaceListDCA.Add(interfaceTypeQueryService.GetWithInterfaceManagementDefinition(_CustomsSettingPM.Tenant, "UT01").First());
            }


            var MessagingServiceInterfaceTypeCodes = new List<string>();
            MessagingServiceInterfaceTypeCodes =
                ContainerAccessor.Container.Registrations
                .Where(t => t.RegisteredType == typeof(IMessagingServiceInterfaceType))
                .Select(t => t.Name).ToList();


            bool isExist = false;
            foreach (var rec in _InterfaceListDCA)
            {
                isExist = false;

                if (rec.InterfaceManagement.InOut == "O")
                {
                    if (MessagingServiceInterfaceTypeCodes.Contains(rec.Code))
                    {
                        isExist = true;
                    }
                }
                else if (rec.InterfaceManagement.InOut == "I")
                {
                    if (!MessagingServiceInterfaceTypeCodes.Contains(rec.InterfaceManagement.ResponseInterfaceCode))
                    {
                        isExist = true;
                    }
                }



                if (isExist)
                {

                    var l = GetAllPreFix(rec);
                    if (l.Count() > 0)
                    {
                        _AllDcaPreFixWithoutInOutUpper.AddRange(l);
                    }
                }
                else
                {
                    Debug.WriteLine("Due Not register in Container: Removing " + rec.Code);
                }


            }
        }

        public bool HasFeature_DcaDirect9200()
        {
            //this.IsDcaActive = !(FeatureLocator.HasFeaturePermession("Customs.Declaration", "DCA"));
            string email = AuthenticationUtil.ResolveUserIdentityName(this._CustomsSettingPM.Tenant);
            bool suppressUnifreightDCAServer = SecurityUtility.CheckFeature("Customs.Declaration", "DCA", this._CustomsSettingPM.Tenant);
            Debug.WriteLine($"suppressUnifreightDCAServer={suppressUnifreightDCAServer} ");
            return suppressUnifreightDCAServer;
            //return SecurityUtility.CheckContactFeature("Customs.Declaration", "DCA", this._CustomsSettingPM.Tenant, email);
        }
        private List<string> GetAllPreFix(InterfaceTenantDefinitionManagementPM rec)
        {
            var splitter = new string[] { Environment.NewLine };
            var sb = new StringBuilder();
            sb.AppendLine(RemoveInOutUpper(rec.InterfaceManagement.DcaPrefixName))
                .AppendLine(RemoveInOutUpper(rec.InterfaceManagement.DcaPrefixName2))
                .AppendLine(RemoveInOutUpper(rec.InterfaceManagement.DcaPrefixName3))
                .AppendLine(RemoveInOutUpper(rec.InterfaceManagement.DcaPrefixName4));

            var l = new List<string>(sb.ToString().Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            return l;
        }

        private string RemoveInOutUpper(string DcaPrefixName)
        {
            DcaPrefixName = DcaPrefixName ?? "";
            DcaPrefixName = DcaPrefixName.ToUpper();
            if (DcaPrefixName.EndsWith("_out.", StringComparison.OrdinalIgnoreCase))
            {
                return DcaPrefixName.Substring(0, DcaPrefixName.Length - 5);

            }
            else if (DcaPrefixName.EndsWith("_In.", StringComparison.OrdinalIgnoreCase))
            {
                return DcaPrefixName.Substring(0, DcaPrefixName.Length - 4);
            }
            return DcaPrefixName;
        }

        private static bool IsAppSettingOn(string appSettingKeyValueIsLogUntilDateyyyyMMdd)
        {
            bool IsOn = false;
            DateTime  stopLogAt = DateTime.MinValue;
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings[appSettingKeyValueIsLogUntilDateyyyyMMdd];//"2018062018HD312280.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {


                if(DateTime.TryParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None,
                                                        out stopLogAt))
                {
                    //stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                    //                                        "yyyyMMdd",
                    //                                        CultureInfo.InvariantCulture,
                    //                                        DateTimeStyles.None);
                    IsOn = DateTime.Now <= stopLogAt;

                }


            }
            return IsOn;
        }
        public void DownloadAll(string debugIIGMessageId, DedicatedCourierDCAModel dedicatedCourierDCAModel)
        {
            if (this.HasFeature_DcaDirect9200() || dedicatedCourierDCAModel != null)
            {
                var sb = new StringBuilder();
                var sw = Stopwatch.StartNew();

                if (IsAppSettingOn("SuppressDownloadDCA.UntilDateyyyyMMdd"))
                {
                    Debug.WriteLine("SuppressDownloadDCA.UntilDateyyyyMMdd");
                }
                else
                {
                    var dcaDirect9200TenantService = new DcaDirect9200TenantService(
                        this._CustomsSettingPM,
                        _AllDcaPreFixWithoutInOutUpper,
                        this._InterfaceListDCA,
                        _AllInterface,
                        dedicatedCourierDCAModel
                        );
                    dcaDirect9200TenantService.DownloadAll(/*debugIIGMessageId*/);
                    if (dedicatedCourierDCAModel != null)
                    {
                        var removeOldOrphanedFilesFromBackupService = new RemoveOldOrphanedFilesFromBackupService();
                        removeOldOrphanedFilesFromBackupService.RemoveOldFiles(dedicatedCourierDCAModel.BackupPath);
                    }
                }
                sb.AppendLine($"DownloadAll({this._CustomsSettingPM.Tenant}):took:{sw.Elapsed}");
                sw.Restart();


                var restoreWaitingImportService = new Restore9100.RestoreWaitingImportMessagesService(_CustomsSettingPM, this._InterfaceListDCA);
                restoreWaitingImportService.RestoreWaitingImportSaveInDB();

                sb.AppendLine($"RestoreWaitingImportSaveInDB({this._CustomsSettingPM.Tenant}):took:{sw.Elapsed}");
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug(sb.ToString());
                return;
            }

            var suppressFeature = false;

            if (!CanIStartWork())
            {
                return;
            }


            _swDownAll = Stopwatch.StartNew();
            bool multi = true;
            if (multi)
            {
                Take50_MultiThread(debugIIGMessageId);
            }
            else
            {
                Take50OneByOne(debugIIGMessageId);
            }

            _swDownAll.Stop();
            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("MoveUnUseDCAFilesToDIr")))
            {
                MoveUnUseDCAFilesToDIr();
            }
        }
        static DateTime _LastErrordateTime = DateTime.MinValue;
        private void Take50_MultiThread(string debugIIGMessageId)
        {
            int iMultiThread = 5;
            int _totalDownload = 0;
            List<DCAFileModel> dcaFileList = null;
            //foreach (var dcaFile in ListOfDCAFile)
            while ((dcaFileList =
                GetNextList(debugIIGMessageId, _totalDownload, iMultiThread)) != null)
            {

                ConcurrentQueue<Exception> exceptionQueue = new ConcurrentQueue<Exception>();
                var timeout = 5 * 60 * 1000; // seconds == 5min
                var cts = new CancellationTokenSource();
                try
                {


                    using (var t = new Timer(_ => cts.Cancel(), null, timeout, -1))
                    {


                        Parallel.ForEach(dcaFileList,
                            new ParallelOptions { CancellationToken = cts.Token },
                            dcaFile =>
                            {

                                try
                                {

                                    var currSelectedFileDownload = dcaFile.SelectedFileDownload;
                                    var messageDCA = _InterfaceListDCA
                                        .First(rec =>
                                            IsMatch(currSelectedFileDownload, rec.InterfaceManagement.DcaPrefixName) ||
                                            IsMatch(currSelectedFileDownload, rec.InterfaceManagement.DcaPrefixName2) ||
                                            IsMatch(currSelectedFileDownload, rec.InterfaceManagement.DcaPrefixName3) ||
                                            IsMatch(currSelectedFileDownload, rec.InterfaceManagement.DcaPrefixName4)
                                    );

                                    var currMessagingService = GetMainMessagingService(messageDCA);
                                    if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(currMessagingService))
                                    {

                                        try
                                        {


                                            Debug.WriteLine("currMessagingService : " + currMessagingService + " Is not Registered in ContainerAccessor.Container,    Due infinite errors i cancel writing log");
                                            //_totalDownload--;
                                            if (DateTime.Now.Subtract(_LastErrordateTime) > TimeSpan.FromMinutes(10))
                                            {
                                                _LastErrordateTime = DateTime.Now;
                                                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("NO MAIN Code (response 2754 of 2750 !!!)  currMessagingService : " + currMessagingService + " Is not Registered in ContainerAccessor.Container,    Due infinite errors i cancel writing log");
                                            }
                                            string myMoreParams = "";
                                            bool myErrorOccurred;
                                            string myMessageOut = "";
                                            _DcaManager.DeleteIncomeFile(//this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant),
                   dcaFile.SelectedFileDownload, this._AppendToDownloadFolderName,
                      ref myMoreParams, out myErrorOccurred, out myMessageOut);
                                        }
                                        catch (Exception)
                                        {

                                            //throw;
                                        }

                                        return;


                                    }



                                    bool dcaMessageFileSuccess = DoDcaMessageFile(messageDCA, dcaFile);//exc handler !!
                                    Debug.WriteLine("DoDcaMessageFile:" + dcaFile.SelectedFileDownload + " Elapsed:" + _swDownAll.Elapsed);
                                    LogMessagingUtil.Instance.Clear();
                                }
                                catch (Exception e)
                                {

                                    exceptionQueue.Enqueue(e);
                                }


                            });
                    }



                }

                //catch (OperationCanceledException e)
                catch (Exception eee)
                {

                    NetCommonHelper.Logger.DevLog.Instance.WriteFatal(eee);
                }
                finally
                {

                    try
                    {
                        exceptionQueue.ToList().ForEach(e1 =>
                        {
                            NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e1);
                        });
                    }
                    catch
                    {


                    }

                    cts.Dispose();
                }

            }
        }



        private void Take50OneByOne(string debugIIGMessageId)
        {
            int _totalDownload = 0;
            DCAFileModel dcaFile = null;
            //foreach (var dcaFile in ListOfDCAFile)
            while ((dcaFile = GetNext(debugIIGMessageId, _totalDownload)) != null)
            {


                var currSelectedFileDownload = dcaFile.SelectedFileDownload;
                var messageDCA = _InterfaceListDCA
                    .First(rec =>
                        IsMatch(currSelectedFileDownload, rec.InterfaceManagement.DcaPrefixName) ||
                        IsMatch(currSelectedFileDownload, rec.InterfaceManagement.DcaPrefixName2) ||
                        IsMatch(currSelectedFileDownload, rec.InterfaceManagement.DcaPrefixName3) ||
                        IsMatch(currSelectedFileDownload, rec.InterfaceManagement.DcaPrefixName4)
                );

                var currMessagingService = GetMainMessagingService(messageDCA);
                if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(currMessagingService))
                {


                    Debug.WriteLine("currMessagingService : " + currMessagingService + " Is not Registered in ContainerAccessor.Container,    Due infinite errors i cancel writing log");
                    _totalDownload--;
                    continue;
                    //ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                    throw new Exception("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                    //return;
                }



                DoDcaMessageFile(messageDCA, dcaFile);

                Debug.WriteLine("DoDcaMessageFile:" + dcaFile.SelectedFileDownload + " Elapsed:" + _swDownAll.Elapsed);
                LogMessagingUtil.Instance.Clear();




            }
        }

        private void MoveUnUseDCAFilesToDIr()
        {

            bool myErrorOccurred;

            string myMoreParams = "";
            string myMessageOut;
            List<string> myFileListing = new List<string>();


            string searchPattren = "*.*";
            string ourSufix = CustomsSettingUtil.GetSufix(_CustomsSettingPM.Tenant);
            Debug.WriteLine(string.Format("searchPattren = {0} _CustomsSettingPM.Tenant = {1} ", searchPattren, _CustomsSettingPM.Tenant));
            //searchPattren = "";
            _DcaManager = GetDcaManagr();
            myMoreParams = "";// _DownloadMoreParams;
            myFileListing = _DcaManager.FileListing(
//this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant), 
searchPattren, _AppendToDownloadFolderName,
ref myMoreParams,
out myErrorOccurred,
out myMessageOut);

            if (myErrorOccurred)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

            }
            myFileListing = myFileListing ?? new List<string>();
            ourSufix = ourSufix.ToLower();
            //myFileListing = myFileListing.Where(f => !f.ToLower().EndsWith(ourPrefiix)).ToList();
            foreach (var fileName in myFileListing)
            {
                if (!fileName.ToLower().EndsWith(ourSufix))
                {
                    Debug.WriteLine($"{fileName} NOT  our Sufix  !!!!!!!! ");
                    MoveUnUseDCAFileToDir(fileName);
                }
                else
                {
                    if (!DCAPrefixIsMapped(fileName))
                    {
                        Debug.WriteLine($"{fileName} NOT  Mapped in Prefix !!!!!!!! ");
                        MoveUnUseDCAFileToDir(fileName);

                    }
                    else
                    {
                        Debug.WriteLine($"{fileName} is Mapped in Prefix ");
                    }
                }
            }
        }

        private bool DCAPrefixIsMapped(string fileName)
        {
            var model = DCAFilePraser.GetDCAFileModel(fileName);

            bool res = _AllDcaPreFixWithoutInOutUpper.Contains(model.PrefixWithout_OutOrIn.ToUpper());
            return res;
        }

        private bool MoveUnUseDCAFileToDir(string fileName)
        {
            Debug.WriteLine("MoveUnUseDCAFileToDIr " + fileName);

            string MoveUnUseDCAFilesToDIr = ConfigurationManager.AppSettings.Get("MoveUnUseDCAFilesToDIr");

            using (var myDCAMoveIncomeFileToDirService = new DCAMoveIncomeFileToDirService(_DcaManager))
            {

                return myDCAMoveIncomeFileToDirService.MoveItToDir(fileName, this._AppendToDownloadFolderName, MoveUnUseDCAFilesToDIr);
            }
        }

        private List<DCAFileModel> GetNextList(string debugIIGMessageId, int _totalDownload, int take)
        {
            var allXmlFileInMyBranch = _MyDCAIncomeDirStateM.LastAllXmlFileInMyBranch ?? new List<string>();//GetFileList();
            if (_swDownAll.Elapsed > TimeSpan.FromSeconds(60))
            {
                allXmlFileInMyBranch = _MyDCAIncomeDirStateM.LastAllXmlFileInMyBranch = GetVaultFileListInCustomDeployStage("");
            }
            var ListOfDCAFile = GetListOfDCAFilesOrderByTime(debugIIGMessageId, allXmlFileInMyBranch);

            if (ListOfDCAFile.Count() < 1)
            {
                Debug.WriteLine("After Filter nothing to Download ");
                return null;
            }
            Debug.WriteLine("Start List :" + ListOfDCAFile.Count() + " Elapsed:" + _swDownAll.Elapsed);
            _totalDownload++;

            if (_totalDownload > 50)
            {
                Debug.WriteLine("Download 50 DCA Files Try Next Tenant ");
                return null;
            }

            var myList = ListOfDCAFile.Take(take).ToList();
            myList.ForEach(
                item =>
                {
                    ListOfDCAFile.Remove(item);
                    if (
                    !_MyDCAIncomeDirStateM
                .LastAllXmlFileInMyBranch
                .Remove(item.SelectedFileDownload)
                )
                    {
                        throw new Exception("Unbelievable !?!?!?!");
                    }

                }

                );

            return myList;
        }
        private DCAFileModel GetNext(string debugIIGMessageId, int _totalDownload)
        {
            var allXmlFileInMyBranch = _MyDCAIncomeDirStateM.LastAllXmlFileInMyBranch ?? new List<string>();//GetFileList();
            if (_swDownAll.Elapsed > TimeSpan.FromSeconds(60))
            {
                allXmlFileInMyBranch = _MyDCAIncomeDirStateM.LastAllXmlFileInMyBranch = GetVaultFileListInCustomDeployStage("");
            }
            var ListOfDCAFile = GetListOfDCAFilesOrderByTime(debugIIGMessageId, allXmlFileInMyBranch);

            if (ListOfDCAFile.Count() < 1)
            {
                Debug.WriteLine("After Filter nothing to Download ");
                return null;
            }
            Debug.WriteLine("Start List :" + ListOfDCAFile.Count() + " Elapsed:" + _swDownAll.Elapsed);
            _totalDownload++;

            if (_totalDownload > 50)
            {
                Debug.WriteLine("Dowload 50 DCA Files Try Next Tenant ");
                return null;
            }
            var my1st = ListOfDCAFile.First();
            ListOfDCAFile.Remove(my1st);
            if (!_MyDCAIncomeDirStateM.LastAllXmlFileInMyBranch.Remove(my1st.SelectedFileDownload))
            {
                throw new Exception("Unbelievable !?!?!?!");
            }
            return my1st;
        }

        private List<DCAFileModel> GetListOfDCAFilesOrderByTime(string debugIIGMessageId, List<string> AllXmlFileInMyBranch)
        {



            var ListOfDCAFile = AllXmlFileInMyBranch.Select(file => DCAFilePraser.GetDCAFileModel(file)).ToList();
            ListOfDCAFile = ListOfDCAFile
                .Where(m => m.ParsedSuccessfully.GetValueOrDefault())
                .Where(m => _AllDcaPreFixWithoutInOutUpper.Contains(m.PrefixWithout_OutOrIn.ToUpper()))
                .ToList()
                ;
            ListOfDCAFile = ListOfDCAFile
                .Where(file => _MyDCAIncomeDirStateM.FileMessagesNotBelong2OurEnvironment.Contains(file.SelectedFileDownload) == false)
                .ToList();


            var dcaUtil = new DcaFilterByEnvironmentService();
            var res = dcaUtil.FilterByEnvironmentListOfDCAFile(_CustomsSettingPM.Tenant, ListOfDCAFile);
            ListOfDCAFile = res.ListOfDCAFile;
            




            if (!string.IsNullOrWhiteSpace(debugIIGMessageId))
            {
                var myDebug = _InterfaceListDCA.Where(mess => mess.Code == debugIIGMessageId).First();
                var filterDebug = GetAllPreFix(myDebug);
                ListOfDCAFile = ListOfDCAFile
                    .Where(m => filterDebug.Contains(m.PrefixWithout_OutOrIn.ToUpper()))
                    .ToList();
            }
            ListOfDCAFile = ListOfDCAFile.OrderBy(m => m.TimStamp).ToList();
            return ListOfDCAFile;
        }

        private bool IsMatch(string SelectedFileDownload, string DcaPrefixName)
        {
            if (String.IsNullOrWhiteSpace(DcaPrefixName))
            {
                return false;
            }
            SelectedFileDownload = SelectedFileDownload.ToUpper();
            DcaPrefixName = RemoveInOutUpper(DcaPrefixName);
            var is1 = SelectedFileDownload.StartsWith(DcaPrefixName);
            return is1;

        }







        private bool CanIStartWork()
        {

            DateTime? ServerDirLastChangeAt = null;
            bool myErrorOccurred = false;
            try
            {

                string myMessageOut = "";
                var myDcaManager = GetDcaManagr();// new DcaManager(_CustomsSettingPM.DCAServiceAddress, _CustomsSettingPM.DCAPartnerVault, _CustomsSettingPM.Tenant);
                var myMoreParams = "";// _DownloadMoreParams;

                var CurrentAllXmlFileInMyBranch = GetVaultFileListInCustomDeployStage();

                if (_MyDCAIncomeDirStateM.LastAllXmlFileInMyBranch.SequenceEqual(CurrentAllXmlFileInMyBranch))
                //unchanged
                {
                    Debug.WriteLine("DCADir unchanged");
                    _MyDCAIncomeDirStateM.LastAllXmlFileInMyBranch = CurrentAllXmlFileInMyBranch;
                    if (_MyDCAIncomeDirStateM.Dir1stChangedAt.HasValue)
                    {
                        _MyDCAIncomeDirStateM.Dir1stChangedAt = null;
                        Debug.WriteLine("Last time Dca Dir Changed ,But Now is Equal, Start Work");
                        return true;

                    }
                    if (_MyDCAIncomeDirStateM.NothingChangeCount > 4)
                    {
                        Debug.WriteLine("Nothing Change But 4 Round Pass Let try Again");
                        _MyDCAIncomeDirStateM.NothingChangeCount = 0;
                        return true;
                    }
                    _MyDCAIncomeDirStateM.NothingChangeCount++;
                    Debug.WriteLine("_MyLastAccessFileInDCADirM.LastAllXmlFileInMyBranch == CurrentAllXmlFileInMyBranch ==> Nothing to do FolderNotChange !! for this tenant ;");
                    return false;
                }
                else
                {//
                    Debug.WriteLine("DCADir Changed ");
                    _MyDCAIncomeDirStateM.LastAllXmlFileInMyBranch = CurrentAllXmlFileInMyBranch;
                    if (_MyDCAIncomeDirStateM.Dir1stChangedAt.HasValue)
                    {
                        Debug.WriteLine("DCADir Changed Again !!!");
                        if (DateTime.Now.Subtract(_MyDCAIncomeDirStateM.Dir1stChangedAt.GetValueOrDefault()) > TimeSpan.FromSeconds(30))
                        {
                            Debug.WriteLine("but Past 30 sec From 1stChange , Start Work");
                            _MyDCAIncomeDirStateM.Dir1stChangedAt = null;
                            return true;
                        }
                    }
                }

                _MyDCAIncomeDirStateM.Dir1stChangedAt = _MyDCAIncomeDirStateM.Dir1stChangedAt ?? DateTime.Now;
                return false;
            }
            finally
            {
                _MyDCAIncomeDirStateM.ErrorOccurred = myErrorOccurred;
                _MyDCAIncomeDirStateM.LastAccessFileInDCADir = ServerDirLastChangeAt;
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




        private List<string> GetVaultFileListInCustomDeployStage(string dcaPrefixName = "")
        {

            bool myErrorOccurred;

            string myMoreParams = "";
            string myMessageOut;
            List<string> myFileListing = new List<string>();



            var searchPattren = dcaPrefixName + "*" + CustomsSettingUtil.GetSufix(_CustomsSettingPM.Tenant);
            Debug.WriteLine(string.Format("searchPattren = {0} _CustomsSettingPM.Tenant = {1} ", searchPattren, _CustomsSettingPM.Tenant));
            //searchPattren = "";
            _DcaManager = GetDcaManagr();
            myMoreParams = "";// _DownloadMoreParams;
            myFileListing = _DcaManager.FileListing(
//this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant), 
searchPattren, _AppendToDownloadFolderName,
ref myMoreParams,
out myErrorOccurred,
out myMessageOut);

            if (myErrorOccurred)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

            }
            myFileListing = myFileListing ?? new List<string>();
            return myFileListing;
        }

        private DcaManager GetDcaManagr()
        {
            _DcaManager = _DcaManager ?? new DcaManager(_CustomsSettingPM.DCAServiceAddress, _CustomsSettingPM.DCAPartnerVault + GetDCAPartnerVaultDownloadSuffix(), _CustomsSettingPM.Tenant);
            return _DcaManager;
        }

        private string GetDCAPartnerVaultDownloadSuffix()
        {
            var DCAPartnerVaultDownloadSuffix = ConfigurationManager.AppSettings["DCAPartnerVaultDownloadSuffix"];
            if (string.IsNullOrWhiteSpace(DCAPartnerVaultDownloadSuffix))
            {
                return string.Empty;
            }
            LogMessagingUtil.Instance.AppendLine($"Due Download Adding Suffix ({DCAPartnerVaultDownloadSuffix}) to DCAPartnerVault ");
            return DCAPartnerVaultDownloadSuffix;
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

        DcaManager _DcaManager;
        private List<string> _AllDcaPreFixWithoutInOutUpper;
        private Stopwatch _swDownAll;

        private bool DoDcaMessageFile(InterfaceTenantDefinitionManagementPM messageDCA, DCAFileModel dcaFile)
        {
            bool myErrorOccurred;
            string myMoreParams = "";
            string myMessageOut;

            string fileContentsBASE64 = "";
            string fileContents = "";
            myMoreParams = "";// _DownloadMoreParams;
            SetLastActivity?.Invoke();

            try
            {
                var sw = Stopwatch.StartNew();
                fileContentsBASE64 = _DcaManager.GetContentsBASE64OfDownloadIncomeFile(
            //this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant),
            dcaFile.SelectedFileDownload, this._AppendToDownloadFolderName,
            //out FileName, out FileContentsBASE64,
            ref myMoreParams,
            out myErrorOccurred, out myMessageOut);
                dcaFile.DownloadLog = "GetContentsBASE64OfDownloadIncomeFile:Took=" + sw.Elapsed.ToString();
                if (myErrorOccurred)
                {
                    if (_EnableLog)
                    {
                        //LogMessage(TRequestParams requestParams, string subject, string InOut, out string communicationLogId)
                    }
                    return false;
                }


                SaveRequestSheet(messageDCA, dcaFile, fileContentsBASE64
                //messageBytes
                );

                LogDoneItemInMemoryAction?.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                var log = LogMessagingUtil.Instance.ToString();
                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                //_sbGatewayLog.Insert(0, "ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                //Debug.WriteLine("ProccessRequest():Exception " + FormatedException.ToString(), true);
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role DbEntityValidationException", null, null);
            }
            catch (CustomsRequestsSheetDomainModelServiceException ex)
            {

                if (ex.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.RequestCancelled)
                {
                    ex.ChangeExceptionMessage("RequestCancelled :Deleting:" + dcaFile.SelectedFileDownload);
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role ", null, null);
                }
                else if (ex.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.DcaMessageNotBelongOurEnvironment)
                {

                    _MyDCAIncomeDirStateM.FileMessagesNotBelong2OurEnvironment.Add(dcaFile.SelectedFileDownload);
                    if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("MoveUnUseDCAFilesToDIr")))
                    {
                        string MoveUnUseDCAFilesToDIr = ConfigurationManager.AppSettings.Get("MoveUnUseDCAFilesToDIr");

                        return this.MoveUnUseDCAFileToDir(dcaFile.SelectedFileDownload);


                    }
                    else
                    {
                        using (var myDCARenameIncomeFileService = new DCARenameIncomeFileService(_DcaManager))
                        {

                            return myDCARenameIncomeFileService.RenameIt(messageDCA, dcaFile, this._AppendToDownloadFolderName);
                        }
                    }
                }
                else
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role", null, null);
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DCA Wroker role", null, null);
                return false;
            }


            myMoreParams = "";// _DownloadMoreParams;
            _DcaManager.DeleteIncomeFile(//this.GetPartnerID(messageDCA.Tenant), this.GetUnifreightEnvironmentID(messageDCA.Tenant),
               dcaFile.SelectedFileDownload, this._AppendToDownloadFolderName,
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

        private void SaveRequestSheet(InterfaceTenantDefinitionManagementPM messageDCA, DCAFileModel dcaFile, string fileContentsBASE64
                //byte[] messageBytes
                )
        {



            string currMessagingService = GetMainMessagingService(messageDCA);
            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(currMessagingService))
            {

                Debug.WriteLine("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass = " + currMessagingService);
                Debug.WriteLine("Due infinite errors i cancel writing log"); return;
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

                anaO.DcaReceivedCustomResponseCorrelation(messageDCA.InterfaceManagement, _CustomsSettingPM.Tenant, dcaFile, fileContents);
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




        public void TestDownloadFile(string dcaFile,
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
            if (messageDCA == null)
            {
                throw new Exception("dcaFile=" + dcaFile + " Not found in InterfaceManagement.DcaPrefixName* ");
            }
            //    var base64String =
            //System.Convert.ToBase64String(bytsDcaFile,
            //                       0,
            //                       bytsDcaFile.Length);
            var selectedDcaFile = DCAFilePraser.GetDCAFileModel(dcaFile);
            this.SaveRequestSheet(messageDCA, selectedDcaFile,
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

        public void DCAServerUploadStatus(string dcaFile, DCAServerUploadStatus myDCAServerUploadStatus)
        {

            var selectedDcaFile = DCAFilePraser.GetDCAFileModel(dcaFile);
            var OurRefExtrenalId = selectedDcaFile.OurRefExtrenalId;
            if (string.IsNullOrWhiteSpace(OurRefExtrenalId))
            {
                throw new Exception("dcaFile=" + dcaFile + " unable to parse his OurRefExtrenalId");
            }
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(this._CustomsSettingPM.Tenant);
            var crs = customsRequestsSheetQueryService.GetSingle(OurRefExtrenalId, false, false);
            if (crs == null)
            {
                throw new Exception("dcaFile=" + dcaFile + " OurRefExtrenalId = " + OurRefExtrenalId + " But Not found in db ");
            }


            //string currMessagingService = GetMainMessagingService(messageDCA);
            if (!ContainerAccessor.Container.IsRegistered<IMessagingServiceInterfaceType>(crs.InterfaceTypeCode))
            {

                Debug.WriteLine("Due infinite errors i cancel writing log"); return;
                //ExceptionHandler.HandleException(null, DateTime.Now, 0, "", "WorkerRole", "DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + currMessagingService);
                throw new Exception("DCA MessagingSheetWR: SaveMessageToAnalyzeQueueN():!ContainerAccessor.Container.IsRegistered :analyzeClass=" + crs.InterfaceTypeCode);
                //return;
            }


            AmitalDebuggerUtil.Break();

            var anaO = ContainerAccessor.Container.Resolve<IMessagingServiceInterfaceType>(crs.InterfaceTypeCode);

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {


                anaO.DCAServerUploadStatus(
                    crs.Tenant,
                    selectedDcaFile, myDCAServerUploadStatus);
                scope.Complete();
            }
        }

        public bool AddUnifreightTester { get; set; }
        public Action LogDoneItemInMemoryAction { get; set; }
        public Action SetLastActivity { get; set; }
    }


    public class DCAIncomeDirStateM
    {
        public DCAIncomeDirStateM(int Tenant)
        {
            this.Tenant = Tenant;
            ObjectCreatedAt = DateTime.Now;
            LastAllXmlFileInMyBranch = new List<string>();
            FileMessagesNotBelong2OurEnvironment = new HashSet<string>();
        }
        public int Tenant { get; private set; }
        public DateTime ObjectCreatedAt { get; private set; }

        public bool ErrorOccurred { get; set; }
        public DateTime? LastAccessFileInDCADir { get; set; }

        public DateTime? Dir1stChangedAt { get; set; }


        List<string> _LastAllXmlFileInMyBranch;

        public List<string> LastAllXmlFileInMyBranch
        {
            get
            {
                _LastAllXmlFileInMyBranch = _LastAllXmlFileInMyBranch ?? new List<string>();
                return _LastAllXmlFileInMyBranch;
            }
            set { _LastAllXmlFileInMyBranch = value; }
        }


        public int NothingChangeCount { get; set; }

        public HashSet<string> FileMessagesNotBelong2OurEnvironment { get; set; }
    }

}
