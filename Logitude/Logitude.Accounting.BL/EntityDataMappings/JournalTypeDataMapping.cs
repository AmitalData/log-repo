
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
   
   public partial class JournalTypeDataMapping: IMapping<JournalTypePM, JournalType>
   {

        public void CustomPMToPOCO(JournalTypePM entityPM, JournalType entityPOCO)
        {
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.EnglishName);
            //entityPOCO.EnglishName = entityPM.EnglishName;

            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Inactive);
            //entityPOCO.Inactive = entityPM.Inactive;

            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.JournalTypeID);
            //entityPOCO.JournalTypeID = entityPM.JournalTypeID;

            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LocalName);
            //entityPOCO.LocalName = entityPM.LocalName;


            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            //BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(JournalTypePM entityPM, JournalType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   