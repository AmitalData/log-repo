
using System;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;
using Logitude.ShipmentOrderModule.Data.Repositories;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;

namespace Logitude.BL.CommonDataModel.Tools.HybridMapping
{
    public class DocumentsFilingHybridMapping
    {
        public static DocumentsFilingPM MapEntityToHybrid(DocumentsFilingPM originalPM)
        {
            byte[] serializedEntity = LogitudeXmlSerializer.SerializeObject(originalPM);
            DocumentsFilingPM documentsFilingPM = LogitudeXmlSerializer.DeserializeObject<DocumentsFilingPM>(serializedEntity);

            ICommonDataContext commonContext = CommonDataContext.GetContext(documentsFilingPM.Tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(documentsFilingPM.Tenant);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(webFreightContext);
            DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(commonContext);
            UserRepository userRepository = new UserRepository(commonContext);
            DepartmentRepository departmentRepository = new DepartmentRepository(commonContext);
            BranchRepository branchRepository = new BranchRepository(commonContext);
            
            if (!string.IsNullOrEmpty(documentsFilingPM.ObjectTableId))
            {
                ObjectTable table = objectTableRepository.GetSingleObjectTable(documentsFilingPM.ObjectTableId, 0, true);
                if (table != null)
                {
                    documentsFilingPM.ObjectTableId = table.Name;
                }
            }
            
            if (!string.IsNullOrEmpty(documentsFilingPM.ChildObjectTableId))
            {
                ObjectTable table = objectTableRepository.GetSingleObjectTable(documentsFilingPM.ChildObjectTableId, 0, true);
                if (table != null)
                {
                    documentsFilingPM.ChildObjectTableId = table.Name;
                }
            }

            if (!string.IsNullOrEmpty(documentsFilingPM.ChildObjectTableId))
            {
                ObjectTable table = objectTableRepository.GetSingleObjectTable(documentsFilingPM.ChildObjectTableId, 0, true);
                if (table != null)
                {
                    documentsFilingPM.ChildEntityName = table.Name;
                }
            }

            if (!string.IsNullOrEmpty(documentsFilingPM.DocumentTypeId))
            {
                DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(documentsFilingPM.DocumentTypeId, documentsFilingPM.Tenant);
                if (documentType != null)
                {
                    documentsFilingPM.DocumentTypeId = documentType.Code;
                }
            }

            if (!string.IsNullOrEmpty(documentsFilingPM.CreatedByUserId))
            {
                User user = userRepository.GetSingleUser(documentsFilingPM.CreatedByUserId, documentsFilingPM.Tenant, true);
                if (user != null)
                {
                    documentsFilingPM.CreatedByUserCode = user.Code;
                }
            }


            if (!string.IsNullOrEmpty(documentsFilingPM.OwnerId))
            {
                User user = userRepository.GetSingleUser(documentsFilingPM.OwnerId, documentsFilingPM.Tenant, true);
                if (user != null)
                {
                    documentsFilingPM.OwnerUserCode = user.Code;
                }
            }


            if (!string.IsNullOrEmpty(documentsFilingPM.ReceivedByUserId))
            {
                User user = userRepository.GetSingleUser(documentsFilingPM.ReceivedByUserId, documentsFilingPM.Tenant, true);
                if (user != null)
                {
                    documentsFilingPM.ReceivedByUserCode = user.Code;
                }
            }
            if (!string.IsNullOrEmpty(documentsFilingPM.UpdatedByUserId))
            {
                User user = userRepository.GetSingleUser(documentsFilingPM.UpdatedByUserId, documentsFilingPM.Tenant, true);
                if (user != null)
                {
                    documentsFilingPM.UpdatedByUserCode = user.Code;
                }
            }

            if (documentsFilingPM.DepartmentId != null)
            {
                Department department = departmentRepository.GetSingleDepartment(documentsFilingPM.DepartmentId, documentsFilingPM.Tenant);
                if (department != null && !string.IsNullOrEmpty(department.Code))
                {
                    documentsFilingPM.DepartmentId = department.Code;
                }

            }

            if (documentsFilingPM.BranchId != null)
            {
                Branch branch = branchRepository.GetSingleBranch(documentsFilingPM.BranchId, documentsFilingPM.Tenant);
                if (branch != null && !string.IsNullOrEmpty(branch.Code))
                {
                    documentsFilingPM.BranchId = branch.Code;
                }

            }

            if (!string.IsNullOrEmpty(documentsFilingPM.DeletedByUserId))
            {
                User user = userRepository.GetSingleUser(documentsFilingPM.DeletedByUserId, documentsFilingPM.Tenant, true);
                if (user != null)
                {
                    documentsFilingPM.ReceivedByUserCode = user.Code;
                }
            }


            foreach (DocumentsFilingMetaDataValuePM metadatavalue in documentsFilingPM.DocumentsFilingMetaDataValues)
            {
                metadatavalue.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                if (!string.IsNullOrEmpty(metadatavalue.DocumentsMetaDataTypeId))
                {
                    //DocumentType documentType = documentTypeRepository.GetSingleDocumentTypes(metadatavalue.DocumentsMetaDataTypeId, documentsFilingPM.Tenant);
                    DocumentsMetaDataType type = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataType(metadatavalue.DocumentsMetaDataTypeId, metadatavalue.Tenant);
                    if (type != null)
                    {
                        metadatavalue.DocumentsMetaDataTypeId = type.Code;
                    }
                }
            }

			if (LogitudeSettings.IsCostomsDeploy)
			{
				ICustomsDocumentQueryServiceExt customsDocumentQueryService = ContainerAccessor.Container.Resolve(typeof(ICustomsDocumentQueryServiceExt), "CustomsDocumentQueryServiceExt", new ParameterOverride("", 1)) as ICustomsDocumentQueryServiceExt;
				//CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
				CustomsDocumentPM customsDoc = customsDocumentQueryService.GetSingle(documentsFilingPM.Id, false, false, documentsFilingPM.Tenant);
				if (customsDoc != null &&
					!String.IsNullOrEmpty(customsDoc.CustomsDocId))
				{
					documentsFilingPM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValuePM()
					{
						DocumentsMetaDataTypeId = "CREF",
						Tenant = documentsFilingPM.Tenant,
						DocumentsFilingId = documentsFilingPM.Id,
						MetaDataValue = customsDoc.CustomsDocId,
					});
				}


				if (customsDoc != null)
				{
					documentsFilingPM.DocumentsFilingMetaDataValues.Add(new DocumentsFilingMetaDataValuePM()
					{
						DocumentsMetaDataTypeId = "DREL",
						Tenant = documentsFilingPM.Tenant,
						DocumentsFilingId = documentsFilingPM.Id,
						MetaDataValue = customsDoc.IsPartOfDeclaration.ToString(),
					});
				}
			}

            return documentsFilingPM;
        }

        public static Response MapEntityToLogitude(DocumentsFilingPM documentsFilingPM)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(documentsFilingPM.Tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(documentsFilingPM.Tenant);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(webFreightContext);
            DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(commonContext);
            UserRepository userRepository = new UserRepository(commonContext);
            DepartmentRepository departmentRepository = new DepartmentRepository(commonContext);
            BranchRepository branchRepository = new BranchRepository(commonContext);
            Response response = new Response();


            if (!string.IsNullOrEmpty(documentsFilingPM.ObjectTableId))
            {
                ObjectTable table = objectTableRepository.GetObjectTableByName(documentsFilingPM.ObjectTableId, 0, true);
                if (table != null)
                {
                    documentsFilingPM.ObjectTableId = table.Id;
                }
                else
                {
                    response.ValidationErrors.Add("ObjectTableId field doesn't exist in the database,Upsert this entity before using it.");
                }
            }

            if (!string.IsNullOrEmpty(documentsFilingPM.ChildObjectTableId))
            {
                ObjectTable table = objectTableRepository.GetObjectTableByName(documentsFilingPM.ChildObjectTableId, 0, true);
                if (table != null)
                {
                    documentsFilingPM.ChildObjectTableId = table.Id;
                }
                else
                {
                    response.ValidationErrors.Add("ChildObjectTableId field doesn't exist in the database,Upsert this entity before using it.");
                }
            }

            if (!string.IsNullOrEmpty(documentsFilingPM.DocumentTypeId))
            {
                DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode(documentsFilingPM.DocumentTypeId, documentsFilingPM.Tenant);
                if (documentType != null)
                {
                    documentsFilingPM.DocumentTypeId = documentType.Id;
                }
                else
                {
                    response.ValidationErrors.Add("DocumentTypeId field doesn't exist in the database,Upsert this entity before using it.");
                }
            }

            ValidateUpdatedByUser(documentsFilingPM, userRepository, response);

            if (!documentsFilingPM.IsAttachment)
            {
                if (!string.IsNullOrEmpty(documentsFilingPM.CreatedByUserId))
                {
                    User user = userRepository.GetSingleUserByCodeOrEmailForTenant(documentsFilingPM.CreatedByUserId, documentsFilingPM.CreatedByUserId, documentsFilingPM.Tenant, true);
                    if (user != null)
                    {
                        documentsFilingPM.CreatedByUserId = user.Id;
                    }
                    else
                    {
                        response.ValidationErrors.Add("CreatedByUserId field doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (!string.IsNullOrEmpty(documentsFilingPM.OwnerId))
                {
                    User user = userRepository.GetSingleUserByCodeOrEmailForTenant(documentsFilingPM.OwnerId, documentsFilingPM.OwnerId, documentsFilingPM.Tenant, true);
                    if (user != null)
                    {
                        documentsFilingPM.OwnerId = user.Id;
                    }
                    else
                    {
                        response.ValidationErrors.Add("OwnerId field doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (!string.IsNullOrEmpty(documentsFilingPM.ReceivedByUserId))
                {
                    User user = userRepository.GetSingleUserByCodeOrEmailForTenant(documentsFilingPM.ReceivedByUserId, documentsFilingPM.OwnerId, documentsFilingPM.Tenant, true);
                    if (user != null)
                    {
                        documentsFilingPM.ReceivedByUserId = user.Id;
                    }
                    else
                    {
                        response.ValidationErrors.Add("ReceivedByUserId field doesn't exist in the database,Upsert this entity before using it.");
                    }
                }

                if (!string.IsNullOrEmpty(documentsFilingPM.DeletedByUserId))
                {
                    User user = userRepository.GetSingleUserByCode(documentsFilingPM.DeletedByUserId, documentsFilingPM.Tenant, true);
                    if (user != null)
                    {
                        documentsFilingPM.DeletedByUserId = user.Id;
                    }
                    else
                    {
                        response.ValidationErrors.Add("DeletedByUserId field doesn't exist in the database,Upsert this entity before using it.");
                    }
                }
            }

            if (!string.IsNullOrEmpty(documentsFilingPM.ObjectTableName))
            {
                ObjectTable table = objectTableRepository.GetObjectTableByName(documentsFilingPM.ObjectTableName, 0, true);
                if (table != null)
                {
                    documentsFilingPM.ObjectTableId = table.Id;
                }
                else
                {
                    response.ValidationErrors.Add("ObjectTableName field doesn't exist in the database,Upsert this entity before using it.");
                }
            }

            if (documentsFilingPM.DepartmentId != null)
            {
                Department department = departmentRepository.GetSingleDepartmentByCode(documentsFilingPM.DepartmentId, documentsFilingPM.Tenant);
                if (department != null)
                {
                    documentsFilingPM.DepartmentId = department.Id;
                }

            }

            if (documentsFilingPM.BranchId != null)
            {
                Branch branch = branchRepository.GetSingleBranchByCode(documentsFilingPM.BranchId, documentsFilingPM.Tenant);
                if (branch != null)
                {
                    documentsFilingPM.BranchId = branch.Id;
                }

            }

            if (!string.IsNullOrEmpty(documentsFilingPM.EntityNumber) && !string.IsNullOrEmpty(documentsFilingPM.ObjectTableName))
            {
                string tablename = documentsFilingPM.ObjectTableName.ToLower();
                switch (tablename)
                {
                    case "shipment":
                        ShipmentRepository shipmentsRep = new ShipmentRepository(documentsFilingPM.Tenant);
                        Shipment shipment = shipmentsRep.GetSingleShipmentByShipmentNumber(documentsFilingPM.EntityNumber, documentsFilingPM.Tenant);
                        if (shipment != null)
                        {
                            documentsFilingPM.EntityId = shipment.Id;
                        }
                        break;
                    case "quote":
                        QuoteRepository quotesRep = new QuoteRepository(documentsFilingPM.Tenant);
                        Quote quote = quotesRep.GetSingleQuoteByNumber(documentsFilingPM.EntityNumber, documentsFilingPM.Tenant);
                        if (quote != null)
                        {
                            documentsFilingPM.EntityId = quote.Id;
                        }
                        break;

                    case "customs.declaration":
                        DeclarationRepository decRep = new DeclarationRepository(documentsFilingPM.Tenant);
                        Declaration dec = decRep.GetSingleDeclarationByNumber(documentsFilingPM.EntityNumber, documentsFilingPM.Tenant);
                        if (dec != null)
                        {
                            documentsFilingPM.EntityId = dec.Id;
                        }
                        break;
                    case "customer":
                        CardRepository cardsRep = new CardRepository(documentsFilingPM.Tenant);
                        Card card = cardsRep.GetSingleCardByCode(documentsFilingPM.EntityNumber, documentsFilingPM.Tenant, true);
                        if (card != null)
                        {
                            documentsFilingPM.EntityId = card.Id;
                        }
                        break;
                    case "shipmentorder":
                        ShipmentOrderRepository shipmentOrderRepository = new ShipmentOrderRepository(documentsFilingPM.Tenant);
                        ShipmentOrder shipmentOrder = shipmentOrderRepository.GetSingleByOrderNumber(documentsFilingPM.EntityNumber, documentsFilingPM.Tenant);
                        documentsFilingPM.EntityId = shipmentOrder?.Id;
                        break;
                }
            }

            foreach (DocumentsFilingMetaDataValuePM metadatavalue in documentsFilingPM.DocumentsFilingMetaDataValues)
            {
                metadatavalue.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                if (!string.IsNullOrEmpty(metadatavalue.DocumentsMetaDataTypeId))
                {
                    DocumentsMetaDataType type = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode(metadatavalue.DocumentsMetaDataTypeId, metadatavalue.Tenant);
                    if (type == null)
                    {
                        type = documentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode(metadatavalue.DocumentsMetaDataTypeId, metadatavalue.Tenant);
                    }

                    if (type != null)
                    {
                        metadatavalue.DocumentsMetaDataTypeCode= metadatavalue.DocumentsMetaDataTypeId;

                        metadatavalue.DocumentsMetaDataTypeId = type.Id;
                    }
                    else
                    {
                        response.ValidationErrors.Add("DocumentsMetaDataTypeId field doesn't exist in the database,Upsert this entity before using it.");
                        break;
                    }
                }
                else
                {
                    response.ValidationErrors.Add("DocumentsMetaDataTypeId field is required.");
                }
            }

            foreach (var error in response.ValidationErrors)
            {
                response.ErrorMessage += error + Environment.NewLine;
            }

            response.HasError = response.ValidationErrors.Count() > 0;

            return response;
        }

        private static void ValidateUpdatedByUser(DocumentsFilingPM documentsFilingPM, UserRepository userRepository, Response response)
        {
            if (string.IsNullOrEmpty(documentsFilingPM.UpdatedByUserId))
            {
                response.ValidationErrors.Add("UpdatedByUserId field is required.");
                return;
            }
            User user = userRepository.GetSingleUserByCode(documentsFilingPM.UpdatedByUserId, documentsFilingPM.Tenant, true);
            if (user != null)
            {
                documentsFilingPM.UpdatedByUserId = user.Id;
            }
            else
            {
                response.ValidationErrors.Add("UpdatedByUserId field doesn't exist in the database,Upsert this entity before using it.");
            }

        }
    }
}


   
  


      