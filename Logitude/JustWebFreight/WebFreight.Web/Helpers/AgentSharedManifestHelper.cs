using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
	public class AgentSharedManifestHelper
	{
        public ManifestSL GetAgentShareManifestSLByEntityId(string entityId ,int tenant)
        {
            ManifestSL manifestSL = null;
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(tenant);
            CommunicationLog sharedAgentcommLog = communicationLogRepository.GetCommunicationLogByEntityIdAndQueueNameAndSubject(entityId, "AgentsSharedLogisticsQueue", "Shared Manifest");
            if (sharedAgentcommLog != null)
            {
                BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                {
                    FileName = sharedAgentcommLog.Document.Id,
                    FolderName = sharedAgentcommLog.Document.Folder,
                    Extension = sharedAgentcommLog.Document.Extension,
                    Tenant = sharedAgentcommLog.Document.Tenant,
                    FileSize = sharedAgentcommLog.Document.FileSize,
                };
                Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                byte[] date = storageservice.Read(fileInfo);
                if (date != null)
                {
                     manifestSL = LogitudeXmlSerializer.DeserializeObject<ManifestSL>(date);

                }
            }
            return manifestSL;
        }

	}
}