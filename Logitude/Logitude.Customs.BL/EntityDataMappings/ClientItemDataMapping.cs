
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

    public partial class ClientItemDataMapping : IMapping<ClientItemPM, ClientItem>
    {

        public void CustomPMToPOCO(ClientItemPM entityPM, ClientItem entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.ClientCode = entityPM.ClientCode;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ItemCode = entityPM.ItemCode;
                entityPOCO.OriginCountryCode = entityPM.OriginCountryCode;
                entityPOCO.ClassificationCode = entityPM.ClassificationCode;
                entityPOCO.ItemDescription = entityPM.ItemDescription;
                entityPOCO.Id = entityPM.Id;
            }
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
        }

        private static void BuildSearchFields(ClientItemPM entityPM, ClientItem poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.ClassificationCode + "," + entityPM.ItemCode + "," + entityPM.ItemDescription;

            entityPM.SearchFields = result.ToLower(); ;
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(ClientItemPM entityPM, ClientItem entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.OriginCountryName);

            if (entityPOCO.OriginCountryCode != null)
            {
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(entityPOCO.OriginCountryCode, false, true);
                if (country != null)
                    entityPM.OriginCountryName = country.LocalName;
            }
        }
    }


}
