
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

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsAutonomyKeywordUpdateService : EntityUpdateService<CustomsAutonomyKeyword, CustomsAutonomyKeywordPM, EntityPM>
    {

        protected override void OnCreating(CustomsAutonomyKeywordPM entityPM, EntityPM entityParentPM)
        {

        
            var poco = (this.Repository as CustomsAutonomyKeywordRepository).GetBykeywordList(entityPM.KeywordsList, entityPM.Tenant);
            if (poco != null)
            {
                throw new Exception($"מילת מפתח זו קיימת כבר- לא ניתן להזין מילת מפתח כפולה");
            }
            entityPM.Id= IdCounter.GetNumber("Customs.CustomsAutonomyKeyword", entityPM.Tenant);
            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(CustomsAutonomyKeywordPM entityPM, CustomsAutonomyKeyword entityPOCO)
        {
            if (entityPM.ChangeSetOp != ChangeSetOperation.Delete)
            {
                var poco = (this.Repository as CustomsAutonomyKeywordRepository).GetBykeywordList(entityPM.KeywordsList, entityPM.Tenant);
                if (poco != null)
                {
                    throw new Exception($"מילת מפתח זו קיימת כבר- לא ניתן להזין מילת מפתח כפולה");
                }
            }
            base.OnUpdating(entityPM, entityPOCO);
        }
    }
}
