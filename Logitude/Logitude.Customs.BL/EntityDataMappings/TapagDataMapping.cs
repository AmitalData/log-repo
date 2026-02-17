
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
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class TapagDataMapping: IMapping<TapagPM, Tapag>
   {

        public void CustomPMToPOCO(TapagPM entityPM, Tapag entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.TapagNumber);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.TapagNumber = entityPM.TapagNumber;
            }
        }

        public void CustomPOCOToPM(TapagPM entityPM, Tapag entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ImporterName);
            CustomMappedPMProperties.Add(PMPropertyNames.CustomerName);
            CustomMappedPMProperties.Add(PMPropertyNames.CustomsBranchName);
            CustomMappedPMProperties.Add(PMPropertyNames.ProfessionUnitTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.TapagTypeName);


            if (entityPOCO.ImporterId != null)
            {
                ClientQueryService clientQueryService = new ClientQueryService(entityPOCO.Tenant);
                ClientPM client = clientQueryService.GetSingle(entityPOCO.ImporterId, false, false);
                if (client != null)
                    entityPM.ImporterName = client.LocalFirstName;
            }

            if (entityPOCO.CustomerId != null)
            {
                CardRepository cardRepository = new CardRepository(entityPOCO.Tenant);
                Card card = cardRepository.GetSingleCard(entityPOCO.CustomerId, entityPOCO.Tenant);
                if (card != null)
                    entityPM.CustomerName = card.LocalName;
            }

            if (entityPOCO.CustomsBranchCode != null)
            {
                CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(entityPOCO.Tenant);
                CustomsHouseTypePM customsHouseType = customsHouseTypeQueryService.GetSingle(entityPOCO.CustomsBranchCode, false, false);
                if (customsHouseType != null)
                    entityPM.CustomsBranchName = customsHouseType.LocalName;
            }

            if (entityPOCO.ProfessionUnitTypeCode != null)
            {
                OrganizationUnitTypeQueryService organizationUnitTypeQueryService = new OrganizationUnitTypeQueryService(entityPOCO.Tenant);
                OrganizationUnitTypePM organizationUnitType = organizationUnitTypeQueryService.GetSingle(entityPOCO.ProfessionUnitTypeCode, false, false);
                if (organizationUnitType != null)
                    entityPM.ProfessionUnitTypeName = organizationUnitType.LocalName;
            }

            if (entityPOCO.TapagTypeCode != null)
            {
                TapagTypeQueryService tapagTypeQueryService = new TapagTypeQueryService(entityPOCO.Tenant);
                TapagTypePM tapagType = tapagTypeQueryService.GetSingle(entityPOCO.TapagTypeCode, false, false);
                if (tapagType != null)
                    entityPM.TapagTypeName = tapagType.LocalName;
            }

            if (!string.IsNullOrWhiteSpace(entityPOCO.ReferantId))
            {
                UserRepository rep = new UserRepository(entityPOCO.Tenant);
                User referantUser = rep.GetSingleUser(entityPOCO.ReferantId, entityPOCO.Tenant);
                if (referantUser != null)
                {
                    entityPM.ReferantName = referantUser.Contact.LocalName != null ? referantUser.Contact.LocalName : referantUser.Contact.EnglishName;
                }
            }

        }
   }


}
   