using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class AWBDescriptionOfGoodsCustomFilter
    {
        private int tenant;
        public AWBDescriptionOfGoodsCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<AWBDescriptionOfGoods> GetFilteredQuery(QueryOperations operations, IQueryable<AWBDescriptionOfGoods> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "AirlineCode")
                    {
                        string str = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(str))
                        {
                            queryableData = queryableData.Where(c => c.AirlineCode == str);
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
