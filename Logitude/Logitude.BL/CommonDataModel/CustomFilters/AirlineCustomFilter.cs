using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class AirlineCustomFilter
    {
        private int tenant;
        public AirlineCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<Airline> GetFilteredQuery(QueryOperations operations, IQueryable<Airline> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "CountryId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.Card.CountryId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "PaymentTermId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.Card.PaymentTermId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "SearchFields")
                    {
                        string filterField = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterField))
                        {
                            queryableData = queryableData.Where(c => c.Card.SearchFields != null && c.Card.SearchFields.ToUpper().Contains(filterField.ToUpper()));
                        }
                    }

                    if (item.FieldName == "TTYPIMA")
                    {
                        string filterField = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterField))
                        {
                            if (filterField.ToUpper() == "GLSHK")
                            {
                                queryableData = queryableData.Where(d => d.GLSHKPIMA != null);
                            }

                            else
                            {
                                queryableData = queryableData.Where(d => d.TTY != null);
                            }
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
