using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System.Data.Entity.Core.Objects;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.Utils;
using System.Data.Entity;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CustomerCustomFilter
    {
        private int tenant;
        public CustomerCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<CustomersDataView> GetFilteredQuery(QueryOperations operations, IQueryable<CustomersDataView> queryableData)
        {            
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "Occasion_CustomersQuery")
                    {
                        string value = item.FieldValue as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.TrimEnd(',');
                            var contactIds = value.Split(',');
                            CardContactRepository cardContactRepository = new CardContactRepository(tenant);
                            var customersId = cardContactRepository.GetCardsContactsForContactIds_Ids(contactIds.ToList(), tenant);
                            var customers = customersId.Split(',');
                            if (customers.Count() > 0)
                            {
                                queryableData = queryableData.Where(c => customers.Contains(c.Id));
                            }
                        }
                    }

                    if (item.FieldName == "MyCustomers")
                    {
                        string loggedUser = AuthenticationUtil.GetAuthenticatedUser();
                        ContactRepository contactRep = new ContactRepository(tenant);
                        Contact loggedContact = contactRep.GetSingleContactByEmail(loggedUser, tenant);
                        queryableData = queryableData.Where(d => d.SalesmanUserId == loggedContact.Id && d.IsCustomer == true && d.InActive == false);
                    }

                    if (item.FieldName == "MyCustomersAsAccountManager")
                    {
                        string loggedUser = AuthenticationUtil.GetAuthenticatedUser();
                        ContactRepository contactRep = new ContactRepository(tenant);
                        Contact loggedContact = contactRep.GetSingleContactByEmail(loggedUser, tenant);

                        queryableData = queryableData.Where(d => d.AccountManagerUserId == loggedContact.Id && d.IsCustomer == true && d.InActive == false);
                    }

                    if (item.FieldName == "PaymentTermId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.PaymentTermId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "CustomersByLastActivity")
                    {
                        queryableData = queryableData.Where(d => d.LastShipmentDate != null);
                    }

                    if (item.FieldName == "CRMChartFilter")
                    {
                        bool? value = item.FieldValue as bool?;

                        if (value == true)
                        {
                            queryableData = queryableData.Where(d => d.SalesmanUserId != null && !d.InActive);
                        }
                    }

                    if (item.FieldName == "ChartCreateDateFilter")
                    {
                        string FromDate = item.FieldValue.ToString() == "null" ? null : item.FieldValue.ToString();
                        string ToDate = item.FieldValue2.ToString() == "null" ? null : item.FieldValue2.ToString();
                        DateTime? FromDateOBJ = StringHelper.GetDate(FromDate);
                        if (FromDate == null)
                        {
                            FromDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }

                        DateTime? ToDateOBJ = StringHelper.GetDate(ToDate);
                        if (ToDateOBJ == null)
                        {
                            ToDateOBJ = TenantServerConfigration.GetCurrentDateTime(tenant);
                        }


                        queryableData = queryableData.Where(d => DbFunctions.TruncateTime(d.CreateDate) >= FromDateOBJ && DbFunctions.TruncateTime(d.CreateDate) <= ToDateOBJ);


                    }

                    if (item.FieldName == "ReadyCustomers")
                    {
                        queryableData = queryableData.Where(d => d.CustomerStatusCode == "WAC" && d.IsCustomer);
                    }

                    if (item.FieldName == "PrimaryContactId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.PrimaryContactId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "CreatedByUserId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.CreatedByUserId == filterFieldId);
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
                                case "PO_TD":
                                case "CS_TD":
                                    {
                                        date1 = todayDate.AddDays(0);
                                        date2 = todayDate.AddDays(0);
                                        break;
                                    }
                                case "PO_YS":
                                case "CS_YS":
                                    {
                                        date1 = todayDate.AddDays(-1);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                                case "PO_LW":
                                case "CS_LW":
                                    {
                                        date1 = todayDate.AddDays(-7);
                                        date2 = todayDate.AddDays(-1);
                                        break;
                                    }
                            }

                            queryableData = queryableData.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= date2);
                        }
                    }

                    if (item.FieldName == "PotentialCustomers")
                    {
                        queryableData = queryableData.Where(d => d.CustomerStatusCode == "POT" && d.IsCustomer);
                    }

                    if (item.FieldName == "ActiveCustomers")
                    {
                        queryableData = queryableData.Where(d => d.CustomerStatusCode == "ACT" && d.IsCustomer);
                    }

                    if (item.FieldName == "InactiveCustomers")
                    {
                        queryableData = queryableData.Where(d => d.CustomerStatusCode == "INA" && d.IsCustomer);
                    }

                    if (item.FieldName == "SharedLogisticsCustomers")
                    {
                        queryableData = queryableData.Where(d => d.CustomerStatusCode == "ACT" && d.IsCustomer && !d.InActive);
                    }


                    if (item.FieldName == "CustomersBusinessUnitFilter")
                    {
                        string mySalesmanUserId = null;
                        string mySalesmanBusinessUnitId = null;

                        if (item.FieldValue != null)
                        {
                            mySalesmanUserId = item.FieldValue.ToString();
                        }

                        if (item.FieldValue2 != null)
                        {
                            mySalesmanBusinessUnitId = item.FieldValue2.ToString();
                        }

                        if (!string.IsNullOrEmpty(mySalesmanUserId))
                        {
                            queryableData = queryableData.Where(d =>d.SalesmanUserId == mySalesmanUserId);
                        }

                        if (!string.IsNullOrEmpty(mySalesmanBusinessUnitId))
                        {
                            queryableData = queryableData.Where(d =>d.SalesmanBusinessUnitId == mySalesmanBusinessUnitId);
                        }
                    }
                }
            }

            queryableData = GetFreelancerCustomers(queryableData, tenant);
            return queryableData;
        }

        public IQueryable<CustomersDataView> GetFreelancerCustomers(IQueryable<CustomersDataView> queryableData, int tenant)
        {
            var frlUtil = new FreelancerCustomersUtil(tenant);

            List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
            
            if (customersIds.Count > 0)
            {
                queryableData = queryableData.Where(d => customersIds.Contains(d.Id));
            }
            
            return queryableData;
        }
    }
}