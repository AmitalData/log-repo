using System;
using System.Collections.Generic;
using System.Linq;

using Logitude.BL.DataContracts;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public class FollowUpsCustomFilter
    {
        private int tenant;
        public FollowUpsCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<ShipmentFollowUpDataView> GetShipmentFollowUpFilteredQuery(QueryOperations operations, IQueryable<ShipmentFollowUpDataView> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
              DateTime todayDate = DateTime.Now.Date;
            DateTime tomorrowDate=DateTime.Now.AddDays(1).Date;
            DateTime afterTommorow = tomorrowDate.AddDays(1).Date;
            DateTime dueDate = DateTime.Now.Date;
            bool showAllFollowUp = false;
          
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.FieldName == "SearchFields")
                {
                    string value = item.FieldValue.ToString();
                    queryableData = queryableData.Where(d => d.SearchFields.Contains(value));
                }

                if (item.IsCustom)
                {
                    

                    if (item.FieldName == "TodayFollowUps")
                    {
                        queryableData = queryableData.Where(d => d.FollowUpDate >= todayDate && d.FollowUpDate < tomorrowDate);
                    }
                    if (item.FieldName == "TomorrowFollowUps")
                    {
                        queryableData = queryableData.Where(d => d.FollowUpDate >= tomorrowDate && d.FollowUpDate < afterTommorow);
                    }
                    if (item.FieldName == "DueDateFollowUps")
                    {
                        queryableData = queryableData.Where(d => d.FollowUpDate <= todayDate);
                    }
                   
                    if (item.FieldName == "ShowAllFollowUps")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showAllFollowUp = true;
                        }

                    }

                    if (item.FieldName == "MyFollowUps")
                    {
                        var userId = operations.UserId;
                        if (string.IsNullOrEmpty(userId))
                        {
                            string email = SecurityUtility.GetAuthenticatedUser();

                            ContactQuery contactQuery = new ContactQuery(tenant);

                            ContactPM loggedContact = contactQuery.GetContactByEmailOnly(email, tenant);
                            userId = loggedContact.Id;
                        }
                        
                        queryableData = queryableData.Where(d => d.FollowUpOwnerId == userId);
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

        public IQueryable<QuoteList> GetQuoteFollowUpFilteredQuery(QueryOperations operations, IQueryable<QuoteList> queryableData)
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


                    if (item.FieldName == "TodayFollowUps")
                    {
                        queryableData = queryableData.Where(d => d.FollowUpDate >= todayDate && d.FollowUpDate < tomorrowDate);
                    }
                    if (item.FieldName == "TomorrowFollowUps")
                    {
                        queryableData = queryableData.Where(d => d.FollowUpDate >= tomorrowDate && d.FollowUpDate < afterTommorow);
                    }
                    if (item.FieldName == "DueDateFollowUps")
                    {
                        queryableData = queryableData.Where(d => d.FollowUpDate <= todayDate);
                    }

                    if (item.FieldName == "ShowAllFollowUps")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showAllFollowUp = true;
                        }

                    }

                }
            }
            if (!showAllFollowUp)
            {
                QueryFilterItem item = queryFilters.Where(d => d.FieldName == "FollowUpOwnerUserId").FirstOrDefault();
                queryableData = queryableData.Where(d => d.FollowUpOwnerId == item.FieldValue);
            }

            return queryableData;

        }

    }
}