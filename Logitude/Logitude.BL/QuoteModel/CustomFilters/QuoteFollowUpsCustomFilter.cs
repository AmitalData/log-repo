using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.QuoteModel.CustomFilters
{
    public class QuoteFollowUpsCustomFilter
    {
        private int tenant;
        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public QuoteFollowUpsCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<QuoteFollowUpDataView> GetQuoteFollowUpFilteredQuery(QueryOperations operations, IQueryable<QuoteFollowUpDataView> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            DateTime todayDate = DateTime.Now.Date;
            DateTime tomorrowDate = DateTime.Now.AddDays(1).Date;
            DateTime afterTommorow = tomorrowDate.AddDays(1).Date;
            DateTime dueDate = DateTime.Now.Date;
            bool showAllFollowUp = false;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "AllFollowUps")
                    {
                        //bool value = Convert.ToBoolean(item.FieldValue);
                        //if (value)
                        //{
                            showAllFollowUp = true;
                        //}
                    }

                    if (item.FieldName == "MyFollowUps")
                    {
                        string email = SecurityUtility.GetAuthenticatedUser();
                        ContactQuery contactQuery = new ContactQuery(tenant);
                        ContactPM loggedContact = contactQuery.GetContactByEmailOnly(email, tenant);

                        queryableData = queryableData.Where(d => d.FollowUpOwnerId == loggedContact.Id);
                    }
                }
            }
            if (!showAllFollowUp)
            {
                QueryFilterItem item = queryFilters.Where(d => d.FieldName == "FollowUpOwnerUserId").FirstOrDefault();
                if (item != null)
                {
                    queryableData = queryableData.Where(d => d.FollowUpOwnerId == item.FieldValue);
                }
            }

            return queryableData;
        }
    }
}