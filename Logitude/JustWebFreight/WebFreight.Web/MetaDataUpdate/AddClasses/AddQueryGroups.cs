using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddQueryGroups
    {
        public static QueryGroup AddQueryGroup(QueryGroupDetails queryGroupDetails, QueryGroupRepository queryGroupRepository)
        {
            Dictionary<string, QueryGroup> tenantFixedAmounts = queryGroupRepository.GetQueryGroups().ToDictionary(d => d.Code, a => a);

            if (tenantFixedAmounts.Keys.Contains(queryGroupDetails.Code))
            {
                QueryGroup queryGroup = queryGroupRepository.GetSingleQueryGroup(queryGroupDetails.Code);
                queryGroup.Name = queryGroupDetails.Name;
                queryGroup.IndexOrder = queryGroupDetails.IndexOrder;
                queryGroupRepository.Update(queryGroup);
                return queryGroup;
            }
            else
            {
                QueryGroup newQueryGroup = new QueryGroup() { Code = queryGroupDetails.Code, Name = queryGroupDetails.Name, IndexOrder = queryGroupDetails.IndexOrder };
                queryGroupRepository.Add(newQueryGroup);
                return newQueryGroup;
            }

        }

        public static QueryGroup AddQueryGroup(QueryGroupDetails queryGroupDetails, QueryGroupRepository queryGroupRepository, Dictionary<string, QueryGroup> tenantQueryGroups)
        {
          
            if (tenantQueryGroups.Keys.Contains(queryGroupDetails.Code))
            {
                QueryGroup queryGroup = tenantQueryGroups[queryGroupDetails.Code];//queryGroupRepository.GetSingleQueryGroup(queryGroupDetails.Code);
                queryGroup.Name = queryGroupDetails.Name;
                queryGroup.IndexOrder = queryGroupDetails.IndexOrder;
                queryGroupRepository.Update(queryGroup);
                return queryGroup;
            }
            else
            {
                QueryGroup newQueryGroup = new QueryGroup() { Code = queryGroupDetails.Code, Name = queryGroupDetails.Name, IndexOrder = queryGroupDetails.IndexOrder };
                queryGroupRepository.Add(newQueryGroup);
                tenantQueryGroups.Add(newQueryGroup.Code, newQueryGroup);
                return newQueryGroup;
            }

        }

        //public static QueryGroup AddQueryGroup(QueryGroupDetails queryGroupDetails, QueryGroupRepository queryGroupRepository)
        //{
        //    Dictionary<string, QueryGroup> tenantFixedAmounts = queryGroupRepository.GetQueryGroups().ToDictionary(d => d.Code, a => a);

        //    if (tenantFixedAmounts.Keys.Contains(queryGroupDetails.Code))
        //    {
        //        QueryGroup queryGroup = queryGroupRepository.GetSingleQueryGroup(queryGroupDetails.Code);
        //        queryGroup.Name = queryGroupDetails.Name;
        //        queryGroup.IndexOrder = queryGroupDetails.IndexOrder;
        //        queryGroupRepository.Update(queryGroup);
        //        return queryGroup;
        //    }
        //    else
        //    {
        //        QueryGroup newQueryGroup = new QueryGroup() { Code = queryGroupDetails.Code, Name = queryGroupDetails.Name, IndexOrder = queryGroupDetails.IndexOrder };
        //        queryGroupRepository.Add(newQueryGroup);
        //        return newQueryGroup;
        //    }

        //}
    }
}