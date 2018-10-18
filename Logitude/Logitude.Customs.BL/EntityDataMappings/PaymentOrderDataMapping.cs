
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class PaymentOrderDataMapping: IMapping<PaymentOrderPM, PaymentOrder>
   {

        public void CustomPMToPOCO(PaymentOrderPM entityPM, PaymentOrder entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
               
                entityPOCO.Id = entityPM.Id;             
                entityPOCO.Tenant = entityPM.Tenant;
            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            entityPOCO.ConcurrencyGUID = entityPM.NewConcurrencyGUID;//Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;    
        }

        private void BuildSearchFields(PaymentOrderPM entityPM, PaymentOrder entityPOCO, bool isInsert)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.PaymentNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.PaymentNumber : result + "," + entityPM.PaymentNumber;
            }


            if (!string.IsNullOrEmpty(entityPM.FirstEntityID))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.FirstEntityID : result + "," + entityPM.FirstEntityID;
            }

            if (!string.IsNullOrEmpty(entityPM.SecondEntityID))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.SecondEntityID : result + "," + entityPM.SecondEntityID;
            }

            if (!string.IsNullOrEmpty(entityPM.ThirdEntityID))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ThirdEntityID : result + "," + entityPM.ThirdEntityID;
            }

            DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPOCO.Tenant);

            foreach (PaymentOrderConnectionTablePM connection in entityPM.PaymentOrderConnectionTables)
            {
                if (connection.ConnectedEntityCode == "D")
                {
                    DeclarationPM declaration = declarationQuery.GetSingle(connection.ConnectedEntityId, false, false);

                    //if (!string.IsNullOrEmpty(declaration.DeclarationNumber))
                    //{
                    //    result = string.IsNullOrEmpty(result) ? declaration.DeclarationNumber : result + "," + declaration.DeclarationNumber;
                    //}

                    if (!string.IsNullOrEmpty(declaration.CustomFileNo))
                    {
                        result = string.IsNullOrEmpty(result) ? declaration.CustomFileNo : result + "," + declaration.CustomFileNo;
                    }


                }
            }
            entityPM.SearchFields = result.ToLower();
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(PaymentOrderPM entityPM, PaymentOrder entityPOCO)
        {

            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerActivityTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomsEntityTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomsHouseName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PaymentOrderTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PaymentProcessName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PaymentStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.NewConcurrencyGUID);

            entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            CardRepository cardRepository = new CardRepository(entityPOCO.Tenant);
            Card card = cardRepository.GetSingleCard(entityPOCO.CustomerId, entityPOCO.Tenant);
            if (card != null)
            {
                entityPM.CustomerName = card.EnglishName;
            }


            

            if (entityPOCO.PaymentOrderTypeCode != null)
            {
                PaymentOrderTypeQueryService paymentOrderTypeQueryService = new PaymentOrderTypeQueryService(entityPOCO.Tenant);
                PaymentOrderTypePM paymentOrderType = paymentOrderTypeQueryService.GetSingle(entityPOCO.PaymentOrderTypeCode, false, true);
                entityPM.PaymentOrderTypeName = paymentOrderType.LocalName;

            }

            if (entityPOCO.PaymentStatusCode != null)
            {
                PaymentOrderStatusQueryService paymentOrderStatusQueryService = new PaymentOrderStatusQueryService(entityPOCO.Tenant);
                PaymentOrderStatusPM paymentOrderStatus = paymentOrderStatusQueryService.GetSingle(entityPOCO.PaymentStatusCode, false, true);
                entityPM.PaymentStatusName = paymentOrderStatus.LocalName;

            }


            if (entityPOCO.ImporterId != null)
            {
                ClientQueryService clientQueryService = new ClientQueryService(entityPOCO.Tenant);
                ClientPM client = clientQueryService.GetSingle(entityPOCO.ImporterId, false, true);
                entityPM.ImporterName = client.FullName;

            }

            if (entityPOCO.PaymentProcessCode != null)
            {
                PaymentProcessQueryService paymentProcessQueryService = new PaymentProcessQueryService(entityPOCO.Tenant);
                PaymentProcessPM paymentProcess = paymentProcessQueryService.GetSingle(entityPOCO.PaymentProcessCode, false, true);
                entityPM.PaymentProcessName = paymentProcess.LocalName;

            }


            if (entityPOCO.CustomerActivityTypeCode != null)
            {
                CustomerActivityTypeQueryService customerActivityTypeQueryService = new CustomerActivityTypeQueryService(entityPOCO.Tenant);
                CustomerActivityTypePM customerActivityType = customerActivityTypeQueryService.GetSingle(entityPOCO.CustomerActivityTypeCode, false, true);
                entityPM.CustomerActivityTypeName = customerActivityType.LocalName;

            }

            if (entityPOCO.CustomsEntityTypeCode != null)
            {
                EntityTypeLookupQueryService entityTypeLookupQueryService = new EntityTypeLookupQueryService(entityPOCO.Tenant);
                EntityTypeLookupPM entityTypeLookup = entityTypeLookupQueryService.GetSingle(entityPOCO.CustomsEntityTypeCode, false, true);
                entityPM.CustomsEntityTypeName = entityTypeLookup.LocalName;
            }

            if (entityPOCO.CustomsHouseCode != null)
            {
                CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(entityPOCO.Tenant);
                CustomsHouseTypePM customsHouseType = customsHouseTypeQueryService.GetSingle(entityPOCO.CustomsHouseCode, false, true);
                entityPM.CustomsHouseName = customsHouseType.LocalName;
            }

            DeficitQueryService deficitQueryService = new DeficitQueryService(entityPOCO.Tenant);
            DeficitPM deficit = deficitQueryService.GetDeficitByPaymentOrderNumberOrTapagId(entityPOCO.PaymentNumber, null, entityPOCO.Tenant);
            if (deficit != null)
            {
                entityPM.HasDeficit = true;
            }

            else if (entityPOCO.CustomsEntityTypeCode == "11122")
            {
                TapagQueryService tapagQueryService = new TapagQueryService(entityPOCO.Tenant);
                TapagPM tapag = tapagQueryService.GetSingleTapagByLeadingFileNumber(entityPOCO.FirstEntityID, entityPOCO.Tenant);
                if (tapag != null)
                {
                   deficit = deficitQueryService.GetDeficitByPaymentOrderNumberOrTapagId(null, tapag.Id, entityPOCO.Tenant);
                   if (deficit != null)
                   {
                       entityPM.HasDeficit = true;
                   }
                }
            }

            DepositQueryService depositQueryService = new DepositQueryService(entityPOCO.Tenant);
            DepositPM deposit = depositQueryService.GetDepositByPaymentOrderNumberOrTapagId(entityPOCO.PaymentNumber, null, entityPOCO.Tenant);
            if (deposit != null)
            {
                entityPM.HasDeposit = true;
            }

            entityPM.DocumentPaymentId = GetDocumentPaymentId(entityPOCO);

            entityPM.CustomerChanged = false;
        }

        private string GetDocumentPaymentId(PaymentOrder entityPOCO)
        {
            var objectTableRep = new ObjectTableRepository(entityPOCO.Tenant);

            var objectTable = objectTableRep.GetObjectTableByName("Customs.PaymentOrder", entityPOCO.Tenant, false);

            var documentRepository = new DocumentsFilingRepository(entityPOCO.Tenant);

            DocumentTypeRepository documentTypeRep = new DocumentTypeRepository(entityPOCO.Tenant);
            DocumentType type =
                //documentTypeRep.GetSingleDocumentTypeByCodeByObjectTableId("POR" ,objectTable.Id ,entityPOCO.Tenant);
                documentTypeRep.GetSingleDocumentTypeByCode("POR", entityPOCO.Tenant);
            if (type == null) return null;
            //Logitude.BL.CommonDataModel.EntityQueries.
            string DocumentsFilingId = null;
            var DocumentId = documentRepository.GetDocumentIdByDocumentType(type.Id, objectTable.Id, entityPOCO.Id, entityPOCO.Tenant, out DocumentsFilingId);
            if (!String.IsNullOrWhiteSpace(DocumentId))
            {
                return DocumentId;
            }
            DocumentId = documentRepository.GetDocumentIdByChild(type.Id, entityPOCO.PaymentNumber, entityPOCO.Tenant);
            return DocumentId;

        }
   }


}
   