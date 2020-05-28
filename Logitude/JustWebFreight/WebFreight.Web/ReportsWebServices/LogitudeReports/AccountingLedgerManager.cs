using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports
{
    public class AccountingLedgerManager
    {
        private int tenant;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private string customerId = null;
        private bool isByCreateDate = true;
        private List<Currency> systemCurrencies;
        private List<ARInvoiceType> aRInvoiceTypes;
        private IQueryable<ARInvoice> iQueryable_ARInvoice;
        private IQueryable<APInvoice> iQueryable_APInvoice;
        private IQueryable<ARPayment> iQueryable_ARPayment;
        private IQueryable<APPayment> iQueryable_APPayment;
        private IQueryable<APPayment> iQueryable_APPaymentExternalAmount;
        private IQueryable<ARInvoice> iQueryable_ARInvoice_All;
        private IQueryable<ARInvoice> iQueryable_ARInvoice_Open;
        private IQueryable<APInvoice> iQueryable_APInvoice_Open;
        private IQueryable<ARPayment> iQueryable_ARPayment_Open;
        private IQueryable<APPayment> iQueryable_APPayment_Open;
        private IQueryable<APPayment> iQueryable_APPayment_OpenExternalAmount;
        private List<AccountingLedger> openingAccounts;
        private List<Branch> branches;
        private List<AccountingPaymentMethod> allPaymentMethods;
        private List<AccountingPaymentMethod> aRPaymentMethods;
        private List<AccountingPaymentMethod> aPPaymentMethods;
        IInvoiceContext myInvoiceContext;
        ICommonDataContext myCommonContext;
        IShipmentsContext myShipmentsContext;
        private ARInvoiceRepository aRInvoiceRepository;
        private ARPaymentRepository aRPaymentRepository;
        private APPaymentRepository aPPaymentRepository;
        private APInvoiceRepository aPInvoiceRepository;
        private AddressRepository addressRepository;
        private AddressQuery addressQuery;
        private CurrencyRepository currencyRepository;
        private ShipmentRepository shipmentRepository;
        private CardRepository cardRepository;
        private List<AccountingLedger> accountingBalanceTempList;
        private AccountingLedgerDataProvider myDataProvider; 

        public AccountingLedgerManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_IsByCreateDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByCreateDate").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "BillToId").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);
            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            if (filterItem_IsByCreateDate != null)
            {
                if (filterItem_IsByCreateDate.FieldValue != null)
                {
                    isByCreateDate = (bool)filterItem_IsByCreateDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime date;
                bool isValid = DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date);
                if (isValid)
                {
                    this.fromDate = date;
                }
            }

            if (filterItem_ToDate != null)
            {
                DateTime date;
                bool isValid = DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date);
                if (isValid)
                {
                    this.toDate = date;
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    customerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }
        }

        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AccountingLedgerDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private void LoadDataProvider()
        {
            myDataProvider = new AccountingLedgerDataProvider();

            this.InitializeReportData();
            this.BuildGeneralReportData();
            this.FilterBaseData();

            #region Open Balance
            openingAccounts = new List<AccountingLedger>();
            List<AccountingLedger> lastLedgers = new List<AccountingLedger>();
            double? Openbalance = 0;

            this.RunOpenBalance();
            var OpenledgerGroups_Customer = from item in openingAccounts
                                            group item by item.CustomerId into g
                                            select new { CustomerId = g.Key, Items = g };

            foreach (var ledgerGroup_Customer in OpenledgerGroups_Customer)
            {
                var OpenledgerGroups = from item in ledgerGroup_Customer.Items
                                       group item by item.Currency into g
                                       select new { CurrencyCode = g.Key, Items = g };

                foreach (var ledgerGroup in OpenledgerGroups)
                {
                    Openbalance = 0;
                    foreach (AccountingLedger ledger in ledgerGroup.Items)
                    {
                        switch (ledger.ReferenceType)
                        {
                            case "A\\P Payment":
                            case "A\\R Invoice":
                            case "A\\P Credit Note":
                                {
                                    Openbalance = Openbalance + ledger.Debit;
                                    ledger.AccountBanalnce = Openbalance;
                                    break;
                                }

                            case "A\\R Payment":
                            case "Credit Note":
                            case "A\\P Invoice":
                            case "External Payment":
                                {
                                    Openbalance = Openbalance - ledger.Credits;
                                    ledger.AccountBanalnce = Openbalance;
                                    break;
                                }
                        }
                    }

                    AccountingLedger lastLedger = ledgerGroup.Items.LastOrDefault();
                    AccountingLedger newLastLedger = new AccountingLedger()
                    {
                        AccountBanalnce = lastLedger.AccountBanalnce,
                        ReferenceType = "Opening Balance",
                        Currency = lastLedger.Currency,
                        CustomerId = lastLedger.CustomerId,
                    };

                    lastLedgers.Add(newLastLedger);
                }
            }
            #endregion

            #region Accounting Balance            
            accountingBalanceTempList = new List<AccountingLedger>();

            this.RunAccountingBalance();

            foreach (AccountingLedger ledger in lastLedgers)
            {
                accountingBalanceTempList.Add(ledger);
            }
            accountingBalanceTempList = accountingBalanceTempList.OrderBy(d => d.CreateDate).ToList();
            #endregion

            myDataProvider.AccountingLedgerList_Customer = new List<AccountingLedger_Customer>();
            var customerGroups = from item in accountingBalanceTempList
                                 group item by item.CustomerId into g
                                 select new { CustomerId = g.Key, CustomerItems = g };

            foreach (var item_customer in customerGroups)
            {
                AccountingLedger_Customer customerRecord = new AccountingLedger_Customer();
                customerRecord.AccountingLedgerList = new List<AccountingLedger>();
                customerRecord.CustomerId = item_customer.CustomerId;

                Card myCustomer = CardRepository.GetSingleCard(item_customer.CustomerId, tenant, false);
                if (myCustomer != null)
                {
                    customerRecord.CustomerName = myCustomer.EnglishName;
                }

                Address customerAddress = addressRepository.GetMainAddressByCardId(item_customer.CustomerId, tenant);
                if (customerAddress != null)
                {
                    customerRecord.CustomerAddress = General.GetAddress_OneLine(customerAddress);
                }

                double? balance = 0;
                var ledgerGroups = from item in item_customer.CustomerItems
                                   group item by item.Currency into g
                                   select new { CurrencyCode = g.Key, Items = g };

                foreach (var ledgerGroup in ledgerGroups)
                {
                    balance = 0;

                    foreach (AccountingLedger ledger in ledgerGroup.Items)
                    {
                        switch (ledger.ReferenceType)
                        {
                            case "A\\P Payment":
                            case "A\\R Invoice":
                            case "A\\P Credit Note":
                                {
                                    balance = balance + ledger.Debit;
                                    ledger.AccountBanalnce = balance;
                                    break;
                                }
                            case "A\\R Payment":
                            case "Credit Note":
                            case "A\\P Invoice":
                            case "External Payment":
                                {
                                    balance = balance - ledger.Credits;
                                    ledger.AccountBanalnce = balance;
                                    break;
                                }
                        }

                        AccountingLedger currencyRecord = new AccountingLedger();
                        currencyRecord.Currency = ledger.Currency;
                        currencyRecord.CreateDate = ledger.CreateDate;
                        currencyRecord.DueDate = ledger.DueDate;
                        currencyRecord.ReferenceNumber = ledger.ReferenceNumber;
                        currencyRecord.ReferenceType = ledger.ReferenceType;
                        currencyRecord.Debit = ledger.Debit;
                        currencyRecord.Credits = ledger.Credits;
                        currencyRecord.AccountBanalnce = ledger.AccountBanalnce;
                        currencyRecord.ShipperReference1 = ledger.ShipperReference1;
                        currencyRecord.ShipperReference2 = ledger.ShipperReference2;
                        currencyRecord.ShipmentNumber = ledger.ShipmentNumber;
                        currencyRecord.Notes = ledger.Notes;
                        currencyRecord.BillToVendor = ledger.BillToVendor;
                        currencyRecord.BranchId = ledger.BranchId;
                        currencyRecord.BranchName = ledger.BranchName;

                        customerRecord.AccountingLedgerList.Add(currencyRecord);
                    }
                }

                var list = (from item in customerRecord.AccountingLedgerList
                            group item by item.Currency into g
                            select new { Currency = g.Key, Items = g });

                customerRecord.AccountingLedgerList = new List<AccountingLedger>();

                foreach (var group in list)
                {
                    List<AccountingLedger> ledgerList = group.Items.ToList();
                    List<AccountingLedger> OpenList = group.Items.Where(d => d.ReferenceType == "Opening Balance").ToList();

                    if (OpenList.Count() == 0)
                    {
                        AccountingLedger accountingLedgerRecord = new AccountingLedger();
                        accountingLedgerRecord.ReferenceType = "Opening Balance";
                        accountingLedgerRecord.AccountBanalnce = 0.00;
                        accountingLedgerRecord.Currency = group.Currency;
                        ledgerList.Add(accountingLedgerRecord);
                    }

                    ledgerList = ledgerList.OrderBy(d => d.CreateDate).ToList();

                    foreach (AccountingLedger ledger in ledgerList)
                    {
                        customerRecord.AccountingLedgerList.Add(ledger);

                        int i = ledgerList.IndexOf(ledger);

                        if (i == 0)
                        {

                        }

                        if (i < ledgerList.Count && i != 0)
                        {
                            ledger.AccountBanalnce = ledgerList[i - 1].AccountBanalnce + (ledgerList[i].Credits != null ? -1 * ledgerList[i].Credits : ledgerList[i].Debit);
                        }

                        ledger.Total = ledger.AccountBanalnce;
                    }
                }

                myDataProvider.AccountingLedgerList_Customer.Add(customerRecord);
            }
        }

        private void InitializeReportData()
        {
            myInvoiceContext = InvoiceContext.GetContext(tenant);
            myCommonContext = CommonDataContext.GetContext(tenant);
            myShipmentsContext = ShipmentsContext.GetContext(tenant);

            aRInvoiceRepository = new ARInvoiceRepository(myInvoiceContext);
            aRPaymentRepository = new ARPaymentRepository(myInvoiceContext);
            aPPaymentRepository = new APPaymentRepository(myInvoiceContext);
            aPInvoiceRepository = new APInvoiceRepository(myInvoiceContext);

            addressRepository = new AddressRepository(myCommonContext);
            addressQuery = new AddressQuery(addressRepository);
            currencyRepository = new CurrencyRepository(myCommonContext);
            shipmentRepository = new ShipmentRepository(myShipmentsContext);
            cardRepository = new CardRepository(myCommonContext);
            systemCurrencies = currencyRepository.GetCurrencies(tenant).ToList();
            aRInvoiceTypes = myInvoiceContext.ARInvoiceTypes.ToList();
            allPaymentMethods = myInvoiceContext.AccountingPaymentMethods.Where(d => d.Tenant == tenant).ToList();
            aRPaymentMethods = allPaymentMethods.Where(d => d.IsAR).ToList();
            aPPaymentMethods = allPaymentMethods.Where(d => d.IsAP).ToList();
            branches = (from d in myCommonContext.Branches where d.Tenant == tenant select d).ToList();
        }
        private void BuildGeneralReportData()
        {
            myDataProvider.FromPeriod = fromDate;
            myDataProvider.ToPeriod = toDate;
            myDataProvider.Name = @"Accounting Ledger";

            if (isByCreateDate)
            {
                myDataProvider.DateType = "Create Date";
            }
            else
            {
                myDataProvider.DateType = "Value Date";
            }

            if (!string.IsNullOrEmpty(customerId))
            {
                Card customer = CardRepository.GetSingleCard(customerId, tenant, true);
                Address customerAddress = addressRepository.GetMainAddressByCardId(customerId, tenant);

                if (customer != null)
                {
                    myDataProvider.CustomerName = customer.EnglishName;
                }

                if (customerAddress != null)
                {
                    myDataProvider.Address = DataProviders.General.GetAddress(customerAddress);
                    myDataProvider.Phone = customerAddress.PhoneNumber;
                    myDataProvider.ZIPCode = customerAddress.ZipCode;

                    if (customerAddress.State != null)
                    {
                        myDataProvider.CustomerState = customerAddress.State.EnglishName;
                    }
                }
            }
            else
            {
                myDataProvider.CustomerName = "All";
            }

            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(tenant, false);
            if (currentTenant != null)
            {
                myDataProvider.CompanyAddress = currentTenant.CompanyAddress;
                myDataProvider.CompanyName = currentTenant.Company;
                myDataProvider.TenantName = currentTenant.Company;
                myDataProvider.Signature = currentTenant.Signature;
                myDataProvider.Logo = DataProviders.General.GetLogo(currentTenant.Id);

                AddressPM address = addressQuery.GetSingleAddressPM(currentTenant.AddressId, currentTenant.Id, false);
                if (address != null)
                {
                    myDataProvider.Address1 = address.Address1;
                    myDataProvider.Address2 = address.Address2;
                    myDataProvider.City = address.City;
                    myDataProvider.Country = address.CountryName;
                    myDataProvider.TenantFax = address.FaxNumber;
                    myDataProvider.TenantPhone = address.PhoneNumber;
                    myDataProvider.State = address.StateEnglishName;
                    myDataProvider.ZipCode = address.ZipCode;
                }
            }
        }
        private void FilterBaseData()
        {
            iQueryable_ARInvoice = aRInvoiceRepository.GetAccountingLedgerARInvoices(tenant);
            iQueryable_APInvoice = aPInvoiceRepository.GetAccountingLedgerAPInvoices(tenant);
            iQueryable_ARPayment = aRPaymentRepository.GetAccountingLedgerARPayments(tenant);
            iQueryable_APPayment = aPPaymentRepository.GetAccountingLedgerAPPayments(tenant);
            iQueryable_APPaymentExternalAmount = iQueryable_APPayment;
            iQueryable_ARInvoice_All = iQueryable_ARInvoice;

            if (!string.IsNullOrEmpty(customerId))
            {
                iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.BillToId == customerId);
                iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == customerId);
                iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.BillToId == customerId);
                iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == customerId);
                iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => d.VendorId == customerId);
            }

            iQueryable_ARInvoice_Open = iQueryable_ARInvoice;
            iQueryable_APInvoice_Open = iQueryable_APInvoice;
            iQueryable_ARPayment_Open = iQueryable_ARPayment;
            iQueryable_APPayment_Open = iQueryable_APPayment;
            iQueryable_APPayment_OpenExternalAmount = iQueryable_APPayment;

            if (isByCreateDate)
            {
                if (fromDate != null)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_ARPayment = iQueryable_ARPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_ARInvoice_Open = iQueryable_ARInvoice_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APInvoice_Open = iQueryable_APInvoice_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_ARPayment_Open = iQueryable_ARPayment_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APPayment_Open = iQueryable_APPayment_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APPayment_OpenExternalAmount = iQueryable_APPayment_OpenExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }

                if (toDate != null)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                    iQueryable_ARPayment = iQueryable_ARPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }
            else
            {
                if (fromDate != null)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_ARPayment = iQueryable_ARPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));

                    iQueryable_ARInvoice_Open = iQueryable_ARInvoice_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APInvoice_Open = iQueryable_APInvoice_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_ARPayment_Open = iQueryable_ARPayment_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APPayment_Open = iQueryable_APPayment_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                    iQueryable_APPayment_OpenExternalAmount = iQueryable_APPayment_OpenExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) < System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }

                if (toDate != null)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                    iQueryable_ARPayment = iQueryable_ARPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }
            }
        }
        private void RunOpenBalance()
        {
            this.RunARInvoiceOpenBalance();
            this.RunAPInvoiceOpenBalance();
            this.RunARPaymentOpenBalance();
            this.RunAPPaymentOpenBalance();
            this.RunExternalAmountOpenBalance();
        }
        private void RunARInvoiceOpenBalance()
        {
            #region AR/ Invoice
            foreach (ARInvoice openARinvoice in iQueryable_ARInvoice_Open)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openARinvoice.InvoiceCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = openARinvoice.InternalNotes;
                accountingLedgerRecord.CustomerId = openARinvoice.BillToId;

                if (openARinvoice.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = openARinvoice.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == openARinvoice.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                if (openARinvoice.ARInvoiceTypeCode == "CD")
                {
                    accountingLedgerRecord.ReferenceType = "Credit Note";
                }

                else if (openARinvoice.ARInvoiceTypeCode == "CC")
                {
                    accountingLedgerRecord.ReferenceType = "Customs Credit";
                }

                else if (openARinvoice.ARInvoiceTypeCode == "CI")
                {
                    accountingLedgerRecord.ReferenceType = "Customs Invoice";
                }

                else
                {
                    accountingLedgerRecord.ReferenceType = "A\\R Invoice";
                }

                if (openARinvoice.ARInvoiceTypeCode == "IN" || openARinvoice.ARInvoiceTypeCode == "MN" || openARinvoice.ARInvoiceTypeCode == "CI")
                {
                    if (openARinvoice.StatusCode == "AC")
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openARinvoice.AmountInInvoiceCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Debit = openARinvoice.AmountInInvoiceCurrency;
                    }
                }
                else
                {
                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openARinvoice.AmountInInvoiceCurrency);
                }

                openingAccounts.Add(accountingLedgerRecord);
            }
            #endregion

        }
        private void RunAPInvoiceOpenBalance()
        {
            #region AP/ Invoice
            foreach (APInvoice openAPInvoice in iQueryable_APInvoice_Open)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openAPInvoice.InvoiceCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = openAPInvoice.InternalNotes;
                accountingLedgerRecord.CustomerId = openAPInvoice.VendorId;

                if (openAPInvoice.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = openAPInvoice.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == openAPInvoice.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                if (openAPInvoice.AmountInInvoiceCurrency > 0)
                {
                    accountingLedgerRecord.ReferenceType = "A\\P Invoice";
                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openAPInvoice.AmountInInvoiceCurrency);
                }
                else
                {
                    accountingLedgerRecord.ReferenceType = "A\\P Credit Note";
                    accountingLedgerRecord.Debit = (double)Math.Abs((decimal)openAPInvoice.AmountInInvoiceCurrency);
                }

                openingAccounts.Add(accountingLedgerRecord);
            }
            #endregion

        }
        private void RunARPaymentOpenBalance()
        {
            #region AR/ Payment
            foreach (ARPayment openARpayment in iQueryable_ARPayment_Open)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.ReferenceType = "A\\R Payment";
                accountingLedgerRecord.CustomerId = openARpayment.BillToId;

                if (openARpayment.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = openARpayment.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == openARpayment.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                if (aRPaymentMethods.Where(d => d.Id == openARpayment.AccountingPaymentMethodId).FirstOrDefault().Code == "FS")
                {
                    if (openARpayment.AmountInPaymentCurrency < 0)
                    {
                        accountingLedgerRecord.Debit = (double)Math.Abs((decimal)openARpayment.AmountInPaymentCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openARpayment.AmountInPaymentCurrency);
                    }
                }

                else
                {
                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openARpayment.AmountInPaymentCurrency);
                }

                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openARpayment.PaymentCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = openARpayment.InternalNotes;
                openingAccounts.Add(accountingLedgerRecord);
            }
            #endregion

        }
        private void RunAPPaymentOpenBalance()
        {
            #region AP/ Payment
            foreach (APPayment openAPpayment in iQueryable_APPayment_Open)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.ReferenceType = "A\\P Payment";
                accountingLedgerRecord.CustomerId = openAPpayment.VendorId;

                if (openAPpayment.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = openAPpayment.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == openAPpayment.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                if (aPPaymentMethods.Where(d => d.Id == openAPpayment.AccountingPaymentMethodId).FirstOrDefault().Code == "FS")
                {
                    if (openAPpayment.AmountInPaymentCurrency < 0)
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openAPpayment.AmountInPaymentCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Debit = (double)Math.Abs((decimal)openAPpayment.AmountInPaymentCurrency);
                    }
                }

                else
                {
                    accountingLedgerRecord.Debit = (double)Math.Abs((decimal)openAPpayment.AmountInPaymentCurrency);
                }

                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openAPpayment.PaymentCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = openAPpayment.InternalNotes;
                openingAccounts.Add(accountingLedgerRecord);
            }
            #endregion
        }
        private void RunExternalAmountOpenBalance()
        {
            this.RunAPPaymentExternal_OpenBalance();
        }
        private void RunAPPaymentExternal_OpenBalance()
        {
            #region AP/ Payment External Amount
            foreach (APPayment openAPpayment in iQueryable_APPayment_OpenExternalAmount)
            {
                if (openAPpayment.ExternalPaymentAmount != null && openAPpayment.ExternalPaymentAmount != 0)
                {
                    AccountingLedger accountingLedgerRecord = new AccountingLedger();
                    accountingLedgerRecord.ReferenceType = "External Payment";
                    accountingLedgerRecord.CustomerId = openAPpayment.VendorId;
                    if (openAPpayment.BranchId != null)
                    {
                        accountingLedgerRecord.BranchId = openAPpayment.BranchId;
                        Branch iBranch = branches.Where(d => d.Id == openAPpayment.BranchId).FirstOrDefault();
                        if (iBranch != null)
                        {
                            accountingLedgerRecord.BranchName = iBranch.EnglishName;
                        }
                    }
                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openAPpayment.ExternalPaymentAmount);
                    accountingLedgerRecord.ValueDate = openAPpayment.ExternalPaymentDate;
                    accountingLedgerRecord.CreateDate = openAPpayment.ExternalPaymentDate;
                    accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openAPpayment.PaymentCurrencyId).FirstOrDefault().Code;
                    accountingLedgerRecord.Notes = openAPpayment.InternalNotes;
                    openingAccounts.Add(accountingLedgerRecord);
                }
            }
            #endregion
        }
        private void RunAccountingBalance()
        {
            this.RunARInvoiceAccountingBalance();
            this.RunAPInvoiceAccountingBalance();
            this.RunARPaymentAccountingBalance();
            this.RunAPPaymentAccountingBalance();
            this.RunExternalAmountAccountingBalance();
        }
        private void RunARInvoiceAccountingBalance()
        {
            #region AR/ Invoice

            foreach (ARInvoice arInvoice in iQueryable_ARInvoice)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();

                string creditedByARInvoiceTypeCode = null;

                if (!string.IsNullOrEmpty(arInvoice.CreditedByARInvoiceId))
                {
                    ARInvoice creditedByInvoice = iQueryable_ARInvoice_All.Where(d => d.Id == arInvoice.CreditedByARInvoiceId).FirstOrDefault();
                    if (creditedByInvoice != null)
                    {
                        creditedByARInvoiceTypeCode = creditedByInvoice.ARInvoiceTypeCode;
                    }
                }

                accountingLedgerRecord.ReferenceNumber = arInvoice.InvoiceNumber;
                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == arInvoice.InvoiceCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.DueDate = arInvoice.DueDate.Value;
                accountingLedgerRecord.Notes = arInvoice.InternalNotes;
                accountingLedgerRecord.CustomerId = arInvoice.BillToId;

                if (arInvoice.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = arInvoice.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == arInvoice.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(arInvoice.BillToId))
                {
                    Card card = cardRepository.GetSingleCard(arInvoice.BillToId, tenant);
                    if (card != null)
                    {
                        accountingLedgerRecord.BillToVendor = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(arInvoice.MainEntityId))
                {
                    Shipment shipment = shipmentRepository.GetSingleShipment(arInvoice.MainEntityId, tenant);
                    if (shipment != null)
                    {
                        accountingLedgerRecord.ShipperReference1 = shipment.ShipperReference1;
                        accountingLedgerRecord.ShipperReference2 = shipment.ShipperReference2;
                        accountingLedgerRecord.ShipmentNumber = shipment.ShipmentNumber;
                    }
                }

                if (isByCreateDate)
                {
                    accountingLedgerRecord.CreateDate = arInvoice.CreateDate.Value;
                }
                else
                {
                    accountingLedgerRecord.CreateDate = arInvoice.InvoiceDate.Value;
                }

                if (arInvoice.ARInvoiceTypeCode == "CD")
                {
                    accountingLedgerRecord.ReferenceType = "Credit Note";
                }

                else if (arInvoice.ARInvoiceTypeCode == "CC")
                {
                    accountingLedgerRecord.ReferenceType = "Customs Credit";
                }

                else if (arInvoice.ARInvoiceTypeCode == "CI")
                {
                    accountingLedgerRecord.ReferenceType = "Customs Invoice";
                }

                else
                {
                    accountingLedgerRecord.ReferenceType = "A\\R Invoice";
                }

                if (arInvoice.ARInvoiceTypeCode == "IN" || arInvoice.ARInvoiceTypeCode == "MN" || arInvoice.ARInvoiceTypeCode == "CI")
                {
                    if (arInvoice.StatusCode == "AC")
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)arInvoice.AmountInInvoiceCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Debit = arInvoice.AmountInInvoiceCurrency;
                    }
                }
                else
                {
                    if (arInvoice.StatusCode == "AC" && (creditedByARInvoiceTypeCode == "CD" || creditedByARInvoiceTypeCode == "CC"))
                    {
                        accountingLedgerRecord.Debit = arInvoice.AmountInInvoiceCurrency;
                    }

                    else
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)arInvoice.AmountInInvoiceCurrency);
                    }
                }

                accountingBalanceTempList.Add(accountingLedgerRecord);
            }
            #endregion
        }
        private void RunAPInvoiceAccountingBalance()
        {
            #region AP/ Invoice
            foreach (APInvoice apInvoice in iQueryable_APInvoice)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.DueDate = apInvoice.DueDate.Value;
                accountingLedgerRecord.ReferenceNumber = apInvoice.InvoiceNumber;
                accountingLedgerRecord.Notes = apInvoice.InternalNotes;
                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == apInvoice.InvoiceCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.CustomerId = apInvoice.VendorId;

                if (apInvoice.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = apInvoice.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == apInvoice.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(apInvoice.VendorId))
                {
                    Card card = cardRepository.GetSingleCard(apInvoice.VendorId, tenant);
                    if (card != null)
                    {
                        accountingLedgerRecord.BillToVendor = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(apInvoice.MainEntityReference))
                {
                    Shipment shipment = shipmentRepository.GetSingleShipmentByShipmentNumber(apInvoice.MainEntityReference, tenant);
                    if (shipment != null)
                    {
                        accountingLedgerRecord.ShipperReference1 = shipment.ShipperReference1;
                        accountingLedgerRecord.ShipperReference2 = shipment.ShipperReference2;
                        accountingLedgerRecord.ShipmentNumber = shipment.ShipmentNumber;
                    }
                }

                if (isByCreateDate)
                {
                    accountingLedgerRecord.CreateDate = apInvoice.CreateDate.Value;
                }
                else
                {
                    accountingLedgerRecord.CreateDate = apInvoice.InvoiceDate.Value;
                }

                if (apInvoice.AmountInInvoiceCurrency > 0)
                {
                    accountingLedgerRecord.ReferenceType = "A\\P Invoice";
                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)apInvoice.AmountInInvoiceCurrency);
                }
                else
                {
                    accountingLedgerRecord.ReferenceType = "A\\P Credit Note";
                    accountingLedgerRecord.Debit = (double)Math.Abs((decimal)apInvoice.AmountInInvoiceCurrency);
                }

                accountingBalanceTempList.Add(accountingLedgerRecord);
            }
            #endregion

        }
        private void RunARPaymentAccountingBalance()
        {
            #region AR/ Payment
            foreach (ARPayment arPayment in iQueryable_ARPayment)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();

                if (isByCreateDate)
                {
                    accountingLedgerRecord.CreateDate = arPayment.CreateDate.Value;
                }
                else
                {
                    accountingLedgerRecord.CreateDate = arPayment.ValueDate.Value;
                }

                accountingLedgerRecord.DueDate = arPayment.ValueDate != null ? arPayment.ValueDate.Value : arPayment.CreateDate.Value;
                accountingLedgerRecord.ReferenceNumber = arPayment.PaymentNo;
                accountingLedgerRecord.ReferenceType = "A\\R Payment";
                accountingLedgerRecord.CustomerId = arPayment.BillToId;

                if (arPayment.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = arPayment.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == arPayment.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(arPayment.BillToId))
                {
                    Card card = cardRepository.GetSingleCard(arPayment.BillToId, tenant);
                    if (card != null)
                    {
                        accountingLedgerRecord.BillToVendor = card.EnglishName;
                    }
                }

                if (aRPaymentMethods.Where(d => d.Id == arPayment.AccountingPaymentMethodId).FirstOrDefault().Code == "FS")
                {
                    if (arPayment.AmountInPaymentCurrency < 0)
                    {
                        accountingLedgerRecord.Debit = (double)Math.Abs((decimal)arPayment.AmountInPaymentCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)arPayment.AmountInPaymentCurrency);
                    }
                }

                else
                {
                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)arPayment.AmountInPaymentCurrency);
                }

                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == arPayment.PaymentCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = arPayment.InternalNotes;
                accountingLedgerRecord.RegisterDate = arPayment.RegisterDate;
                accountingLedgerRecord.ValueDate = arPayment.ValueDate;
                accountingLedgerRecord.PaymentMethod = aRPaymentMethods.Where(d => d.Id == arPayment.AccountingPaymentMethodId).FirstOrDefault().Name;

                accountingBalanceTempList.Add(accountingLedgerRecord);
            }
            #endregion
        }
        private void RunAPPaymentAccountingBalance()
        {
            #region AP/ Payment
            foreach (APPayment apPayment in iQueryable_APPayment)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();

                if (isByCreateDate)
                {
                    accountingLedgerRecord.CreateDate = apPayment.CreateDate.Value;
                }
                else
                {
                    accountingLedgerRecord.CreateDate = apPayment.ValueDate.Value;
                }

                accountingLedgerRecord.DueDate = apPayment.ValueDate != null ? apPayment.ValueDate.Value : apPayment.CreateDate.Value;
                accountingLedgerRecord.ReferenceNumber = apPayment.PaymentNo;
                accountingLedgerRecord.ReferenceType = "A\\P Payment";
                accountingLedgerRecord.CustomerId = apPayment.VendorId;

                if (apPayment.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = apPayment.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == apPayment.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(apPayment.VendorId))
                {
                    Card card = cardRepository.GetSingleCard(apPayment.VendorId, tenant);
                    if (card != null)
                    {
                        accountingLedgerRecord.BillToVendor = card.EnglishName;
                    }
                }

                if (aPPaymentMethods.Where(d => d.Id == apPayment.AccountingPaymentMethodId).FirstOrDefault().Code == "FS")
                {
                    if (apPayment.AmountInPaymentCurrency < 0)
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)apPayment.AmountInPaymentCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Debit = (double)Math.Abs((decimal)apPayment.AmountInPaymentCurrency);
                    }
                }

                else
                {
                    accountingLedgerRecord.Debit = (double)Math.Abs((decimal)apPayment.AmountInPaymentCurrency);
                }

                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == apPayment.PaymentCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = apPayment.InternalNotes;
                accountingLedgerRecord.RegisterDate = apPayment.RegisterDate;
                accountingLedgerRecord.ValueDate = apPayment.ValueDate;
                accountingLedgerRecord.PaymentMethod = aPPaymentMethods.Where(d => d.Id == apPayment.AccountingPaymentMethodId).FirstOrDefault().Name;

                accountingBalanceTempList.Add(accountingLedgerRecord);
            }
            #endregion
        }
        private void RunExternalAmountAccountingBalance()
        {
            this.RunAPPaymentExternal_AccountingBalance();
        }
        private void RunAPPaymentExternal_AccountingBalance()
        {
            #region AP/ Payment External 
            foreach (APPayment apPayment in iQueryable_APPaymentExternalAmount)
            {
                if (apPayment.ExternalPaymentAmount != null && apPayment.ExternalPaymentAmount != 0)
                {
                    AccountingLedger accountingLedgerRecord = new AccountingLedger();

                    if (isByCreateDate)
                    {
                        accountingLedgerRecord.CreateDate = apPayment.CreateDate.Value;
                    }
                    else
                    {
                        accountingLedgerRecord.CreateDate = apPayment.ValueDate.Value;
                    }

                    accountingLedgerRecord.DueDate = apPayment.ValueDate != null ? apPayment.ValueDate.Value : apPayment.CreateDate.Value;
                    accountingLedgerRecord.ReferenceNumber = apPayment.PaymentNo;
                    accountingLedgerRecord.ReferenceType = "External Payment";
                    accountingLedgerRecord.CustomerId = apPayment.VendorId;

                    if (apPayment.BranchId != null)
                    {
                        accountingLedgerRecord.BranchId = apPayment.BranchId;

                        Branch iBranch = branches.Where(d => d.Id == apPayment.BranchId).FirstOrDefault();
                        if (iBranch != null)
                        {
                            accountingLedgerRecord.BranchName = iBranch.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(apPayment.VendorId))
                    {
                        Card card = cardRepository.GetSingleCard(apPayment.VendorId, tenant);
                        if (card != null)
                        {
                            accountingLedgerRecord.BillToVendor = card.EnglishName;
                        }
                    }


                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)apPayment.ExternalPaymentAmount);

                    accountingLedgerRecord.ValueDate = apPayment.ExternalPaymentDate;
                    accountingLedgerRecord.CreateDate = apPayment.ExternalPaymentDate;
                    accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == apPayment.PaymentCurrencyId).FirstOrDefault().Code;
                    accountingLedgerRecord.Notes = apPayment.InternalNotes;
                    accountingLedgerRecord.RegisterDate = apPayment.RegisterDate;
                    accountingLedgerRecord.ValueDate = apPayment.ValueDate;
                    accountingLedgerRecord.PaymentMethod = aPPaymentMethods.Where(d => d.Id == apPayment.AccountingPaymentMethodId).FirstOrDefault().Name;

                    accountingBalanceTempList.Add(accountingLedgerRecord);
                }
            }
            #endregion
        }
    }
}