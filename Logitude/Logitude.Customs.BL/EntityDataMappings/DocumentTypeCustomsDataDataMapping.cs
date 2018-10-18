
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
   
   public partial class DocumentTypeCustomsDataDataMapping: IMapping<DocumentTypeCustomsDataPM, DocumentTypeCustomsData>
   {

        public void CustomPMToPOCO(DocumentTypeCustomsDataPM entityPM, DocumentTypeCustomsData entityPOCO)
        {
            //throw new NotImplementedException();
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DocumentTypeId);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DocumentTypeId = entityPM.DocumentTypeId;
                //entityPOCO.Tenant = entityPM.Tenant;



            }


            
        }

        public void CustomPOCOToPM(DocumentTypeCustomsDataPM entityPM, DocumentTypeCustomsData entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   