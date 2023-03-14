
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
            Contact contact = GetLoggedContact(tenant);

            List<QueryFilterItem> customQueryFilters = operations.QueryFilterItems.Where(q => q.IsCustom).ToList();

            foreach (QueryFilterItem item in customQueryFilters)
            {
                if (item.FieldName == "MyOpenTasks")
                {
                    queryableData = queryableData.Where(d => d.IsClosed == false && d.OwnerId == contact.Id);
                }
            }
            return queryableData;
        }

        private static Contact GetLoggedContact(int tenant)
        {
            ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
            string loggedUserEmail = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(myContext);
            Contact contact = contactRepository.GetSingleContactByEmail(loggedUserEmail, tenant);
            return contact;
        }
    }
}