
using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class AgentSharedManifestCustomFilter
    {
        public AgentSharedManifestCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        private int tenant;
        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<AgentSharedManifest> GetFilteredQuery(QueryOperations operations, IQueryable<AgentSharedManifest> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "BarDataCustomFilter")
                    {
                        string value = item.FieldValue as string;
                   
                        if (!string.IsNullOrEmpty(value))
                        {
                            string[] result = value.Split('@');
                            string dates = result.Length >0? result[0]:"";
                            string type = result.Length > 1? result[1] : "";

                            if (!string.IsNullOrEmpty(dates) && !string.IsNullOrEmpty(type))
                            {
                                string date1 = String.Empty;
                                string date2 = String.Empty;
                                DateTime? dateTime1 = null;
                                DateTime? dateTime2 = null;
                                int year = DateTime.Now.Year;


                                if (type == "1" || type == "2")
                                {
                                    if (dates.Contains("-"))
                                    {

                                        date1 = dates.Split('-').Length > 0 && !string.IsNullOrEmpty(dates.Split('-')[0]) ? dates.Split('-')[0] : "";
                                        date2 = dates.Split('-').Length > 1 && !string.IsNullOrEmpty(dates.Split('-')[1]) ? dates.Split('-')[1] : "";

                                        if (!string.IsNullOrEmpty(date1) && date1.Split('/').Length > 1 && !string.IsNullOrEmpty(date1.Split('/')[0]) && !string.IsNullOrEmpty(date1.Split('/')[1]))
                                        {
                                            dateTime1 = new DateTime(year, Int32.Parse(date1.Split('/')[1]), Int32.Parse(date1.Split('/')[0]));
                                        }

                                        if (!string.IsNullOrEmpty(date2) && date2.Split('/').Length > 1 && !string.IsNullOrEmpty(date2.Split('/')[0]) && !string.IsNullOrEmpty(date2.Split('/')[1]))
                                        {
                                            dateTime2 = new DateTime(year, Int32.Parse(date2.Split('/')[1]), Int32.Parse(date2.Split('/')[0]));
                                        }

                                        if (dateTime1 != null && dateTime2 != null)
                                        {
                                            DateTime beforeDateTime = (DateTime)dateTime1;
                                            beforeDateTime = beforeDateTime.AddDays(-1);

                                            DateTime nextDateTime = (DateTime)dateTime2;
                                            nextDateTime = nextDateTime.AddDays(1);

                                            queryableData = queryableData.Where(c => c.CreateDate > beforeDateTime && c.CreateDate < nextDateTime);
                                        }
                                    }
                                }
                                else if (type == "0")
                                {
                                    date1 = dates;

                                    if (!string.IsNullOrEmpty(date1) && date1.Split('/').Length > 1 && !string.IsNullOrEmpty(date1.Split('/')[0]) && !string.IsNullOrEmpty(date1.Split('/')[1]))
                                    {
                                        dateTime1 = new DateTime(year, Int32.Parse(date1.Split('/')[1]), Int32.Parse(date1.Split('/')[0]));
                                    }
                                    if (dateTime1 != null)
                                    {
                                        DateTime beforeDateTime = (DateTime)dateTime1;
                                        beforeDateTime = beforeDateTime.AddDays(-1);

                                        DateTime nextDateTime = (DateTime)dateTime1;
                                        nextDateTime = nextDateTime.AddDays(1);
                                        queryableData = queryableData.Where(c => c.CreateDate > beforeDateTime && c.CreateDate < nextDateTime);
                                    }
                                }
                                else if (type == "3")
                                {
                                    date1 = dates;
                                    if (!string.IsNullOrEmpty(date1) && date1.Split('/').Length > 1 && !string.IsNullOrEmpty(date1.Split('/')[0]) && !string.IsNullOrEmpty(date1.Split('/')[1]))
                                    {
                                        dateTime1 = new DateTime(Int32.Parse(date1.Split('/')[1]), Int32.Parse(date1.Split('/')[0]), 1);
                                    }
                                    if (dateTime1 != null)
                                    {
                                        DateTime beforeDateTime = (DateTime)dateTime1;
                                        beforeDateTime = beforeDateTime.AddDays(-1);

                                        DateTime nextDateTime = (DateTime)dateTime1;
                                        nextDateTime = nextDateTime.AddMonths(1);
                                        queryableData = queryableData.Where(c => c.CreateDate > beforeDateTime && c.CreateDate < nextDateTime);
                                    }
                                }
                            }


                        }
                    }

                }
            }

            return queryableData;
        }
    }
}