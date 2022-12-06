using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.FTP;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public class AccountingTranferViaFTPHelper
    {
        private int tenant;
        private string objectTableId;
        private string FTPDetailId;
        private ICommonDataContext commonContext;

        public string DocumentId;
        public string DocumentFolder;
        public string DocumentExtension;
        public AccountingTranferViaFTPHelper(int tenant, string objectTableId, string FTPDetailId)
        {
            this.tenant = tenant;
            this.objectTableId = objectTableId;
            this.FTPDetailId = FTPDetailId;
            this.commonContext = CommonDataContext.GetContext(tenant);
        }

        public CommunicationLog CreateCommunicationLog(byte[] ByteData, string fileName, string entityId, string accountingSystemCode)
        {
            TenantRepository tenantrepository = new TenantRepository(commonContext);
            Tenant curtenant = tenantrepository.GetSingleTenant(tenant);

            DocumentRepository documentRepository = new DocumentRepository(this.commonContext);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(this.commonContext);

            string xmlTarget = "Exavault";
            string xmlSubject = accountingSystemCode == "GI" ? "Generic Interface" : "Advanced Generic Interface";

            Document document = new Document()
            {
                CreateDate = System.DateTime.Now,
                Extension = "xml",
                FileSize = ByteData.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = xmlTarget.ToLower(),
            };

            documentRepository.Add(document);
            
            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = System.DateTime.UtcNow,
                From = curtenant.Company,
                To = xmlTarget,
                InOut = "O",
                EntityId = entityId,
                ObjectTableId = objectTableId,                
                Subject = xmlSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                DocumentId = document.Id,
                SearchFields = xmlTarget + "," + "O" + "," + xmlSubject,
                CreateDateUTC = System.DateTime.UtcNow,
                LogSettings = this.GetLogSettings(xmlSubject, fileName),
                QueueName = "FTPCommunicationLogQueue",
            };

            communicationLogRepository.Add(commLog);
            commonContext.SaveChanges();

            this.DocumentId = document.Id;
            this.DocumentFolder = document.Folder;
            this.DocumentExtension = document.Extension;

            return commLog;
        }

        private string GetLogSettings(string xmlSubject, string fileName)
        {
            string myResult = null;

            if (this.FTPDetailId != null)
            {
                FTPDetail fTPDetail = (from d in this.commonContext.FTPDetails where d.Id == this.FTPDetailId select d).FirstOrDefault();

                if (fTPDetail != null)
                {
                    LogSettings settings = new LogSettings()
                    {
                        Host = fTPDetail.Host,
                        Folder = fTPDetail.Folder + "\\fromlogitude",
                        Username = fTPDetail.UserName,
                        Password = fTPDetail.Password,
                        Filename = fileName,
                        UseSFTP = fTPDetail.UseSFTP,
                    };

                    myResult = JsonConvert.SerializeObject(settings);
                }
            }

            return myResult;
        }

        public void Test(CommunicationLog log, int tenant)
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            DocumentRepository documentRepository = new DocumentRepository(commoncontext);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(commoncontext);

            Document document = documentRepository.GetSingleDocument(tenant, log.DocumentId);
            Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = document.FileSize,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            byte[] filedata = storageservice.Read(fileInfo);

            if (filedata != null)
            {
                if (!string.IsNullOrEmpty(log.LogSettings))
                {
                    LogSettings settingsData = JsonConvert.DeserializeObject<LogSettings>(log.LogSettings);
                    if (settingsData != null)
                    {
                        string ftpHostIP = settingsData.Host;
                        string ftpUserName = settingsData.Username;
                        string ftpPassword = settingsData.Password;
                        string ftpFolderName = settingsData.Folder;
                        string fileName = (!string.IsNullOrEmpty(settingsData.Filename) ? settingsData.Filename : document.Id) + "." + document.Extension;
                        string p_message = "";
                        if (!settingsData.UseSFTP)
                        {

                            ////ftp://192.116.221.106/temp1
                            //string hostIP = @"ftp://" + settingsData.host;

                            FTPService ftpService = new FTPService(ftpHostIP, settingsData.Username, settingsData.Password);

                            ftpService.Upload(fileName, settingsData.Folder, filedata, out p_message);
                            log.Logs += Environment.NewLine + DateTime.Now.ToString() + " : " + p_message;
                        }
                        else
                        {
                            ftpHostIP = settingsData.Host;
                            string p_status = "";

                            SFTPService sftpService = new SFTPService();
                            sftpService.Logon(ftpHostIP, ftpUserName, ftpPassword, "22", ftpFolderName, out p_status, out p_message);
                            log.Logs += p_message;
                            if (p_status == "0")
                            {

                                sftpService.Upload(fileName, filedata, true, true, out p_status, out p_message);

                                if (p_status == "-1")
                                    throw new FTPServiceException("SFTP upload file failed: " + p_message);
                            }
                            else
                                throw new FTPServiceException("SFTP Login failed: " + p_message);

                            log.Logs += p_message;
                        }
                    }
                }

                log.CommunicationStatusTypeCode = "D";
                log.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                log.DoneDateUTC = DateTime.UtcNow;
                log.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                log.LastStatusDateUTC = DateTime.UtcNow;
                communicationLogRep.Update(log);
                communicationLogRep.SubmitChanges();

            }
            else
            {
                throw new Exception("The file data was not found!");
            }
        }
    }

    public class LogSettings
    {
        public string Host { get; set; }
        public string Folder { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Filename { get; set; }
        public bool UseSFTP { get; set; }
    }
}
