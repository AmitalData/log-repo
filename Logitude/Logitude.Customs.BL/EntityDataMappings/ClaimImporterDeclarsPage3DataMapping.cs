
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
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClaimImporterDeclarsPage3DataMapping: IMapping<ClaimImporterDeclarsPage3PM, ClaimImporterDeclarsPage3>
   {

        public void CustomPMToPOCO(ClaimImporterDeclarsPage3PM entityPM, ClaimImporterDeclarsPage3 entityPOCO)
        {
            //throw new NotImplementedException();
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ClaimId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNo);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                entityPOCO.ClaimId = entityPM.ClaimId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.LineNo = entityPM.LineNo;
            }
        }

        public void CustomPOCOToPM(ClaimImporterDeclarsPage3PM entityPM, ClaimImporterDeclarsPage3 entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ImporterDeclarationTypeName);

            if (entityPOCO.ImporterLoiDeclarationTypeCode != null)
            {
                ImporterDeclarationTypeQueryService importerDeclarationTypeQueryService = new ImporterDeclarationTypeQueryService(entityPOCO.Tenant);
                ImporterDeclarationTypePM importerDeclarationTypePM = importerDeclarationTypeQueryService.GetSingle(entityPOCO.ImporterLoiDeclarationTypeCode, false, true);
                entityPM.ImporterDeclarationTypeName = importerDeclarationTypePM.LocalName;
            }
        }
   }


}
   