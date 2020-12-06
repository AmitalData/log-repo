using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Utils;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.CustomFilters
{
    class DeclarationReferantDataCustomFilters
    {
        public IQueryable<DeclarationReferantData> GetFilteredQuery(IQueryable<DeclarationReferantData> queryableData)
        {
            queryableData = queryableData.Where(d => (d.DeclarationId == null));
            return queryableData; 
        }


        public IQueryable<DeclarationReferantData> GetFreelancerDeclarationReferantDatas(QueryOperations operations, IQueryable<DeclarationReferantData> queryableData, int tenant)
        {
            var context = CustomContext.GetContext(tenant);

            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            if (frlUtil.user.IsFreelancer)
            {
                List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
                if (customersIds.Count > 0)
                {
                    queryableData = (from a in queryableData
                                    join d in context.Declarations
                                    on a.DeclarationId equals d.Id
                                    where customersIds.Contains(d.CustomerId)
                                    select  a)
                                    ;

                   // queryableData = queryableData.Where(d => customersIds.Contains(d.));
                }
            }

            return queryableData;
        }
    }
}
