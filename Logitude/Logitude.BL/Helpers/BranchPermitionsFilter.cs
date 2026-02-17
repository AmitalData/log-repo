using System.Collections.Generic;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.Helpers
{
    public class BranchPermitionsFilter
    {
        public static void AddUserBranchRestrictionFilters(QueryOperations queryOperations, int tenant)
        {
            try
            {
                ContactQuery contactRep = new ContactQuery(tenant);
                UserQuery userQuery = new UserQuery(tenant);

                ContactPM contact = contactRep.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, false);
                UserPM user = userQuery.GetSinglePM(contact.Id, tenant);
                if (user != null && user.IsBranchRestricted)
                {
                    string values = "";
                    foreach (var item in user.UserPermittedBranches)
                    {

                        values += item.BranchId + ",";
                    }

                    queryOperations.SetFilter("BranchId", values.TrimEnd(','), false, "InListExact", null, false);
                }
            }
            catch
            {
            }
        }

        public static IQueryable<T> AddUserBranchRestrictionFilters<T>(QueryOperations queryOperations, IQueryable<T> queryData, int tenant)
        {
            IQueryable<T> result = queryData;
            try
            {
                ContactQuery contactRep = new ContactQuery(tenant);
                UserQuery userQuery = new UserQuery(tenant);

                ContactPM contact = contactRep.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, false);
                UserPM user = userQuery.GetSinglePM(contact.Id, tenant);
                if (user != null && user.IsBranchRestricted)
                {
                    string values = "";
                    foreach (var item in user.UserPermittedBranches)
                    {

                        values += item.BranchId + ",";
                    }

                    queryOperations.SetFilter("BranchId", values.TrimEnd(','), false, "InListExact", null, false);

                    GenericFilter filter = new GenericFilter();
                    result = filter.GetFilteredQuery<T>(queryOperations, queryData);
                }                 

                return result;
            }
            catch
            {
                return queryData;
            }
        }

        public static T AddUserBranchRestrictionFilters<T>(QueryOperations queryOperations, T entity, int tenant)
        {
            if (entity != null)
            {
                T result = entity;
                try
                {
                    IQueryable<T> queryData = (new List<T>() { entity }).AsQueryable<T>();
                    ContactQuery contactRep = new ContactQuery(tenant);
                    UserQuery userQuery = new UserQuery(tenant);

                    ContactPM contact = contactRep.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), tenant, false);
                    UserPM user = userQuery.GetSinglePM(contact.Id, tenant);
                    if (user != null && user.IsBranchRestricted)
                    {
                        string values = "";
                        foreach (var item in user.UserPermittedBranches)
                        {

                            values += item.BranchId + ",";
                        }

                        queryOperations.SetFilter("BranchId", values.TrimEnd(','), false, "InListExact", null, false);

                        GenericFilter filter = new GenericFilter();
                        queryData = filter.GetFilteredQuery<T>(queryOperations, queryData);
                        result = queryData.FirstOrDefault();
                    }

                    return result;
                }
                catch
                {
                    return result;
                }
            }
            else
            {
                return entity;
            }
        }
    }
}
