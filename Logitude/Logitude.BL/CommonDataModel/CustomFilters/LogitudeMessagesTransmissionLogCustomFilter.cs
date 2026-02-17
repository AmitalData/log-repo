using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class LogitudeMessagesTransmissionLogCustomFilter
    {
        private int tenant;
        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public LogitudeMessagesTransmissionLogCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<LogitudeMessagesTransmissionLog> GetFilteredQuery(QueryOperations operations, IQueryable<LogitudeMessagesTransmissionLog> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
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
                                case "FWB_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FWB");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }

                                case "FWB_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FWB");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }

                                case "FWB_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FWB");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }

                                case "FHL_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FHL");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }

                                case "FHL_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FHL");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }

                                case "FHL_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FHL");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }

                                case "FFR_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FFR");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }

                                case "FFR_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FFR");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }

                                case "FFR_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => d.MessageTypeCode == "FFR");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.SentDate) <= date2);
                                        break;
                                    }
                            }
                        }
                    }

                    else if (item.FieldName == "ActivityStatusChartFilter")
                    {
                        if (item.FieldValue != null && item.FieldValue2 != null)
                        {
                            int month = Convert.ToInt32(item.FieldValue);
                            int year = Convert.ToInt32(item.FieldValue2);

                            queryableData = queryableData.Where(d => (d.SentDate.Value.Month == month && d.SentDate.Value.Year == year));
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
