
using Logitude.Workflow.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.Workflow.Data.CustomFilters
{
    public class TaskCustomFilter
    {
        public static IQueryable<Task> GetFilteredQuery(QueryOperations operations, IQueryable<Task> queryableData, int tenant)
        {
            List<QueryFilterItem> customQueryFilters = operations.QueryFilterItems.Where(q => q.IsCustom).ToList();

            foreach (QueryFilterItem customQueryFilter in customQueryFilters)
            {
                if (customQueryFilter.FieldName == "MyOpenTasks")
                {
                    Contact contact = GetLoggedContact(tenant);
                    string contactId = contact?.Id;
                    queryableData = queryableData.Where(d => d.IsClosed == false && d.OwnerId == contactId);
                }
            }
            return queryableData;
        }

        private static Contact GetLoggedContact(int tenant)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            string loggedUserEmail = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(commonDataContext);
            Contact contact = contactRepository.GetSingleContactByEmail(loggedUserEmail, tenant);
            return contact;
        }
    }
}