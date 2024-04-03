using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class ScreenCustomFilter
    {
        private int tenant;
        public ScreenCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }



        public IQueryable<Screen> GetFilteredQuery(QueryOperations operations, IQueryable<Screen> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ChildScreenGrid")
                    {
                        string fieldValue = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(fieldValue))
                        {
                            queryableData = queryableData.Where(s => s.ObjectTable.ParentObjectTableId == fieldValue
                                                                  && s.ObjectTable.IsCustom
                                                                  && s.ObjectTable.AvailableInCustomization
                                                                  && !s.Inactive 
                                                                  && s.Type == "Grid"
                                                                  && s.Tenant == Tenant);
                        }
                    }
                    else if (item.FieldName == "ObjectTableId")
                    {
                        queryableData = FilterDataByObjectTableId(queryableData, item);
                    }
                }
            }

            return queryableData;
        }

        private IQueryable<Screen> FilterDataByObjectTableId(IQueryable<Screen> queryableData, QueryFilterItem item)
        {
            string fieldValue = item.FieldValue as string;

            if (!string.IsNullOrEmpty(fieldValue))
            {
                queryableData = queryableData.Where(s => s.ObjectTableId == fieldValue
                                                      && !s.Inactive
                                                      && s.Tenant == Tenant);
            }

            return queryableData;
        }

    }
}
