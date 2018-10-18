
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
   
   public partial class CustomsAirlineDataMapping: IMapping<CustomsAirlinePM, CustomsAirline>
   {

        public void CustomPMToPOCO(CustomsAirlinePM entityPM, CustomsAirline entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            entityPOCO.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.AirlineCode + "," + entityPM.AirlinePrefix;
            entityPM.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.AirlineCode + "," + entityPM.AirlinePrefix;
        }

        public void CustomPOCOToPM(CustomsAirlinePM entityPM, CustomsAirline entityPOCO)
        {
            entityPM.SearchFields = entityPOCO.EnglishName + "," + entityPOCO.LocalName + "," + entityPOCO.AirlineCode + "," + entityPOCO.AirlinePrefix;
        }
   }


}
   