using Logitude.Server.Tools.Helpers;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.Data.CustomFilters
{
    public class TMProjectCustomFilter
    {
        public static IQueryable<TMProject> GetFilteredQuery(QueryOperations operations, IQueryable<TMProject> queryableData, int tenant)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "MyAllOpenProjects")
                    {
                        ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
                        string loggedUser = AuthenticationUtil.GetAuthenticatedUser();
                        ContactRepository contactRep = new ContactRepository(myContext);
                        Contact contact = contactRep.GetSingleContactByEmail(loggedUser, tenant);
                        queryableData = queryableData.Where(d => d.OwnerId == contact.Id);
                    }
                }
            }

            return queryableData;
        }
    }
}
