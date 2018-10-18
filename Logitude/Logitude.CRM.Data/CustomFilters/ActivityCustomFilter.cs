using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Helpers;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace Logitude.CRM.Data.CustomFilters
{
    public class ActivityCustomFilter
    {
        public static IQueryable<Activity> GetFilteredQuery(QueryOperations operations, IQueryable<Activity> queryableData, int tenant)
        {
            ICommonDataContext myContext = CommonDataContext.GetContext(tenant);

            string loggedUser = Tools.GetAuthenticatedUser();
            ContactRepository contactRep = new ContactRepository(myContext);
            Contact contact = contactRep.GetSingleContactByEmail(loggedUser, tenant);

            UserRepository userRepository = new UserRepository(myContext);
            User myUser = userRepository.GetSingleUser(contact.Id, tenant);
            if(myUser != null)
            {
                if (myUser.IsBranchRestricted)
                {
                    List<string> allBranchesId = (from d in myContext.UserPermittedBranches
                                                where d.UserId == myUser.Id
                                                select d.BranchId).ToList();

                    if (allBranchesId.Count > 0)
                    {
                        queryableData = queryableData.Where(d => allBranchesId.Contains(d.BranchId));
                    }
                }
            }

            bool showIsCancelled = false;
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    //if (item.FieldName == "ActivityTeams")
                    //{
                    //    string idsString = item.FieldValue.ToString();
                    //    List<string> ids = idsString.Split(',').ToList();
                    //    queryableData = queryableData.Where(d => ids.Contains(d.TeamId));
                    //}

                    if (item.FieldName == "MyOpenActivities")
                    {
                        queryableData = queryableData.Where(d => d.IsOpen && d.OwnerId == contact.Id);
                    }

                    if (item.FieldName == "AllOpenActivities")
                    {
                        queryableData = queryableData.Where(d => d.IsOpen);
                    }

                    if (item.FieldName == "MyClosedActivities")
                    {
                        queryableData = queryableData.Where(d => !d.IsOpen && d.OwnerId == contact.Id);
                    }

                    if (item.FieldName == "AllClosedActivities")
                    {
                        queryableData = queryableData.Where(d => !d.IsOpen);
                    }

                    if (item.FieldName == "ActivityNext7DaysCustomFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            if (item.FieldValue.ToString() == "no date")
                            {
                                //queryableData = queryableData.Where(d => d.DueDate == null);
                                queryableData = from a in queryableData
                                                where
                                                a.StartDateTime == null && a.DueDate == null
                                                select a;
                            }

                            else if (item.FieldValue.ToString() == "old")
                            {
                                DateTime? date = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                                //queryableData = queryableData.Where(d => d.DueDate != null && DbFunctions.TruncateTime(d.DueDate) < date);
                                queryableData = from a in queryableData
                                                where
                                                (a.StartDateTime != null && DbFunctions.TruncateTime(a.StartDateTime) < date)
                                                ||
                                                (a.StartDateTime == null && a.DueDate != null && DbFunctions.TruncateTime(a.DueDate) < date)
                                                select a;
                            }

                            else
                            {
                                DateTime? date = Convert.ToDateTime(item.FieldValue);//item.FieldValue as DateTime?;
                                //queryableData = queryableData.Where(d => d.DueDate != null && DbFunctions.TruncateTime(d.DueDate) == date);
                                queryableData = from a in queryableData
                                                where
                                                (a.StartDateTime != null && DbFunctions.TruncateTime(a.StartDateTime) == date)
                                                ||
                                                (a.StartDateTime == null && a.DueDate != null && DbFunctions.TruncateTime(a.DueDate) == date)
                                                select a;
                            }
                        }
                    }

                    if (item.FieldName == "ChartCreateDateFilter")
                    {
                        string FromDate = item.FieldValue.ToString() == "null" ? null : item.FieldValue.ToString();
                        string ToDate = item.FieldValue2.ToString() == "null" ? null : item.FieldValue2.ToString();
                        DateTime? FromDateOBJ = Tools.GetDate(FromDate);
                        if (FromDate == null)
                        {
                            FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        DateTime? ToDateOBJ = Tools.GetDate(ToDate);
                        if (ToDateOBJ == null)
                        {
                            ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }


                        queryableData = queryableData.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= FromDateOBJ && DbFunctions.TruncateTime(d.CreateDate) <= ToDateOBJ);

                    }

                    if (item.FieldName == "ChartCompleteDateFilter")
                    {

                        string FromDate = item.FieldValue.ToString() == "null" ? null : item.FieldValue.ToString();
                        string ToDate = item.FieldValue2.ToString() == "null" ? null : item.FieldValue2.ToString();
                        DateTime? FromDateOBJ = Tools.GetDate(FromDate);
                        if (FromDate == null)
                        {
                            FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        DateTime? ToDateOBJ = Tools.GetDate(ToDate);
                        if (ToDateOBJ == null)
                        {
                            ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        queryableData = queryableData.Where(d => DbFunctions.TruncateTime(d.CompleteDate) >= FromDateOBJ && DbFunctions.TruncateTime(d.CompleteDate) <= ToDateOBJ);
                            
                        
                    }

                    if (item.FieldName == "MeetingsSummary")
                    {
                        queryableData = queryableData.Where(d => d.ActivityTypeCode == "AP" && !d.IsOpen);
                    }

                    if (item.FieldName == "DailySpotlightFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();

                            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                            DateTime date1 = todayDate;
                            DateTime date2 = todayDate;

                            switch (code)
                            {
                                case "AC_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "AC_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "AC_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                            }

                            queryableData = queryableData.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= date1 && DbFunctions.TruncateTime(d.CreateDate) <= date2);
                        }
                    }

                    if (item.FieldName == "CancelledActivities")
                    {
                        showIsCancelled = true;
                    }

                    if (item.FieldName == "ConnectedtoCustomer")
                    {
                        bool myFlag = (bool)item.FieldValue;
                        if (myFlag)
                        {
                            queryableData = queryableData.Where(d => d.CustomerId != null);
                        }
                        else
                        {
                            queryableData = queryableData.Where(d => d.CustomerId == null);
                        }
                    }

                    if (item.FieldName == "ConnectedtoOpportunity")
                    {
                        bool myFlag = (bool)item.FieldValue;
                        if (myFlag)
                        {
                            queryableData = queryableData.Where(d => d.OpportunityId != null);
                        }
                        else
                        {
                            queryableData = queryableData.Where(d => d.OpportunityId == null);
                        }
                    }

                    if (item.FieldName == "ConnectedtoTicket")
                    {
                        bool myFlag = (bool)item.FieldValue;
                        if (myFlag)
                        {
                            queryableData = queryableData.Where(d => d.TicketId != null);
                        }
                        else
                        {
                            queryableData = queryableData.Where(d => d.TicketId == null);
                        }
                    }

                    if (item.FieldName == "ConnectedtoQuote")
                    {
                         bool myFlag = (bool)item.FieldValue;
                         if (myFlag)
                         {
                             queryableData = queryableData.Where(d => d.QuoteId != null);
                         }
                         else
                         {
                             queryableData = queryableData.Where(d => d.QuoteId == null);
                         }
                    }
                }
            }

            if (showIsCancelled)
            {
                queryableData = queryableData.Where(d => d.ActivityStatusCode == "X");
            }

            else
            {
                queryableData = queryableData.Where(d => d.ActivityStatusCode != "X");
            }            

            return queryableData;
        }
    }
}