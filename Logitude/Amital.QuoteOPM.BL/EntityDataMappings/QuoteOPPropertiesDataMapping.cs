
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
using Logitude.Server.Tools.Counters;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{

    public partial class QuoteOPPropertiesDataMapping : IMapping<QuoteOPPropertiesPM, QuoteOPProperties>
    {

        public void CustomPMToPOCO(QuoteOPPropertiesPM entityPM, QuoteOPProperties entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.QuoteID);

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.IndexOrder);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.QuoteID = entityPM.QuoteID;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.IndexOrder = entityPM.IndexOrder;
            }
        }

        public void CustomPOCOToPM(QuoteOPPropertiesPM entityPM, QuoteOPProperties entityPOCO)
        {
            if(entityPOCO.FromPortId != null)
            {

            }
        }
    }


}
