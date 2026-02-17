using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class PortCustomFilter
    {
        public PortCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant { get; set; }

        public IQueryable<Port> GetFilteredQuery(QueryOperations operations, IQueryable<Port> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "Country")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.Country.EnglishName.ToUpper().StartsWith(value.ToUpper()) || d.Country.Code.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "CountryCodeOrName")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.Country.EnglishName.ToUpper().StartsWith(value.ToUpper()) || d.Country.Code.ToUpper().StartsWith(value.ToUpper()) || d.Country.LocalName.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "CodeOrName")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d => d.EnglishName.ToUpper().StartsWith(value.ToUpper()) || d.Code.ToUpper().StartsWith(value.ToUpper()) || d.LocalName.ToUpper().StartsWith(value.ToUpper()));
                        }
                    }

                    if (item.FieldName == "TransportModeId")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            if (value == "A")
                            {
                                queryableData = queryableData.Where(d => d.IsAir == true);
                            }

                            if (value == "O")
                            {
                                queryableData = queryableData.Where(d => d.IsOcean == true);
                            }

                            if (value == "I")
                            {
                                queryableData = queryableData.Where(d => d.IsInland == true);
                            }


                        }
                    }
                }


            }

            return queryableData;
        }
    }
}
