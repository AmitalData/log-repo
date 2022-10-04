using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using WebFreight.Web.Helpers.CallBack.Handler.EmailDocument;

namespace WebFreight.Web.Helpers.CallBack.Handler
{
    public class EmailDocumentHandlerService : IHandlerService
    {
        public void Handel(string handlerArgs, object result)
        {
            EmailDocumentHandlerArgs emailDocumentHandlerArgs = LogitudeXmlSerializer.DeserializeObject<EmailDocumentHandlerArgs>(handlerArgs);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(emailDocumentHandlerArgs.Tenant);
            ObjectTable objectTable = objectTableRepository.GetObjectTableById(emailDocumentHandlerArgs.ObjectTableId, emailDocumentHandlerArgs.Tenant);
            string objectTableName = objectTable.Name;
            switch (objectTableName)
            {
                case "ARInvoice":
                    {
                        ARInvoiceDocumentHandlerService.SetInvoiceAsSent(emailDocumentHandlerArgs);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }




        public static string GetCallBackDetailsXml(string documentFilingId, int tenant)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("ARS",tenant)) return "";
            string emailDocumentHandlerSerializerArgs = GetEmailDocumentHandlerCallBackArgs(documentFilingId, tenant);
            if (string.IsNullOrEmpty(emailDocumentHandlerSerializerArgs)) return "";

            CallBackDetails callBackDetails = new CallBackDetails
            {
                HandlerServiceName = "EmailDocumentHandlerService",
                HandlerArgs = emailDocumentHandlerSerializerArgs
            };

            return LogitudeXmlSerializer.SerializeObjectToXmlElementString(callBackDetails);
        }

        public static string GetCallBackDetailsXml(EmailDocumentHandlerArgs emailDocumentHandlerArgs)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("ARS", emailDocumentHandlerArgs.Tenant)) return "";
            if (emailDocumentHandlerArgs == null) return "";
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(emailDocumentHandlerArgs.Tenant);
            string documentTypeCode = documentTypeRepository.GetDocumentTypeCodeById(emailDocumentHandlerArgs.DocumentId, emailDocumentHandlerArgs.Tenant);
            if (string.IsNullOrEmpty(documentTypeCode) || !IsCorrectDocument(documentTypeCode)) return "";

            CallBackDetails callBackDetails = new CallBackDetails
            {
                HandlerServiceName = "EmailDocumentHandlerService",
                HandlerArgs = LogitudeXmlSerializer.SerializeObjectToXmlElementString(emailDocumentHandlerArgs)
            };

            return LogitudeXmlSerializer.SerializeObjectToXmlElementString(callBackDetails);
        }


        private static string GetEmailDocumentHandlerCallBackArgs(string documentFilingId, int tenant)
        {
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(tenant);
            var documentFiling = documentsFilingRepository.GetSingleWithIncludeDocumentType(documentFilingId, tenant);
            if (documentFiling == null || documentFiling.DocumentType == null || !IsCorrectDocument(documentFiling.DocumentType.Code)) return null;

            EmailDocumentHandlerArgs emailDocumentHandlerArgs = new EmailDocumentHandlerArgs()
            {
                EntityId = string.IsNullOrEmpty(documentFiling.ChildEntityId) ? documentFiling.EntityId : documentFiling.ChildEntityId,
                ObjectTableId = documentFiling.DocumentType.ObjectTableId,
                Tenant = tenant
            };

            return LogitudeXmlSerializer.SerializeObjectToXmlElementString(emailDocumentHandlerArgs);
        }

        private static bool IsCorrectDocument(string documentTypeCode)
        {
            if (string.IsNullOrEmpty(documentTypeCode)) return false;
            List<string> invoiceCorrectDocuments = new List<string>();
            invoiceCorrectDocuments.Add("999S");//invoice
            invoiceCorrectDocuments.Add("999CI");//Custom invoice
            invoiceCorrectDocuments.Add("999M");//Manifest Invoice
            invoiceCorrectDocuments.Add("999C");//consolidation invoice
            invoiceCorrectDocuments.Add("999G");//general invoice
            return invoiceCorrectDocuments.Contains(documentTypeCode);
        }
    }


    [DataContract(Namespace = "")]
    public class EmailDocumentHandlerArgs
    {
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public string EntityId { get; set; }
        [DataMember]
        public string ObjectTableId { get; set; }

        [DataMember]
        public string DocumentId { get; set; }
    }


}