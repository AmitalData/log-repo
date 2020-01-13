
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
            CustomsAutonomyKeywordDetails customsAutonomyKeywordDetails = new CustomsAutonomyKeywordDetails();
            if (customsAutonomyKeywordDetails.GetAllCustomsAutonomyKeywords().FirstOrDefault(x=> x.Code== entityPM.KeywordtypeCode)==null)
            {
                throw new Exception($"Insert {entityPM.KeywordtypeCode} not allowed !! Code is not exist (ID:{entityPM.Id})");

            }
            var poco = (this.Repository as CustomsAutonomyKeywordRepository).GetByKeywordtypeCode(entityPM.KeywordtypeCode, entityPM.Tenant);
            if (poco!=null)
            {
                throw new Exception($"Insert {entityPM.KeywordtypeCode} not allowed !! Due already exist (ID:{entityPM.Id})");
            }
            entityPM.Id= IdCounter.GetNumber("Customs.CustomsAutonomyKeyword", entityPM.Tenant);
            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(CustomsAutonomyKeywordPM entityPM, CustomsAutonomyKeyword entityPOCO)
        {
            if (entityPM.KeywordtypeCode!=entityPOCO.KeywordtypeCode && !String.IsNullOrWhiteSpace(entityPOCO.KeywordtypeCode))
            {
                throw new Exception($"Change  KeywordtypeCode : {entityPM.KeywordtypeCode} not allowed !! -candidate key");
            }
            base.OnUpdating(entityPM, entityPOCO);
        }
    }
}
