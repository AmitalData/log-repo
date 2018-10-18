
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CouriersVatDataMapping: IMapping<CouriersVatPM, CouriersVat>
   {

        public void CustomPMToPOCO(CouriersVatPM entityPM, CouriersVat entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            entityPOCO.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.VatNumber;
            entityPM.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.VatNumber;
        }

        public void CustomPOCOToPM(CouriersVatPM entityPM, CouriersVat entityPOCO)
        {
            entityPM.SearchFields  = entityPOCO.EnglishName + "," + entityPOCO.LocalName + "," + entityPOCO.VatNumber;
        }

    }


}
   