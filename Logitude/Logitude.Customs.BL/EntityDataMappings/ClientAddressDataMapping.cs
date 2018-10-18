
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClientAddressDataMapping: IMapping<ClientAddressPM, ClientAddress>
   {

        public void CustomPMToPOCO(ClientAddressPM entityPM, ClientAddress entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ClientId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.AddressId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                
                entityPOCO.ClientId = entityPM.ClientId;             
                entityPOCO.AddressId = entityPM.AddressId;            
                entityPOCO.Tenant = entityPM.Tenant;


            }
        }

        public void CustomPOCOToPM(ClientAddressPM entityPM, ClientAddress entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.AddressPurposeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AddressTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactRoleTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactStateName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AuthorizedSignerPermit1Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AuthorizedSignerPermit2Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AuthorizedSignerPermit3Name);
            this.CustomMappedPMProperties.Add(PMPropertyNames.LocalCityName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EnglishCountryName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EnglishSubCountryName);

            if (entityPOCO.AddressPurposeCode != null)
            {
                AddressPurposeQueryService entityQueryService = new AddressPurposeQueryService(entityPOCO.Tenant);
                AddressPurposePM entity = entityQueryService.GetSingle(entityPOCO.AddressPurposeCode, false, true);
                entityPM.AddressPurposeName = entity.LocalName;
            }

            if (entityPOCO.AddressTypeCode != null)
            {
                CustomsAddressTypeQueryService entityQueryService = new CustomsAddressTypeQueryService(entityPOCO.Tenant);
                CustomsAddressTypePM entity = entityQueryService.GetSingle(entityPOCO.AddressTypeCode, false, true);
                entityPM.AddressTypeName = entity.LocalName;
            }

            if (entityPOCO.ContactRoleTypeCode != null)
            {
                ContactRoleTypeQueryService entityQueryService = new ContactRoleTypeQueryService(entityPOCO.Tenant);
                ContactRoleTypePM entity = entityQueryService.GetSingle(entityPOCO.ContactRoleTypeCode, false, true);
                entityPM.ContactRoleTypeName = entity.LocalName;
            }

            if (entityPOCO.ContactStateCode != null)
            {
                AddressContactStateQueryService entityQueryService = new AddressContactStateQueryService(entityPOCO.Tenant);
                AddressContactStatePM entity = entityQueryService.GetSingle(entityPOCO.ContactStateCode, false, true);
                entityPM.ContactStateName = entity.LocalName;
            }

            if (entityPOCO.AuthorizedSignerPermit1 != null)
            {
                AuthorizedSignerPermitQueryService entityQueryService = new AuthorizedSignerPermitQueryService(entityPOCO.Tenant);
                AuthorizedSignerPermitPM entity = entityQueryService.GetSingle(entityPOCO.AuthorizedSignerPermit1, false, true);
                entityPM.AuthorizedSignerPermit1Name = entity.LocalName;
            }

            if (entityPOCO.AuthorizedSignerPermit2 != null)
            {
                AuthorizedSignerPermitQueryService entityQueryService = new AuthorizedSignerPermitQueryService(entityPOCO.Tenant);
                AuthorizedSignerPermitPM entity = entityQueryService.GetSingle(entityPOCO.AuthorizedSignerPermit2, false, true);
                entityPM.AuthorizedSignerPermit2Name = entity.LocalName;
            }

            if (entityPOCO.AuthorizedSignerPermit3 != null)
            {
                AuthorizedSignerPermitQueryService entityQueryService = new AuthorizedSignerPermitQueryService(entityPOCO.Tenant);
                AuthorizedSignerPermitPM entity = entityQueryService.GetSingle(entityPOCO.AuthorizedSignerPermit3, false, true);
                entityPM.AuthorizedSignerPermit3Name = entity.LocalName;
            }

            if (entityPOCO.LocalCityCode != null)
            {
                CityQueryService entityQueryService = new CityQueryService(entityPOCO.Tenant);
                CityPM entity = entityQueryService.GetSingle(entityPOCO.LocalCityCode, false, true);
                entityPM.LocalCityName = entity.LocalName;
            }

            if (entityPOCO.EnglishCountryCode != null)
            {
                CustomsCountryQueryService entityQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM entity = entityQueryService.GetSingle(entityPOCO.EnglishCountryCode, false, true);
                entityPM.EnglishCountryName = entity.LocalName;
            }


            if (entityPOCO.EnglishSubCountryCode != null)
            {
                SubCountryQueryService entityQueryService = new SubCountryQueryService(entityPOCO.Tenant);
                SubCountryPM entity = entityQueryService.GetSingle(entityPOCO.EnglishSubCountryCode, false, true);
                entityPM.EnglishSubCountryName = entity.LocalName;
            }

        }
   }


}
   