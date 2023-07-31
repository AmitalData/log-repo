
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
   
   public partial class OcrDocumentDataMapping: IMapping<OcrDocumentPM, OcrDocument>
   {

        public void CustomPMToPOCO(OcrDocumentPM entityPM, OcrDocument entityPOCO)
        {
            //throw new NotImplementedException();

            if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Id))
            {
                entityPOCO.Id = entityPM.Id;
            }
        }

        public void CustomPOCOToPM(OcrDocumentPM entityPM, OcrDocument entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   