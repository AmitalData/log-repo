using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.AutomationModel.SendInterface
{
  public  class SendInterfaceDataContractService
    {
        private object sendInterfaceDataContractObject = null;
        private int tenant;
        private DocumentRepository documentRepository = null;
        private string sendInterfaceDataContractFileName = string.Empty;

        public SendInterfaceDataContractService(byte[] objectData, AutomationSendInterface automationSendInterface, int tenant)
        {
            this.tenant = tenant;

            string instanceTypePath = "WebFreight.Web.Helpers.AutomationModel.SendInterface." + automationSendInterface.InterfaceName + "SendInterfaceDataContract";
            Type instanceAssemblyType = SendInterfaceDataContract.GetInstanceAssemblyType("WebFreight.Web", instanceTypePath);
            if (instanceAssemblyType == null) { throw new ApplicationException("Interface Class Not Found!"); }

            object instanceQueryService = Activator.CreateInstance(instanceAssemblyType, new object[] { objectData });
            
            SendInterfaceDataContractFileNameArgs sendInterfaceDataContractFileNameArgs = GetNewInstanceSendInterfaceDataContractFileNameArgs(automationSendInterface, tenant);
            sendInterfaceDataContractFileName = (string)SendInterfaceDataContract.GetMethodValue(instanceQueryService, "GetFileName", new object[] { sendInterfaceDataContractFileNameArgs });
            
            SendInterfaceDataContractObjectArgs sendInterfaceDataContractObjectArgs = GetNewInstanceSendInterfaceDataContractObjectArgs(automationSendInterface, tenant);
            sendInterfaceDataContractObject = SendInterfaceDataContract.GetMethodValue(instanceQueryService, "GetObject", new object[] { sendInterfaceDataContractObjectArgs });

            documentRepository = new DocumentRepository(tenant);
        }

        private SendInterfaceDataContractObjectArgs GetNewInstanceSendInterfaceDataContractObjectArgs(AutomationSendInterface automationSendInterface, int tenant)
        {
            return new SendInterfaceDataContractObjectArgs
            {
                Tenant = tenant,
                AutomationSendInterface = automationSendInterface
            };
        }

        private SendInterfaceDataContractFileNameArgs GetNewInstanceSendInterfaceDataContractFileNameArgs(AutomationSendInterface automationSendInterface, int tenant)
        {
            return new SendInterfaceDataContractFileNameArgs
            {
                Tenant = tenant,
                Format = automationSendInterface.Format
            };
        }

        public string GetDataContractDocumentId(string format)
        {
            string dataContractDocumentId = string.Empty;
            if (format == "XML") dataContractDocumentId = GetDataContractXMLDocumentId();
            else dataContractDocumentId = GetDataContractJosnDocumentId();
            return dataContractDocumentId;
        }

        private string GetDataContractXMLDocumentId()
        {
            string sendInterfaceDataContractXMLDocumentId = string.Empty;
            if ( sendInterfaceDataContractObject!=null)
            {
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(sendInterfaceDataContractObject, true);
                byte[] xmlfile = Encoding.UTF8.GetBytes(xmlstring);
                Document document = CreateDocument(xmlfile, "xml");
                StorageDataService.WriteFileOnStorage(new StorageDataArgs() { FileName = document.Id, Extension = document.Extension, FolderName = document.Folder, FileData = xmlfile, Tenant = document.Tenant });
                sendInterfaceDataContractXMLDocumentId = document.Id;
            }

            return sendInterfaceDataContractXMLDocumentId;

        }

        private string GetDataContractJosnDocumentId()
        {
            string sendInterfaceDataContractJosnDocumentId = string.Empty;
            if (sendInterfaceDataContractObject != null)
            {
                string josnString = LogitudeXmlSerializer.SerializeObjectToJosnString(sendInterfaceDataContractObject);
                byte[] josnfile = Encoding.UTF8.GetBytes(josnString);
                Document document = CreateDocument(josnfile, "Json");
                StorageDataService.WriteFileOnStorage(new StorageDataArgs() { FileName = document.Id, Extension = document.Extension, FolderName = document.Folder, FileData = josnfile, Tenant = document.Tenant });
                sendInterfaceDataContractJosnDocumentId = document.Id;
            }
            return sendInterfaceDataContractJosnDocumentId;
        }
                
        private Document CreateDocument(byte[] fileData, string extension)
        {
            Document document = new Document
            {
                Id = IdCounter.GetNumber("Document", tenant).ToString(),
                Tenant = tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                FileName = sendInterfaceDataContractFileName,
                FileSize = fileData.Length,
                Extension = extension,
                HasFile = true,
                Folder = "others",

            };
            documentRepository.Add(document);
            documentRepository.SubmitChanges();
 
            return document;
        }
    }
}
