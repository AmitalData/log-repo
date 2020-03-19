
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
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class SLAHeaderDataMapping: IMapping<SLAHeaderPM, SLAHeader>
   {
        public void CustomPMToPOCO(SLAHeaderPM entityPM, SLAHeader entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(SLAHeaderPM entityPM, SLAHeader entityPOCO)
        {
            //throw new NotImplementedException();
        }

        private void BuildSearchFields(SLAHeaderPM entityPM, SLAHeader entityPOCO, bool p)
        {
            string result = entityPM.Name;
            entityPM.SearchFields = result;
            entityPOCO.SearchFields = result;
        }
    }
}
   