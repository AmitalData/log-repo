
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
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class OccasionDataMapping: IMapping<OccasionPM, Occasion>
   {

        public void CustomPMToPOCO(OccasionPM entityPM, Occasion entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(OccasionPM entityPM, Occasion entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(OccasionPM entityPM, Occasion entityPOCO, bool p)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.Name))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            }

            if (!string.IsNullOrEmpty(entityPM.OwnerId))
            {
                Contact owner = ContactRepository.GetSingleContact(entityPM.OwnerId, entityPM.Tenant, true);
                if (owner != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, owner.EnglishName);
                }
            }

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   