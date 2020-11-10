using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.Service
{
  public  class SendInterfaceDataContractService
    {
        private object sendInterfaceDataContractObject = null;
        private int tenant;
        private DocumentRepository documentRepository = null;
        private string sendInterfaceDataContractFileName = string.Empty;

        public SendInterfaceDataContractService(object entityPM,  string computingPartnerId , int tenant)
        {
            this.tenant = tenant;
            string computingPartnerCode = GetComputingPartnerCode(tenant, computingPartnerId);
            sendInterfaceDataContractFileName = GetSendInterfaceDataContractFileName(entityPM);
            sendInterfaceDataContractObject = GetSendInterfaceDataContractObject(entityPM, computingPartnerCode);
            documentRepository = new DocumentRepository(tenant);
        }

        public string GetSendInterfaceDataContractDocumentId(string format)
        {
            string dataContractDocumentId = string.Empty;
            if (format == "XML") dataContractDocumentId = GetSendInterfaceDataContractXMLDocumentId();
            else dataContractDocumentId = GetSendInterfaceDataContractJosnDocumentId();
            return dataContractDocumentId;
        }

        private string GetSendInterfaceDataContractXMLDocumentId()
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

        private string GetSendInterfaceDataContractJosnDocumentId()
        {
            string sendInterfaceDataContractJosnDocumentId = string.Empty;
            if (sendInterfaceDataContractObject != null)
            {
                string josnString = LogitudeXmlSerializer.SerializeObjectToJosnString(sendInterfaceDataContractObject);
                byte[] josnfile = Encoding.UTF8.GetBytes(josnString);
                Document document = CreateDocument(josnfile, "Josn");
                StorageDataService.WriteFileOnStorage(new StorageDataArgs() { FileName = document.Id, Extension = document.Extension, FolderName = document.Folder, FileData = josnfile, Tenant = document.Tenant });
                sendInterfaceDataContractJosnDocumentId = document.Id;
            }
            return sendInterfaceDataContractJosnDocumentId;
        }

        private object GetSendInterfaceDataContractObject(object entityPM , string computingPartnerCode)
        {
            object sendInterfaceDataContractObject = null;
            Assembly blAssembly = Assembly.Load("Logitude.BL");
            string typePath = "Logitude.BL.ShipmentsModel.APIDataContract.ApiV1." + sendInterfaceDataContractFileName + "QueryService";
            Type type = blAssembly.GetType(typePath);
            var queryService = Activator.CreateInstance(type, new object[] { tenant });
            MethodInfo methodInfo = queryService.GetType().GetMethods().Where(d => d.Name == (sendInterfaceDataContractFileName + "DataMapping")).FirstOrDefault();
            object[] parameters = new object[] { entityPM, tenant, computingPartnerCode };
            if (methodInfo != null) sendInterfaceDataContractObject = methodInfo.Invoke(queryService, parameters);
            return sendInterfaceDataContractObject;
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

        private string GetSendInterfaceDataContractFileName(object entityPM)
        {
            string sendInterfaceDataContractFileName = GetPropertyValueFromObject("ShipmentLevelName" , entityPM);
            if (sendInterfaceDataContractFileName == "Consol") sendInterfaceDataContractFileName = "Master";
            return sendInterfaceDataContractFileName;
        }

        private string GetPropertyValueFromObject(string propertyName, object entity)
        {
            string propertyValue = string.Empty;
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(entity).ToString();
            }
            return propertyValue;
        }

        private static string GetComputingPartnerCode(int tenant, string computingPartnerId)
        {
            ComputingPartnerRepository computingPartnerRepository = new ComputingPartnerRepository(tenant);
            string computingPartnerCode = computingPartnerRepository.GetSingleComputingPartnerCodeById(computingPartnerId);
            return computingPartnerCode;
        }

    }
}
