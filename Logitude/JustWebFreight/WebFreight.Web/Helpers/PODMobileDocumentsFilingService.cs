using EvoPdf;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Document = Simplog.Data.CommonDataModel.EntityPOCOs.Document;

namespace WebFreight.Web.Helpers
{
    public class PODMobileDocumentsFilingService
    {
        private ObjectTableRepository objectTableRepository;
        private ContactRepository contactRepository;
        private DocumentRepository documentRepository;

        private PODMobileDocumentsFilingArgs podMobileDocumentsFilingArgs;
        private int tenant;
        private string objectTableId;
        private string userId;
        private Document document;


        public PODMobileDocumentsFilingService(PODMobileDocumentsFilingArgs podMobileDocumentsFilingArgs)
        {

            this.podMobileDocumentsFilingArgs = podMobileDocumentsFilingArgs;
            this.tenant = podMobileDocumentsFilingArgs.Tenant;
            InitializeRepository();
            LoadData();

        }


        private void LoadData()
        {
            objectTableId = objectTableRepository.GetObjectTableIdByName("Shipment");
            userId = contactRepository.GetConactIdByemail("system@tenant" + tenant.ToString() + ".com", tenant);
            document = documentRepository.GetSingleDocument(tenant, this.podMobileDocumentsFilingArgs.DocumentId);
        }

        private void InitializeRepository()
        {
            objectTableRepository = new ObjectTableRepository(tenant);
            contactRepository = new ContactRepository(tenant);
            documentRepository = new DocumentRepository(tenant);
        }

        public DocumentsFilingPM Create()
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
            DocumentsFilingService documentsService = new DocumentsFilingService(objectContext, tenant);
            DocumentsFilingPM extDocPM = GetNewInstanceFromDocumentsFilingPM();
            documentsService.Create(extDocPM, null, null, true);

            return extDocPM;
        }


        public DocumentsFilingPM GetNewInstanceFromDocumentsFilingPM()
        {
            return   new DocumentsFilingPM()
            {
                Id = IdCounter.GetNumber("DocumentsFiling", tenant).ToString(),
                DirectionCode = "I",
                Tenant = tenant,
                DocumentId = podMobileDocumentsFilingArgs.DocumentId,
                EntityId = podMobileDocumentsFilingArgs.ShipmentId,
                DocumentTypeId = podMobileDocumentsFilingArgs.DocumentTypeId,
                ObjectTableId = objectTableId,
                CreatedByUserId = userId,
                CreateDate = DateTime.Now,
                OwnerId = userId,
                UpdatedByUserId = userId,
                ExternalEntityName = "Shipment",
                EntityReference = podMobileDocumentsFilingArgs.ShipmentNumber,
                FileExtension = document.Extension,
                FileSize = document.FileSize,
                Folder = "docsin",
                HasFile = true,
                FileName = document.FileName,
                Received = true,
                ReceivedDate = DateTime.Now,
                ReceivedByUserId = userId,
                Notes = podMobileDocumentsFilingArgs.Note,
                Description = podMobileDocumentsFilingArgs.DocumentTypeName + " for file " + podMobileDocumentsFilingArgs.ShipmentNumber,
                IsFromUnifreightPodMobile = true,
            };
        }

    }


    public class PODMobileDocumentsFilingArgs
    {
        public string ShipmentNumber { get; set; }
        public string ContactId { get; set; }
        public string DocumentTypeName { get; set; }
        public string ObjectTableId { get; set; }
        public string DocumentTypeId { get; set; }
        public string ShipmentId { get; set; }
        public string UserId { get; set; }
        public string DocumentId { get; set; }
        public string Note { get; set; }
        public int Tenant { get; set; }

        
    }
}