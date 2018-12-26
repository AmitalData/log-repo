

using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CustomsShipperCustomFilter
    {
        private int tenant;
        public CustomsShipperCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<CustomsShipper> GetFilteredQuery(QueryOperations operations, IQueryable<CustomsShipper> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

        
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {

                    if (item.FieldName == "DepositionsDateFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            DateTime todayDate = DateTime.Now.Date;
                            if (item.FieldValue.ToString() == "InValidDepositions")
                            {

                                queryableData = queryableData.Where(d => d.ValidityEndDate < todayDate);
                            }
                            else if (item.FieldValue.ToString() == "EndNext30Days")
                            {
                                DateTime next30DaysDate = todayDate.AddDays(30);

                                queryableData = queryableData.Where(d => d.ValidityEndDate > todayDate &&   d.ValidityEndDate < next30DaysDate);
                            }

                        }

                    }
                }
            }

            return queryableData;
        }
    }
}
