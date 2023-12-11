using Devart.Data.Linq;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Utils;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
        public IQueryable<DeclarationReferantData> GetFilteredStatusOccuredQuery(QueryOperations operations,IQueryable<DeclarationReferantData> queryableData, ICustomContext context)
        {


           /* queryableData = (from a in queryableData
                             join declarationStatus in context.DeclarationStatuses.Where(decStatus => decStatus.StatusCode.Status_Code == "TST").DefaultIfEmpty()
                             on a.DeclarationIdToDisplay equals declarationStatus.DeclarationId
                             select a)
                             ;*/

            var q = context.DeclarationStatuses.Where(decStatus => decStatus.StatusCode.Status_Code == "TST").Select (r=>r.DeclarationId);
            queryableData = (from a in queryableData.Where( r=>q.Contains(r.DeclarationId) )
                             
                             select a)
                             ;
            //var list = queryableData.ToList();
            return queryableData;
            /* iQueryable = (from a in iQueryable
                           join declarationStatus in context.DeclarationStatuses.Where(x => x.StatusCode.Status_Code == "TST")
                           on a.DeclarationIdToDisplay equals declarationStatus.DeclarationId into qjoinDeclarationStatuses
                           select a);*/
            //qjoinDeclarationStatuses.DefaultIfEmpty().Select(r=>r.StatusCode.Status_Code),
        }

        public IQueryable<DeclarationReferantData> GetFreelancerDeclarationReferantDatas(QueryOperations operations, IQueryable<DeclarationReferantData> queryableData, int tenant , ICustomContext context)
        {
            // var context = CustomContext.GetContext(tenant);
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.FieldName == "OccuredStatus")
                {
                    var q = context.DeclarationStatuses.Where(decStatus => item.FieldValue.ToString().Contains(decStatus.StatusCode.Status_Code)).Select(r => r.DeclarationId);
                    queryableData = (from a in queryableData.Where(r => q.Contains(r.DeclarationId)) select a);
                }
                if (item.FieldName == "NotOccuredStatus")
                {
                    var notContainsFilter = item.FieldValue.ToString();
                    var notContainsQuery = context.DeclarationStatuses
                        .Where(decStatus => notContainsFilter.Contains(decStatus.StatusCode.Status_Code))
                        .Select(r => r.DeclarationId);
                    queryableData = queryableData.Where(r => !notContainsQuery.Contains(r.DeclarationId));
                }
            }
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
