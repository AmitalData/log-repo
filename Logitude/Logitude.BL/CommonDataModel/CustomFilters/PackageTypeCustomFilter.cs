using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class PackageTypeCustomFilter
    {
        private int tenant;
        public PackageTypeCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<PackageType> GetFilteredQuery(QueryOperations operations, IQueryable<PackageType> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            DateTime todayDate = DateTime.Now.Date;
            DateTime tomorrowDate = DateTime.Now.AddDays(1).Date;
            DateTime afterTommorow = tomorrowDate.AddDays(1).Date;
            DateTime dueDate = DateTime.Now.Date;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {

                    if (item.FieldName == "TransportModeId")
                    {
                        if (item.FieldValue.ToString() == "A")
                        {
                            queryableData = queryableData.Where(d => d.IsAir == true);
                        }

                    }
                }
            }

            return queryableData;
        }
    }
}
