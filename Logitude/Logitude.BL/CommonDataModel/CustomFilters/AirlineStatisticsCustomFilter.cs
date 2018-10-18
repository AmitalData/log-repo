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
    public class AirlineStatisticsCustomFilter
    {
        private int tenant;
        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public AirlineStatisticsCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<AirlineStatistics> GetFilteredQuery(QueryOperations operations, IQueryable<AirlineStatistics> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "TopParticipantsFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();
                            int lastDays = Convert.ToInt32(item.FieldValue2);

                            int days = lastDays + 1;
                            DateTime lastDate = DateTime.Today.Date.AddDays(days);

                            switch (code)
                            {
                                case "FWB":
                                    {
                                        queryableData = queryableData.Where(d => d.MessageType == "FWB");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) >= lastDate);
                                        break;
                                    }

                                case "FHL":
                                    {
                                        queryableData = queryableData.Where(d => d.MessageType == "FHL");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) >= lastDate);
                                        break;
                                    }

                                case "FFR":
                                    {
                                        queryableData = queryableData.Where(d => d.MessageType == "FFR");
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) >= lastDate);
                                        break;
                                    }
                            }
                        }
                    }

                    else if (item.FieldName == "BookingsInProgressFilter")
                    {
                        string code = item.FieldValue.ToString();

                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        DateTime date1 = todayDate.AddDays(-100);
                        DateTime date2 = todayDate.AddHours(23).AddMinutes(59).AddSeconds(59);

                        if (!string.IsNullOrEmpty(code))
                        {
                            queryableData = queryableData.Where(a => !string.IsNullOrEmpty(a.BookingId) && a.MessagingStatus == code);
                            queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.EntitiyCreateDate) <= date2);
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
