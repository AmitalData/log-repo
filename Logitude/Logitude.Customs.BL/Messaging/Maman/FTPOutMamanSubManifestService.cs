using Logitude.BL.Helpers;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class FTPOutMamanSubManifestService///using  by FTPCommunicationWorkerRole
    {


        private string communicationSubject= "שידור פנימיים מסוכנים לממן";
        public void BuildCommunicationLog(byte[] bytearray, int tenant, string entityId, string FileName= null)///using  by FTPCommunicationWorkerRole
        {
            if (String.IsNullOrWhiteSpace(FileName))
            {
                FileName = GetDefaultFileName();
            }
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("Customs.CourierMaster");

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);

            string target = "Maman";
            string xmlSubject = communicationSubject;
            string host = "";
            string folder = "";
            string username = "";
            string password = "";

            var myCustomsPartnerFtpQueryService = new CustomsPartnerFtpQueryService(tenant);
            var pmCustomsPartnerFtp = myCustomsPartnerFtpQueryService.GetBy(tenant, CustomsPartnerFtpDetails.InterfaceName_SubManifest, CustomsPartnerFtpDetails.PartnerCode_Mamam, CustomsPartnerFtpDetails.TypeCode_Out);


            //CustomsInterfaceSettingRepository customsInterfaceSettingRepository = new CustomsInterfaceSettingRepository(tenant);
            ////('CMN', 'Maman Courier', 'CMN,Maman Courier', '0', 'IM')
            //CustomsInterfaceSetting interfaceSetting = (from d in commonContext.CustomsInterfaceSettings
            //                                            where d.Tenant == tenant && d.ImportToUSAInterfaceCode == "CMN"
            //                                            select d).FirstOrDefault();

            //if (interfaceSetting != null)
            if (pmCustomsPartnerFtp != null)
            {
                //string artemusOutSettingsId = interfaceSetting.ArtemusOutSettingsId;
                string FtpDetailsId = pmCustomsPartnerFtp.FtpDetailsId;
                FTPDetailRepository fTPDetailRepository = new FTPDetailRepository(tenant);
                FTPDetail fTPDetail = (from d in commonContext.FTPDetails
                                       where d.Tenant == tenant && d.Id == FtpDetailsId
                                       select d).FirstOrDefault();

                if (fTPDetail != null)
                {
                    host = fTPDetail.Host;
                    folder = fTPDetail.Folder;
                    username = fTPDetail.UserName;
                    password = fTPDetail.Password;
                }

                //if (communicationSubject == "BOL")
                //{
                //    communicationSubject = "BL";
                //}
                //string filename = FileName;
                var settings = new CommunicationLogSettings() { host = host, folder = folder, username = username, password = password, filename = FileName };
                var settingsData = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(settings);

                Document document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "HWB",
                    FileSize = bytearray.Length,
                    Tenant = Convert.ToInt32(tenant),
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = target.ToLower(),
                };

                documentRepository.Add(document);
                documentRepository.SubmitChanges();

                CommunicationLog commLog = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    LastStatusDateUTC = DateTime.UtcNow,
                    To = target,
                    InOut = "O",
                    EntityId = entityId,
                    ObjectTableId = objectTableId,
                    Subject = xmlSubject,
                    Tenant = tenant,
                    CommunicationLogTypeCode = "T",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CommunicationStatusTypeCode = "W",
                    DocumentId = document.Id,
                    CreateDateUTC = DateTime.UtcNow,
                    LogSettings = settingsData,
                    QueueName = "FTPCommunicationLogQueue" ///using  by FTPCommunicationWorkerRole
                };

                communicationLogRepository.Add(commLog);
                communicationLogRepository.SubmitChanges();

                Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = bytearray.Length,
                };

                Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                storageservice.Write(bytearray.ToArray(), fileInfo);

                SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);

#if false
                ShipmentCustomsTransmissionArgs args = new ShipmentCustomsTransmissionArgs()
                {
                    ShipmentId = "shipmentId",
                    MessageType = communicationSubject == "BL" ? "ARBL" : "ASVO",
                    Status = "SENT",
                    CommunicationLogId = commLog.Id,
                };

                ShipmentCustomsTransmissionHelper transmissionHelper = new ShipmentCustomsTransmissionHelper(tenant);
                transmissionHelper.Run(args);
#endif
            }
        }

        private static string GetDefaultFileName()
        {
            string FileName;
            DateTime @now = DateTime.Now;
            string MM = "00" + @now.Month.ToString();
            MM = MM.Substring(MM.Length - 2);
            string dd = "00" + @now.Day.ToString();
            dd = dd.Substring(dd.Length - 2);
            var hhmn = @now.ToString("HH:mm").Replace(":", String.Empty);
            FileName = $"A{MM}{dd}{hhmn}";//.HWB";
            return FileName;
        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send FTP CommunicationLog Queue", null, null);
            }
        }
    }

    public class CommunicationLogSettings
    {
        public string host { get; set; }
        public string folder { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string filename { get; set; }
    }
}
