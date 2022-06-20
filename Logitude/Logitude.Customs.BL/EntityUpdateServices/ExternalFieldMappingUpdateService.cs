
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.BL;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ExternalFieldMappingUpdateService : EntityUpdateService<ExternalFieldMapping, ExternalFieldMappingPM, EntityPM>
    {

        protected override void OnCreating(ExternalFieldMappingPM entityPM, EntityPM entityParentPM)
        {
            var poco = (this.Repository as ExternalFieldMappingRepository).GetSingleByStatusFieldTypeAndStatusCode(entityPM.StatusFieldType,entityPM.StatusCode, entityPM.Tenant);
            if (poco != null)
            {
                throw new Exception($"מיפוי זה קיימ כבר- לא ניתן להזין מיפוי כפול");
            }
            entityPM.Id= IdCounter.GetNumber("Customs.ExternalFieldMapping", entityPM.Tenant);
            var externalFieldMappingBL = new ExternalFieldMappingBL();
            entityPM.Field = externalFieldMappingBL.GetAvailableFieldByStatusFieldType(entityPM.Tenant, entityPM.StatusFieldType);
            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(ExternalFieldMappingPM entityPM, ExternalFieldMapping entityPOCO)
        {
           /* if (entityPM.ChangeSetOp != ChangeSetOperation.Delete)
            {
                var poco = (this.Repository as CustomsAutonomyKeywordRepository).GetBykeywordList(entityPM.KeywordsList, entityPM.Tenant);
                if (poco != null)
                {
                    throw new Exception($"מילת מפתח זו קיימת כבר- לא ניתן להזין מילת מפתח כפולה");
                }
            }*/
            base.OnUpdating(entityPM, entityPOCO);
        }

    }
}
