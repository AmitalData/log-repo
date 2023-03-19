using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Data.Entity;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Resolvers;
using Logitude.BL.QuoteModel.CustomFilters;

namespace Logitude.BL.QuoteModel
{
    public class QuoteCustomFilter
    {
        private int tenant;
          public QuoteCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

          public int Tenant
          {
              get { return tenant; }
              set { tenant = value; }
          }

        public IQueryable<Quote> GetFilteredQuery(QueryOperations operations, IQueryable<Quote> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            bool showIsCancelled = false;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "From")
                    {
                        queryableData = DigitalPortalCustomFilter.ApplyFromToFilter(item, queryableData);
                    }

                    if (item.FieldName == "To")
                    {
                        queryableData = DigitalPortalCustomFilter.ApplyFromToFilter(item, queryableData, true);
                    }

                    if (item.FieldName == "DigitalPortalQuotesSearchFields")
                    {
                        queryableData = DigitalPortalCustomFilter.ApplySearchFilter(item, queryableData);
                    }

                    if (item.FieldName == "TransportModeFilter")
                    {
                        queryableData = DigitalPortalCustomFilter.ApplyTransportModeFilter(item, queryableData, tenant);
                    }

                    if (item.FieldName == "StatusFilter")
                    {
                        queryableData = DigitalPortalCustomFilter.ApplyStatusFilter(item, queryableData);
                    }

                    if (item.FieldName == "IsCancelled")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsCancelled = true;
                        }
                    }

                    if (item.FieldName == "AcceptedWithoutShipments")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            queryableData = queryableData.Where(d => d.Stage.Code == "QTAC" && (d.UsageCount == 0 || d.UsageCount == null));
                        }
                    }

                    if (item.FieldName == "Reference")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(
                                d => d.ShipperReference1.StartsWith(value)
                                    || d.ShipperReference2.StartsWith(value)
                                    || d.ConsigneeReference1.StartsWith(value)
                                    || d.ConsigneeReference2.StartsWith(value)
                                    || d.QuoteNumber.StartsWith(value)
                                    );
                        }
                    }

                    if (item.FieldName == "FromOrToPort")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.FromPort.Code.ToUpper().StartsWith(value.ToUpper()) || d.ToPort.Code.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "Client")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {

                            queryableData = queryableData.Where(d => d.ShipperCard.EnglishName.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "OpenQuotes")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == false);
                    }
                    
                    if (item.FieldName == "ClosedQuotes")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed == true);
                    }

                    if (item.FieldName == "IsExpiredQuote")
                    {
                        queryableData = queryableData.Where(d => d.ExpirationDate < DateTime.Now && !d.IsCancelled && !d.IsClosed);
                    }

                    if (item.FieldName == "MyQuotes")
                    {
                        ContactPM contact = LoggedContactResolver.GetLoggedContact(tenant);
                        queryableData = queryableData.Where(d => d.SalesmanUserId == contact.Id && !d.IsCancelled);
                    }

                    if (item.FieldName == "IsCreatedQuote")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            queryableData = queryableData.Where(d => d.Stage.Code == "QTCR");
                        }
                    }

                    if (item.FieldName == "IsDraftQuote")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            queryableData = queryableData.Where(d => d.Stage.Code == "QTDR");
                        }
                    }

                    if (item.FieldName == "IsSentQuote")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            queryableData = queryableData.Where(d => d.Stage.Code == "QTST");
                        }
                    }

                    if (item.FieldName == "IsAcceptedQuote")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            queryableData = queryableData.Where(d => d.Stage.Code == "QTAC");
                        }
                    }

                    if (item.FieldName == "ChartCreateDateFilter")
                    {
                        string FromDate = item.FieldValue.ToString() == "null" ? null : item.FieldValue.ToString();
                        string ToDate = item.FieldValue2.ToString() == "null" ? null : item.FieldValue2.ToString();
                        DateTime? FromDateOBJ = StringHelper.GetDate(FromDate);
                        if (FromDate == null)
                        {
                            FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        DateTime? ToDateOBJ = StringHelper.GetDate(ToDate);
                        if (ToDateOBJ == null)
                        {
                            ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        queryableData = queryableData.Where(d => DbFunctions.TruncateTime(d.OpenDate) >= DbFunctions.TruncateTime(FromDateOBJ) && DbFunctions.TruncateTime(d.OpenDate) <= DbFunctions.TruncateTime(ToDateOBJ));
                    }

                    if (item.FieldName == "QuoteConversionDateFilter")
                    {
                        DateTime? fromDate = null;
                        DateTime? toDate = null;

                        if (item.FieldValue != null)
                        {
                            fromDate = Convert.ToDateTime(item.FieldValue);
                        }

                        if (item.FieldValue2 != null)
                        {
                            toDate = Convert.ToDateTime(item.FieldValue2);
                        }

                        if (fromDate != null && toDate != null)
                        {
                            QuoteStageRepository quoteStageRepository = new QuoteStageRepository(tenant);
                            string stage1Id = quoteStageRepository.GetQuoteStageIdByCode("QTCR", tenant);
                            string stage2Id = quoteStageRepository.GetQuoteStageIdByCode("QTDR", tenant);

                            queryableData = queryableData.Where(d => d.StageId != stage1Id && d.StageId != stage2Id && DbFunctions.TruncateTime(d.OpenDate) >= DbFunctions.TruncateTime(fromDate) && DbFunctions.TruncateTime(d.OpenDate) <= DbFunctions.TruncateTime(toDate));
                        }
                    }

                    if (item.FieldName == "SentQuotesKPIChartFilter")
                    {
                        DateTime? fromDate = item.FieldValue != null && item.FieldValue.ToString() == "null" ? null : StringHelper.GetDate(item.FieldValue.ToString());

                        DateTime? toDate = item.FieldValue2 != null && item.FieldValue2.ToString() == "null" ? null : StringHelper.GetDate(item.FieldValue2.ToString().Split(';')[0]);

                        string category = item.FieldValue2 != null && item.FieldValue2.ToString() == "null" ? null : item.FieldValue2.ToString().Split(';')[1];

                        queryableData = queryableData.Where(d => (DbFunctions.TruncateTime(d.OpenDate) >= fromDate && DbFunctions.TruncateTime(d.OpenDate) <= toDate) && d.SentDate != null && d.RequestDate != null);
                        queryableData = this.SentQuotesKPIChartFilter_Query(queryableData, category);
                    }

                    if (item.FieldName == "ChartAcceptedDateFilter")
                    {

                        string FromDate = item.FieldValue.ToString() == "null" ? null : item.FieldValue.ToString();
                        string ToDate = item.FieldValue2.ToString() == "null" ? null : item.FieldValue2.ToString();
                        DateTime? FromDateOBJ = StringHelper.GetDate(FromDate);
                        if (FromDate == null)
                        {
                            FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        DateTime? ToDateOBJ = StringHelper.GetDate(ToDate);
                        if (ToDateOBJ == null)
                        {
                            ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }


                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) >= FromDateOBJ && System.Data.Entity.DbFunctions.TruncateTime(d.AcceptedDate) <= ToDateOBJ);
                            
                        
                    }

                    if (item.FieldName == "ChartDeclinedDateFilter")
                    {                        
                        string FromDate = item.FieldValue.ToString() == "null" ? null : item.FieldValue.ToString();
                        string ToDate = item.FieldValue2.ToString() == "null" ? null : item.FieldValue2.ToString();
                        DateTime? FromDateOBJ = StringHelper.GetDate(FromDate);
                        if (FromDate == null)
                        {
                            FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        DateTime? ToDateOBJ = StringHelper.GetDate(ToDate);
                        if (ToDateOBJ == null)
                        {
                            ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }
                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) >= FromDateOBJ && System.Data.Entity.DbFunctions.TruncateTime(d.DeclinedDate) <= ToDateOBJ);                                                   
                    }

                    if (item.FieldName == "OpenQuotes")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            queryableData = queryableData.Where(d => !d.IsClosed);
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
                                case "QT_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }

                                case "QT_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }

                                case "QT_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                            }

                            queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.OpenDate) <= date2);
                        }
                    }

                    if (item.FieldName == "NotConnectedOpportunity")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            queryableData = queryableData.Where(d => d.OpportunityId == null);
                        }
                    }

                    if (item.FieldName == "TopQuotes")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            QuoteStageRepository quoteStageRepository = new QuoteStageRepository(tenant);
                            string stage1Id = quoteStageRepository.GetQuoteStageIdByCode("QTDC", tenant);
                            string stage2Id = quoteStageRepository.GetQuoteStageIdByCode("QTAC", tenant);

                            if (!string.IsNullOrEmpty(stage1Id))
                            {
                                queryableData = queryableData.Where(d => d.StageId != stage1Id);
                            }

                            if (!string.IsNullOrEmpty(stage2Id))
                            {
                                queryableData = queryableData.Where(d => d.StageId != stage2Id);
                            }
                        }
                    }

                    if (item.FieldName == "RoutingRatesAgentId")
                    {
                        if (item.FieldValue != null)
                        {
                            string agentId = item.FieldValue as string;
                            
                            queryableData = queryableData.Where(d => d.QuoteTypeCode == "A" || (d.QuoteTypeCode == "P" && d.AgentId == agentId));                           
                        }
                    }

                    if (item.FieldName == "IsShowingUsedSpotRateQuotes")
                    {
                        bool isShowingUsedQuotes = false;

                        if (item.FieldValue != null)
                        {
                            isShowingUsedQuotes = Convert.ToBoolean(item.FieldValue);
                        }

                        if (isShowingUsedQuotes)
                        {
                            
                        }

                        else
                        {
                            queryableData = queryableData.Where(d => d.QuoteTypeCode == "P" || (d.QuoteTypeCode == "A" && (d.UsageCount == 0 || d.UsageCount == null)));
                        }
                    }

                    if (item.FieldName == "IsShowingExpiredQuotes")
                    {
                        bool isShowingExpiredQuotes = false;

                        if (item.FieldValue != null)
                        {
                            isShowingExpiredQuotes = Convert.ToBoolean(item.FieldValue);
                        }

                        if (!isShowingExpiredQuotes)
                        {
                            queryableData = queryableData.Where(d => d.ExpirationDate == null || d.ExpirationDate >= DateTime.Now);
                        }
                        else
                        {
                            queryableData =  queryableData.Where(d => d.ExpirationDate == null || d.ExpirationDate >= DateTime.Now || d.ExpirationDate < DateTime.Now);
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

        private IQueryable<Quote> SentQuotesKPIChartFilter_Query(IQueryable<Quote> queryableData, string category)
        {
            if (category == "< 1d")
            {
                queryableData = queryableData.Where(a => (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value < 1);
            }
            else if (category == "1-2 d")
            {
                queryableData = queryableData.Where(a => (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value >= 1 && (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value <=2 );
            }
            else if (category == "3-4 d")
            {
                queryableData = queryableData.Where(a => (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value >= 3 && (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value <= 4);
            }
            else if (category == "5-6 d")
            {
                queryableData = queryableData.Where(a => (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value >= 5 && (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value <= 6);
            }
            else if (category == "7-8 d")
            {
                queryableData = queryableData.Where(a => (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value >= 7 && (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value <= 8);
            }
            else if (category == "9+ d")
            {
                queryableData = queryableData.Where(a => (System.Data.Entity.DbFunctions.DiffDays(a.RequestDate, a.SentDate)).Value >= 9);
            }

            return queryableData;
        }
    }
}