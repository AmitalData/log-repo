using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.DataContracts;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.DataContracts;
using WebFreight.Web.CommonDataModel.DomainServices;

namespace WebFreight.Web.App_Code
{
    public class CustomersDataController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public CustomerPM GetSingleCustomerPM(string singleCustomerId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            CustomerQuery entityQuery = new CustomerQuery(tenant);
            CustomerPM entityPM = entityQuery.GetSinglePM(singleCustomerId, tenant);

            return entityPM;
        }

        public List<CustomerList> PostFilteredCustomers(int tenant, CustomerFilters filters)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            CustomerRepository customerRepository = new CustomerRepository(tenant);
            CustomerQuery customerQuery = new CustomerQuery(customerRepository);

            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);

            QueryOperations queryOperations = new QueryOperations();
            queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, true);

            if (filters.IsMyCustomers)
            {
                queryOperations.SetFilter("MyCustomers", true, true, "Contains", null, true);
            }

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            CustomerCustomFilter customfilters = new CustomerCustomFilter(tenant);
            IQueryable<CustomersDataView> customers = customerRepository.GetCustomersDataViews(tenant);
            customers = customfilters.GetFilteredQuery(queryOperations, customers);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            customers = filter.GetFilteredQuery<CustomersDataView>(nonListQueryOperation, customers);
            int skippedCustomers = 0; //queryOperations.PageIndex;

            var query2 = from customer in customers
                         select new CustomerList()
                         {
                             Code = customer.Code,
                             EnglishName = customer.EnglishName,
                             LocalName = customer.LocalName,
                             ReceivablesAccountingCard = customer.ReceivablesAccountingCard,
                             PayablesAccountingCard = customer.PayablesAccountingCard,
                             InActive = customer.InActive,
                             Notes = customer.Notes,
                             Website = customer.Website,
                             SalesmanUserId = customer.SalesmanUserId,
                             Id = customer.Id,
                             PaymentTermId = customer.PaymentTermId,
                             CreateDate = customer.CreateDate,
                             Tenant = customer.Tenant,
                             VatNumber = customer.VatNumber,
                             SearchFields = customer.SearchFields,
                             PaymentTermEnglishName = customer.PaymentTermEnglishName ,
                             InvoiceCurrencyId = customer.InvoiceCurrencyId,
                             LastShipmentDate = customer.LastShipmentDate,
                             StartWorkingDate = customer.StartWorkingDate,
                             StartWorkingManuallySet = customer.StartWorkingManuallySet,
                             AccountManagerUserEnglishName = customer.AccountManagerUserEnglishName,
                             SalesmanUserEnglishName = customer.SalesmanUserEnglishName,
                             CityName = customer.CityName,
                             VatTypeId = customer.VatTypeId,
                             Field1 = customer.Field1,
                             Field2 = customer.Field2,
                             Field3 = customer.Field3,
                             Field4 = customer.Field4,
                             Field5 = customer.Field5,
                             Field6 = customer.Field6,
                             Field7 = customer.Field7,
                             Field8 = customer.Field8,
                             Field9 = customer.Field9,
                             Field10 = customer.Field10,
                             RankCode = customer.RankCode,
                             RankName = customer.RankName,
                             TeamName = customer.TeamName,
                             SharedLogisticsInvitationStatusName = customer.SharedLogisticsInvitationStatusName,
                             CargoTrackingInvitationStatusName = customer.CargoTrackingInvitationStatusName,
                             LastLoginDate = customer.LastLoginDate,
                             LastLoginDateViaPC = customer.LastLoginDateViaPC,
                             LastLoginDateViaMobile = customer.LastLoginDateViaMobile,
                             InvitationDate = customer.InvitationDate,
                             CargoTrackingInvitationDate = customer.CargoTrackingInvitationDate,

                             ClassifierName = customer.ClassifierName,
                             CollectorName = customer.CollectorName,
                             //ClassifierName = TenantContext.Current.ContactContext.UserLists.Where(d => d.Id == ClassifierId).FirstOrDefault().EnglishName;
                             //viewModel.CollectorName = TenantContext.Current.ContactContext.UserLists.Where(d => d.Id == CollectorId).FirstOrDefault().EnglishName;
                         };

            query2 = filter.GetFilteredQuery<CustomerList>(listQueryOperation, query2);

            IQueryable<CustomerList> bigQuery = query2;

            bigQuery = bigQuery.OrderBy(d => d.EnglishName);

            bigQuery = bigQuery.Skip(skippedCustomers);
            if (filters.Take != 0)
            {
                bigQuery = bigQuery.Take(filters.Take);
            }
            else
            {
                bigQuery = bigQuery.Take(25);
            }

            List<CustomerList> listQuery = bigQuery.ToList();

            //CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            //customFieldResolver.SetCustomFieldsValues("Customer", tenant, listQuery.Cast<object>().ToList());

            return bigQuery.ToList();
        }

        public List<CustomerProductActualDataPM> GetCustomerActualData(string actualDataCustomerId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            CustomerProductActualDataQuery query = new CustomerProductActualDataQuery(tenant);
            return query.GetCustomerActualData(actualDataCustomerId, DateTime.Now.Year, DateTime.Now.Month, tenant);
        }

        public List<ChartingDataClass> GetCustomerShipmentsData(string customerid, int tenant, int lastMonthsCount)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);

            List<DashBoardClass> list = shipmentQuery.GetShipmentsByMonthDashBoard(null,lastMonthsCount,0, tenant, customerid);

            List<ChartingDataClass> data = new List<ChartingDataClass>();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            
            if (lastMonthsCount == -12)
            {               
                data = (from a in list
                            group a by new { a.month, a.year } into g
                            select new ChartingDataClass()
                            {
                                Id = g.Key.month + "-" + g.Key.year,
                                Month = g.Key.month,
                                Year = g.Key.year,
                                LabelProperty = g.Key.month + "-" + g.Key.year,
                                Shipments = g.Count(),
                                GrossWeight = g.Sum(s => s.sumGrossWeight),
                                ChargeableWeight = g.Sum(s => s.sumChargeableWeight),
                                ProfitInLocal = g.Sum(s => s.totalProfitInLocalCurrency),
                                ProfitInProfit = g.Sum(s => s.totalProfitInProfitCurrency),
                                ReceivablesInLocal = g.Sum(s => s.ReceivablesInLocalCurrency),
                                ReceivablesInProfit = g.Sum(s => s.ReceivablesInProfitCurrency),
                            }).ToList();

                DateTime date1 = todayDate.AddMonths(-11);

                while (date1 <= todayDate)
                {
                    string key = date1.Month + "-" + date1.Year;

                    ChartingDataClass item = data.Where(d => d.Id == key).FirstOrDefault();
                    if (item == null)
                    {
                        data.Add(new ChartingDataClass()
                        {
                            Id=  key,
                            Month = date1.Month,
                            Year = date1.Year,
                            LabelProperty = key,
                            Shipments = 0,
                            GrossWeight = 0,
                            ChargeableWeight = 0,
                            ProfitInProfit = 0,
                            ProfitInLocal = 0,
                            ReceivablesInLocal = 0,
                            ReceivablesInProfit = 0,
                        });
                    }

                    date1 = date1.AddMonths(1);
                }

            }

            else
            {               
                data = (from a in list
                            group a by a.year into g
                            select new ChartingDataClass()
                            {
                                Id = "" + g.Key,
                                Month = 0,
                                Year = g.Key,
                                LabelProperty = "" + g.Key,
                                Shipments = g.Count(),
                                GrossWeight = g.Sum(s => s.sumGrossWeight),
                                ChargeableWeight = g.Sum(s => s.sumChargeableWeight),
                                ProfitInLocal = g.Sum(s => s.totalProfitInLocalCurrency),
                                ProfitInProfit = g.Sum(s => s.totalProfitInProfitCurrency),
                                ReceivablesInLocal = g.Sum(s => s.ReceivablesInLocalCurrency),
                                ReceivablesInProfit = g.Sum(s => s.ReceivablesInProfitCurrency),
                            }).ToList();

                DateTime date1 = todayDate.AddYears(-2);

                while (date1 <= todayDate)
                {
                    string key = date1.Year.ToString();

                    ChartingDataClass item = data.Where(d => d.Id == key).FirstOrDefault();
                    if (item == null)
                    {
                        data.Add(new ChartingDataClass()
                        {
                            Id = key,
                            Month = 0,
                            Year = date1.Year,
                            LabelProperty = key,
                            Shipments = 0,
                            GrossWeight = 0,
                            ChargeableWeight = 0,
                            ProfitInProfit = 0,
                            ProfitInLocal = 0,
                            ReceivablesInLocal = 0,
                            ReceivablesInProfit = 0,
                        });
                    }

                    date1 = date1.AddYears(1);
                }
            }

            return data.OrderBy(d => d.Year).ThenBy(d=>d.Month).ToList();
        }

        public List<ChartingDataClass> GetQuoteStatusChartData(string quotesChartCustomerId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<ChartingDataClass> result = new List<ChartingDataClass>();
            
            QuoteRepository quoteRepository = new QuoteRepository(tenant);
            IQueryable<Quote> dataSourceQuery = quoteRepository.GetQuotes(tenant);

            result = (from d in dataSourceQuery
                      where !d.IsCancelled && d.CustomerId == quotesChartCustomerId
                      group d by new { d.StageId, d.Stage.Name } into g
                      select new ChartingDataClass()
                      {
                          Id = g.Key.StageId,
                          StringProperty = g.Key.Name,
                          IntegerProperty = g.Count()
                      }).ToList();

            return result;
        }

        public List<AddressPM> GetAddressesbyCardId(string addressesCustomerId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            AddressQuery addressQuery = new AddressQuery(tenant);
            List<AddressPM> result = addressQuery.GetAddressesByCardId(addressesCustomerId, tenant);            
            return result;
        }

        public List<ContactPM> GetContactsbyCardId(string contactsCustomerId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ContactQuery contactQuery = new ContactQuery(tenant);
            List<ContactPM> result = contactQuery.GetContactsbyCardId(contactsCustomerId, tenant).ToList();
            return result;
        }

        public CRMMoneyInformation GetCRMMoneyInformation(string CRMMoneyCustomerId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentRepository shipmentrep = new ShipmentRepository(tenant);
            ARInvoiceRepository invoiceRep = new ARInvoiceRepository(tenant);
            ARPaymentRepository paymentrep = new ARPaymentRepository(tenant);
            ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(invoiceRep);

            CRMMoneyInformation moneyinfo = new CRMMoneyInformation();
            moneyinfo.ARPayments = paymentrep.GetARPaymentForCustomer(tenant, CRMMoneyCustomerId);
            moneyinfo.InvoicesDue = arInvoiceQuery.GetInvoicesDueForCustomer(tenant, CRMMoneyCustomerId);
            moneyinfo.OpenARInvoices = invoiceRep.GetOpenARInvoicesForCustomer(tenant, CRMMoneyCustomerId);
            moneyinfo.OpenReceivables = shipmentrep.GetOpenReceivablesForCustomer(tenant, CRMMoneyCustomerId);

            return moneyinfo;
        }


        public CRMDataCounts GetDataCountsForCRM(string customerId, int tenant)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            PartnersDomainService partnersDomain = new PartnersDomainService();
            CRMDataCounts result = partnersDomain.GetDataCountsForCRM(tenant, customerId);

            return result;
        }


        //
    }
}