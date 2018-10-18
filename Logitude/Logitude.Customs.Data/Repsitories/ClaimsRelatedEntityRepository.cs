
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class ClaimsRelatedEntityRepository : IRepository<ClaimsRelatedEntity>
    {

        public List<ClaimsRelatedEntity> GetMulti(EntityKeyFields entityKeys)
        {
            ClaimKeys claimKeys = entityKeys as ClaimKeys;

            return (from a in context.ClaimsRelatedEntities
                    where a.ClaimId == claimKeys.Id
                    select a).ToList();
        }


        public string GetClaimIdByTapagNumber(string fileNumber, int numeral, int tenant)
        {
            if (String.IsNullOrWhiteSpace(fileNumber)) return "";
            return (
                  from rec in context.ClaimsRelatedEntities
                  where rec.TapagNumber == fileNumber && rec.Numeral == numeral && rec.Tenant == tenant
                  select rec.ClaimId
                  )
                  .FirstOrDefault();
        }

        public ClaimsRelatedEntity GetRelatedEntityByTapagNumber(string fileNumber, int numeral, int tenant)
        {
            if (String.IsNullOrWhiteSpace(fileNumber)) return null;
            return (
                  from rec in context.ClaimsRelatedEntities
                  where rec.TapagNumber == fileNumber && rec.Numeral == numeral && rec.Tenant == tenant
                  select rec
                  )
                  .FirstOrDefault();
        }

        public List<ClaimsRelatedEntity> GetCREsByCounterKeys(string claimId, List<int> keys, int tenant)
        {
            return (from a in context.ClaimsRelatedEntities
                    where a.ClaimId == claimId && a.Tenant == tenant && keys.Contains(a.EntityCounterKey)
                    select a).ToList();
        }
    }

}
