
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
   
   public partial class DecCargoSplitConsPackDetDataMapping: IMapping<DecCargoSplitConsPackDetPM, DecCargoSplitConsPackDet>
   {

        public void CustomPMToPOCO(DecCargoSplitConsPackDetPM entityPM, DecCargoSplitConsPackDet entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationCargoSplitId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DecCargoSplitConsLineNo);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DecCargoSplitConsItemLine);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.PackageLine);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationCargoSplitId = entityPM.DeclarationCargoSplitId;
                entityPOCO.DecCargoSplitConsLineNo = entityPM.DecCargoSplitConsLineNo;
                entityPOCO.DecCargoSplitConsItemLine = entityPM.DecCargoSplitConsItemLine;
                entityPOCO.PackageLine = entityPM.PackageLine;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(DecCargoSplitConsPackDetPM entityPM, DecCargoSplitConsPackDet entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.PackageTypeName);
            if (entityPOCO.PackageTypeCode != null)
            {
                PackingTypeQueryService packingTypeQueryService = new PackingTypeQueryService(entityPOCO.Tenant);
                PackingTypePM packingType = packingTypeQueryService.GetSingle(entityPOCO.PackageTypeCode, false, true);
                entityPM.PackageTypeName = packingType.LocalName;
            }

        }
   }


}
   