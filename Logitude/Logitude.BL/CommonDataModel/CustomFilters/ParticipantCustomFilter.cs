using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class ParticipantCustomFilter
    {
        private int tenant;
        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public ParticipantCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<Participant> GetFilteredQuery(QueryOperations operations, IQueryable<Participant> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "CountryId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.Card.CountryId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "PaymentTermId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.Card.PaymentTermId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "InvoiceCurrencyId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.Card.InvoiceCurrencyId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "VatTypeId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.Card.VatTypeId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "DailySpotlightFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            string code = item.FieldValue.ToString();

                            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                            DateTime date1 = todayDate;
                            DateTime date2 = todayDate;

                            switch (code)
                            {
                                case "PART_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);

                                        queryableData = queryableData.Where(d => !d.Card.InActive);
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.Card.CreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.Card.CreateDate) <= date2);
                                        break;
                                    }

                                case "PART_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => !d.Card.InActive);
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.Card.CreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.Card.CreateDate) <= date2);
                                        break;
                                    }

                                case "PART_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => !d.Card.InActive);
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.Card.CreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.Card.CreateDate) <= date2);
                                        break;
                                    }

                                case "NEWP_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);

                                        queryableData = queryableData.Where(d => !d.Card.InActive);
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.RegistrationDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.RegistrationDate) <= date2);
                                        break;
                                    }

                                case "NEWP_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => !d.Card.InActive);
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.RegistrationDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.RegistrationDate) <= date2);
                                        break;
                                    }

                                case "NEWP_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);

                                        queryableData = queryableData.Where(d => !d.Card.InActive);
                                        queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.RegistrationDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.RegistrationDate) <= date2);
                                        break;
                                    }
                            }
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
