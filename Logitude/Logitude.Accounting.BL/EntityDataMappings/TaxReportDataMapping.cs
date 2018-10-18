
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class TaxReportDataMapping: IMapping<TaxReportPM, TaxReport>
   {

        public void CustomPMToPOCO(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.StatusCode);

            if (entityPOCO.StatusCode != null)
            {
                VatReportStatusQueryService queryService = new VatReportStatusQueryService(entityPOCO.Tenant);
                VatReportStatusPM status = queryService.GetSingle(entityPOCO.StatusCode,false,false);
                if (status != null)
                {
                    entityPM.StatusEnglishName = status.EnglishName;
                    entityPM.StatusLocalName = status.LocalName;
                }
            }
        }
    }


}
   