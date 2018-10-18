
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
   
   public partial class ClaimImporterDeclarsPage3ADataMapping: IMapping<ClaimImporterDeclarsPage3APM, ClaimImporterDeclarsPage3A>
   {

        public void CustomPMToPOCO(ClaimImporterDeclarsPage3APM entityPM, ClaimImporterDeclarsPage3A entityPOCO)
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

        public void CustomPOCOToPM(ClaimImporterDeclarsPage3APM entityPM, ClaimImporterDeclarsPage3A entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.CommercialSaleTypeName);

            if (entityPOCO.CommercialSaleTypeCode != null)
            {
                CommercialSaleQueryService commercialSaleQueryService = new CommercialSaleQueryService(entityPOCO.Tenant);
                CommercialSalePM commercialSalePM = commercialSaleQueryService.GetSingle(entityPOCO.CommercialSaleTypeCode, false, true);
                entityPM.CommercialSaleTypeName = commercialSalePM.LocalName;
            }
        }
   }


}
   