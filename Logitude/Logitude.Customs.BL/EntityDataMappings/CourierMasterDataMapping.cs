
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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CourierMasterDataMapping: IMapping<CourierMasterPM, CourierMaster>
   {

        public void CustomPMToPOCO(CourierMasterPM entityPM, CourierMaster entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;

     
        }

        public void CustomPOCOToPM(CourierMasterPM entityPM, CourierMaster entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AirlinePrefix);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EstimatedArrivalDateOnly);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EstimatedArrivalTimeOnly);

            AirlineRepository rep = new AirlineRepository(entityPM.Tenant);
            UserRepository userRep = new UserRepository(entityPM.Tenant);
            Airline airline = rep.GetSingleAirline(entityPOCO.AirlineId, entityPOCO.Tenant);
            if (airline != null)
            {
                entityPM.AirlinePrefix = airline.Prefix;
          
            }

            User user = userRep.GetSingleUser(entityPM.CreatedByUserId, entityPM.Tenant);
            if(user != null)
            {
                entityPM.CreatedByUserName = user.Contact.LocalName != null ? user.Contact.LocalName : user.Contact.EnglishName;
            }

            User UpdatedByUser = userRep.GetSingleUser(entityPM.UpdatedByUserId, entityPM.Tenant);
            if (UpdatedByUser != null)
            {
                entityPM.UpdatedByUserName = UpdatedByUser.Contact.LocalName != null ? UpdatedByUser.Contact.LocalName : UpdatedByUser.Contact.EnglishName;
            }

            if (entityPOCO.EstimatedArrivalDate != null && entityPOCO.EstimatedArrivalDate.HasValue)
            {
                entityPM.EstimatedArrivalDateOnly = entityPOCO.EstimatedArrivalDate.Value.Date;
                entityPM.EstimatedArrivalTimeOnly = (DateTime)entityPOCO.EstimatedArrivalDate;
            }

        }

        private static void BuildSearchFields(CourierMasterPM entityPM, CourierMaster poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.MAWB))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.MAWB : result + "," + entityPM.MAWB;
            }

            if (!string.IsNullOrEmpty(entityPM.HAWB))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.HAWB : result + "," + entityPM.HAWB;
            }

            AirlineRepository rep = new AirlineRepository(entityPM.Tenant);

            Airline airline = rep.GetSingleAirline(poco.AirlineId, poco.Tenant);
            if (airline != null)
            {
                result = string.IsNullOrEmpty(result) ? airline.Prefix : result + "," + airline.Prefix;
            }

           
            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
    }


}
   