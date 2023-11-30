
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.CustomFilters
{
    public class ObjectFieldCustomFilter
    {
        private int tenant;
        public ObjectFieldCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<ObjectField> GetFilteredQuery(QueryOperations operations, IQueryable<ObjectField> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "AllowedinAutomationConditions")
                    {
                        if (item.FieldValue != null)
                        {
                            queryableData = queryableData.Where(d => d.AllowedinAutomationConditions == true || d.IsCustom == true);
                        }
                    }
                    if (item.FieldName == "RecordType")
                    {
                        if (item.FieldValue != null)
                        {
                            queryableData = queryableData = queryableData.Where(d => string.IsNullOrEmpty(d.RecordType) || d.DisplayInAutomationAsEnitity || (!string.IsNullOrEmpty(d.RecordType) && d.RecordType.Contains(item.FieldValue.ToString())));
                        }
                    }
                    if (item.FieldName == "CanAutomateSetValue")
                    {
                        if (item.FieldValue != null)
                        {
                            queryableData = queryableData.Where(d => d.CanAutomateSetValue == true || d.IsCustom == true);
                        }
                    }

                    if (item.FieldName == "IsFullCustom")
                    {
                        if (item.FieldValue != null && item.FieldValue.GetType().Equals(typeof(bool)))
                        {
                            bool filterValue = Convert.ToBoolean(item.FieldValue);
                            queryableData = queryableData.Where(d => d.IsCustom == filterValue || d.ObjectTable.IsCustom == filterValue);
                        }
                    }
                }
            }

            return queryableData;
        }

    }
}
