using System;
using System.Collections.Generic;
using System.Data.Entity;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;


namespace Simplog.Data.InvoiceModel.Mocks
{
    public class MockInvoiceContext : IInvoiceContext
    {
        List<ARInvoice> arinvoices;
        MockObjectSet<ARInvoice> arInvoiceObjectSet;
        public IDbSet<ARInvoice> ARInvoices
        {
            get
            {
                if (arinvoices == null)
                {
                    arinvoices = new List<ARInvoice>() 
                    {
                        new ARInvoice()
                        {
                             Id = "1-1",
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInLocalCurrency = 2000,
                AmountInInvoiceCurrency = 2000,
                AmountInProfitCurrency = 2000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                CreatedByUserId = "1-1",
                DueDate = DateTime.Now.Date.AddDays(30),
                ExchangeRateDate = DateTime.Now.Date,
                DraftNumber = "1002",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
               
                InvoiceNumber = "1002",
                IsClosed = false,
                LocalCurrencyId = "1-1",
                MainEntityId = "1-1",
               
                PaymentTermId = "1-1",
                ProfitCurrencyExchangeRate = 1,
                ProfitCurrencyId = "1-1",
                Tenant = 1,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "DR",
                UpdatedByUserId = "1-1",
                
                        },

                          new ARInvoice()
                        {
                             Id = "1-3",
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInLocalCurrency = 3000,
                AmountInInvoiceCurrency = 3000,
                AmountInProfitCurrency = 3000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                CreatedByUserId = "1-1",
                DueDate = DateTime.Now.Date.AddDays(30),
                ExchangeRateDate = DateTime.Now.Date,
                DraftNumber = "1001",
                InvoiceCurrencyId = "1-2",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
               
                InvoiceNumber = "1001",
                IsClosed = false,
                LocalCurrencyId = "1-2",
                MainEntityId = "1-1",
               
                PaymentTermId = "1-1",
                ProfitCurrencyExchangeRate = 1,
                ProfitCurrencyId = "1-2",
                Tenant = 2,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "DR",
                UpdatedByUserId = "1-1",
                
                        },
                               new ARInvoice()
                        {
                             Id = "1-4",
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInLocalCurrency = 4000,
                AmountInInvoiceCurrency = 4000,
                AmountInProfitCurrency = 4000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                CreatedByUserId = "1-1",
                DueDate = DateTime.Now.Date.AddDays(30),
                ExchangeRateDate = DateTime.Now.Date,
                DraftNumber = "1001",
                InvoiceCurrencyId = "1-3",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
               
                InvoiceNumber = "1001",
                IsClosed = false,
                LocalCurrencyId = "1-3",
                MainEntityId = "1-1",
               
                PaymentTermId = "1-1",
                ProfitCurrencyExchangeRate = 1,
                ProfitCurrencyId = "1-3",
                Tenant = 1,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "DR",
                UpdatedByUserId = "1-1",
               
                        },
                    };
                    arInvoiceObjectSet = new MockObjectSet<ARInvoice>(arinvoices);
                }
                return arInvoiceObjectSet;
            }
        }

        List<ARInvoiceLine> aRInvoiceLines;
        MockObjectSet<ARInvoiceLine> aRInvoiceLineObjectSet;
        public IDbSet<ARInvoiceLine> ARInvoiceLines
        {
            get
            {
                if (aRInvoiceLines == null)
                {
                    aRInvoiceLines = new List<ARInvoiceLine>()
                    {
                        new ARInvoiceLine(){ Id="1-1", Tenant=1 }
                    };

                    aRInvoiceLineObjectSet = new MockObjectSet<ARInvoiceLine>(aRInvoiceLines);
                }
                return aRInvoiceLineObjectSet;
            }
        }

        List<ARInvoiceType> aRInvoiceTypes;
        MockObjectSet<ARInvoiceType> aRInvoiceTypeObjectSet;
        public IDbSet<ARInvoiceType> ARInvoiceTypes
        {
            get
            {
                if (aRInvoiceTypes == null)
                {
                    aRInvoiceTypes = new List<ARInvoiceType>()
                    {
                        new ARInvoiceType(){ Code="IT"}
                    };

                    aRInvoiceTypeObjectSet = new MockObjectSet<ARInvoiceType>(aRInvoiceTypes);
                }
                return aRInvoiceTypeObjectSet;
            }
        }

       
        public IDbSet<ARInvoiceStatus> ARInvoiceStatuses
        {
            get
            {
                return new MockObjectSet<ARInvoiceStatus>(new List<ARInvoiceStatus>()
                {
                    new ARInvoiceStatus()
                    {
                        Code = "AC",
                        Name = "Approval Cancel",
                        SearchFields = "AC,Approval Cancel"
                    },
                    new ARInvoiceStatus()
                    {
                        Code = "AD",
                        Name = "Approved",
                        SearchFields = "AD,Approved"
                    },
                    new ARInvoiceStatus()
                    {
                        Code = "DR",
                        Name = "Draft",
                        SearchFields = "DR,Draft"
                    },
                    new ARInvoiceStatus()
                    {
                        Code = "PP",
                        Name = "Partially Paid",
                        SearchFields = "PP,Partially Paid"
                    },
                    new ARInvoiceStatus()
                    {
                        Code = "VD",
                        Name = "Void",
                        SearchFields = "VD,Void"
                    },
                    new ARInvoiceStatus()
                    {
                        Code = "PR",
                        Name = "Printed",
                        SearchFields = "PR,Printed"
                    },
                    new ARInvoiceStatus()
                    {
                        Code = "PD",
                        Name = "Paid",
                        SearchFields = "PD,Paid"
                    },
                });
            }
        }

        List<ARInvoiceTotalVAT> aRInvoiceTotalVATs;
        MockObjectSet<ARInvoiceTotalVAT> aRInvoiceTotalVATObjectSet;
        public IDbSet<ARInvoiceTotalVAT> ARInvoiceTotalVATs
        {
            get
            {
                if (aRInvoiceTotalVATs == null)
                {
                    aRInvoiceTotalVATs = new List<ARInvoiceTotalVAT>()
                    {
                        new ARInvoiceTotalVAT(){ Id="1-1", Tenant=1 }
                    };

                    aRInvoiceTotalVATObjectSet = new MockObjectSet<ARInvoiceTotalVAT>(aRInvoiceTotalVATs);
                }
                return aRInvoiceTotalVATObjectSet;
            }
        }

        List<ARInvoiceEntity> aRInvoiceEntities;
        MockObjectSet<ARInvoiceEntity> aRInvoiceEntityObjectSet;
        public IDbSet<ARInvoiceEntity> ARInvoiceEntities
        {
            get
            {
                if (aRInvoiceEntities == null)
                {
                    aRInvoiceEntities = new List<ARInvoiceEntity>()
                    {
                        new ARInvoiceEntity(){ Id="1-1", Tenant=1 }
                    };

                    aRInvoiceEntityObjectSet = new MockObjectSet<ARInvoiceEntity>(aRInvoiceEntities);
                }
                return aRInvoiceEntityObjectSet;
            }
        }

        public IDbSet<Account> Accounts
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<AccountType> AccountTypes
        {
            get { throw new NotImplementedException(); }
        }

        List<ARPayment> aRPayments;
        MockObjectSet<ARPayment> aRPaymentObjectSet;
        public IDbSet<ARPayment> ARPayments
        {
            get
            {
                if (aRPayments == null)
                {
                    aRPayments = new List<ARPayment>()
                    {
                        new ARPayment(){ Id="1-1", Tenant=1 }
                    };

                    aRPaymentObjectSet = new MockObjectSet<ARPayment>(aRPayments);
                }
                return aRPaymentObjectSet;
            }
        }

        List<AccountingPaymentMethod> paymentMethods;
        MockObjectSet<AccountingPaymentMethod> paymentMethodObjectSet;
        public IDbSet<AccountingPaymentMethod> PaymentMethods
        {
            get
            {
                if (paymentMethods == null)
                {
                    paymentMethods = new List<AccountingPaymentMethod>()
                    {
                        new AccountingPaymentMethod(){ Code="PM" }
                    };

                    paymentMethodObjectSet = new MockObjectSet<AccountingPaymentMethod>(paymentMethods);
                }
                return paymentMethodObjectSet;
            }
        }

        List<ARPaymentStatus> aRPaymentStatuses;
        MockObjectSet<ARPaymentStatus> aRPaymentStatusObjectSet;
        public IDbSet<ARPaymentStatus> ARPaymentStatus
        {
            get
            {
                if (aRPaymentStatuses == null)
                {
                    aRPaymentStatuses = new List<ARPaymentStatus>()
                    {
                        new ARPaymentStatus(){ Code="PS" }
                    };

                    aRPaymentStatusObjectSet = new MockObjectSet<ARPaymentStatus>(aRPaymentStatuses);
                }
                return aRPaymentStatusObjectSet;
            }
        }

        List<ARInvoicePayment> aRInvoicePayments;
        MockObjectSet<ARInvoicePayment> aRInvoicePaymentObjectSet;
        public IDbSet<ARInvoicePayment> ARInvoicePayments
        {
            get
            {
                if (aRInvoicePayments == null)
                {
                    aRInvoicePayments = new List<ARInvoicePayment>()
                    {
                        new ARInvoicePayment(){ Id="1-1", Tenant=1 }
                    };

                    aRInvoicePaymentObjectSet = new MockObjectSet<ARInvoicePayment>(aRInvoicePayments);
                }
                return aRInvoicePaymentObjectSet;
            }
        }

    
        List<APInvoice> apinvoices;
        MockObjectSet<APInvoice> apInvoiceObjectSet;
        public IDbSet<APInvoice> APInvoices
        {
            get
            {
                if (apinvoices == null)
                {
                    apinvoices = new List<APInvoice>() 
                    {
                        new APInvoice()
                        {
                             Id = "1-1",
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInLocalCurrency = 2000,
                AmountInInvoiceCurrency = 2000,
                AmountInProfitCurrency = 2000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                CreatedByUserId = "1-1",
                DueDate = DateTime.Now.Date.AddDays(30),
                ExchangeRateDate = DateTime.Now.Date,
                InternalNumber = "1002",
                InvoiceCurrencyId = "1-1",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
               
                InvoiceNumber = "1002",
                IsClosed = false,
                LocalCurrencyId = "1-1",
                MainEntityId = "1-1",
               
                PaymentTermId = "1-1",
                ProfitCurrencyExchangeRate = 1,
                ProfitCurrencyId = "1-1",
                Tenant = 1,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",
                        },

                          new APInvoice()
                        {
                             Id = "1-3",
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInLocalCurrency = 3000,
                AmountInInvoiceCurrency = 3000,
                AmountInProfitCurrency = 3000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                CreatedByUserId = "1-1",
                DueDate = DateTime.Now.Date.AddDays(30),
                ExchangeRateDate = DateTime.Now.Date,
                InternalNumber = "1001",
                InvoiceCurrencyId = "1-2",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
               
                InvoiceNumber = "1001",
                IsClosed = false,
                LocalCurrencyId = "1-2",
                MainEntityId = "1-1",
               
                PaymentTermId = "1-1",
                ProfitCurrencyExchangeRate = 1,
                ProfitCurrencyId = "1-2",
                Tenant = 2,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",
                        },
                               new APInvoice()
                        {
                             Id = "1-4",
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInLocalCurrency = 4000,
                AmountInInvoiceCurrency = 4000,
                AmountInProfitCurrency = 4000,
                BranchId = "1-1",
                CreateDate = DateTime.Now.Date,
                CreatedByUserId = "1-1",
                DueDate = DateTime.Now.Date.AddDays(30),
                ExchangeRateDate = DateTime.Now.Date,
                InternalNumber = "1001",
                InvoiceCurrencyId = "1-3",
                InvoiceCurrencyExchangeRate = 1,
                InvoiceDate = DateTime.Now.Date,
               
                InvoiceNumber = "1001",
                IsClosed = false,
                LocalCurrencyId = "1-3",
                MainEntityId = "1-1",
               
                PaymentTermId = "1-1",
                ProfitCurrencyExchangeRate = 1,
                ProfitCurrencyId = "1-3",
                Tenant = 1,
                UpdateDate = DateTime.Now.Date,
                StatusCode = "WA",
                UpdatedByUserId = "1-1",
                VendorId = "1-1",
                        },
                    };
                    apInvoiceObjectSet = new MockObjectSet<APInvoice>(apinvoices);
                }
                return apInvoiceObjectSet;
            }
        }

        List<APInvoiceLine> apInvoiceLines;
        MockObjectSet<APInvoiceLine> apInvoiceLineObjectSet;
        public IDbSet<APInvoiceLine> APInvoiceLines
        {
            get
            {
                if (apInvoiceLines == null)
                {
                    apInvoiceLines = new List<APInvoiceLine>()
                    {

                    };
                    apInvoiceLineObjectSet = new MockObjectSet<APInvoiceLine>(apInvoiceLines);
                }
                return apInvoiceLineObjectSet;
            }
        }

        public IDbSet<APInvoiceStatus> APInvoiceStatus
        {
            get
            {
                return new MockObjectSet<APInvoiceStatus>(new List<APInvoiceStatus>() 
            {
                new APInvoiceStatus() 
                {
                    Code = "AC", Name = "Approval Cancel", SearchFields = "AC,Approval Cancel" 
                } ,
                new APInvoiceStatus() 
                {
                    Code = "AD", Name = "Approved", SearchFields = "AD,Approved" 
                } ,
                new APInvoiceStatus() 
                {
                    Code = "PD", Name = "Paid", SearchFields = "PD,Paid" 
                } ,
                new APInvoiceStatus() 
                {
                    Code = "PP", Name = "Partially Paid", SearchFields = "PP,Partially Paid" 
                } ,
                new APInvoiceStatus() 
                {
                    Code = "VD", Name = "Void", SearchFields = "VD,Void" 
                } ,
                new APInvoiceStatus() 
                {
                    Code = "WA", Name = "Waiting For Approval", SearchFields = "WA,Waiting For Approval" 
                } ,
            });
            }
        }

        List<APInvoiceTotalVAT> invoiceTotalVats;
        MockObjectSet<APInvoiceTotalVAT> invoiceTotalVatsObjectSet;
        public IDbSet<APInvoiceTotalVAT> APInvoiceTotalVATs
        {
            get
            {
                if (invoiceTotalVats == null)
                {
                    invoiceTotalVats = new List<APInvoiceTotalVAT>();
                    invoiceTotalVatsObjectSet = new MockObjectSet<APInvoiceTotalVAT>(invoiceTotalVats);
                }
                return invoiceTotalVatsObjectSet;
            }
        }

        public IDbSet<APInvoiceType> APInvoiceTypes
        {
            get { throw new NotImplementedException(); }
        }

        List<APInvoiceEntity> apInvoiceEntities;
        MockObjectSet<APInvoiceEntity> apInvoiceEntitiesObjectSet;
        public IDbSet<APInvoiceEntity> APInvoiceEntities
        {
            get
            {
                if (apInvoiceEntities == null)
                {
                    apInvoiceEntities = new List<APInvoiceEntity>();

                    apInvoiceEntitiesObjectSet = new MockObjectSet<APInvoiceEntity>(apInvoiceEntities);
                }
                return apInvoiceEntitiesObjectSet;
            }
        }

        public IDbSet<APPayment> APPayments
        {
            get { throw new NotImplementedException(); }
        }
        
        public IDbSet<APPaymentStatus> APPaymentStatus
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<APInvoicePayment> APInvoicePayments
        {
            get { return new MockObjectSet<APInvoicePayment>(new List<APInvoicePayment>()); }
        }

        public IDbSet<AccountingTransferHeader> AccountingTransferHeaders
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<AccountingTransferLine> AccountingTransferLines
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<AccountingTransferType> AccountingTransferTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ARInvoiceTransferStatus> ARInvoiceTransferStatuses
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<APInvoiceTransferStatus> APInvoiceTransferStatuses
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<ARPaymentTransferStatus> ARPaymentTransferStatuses
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<APPaymentTransferStatus> APPaymentTransferStatuses
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ExternalSystemsTablesCode> ExternalSystemsTablesCodes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ExternalSystemsMissingTranslation> ExternalSystemsMissingTranslations
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<ExternalSystemsSyncStatus> ExternalSystemsSyncStatuses
        {
            get { throw new NotImplementedException(); }
            set { }
        }
        public IDbSet<ExpenseAllocationSetting> ExpenseAllocationSettings
        {
            get { throw new NotImplementedException(); }
            set { }
        }

        public IDbSet<ExpenseAllocationFlow> ExpenseAllocationFlows
        {
            get { throw new NotImplementedException(); }
            set { }
        }
        List<CreditCardType> creditCardTypes;
        MockObjectSet<CreditCardType> creditCardTypeObjectSet;
        public IDbSet<CreditCardType> CreditCardTypes
        {
            get
            {
                if (creditCardTypes == null)
                {
                    creditCardTypes = new List<CreditCardType>()
                    {
                        new CreditCardType(){ Code="CC" }
                    };

                    creditCardTypeObjectSet = new MockObjectSet<CreditCardType>(creditCardTypes);
                }
                return creditCardTypeObjectSet;
            }
        }

        public IDbSet<AccountingSystemsSetting> AccountingSystemsSettings
        {
            get { throw new NotImplementedException(); }
            set { }
        }
        public void SetAsModified(object entity)
        {
            //throw new NotImplementedException();
        }

        public void DetectChanges()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return 1;
        }



        public System.Data.Common.DbConnection GetConnection()
        {
            throw new NotImplementedException();
        }


        public DbContext GetActiveDbContext()
        {
            throw new NotImplementedException();
        }


        public IDbSet<AccountingSystemsSyncStatus> AccountingSystemsSyncStatuses
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public IDbSet<QuickbooksSyncRequestTicket> QuickbooksSyncRequestTickets
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public IDbSet<ARInvoiceLineAction> ARInvoiceLineActions
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {

            }
        }

        public IDbSet<SATInterface> SATInterfaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<SATInterfaceSetting> SATInterfaceSettings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<SATPaymentMethod> SATPaymentMethods
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<BankAccountLite> BankAccountLites
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<SATTransferStatus> SATTransferStatus
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<SATInvoiceStatus> SATInvoiceStatus
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<AccountingPaymentMethod> AccountingPaymentMethods
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<ARInvoiceStocksStatus> ARInvoiceStocksStatus
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<ARInvoiceStock> ARInvoiceStocks
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<ARInvoiceStockLine> ARInvoiceStockLines
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<ARInvoicesSignedStatus> ARInvoicesSignedStatuses
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<ConfirmationNumberStatus> ConfirmationNumberStatuses
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<ConfirmationNumberDefault> ConfirmationNumberDefaults
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ARPaymentChequeReplica> ARPaymentChequeReplicas
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ARPaymentChequeStatusReplica> ARPaymentChequeStatusReplicas
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ARPaymentBankTranfer> ARPaymentBankTranfers
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ARInvoiceChargesConstraint> ARInvoiceChargesConstraints => throw new NotImplementedException();

        public IDbSet<QBOGlobalTaxCalculation> QBOGlobalTaxCalculations {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<ARInvoiceAnalytic> ARInvoiceAnalytics => throw new NotImplementedException();

        public IDbSet<APInvoiceAnalytic> APInvoiceAnalytics => throw new NotImplementedException();

        public IDbSet<DigitalInvoicesCounterDataView> DigitalInvoicesCounterDataView
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ControlForInvoiceLinesDataView> ControlForInvoiceLinesDataView
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
