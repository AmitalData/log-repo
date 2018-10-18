using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

using WebFreight.Web.Helpers;
using Logitude.Server.Tools.Counters;
namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddRanks
    {
        public static void AddRank(RankDetails rankDetails, RankRepository rankRepository, Dictionary<string, Rank> tenantRanks)
        {
            if (tenantRanks.Keys.Contains(rankDetails.Code))
            {
                Rank rank = tenantRanks[rankDetails.Code];
                rank.Name = rankDetails.Name;
                rank.Tenant = rankDetails.Tenant;
                rank.SearchFields = rankDetails.SearchFields;
                rankRepository.Update(rank);
            }
            else
            {
                Rank newRank = new Rank()
                {
                    Tenant = rankDetails.Tenant,
                    Name = rankDetails.Name,
                    Code = rankDetails.Code,
                    Id = IdCounter.GetNumber("Rank", rankDetails.Tenant).ToString(),
                    SearchFields=rankDetails.SearchFields,
                };
                rankRepository.Add(newRank);
            }
        }
    }
}