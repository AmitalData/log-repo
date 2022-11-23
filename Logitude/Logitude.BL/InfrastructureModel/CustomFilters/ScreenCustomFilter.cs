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
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(s => s.ObjectTable.ParentObjectTableId == filterFieldId 
                                                                  && s.ObjectTable.IsCustom
                                                                  && s.ObjectTable.AvailableInCustomization
                                                                  && !s.Inactive 
                                                                  && s.Type == "Grid");
                        }
                    }
                }
            }

            return queryableData;
        }

       
    }
}
