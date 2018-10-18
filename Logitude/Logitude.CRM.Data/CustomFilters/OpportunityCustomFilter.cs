using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.Helpers;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Logitude.CRM.Data.Helpers;

namespace Logitude.CRM.Data.CustomFilters
{
    public class OpportunityCustomFilter
    {
        public static IQueryable<Opportunity> GetFilteredQuery(QueryOperations operations, IQueryable<Opportunity> queryableData,int tenant)
        {
            bool showIsCancelled = false;
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    string loggedUser = Tools.GetAuthenticatedUser();
                    ContactRepository contactRep = new ContactRepository(tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(loggedUser, tenant);

                    if (item.FieldName == "MyOpenOpportunities")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.OwnerId == contact.Id);
                    }

                    if (item.FieldName == "AllOpenOpportunities")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false);
                    }

                    if (item.FieldName == "MyClosedOpportunities")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == true && d.OwnerId == contact.Id);
                    }

                    if (item.FieldName == "AllClosedOpportunities")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == true);
                    }

                    if (item.FieldName == "OpenByStageOpp")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false && d.LastStageDate != null);
                    }

                    if (item.FieldName == "ChartCreateDateFilter")
                    {
                        string FromDate = item.FieldValue.ToString()=="null"?null: item.FieldValue.ToString();
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

                    if (item.FieldName == "ChartActualClosingDateFilter")
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

                        queryableData = queryableData.Where(d => DbFunctions.TruncateTime(d.ActualClosingDate) >= FromDateOBJ && DbFunctions.TruncateTime(d.ActualClosingDate) <= ToDateOBJ);

                        
                    }

                    if (item.FieldName == "FunnelFilterCode")
                    {
                        if (item.FieldValue != null)
                        {
                            string filterCode = item.FieldValue.ToString();

                            if (filterCode == "SHI")
                            {
                                queryableData = queryableData.Where(s => s.NumberOfShipments != null);
                            }
                        }
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
                                case "OP_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "OP_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "OP_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                            }

                            queryableData = queryableData.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= date1 && DbFunctions.TruncateTime(d.CreateDate) <= date2);
                        }
                    }

                    if (item.FieldName == "CancelledOpportunities")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsCancelled = true;
                        }
                    }
                }
            }

            if (showIsCancelled)
            {
                queryableData = queryableData.Where(d => d.IsCancelled == true);
            }

            else
            {
                queryableData = queryableData.Where(d => d.IsCancelled == false);
            }

            return queryableData;
        }
    }
}