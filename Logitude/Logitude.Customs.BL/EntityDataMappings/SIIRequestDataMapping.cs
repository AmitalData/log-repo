
using System;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.Data.Repsitories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using static Logitude.Customs.Data.Repsitories.SIIRequestRepository;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class SIIRequestDataMapping: IMapping<SIIRequestPM, SIIRequest>
   {
        
        public void CustomPMToPOCO(SIIRequestPM entityPM, SIIRequest entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.RequestDate);
                entityPOCO.RequestDate = DateTime.Now;
            }
           
                
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;


            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            entityPOCO.DeclarationId = entityPM.DeclarationId;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = !string.IsNullOrEmpty(entityPM.SearchFields) && entityPM.SearchFields.Length >= 1000 ? entityPM.SearchFields.Substring(0, 999) : entityPM.SearchFields;

        }



        public void CustomPOCOToPM(SIIRequestPM entityPM, SIIRequest entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VesselName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ManifestNumber);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UnloadDate);
            this.CustomMappedPMProperties.Add(PMPropertyNames.WareHouseCityName);


            var siiRepo = new SIIRequestRepository(entityPOCO.Tenant);
            var defaultValueQueryService = new DefaultValueQueryService(entityPOCO.Tenant);
            var contactQuery = new ContactQuery(entityPOCO.Tenant);

            var agg = siiRepo.GetAggregateForSii(entityPOCO.Tenant, entityPOCO.DeclarationId);
            if (agg == null) return;              
            
            MapAggregateToPM(entityPM, agg);
            MapContact(entityPM, entityPOCO, agg, defaultValueQueryService, contactQuery);

            if (!string.IsNullOrWhiteSpace(entityPOCO.WareHouseCity))
            {
                var cityQueryService = new CityQueryService(entityPOCO.Tenant);
                var city = cityQueryService.GetSingle(entityPOCO.WareHouseCity, false, true);
                if (city != null)
                {
                    entityPM.WareHouseCityName = city.LocalName;
                }
            }
        }


        private static void MapAggregateToPM(SIIRequestPM entityPM, SiiAgg agg)
        {
            entityPM.ImporterId = string.IsNullOrEmpty(agg.ImporterInternalId)
                                       ? agg.ImporterCode
                                       : agg.ImporterInternalId;

            entityPM.VesselName = agg.VesselLocalName;
            entityPM.ManifestNumber = agg.ManifestNumber;
            entityPM.OriginCountryCode = agg.OriginCountryCode;
            entityPM.UnloadPortCode = agg.UnloadPortCode;

            if (agg.UnloadDate.HasValue)
                entityPM.UnloadDate = agg.UnloadDate.Value;
        }

 
        private static void MapContact(
            SIIRequestPM entityPM,
            SIIRequest entityPOCO,
            dynamic agg,
            DefaultValueQueryService defaultValueQueryService,
            ContactQuery contactQuery)
        {
            ContactPM contactPM = null;

            if (entityPOCO.ContactId == null && agg.CustomerId != null)
            {
                var customerCard = CardRepository.GetSingleCard(agg.CustomerId, entityPOCO.Tenant, true);
                if (customerCard != null)
                {
                    var defaultContactKey = defaultValueQueryService.GetDefault(
                                                 "ISRAEL",
                                                 "CGG_CONT_STDI",
                                                 "NON",
                                                 customerCard.Code,
                                                 entityPOCO.Tenant);

                    if (!string.IsNullOrEmpty(defaultContactKey))
                    {
                        contactPM = contactQuery.GetSingleContactByExternalId(
                                        defaultContactKey,
                                        entityPOCO.Tenant);
                    }
                }
            }
            else if (entityPOCO.ContactId != null)
            {
                contactPM = contactQuery.GetSinglePMFromCache(
                                entityPOCO.ContactId,
                                entityPOCO.Tenant);
            }

            if (contactPM == null) return;

            entityPM.ContactName = contactPM.LocalName ?? contactPM.EnglishName;
            entityPM.ContactEmail = contactPM.Email;
            entityPM.ContactCellPhone = contactPM.Mobile;
            entityPM.ContactFax = contactPM.Fax;
            entityPM.ContactTel = contactPM.BusinessPhone;
            entityPM.ContactId = contactPM.Id;
        }


        private void BuildSearchFields(SIIRequestPM entityPM, SIIRequest entityPOCO, bool v)
        {
        }
    }


}
   