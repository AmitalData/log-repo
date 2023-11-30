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
using WebFreight.Web.Services;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports
{
    public class AccountingLedgerManager
    {
        private int tenant;

        private DateTime? FromDate = null;
        private DateTime? ToDate = null;
        private string CustomerId = null;
        private string PartnerId = null;
        private bool IsByCreateDate = true;

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
            QueryFilterItem filterItemByPartnerId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "PartnerId").FirstOrDefault();
           
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);
            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            if (filterItem_IsByCreateDate != null)
            {
                if (filterItem_IsByCreateDate.FieldValue != null)
                {
                    IsByCreateDate = (bool)filterItem_IsByCreateDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime date;
                bool isValid = DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date);
                if (isValid)
                {
                    this.FromDate = date;
                }
            }

            if (filterItem_ToDate != null)
            {
                DateTime date;
                bool isValid = DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date);
                if (isValid)
                {
                    this.ToDate = date;
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    CustomerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }

            if (filterItemByPartnerId != null)
            {
                if (filterItemByPartnerId.FieldValue != null)
                {
                    PartnerId = filterItemByPartnerId.FieldValue.ToString();
                }
            }
        }

        public byte[] GetData()
        {
            AccountingLedgerDataProvider myDataProvider = new AccountingLedgerDataProvider();
            myDataProvider = this.LoadDataProvider();
            return new ReportMemoryStreamService().Convert(myDataProvider, typeof(AccountingLedgerDataProvider), tenant);
        }

        private AccountingLedgerDataProvider LoadDataProvider()
        {
            AccountingLedgerDataProvider myDataProvider = new AccountingLedgerDataProvider();

            IInvoiceContext myInvoiceContext = InvoiceContext.GetContext(tenant);
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
            IShipmentsContext myShipmentsContext = ShipmentsContext.GetContext(tenant);

            ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(myInvoiceContext);
            ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(myInvoiceContext);
            APPaymentRepository aPPaymentRepository = new APPaymentRepository(myInvoiceContext);
            APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(myInvoiceContext);

            AddressRepository addressRepository = new AddressRepository(myCommonContext);
            AddressQuery addressQuery = new AddressQuery(addressRepository);
            CurrencyRepository currencyRepository = new CurrencyRepository(myCommonContext);
            ShipmentRepository shipmentRepository = new ShipmentRepository(myShipmentsContext);
            CardRepository cardRepository = new CardRepository(myCommonContext);

            List<Currency> systemCurrencies = currencyRepository.GetCurrencies(tenant).ToList();
            List<ARInvoiceType> ARInvoiceTypes = myInvoiceContext.ARInvoiceTypes.ToList();
            List<AccountingPaymentMethod> AllPaymentMethods = myInvoiceContext.AccountingPaymentMethods.Where(d => d.Tenant == tenant).ToList();
            List<AccountingPaymentMethod> ARPaymentMethods = AllPaymentMethods.Where(d => d.IsAR).ToList();
            List<AccountingPaymentMethod> APPaymentMethods = AllPaymentMethods.Where(d => d.IsAP).ToList();
            List<Branch> branches = (from d in myCommonContext.Branches where d.Tenant == tenant select d).ToList();

            #region General Data

            myDataProvider.FromPeriod = FromDate;
            myDataProvider.ToPeriod = ToDate;
            myDataProvider.Name = @"Accounting Ledger";

            if (IsByCreateDate)
            {
                myDataProvider.DateType = "Create Date";
            }
            else
            {
                myDataProvider.DateType = "Value Date";
            }

            if (!string.IsNullOrEmpty(CustomerId))
            {
                Card customer = CardRepository.GetSingleCard(CustomerId, tenant, true);
                Address customerAddress = addressRepository.GetMainAddressByCardId(CustomerId, tenant);

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
            #endregion

            #region Base Data Filtered

            IQueryable<ARInvoice> iQueryable_ARInvoice = aRInvoiceRepository.GetAccountingLedgerARInvoices(tenant);
            IQueryable<APInvoice> iQueryable_APInvoice = aPInvoiceRepository.GetAccountingLedgerAPInvoices(tenant);
            IQueryable<ARPayment> iQueryable_ARPayment = aRPaymentRepository.GetAccountingLedgerARPayments(tenant);
            IQueryable<APPayment> iQueryable_APPayment = aPPaymentRepository.GetAccountingLedgerAPPayments(tenant);
            IQueryable<APPayment> iQueryable_APPaymentExternalAmount = iQueryable_APPayment;

            IQueryable<ARInvoice> iQueryable_ARInvoice_All = iQueryable_ARInvoice;

            if (!string.IsNullOrEmpty(CustomerId))
            {
                iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.BillToId == CustomerId);
                iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == CustomerId);
                iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.BillToId == CustomerId);
                iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == CustomerId);
                iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => d.VendorId == CustomerId);
            }

            if (!string.IsNullOrEmpty(PartnerId))
            {
                iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => d.PartnerId == PartnerId);         
                iQueryable_ARPayment = iQueryable_ARPayment.Where(d => d.PartnerId == PartnerId);  

                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => d.VendorId == PartnerId);
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => d.VendorId == PartnerId);
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => d.VendorId == PartnerId);
                
            }

            IQueryable<ARInvoice> iQueryable_ARInvoice_Open = iQueryable_ARInvoice;
            IQueryable<APInvoice> iQueryable_APInvoice_Open = iQueryable_APInvoice;
            IQueryable<ARPayment> iQueryable_ARPayment_Open = iQueryable_ARPayment;
            IQueryable<APPayment> iQueryable_APPayment_Open = iQueryable_APPayment;
            IQueryable<APPayment> iQueryable_APPayment_OpenExternalAmount = iQueryable_APPayment;

            if (IsByCreateDate)
            {
                if (FromDate != null)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_ARPayment = iQueryable_ARPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_ARInvoice_Open = iQueryable_ARInvoice_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APInvoice_Open = iQueryable_APInvoice_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_ARPayment_Open = iQueryable_ARPayment_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APPayment_Open = iQueryable_APPayment_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APPayment_OpenExternalAmount = iQueryable_APPayment_OpenExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                }

                if (ToDate != null)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                    iQueryable_ARPayment = iQueryable_ARPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                }
            }

            else
            {
                if (FromDate != null)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_ARPayment = iQueryable_ARPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));

                    iQueryable_ARInvoice_Open = iQueryable_ARInvoice_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APInvoice_Open = iQueryable_APInvoice_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_ARPayment_Open = iQueryable_ARPayment_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APPayment_Open = iQueryable_APPayment_Open.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                    iQueryable_APPayment_OpenExternalAmount = iQueryable_APPayment_OpenExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) < System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                }

                if (ToDate != null)
                {
                    iQueryable_ARInvoice = iQueryable_ARInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                    iQueryable_APInvoice = iQueryable_APInvoice.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.InvoiceDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                    iQueryable_ARPayment = iQueryable_ARPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                    iQueryable_APPayment = iQueryable_APPayment.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ValueDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                    iQueryable_APPaymentExternalAmount = iQueryable_APPaymentExternalAmount.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ExternalPaymentDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                }
            }

            #endregion

            #region Open Balance
            List<AccountingLedger> OpeningAccounts = new List<AccountingLedger>();
            List<AccountingLedger> lastLedgers = new List<AccountingLedger>();
            double? Openbalance = 0;
            double? Openbalance_local = 0;

            #region AR/ Invoice
            foreach (ARInvoice openARinvoice in iQueryable_ARInvoice_Open)
            {
                string creditedByARInvoiceTypeCode = null;

                if (!string.IsNullOrEmpty(openARinvoice.CreditedByARInvoiceId))
                {
                    ARInvoice creditedByInvoice = iQueryable_ARInvoice_All.Where(d => d.Id == openARinvoice.CreditedByARInvoiceId).FirstOrDefault();
                    if (creditedByInvoice != null)
                    {
                        creditedByARInvoiceTypeCode = creditedByInvoice.ARInvoiceTypeCode;
                    }
                }

                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openARinvoice.InvoiceCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = openARinvoice.InternalNotes;
                accountingLedgerRecord.CustomerId = openARinvoice.BillToId;
                accountingLedgerRecord.PartnerId = openARinvoice.PartnerId;
                accountingLedgerRecord.Description = openARinvoice.Description + " " + openARinvoice.MainEntityReference;

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
                    accountingLedgerRecord.ReferenceType = "Credit Note";                

                else if (openARinvoice.ARInvoiceTypeCode == "CC")                
                    accountingLedgerRecord.ReferenceType = "Customs Credit";                

                else if (openARinvoice.ARInvoiceTypeCode == "CI")                
                    accountingLedgerRecord.ReferenceType = "Customs Invoice";                

                else                
                    accountingLedgerRecord.ReferenceType = "A\\R Invoice";                

                if (openARinvoice.ARInvoiceTypeCode == "IN" || openARinvoice.ARInvoiceTypeCode == "MN" || openARinvoice.ARInvoiceTypeCode == "CI")
                {
                    if (openARinvoice.StatusCode == "AC")
                    {
                        accountingLedgerRecord.IsAutoCredit = true;
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openARinvoice.AmountInInvoiceCurrency);
                        accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)openARinvoice.AmountInLocalCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Debit = openARinvoice.AmountInInvoiceCurrency;
                        accountingLedgerRecord.DebitInLocalCurrency = openARinvoice.AmountInLocalCurrency;
                    }
                }
                else
                {
                    if (openARinvoice.StatusCode == "AC" && (creditedByARInvoiceTypeCode == "CD" || creditedByARInvoiceTypeCode == "CC"))
                    {
                        accountingLedgerRecord.IsAutoCredit = true;
                        accountingLedgerRecord.Debit = openARinvoice.AmountInInvoiceCurrency;
                        accountingLedgerRecord.DebitInLocalCurrency = openARinvoice.AmountInLocalCurrency;
                    }

                    else
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openARinvoice.AmountInInvoiceCurrency);
                        accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)openARinvoice.AmountInLocalCurrency);
                    }
                }

                OpeningAccounts.Add(accountingLedgerRecord);
            }
            #endregion

            #region AP/ Invoice
            foreach (APInvoice openAPInvoice in iQueryable_APInvoice_Open)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openAPInvoice.InvoiceCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = openAPInvoice.InternalNotes;
                accountingLedgerRecord.CustomerId = openAPInvoice.VendorId;
                accountingLedgerRecord.Description = openAPInvoice.Description + " " + openAPInvoice.MainEntityReference;

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
                    accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)openAPInvoice.AmountInLocalCurrency);
                }
                else
                {
                    accountingLedgerRecord.ReferenceType = "A\\P Credit Note";
                    accountingLedgerRecord.Debit = (double)Math.Abs((decimal)openAPInvoice.AmountInInvoiceCurrency);
                    accountingLedgerRecord.DebitInLocalCurrency = (double)Math.Abs((decimal)openAPInvoice.AmountInLocalCurrency);

                }

                OpeningAccounts.Add(accountingLedgerRecord);
            }
            #endregion

            #region AR/ Payment
            foreach (ARPayment openARpayment in iQueryable_ARPayment_Open)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.ReferenceType = "A\\R Payment";
                accountingLedgerRecord.CustomerId = openARpayment.BillToId;
                accountingLedgerRecord.PartnerId = openARpayment.PartnerId;

                if (openARpayment.BranchId != null)
                {
                    accountingLedgerRecord.BranchId = openARpayment.BranchId;

                    Branch iBranch = branches.Where(d => d.Id == openARpayment.BranchId).FirstOrDefault();
                    if (iBranch != null)
                    {
                        accountingLedgerRecord.BranchName = iBranch.EnglishName;
                    }
                }

                AccountingPaymentMethod paymentMethod = ARPaymentMethods.Where(d => d.Id == openARpayment.AccountingPaymentMethodId).FirstOrDefault();
                if (paymentMethod != null && paymentMethod.Code == "FS")
                {
                    if (openARpayment.AmountInPaymentCurrency < 0)
                    {
                        accountingLedgerRecord.Debit = (double)Math.Abs((decimal)openARpayment.AmountInPaymentCurrency);
                        accountingLedgerRecord.DebitInLocalCurrency = (double)Math.Abs((decimal)openARpayment.AmountInLocalCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openARpayment.AmountInPaymentCurrency);
                        accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)openARpayment.AmountInLocalCurrency);
                    }
                }

                else
                {
                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openARpayment.AmountInPaymentCurrency);
                    accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)openARpayment.AmountInLocalCurrency);
                }

                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openARpayment.PaymentCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = openARpayment.InternalNotes;
                OpeningAccounts.Add(accountingLedgerRecord);
            }
            #endregion

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

                AccountingPaymentMethod paymentMethod = APPaymentMethods.Where(d => d.Id == openAPpayment.AccountingPaymentMethodId).FirstOrDefault();
                if (paymentMethod != null && paymentMethod.Code == "FS")
                {
                    if (openAPpayment.AmountInPaymentCurrency < 0)
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)openAPpayment.AmountInPaymentCurrency);
                        accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)openAPpayment.AmountInLocalCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Debit = (double)Math.Abs((decimal)openAPpayment.AmountInPaymentCurrency);
                        accountingLedgerRecord.DebitInLocalCurrency = (double)Math.Abs((decimal)openAPpayment.AmountInLocalCurrency);
                    }
                }

                else
                {
                    accountingLedgerRecord.Debit = (double)Math.Abs((decimal)openAPpayment.AmountInPaymentCurrency);
                    accountingLedgerRecord.DebitInLocalCurrency = (double)Math.Abs((decimal)openAPpayment.AmountInLocalCurrency);
                }

                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openAPpayment.PaymentCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = openAPpayment.InternalNotes;
                OpeningAccounts.Add(accountingLedgerRecord);
            }
            #endregion

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
                    accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)openAPpayment.ExternalPaymentAmount);
                    accountingLedgerRecord.ValueDate = openAPpayment.ExternalPaymentDate;
                    accountingLedgerRecord.CreateDate = openAPpayment.ExternalPaymentDate;
                    accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == openAPpayment.PaymentCurrencyId).FirstOrDefault().Code;
                    accountingLedgerRecord.Notes = openAPpayment.InternalNotes;
                    OpeningAccounts.Add(accountingLedgerRecord);
                }
            }
            #endregion
            
            var OpenledgerGroups_Customer = from item in OpeningAccounts
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
                    Openbalance_local = 0;
                    foreach (AccountingLedger ledger in ledgerGroup.Items)
                    {
                        switch (ledger.ReferenceType)
                        {
                            case "A\\P Payment":                            
                            case "A\\P Credit Note":                            
                                {
                                    Openbalance = Openbalance + ledger.Debit;
                                    Openbalance_local = Openbalance_local + ledger.DebitInLocalCurrency;
                                    ledger.AccountBanalnce = Openbalance;
                                    ledger.AccountBalanceInLocalCurrency = Openbalance_local;
                                    break;
                                }


                            case "A\\R Payment":
                            case "A\\P Invoice":
                            case "External Payment":
                                {
                                    Openbalance = Openbalance - ledger.Credits;
                                    Openbalance_local = Openbalance_local - ledger.CreditInLocalCurrency;
                                    ledger.AccountBanalnce = Openbalance;
                                    ledger.AccountBalanceInLocalCurrency = Openbalance_local;
                                    break;
                                }

                            case "A\\R Invoice":
                            case "Customs Invoice":
                                {
                                    if (ledger.IsAutoCredit)
                                    {
                                        Openbalance = Openbalance - ledger.Credits;
                                        Openbalance_local = Openbalance_local - ledger.CreditInLocalCurrency;
                                        ledger.AccountBanalnce = Openbalance;
                                        ledger.AccountBalanceInLocalCurrency = Openbalance_local;
                                    }

                                    else
                                    {
                                        Openbalance = Openbalance + ledger.Debit;
                                        Openbalance_local = Openbalance_local + ledger.DebitInLocalCurrency;
                                        ledger.AccountBanalnce = Openbalance;
                                        ledger.AccountBalanceInLocalCurrency = Openbalance_local;
                                    }

                                    break;
                                }

                            case "Credit Note":
                            case "Customs Credit":
                                {
                                    if (ledger.IsAutoCredit)
                                    {
                                        Openbalance = Openbalance + ledger.Debit;
                                        Openbalance_local = Openbalance_local + ledger.DebitInLocalCurrency;
                                        ledger.AccountBanalnce = Openbalance;
                                        ledger.AccountBalanceInLocalCurrency = Openbalance_local;
                                    }

                                    else
                                    {
                                        Openbalance = Openbalance - ledger.Credits;
                                        Openbalance_local = Openbalance_local - ledger.CreditInLocalCurrency;
                                        ledger.AccountBanalnce = Openbalance;
                                        ledger.AccountBalanceInLocalCurrency = Openbalance_local;
                                    }
                                    break;
                                }
                        }
                    }

                    AccountingLedger lastLedger = ledgerGroup.Items.LastOrDefault();
                    AccountingLedger newLastLedger = new AccountingLedger()
                    {
                        AccountBanalnce = lastLedger.AccountBanalnce,
                        AccountBalanceInLocalCurrency = lastLedger.AccountBalanceInLocalCurrency,
                        ReferenceType = "Opening Balance",
                        Currency = lastLedger.Currency,
                        CustomerId = lastLedger.CustomerId,
                    };

                    lastLedgers.Add(newLastLedger);
                }
            }
            #endregion

            #region Accounting Balance            
            List<AccountingLedger> tempList = new List<AccountingLedger>();

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
                accountingLedgerRecord.PartnerId = arInvoice.PartnerId;///when the filter is not null 
                accountingLedgerRecord.MasterNumber = arInvoice.MasterNumber;
                accountingLedgerRecord.HouseNumber = arInvoice.HouseNumber;
                accountingLedgerRecord.Description = arInvoice.Description + " " + arInvoice.MainEntityReference;

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
                        accountingLedgerRecord.ProjectNumber = shipment.ProjectNumber;
                        accountingLedgerRecord.CustomerReference1 = shipment.CustomerReference1;
                        accountingLedgerRecord.CustomerReference2 = shipment.CustomerReference2;
                    }
                }

                if (IsByCreateDate)
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
                        accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)arInvoice.AmountInLocalCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Debit = arInvoice.AmountInInvoiceCurrency;
                        accountingLedgerRecord.DebitInLocalCurrency = arInvoice.AmountInLocalCurrency;
                    }
                }
                else
                {
                    if (arInvoice.StatusCode == "AC" && (creditedByARInvoiceTypeCode == "CD" || creditedByARInvoiceTypeCode == "CC"))
                    {
                        accountingLedgerRecord.Debit = arInvoice.AmountInInvoiceCurrency;
                        accountingLedgerRecord.DebitInLocalCurrency = arInvoice.AmountInLocalCurrency;
                    }

                    else
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)arInvoice.AmountInInvoiceCurrency);
                        accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)arInvoice.AmountInLocalCurrency);
                    }
                }

                tempList.Add(accountingLedgerRecord);
            }
            #endregion

            #region AP/ Invoice
            foreach (APInvoice apInvoice in iQueryable_APInvoice)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();
                accountingLedgerRecord.DueDate = apInvoice.DueDate.Value;
                accountingLedgerRecord.ReferenceNumber = apInvoice.InvoiceNumber;
                accountingLedgerRecord.Notes = apInvoice.InternalNotes;
                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == apInvoice.InvoiceCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.CustomerId = apInvoice.VendorId;
                accountingLedgerRecord.MasterNumber = apInvoice.MasterNumber;
                accountingLedgerRecord.HouseNumber = apInvoice.HouseNumber;
                accountingLedgerRecord.Description = apInvoice.Description + " " + apInvoice.MainEntityReference;

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
                        accountingLedgerRecord.ProjectNumber = shipment.ProjectNumber;
                        accountingLedgerRecord.CustomerReference1 = shipment.CustomerReference1;
                        accountingLedgerRecord.CustomerReference2 = shipment.CustomerReference2;
                    }
                }

                if (IsByCreateDate)
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
                    accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)apInvoice.AmountInLocalCurrency);
                }
                else
                {
                    accountingLedgerRecord.ReferenceType = "A\\P Credit Note";
                    accountingLedgerRecord.Debit = (double)Math.Abs((decimal)apInvoice.AmountInInvoiceCurrency);
                    accountingLedgerRecord.DebitInLocalCurrency = (double)Math.Abs((decimal)apInvoice.AmountInLocalCurrency);
                }

                tempList.Add(accountingLedgerRecord);
            }
            #endregion

            #region AR/ Payment
            foreach (ARPayment arPayment in iQueryable_ARPayment)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();

                if (IsByCreateDate)
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
                accountingLedgerRecord.PartnerId = arPayment.PartnerId;
                

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

                AccountingPaymentMethod paymentMethod = ARPaymentMethods.Where(d => d.Id == arPayment.AccountingPaymentMethodId).FirstOrDefault();
                if (paymentMethod != null && paymentMethod.Code == "FS")
                {
                    if (arPayment.AmountInPaymentCurrency < 0)
                    {
                        accountingLedgerRecord.Debit = (double)Math.Abs((decimal)arPayment.AmountInPaymentCurrency);
                        accountingLedgerRecord.DebitInLocalCurrency = (double)Math.Abs((decimal)arPayment.AmountInLocalCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)arPayment.AmountInPaymentCurrency);
                        accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)arPayment.AmountInLocalCurrency);
                    }
                }

                else
                {
                    accountingLedgerRecord.Credits = (double)Math.Abs((decimal)arPayment.AmountInPaymentCurrency);
                    accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)arPayment.AmountInLocalCurrency);
                }

                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == arPayment.PaymentCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = arPayment.InternalNotes;
                accountingLedgerRecord.RegisterDate = arPayment.RegisterDate;
                accountingLedgerRecord.ValueDate = arPayment.ValueDate;
                accountingLedgerRecord.PaymentMethod = paymentMethod == null ? null : paymentMethod.Name;

                tempList.Add(accountingLedgerRecord);
            }
            #endregion

            #region AP/ Payment
            foreach (APPayment apPayment in iQueryable_APPayment)
            {
                AccountingLedger accountingLedgerRecord = new AccountingLedger();

                if (IsByCreateDate)
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

                AccountingPaymentMethod paymentMethod = APPaymentMethods.Where(d => d.Id == apPayment.AccountingPaymentMethodId).FirstOrDefault();
                if (paymentMethod != null && paymentMethod.Code == "FS")
                {
                    if (apPayment.AmountInPaymentCurrency < 0)
                    {
                        accountingLedgerRecord.Credits = (double)Math.Abs((decimal)apPayment.AmountInPaymentCurrency);
                        accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)apPayment.AmountInLocalCurrency);
                    }

                    else
                    {
                        accountingLedgerRecord.Debit = (double)Math.Abs((decimal)apPayment.AmountInPaymentCurrency);
                        accountingLedgerRecord.DebitInLocalCurrency = (double)Math.Abs((decimal)apPayment.AmountInLocalCurrency);
                    }
                }

                else
                {
                    accountingLedgerRecord.Debit = (double)Math.Abs((decimal)apPayment.AmountInPaymentCurrency);
                    accountingLedgerRecord.DebitInLocalCurrency = (double)Math.Abs((decimal)apPayment.AmountInLocalCurrency);
                }

                accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == apPayment.PaymentCurrencyId).FirstOrDefault().Code;
                accountingLedgerRecord.Notes = apPayment.InternalNotes;
                accountingLedgerRecord.RegisterDate = apPayment.RegisterDate;
                accountingLedgerRecord.ValueDate = apPayment.ValueDate;
                accountingLedgerRecord.PaymentMethod = paymentMethod == null ? null : paymentMethod.Name;

                tempList.Add(accountingLedgerRecord);
            }
            #endregion

            #region AP/ Payment External 
            foreach (APPayment apPayment in iQueryable_APPaymentExternalAmount)
            {
                if (apPayment.ExternalPaymentAmount != null && apPayment.ExternalPaymentAmount != 0)
                {
                    AccountingLedger accountingLedgerRecord = new AccountingLedger();

                    if (IsByCreateDate)
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
                    accountingLedgerRecord.CreditInLocalCurrency = (double)Math.Abs((decimal)apPayment.ExternalPaymentAmount);
                    accountingLedgerRecord.ValueDate = apPayment.ExternalPaymentDate;
                    accountingLedgerRecord.CreateDate = apPayment.ExternalPaymentDate;
                    accountingLedgerRecord.Currency = systemCurrencies.Where(d => d.Id == apPayment.PaymentCurrencyId).FirstOrDefault().Code;
                    accountingLedgerRecord.Notes = apPayment.InternalNotes;
                    accountingLedgerRecord.RegisterDate = apPayment.RegisterDate;
                    accountingLedgerRecord.ValueDate = apPayment.ValueDate;
                    accountingLedgerRecord.PaymentMethod = APPaymentMethods.Where(d => d.Id == apPayment.AccountingPaymentMethodId).FirstOrDefault().Name;

                    tempList.Add(accountingLedgerRecord);
                }
            }
            #endregion

            foreach (AccountingLedger ledger in lastLedgers)
            {
                tempList.Add(ledger);
            }

            tempList = tempList.OrderBy(d => d.CreateDate).ToList();
            #endregion

            myDataProvider.AccountingLedgerList_Customer = new List<AccountingLedger_Customer>();
            /* var customerGroups = from item in tempList
                                  group item by new { item.CustomerId,item.PartnerId} into g
                                  select new { CustomerId = g.Key.CustomerId, PartnerId = g.Key.PartnerId, CustomerItems = g };
             */
            var customerGroups = from item in tempList
                                 group item by item.CustomerId into g
                                 select new { CustomerId = g.Key, CustomerItems = g };
            foreach (var item_customer in customerGroups)
            {
                Card myCustomer = null;
                Card partnerData = null;
                AccountingLedger_Customer customerRecord = new AccountingLedger_Customer();
                customerRecord.AccountingLedgerList = new List<AccountingLedger>();
                customerRecord.CustomerId = item_customer.CustomerId;

          

                if (item_customer.CustomerId != null)
                {
                    myCustomer = CardRepository.GetSingleCard(item_customer.CustomerId, tenant, false);
                    Address customerAddress = addressRepository.GetMainAddressByCardId(item_customer.CustomerId, tenant);
                    if (customerAddress != null)
                    {
                        customerRecord.CustomerAddress = General.GetAddress_OneLine(customerAddress);
                    }

                }
               var partners = from item in tempList
                           where item.CustomerId == item_customer.CustomerId
                           group item by item.PartnerId into g
                           select new { PartnerId = g.Key};

                foreach (var partner in partners)
                {
                    partnerData = CardRepository.GetSingleCard(partner.PartnerId, tenant, false);
                    if (partnerData != null)
                    {
                        customerRecord.PartnerName = customerRecord.PartnerName == null ? partnerData.EnglishName : customerRecord.PartnerName + ", " + partnerData.EnglishName;
                    }
                }
          
               
                if (myCustomer != null)
                {
                    
                    customerRecord.CustomerName = myCustomer.EnglishName;
                    customerRecord.CardCode = myCustomer.Code;
                }



                double? balance = 0;
                double? balance_local = 0;
                var ledgerGroups = from item in item_customer.CustomerItems
                                   group item by item.Currency into g
                                   select new { CurrencyCode = g.Key, Items = g };

                foreach (var ledgerGroup in ledgerGroups)
                {
                    balance = 0;
                    balance_local = 0;

                    foreach (AccountingLedger ledger in ledgerGroup.Items)
                    {
                        switch (ledger.ReferenceType)
                        {
                            case "A\\P Payment":
                            case "A\\R Invoice":
                            case "A\\P Credit Note":
                                {
                                    balance = balance + ledger.Debit;
                                    balance_local = balance_local + ledger.DebitInLocalCurrency;
                                    ledger.AccountBanalnce = balance;
                                    ledger.AccountBalanceInLocalCurrency = balance_local;
                                    break;
                                }

                            case "A\\R Payment":
                            case "Credit Note":
                            case "A\\P Invoice":
                            case "External Payment":
                                {
                                    balance = balance - ledger.Credits;
                                    balance_local = balance_local - ledger.CreditInLocalCurrency;
                                    ledger.AccountBanalnce = balance;
                                    ledger.AccountBalanceInLocalCurrency = balance_local;
                                    break;
                                }
                        }

                        if (ledger.AccountBanalnce == null)
                            ledger.AccountBanalnce = 0;

                        if (ledger.AccountBalanceInLocalCurrency == null)
                            ledger.AccountBalanceInLocalCurrency = 0;

                        AccountingLedger currencyRecord = new AccountingLedger();
                        currencyRecord.Currency = ledger.Currency;
                        currencyRecord.CreateDate = ledger.CreateDate;
                        currencyRecord.DueDate = ledger.DueDate;
                        currencyRecord.ReferenceNumber = ledger.ReferenceNumber;
                        currencyRecord.ReferenceType = ledger.ReferenceType;
                        currencyRecord.Debit = ledger.Debit;
                        currencyRecord.DebitInLocalCurrency = ledger.DebitInLocalCurrency;
                        currencyRecord.Credits = ledger.Credits;
                        currencyRecord.CreditInLocalCurrency = ledger.CreditInLocalCurrency;
                        currencyRecord.AccountBanalnce = ledger.AccountBanalnce;
                        currencyRecord.AccountBalanceInLocalCurrency = ledger.AccountBalanceInLocalCurrency;
                        currencyRecord.ShipperReference1 = ledger.ShipperReference1;
                        currencyRecord.ShipperReference2 = ledger.ShipperReference2;
                        currencyRecord.ShipmentNumber = ledger.ShipmentNumber;
                        currencyRecord.Notes = ledger.Notes;
                        currencyRecord.BillToVendor = ledger.BillToVendor;
                        currencyRecord.BranchId = ledger.BranchId;
                        currencyRecord.BranchName = ledger.BranchName;
                        currencyRecord.MasterNumber = ledger.MasterNumber;
                        currencyRecord.HouseNumber = ledger.HouseNumber;
                        currencyRecord.Description = ledger.Description;
                        currencyRecord.ProjectNumber = ledger.ProjectNumber;
                        currencyRecord.CustomerReference1 = ledger.CustomerReference1;
                        currencyRecord.CustomerReference2 = ledger.CustomerReference2;
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
                        accountingLedgerRecord.AccountBalanceInLocalCurrency = 0.00;
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
                            ledger.AccountBalanceInLocalCurrency = ledgerList[i - 1].AccountBalanceInLocalCurrency + (ledgerList[i].CreditInLocalCurrency != null ? -1 * ledgerList[i].CreditInLocalCurrency : ledgerList[i].DebitInLocalCurrency);
                        }

                        ledger.Total = ledger.AccountBanalnce;
                    }
                }

                myDataProvider.AccountingLedgerList_Customer.Add(customerRecord);
            }

            return myDataProvider;
        }
    }
}