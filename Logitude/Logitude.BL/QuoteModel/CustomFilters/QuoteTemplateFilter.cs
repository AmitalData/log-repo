using Simplog.Data.QuoteModel.EntityPOCOs;

using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.QuoteModel.CustomFilters
{
    public class QuoteTemplateFilter
    {

        private int tenant;
        public QuoteTemplateFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<QuoteTemplate> GetQuoteTemplateFilteredQuery(QueryOperations operations, IQueryable<QuoteTemplate> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
         
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {


                    if (item.FieldName == "Name")
                    {


                        if (item.FieldName == "Name")
                        {
                            string value = item.FieldValue as string;
                            if (queryableData.Count() != 0)
                            {
                                queryableData = queryableData.Where(
                                    d => d.Name.StartsWith(value)

                                        );
                            }
                        }
                    } }}

                    return queryableData;
           
            }



       

    }
}
