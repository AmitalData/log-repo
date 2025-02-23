using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class PendingByKeywordUpdateService : EntityUpdateService<PendingByKeyword, PendingByKeywordPM, EntityPM>
    {

        protected override void OnCreating(PendingByKeywordPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.PendingByKeyword", entityPM.Tenant);
            if (entityPM.ExceptKeywords != null)
            {
                var ExceptKeywordspunctuation = entityPM.ExceptKeywords.Where(Char.IsPunctuation).Distinct().ToArray();
                var ExceptKeywordslist = entityPM.ExceptKeywords.Split().Select(x => x.Trim(ExceptKeywordspunctuation)).ToList();

                var keywordsListpunctuation = entityPM.KeywordsList.Where(Char.IsPunctuation).Distinct().ToArray();
                var KeywordsListlist = entityPM.KeywordsList.Split().Select(x => x.Trim(keywordsListpunctuation)).ToList();
                if (ExceptKeywordslist.SequenceEqual(KeywordsListlist))
                {
                    throw new Exception($"אותו צירוף מילים אסור להיות זהה בשדה צירוף מילים להחרגה ושדה מילות מפתח");
                }
            }

        }

        protected override void OnUpdating(PendingByKeywordPM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.CourierPendingReasonId))
            {
              
                CourierPendingReasonQueryService courierPendingReasonQueryService = new CourierPendingReasonQueryService(entityPM.Tenant);
                CourierPendingReasonPM courierPendingReason = courierPendingReasonQueryService.GetSingle(entityPM.CourierPendingReasonId, false, true);
                entityPM.CourierPendingReasonCode = courierPendingReason.Code;
              
             
            }
            if (entityPM.ExceptKeywords != null)
            {
                var ExceptKeywordspunctuation = entityPM.ExceptKeywords.Where(Char.IsPunctuation).Distinct().ToArray();
                var ExceptKeywordslist = entityPM.ExceptKeywords.Split().Select(x => x.Trim(ExceptKeywordspunctuation)).ToList();

                var keywordsListpunctuation = entityPM.KeywordsList.Where(Char.IsPunctuation).Distinct().ToArray();
                var KeywordsListlist = entityPM.KeywordsList.Split().Select(x => x.Trim(keywordsListpunctuation)).ToList();
                if (ExceptKeywordslist.SequenceEqual(KeywordsListlist))
                {
                    throw new Exception($"אותו צירוף מילים אסור להיות זהה בשדה צירוף מילים להחרגה ושדה מילות מפתח");
                }
            }
            //if (!String.IsNullOrWhiteSpace(entityPM.KeywordsList))
            //{
            //    entityPM.KeywordsList = NormalyzekeyWordsList(entityPM.KeywordsList);
            //}
        }

        public string NormalyzekeyWordsList(string keyWordsList)
        {

            keyWordsList = keyWordsList.Replace(" ", ",");
            keyWordsList = "," + keyWordsList + ",";
            while (keyWordsList.Contains(",,"))
            {
                keyWordsList = keyWordsList.Replace(",,", ",");
            }

            return keyWordsList;
        }
    }
}
