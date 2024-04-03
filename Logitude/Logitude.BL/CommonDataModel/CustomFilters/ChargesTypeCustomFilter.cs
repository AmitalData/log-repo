using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class ChargesTypeCustomFilter
    {
        public int Tenant { get; set; }
        public ChargesTypeCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<ChargesType> GetFilteredQuery(QueryOperations operations, IQueryable<ChargesType> queryableData)
        {



            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ChargeTypesByDirectionFilter")
                    {
                        queryableData = this.FilterByDirection(queryableData, item);
                    }
                }
            }
           
            return queryableData;
        }

        private IQueryable<ChargesType> FilterByDirection(IQueryable<ChargesType> queryableData, QueryFilterItem item)
        {
            string directionCode = item.FieldValue.ToString();
            switch (directionCode)
            {
                case "E":
                    {
                        queryableData = queryableData.Where(d => !d.IsDirectionRestricted || (d.IsDirectionRestricted && d.IsActiveInExport));
                        break;
                    }

                case "I":
                    {
                        queryableData = queryableData.Where(d => !d.IsDirectionRestricted || (d.IsDirectionRestricted && d.IsActiveInImport));
                        break;
                    }

                case "D":
                    {
                        queryableData = queryableData.Where(d => !d.IsDirectionRestricted || (d.IsDirectionRestricted && d.IsActiveInDomestic));
                        break;
                    }

                case "R":
                    {
                        queryableData = queryableData.Where(d => !d.IsDirectionRestricted || (d.IsDirectionRestricted && d.IsActiveInDrop));
                        break;
                    }
            }
            return queryableData;
        }
    }
}
