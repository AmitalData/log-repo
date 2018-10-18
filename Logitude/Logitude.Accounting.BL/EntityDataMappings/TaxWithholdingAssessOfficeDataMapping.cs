
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class TaxWithholdingAssessOfficeDataMapping: IMapping<TaxWithholdingAssessOfficePM, TaxWithholdingAssessOffice>
   {

        public void CustomPMToPOCO(TaxWithholdingAssessOfficePM entityPM, TaxWithholdingAssessOffice entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Code);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Code = entityPM.Code;
                entityPOCO.Tenant = entityPM.Tenant;
            }



            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        private static void BuildSearchFields(TaxWithholdingAssessOfficePM entityPM, TaxWithholdingAssessOffice poco, bool isNewEntity)
        {
            string result = "";


            if (!string.IsNullOrEmpty(entityPM.Code))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.Code : result + "," + entityPM.Code;

            }
            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.Name : result + "," + entityPM.Name;

            }
            if (!string.IsNullOrEmpty(entityPM.LocalName))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.LocalName : result + "," + entityPM.LocalName;

            }
            entityPM.SearchFields = result;
            poco.SearchFields = result;

        }

        public void CustomPOCOToPM(TaxWithholdingAssessOfficePM entityPM, TaxWithholdingAssessOffice entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   