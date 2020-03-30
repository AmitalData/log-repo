using Logitude.Server.Tools.Counters;
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

        public CommunicationLog CreateCommunicationLog(byte[] ByteData, string fileName, string entityId)
        {
            TenantRepository tenantrepository = new TenantRepository(commonContext);
            Tenant curtenant = tenantrepository.GetSingleTenant(tenant);

            DocumentRepository documentRepository = new DocumentRepository(this.commonContext);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(this.commonContext);

            string xmlTarget = "Exavault";
            string xmlSubject = "Generic Interface";

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
            documentRepository.SubmitChanges();
            
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
                        Folder = fTPDetail.Folder,
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
