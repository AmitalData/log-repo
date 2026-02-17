using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.CustomFilters
{
    public class GLAccountCustomFilter
    {
        private int tenant;
        public GLAccountCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }



        public IQueryable<GLAccount> GetFilteredQuery(QueryOperations operations, IQueryable<GLAccount> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {

                    if (item.FieldName == "Name")
                    {
                        string tString = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(tString))
                        {
                            queryableData = queryableData.Where(c => c.EnglishName.StartsWith(tString) || c.LocalName.StartsWith(tString));
                        }
                    }

                    if (item.FieldName == "DisplayNumberOrName")
                    {
                        string value = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(value))
                        {
                            queryableData = queryableData.Where(d => d.EnglishName.StartsWith(value) || d.DisplayNumber.StartsWith(value) || d.LocalName.StartsWith(value));
                        }
                    }
                    if (item.FieldName == "IsParent")
                    {
                        string value = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(value))
                        {
                           
                            IQueryable<GLAccount> query = queryableData.Where(d => d.ParentAccountId != null);
                            List<string> ids = (from d in query
                                                   select d.ParentAccountId).ToList();
                            queryableData = queryableData.Where(d =>! ids.Contains(d.Id));
                                       
                        }
                    }
                    if (item.FieldName == "SingleAndMultiCurrencyAccount")
                    {
                        string value = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(value))
                        {
                            queryableData = queryableData.Where(d => d.CurrencyId == value || d.IsMultiCurrency == true);
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
