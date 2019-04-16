
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   
   public partial class TariffDataMapping: IMapping<TariffPM, Tariff>
   {


        public void CustomPMToPOCO(TariffPM entityPM, Tariff entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);

            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            //entityPOCO.ContractNumber = entityPM.ContractNumber;
            //entityPOCO.StartDate = entityPM.StartDate;
            //entityPOCO.ExpirationDate = entityPM.ExpirationDate;
            //entityPOCO.LastExpirationDate = entityPM.LastExpirationDate;
            //entityPOCO.InActive = entityPM.InActive;
            //entityPOCO.LastVersion = entityPM.LastVersion;
            //entityPOCO.Name = entityPM.Name;
            //entityPOCO.SellerId = entityPM.SellerId;
            //entityPOCO.TypeCode = entityPM.TypeCode;
            //entityPOCO.CreateDate = entityPM.CreateDate;
            //entityPOCO.UpdateDate = entityPM.UpdateDate;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);



        }

        public void CustomPOCOToPM(TariffPM entityPM, Tariff entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(TariffPM entityPM, Tariff entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }


}
   