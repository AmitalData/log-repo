
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsPartnersItemDataMapping: IMapping<CustomsPartnersItemPM, CustomsPartnersItem>
   {

        public void CustomPMToPOCO(CustomsPartnersItemPM entityPM, CustomsPartnersItem entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
             
            }
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        private static void BuildSearchFields(CustomsPartnersItemPM entityPM, CustomsPartnersItem poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.ItemCode))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ItemCode : result + "," + entityPM.ItemCode;
            }

            if (!string.IsNullOrEmpty(entityPM.ClassificationCode))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ClassificationCode : result + "," + entityPM.ClassificationCode;
            }

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.Name : result + "," + entityPM.Name;
            }

            CustomsVendorQueryService vendorQuery = new CustomsVendorQueryService(poco.Tenant);

            if (!string.IsNullOrEmpty(entityPM.VendorId))
            {
                CustomsVendorPM vendor = vendorQuery.GetSingle(entityPM.VendorId, false, false);
                result = string.IsNullOrEmpty(result) ? vendor.VendorName : result + "," + vendor.VendorName;
            }

            ClientQueryService clientQuery = new ClientQueryService(poco.Tenant);

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                ClientPM client = clientQuery.GetSingle(entityPM.CustomerId, false, false);
                result = string.IsNullOrEmpty(result) ? client.FullName : result + "," + client.FullName;
            }


            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(CustomsPartnersItemPM entityPM, CustomsPartnersItem entityPOCO)
        {
            entityPM.IsClassificationCodeValid = true;
        }
   }


}
   