using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;

namespace CommunicationWorkerRole.Services.ShipmentOrderDocuments
{
    public class ShipmentOrderDocumentsFilingAMMap
    {
        private const string shipmentOrderCodePrefix = "SO";
        private readonly int tenant;
        private readonly ObjectTableRepository objectTableRepository;
        private readonly ShipmentOrderQueryService shipmentOrderQueryService;
        public ShipmentOrderDocumentsFilingAMMap(int tenant, ObjectTableRepository objectTableRepository)
        {
            this.tenant = tenant;
            this.objectTableRepository = objectTableRepository;
            shipmentOrderQueryService = new ShipmentOrderQueryService(tenant);
        }

        public DocumentsFilingAM Map(DocumentsFilingPM documentsFiling, string shipmentOrderId)
        {
            var shipmentOrder = shipmentOrderQueryService.GetSinglePM(shipmentOrderId, tenant);
            var objectTable = objectTableRepository.GetSingleObjectTable(documentsFiling.ObjectTableId, tenant, true);
            BlobFileInfo fileInfo = BuildFileInfo(documentsFiling);
            var datainByte = GetDataInByte(documentsFiling, fileInfo);


            DocumentsFilingAM documentsFilingAM = new DocumentsFilingAM();
            documentsFilingAM.ImporterTenant = shipmentOrder.CustomerTenantNumber.Value;
            documentsFilingAM.EntityNumber = shipmentOrder.CustomerShipmentNumber;
            documentsFilingAM.DocumentType = new CodeProperties() { Code = GetDocumentTypeCode(documentsFiling) };
            documentsFilingAM.Description = documentsFiling.Description;
            documentsFilingAM.FileSize = datainByte != null ? datainByte.Length : 0;
            documentsFilingAM.ObjectTableName = objectTable.Name;
            documentsFilingAM.Extension = documentsFiling.FileExtension;
            documentsFilingAM.IsDigitallySigned = documentsFiling.IsDigitallySigned;
            documentsFilingAM.SignersList = documentsFiling.SignersList;
            documentsFilingAM.FileName = documentsFiling.FileName;
            documentsFilingAM.IsSharedWithCustomer = documentsFiling.IsSharedWithCustomer;
            documentsFilingAM.IsDeleted = documentsFiling.IsDeleted;
            documentsFilingAM.Code = documentsFiling.Code;
            documentsFilingAM.ExternalCode = documentsFiling.Code;
            documentsFilingAM.IsRequested = documentsFiling.IsRequested;
            documentsFilingAM.Tenant = documentsFiling.Tenant;
            documentsFilingAM.Notes = documentsFiling.Notes;
            documentsFilingAM.FileInfo = GetFileInformation(fileInfo, datainByte, shipmentOrder);
            documentsFilingAM.FileData = datainByte;
            documentsFilingAM.DocumentId = documentsFiling.DocumentId;
            return documentsFilingAM;
        }

        private FileInformation GetFileInformation(BlobFileInfo fileInfo, byte[] datainByte, ShipmentOrderPM shipmentOrder)
        {
            var fileInformation = new FileInformation();
            fileInformation.FileName = fileInfo.FileName + "." + fileInfo.Extension;
            fileInformation.FileSize = datainByte.Length;
            fileInformation.Tenant = shipmentOrder.CustomerTenantNumber.Value;
            return fileInformation;
        }

        private BlobFileInfo BuildFileInfo(DocumentsFilingPM documentsFiling)
        {
            return new BlobFileInfo()
            {
                FileName = documentsFiling.DocumentId,
                FolderName = documentsFiling.Folder,
                Extension = documentsFiling.FileExtension,
                Tenant = tenant,
                FileSize = documentsFiling.FileSize,
            };
        }

        private byte[] GetDataInByte(DocumentsFilingPM documentFilingPM, BlobFileInfo fileInfo)
        {
            if (!documentFilingPM.HasFile) return null;
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            byte[] datainByte = storageservice.Read(fileInfo);
            if (datainByte == null) throw new Exception("The physical file for this Document may be Damaged or not exists. ");
            return datainByte;
        }

        private static string GetDocumentTypeCode(DocumentsFilingPM documentsFiling)
        {
            return documentsFiling.DocumentTypeCode.StartsWith(shipmentOrderCodePrefix) ? documentsFiling.DocumentTypeCode.Replace(shipmentOrderCodePrefix, "") : documentsFiling.DocumentTypeCode;
        }
    }

}

