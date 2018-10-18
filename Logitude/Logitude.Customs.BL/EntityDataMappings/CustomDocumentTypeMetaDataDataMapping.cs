
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
   
   public partial class CustomDocumentTypeMetaDataDataMapping: IMapping<CustomDocumentTypeMetaDataPM, CustomDocumentTypeMetaData>
   {

        public void CustomPMToPOCO(CustomDocumentTypeMetaDataPM entityPM, CustomDocumentTypeMetaData entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DocumentTypeCode);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.MetaDataTypeCode);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
              
                entityPOCO.DocumentTypeCode = entityPM.DocumentTypeCode;            
                entityPOCO.MetaDataTypeCode = entityPM.MetaDataTypeCode;

            }

           

        }

        public void CustomPOCOToPM(CustomDocumentTypeMetaDataPM entityPM, CustomDocumentTypeMetaData entityPOCO)
        {
           
        }
   }


}
   