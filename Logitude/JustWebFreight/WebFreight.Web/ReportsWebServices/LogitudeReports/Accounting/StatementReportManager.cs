using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting
{
    public class StatementReportManager
    {
        private int tenant;
        private string billToId = null;
        private string partnerId = null;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private bool includeDraftInvoices = false;
        private string ARAPFilter = null;
        private string invoicePaymentFilter = null;
        private string currencyCodeFilter = null;
        private bool isByDueDateFilter = false; 

        private ICommonDataContext commonContext;
        private AddressRepository addressRepository;
        private CustomFieldResolver customFieldResolver;
        private StatementDataProvider dataProvider;

        public StatementReportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_BillTo = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "BillToId").FirstOrDefault();
            QueryFilterItem filterItem_PartnerId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "PartnerId").FirstOrDefault();
            QueryFilterItem filterItem_DueDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "DueDate").FirstOrDefault();
            QueryFilterItem filterItem_IncludeDraftInvoices = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeDraftInvoices").FirstOrDefault();
            QueryFilterItem filterItem_ARAPFilter = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ARAPFilter").FirstOrDefault();
            QueryFilterItem filterItem_InvoicePaymentFilter = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoicePaymentFilter").FirstOrDefault();
            QueryFilterItem filterItem_CurrencyCodeFilter = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "CurrencyCode").FirstOrDefault();
            QueryFilterItem filterItem_IsByDueDateFilter = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByDueDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();


            if (filterItem_BillTo != null)
            {
                if (filterItem_BillTo.FieldValue != null)
                {
                    billToId = filterItem_BillTo.FieldValue.ToString();
                }
            }

            if (filterItem_PartnerId != null)
            {
                if (filterItem_PartnerId.FieldValue != null)
                {
                    partnerId = filterItem_PartnerId.FieldValue.ToString();
                }
            }

            if (filterItem_DueDate != null)
            {
                if (filterItem_DueDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_DueDate.FieldValue;
                }
            }

            if(filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }

            if (filterItem_IncludeDraftInvoices != null)
            {
                if (filterItem_IncludeDraftInvoices.FieldValue != null)
                {
                    includeDraftInvoices = (bool)filterItem_IncludeDraftInvoices.FieldValue;
                }
            }

            if (filterItem_ARAPFilter != null)
            {
                if (filterItem_ARAPFilter.FieldValue != null)
                {
                    ARAPFilter = filterItem_ARAPFilter.FieldValue.ToString();
                }
            }

            if (filterItem_InvoicePaymentFilter != null)
            {
                if (filterItem_InvoicePaymentFilter.FieldValue != null)
                {
                    invoicePaymentFilter = filterItem_InvoicePaymentFilter.FieldValue.ToString();
                }
            }

            if (filterItem_CurrencyCodeFilter != null)
            {
                if (filterItem_CurrencyCodeFilter.FieldValue != null)
                {
                    currencyCodeFilter = filterItem_CurrencyCodeFilter.FieldValue.ToString();
                }
            }

            if (filterItem_IsByDueDateFilter != null)
            {
                if (filterItem_IsByDueDateFilter.FieldValue != null)
                {
                    isByDueDateFilter = (bool)filterItem_IsByDueDateFilter.FieldValue;
                }
            }
        }

        public byte[] GetData()
        {
            StatementDataProvider myDataProvider = new StatementDataProvider();

            myDataProvider = this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(StatementDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private StatementDataProvider LoadDataProvider()
        {
            dataProvider = new StatementDataProvider();
            dataProvider.StatementRecordList = new List<StatementRecord>();
            dataProvider.StatementAgingSummaryRecordList = new List<StatmentAging>();

            customFieldResolver = new CustomFieldResolver();            
            commonContext = CommonDataContext.GetContext(tenant);                       
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            addressRepository = new AddressRepository(commonContext);

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;            

            this.FillGeneralData();
            List<StatementRecord> totalList = this.FillStatementRecordData();

            var results  = from p in dataProvider.StatementRecordList
                                    group p by p.Currency into g
                                    select new { Currency = g.Key, records = g.ToList() };
            if (currencyCodeFilter != null)
            {
                results = results.Where(a=>a.Currency == currencyCodeFilter);
            }
           
            foreach (var result in results)
            {
                List<StatementRecord> statementRecords = result.records.ToList();

                List<StatementRecord> currentDue = statementRecords.Where(d => d.DueDate >= todayDate).ToList();
                List<StatementRecord> Due1_30 = statementRecords.Where(d => (todayDate - d.DueDate).TotalDays >= 1 && (todayDate - d.DueDate).TotalDays <= 30).ToList();
                List<StatementRecord> Due31_60 = statementRecords.Where(d => (((todayDate - d.DueDate).TotalDays) > 30) && (((todayDate - d.DueDate).TotalDays) <= 60)).ToList();
                List<StatementRecord> Due61_90 = statementRecords.Where(d => (((todayDate - d.DueDate).TotalDays) > 60) && (((todayDate - d.DueDate).TotalDays) <= 90)).ToList();
                List<StatementRecord> Due90 = statementRecords.Where(d => (todayDate - d.DueDate).TotalDays > 90).ToList();

                List<StatementRecord> Due1_15 = statementRecords.Where(d => (todayDate - d.DueDate).TotalDays >= 1 && (todayDate - d.DueDate).TotalDays <= 15).ToList();
                List<StatementRecord> Due16_30 = statementRecords.Where(d => (((todayDate - d.DueDate).TotalDays) > 15) && (((todayDate - d.DueDate).TotalDays) <= 30)).ToList();
                List<StatementRecord> Due91_120 = statementRecords.Where(d => (((todayDate - d.DueDate).TotalDays) > 90) && (((todayDate - d.DueDate).TotalDays) <= 120)).ToList();
                List<StatementRecord> Due120 = statementRecords.Where(d => (todayDate - d.DueDate).TotalDays > 120).ToList();

                List<StatementRecord> Due31_45 = statementRecords.Where(d => (todayDate - d.DueDate).TotalDays >= 31 && (todayDate - d.DueDate).TotalDays <= 45).ToList();
                List<StatementRecord> Due46_60 = statementRecords.Where(d => (((todayDate - d.DueDate).TotalDays) > 45) && (((todayDate - d.DueDate).TotalDays) <= 60)).ToList();
                List<StatementRecord> Due61_75 = statementRecords.Where(d => (todayDate - d.DueDate).TotalDays > 60 && (todayDate - d.DueDate).TotalDays <= 75).ToList();
                List<StatementRecord> Due76_90 = statementRecords.Where(d => (((todayDate - d.DueDate).TotalDays) > 75) && (((todayDate - d.DueDate).TotalDays) <= 90)).ToList();

                double? currentResult = (currentDue.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due1_30Result = (Due1_30.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due31_60Result = (Due31_60.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due61_90Result = (Due61_90.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due90Result = (Due90.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));

                double? due1_15Result = (Due1_15.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due16_30Result = (Due16_30.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due91_120Result = (Due91_120.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due120Result = (Due120.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));

                double? due31_45Result = (Due31_45.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due46_60Result = (Due46_60.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due61_75Result = (Due61_75.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));
                double? due76_90Result = (Due76_90.Sum(d => ((d.Debit != null ? d.Debit : 0) - (d.Credit != null ? d.Credit : 0))));

                StatmentAging agingRecord = new StatmentAging()
                {
                    Currency = result.Currency,
                    currentDue = currentResult,
                    Due1_30 = due1_30Result,
                    Due31_60 = due31_60Result,
                    Due61_90 = due61_90Result,
                    Due90 = due90Result,
                    Due1_15 = due1_15Result,
                    Due16_30 = due16_30Result,
                    Due91_120 = due91_120Result,
                    Due120 = due120Result,
                    Due31_45 = due31_45Result,
                    Due46_60 = due46_60Result,
                    Due61_75 = due61_75Result,
                    Due76_90 = due76_90Result,
                };

                dataProvider.StatementAgingSummaryRecordList.Add(agingRecord);
            }

            if (currencyCodeFilter != null)
            {
                dataProvider.StatementRecordList = dataProvider.StatementRecordList.Where(or => or.Currency == currencyCodeFilter).ToList();
                dataProvider.StatementAgingSummaryRecordList = dataProvider.StatementAgingSummaryRecordList.Where(d => d.Currency == currencyCodeFilter).ToList();
            }
            else
            {
                dataProvider.StatementRecordList = dataProvider.StatementRecordList.OrderBy(or => or.Currency).ToList();
                dataProvider.StatementAgingSummaryRecordList = dataProvider.StatementAgingSummaryRecordList.OrderBy(d => d.Currency).ToList();
            }
            List<StatementGroup> finalResults = (from p in dataProvider.StatementRecordList
                                                 group p by p.Currency into g
                                                 select new StatementGroup()
                                                 {
                                                     Currency = g.Key,
                                                     StatementRecordList = g.ToList(),
                                                 }).ToList();

            foreach (StatementGroup group in finalResults)
            {
                List<StatmentAging> agingList = dataProvider.StatementAgingSummaryRecordList.Where(d => d.Currency == group.Currency).ToList();
                group.StatementAgingSummaryRecordList = agingList;
            }

            dataProvider.StatementGroupList = finalResults.OrderBy(d => d.Currency).ToList();            
            return dataProvider;
        }
        
        private void FillGeneralData()
        {
            FillFiltersFieldsValuesInDataProvider();
            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            Address tenantAddress = addressRepository.GetSingleAddress(currentTenant.AddressId, currentTenant.Id);
            Card FilterdCustomer = null;
            Address FilterdCustomerAddress = null;

            if (!string.IsNullOrEmpty(billToId))
            {
                FilterdCustomer = CardRepository.GetSingleCard(billToId, tenant, true);
                FilterdCustomerAddress = addressRepository.GetMainAddressByCardId(billToId, tenant);
            }
            else if (!string.IsNullOrEmpty(partnerId))
            {
                FilterdCustomer = CardRepository.GetSingleCard(partnerId, tenant, true);
                FilterdCustomerAddress = addressRepository.GetMainAddressByCardId(partnerId, tenant);
            }

            dataProvider.GeneralAddress = DataProviders.General.GetAddress(tenantAddress);
            dataProvider.TenantName = currentTenant.Company;
            dataProvider.Signature = currentTenant.Signature;
            dataProvider.BankDetails = currentTenant.BankDetails;
            dataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);
            dataProvider.Name = @"Statement";

            if (tenantAddress != null)
            {
                dataProvider.Address1 = tenantAddress.Address1;
                dataProvider.Address2 = tenantAddress.Address2;
                dataProvider.City = tenantAddress.City;
                dataProvider.Country = tenantAddress.Country == null ? null : tenantAddress.Country.EnglishName;
                dataProvider.TenantFax = tenantAddress.FaxNumber;
                dataProvider.TenantPhone = tenantAddress.PhoneNumber;
                dataProvider.State = tenantAddress.State == null ? null : tenantAddress.State.EnglishName;
                dataProvider.ZipCode = tenantAddress.ZipCode;
            }

            if (FilterdCustomerAddress!= null)
            {
                dataProvider.Address = DataProviders.General.GetAddress(FilterdCustomerAddress);
                dataProvider.Phone = FilterdCustomerAddress.PhoneNumber;
                dataProvider.Fax = FilterdCustomerAddress.FaxNumber;
            }

            if (FilterdCustomer != null)
            {
                dataProvider.CustomerName = FilterdCustomer.EnglishName;
            }

            else
            {
                dataProvider.CustomerName = "All";
            }
        }
        private List<StatementRecord> FillStatementRecordData()
        {
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(invoiceContext);
            ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(invoiceContext);
            APPaymentRepository aPPaymentRepository = new APPaymentRepository(invoiceContext);
            APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(invoiceContext);

            List<StatementRecord> list_ARInvoices = new List<StatementRecord>();
            List<StatementRecord> list_APInvoices = new List<StatementRecord>();
            List<StatementRecord> list_ARPayments = new List<StatementRecord>();
            List<StatementRecord> list_APPayments = new List<StatementRecord>();

            List<Currency> allCurrencies = (from d in commonContext.Currencies where d.Tenant == tenant select d).ToList();
            
            switch (invoicePaymentFilter)
            {
                case "Invoices":
                    {
                        switch (ARAPFilter)
                        {
                            case "AR":
                                {
                                    IQueryable<ARInvoice> iQueryable_ARInvoice = aRInvoiceRepository.GetUnpaidAndDraftARInvoices(tenant);
                                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => !d.IsConstituentInvoice);

                                    if (!includeDraftInvoices)
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.StatusCode != "DR");
                                    }

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.BillToId == billToId);
                                    }

                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.PartnerId == partnerId);
                                    }

                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }
                                    }

                                    list_ARInvoices = this.BuildList_ARInvoice(iQueryable_ARInvoice);
                                    break;
                                }

                            case "AP":
                                {
                                    IQueryable<APInvoice> iQueryable_APInvoice = aPInvoiceRepository.GetUnpaidAPInvoices(tenant);

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == billToId);
                                    }
                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == partnerId);
                                    }

                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }

                                        else
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }
                                    }

                                    list_APInvoices = this.BuildList_APInvoice(iQueryable_APInvoice);
                                    break;
                                }

                            default:
                                {
                                    IQueryable<ARInvoice> iQueryable_ARInvoice = aRInvoiceRepository.GetUnpaidAndDraftARInvoices(tenant);
                                    IQueryable<APInvoice> iQueryable_APInvoice = aPInvoiceRepository.GetUnpaidAPInvoices(tenant);

                                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => !d.IsConstituentInvoice);

                                    if (!includeDraftInvoices)
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.StatusCode != "DR");
                                    }

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.BillToId == billToId);
                                        iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == billToId);
                                    }

                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.PartnerId == partnerId);
                                        iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == partnerId);
                                    }

                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }

                                        else
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }
                                    }

                                    list_ARInvoices = this.BuildList_ARInvoice(iQueryable_ARInvoice);
                                    list_APInvoices = this.BuildList_APInvoice(iQueryable_APInvoice);
                                    break;
                                }
                        }                   
                        
                        break;
                    }

                case "Payments":
                    {
                        switch (ARAPFilter)
                        {
                            case "AR":
                                {
                                    IQueryable<ARPayment> iQueryable_ARPayment = aRPaymentRepository.GetOpenedARPayments(tenant);

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.BillToId == billToId);
                                    }

                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.PartnerId == partnerId);
                                    }

                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }
                                    }

                                    list_ARPayments = this.BuildList_ARPayment(iQueryable_ARPayment);
                                    break;
                                }

                            case "AP":
                                {
                                    IQueryable<APPayment> iQueryable_APPayment = aPPaymentRepository.GetOpenedAPPayments(tenant);

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == billToId);
                                    }
                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == partnerId);
                                    }

                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }

                                        else
                                        {
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }
                                    }

                                    list_APPayments = this.BuildList_APPayment(iQueryable_APPayment);
                                    break;
                                }

                            default:
                                {
                                    IQueryable<ARPayment> iQueryable_ARPayment = aRPaymentRepository.GetOpenedARPayments(tenant);
                                    IQueryable<APPayment> iQueryable_APPayment = aPPaymentRepository.GetOpenedAPPayments(tenant);

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.BillToId == billToId);
                                        iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == billToId);
                                    }

                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.PartnerId == partnerId);
                                        iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == partnerId);
                                    }


                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }
                                    }

                                    list_ARPayments = this.BuildList_ARPayment(iQueryable_ARPayment);
                                    list_APPayments = this.BuildList_APPayment(iQueryable_APPayment);
                                    break;
                                }
                        }
                        
                        break;
                    }

                default:
                    {
                        switch (ARAPFilter)
                        {
                            case "AR":
                                {
                                    IQueryable<ARInvoice> iQueryable_ARInvoice = aRInvoiceRepository.GetUnpaidAndDraftARInvoices(tenant);
                                    IQueryable<ARPayment> iQueryable_ARPayment = aRPaymentRepository.GetOpenedARPayments(tenant);

                                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => !d.IsConstituentInvoice);

                                    if (!includeDraftInvoices)
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.StatusCode != "DR");
                                    }

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.BillToId == billToId);
                                        iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.BillToId == billToId);
                                    }

                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.PartnerId == partnerId);
                                        iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.PartnerId == partnerId);
                                    }

                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));

                                        }
                                    }
                                
                                    list_ARInvoices = this.BuildList_ARInvoice(iQueryable_ARInvoice);
                                    list_ARPayments = this.BuildList_ARPayment(iQueryable_ARPayment);
                                    break;
                                }

                            case "AP":
                                {
                                    IQueryable<APInvoice> iQueryable_APInvoice = aPInvoiceRepository.GetUnpaidAPInvoices(tenant);
                                    IQueryable<APPayment> iQueryable_APPayment = aPPaymentRepository.GetOpenedAPPayments(tenant);

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == billToId);
                                        iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == billToId);
                                    }

                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == partnerId);
                                        iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == partnerId);
                                    }


                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));

                                        }

                                        else
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }
                                    }

                                    list_APInvoices = this.BuildList_APInvoice(iQueryable_APInvoice);
                                    list_APPayments = this.BuildList_APPayment(iQueryable_APPayment);
                                    break;
                                }

                            default:
                                {
                                    IQueryable<ARInvoice> iQueryable_ARInvoice = aRInvoiceRepository.GetUnpaidAndDraftARInvoices(tenant);
                                    IQueryable<APInvoice> iQueryable_APInvoice = aPInvoiceRepository.GetUnpaidAPInvoices(tenant);
                                    IQueryable<ARPayment> iQueryable_ARPayment = aRPaymentRepository.GetOpenedARPayments(tenant);
                                    IQueryable<APPayment> iQueryable_APPayment = aPPaymentRepository.GetOpenedAPPayments(tenant);

                                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => !d.IsConstituentInvoice);

                                    if (!includeDraftInvoices)
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.StatusCode != "DR");
                                    }

                                    if (!string.IsNullOrEmpty(billToId))
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.BillToId == billToId);
                                        iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == billToId);
                                        iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.BillToId == billToId);
                                        iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == billToId);
                                    }

                                    if (!string.IsNullOrEmpty(partnerId))
                                    {
                                        iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.PartnerId == partnerId);
                                        iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == partnerId);
                                        iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.PartnerId == partnerId);
                                        iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == partnerId);
                                    }

                                    if (fromDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                                        }
                                    }

                                    if (toDate != null)
                                    {
                                        if (isByDueDateFilter)
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.DueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.DueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.ValueDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }

                                        else
                                        {
                                            iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.InvoiceDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                            iQueryable_APPayment = iQueryable_APPayment.Where(d => d.RegisterDate != null && System.Data.Entity.DbFunctions.TruncateTime(d.RegisterDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                                        }
                                    }

                                    list_ARInvoices = this.BuildList_ARInvoice(iQueryable_ARInvoice);
                                    list_APInvoices = this.BuildList_APInvoice(iQueryable_APInvoice);
                                    list_ARPayments = this.BuildList_ARPayment(iQueryable_ARPayment);
                                    list_APPayments = this.BuildList_APPayment(iQueryable_APPayment);
                                    break;
                                }
                        }
                        
                        break;
                    }
            }
            
            List<StatementRecord> totalList = new List<StatementRecord>();
            totalList = list_ARInvoices.Concat(list_APInvoices).Concat(list_ARPayments).Concat(list_APPayments).ToList();

            List<string> allCardIds = totalList.Select(s => s.BillToVendorId).ToList();
            IQueryable<Card> allCards = commonContext.Cards.Where(d => d.Tenant == tenant && allCardIds.Contains(d.Id));

            List<string> allShipmentIds = totalList.Select(s => s.ShipmentId).ToList();
            List<ShipmentEntityClass> allShipmentData = (from d in shipmentsContext.Shipments.Include("ShipperCard").Include("ConsigneeCard").Include("Direction")
                                                         where d.Tenant == tenant && allShipmentIds.Contains(d.Id)
                                                         select new ShipmentEntityClass
                                                         {
                                                             ShipmentId = d.Id,
                                                             ShipperName = d.ShipperCard == null ? null : d.ShipperCard.EnglishName,
                                                             ShipperRef1 = d.ShipperReference1,
                                                             ShipperRef2 = d.ShipperReference2,
                                                             DescriptionOfGoods = d.DescriptionOfGoods,
                                                             ConsigneeName = d.ConsigneeCard == null ? null : d.ConsigneeCard.EnglishName,
                                                             Direction = d.Direction == null ? null : d.Direction.Name,
                                                         }).ToList();

            List<Branch> branches = (from d in commonContext.Branches where d.Tenant == tenant select d).ToList();

            foreach (StatementRecord record in totalList)
            {
                if (!string.IsNullOrEmpty(record.CurrencyId))
                {
                    Currency myCurrency = allCurrencies.Where(d => d.Id == record.CurrencyId).FirstOrDefault();

                    if (myCurrency != null)
                    {
                        record.Currency = myCurrency.Code;
                    }
                }

                if (record.Debit != null)
                {
                    record.Debit = (double)Math.Abs((decimal)record.Debit);
                    record.DebitWithZero = record.Debit.Value;
                }

                if (record.Credit != null)
                {
                    record.Credit = (double)Math.Abs((decimal)record.Credit);
                    record.CreditWithZero = record.Credit.Value;
                }

                int i = totalList.IndexOf(record);
                if (i < totalList.Count && i != 0)
                {
                    record.Balance = totalList[i - 1].Balance + (totalList[i].Credit != null ? -1 * totalList[i].Credit : totalList[i].Debit);
                }

                Card card = allCards.Where(d => d.Id == record.BillToVendorId).FirstOrDefault();
                if (card != null)
                {
                    record.BillToVendor = card.EnglishName;
                }

                ShipmentEntityClass shipmentEntity = allShipmentData.Where(d => d.ShipmentId == record.ShipmentId).FirstOrDefault();
                if (shipmentEntity != null)
                {
                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipmentEntity, record);

                    record.SupplierName = shipmentEntity.ShipperName;

                    if (!string.IsNullOrEmpty(shipmentEntity.ShipperRef1) && !string.IsNullOrEmpty(shipmentEntity.ShipperRef2))
                    {
                        record.SupplierRefrence = shipmentEntity.ShipperRef1 + "," + shipmentEntity.ShipperRef2;
                    }

                    else if (string.IsNullOrEmpty(shipmentEntity.ShipperRef2))
                    {
                        record.SupplierRefrence = shipmentEntity.ShipperRef1;
                    }
                    else if (string.IsNullOrEmpty(shipmentEntity.ShipperRef1))
                    {
                        record.SupplierRefrence = shipmentEntity.ShipperRef2;
                    }

                    record.DescriptionOfGoods = shipmentEntity.DescriptionOfGoods;
                    record.Shipper = shipmentEntity.ShipperName;
                    record.Consignee = shipmentEntity.ConsigneeName;
                    record.ShipmentDirection = shipmentEntity.Direction;
                }

                if (record.BranchId != null)
                {
                    Branch iBranch = branches.Where(d => d.Id == record.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        record.BranchName = iBranch.EnglishName;
                    }
                }
            }

            dataProvider.StatementRecordList = totalList;
            
            return totalList;
        }
        private List<StatementRecord> BuildList_ARInvoice(IQueryable<ARInvoice> iQueryable)
        {
            List<StatementRecord> myList = new List<StatementRecord>();
            List<ARInvoice> iQueryableList = iQueryable.ToList();
            double? openbalance = 0;
            foreach (ARInvoice d in iQueryableList)
            {
                StatementRecord item = new StatementRecord();
                item.MasterNumber = d.MasterNumber;
                item.Desicription = d.Description + " " + d.MainEntityReference;
                item.ShipmentId = d.MainEntityId;
                item.HouseNumber = d.HouseNumber;
                item.Date = d.InvoiceDate.Value;
                item.DueDate = d.DueDate.Value;
                item.OurRefrence = d.StatusCode == "DR" ? d.DraftNumber : d.InvoiceNumber;
                item.YourRefrence = d.CustomerRef;
                item.CurrencyId = d.InvoiceCurrencyId;
                item.Type = d.ARInvoiceTypeCode == "CD" ? "Credit Note" : (d.ARInvoiceTypeCode == "CC" ? "Customs Credit Note" : (d.ARInvoiceTypeCode == "CI" ? "Customs Invoice" : "A\\R Invoice"));
                item.Debit = d.AmountDue == null ? null : ((d.ARInvoiceTypeCode == "CD" || d.ARInvoiceTypeCode == "CC") ? null : d.AmountDue);
                item.DebitWithZero = d.AmountDue == null ? 0 : ((d.ARInvoiceTypeCode == "CD" || d.ARInvoiceTypeCode == "CC") ? 0 : d.AmountDue.Value);
                openbalance = openbalance + item.Debit;
                item.Balance = openbalance;
                item.Credit = d.AmountDue == null ? null : ((d.ARInvoiceTypeCode != "CD" && d.ARInvoiceTypeCode != "CC") ? null : d.AmountDue);
                item.CreditWithZero = d.AmountDue == null ? 0 : ((d.ARInvoiceTypeCode != "CD" && d.ARInvoiceTypeCode != "CC") ? 0 : d.AmountDue.Value);
                item.Notes = d.InternalNotes;
                item.BillToVendorId = d.BillToId;
                item.InvoiceStatus = d.Status == null ? null : d.Status.Name;
                item.InvoiceAmount = d.AmountInLocalCurrency;
                item.InvoiceAmountInInvoiceCurrency = d.AmountInInvoiceCurrency;
                item.AmountPaidInInvoiceCurrency = d.AmountInInvoiceCurrency - d.AmountDue;
                item.AmountPaid = d.AmountInLocalCurrency - d.AmountDueInLocalCurrency;
                item.BranchId = d.BranchId;
                item.ShipmentNumber = d.MainEntityReference;
                item.OriginalAmount = d.AmountInInvoiceCurrency;
                customFieldResolver.SetDataProviderCustomFieldsValues("ARInvoice", tenant, d, item);

                myList.Add(item);
            }

            return myList;
        }
        private List<StatementRecord> BuildList_APInvoice(IQueryable<APInvoice> iQueryable)
        {
            List<StatementRecord> myList = new List<StatementRecord>();
            List<APInvoice> iQueryableList = iQueryable.ToList();
            double? openbalance = 0;
            foreach (APInvoice d in iQueryableList)
            {
                StatementRecord item = new StatementRecord();
                item.MasterNumber = d.MasterNumber;
                item.Desicription = d.Description + " " + d.MainEntityReference;
                item.ShipmentId = d.MainEntityId;
                item.HouseNumber = d.HouseNumber;
                item.Date = d.InvoiceDate.Value;
                item.DueDate = d.DueDate.Value;
                item.OurRefrence = d.InternalNumber;
                item.CurrencyId = d.InvoiceCurrencyId;
                item.Type = "A\\P Invoice";
                item.Debit = d.AmountDue == null ? null : (d.AmountDue > 0 ? null : d.AmountDue);
                item.DebitWithZero = d.AmountDue == null ? 0 : (d.AmountDue > 0 ? 0 : d.AmountDue.Value);
                item.Credit = d.AmountDue == null ? null : (d.AmountDue > 0 ? d.AmountDue : null);
                item.CreditWithZero = d.AmountDue == null ? 0 : (d.AmountDue > 0 ? d.AmountDue.Value : 0);
                openbalance = openbalance - item.Credit;
                item.Balance = openbalance;
                item.Notes = d.InternalNotes;
                item.YourRefrence = d.InvoiceNumber;
                item.BillToVendorId = d.VendorId;
                item.InvoiceStatus = d.Status == null ? null : d.Status.Name;
                item.InvoiceAmount = d.AmountInLocalCurrency;
                item.AmountPaid = d.AmountInLocalCurrency - d.AmountDueInLocalCurrency;
                item.InvoiceAmountInInvoiceCurrency = d.AmountInInvoiceCurrency;
                item.AmountPaidInInvoiceCurrency = d.AmountInInvoiceCurrency - d.AmountDue;
                item.BranchId = d.BranchId;
                item.ShipmentNumber = d.MainEntityReference;
                item.OriginalAmount = d.AmountInInvoiceCurrency;
                myList.Add(item);
            }

            return myList;
        }
        private List<StatementRecord> BuildList_ARPayment(IQueryable<ARPayment> iQueryable)
        {
            List<StatementRecord> myList = new List<StatementRecord>();
            List<ARPayment> iQueryableList = iQueryable.ToList();
            double? openbalance = 0;

            foreach (ARPayment d in iQueryableList)
            {
                StatementRecord item = new StatementRecord();
                item.Date = d.CreateDate.Value;
                item.DueDate = d.ValueDate != null ? d.ValueDate.Value : d.CreateDate.Value;
                item.OurRefrence = d.PaymentNo;
                item.CurrencyId = d.PaymentCurrencyId;
                item.Type = "A\\R Payment";
                item.Credit = d.OpenAmount == null ? null : d.OpenAmount;
                item.CreditWithZero = d.OpenAmount == null ? 0 : d.OpenAmount.Value;
                openbalance = openbalance - item.Credit;
                item.Balance = openbalance;
                item.Notes = d.InternalNotes;
                item.RegisterDate = d.RegisterDate;
                item.ValueDate = d.ValueDate;
                item.PaymentMethod = d.AccountingPaymentMethod == null ? null : d.AccountingPaymentMethod.Name;
                item.BillToVendorId = d.BillToId;
                item.BranchId = d.BranchId;
                item.PaymentStatus = d.Status == null ? null : d.Status.Name;
                item.ShipmentNumber = d.ShipmentNumber;
                item.OriginalAmount = d.AmountInPaymentCurrency;
                myList.Add(item);
            }

            return myList;
        }


        private List<StatementRecord> BuildList_APPayment(IQueryable<APPayment> iQueryable)
        {

            List<StatementRecord> myList = new List<StatementRecord>();
            List<APPayment> iQueryableList = iQueryable.ToList();
            double? openbalance = 0;

            foreach (APPayment d in iQueryableList)
            {
                StatementRecord item = new StatementRecord();
                item.Date = d.CreateDate.Value;
                item.DueDate = d.ValueDate != null ? d.ValueDate.Value : d.CreateDate.Value;
                item.OurRefrence = d.PaymentNo;
                item.CurrencyId = d.PaymentCurrencyId;
                item.Type = "A\\P Payment";
                item.Debit = d.OpenAmount == null ? null : d.OpenAmount;
                item.DebitWithZero = d.OpenAmount == null ? 0 : d.OpenAmount.Value;
                openbalance = openbalance + item.Debit;
                item.Balance = openbalance;
                item.Notes = d.InternalNotes;
                item.RegisterDate = d.RegisterDate;
                item.ValueDate = d.ValueDate;
                item.PaymentMethod = d.AccountingPaymentMethod == null ? null : d.AccountingPaymentMethod.Name;
                item.BillToVendorId = d.VendorId;
                item.BranchId = d.BranchId;
                item.PaymentStatus = d.Status == null ? null : d.Status.Name;
                item.OriginalAmount = d.AmountInPaymentCurrency;

                myList.Add(item);
            }
            return myList;
        }
        private void FillFiltersFieldsValuesInDataProvider()
        {
            dataProvider.FromDate = fromDate != null ? fromDate : null;
            dataProvider.ToDate = toDate != null ? toDate : null;
            dataProvider.Currency = currencyCodeFilter != null ? currencyCodeFilter : "All Currencies";
            dataProvider.AROrAPFilter = ARAPFilter == null ? "All AR and AP" : ARAPFilter;
            dataProvider.PaymentOrInvoiceFilter = invoicePaymentFilter == null ? "All Payments and Invoices" : invoicePaymentFilter;
        }
    }

    public class ShipmentEntityClass
    {
        public string ShipmentId { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public string ShipperRef1 { get; set; }
        public string ShipperRef2 { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string Direction { get; set; }
    }
}