using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.QuoteModel.CustomFilters
{
    public class QuoteTemplateCustomFilter
    {
        public QuoteTemplateCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        private int tenant;
        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<QuoteTemplate> GetFilteredQuery(QueryOperations operations, IQueryable<QuoteTemplate> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "TemplateTypeCode")
                    {
                        string templateTypeCode = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(templateTypeCode))
                        {
                            queryableData = queryableData.Where(c => c.TemplateTypeCode == templateTypeCode);
                        }
                    }

                 
                }
            }

            return queryableData;
        }
    }
}