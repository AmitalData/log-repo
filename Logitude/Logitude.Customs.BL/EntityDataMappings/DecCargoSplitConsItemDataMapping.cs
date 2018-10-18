
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
   
   public partial class DecCargoSplitConsItemDataMapping: IMapping<DecCargoSplitConsItemPM, DecCargoSplitConsItem>
   {

        public void CustomPMToPOCO(DecCargoSplitConsItemPM entityPM, DecCargoSplitConsItem entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationCargoSplitId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DecCargoSplitConsLineNo);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ItemLine);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationCargoSplitId = entityPM.DeclarationCargoSplitId;
                entityPOCO.DecCargoSplitConsLineNo = entityPM.DecCargoSplitConsLineNo;
                entityPOCO.ItemLine = entityPM.ItemLine;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(DecCargoSplitConsItemPM entityPM, DecCargoSplitConsItem entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.RequestReasonName);
            if (entityPOCO.RequestReasonCode != null)
            {
                SplitOrMergeReasonQueryService splitOrMergeReasonQueryService = new SplitOrMergeReasonQueryService(entityPOCO.Tenant);
                SplitOrMergeReasonPM splitOrMergeReason = splitOrMergeReasonQueryService.GetSingle(entityPOCO.RequestReasonCode, false, true);
                entityPM.RequestReasonName = splitOrMergeReason.LocalName;
            }

        }
   }


}
   