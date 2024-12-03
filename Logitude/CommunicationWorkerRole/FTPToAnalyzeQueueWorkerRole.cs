using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.FTP;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class FTPToAnalyzeQueueWorkerRole : WorkerEntryPoint
    {
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        CustomsInterfaceSettingRepository customsInterfaceSettingRepository = new CustomsInterfaceSettingRepository(0);
                        List<CustomsInterfaceSetting> artimusSettings = customsInterfaceSettingRepository.GetAllArtimusCustomsInterfaceSettings().ToList();
                        foreach (CustomsInterfaceSetting setting in artimusSettings)
                        {
                            FTPDetailRepository ftpDetailsRepository = new FTPDetailRepository(setting.Tenant);
                            FTPDetail ftpDetail = ftpDetailsRepository.GetSingleFTPDetail(setting.ArtemusInSettingsId, setting.Tenant);
                            ReadFTPFiles(ftpDetail, "Artemus");
                        }

                        ICommonDataContext myCommonContext = CommonDataContext.GetContext((int)Tenant);
                        List<INTTRASetting> allINTTRASetting = myCommonContext.INTTRASettings.Where(d => d.InSettingsId != null).ToList();
                        foreach (INTTRASetting item in allINTTRASetting)
                        {
                            FTPDetailRepository ftpDetailsRepository = new FTPDetailRepository(myCommonContext);
                            FTPDetail ftpDetail = ftpDetailsRepository.GetSingleFTPDetail(item.InSettingsId, item.Tenant);
                            this.ReadFTPFiles(ftpDetail, "INTTRA");
                        }
                        
                        List<AccountingSetting> accountingSettings = myCommonContext.AccountingSettings.Where(d => d.TransferFTPDetailId != null).ToList();
                        foreach (AccountingSetting item in accountingSettings)
                        {
                            FTPDetailRepository ftpDetailsRepository = new FTPDetailRepository(myCommonContext);
                            FTPDetail ftpDetail = ftpDetailsRepository.GetSingleFTPDetail(item.TransferFTPDetailId, item.Id);
                            this.ReadFTPFiles(ftpDetail, "Transfer");
                        }

                        LogDoneItemInMemory();
                        Thread.Sleep(60000);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FTP To AnalyzeQueue WorkerRole", ex.Message, null);
                        Thread.Sleep(60000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "FTPToAnalyzeQueue";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }
        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private void ReadFTPFiles(FTPDetail ftpDetail, string myService)
        {
            try
            {
                FTPService ftpService = new FTPService(ftpDetail.Host, ftpDetail.UserName, ftpDetail.Password);
                string[] directoryFiles = ftpService.DirectoryListSimple(ftpDetail.Folder);

                foreach (string fileName in directoryFiles)
                {
                    string extention = Path.GetExtension(fileName);
                    if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(extention))
                    {
                        string p_message = "";
                        byte[] fileData = ftpService.Download(fileName, out p_message);

                        switch (myService)
                        {
                            case "Artemus":
                                {
                                    SaveMessageToAnalyzeQueue_Artemus(fileName, fileData, ftpDetail.Tenant);
                                    break;
                                }

                            case "INTTRA":
                                {
                                    SaveMessageToAnalyzeQueue_INTTRA(fileName, fileData, ftpDetail.Tenant);
                                    break;
                                }

                            case "Transfer":
                                {
                                    SaveMessageToAnalyzeQueue_Transfer(fileName, fileData, ftpDetail.Tenant);
                                    break;
                                }
                        }

                        ftpService.Delete(fileName);
                    }
                }
            }
            catch (FTPServiceException exc)
            {
                // will add log in the future
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "FTP To AnalyzeQueue WorkerRole", ex.Message, null);
            }
        }

        private void SaveMessageToAnalyzeQueue_Artemus(string fileName, byte[] messageData, int tenant)
        {            
            fileName = fileName.Split('/')[fileName.Split('/').Length - 1].ToLower();
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            
            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                Subject = fileName.StartsWith("bl") ? "BL Response" : (fileName.StartsWith("voyage") ? "Voyage Response" : "Artemus Response"),
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "Artemus",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageData,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = true,
                Tenant = tenant,
                FileSize = messageData.Length,
                FileName = fileName,
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }
        private void SaveMessageToAnalyzeQueue_INTTRA(string fileName, byte[] fileBytes, int tenant)
        {
            fileName = fileName.Split('/')[fileName.Split('/').Length - 1].ToLower();
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                //Subject = fileName.StartsWith("bl") ? "BL Response" : (fileName.StartsWith("voyage") ? "Voyage Response" : "Artemus Response"),
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "INTTRA",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = fileBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                //Tenant = tenant,
                FileSize = fileBytes.Length,
                FileName = fileName,
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }
        private void SaveMessageToAnalyzeQueue_Transfer(string fileName, byte[] fileBytes, int tenant)
        {
            fileName = fileName.Split('/')[fileName.Split('/').Length - 1].ToLower();
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();

            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "Generic Interface",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = fileBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = fileBytes.Length,
                FileName = fileName,
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }
    }
}
