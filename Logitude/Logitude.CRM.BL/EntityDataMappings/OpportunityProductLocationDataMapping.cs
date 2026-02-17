
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CRM.BL.EntityDataMappings
{

    public partial class OpportunityProductLocationDataMapping : IMapping<OpportunityProductLocationPM, OpportunityProductLocation>
    {
        public void CustomPMToPOCO(OpportunityProductLocationPM entityPM, OpportunityProductLocation entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.OpportunityId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OpportunityProductTypeCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.LineNumber);

            entityPOCO.OpportunityProductTypeCode = entityPM.OpportunityProductTypeCode;
            entityPOCO.OpportunityId = entityPM.OpportunityId;
            entityPOCO.LineNumber = entityPM.LineNumber;
        }

        public void CustomPOCOToPM(OpportunityProductLocationPM entityPM, OpportunityProductLocation entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.LocationCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.LocationName);

            CountryRepository countryRep = new CountryRepository(entityPOCO.Tenant);
            GlobalZoneRepository zoneRep = new GlobalZoneRepository(entityPOCO.Tenant);

            if (!string.IsNullOrEmpty(entityPOCO.CountryId))
            {
                Country country = countryRep.GetSingleCountry(entityPOCO.CountryId, entityPOCO.Tenant);
                if (country != null)
                {
                    entityPM.LocationCode = country.Code;
                    entityPM.LocationName = country.EnglishName;
                }
            }

            //else if (!string.IsNullOrEmpty(entityPOCO.GlobalZoneId))
            //{
            //    GlobalZone zone = zoneRep.GetSingleGlobalZone(entityPOCO.GlobalZoneId, entityPOCO.Tenant);
            //    if (zone != null)
            //    {
            //        entityPM.LocationCode = zone.Code + "_G";
            //        entityPM.LocationName = zone.EnglishName;
            //    }
            //}
        }
    }
}
   