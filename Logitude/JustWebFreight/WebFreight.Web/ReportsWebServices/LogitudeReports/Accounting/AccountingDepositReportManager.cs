using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using System.Data.Entity;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class AccountingDepositReportManager
    {
        private ARInvoiceDepositDataProvider aRInvoiceDepositDataProvider;
        private int tenant;
        private DateTime fromDate;
        private DateTime toDate;
        private string branchId = null;
        private string customerId = null;
        private bool isLocalCurrency = true;
        private string currencyId = null;
        private IQueryable<ARPayment> iQueryable;
        private IInvoiceContext invoiceContext;
        private ARInvoiceQuery arInvoiceQuery;
        private ARPaymentQuery arPaymentQuery;
        private ShipmentQuery shipmentQuery;

        public AccountingDepositReportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            this.aRInvoiceDepositDataProvider = new ARInvoiceDepositDataProvider();
            invoiceContext = InvoiceContext.GetContext(tenant);
            arInvoiceQuery = new ARInvoiceQuery(tenant);
            arPaymentQuery = new ARPaymentQuery(tenant);
            shipmentQuery=  new ShipmentQuery(tenant);
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            this.BuildReportFilters(iQueryOperations);
            this.GetData();
        }
        private void BuildReportFilters(QueryOperations queryOperations)
        {
            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_BranchId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BranchId").FirstOrDefault();
            QueryFilterItem filterItem_LocalCurrency = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LocalCurrency").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CustomerId").FirstOrDefault();
            QueryFilterItem filterItem_CurrencyId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CurrencyId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_BranchId != null)
            {
                if (filterItem_BranchId.FieldValue != null)
                {
                    branchId = filterItem_BranchId.FieldValue.ToString();
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItem_LocalCurrency != null)
            {
                if (filterItem_LocalCurrency.FieldValue != null)
                {
                    isLocalCurrency = (bool)filterItem_LocalCurrency.FieldValue;
                }
            }

            if (filterItem_CurrencyId != null)
            {
                if (filterItem_CurrencyId.FieldValue != null)
                {
                    currencyId = filterItem_CurrencyId.FieldValue.ToString();
                }
            }
        }
        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ARInvoiceDepositDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, this.aRInvoiceDepositDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private void LoadDataProvider()
        {
            this.FilterByQueryFilters();
            this.BuildReportData();
        }
        private void FilterByQueryFilters()
        {
            this.iQueryable = (from f in invoiceContext.ARPayments.Include("PaymentCurrency").Include("LocalCurrency").Include("AccountingPaymentMethod").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("BankAccountLite").Include("AccountingPaymentMethod")
                               where f.Tenant == tenant
                               select f);

            if (!string.IsNullOrEmpty(branchId))
            {
                iQueryable = iQueryable.Where(d => d.BranchId == branchId);
            }

            if (!string.IsNullOrEmpty(customerId))
            {
                iQueryable = iQueryable.Where(d => d.BillToId == customerId);
            }

            if (!isLocalCurrency)
            {
                if (!string.IsNullOrEmpty(currencyId))
                {
                    iQueryable = iQueryable.Where(d => d.PaymentCurrencyId == currencyId);
                }
            }

            iQueryable = (from d in iQueryable
                          where
                          (d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= fromDate && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= toDate)
                          ||
                          (d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= fromDate && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= toDate)
                          select d);
        }
        private void BuildReportData()
        {
            this.MapDirectFieldsOfDataProvider();
            this.FillARPaymentDataList();
            this.MapARPaymentDataListOfDataProvider();
        }

        private void MapDirectFieldsOfDataProvider()
        {
            this.aRInvoiceDepositDataProvider.FromDate = fromDate;
            this.aRInvoiceDepositDataProvider.ToDate = toDate;
            this.aRInvoiceDepositDataProvider.Logo = WebFreight.Web.DataProviders.General.GetLogo(tenant);
        }

        private void FillARPaymentDataList()
        {
            this.aRInvoiceDepositDataProvider.ARPaymentDataList = (from d in iQueryable
                                                                   select new ARPaymentDataProvider()
                                                                   {
                                                                       PaymentId = d.Id,
                                                                       ARPaymentNo = d.PaymentNo,
                                                                       PaymentRef = d.PaymentNo,
                                                                       PaymentReference = d.ChequeOrPaymentRef,
                                                                       Bank = d.Bank,
                                                                       ValueDate = d.ValueDate,
                                                                       PaymentMethodName = d.AccountingPaymentMethod == null ? null : d.AccountingPaymentMethod.Name,
                                                                       IssuedByUserName = d.CreatedByUser == null ? null : d.CreatedByUser.Contact.EnglishName,
                                                                       PaymentCurrencyCode = isLocalCurrency ? (d.LocalCurrency == null ? null : d.LocalCurrency.Code) : (d.PaymentCurrency == null ? null : d.PaymentCurrency.Code),
                                                                       Amount = isLocalCurrency ? d.AmountInLocalCurrency : d.AmountInPaymentCurrency,
                                                                       PaymentMethodRef = d.AccountingPaymentMethod.Code == "CH" || d.AccountingPaymentMethod.Code == "BT" || d.AccountingPaymentMethod.Code == "CC" ? d.ChequeOrPaymentRef : d.AccountingPaymentMethod.Code == "CA" ? "Cash" : d.AccountingPaymentMethod.Code == "FS" ? "Offsetting" : "",
                                                                       BankCode = d.BankAccountLite != null ? d.BankAccountLite.BankCode : null,
                                                                       BankAccountEnglishName = d.BankAccountLite != null ? d.BankAccountLite.EnglishName : null,
                                                                       BankAccountLocalName = d.BankAccountLite != null ? d.BankAccountLite.LocalName : null,
                                                                       AccountNumber = d.BankAccountLite != null ? d.BankAccountLite.AccountNumber : null,
                                                                       BranchNumber = d.BankAccountLite != null ? d.BankAccountLite.BranchNumber : null,
                                                                   }).OrderBy(o => o.PaymentCurrencyCode).ToList();

        }
        private void MapARPaymentDataListOfDataProvider()
        {
            foreach (ARPaymentDataProvider item in this.aRInvoiceDepositDataProvider.ARPaymentDataList)
            {
                item.PaidAPInvoicesList = new List<ARPaymentDataProvider.ReportARInvoicePayments>();
                List<ARInvoicePayment> APinvoicePayments = (from a in invoiceContext.ARInvoicePayments where a.ARPaymentId == item.PaymentId && a.Tenant == tenant select a).ToList();

                if (APinvoicePayments.Count == 0)
                {
                    WebFreight.Web.DataProviders.ARPaymentDataProvider.ReportARInvoicePayments reportAPIPayment = new WebFreight.Web.DataProviders.ARPaymentDataProvider.ReportARInvoicePayments();
                    reportAPIPayment.Reference = "";
                    reportAPIPayment.BillTo = "";
                    reportAPIPayment.InvoiceNumber = "";
                    reportAPIPayment.AmountPaid = null;
                    reportAPIPayment.OriginalAmount = null;
                    reportAPIPayment.InvocieDate = null;
                    reportAPIPayment.DueDate = null;
                    item.PaidAPInvoicesList.Add(reportAPIPayment);
                }

                else
                {
                    double amountpaidSum = 0;
                    foreach (ARInvoicePayment apiInvoicePayment in APinvoicePayments)
                    {
                        WebFreight.Web.DataProviders.ARPaymentDataProvider.ReportARInvoicePayments reportAPIPayment = new WebFreight.Web.DataProviders.ARPaymentDataProvider.ReportARInvoicePayments();
                        apiInvoicePayment.ARInvoice = arInvoiceQuery.GetSingleARInvoice(apiInvoicePayment.ARInvoiceId, tenant);
                        apiInvoicePayment.ARPayment = arPaymentQuery.GetSingleARPayment(apiInvoicePayment.ARPaymentId, tenant);

                        if (apiInvoicePayment.ARInvoice != null && apiInvoicePayment.ARPayment != null)
                        {
                            reportAPIPayment.Reference = apiInvoicePayment.ARInvoice.CustomerRef;
                            reportAPIPayment.BillTo = apiInvoicePayment.ARInvoice.BillTo.LocalName;
                            reportAPIPayment.InvoiceNumber = apiInvoicePayment.ARInvoice.InvoiceNumber;
                            reportAPIPayment.InvocieDate = apiInvoicePayment.ARInvoice.InvoiceDate;
                            reportAPIPayment.DueDate = apiInvoicePayment.ARInvoice.DueDate;
                            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                            customFieldResolver.SetDataProviderCustomFieldsValues("ARInvoice", tenant, apiInvoicePayment.ARInvoice, reportAPIPayment);
                            if (isLocalCurrency)
                            {
                                reportAPIPayment.OriginalAmount = apiInvoicePayment.ARInvoice.AmountInLocalCurrency;
                            }
                            else
                            {
                                reportAPIPayment.OriginalAmount = apiInvoicePayment.ARInvoice.AmountInInvoiceCurrency;
                            }

                            if (isLocalCurrency)
                            {
                                reportAPIPayment.AmountPaid = apiInvoicePayment.LocalAmount;
                            }
                            else
                            {
                                reportAPIPayment.AmountPaid = apiInvoicePayment.ForeignAmount;
                            }
                            if (reportAPIPayment.AmountPaid != null)
                            {
                                amountpaidSum += (double)reportAPIPayment.AmountPaid;
                            }
                        }
                      
                        reportAPIPayment.ShipmentNumber = apiInvoicePayment.ARInvoice.MainEntityReference != null ? apiInvoicePayment.ARInvoice.MainEntityReference : "";
                        reportAPIPayment.MasterNumber = apiInvoicePayment.ARInvoice.MasterNumber != null ? apiInvoicePayment.ARInvoice.MasterNumber : "";

                        reportAPIPayment.MasterShipmentNumber = shipmentQuery.GetMasterNumberFromHouseShipmentByShipmentNumber(reportAPIPayment.ShipmentNumber, tenant);
                        item.PaidAPInvoicesList.Add(reportAPIPayment);
                    }

                    item.sumInvoices = amountpaidSum;
                }
            }
        }
    }
}