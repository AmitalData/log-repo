
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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Customs.BL.EntityDataMappings
{

    public partial class DeclarationDataMapping : IMapping<DeclarationPM, Declaration>
    {
        public bool SuppressNewConcurrencyGUID { get;  set; }

        public void CustomPMToPOCO(DeclarationPM entityPM, Declaration entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CreateDateTime);
            entityPOCO.CreateDateTime = entityPM.CreateDateTime;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CreatedByUserId);
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.UpdateDateTime);
            entityPOCO.UpdateDateTime = entityPM.UpdateDateTime;


            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CustomerId);
            entityPOCO.CustomerId = entityPM.CustomerId;


            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CourierSearchFields);
            BuildCourierSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.CourierSearchFields = entityPM.CourierSearchFields;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);
            entityPOCO.ConcurrencyGUID = entityPM.NewConcurrencyGUID;//Guid.NewGuid().ToString();
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;            
        }

        public void CustomPOCOToPM(DeclarationPM entityPM, Declaration entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.DeclarationNumberandVersionId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.DeclarationOfficeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AutonomyRegionTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterEntitlementTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterPassCountryName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TransferImporterCountryName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ProcedureCurrentName);
            //this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.DepartmentName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.DeclarationDocumentTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EntitleImporterName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TransportModeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EntitleImporterCountryName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.NewConcurrencyGUID);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TransferImporterTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EntitleImporterTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerVatNo);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CourierData);
            this.CustomMappedPMProperties.Add(PMPropertyNames.WeightValueName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.StorageStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CourierCustomStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ManifestCargoStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CourierSuspentionReasonName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AcceptanceStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CourierSuspentionName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AmendmentStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AmendmentMessage);
            this.CustomMappedPMProperties.Add(PMPropertyNames.IsAmendmentDisplayOnly);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AutomaticPayment);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CancelRequestStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AmendmentRejectionReasonName);

            var amendmentStatusRepository = new AmendmentStatusQueryService(entityPOCO.Tenant);
            var amendmentStatus = amendmentStatusRepository.GetSingle(entityPOCO.AmendmentStatus,false,true);
            if (amendmentStatus != null)
            {
                entityPM.AmendmentStatusName = amendmentStatus.Name;

            }


           if(entityPOCO.IsAmendment == true)
            {
                AmendRequestRejectReasonTypeRepository amendRequestRejectReasonTypeRepository = new AmendRequestRejectReasonTypeRepository(entityPOCO.Tenant);
                AmendRequestRejectReasonType amendRequestRejectReasonType = amendRequestRejectReasonTypeRepository.GetSingle(entityPOCO.AmendmentRejectionReason);
                if (amendRequestRejectReasonType != null)
                {
                    entityPM.AmendmentRejectionReasonName = amendRequestRejectReasonType.LocalName;

                }
            }
          

            CancellationRequestStatusRepository cancellationRequestStatusRepository = new CancellationRequestStatusRepository(entityPOCO.Tenant);
            CancellationRequestStatus cancellationRequestStatus = cancellationRequestStatusRepository.GetSingle(entityPOCO.CancelRequestStatusCode);
            if (cancellationRequestStatus != null)
            {
                entityPM.CancelRequestStatusName = cancellationRequestStatus.LocalName;

            }

            DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPOCO.Tenant);


            if (entityPOCO.IsAmendment == true && entityPOCO.AmendmentStatus != "2" && entityPOCO.AmendmentStatus != null)
            {
                entityPM.AmendmentMessage = "לתצוגה בלבד - " + TranslateTextsClass.Translate("Customs.Declaration.O.IsAmendment", entityPOCO.Tenant, true) + ' ' + entityPM.AmendmentStatusName;
                entityPM.IsAmendmentDisplayOnly = true;
            }

            else if (entityPOCO.IsAmendment == true && entityPOCO.AmendmentStatus == "2")
            {
                entityPM.AmendmentMessage = TranslateTextsClass.Translate("Customs.Declaration.O.IsAmendment", entityPOCO.Tenant, true) + ' ' + entityPM.AmendmentStatusName;

            }

            else if (entityPOCO.IsAmendment != true)
            {
                var declarations = declarationQuery.GetDeclarationAmendmentsByIdCache(entityPOCO.Tenant, entityPOCO.Id);

                var declaration = declarations.FirstOrDefault(x => new string[] { "1",  "3", "6" }.Contains(x.AmendmentStatus));
                if (declaration != null)
                {
                    entityPM.AmendmentMessage = TranslateTextsClass.Translate("Customs.Declaration.O.ExistsAmendments", entityPOCO.Tenant, true) + ' ' + declaration.AmendmentStatusName;
                }
                //else
                //{
                //    entityPM.AmendmentMessage = TranslateTextsClass.Translate("Customs.Declaration.O.ExistsAmendments", entityPOCO.Tenant , true);
                //}
            }



            //CardRepository rep = new CardRepository(entityPM.Tenant);
            Card customerCard = CardRepository.GetSingleCard(entityPOCO.CustomerId, entityPOCO.Tenant, true);
            if (customerCard != null)
            {
                entityPM.CustomerName = customerCard.LocalName != null ? customerCard.LocalName : customerCard.EnglishName;
                entityPM.CustomerCode = customerCard.Code;
                entityPM.CustomerVatNo = customerCard.VatNumber;
            }

            var transportModeRep = new TransportModeRepository(entityPOCO.Tenant);
            TransportMode transportMode = transportModeRep.GetSingleTransportMode(entityPOCO.TransportModeId);
            if (transportMode != null)
            {
                entityPM.TransportModeName = transportMode.Name;

            }



            if (!string.IsNullOrEmpty(entityPOCO.DepartmentId))
            {
                DepartmentRepository departmentRep = new DepartmentRepository(entityPOCO.Tenant);
                Department department = departmentRep.GetSingleDepartmentCache(entityPOCO.DepartmentId, entityPOCO.Tenant);
                if (department != null)
                {
                    entityPM.DepartmentName = department.LocalName != null ? department.LocalName : department.EnglishName;
                }
            }

            if (entityPOCO.DeclarationNumber != null && entityPOCO.VersionId != null)
            {
                entityPM.DeclarationNumberandVersionId = entityPOCO.DeclarationNumber + "-" + entityPOCO.VersionId;
            }

            else if (entityPOCO.DeclarationNumber == null && entityPOCO.VersionId != null)
            {
                entityPM.DeclarationNumberandVersionId = entityPOCO.VersionId;
            }

            else if (entityPOCO.DeclarationNumber != null && entityPOCO.VersionId == null)
            {
                entityPM.DeclarationNumberandVersionId = entityPOCO.DeclarationNumber;
            }

            if (entityPOCO.DeclarationOfficeCode != null)
            {
                CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(entityPOCO.Tenant);
                CustomsHouseTypePM declarationOffice = customsHouseTypeQueryService.GetSingle(entityPOCO.DeclarationOfficeCode, false, true);
                entityPM.DeclarationOfficeName = declarationOffice.LocalName;
            }

            if (entityPOCO.AutonomyRegionTypeCode != null)
            {
                AutonomyTypeQueryService autonomyTypeQueryService = new AutonomyTypeQueryService(entityPOCO.Tenant);
                AutonomyTypePM autonomyType = autonomyTypeQueryService.GetSingle(entityPOCO.AutonomyRegionTypeCode, false, true);
                entityPM.AutonomyRegionTypeName = autonomyType.LocalName;
            }

            CustomerIdentifyTypeQueryService customerIdentifyTypeQueryService = new CustomerIdentifyTypeQueryService(entityPOCO.Tenant);
            if (entityPOCO.ImporterTypeCode != null)
            {

                CustomerIdentifyTypePM importerType = customerIdentifyTypeQueryService.GetSingle(entityPOCO.ImporterTypeCode, false, true);
                entityPM.ImporterTypeName = importerType != null ? importerType.LocalName : null;
            }


            if (entityPOCO.TransferImporterTypeCode != null)
            {

                CustomerIdentifyTypePM transferImporterType = customerIdentifyTypeQueryService.GetSingle(entityPOCO.TransferImporterTypeCode, false, true);
                entityPM.TransferImporterTypeName = transferImporterType != null ? transferImporterType.LocalName : null;
            }

            if (entityPOCO.EntitleImporterTypeCode != null)
            {

                CustomerIdentifyTypePM entitleImporterType = customerIdentifyTypeQueryService.GetSingle(entityPOCO.EntitleImporterTypeCode, false, true);
                entityPM.EntitleImporterTypeName = entitleImporterType != null ? entitleImporterType.LocalName : null;
            }



            if (entityPOCO.ImporterEntitlementTypeCode != null)
            {
                EntitlementTypeQueryService entitlementTypeQueryService = new EntitlementTypeQueryService(entityPOCO.Tenant);
                EntitlementTypePM entitlementType = entitlementTypeQueryService.GetSingle(entityPOCO.ImporterEntitlementTypeCode, false, true);
                entityPM.ImporterEntitlementTypeName = entitlementType.LocalName;
            }

            if (entityPOCO.ImporterPassCountryCode != null)
            {
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(entityPOCO.ImporterPassCountryCode, false, true);
                entityPM.ImporterPassCountryName = country.LocalName;
            }

            if (entityPOCO.TransferImporterCountryCode != null)
            {
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(entityPOCO.TransferImporterCountryCode, false, true);
                entityPM.ImporterPassCountryName = country.LocalName;
            }

            if (entityPOCO.ProcedureCurrentCode != null)
            {
                GovernmentProcedureTypeQueryService governmentProcedureTypeQueryService = new GovernmentProcedureTypeQueryService(entityPOCO.Tenant);
                GovernmentProcedureTypePM governmentProcedureType = governmentProcedureTypeQueryService.GetSingle(entityPOCO.ProcedureCurrentCode, false, true);
                entityPM.ProcedureCurrentName = governmentProcedureType.LocalName;
            }

            if (entityPOCO.ImporterCode != null)
            {
                ClientQueryService clientQueryService = new ClientQueryService(entityPOCO.Tenant);
                ClientPM client = clientQueryService.GetClientByCode(entityPOCO.ImporterCode, entityPOCO.Tenant);
                if (client != null)
                {

                    if (!string.IsNullOrEmpty(client.FullName)) entityPM.CalculatedImporterName = client.FullName.Substring(0, Math.Min(35, client.FullName.Length));
                    FacilitationTypeQueryService FacilitationTypeQueryService = new FacilitationTypeQueryService(entityPOCO.Tenant);
                    FacilitationTypePM FacilitationType = FacilitationTypeQueryService.GetSingle(client.FacilitationTypeCode, false, true);
                    entityPM.FacilityTypeName = FacilitationType != null ? FacilitationType.LocalName : null;
                }
            }

            if (entityPOCO.DeclarationStatusTypeCode != null || entityPOCO.CourierSuspentionCode != null)
            {
                DeclarationStatusTypeQueryService declarationStatusTypeQueryService = new DeclarationStatusTypeQueryService(entityPOCO.Tenant);
                if (entityPOCO.DeclarationStatusTypeCode != null)
                {
                    DeclarationStatusTypePM declarationStatusType = declarationStatusTypeQueryService.GetSingle(entityPOCO.DeclarationStatusTypeCode, false, true);
                    entityPM.DeclarationStatusTypeName = declarationStatusType.LocalName;
                }

                if (entityPOCO.CourierSuspentionCode != null)
                {
                    DeclarationStatusTypePM declarationStatusType = declarationStatusTypeQueryService.GetSingle(entityPOCO.CourierSuspentionCode, false, true);
                    entityPM.CourierSuspentionName = declarationStatusType.LocalName;
                }

            }

            if (entityPOCO.StorageStatusCode != null)
            {
                StorageStatusQueryService storageStatusQueryService = new StorageStatusQueryService(entityPOCO.Tenant);
                StorageStatusPM storageStatusPM = storageStatusQueryService.GetSingle(entityPOCO.StorageStatusCode, false, true);
                entityPM.StorageStatusName = storageStatusPM.LocalName;
            }


            //if (entityPOCO.AmendmentCorrectedByUserId != null)
            //{
            //    StorageStatusQueryService storageStatusQueryService = new StorageStatusQueryService(entityPOCO.Tenant);
            //     storageStatusPM = storageStatusQueryService.GetSingle(entityPOCO.AmendmentCorrectedByUserId, false, true);
            //    entityPM.StorageStatusName = storageStatusPM.LocalName;
            //}
            if (entityPOCO.EntitleImporterCode != null)
            {
                ClientQueryService clientQueryService = new ClientQueryService(entityPOCO.Tenant);
                ClientPM client = clientQueryService.GetClientByCode(entityPOCO.EntitleImporterCode, entityPOCO.Tenant);
                if (client != null)
                {
                    entityPM.CalculatedEntitleImporterName = client.FullName;
                }
            }

            if (entityPOCO.TransferImporterCode != null)
            {
                ClientQueryService clientQueryService = new ClientQueryService(entityPOCO.Tenant);
                ClientPM client = clientQueryService.GetClientByCode(entityPOCO.TransferImporterCode, entityPOCO.Tenant);
                if (client != null)
                {

                    entityPM.CalculatedTransferImporterName = client.FullName;
                }
            }

            if (entityPOCO.EntitleImporterCountryCode != null)
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM customsCountry = customsCountryQueryService.GetSingle(entityPOCO.EntitleImporterCountryCode, false, false);
                if (customsCountry != null)
                    entityPM.EntitleImporterCountryName = customsCountry.LocalName;
            }

            if (entityPOCO.StorageStatusCode != null)
            {
                StorageStatusQueryService storageStatusQueryService = new StorageStatusQueryService(entityPOCO.Tenant);
                StorageStatusPM storageStatusPM = storageStatusQueryService.GetSingle(entityPOCO.StorageStatusCode, false, false);
                if (storageStatusPM != null)
                    entityPM.StorageStatusName = storageStatusPM.LocalName;
            }
            if (this.SuppressNewConcurrencyGUID)
            {
                LogMessagingUtil.Instance.AppendLine("SuppressNewConcurrencyGUID");
                entityPM.NewConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }
            else
            {

                entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            }

            //var move2MenuButtonHandler = true;
            //if (!move2MenuButtonHandler)
            //{

            //    var myCustomsDocumentQueryService = new CustomsDocumentQueryService(entityPOCO.Tenant);
            //    entityPM.DocumentDeclarationId =
            //    myCustomsDocumentQueryService.GetDocumentDeclarationId(entityPOCO.Id, entityPOCO.Tenant);


            //}
            var supplierInvoiceRepo = new SupplierInvoiceRepository(entityPOCO.Tenant);
            entityPM.IsAccumulated = supplierInvoiceRepo.DeclarationIsAccumulated(entityPOCO.Id);


            CourierDeclarationQueryService courierDeclarationService = new CourierDeclarationQueryService(entityPOCO.Tenant);
            CourierDeclarationPM courierDeclaration = courierDeclarationService.GetCourierDeclarationByDeclarationId(entityPOCO.Id, entityPOCO.Tenant);
            if (courierDeclaration != null)
            {
                CourierMasterQueryService courierMasterService = new CourierMasterQueryService(entityPOCO.Tenant);
                CourierMasterPM courierMaster = courierMasterService.GetSingle(courierDeclaration.CourierMasterId, false, true);
                if (courierMaster != null)
                {
                    entityPM.CourierData = courierMaster.AirlinePrefix + "-" + courierMaster.MAWB;
                }
            }

            DeclarationPaymentQueryService declarationPaymentQueryService = new DeclarationPaymentQueryService(entityPOCO.Tenant);
            DeclarationPaymentPM declarationPaymentPM = declarationPaymentQueryService.GetSingle(entityPOCO.Id, false, false);
         if(declarationPaymentPM!=null)   entityPM.AutomaticPayment = declarationPaymentPM.AutomaticPayment;

            if (entityPOCO.WeightValue != null)
            {
                FreightPaymentMethodQueryService freightPaymentMethodQueryService = new FreightPaymentMethodQueryService(entityPOCO.Tenant);
                FreightPaymentMethodPM freightPaymentMethodPM = freightPaymentMethodQueryService.GetSingle(entityPOCO.WeightValue, false, true);
                entityPM.WeightValueName = freightPaymentMethodPM.LocalName;
            }

            if (entityPOCO.CourierCustomStatusCode != null)
            {
                CourierCustomStatusQueryService courierCustomStatusQueryService = new CourierCustomStatusQueryService(entityPOCO.Tenant);
                CourierCustomStatusPM courierCustomStatus = courierCustomStatusQueryService.GetSingle(entityPOCO.CourierCustomStatusCode, false, true);
                if (courierCustomStatus != null)
                {
                    entityPM.CourierCustomStatusName = courierCustomStatus.LocalName;
                }
            }

            if (entityPOCO.ManifestCargoStatusCode != null)
            {
                ManifestCargoStatusQueryService manifestCargoStatusQueryService = new ManifestCargoStatusQueryService(entityPOCO.Tenant);
                ManifestCargoStatusPM manifestCargoStatusPM = manifestCargoStatusQueryService.GetSingle(entityPOCO.ManifestCargoStatusCode, false, true);
                if (manifestCargoStatusPM != null)
                {
                    entityPM.ManifestCargoStatusName = manifestCargoStatusPM.LocalName;
                }
            }

            if (entityPOCO.CourierSuspentionReasonCode != null)
            {
                AgentTalkBackTypeQueryService agentTalkBackTypeQueryService = new AgentTalkBackTypeQueryService(entityPOCO.Tenant);
                AgentTalkBackTypePM agentTalkBackTypePM = agentTalkBackTypeQueryService.GetSingle(entityPOCO.CourierSuspentionReasonCode, false, true);
                if (agentTalkBackTypePM != null)
                {
                    entityPM.CourierSuspentionReasonName = agentTalkBackTypePM.LocalName;
                }
            }

            UpdateCourierDeclarationFields(entityPM, entityPOCO);

            if (entityPOCO.AcceptanceStatusCode != null)
            {
                AcceptanceStatusQueryService acceptanceStatusQueryService = new AcceptanceStatusQueryService(entityPOCO.Tenant);
                AcceptanceStatusPM acceptanceStatus = acceptanceStatusQueryService.GetSingle(entityPOCO.AcceptanceStatusCode, false, true);
                if (acceptanceStatus != null)
                {
                    entityPM.AcceptanceStatusName = acceptanceStatus.LocalName;
                }
            }

            if (entityPM.CourierPendingReasonList != null)
            {

                entityPM.CourierPendingReasonList = getCourierPendingReasonName(entityPM);
            }

            if (entityPM.FastIndividualProcessCode != null)
            {
                if (entityPM.FastIndividualProcessCode == "F")
                {
                    entityPM.FastIndividualProcessCode = "מהיר";
                }
                else if (entityPM.FastIndividualProcessCode == "I")
                {
                    entityPM.FastIndividualProcessCode = "פרטני";
                }
            }

        }

        public static void UpdateCourierDeclarationFields(DeclarationPM entityPM, Declaration entityPOCO)
        {
            if (entityPOCO.IsCourierDeclaration == true)
            {
                //CourierDeclarationQueryService courierDeclarationQueryService = new CourierDeclarationQueryService(entityPOCO.Tenant);
                //entityPM.MAWBCourierMaster = courierDeclarationQueryService.GetMAWBCourierMasterByDeclarationId(entityPOCO.Id, entityPOCO.Tenant);

                CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(entityPOCO.Tenant);
                CourierMasterPM courierMasterPM = courierMasterQueryService.GetByDeclarationIdCache(entityPOCO.Id, entityPOCO.Tenant);
                if (courierMasterPM != null)
                {
                    entityPM.CourierMasterId = courierMasterPM.Id;
                    entityPM.MAWBCourierMaster = courierMasterPM.MAWB;
                }
                bool fastWithoutCache_NotNeedName = true;
                if (fastWithoutCache_NotNeedName)
                {
                    var repoDeclarationCourierStatus = new DeclarationCourierStatusRepository(entityPOCO.Tenant);
                    var pocoDeclarationCourierStatus = repoDeclarationCourierStatus
                        .GetDeclarationsByIds(new List<string>() { entityPOCO.Id }, entityPOCO.Tenant)
                        .FirstOrDefault();
                    if (pocoDeclarationCourierStatus != null)
                    {
                        entityPM.CourierManifestStatusCode = pocoDeclarationCourierStatus.CourierManifestStatusCode;
                        entityPM.CourierPaymentStatusCode = pocoDeclarationCourierStatus.CourierPaymentStatusCode;

                    }
                }

            }
        }

        private static void BuildSearchFields(DeclarationPM entityPM, Declaration poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.DeclarationNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.DeclarationNumber : result + "," + entityPM.DeclarationNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.CustomFileNo))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CustomFileNo : result + "," + entityPM.CustomFileNo;
            }

            ClientRepository clientRepository = new ClientRepository(entityPM.Tenant);
            ClientKeys clientKeys = new ClientKeys() { Id = entityPM.ImporterId };
            Client client = clientRepository.GetSingle(clientKeys);
            if (client != null)
            {
                result = string.IsNullOrEmpty(result) ? client.FullName : result + "," + client.FullName;
            }

         //   if (isNewEntity)
        //    {
                foreach (ConsignmentPM item in entityPM.Consignments)
                {
                    if (!string.IsNullOrEmpty(item.ManifestNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? item.ManifestNumber : result + "," + item.ManifestNumber;
                    }

                    if (!string.IsNullOrEmpty(item.SecondCargoID))
                    {
                        result = string.IsNullOrEmpty(result) ? item.SecondCargoID : result + "," + item.SecondCargoID;
                    }

                    if (!string.IsNullOrEmpty(item.ThirdCargoID))
                    {
                        result = string.IsNullOrEmpty(result) ? item.ThirdCargoID : result + "," + item.ThirdCargoID;
                    }
                }
            //   }

            //    else
            //{
            //    ConsignmentRepository consignmentRepository = new ConsignmentRepository(entityPM.Tenant);
            //    DeclarationKeys entityKeys = new DeclarationKeys() { Id = entityPM.Id };
            //    List<Consignment> consignments = consignmentRepository.GetMulti(entityKeys);
            //    foreach (Consignment item in consignments)
            //    {
            //        if (!string.IsNullOrEmpty(item.ManifestNumber))
            //        {
            //            result = string.IsNullOrEmpty(result) ? item.ManifestNumber : result + "," + item.ManifestNumber;
            //        }

            //        if (!string.IsNullOrEmpty(item.SecondCargoID))
            //        {
            //            result = string.IsNullOrEmpty(result) ? item.SecondCargoID : result + "," + item.SecondCargoID;
            //        }

            //        if (!string.IsNullOrEmpty(item.ThirdCargoID))
            //        {
            //            result = string.IsNullOrEmpty(result) ? item.ThirdCargoID : result + "," + item.ThirdCargoID;
            //        }
            //    }
            // }
            if (entityPM.Direction == "E")
            {
                if (!string.IsNullOrEmpty(entityPM.ExportFile))
                {
                    result = string.IsNullOrEmpty(result) ? entityPM.ExportFile : result + "," + entityPM.ExportFile;
                }
            }
            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

        private static void BuildCourierSearchFields(DeclarationPM entityPM, Declaration poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.DeclarationNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.DeclarationNumber : result + "," + entityPM.DeclarationNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.CustomFileNo))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CustomFileNo : result + "," + entityPM.CustomFileNo;
            }

            if (!string.IsNullOrEmpty(entityPM.CourierHAWB))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.CourierHAWB : result + "," + entityPM.CourierHAWB;
            }

            if (!string.IsNullOrEmpty(entityPM.ImporterId))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ImporterId : result + "," + entityPM.ImporterId;

                ClientRepository clientRepository = new ClientRepository(entityPM.Tenant);
                ClientKeys clientKeys = new ClientKeys() { Id = entityPM.ImporterId };
                Client client = clientRepository.GetSingle(clientKeys);
                if (client != null)
                {
                    result = string.IsNullOrEmpty(result) ? client.FullName : result + "," + client.FullName;
                }
            }
            else if(!string.IsNullOrEmpty(entityPM.ImporterName))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ImporterName : result + "," + entityPM.ImporterName;
            }


            Card customerCard = CardRepository.GetSingleCard(entityPM.CustomerId, entityPM.Tenant, true);
            if (customerCard != null)
            {
                var customerName = customerCard.LocalName != null ? customerCard.LocalName : customerCard.EnglishName;
                result = string.IsNullOrEmpty(result) ? customerName : result + "," + customerName;
            }

            entityPM.CourierSearchFields = result.ToLower();
            poco.CourierSearchFields = entityPM.CourierSearchFields;
        }

        private string getCourierPendingReasonName(DeclarationPM entityPM)
        {
            var courierPendingReasonList = entityPM.CourierPendingReasonList;
            if (!string.IsNullOrWhiteSpace(courierPendingReasonList))
            {
                if (courierPendingReasonList.Contains(","))
                {
                    courierPendingReasonList = "רשימה";
                }
                else
                {
                    CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(entityPM.Tenant);
                    CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle(courierPendingReasonList, false, false);
                    courierPendingReasonList = courierPendingReasonPM.LocalName;
                }
            }
            return courierPendingReasonList;
        }
    }

}
   