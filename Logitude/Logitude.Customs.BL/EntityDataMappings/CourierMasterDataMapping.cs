
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
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;

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
            this.CustomMappedPMProperties.Add(PMPropertyNames.AirlineName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EstimatedArrivalDateOnly);
            this.CustomMappedPMProperties.Add(PMPropertyNames.EstimatedArrivalTimeOnly);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OriginPortName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.GatewayPortName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.WeightValueName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.StorageSiteName);

            CustomsAirlineRepository rep = new CustomsAirlineRepository(entityPM.Tenant);
            UserRepository userRep = new UserRepository(entityPM.Tenant);
            CustomsAirline customsAirline = rep.GetSingle(entityPOCO.AirlineId, entityPOCO.Tenant);
            if (customsAirline != null)
            {
                entityPM.AirlinePrefix = customsAirline.AirlinePrefix;
                entityPM.AirlineName = customsAirline.LocalName;
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

            if (entityPOCO.OriginPortCode != null)
            {
                InternationalSiteQueryService internationalSiteQueryService = new InternationalSiteQueryService(entityPOCO.Tenant);
                InternationalSitePM internationalSitePM = internationalSiteQueryService.GetSingle(entityPOCO.OriginPortCode, false, true);
                if(internationalSitePM != null)
                {
                    entityPM.OriginPortName = internationalSitePM.LocalName;
                }
            }

            if (entityPOCO.GatewayPortCode != null)
            {
                InternationalSiteQueryService internationalSiteQueryService = new InternationalSiteQueryService(entityPOCO.Tenant);
                InternationalSitePM internationalSitePM = internationalSiteQueryService.GetSingle(entityPOCO.GatewayPortCode, false, true);
                if (internationalSitePM != null)
                {
                    entityPM.GatewayPortName = internationalSitePM.LocalName;
                }
            }

            if (entityPOCO.WeightValueCode != null)
            {
                FreightPaymentMethodQueryService freightPaymentMethodQueryService = new FreightPaymentMethodQueryService(entityPOCO.Tenant);
                FreightPaymentMethodPM freightPaymentMethodPM = freightPaymentMethodQueryService.GetSingle(entityPOCO.WeightValueCode, false, true);
                if (freightPaymentMethodPM != null)
                {
                    entityPM.WeightValueName = freightPaymentMethodPM.LocalName;
                }
            }

            if (entityPOCO.StorageSiteCode != null)
            {
                DeliverySiteTypeQueryService deliverySiteTypeQueryService = new DeliverySiteTypeQueryService(entityPOCO.Tenant);
                DeliverySiteTypePM deliverySiteTypePM = deliverySiteTypeQueryService.GetSingle(entityPOCO.StorageSiteCode, false, true);
                if (deliverySiteTypePM != null)
                {
                    entityPM.StorageSiteName = deliverySiteTypePM.LocalName;
                }
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

            CustomsAirlineRepository rep = new CustomsAirlineRepository(entityPM.Tenant);

            CustomsAirline customsAirline = rep.GetSingle(poco.AirlineId, poco.Tenant);
            if (customsAirline != null)
            {
                result = string.IsNullOrEmpty(result) ? customsAirline.AirlinePrefix : result + "," + customsAirline.AirlinePrefix;
            }

           
            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
    }


}
   