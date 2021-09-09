
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPDocumentVersionDataMapping: IMapping<QuoteOPDocumentVersionPM, QuoteOPDocumentVersion>
   {

        public void CustomPMToPOCO(QuoteOPDocumentVersionPM entityPM, QuoteOPDocumentVersion entityPOCO)
        {
            //throw new NotImplementedException();
            
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.QuoteOPId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {


                entityPOCO.QuoteOPId = entityPM.QuoteOPId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(QuoteOPDocumentVersionPM entityPM, QuoteOPDocumentVersion entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   