
using CustomsWorkerRole.L2U;

using Logitude.Server.Tools.Models;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
//using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnifreightIIG.UServer;
using CustomsWorkerRole.Queue;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System.Net.Http;
using Logitude.Customs.BL.Messaging.Maman;
using Microsoft.Practices.Unity;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.FTP;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.Repositories;
using Unifreight.Data.AmitalModel;
using Logitude.Server.Tools.Utils;
using Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue;

namespace CustomsWorkerRole
{
    /* 
     * ///couriernet_global _global _global
Insert into BATCHSERVICESDEFINITIONS (CODE,CLASSNAME) values ('FTPToAnalyzeQueueWR','FTPToAnalyzeQueueWR');
Insert into BATCHSERVICESDEFINITIONMODS (CODE,INACTIVE,NUMBEROFTHREADS) values ('FTPToAnalyzeQueueWR',0,1);


        
INSERT INTO "ANALYZEQUEUESTATUS" (CODE, NAME) VALUES ('D', 'Done')
INSERT INTO "ANALYZEQUEUESTATUS" (CODE, NAME) VALUES ('F', 'Fail')
INSERT INTO "ANALYZEQUEUESTATUS" (CODE, NAME) VALUES ('W', 'Waiting')

     */

    //public class SendWebAPI2MamanGWMessageECTHRDataWR
    public class FTPToAnalyzeQueueWR
        : CustomsWorkerEntryPoint
    {
        QueueDescription _QueueDescription;
        QueueClient _QueueClient;
        public override void Run()
        {

            while (true)
            {

                if (!General.IsUpdating())
                {
                    try
                    {

                        WorkOnce();
                        Thread.Sleep(TimeSpan.FromSeconds(_SeedTenant));
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "FTPToAnalyzeQueueWR : Run() Method", null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }


            }

        }
        bool _OnStartDone = false;
        //private DbQueueService _IQueueService;
        private ICommonDataContext _ICommonDataContext;

        private CommunicationLogRepository _CommunicationLogRep;
        private int _Tenant;

        private CommunicationLog _WaitingCommLog;

        private DateTime _LastCreateFtpDefinition;
        private List<CustomsPartnerFtpPM> _FtpDefinitions;
        private int _SeedTenant = 1;

        //private QueueResponse _ReceivedBrokeredMessage;

        public override bool OnStart()
        {
            try
            {
                if (_OnStartDone) return true;
                _OnStartDone = true;
                DoneItemsInRange = new Dictionary<DateTime, int>();


                var myClass = this.GetType().Name;

                CreateFtpDefinitionsEvery10Min();

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "amital send data worker role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.


            return base.OnStart();
        }

        private void CreateFtpDefinitionsEvery10Min()
        {
            if (DateTime.Now.Subtract(_LastCreateFtpDefinition) < TimeSpan.FromMinutes(15))
            {
                return;
            }
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {

                //ProccessReceivedMessage();


                _LastCreateFtpDefinition = DateTime.Now;
                _FtpDefinitions = new List<CustomsPartnerFtpPM>();
                var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var ftpIncustomsPartnerFtpDetails = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.TypeCode == CustomsPartnerFtpDetails.TypeCode_In)
                    .Where(r => r.ViaMethod == "FTP")
                    .ToList();

                ftpIncustomsPartnerFtpDetails.ForEach(ftpIncustomsPartnerFtpDetail =>
                {

                    var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(_SeedTenant);
                    var pmCustomsPartnerFtps = myCustomsPartnerFtpQueryService.GetAllTenantBy(ftpIncustomsPartnerFtpDetail.Code /*CustomsPartnerFtpDetails.InterfaceName_ECSPCL*/,
                        ftpIncustomsPartnerFtpDetail.Partner,
                        ftpIncustomsPartnerFtpDetail.TypeCode);


                    foreach (var pmCustomsPartnerFtp in pmCustomsPartnerFtps)
                    {
                        //if (pmCustomsPartnerFtp != null)
                        {


                            FTPDetailRepository ftpDetailsRepository = new FTPDetailRepository(_SeedTenant);
                            FTPDetail ftpDetail = ftpDetailsRepository.GetSingleFTPDetail(pmCustomsPartnerFtp.FtpDetailsId, pmCustomsPartnerFtp.Tenant);
                            if (ftpDetail != null)
                            {

                                pmCustomsPartnerFtp.MyFtpDetail = ftpDetail;
                                _FtpDefinitions.Add(pmCustomsPartnerFtp);
                            }
                        }

                    }
                });
            }
        }

        public override void WorkOnce()
        {

            try
            {
                OnStart();

                WorkUntilQEmpty_Db();


            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole" + this.GetType().Name, " : Run() Method", null);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                _OnStartDone = false;
            }


        }

        private void WorkUntilQEmpty_Db()
        {
            CreateFtpDefinitionsEvery10Min();

            while (!WorkerRoleServiceLocator.PleaseShutDown)
            {

                foreach (CustomsPartnerFtpPM ftpDef in _FtpDefinitions)
                {
                    LastActivity = DateTime.UtcNow;
                    if (ftpDef.MyFtpDetail.UseSFTP)
                    {
                        DownloadSFTPFiles(ftpDef);
                    }
                    else
                    {
                        DownloadFTPFiles(ftpDef);
                    }
                    if (WorkerRoleServiceLocator.PleaseShutDown)
                    {
                        break;
                    }
                }


                //Thread.Sleep(TimeSpan.FromSeconds(5));

                Thread.Sleep(TimeSpan.FromSeconds(15));//not using soo mach 
                break;


            }
        }
        static List<string> _BadFileNamesCache = new List<string>();
        static DateTime _LastClearCacheBadFileNames = DateTime.MinValue;

        private void DownloadSFTPFiles(CustomsPartnerFtpPM customsPartnerFtpPM)
        {

            try
            {
                if (DateTime.Now.Subtract(_LastClearCacheBadFileNames) > TimeSpan.FromHours(1))
                {
                    ClearBadFileNamesCache();
                }
                string p_message = "";
                string p_status = "";

                var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var defInterfaceDetails = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.Code == customsPartnerFtpPM.InterfaceName).First();
                Debug.WriteLine($"DownloadFTPFiles({customsPartnerFtpPM.InterfaceName},T{customsPartnerFtpPM.Tenant})");
                var ftpDetail = customsPartnerFtpPM.MyFtpDetail;

                Debug.WriteLine($"FTPService({ftpDetail.Host}, {ftpDetail.UserName}, {ftpDetail.Password})");
                SFTPService sftpService = new SFTPService();
                sftpService.Logon(ftpDetail.Host, ftpDetail.UserName, ftpDetail.Password, "22", ftpDetail.Folder, out p_status, out p_message);
                Debug.WriteLine($"DirectoryListSimple({ftpDetail.Folder})");
               
                var directoryFiles = sftpService.DirList("*", true, false, out p_status, out p_message).ToList();
                Debug.WriteLine($"directoryFiles.Count=({directoryFiles.Count})");
                if (!string.IsNullOrWhiteSpace(customsPartnerFtpPM.FileExt))
                {

                    Debug.WriteLine($"FileExt=({customsPartnerFtpPM.FileExt})");
                    directoryFiles = directoryFiles.Where(f => (
                    Path.GetExtension(f)
                    .Contains(customsPartnerFtpPM.FileExt)))
                    .ToList();
                    Debug.WriteLine($"directoryFiles.Count=({directoryFiles.Count})");
                }
                if (!string.IsNullOrWhiteSpace(customsPartnerFtpPM.FileName))
                {
                    Debug.WriteLine($"FileExt=({customsPartnerFtpPM.FileName})");
                    directoryFiles = directoryFiles.Where(f => (
                     Path.GetFileNameWithoutExtension(f)
                    .Contains(customsPartnerFtpPM.FileName)))
                    .ToList();
                    Debug.WriteLine($"directoryFiles.Count=({directoryFiles.Count})");
                }
                directoryFiles = directoryFiles.Where(r => !String.IsNullOrWhiteSpace(r)).ToList();
                directoryFiles = directoryFiles.OrderBy(fileName => fileName).ToList();
                foreach (string fileName in directoryFiles)
                {
                    if (_BadFileNamesCache.Contains(fileName))
                    {
                        Debug.WriteLine($"continue>BadFileNamesCache({fileName})");
                        continue;
                    }

                    var fileWithFolder = ftpDetail.Folder + "/" + Path.GetFileName(fileName);//in linux i get folder\fileName  in win only file name !!
                    Debug.WriteLine($"ftpService.Download({fileWithFolder})");
                    byte[] fileData = sftpService.DownloadFile(fileName, out p_status, out p_message);
                    Debug.WriteLine($"SaveMessageToAnalyzeQueue");
                    int tenant = customsPartnerFtpPM.Tenant;
                    LastActivity = DateTime.UtcNow;
                    try
                    {
                        Debug.WriteLine($"fileData.Length == {fileData.Length}");
                        if (fileData.Length > 0)
                        {

                            Debug.WriteLine($"SaveAnalyzeQueue({defInterfaceDetails}, {fileName}, {fileData}, {tenant})");
                            SaveAnalyzeQueue(defInterfaceDetails, fileName, fileData, tenant);
                        }
                        Debug.WriteLine($"ftpService.Delete({fileName})");
                        sftpService.DeleteFile(fileName, out p_status, out p_message);
                        LogDoneItemInMemory();

                    }
                    catch (Exception ex1)
                    {
                        _BadFileNamesCache.Add(fileName);
                        ExceptionHandler.HandleException(ex1, DateTime.Now, 0, null, "FTP To AnalyzeQueue WorkerRole", ex1.Message, null);

                    }
                }

            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FTP To AnalyzeQueue WorkerRole", ex.Message, null);
            }
        }
        private void DownloadFTPFiles(CustomsPartnerFtpPM customsPartnerFtpPM)
        {

            try
            {
                if (DateTime.Now.Subtract( _LastClearCacheBadFileNames)> TimeSpan.FromHours(1))
                {
                    ClearBadFileNamesCache();
                }
                var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var defInterfaceDetails = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.Code == customsPartnerFtpPM.InterfaceName).First();
                Debug.WriteLine($"DownloadFTPFiles({customsPartnerFtpPM.InterfaceName})");
                var ftpDetail = customsPartnerFtpPM.MyFtpDetail;

                Debug.WriteLine($"FTPService({ftpDetail.Host}, {ftpDetail.UserName}, {ftpDetail.Password})");
                FTPService ftpService = new FTPService(ftpDetail.Host, ftpDetail.UserName, ftpDetail.Password);
                Debug.WriteLine($"DirectoryListSimple({ftpDetail.Folder})");
                var directoryFiles = ftpService.DirectoryListSimple(ftpDetail.Folder).ToList();
                Debug.WriteLine($"directoryFiles.Count=({directoryFiles.Count})");
                if (!string.IsNullOrWhiteSpace(customsPartnerFtpPM.FileExt))
                {

                    Debug.WriteLine($"FileExt=({customsPartnerFtpPM.FileExt})");
                    directoryFiles = directoryFiles.Where(f => (
                    Path.GetExtension(f)
                    .Contains(customsPartnerFtpPM.FileExt)))
                    .ToList();
                    Debug.WriteLine($"directoryFiles.Count=({directoryFiles.Count})");
                }
                if (!string.IsNullOrWhiteSpace(customsPartnerFtpPM.FileName))
                {
                    Debug.WriteLine($"FileExt=({customsPartnerFtpPM.FileName})");
                    directoryFiles = directoryFiles.Where(f => (
                     Path.GetFileNameWithoutExtension(f)
                    .Contains(customsPartnerFtpPM.FileName)))
                    .ToList();
                    Debug.WriteLine($"directoryFiles.Count=({directoryFiles.Count})");
                }
                directoryFiles = directoryFiles.Where(r => !String.IsNullOrWhiteSpace(r)).ToList();
                directoryFiles = directoryFiles.OrderBy(fileName => fileName).ToList();
                foreach (string fileName in directoryFiles)
                {
                    if (_BadFileNamesCache.Contains(fileName))
                    {
                        Debug.WriteLine($"continue>BadFileNamesCache({fileName})");
                        continue;
                    }

                    var fileWithFolder = ftpDetail.Folder + "/" + Path.GetFileName(fileName);//in linux i get folder\fileName  in win only file name !!
                    Debug.WriteLine($"ftpService.Download({fileWithFolder})");
					string p_message = "";
					byte[] fileData = ftpService.Download(fileWithFolder,out p_message);



                    Debug.WriteLine($"SaveMessageToAnalyzeQueue");

                    int tenant = customsPartnerFtpPM.Tenant;
                    LastActivity = DateTime.UtcNow;
                    try
                    {
                        Debug.WriteLine($"fileData.Length == {fileData.Length}");
                        if (fileData.Length > 0)
                        {
                            
                            SaveAnalyzeQueue(defInterfaceDetails, fileName, fileData, tenant);
                        }
                        Debug.WriteLine($"ftpService.Delete({fileName})");
                        ftpService.Delete(fileWithFolder);
                        LogDoneItemInMemory();

                    }
                    catch (Exception ex1)
                    {
                        _BadFileNamesCache.Add(fileName);
                        ExceptionHandler.HandleException(ex1, DateTime.Now, 0, null, "FTP To AnalyzeQueue WorkerRole", ex1.Message, null);

                    }
                }

            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FTP To AnalyzeQueue WorkerRole", ex.Message, null);
            }
        }

        public static void SaveAnalyzeQueue(InterfaceDetails defInterfaceDetails, string fileName, byte[] fileData, int tenant)
        {
            var analyzeQueueUtil = new AnalyzeQueueUtil();
            analyzeQueueUtil.SaveMessageToAnalyzeQueue(fileName, fileData, tenant, "", defInterfaceDetails, null);
        }

        private void ClearBadFileNamesCache()
        {
            _LastClearCacheBadFileNames = DateTime.Now;
            _BadFileNamesCache.Clear();
        }

        public static void SaveAnalyzeQueueFromCode(int tenant, string interfaceCode, string fileName, byte[] fileData)
        {
            var customsPartnerFtpDetails = new CustomsPartnerFtpDetails();
            var defInterfaceDetails = customsPartnerFtpDetails.GetAllInterfaceDetails()
                    .Where(r => r.Code == interfaceCode).First();
            SaveAnalyzeQueue(defInterfaceDetails, fileName, fileData, tenant);
        }
    }



}
