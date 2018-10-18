
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
   
   public partial class ConsignmentPackageDataMapping: IMapping<ConsignmentPackagePM, ConsignmentPackage>
   {

        public void CustomPMToPOCO(ConsignmentPackagePM entityPM, ConsignmentPackage entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ConsignmentNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
             
                entityPOCO.DeclarationId = entityPM.DeclarationId;            
                entityPOCO.ConsignmentNumber = entityPM.ConsignmentNumber;             
                entityPOCO.LineNumber = entityPM.LineNumber;               
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(ConsignmentPackagePM entityPM, ConsignmentPackage entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.PackageMeasureQualifierName);
            CustomMappedPMProperties.Add(PMPropertyNames.PackageTypeName);
            if (entityPOCO.PackageTypeCode != null)
            {
                PackingTypeQueryService packingTypeQueryService=new PackingTypeQueryService(entityPOCO.Tenant);
                PackingTypePM packingType = packingTypeQueryService.GetSingle(entityPOCO.PackageTypeCode, false, true);
                entityPM.PackageTypeName = packingType.LocalName;
            }

            if (entityPOCO.PackageMeasureQualifierCode != null)
            {
                PackageMeasureQualifierQueryService packageMeasureQualifierQueryService = new PackageMeasureQualifierQueryService(entityPOCO.Tenant);
                PackageMeasureQualifierPM packageMeasureQualifier = packageMeasureQualifierQueryService.GetSingle(entityPOCO.PackageMeasureQualifierCode, false, true);
                entityPM.PackageMeasureQualifierName = packageMeasureQualifier.LocalName;
            }
        }
   }


}
   