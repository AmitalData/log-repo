using System.Linq;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARPaymentQuery
    {
        ARPaymentRepository repository;
        public ARPaymentQuery()
        {
            repository = new ARPaymentRepository(); 
        }
        public ARPaymentQuery(int tenant)
        {
            repository = new ARPaymentRepository(tenant);
        }
        public ARPaymentQuery(ARPaymentRepository arPaymentRepository)
        {
            repository = arPaymentRepository;
        }

        public ARPaymentPM GetSinglePM(string id, int tenant)
        {
            ARPaymentPM payment = (from a in repository.context.ARPayments.Include("LocalCurrency").Include("Status").Include("TransferStatus").Include("PaymentCurrency").Include("BillToCard").Include("TransferStatus").Include("SATTransferStatus").Include("AccountingPaymentMethod").Include("Branch")
                                   where a.Id == id && a.Tenant == tenant
                                   select new ARPaymentPM()
                                   {
                                       ARAccountId = a.ARAccountId,
                                       AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                                       AccountingPaymentMethodCode = a.AccountingPaymentMethod == null ? null : a.AccountingPaymentMethod.Code,
                                       BillToAddressId = a.BillToAddressId,
                                       BillToId = a.BillToId,
                                       BranchId = a.BranchId,
                                       CreatedByUserId = a.CreatedByUserId,
                                       CreateDate = a.CreateDate,
                                       DebitAccountId = a.DebitAccountId,
                                       ExchangeRateDate = a.ExchangeRateDate,
                                       AmountInLocalCurrency = a.AmountInLocalCurrency,
                                       AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                                       Id = a.Id,
                                       InternalNotes = a.InternalNotes,
                                       IsClosed = a.IsClosed,
                                       LocalCurrencyId = a.LocalCurrencyId,
                                       PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                                       PaidBy = a.PaidBy,
                                       PaymentCurrencyId = a.PaymentCurrencyId,
                                       RegisterDate = a.RegisterDate,
                                       PaymentNo = a.PaymentNo,
                                       PrintByUserId = a.PrintByUserId,
                                       PrintDate = a.PrintDate,
                                       PrintNotes = a.PrintNotes,
                                       StatusCode = a.StatusCode,
                                       Tenant = a.Tenant,
                                       OpenAmount = a.OpenAmount,
                                       OpenAmountInLocalCurrency = a.OpenAmountInLocalCurrency,
                                       //SearchFields = a.SearchFields,
                                       Account = a.Account,
                                       Bank = a.Bank,
                                       BankBranch = a.BankBranch,
                                       ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                                       ValueDate = a.ValueDate,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       CreditCardTypeId = a.CreditCardTypeId,
                                       BankAccountId = a.BankAccountId,
                                       CashbookId = a.CashbookId,
                                       TransferTries = a.TransferTries,
                                       TransferError = a.TransferError,
                                       IsTransferStarted = a.IsTransferStarted,
                                       TransferStatusCode = a.TransferStatusCode,
                                       ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                       ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                       InvoiceNumber = a.InvoiceNumber,
                                       ShipmentNumber = a.ShipmentNumber,
                                       SATPaymentMethodCode = a.SATPaymentMethodCode,
                                       AmountInProfitCurrency = a.AmountInProfitCurrency,
                                       ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                       StatusName = a.Status != null ? a.Status.Name : null,
                                       TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                       BillToName = a.BillToCard == null ? "" : a.BillToCard.EnglishName,
                                       BillToLocalName = a.BillToCard == null ? "" : a.BillToCard.LocalName,
                                       BillToPartnerTypeId = a.BillToCard == null ? "" : a.BillToCard.PartnerTypeId,
                                       LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                       PaymentCurrencyCode = a.PaymentCurrency == null ? null : a.PaymentCurrency.Code,
                                       SATXML = a.SATXML,
                                       SATTransferStatusCode = a.SATTransferStatusCode,
                                       TransmissionError = a.TransmissionError,
                                       BankAccountLiteId = a.BankAccountLiteId,
                                       BankAccountName = a.BankAccountLite != null ? a.BankAccountLite.EnglishName : null,
                                       SATTransferStatusName = a.SATTransferStatus != null ? a.SATTransferStatus.Name : null,
                                       MetodoPagoCode = a.MetodoPagoCode,
                                       TipoCadenaPago = a.TipoCadenaPago,
                                       CadPago = a.CadPago,
                                       CertPago = a.CertPago,
                                       SelloPago = a.SelloPago,
                                       SATApprovalDate = a.SATApprovalDate,
                                       ApprovedDate = a.ApprovedDate,
                                       ApprovedByUserId = a.ApprovedByUserId,
                                       FirstApproveDate = a.FirstApproveDate,
                                       IsFullAccounting = a.IsFullAccounting,
                                       FechaPago = a.FechaPago,
                                       IsExternalEntity = a.IsExternalEntity,
                                       BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                       CreatedByPartner = a.CreatedByPartner,
                                       IsPaymentNumberManuallySet = a.IsPaymentNumberManuallySet,
                                   }).FirstOrDefault();


            BuildAllInvoicesNumbersField(payment);

            ARInvoicePaymentRepository entityRepository = new ARInvoicePaymentRepository(repository.context);
            ARInvoicePaymentQuery entityQuery = new ARInvoicePaymentQuery(entityRepository);
            payment.PaymentInvoices = entityQuery.GetARPaymentInvoicePMsForPayment(payment.Id, tenant);

            payment.ARPaymentChequeReplicas = GetARPaymentChequeReplicasByPaymentId(payment.Id, tenant);
            GetGLAccountFields(payment);
            payment =  SetJournalFields(payment);
            ARPaymentPM securedPM = new ARPaymentPM();
            SecuredMapping.GetMappedPM(payment, securedPM, "ARPayment", tenant);


            return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);
        }

        private List<ARPaymentChequeReplicaPM> GetARPaymentChequeReplicasByPaymentId(string paymentid, int tenant)
        {
            ARPaymentChequeReplicaQuery aRPaymentChequeReplicaQuery = new ARPaymentChequeReplicaQuery(tenant);
            return aRPaymentChequeReplicaQuery.GetARPaymentChequeReplicaPMsByPaymentId(paymentid, tenant);
        }

        void GetGLAccountFields(ARPaymentPM paymentPM)
        {
            GLAccountPM glaccount = getGLAccount(paymentPM.BillToId, paymentPM.Tenant);
            if (glaccount != null)
            {
                paymentPM.GLAccountId = glaccount.Id;
                paymentPM.GLAccountRecoMethodCode = glaccount.ReconcileMethodCode;
            }
        }
        private ARPaymentPM SetJournalFields(ARPaymentPM payment)
        {
            JournalPM journal = GetJournalByPaymentId(payment.Id, payment.Tenant);
            if (journal != null)
            {
                payment.JournalId = journal.Id;
                payment.JournalNumber = journal.JournalNumber;
            }
            return payment;
        }

        private JournalPM GetJournalByPaymentId(string paymentId, int tenant)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            JournalPM journal = journalQuery.GetJournalIdByAccountingEntityId(paymentId, tenant);
            return journal;



        }







        private GLAccountPM getGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }

            return glaAccount;
        }



        public ARPayment GetSingleARPayment(string id, int tenant)
        {
            ARPayment payment = (from a in repository.context.ARPayments.Include("LocalCurrency").Include("Status")
                                 where a.Id == id && a.Tenant == tenant
                                 select a).FirstOrDefault();

            return payment;
        }

        public bool CheckARPaymentNumber(string number,string id, int tenant)
        {
            bool exist = (from a in repository.context.ARPayments.Include("LocalCurrency").Include("Status")
                                 where a.PaymentNo == number &&a.Id != id && a.Tenant == tenant
                                 select a).Any();

            return exist;
        }


        public ARPaymentPM GetSinglePaymentByPaymentNumber_00(string paymentNo, int tenant)
        {
            
            AccountingPaymentMethodRepository paymentMethodRep = new AccountingPaymentMethodRepository(repository.context);
            ARPaymentStatusRepository arpaymentStatusRep = new ARPaymentStatusRepository(repository.context);
            ARPaymentPM payment = (from a in repository.context.ARPayments.Include("LocalCurrency").Include("TransferStatus").Include("Branch")
                                   where a.PaymentNo == paymentNo && a.Tenant == tenant
                                   select new ARPaymentPM()
                                   {
                                       ARAccountId = a.ARAccountId,
                                       AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                                       BillToAddressId = a.BillToAddressId,
                                       BillToId = a.BillToId,
                                       BillToPartnerTypeId = a.BillToCard == null ? "" : a.BillToCard.PartnerTypeId,
                                       BranchId = a.BranchId,
                                       CreatedByUserId = a.CreatedByUserId,
                                       CreateDate = a.CreateDate,
                                       DebitAccountId = a.DebitAccountId,
                                       ExchangeRateDate = a.ExchangeRateDate,
                                       AmountInLocalCurrency = a.AmountInLocalCurrency,
                                       AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                                       Id = a.Id,
                                       InternalNotes = a.InternalNotes,
                                       IsClosed = a.IsClosed,
                                       LocalCurrencyId = a.LocalCurrencyId,
                                       LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                       PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                                       PaidBy = a.PaidBy,
                                       PaymentCurrencyId = a.PaymentCurrencyId,
                                       RegisterDate = a.RegisterDate,
                                       PaymentNo = a.PaymentNo,
                                       PrintByUserId = a.PrintByUserId,
                                       PrintDate = a.PrintDate,
                                       PrintNotes = a.PrintNotes,
                                       StatusCode = a.StatusCode,
                                       OpenAmount = a.OpenAmount,
                                       OpenAmountInLocalCurrency = a.OpenAmountInLocalCurrency,
                                       Tenant = a.Tenant,
                                       //SearchFields = a.SearchFields,
                                       Account = a.Account,
                                       Bank = a.Bank,
                                       BankBranch = a.BankBranch,
                                       ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                                       ValueDate = a.ValueDate,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       CreditCardTypeId = a.CreditCardTypeId,
                                       BankAccountId = a.BankAccountId,
                                       BankAccountLiteId = a.BankAccountLiteId,
                                       BankAccountName = a.BankAccountLite != null ? a.BankAccountLite.EnglishName : null,
                                       CashbookId = a.CashbookId,
                                       TransferTries = a.TransferTries,
                                       TransferError = a.TransferError,
                                       IsTransferStarted = a.IsTransferStarted,
                                       TransferStatusCode = a.TransferStatusCode,
                                       TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                       ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                       ExternalAccountingEntityId=a.ExternalAccountingEntityId,
                                       InvoiceNumber = a.InvoiceNumber,
                                       ShipmentNumber = a.ShipmentNumber,
                                       SATPaymentMethodCode = a.SATPaymentMethodCode,
                                       SATTransferStatusCode = a.SATTransferStatusCode,
                                       TransmissionError = a.TransmissionError,
                                       SATTransferStatusName = a.SATTransferStatus != null ? a.SATTransferStatus.Name : null,
                                       MetodoPagoCode = a.MetodoPagoCode,
                                       TipoCadenaPago = a.TipoCadenaPago,
                                       CadPago = a.CadPago,
                                       CertPago = a.CertPago,
                                       SelloPago = a.SelloPago,
                                       SATApprovalDate = a.SATApprovalDate,
                                       ApprovedDate = a.ApprovedDate,
                                       ApprovedByUserId = a.ApprovedByUserId,
                                       FirstApproveDate = a.FirstApproveDate,
                                       IsFullAccounting = a.IsFullAccounting,
                                       FechaPago = a.FechaPago,
                                       BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                       CreatedByPartner = a.CreatedByPartner,
                                       IsPaymentNumberManuallySet = a.IsPaymentNumberManuallySet,
                                   }).FirstOrDefault();
            if (payment != null)
            {

                Currency currency = CurrencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, payment.Tenant, true);
                payment.PaymentCurrencyCode = currency != null ? currency.Code : null;

                ARPaymentStatus status = arpaymentStatusRep.GetSingleARPaymentStatus(payment.StatusCode);
                payment.StatusName = status != null ? status.Name : null;

                Contact createdByuser = ContactRepository.GetSingleContact(payment.CreatedByUserId, payment.Tenant, true);
                payment.CreatedByUserName = createdByuser != null ? createdByuser.EnglishName : null;

                Card billto = CardRepository.GetSingleCard(payment.BillToId, payment.Tenant, true);
                payment.BillToName = billto != null ? billto.EnglishName : null;
                payment.BillToLocalName = billto != null ? billto.LocalName : null;

                AccountingPaymentMethod method = paymentMethodRep.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant);
                payment.AccountingPaymentMethodName = method != null ? method.Name : null;
                payment.AccountingPaymentMethodCode = method != null ? method.Code : null;
                payment.ARPaymentChequeReplicas = GetARPaymentChequeReplicasByPaymentId(payment.Id, tenant);
                if (payment.BankAccountId != null)
                {
                    payment.BankAccountNumber = GetBankAccountNumberById(payment.BankAccountId, payment.Tenant);
                }
            }

            return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), payment, tenant); ;
        }
        private string GetBankAccountNumberById(string id, int tenant)
        {
            IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
            BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(id, tenant);
            if(bankAccount!= null)
            {
                return bankAccount.AccountNumber;
            }
            else
            {
                return null;
            }


        }

        public IQueryable<ARPaymentList> GetIQueryableEntityList(IQueryable<ARPayment> iQueryable)
        {
            IQueryable<ARPaymentList> query2 = from entity in iQueryable.Include("ARAccount").Include("AccountingPaymentMethod").Include("BillToCard").Include("CreatedByUser.Contact").Include("DebitAccount").Include("LocalCurrency").Include("PaymentCurrency").Include("Status").Include("TransferStatus").Include("SATTransferStatus").Include("Branch").Include("BankAccountLite")
                                               select new ARPaymentList()
                                               {
                                                   ARAccountId = entity.ARAccountId,
                                                   ARAccountName = entity.ARAccount == null ? null : entity.ARAccount.Name,
                                                   AccountingPaymentMethodId = entity.AccountingPaymentMethodId,
                                                   AccountingPaymentMethodCode = entity.AccountingPaymentMethod == null ? null : entity.AccountingPaymentMethod.Code,
                                                   AccountingPaymentMethodName = entity.AccountingPaymentMethod == null ? null : entity.AccountingPaymentMethod.Name,
                                                   BillToAddressId = entity.BillToAddressId,
                                                   BillToId = entity.BillToId,
                                                   BillToName = entity.BillToCard == null ? null : entity.BillToCard.EnglishName,
                                                   BillToLocalName = entity.BillToCard == null ? null : entity.BillToCard.LocalName,
                                                   BranchId = entity.BranchId,
                                                   BranchName = entity.Branch == null ? null : entity.Branch.EnglishName,
                                                   CreateByUserId = entity.CreatedByUserId,
                                                   CreatedByUserName = entity.CreatedByUser == null ? null : (entity.CreatedByUser.Contact == null ? null : entity.CreatedByUser.Contact.EnglishName),
                                                   CreateDate = entity.CreateDate,
                                                   DebitAccountId = entity.DebitAccountId,
                                                   CreditAccountName = entity.DebitAccount == null ? null : entity.DebitAccount.Name,
                                                   ExchangeRateDate = entity.ExchangeRateDate,
                                                   AmountInLocalCurrency = entity.AmountInLocalCurrency,
                                                   AmountInPaymentCurrency = entity.AmountInPaymentCurrency,
                                                   Id = entity.Id,
                                                   PaymentMethodName = entity.AccountingPaymentMethod == null ? null : entity.AccountingPaymentMethod.Name,
                                                   InternalNotes = entity.InternalNotes,
                                                   IsClosed = entity.IsClosed,
                                                   LocalCurrencyId = entity.LocalCurrencyId,
                                                   LocalCurrencyCode = entity.LocalCurrency != null ? entity.LocalCurrency.Code : null,
                                                   PaymentCurrencyExchangeRate = entity.PaymentCurrencyExchangeRate,
                                                   PaidBy = entity.PaidBy,
                                                   PaymentCurrencyId = entity.PaymentCurrencyId,
                                                   PaymentCurrencyCode = entity.PaymentCurrency == null ? null : entity.PaymentCurrency.Code,
                                                   RegisterDate = entity.RegisterDate,
                                                   PaymentNo = entity.PaymentNo,
                                                   PrintByUserId = entity.PrintByUserId,
                                                   PrintDate = entity.PrintDate,
                                                   PrintNotes = entity.PrintNotes,
                                                   StatusCode = entity.StatusCode,
                                                   StatusName = entity.Status == null ? null : entity.Status.Name,
                                                   Tenant = entity.Tenant,
                                                   SearchFields = entity.SearchFields,
                                                   OpenAmount = entity.OpenAmount,
                                                   OpenAmountInLocalCurrency = entity.OpenAmountInLocalCurrency,
                                                   CreditCardTypeId = entity.CreditCardTypeId,
                                                   Account = entity.Account,
                                                   ValueDate = entity.ValueDate,
                                                   ChequeOrPaymentRef = entity.ChequeOrPaymentRef,
                                                   Bank = entity.Bank,
                                                   BankBranch = entity.BankBranch,
                                                   AmountInProfitCurrency = entity.AmountInProfitCurrency,
                                                   ProfitCurrencyExchangeRate = entity.ProfitCurrencyExchangeRate,
                                                   BankAccountId = entity.BankAccountId,
                                                   BankAccountLiteId = entity.BankAccountLiteId,
                                                   BankAccountName = entity.BankAccountLite != null ? entity.BankAccountLite.EnglishName : null,
                                                   CashbookId = entity.CashbookId,
                                                   TransferTries = entity.TransferTries,
                                                   TransferError = entity.TransferError,
                                                   IsTransferStarted = entity.IsTransferStarted,
                                                   TransferStatusCode = entity.TransferStatusCode,
                                                   TransferStatusName = entity.TransferStatus == null ? "" : entity.TransferStatus.Name,
                                                   ReadyForTransfer = entity.TransferStatusCode == "RD" ? true : false,
                                                   InvoiceNumber = entity.InvoiceNumber,
                                                   ShipmentNumber = entity.ShipmentNumber,
                                                   SATPaymentMethodCode = entity.SATPaymentMethodCode,
                                                   SATTransferStatusCode = entity.SATTransferStatusCode,
                                                   SATTransferStatusName = entity.SATTransferStatus != null ? entity.SATTransferStatus.Name : null,
                                                   TransmissionError = entity.TransmissionError,

                                                   MetodoPagoCode = entity.MetodoPagoCode,
                                                   TipoCadenaPago = entity.TipoCadenaPago,
                                                   CadPago = entity.CadPago,
                                                   CertPago = entity.CertPago,
                                                   SelloPago = entity.SelloPago,
                                                   SATApprovalDate = entity.SATApprovalDate,
                                                   ApprovedDate = entity.ApprovedDate,
                                                   ApprovedByUserId = entity.ApprovedByUserId,
                                                   FirstApproveDate = entity.FirstApproveDate,
                                                   IsFullAccounting = entity.IsFullAccounting,
                                                   FechaPago = entity.FechaPago,
                                                   CreatedByPartner = entity.CreatedByPartner,
                                                   IsPaymentNumberManuallySet = entity.IsPaymentNumberManuallySet,
                                               };
            return query2;
        }

        public IQueryable<ARPaymentList> GetOpenedARPayments(int tenant)
        {
            var query = from entity in repository.context.ARPayments.Include("ARAccount").Include("AccountingPaymentMethod").Include("BillToCard").Include("CreatedByUser.Contact").Include("DebitAccount").Include("LocalCurrency").Include("PaymentCurrency").Include("Status").Include("TransferStatus").Include("BankAccountLite").Include("Branch")
                        where entity.Tenant == tenant && entity.StatusCode != "DR" && entity.StatusCode != "VD" && entity.StatusCode != "LL" && !entity.IsClosed
                        select new ARPaymentList()
                        {
                            ARAccountId = entity.ARAccountId,
                            ARAccountName = entity.ARAccount == null ? null : entity.ARAccount.Name,
                            AccountingPaymentMethodId = entity.AccountingPaymentMethodId,
                            AccountingPaymentMethodName = entity.AccountingPaymentMethod == null ? null : entity.AccountingPaymentMethod.Name,
                            AccountingPaymentMethodCode = entity.AccountingPaymentMethod == null ? null : entity.AccountingPaymentMethod.Code,
                            BillToAddressId = entity.BillToAddressId,
                            BillToId = entity.BillToId,
                            BillToName = entity.BillToCard == null ? null : entity.BillToCard.EnglishName,
                            BillToLocalName = entity.BillToCard == null ? null : entity.BillToCard.LocalName,
                            BranchId = entity.BranchId,
                            CreateByUserId = entity.CreatedByUserId,
                            CreatedByUserName = entity.CreatedByUser == null ? null : (entity.CreatedByUser.Contact == null ? null : entity.CreatedByUser.Contact.EnglishName),
                            CreateDate = entity.CreateDate,
                            DebitAccountId = entity.DebitAccountId,
                            CreditAccountName = entity.DebitAccount == null ? null : entity.DebitAccount.Name,
                            ExchangeRateDate = entity.ExchangeRateDate,
                            AmountInLocalCurrency = entity.AmountInLocalCurrency,
                            AmountInPaymentCurrency = entity.AmountInPaymentCurrency,
                            Id = entity.Id,
                            InternalNotes = entity.InternalNotes,
                            IsClosed = entity.IsClosed,
                            LocalCurrencyId = entity.LocalCurrencyId,
                            LocalCurrencyCode = entity.LocalCurrency != null ? entity.LocalCurrency.Code : null,
                            PaymentCurrencyExchangeRate = entity.PaymentCurrencyExchangeRate,
                            PaidBy = entity.PaidBy,
                            PaymentCurrencyId = entity.PaymentCurrencyId,
                            PaymentCurrencyCode = entity.PaymentCurrency == null ? null : entity.PaymentCurrency.Code,
                            RegisterDate = entity.RegisterDate,
                            PaymentNo = entity.PaymentNo,
                            PrintByUserId = entity.PrintByUserId,
                            PrintDate = entity.PrintDate,
                            PrintNotes = entity.PrintNotes,
                            StatusCode = entity.StatusCode,
                            StatusName = entity.Status == null ? null : entity.Status.Name,
                            Tenant = entity.Tenant,
                            SearchFields = entity.SearchFields,
                            OpenAmount = entity.OpenAmount,
                            OpenAmountInLocalCurrency = entity.OpenAmountInLocalCurrency,
                            CreditCardTypeId = entity.CreditCardTypeId,
                            Account = entity.Account,
                            ValueDate = entity.ValueDate,
                            ChequeOrPaymentRef = entity.ChequeOrPaymentRef,
                            Bank = entity.Bank,
                            BankBranch = entity.BankBranch,
                            AmountInProfitCurrency = entity.AmountInProfitCurrency,
                            ProfitCurrencyExchangeRate = entity.ProfitCurrencyExchangeRate,
                            BankAccountId = entity.BankAccountId,
                            BankAccountLiteId = entity.BankAccountLiteId,
                            CashbookId = entity.CashbookId,
                            TransferTries = entity.TransferTries,
                            TransferError = entity.TransferError,
                            IsTransferStarted = entity.IsTransferStarted,
                            TransferStatusCode = entity.TransferStatusCode,
                            TransferStatusName = entity.TransferStatus == null ? "" : entity.TransferStatus.Name,
                            ReadyForTransfer = entity.TransferStatusCode == "RD" ? true : false,
                            InvoiceNumber = entity.InvoiceNumber,
                            ShipmentNumber = entity.ShipmentNumber,
                            SATPaymentMethodCode = entity.SATPaymentMethodCode,
                            SATTransferStatusCode = entity.SATTransferStatusCode,
                            SATTransferStatusName = entity.SATTransferStatus != null ? entity.SATTransferStatus.Name : null,
                            TransmissionError = entity.TransmissionError,
                            MetodoPagoCode = entity.MetodoPagoCode,
                            TipoCadenaPago = entity.TipoCadenaPago,
                            CadPago = entity.CadPago,
                            CertPago = entity.CertPago,
                            SelloPago = entity.SelloPago,
                            SATApprovalDate = entity.SATApprovalDate,
                            BankAccountName = entity.BankAccountLite != null ? entity.BankAccountLite.EnglishName : null,
                            ApprovedDate = entity.ApprovedDate,
                            ApprovedByUserId = entity.ApprovedByUserId,
                            FirstApproveDate = entity.FirstApproveDate,
                            IsFullAccounting = entity.IsFullAccounting,
                            FechaPago = entity.FechaPago,
                            BranchName = entity.Branch == null ? null : entity.Branch.EnglishName,
                            CreatedByPartner = entity.CreatedByPartner,
                            IsPaymentNumberManuallySet = entity.IsPaymentNumberManuallySet,
                        };

            return query;
        }

        public IQueryable<ARPaymentList> GetARPaymentsList_00(int tenant)
        {
            var query = from entity in repository.context.ARPayments.Include("AccountingPaymentMethod").Include("CreatedByUser.Contact").Include("TransferStatus").Include("BankAccountLite").Include("Branch")
                        where entity.Tenant == tenant
                        select new ARPaymentList()
                        {
                            ARAccountId = entity.ARAccountId,
                            ARAccountName = entity.ARAccount == null ? null : entity.ARAccount.Name,
                            AccountingPaymentMethodId = entity.AccountingPaymentMethodId,
                            AccountingPaymentMethodName = entity.AccountingPaymentMethod == null ? null : entity.AccountingPaymentMethod.Name,
                            AccountingPaymentMethodCode = entity.AccountingPaymentMethod == null ? null : entity.AccountingPaymentMethod.Code,
                            BillToAddressId = entity.BillToAddressId,
                            BillToId = entity.BillToId,
                            BillToName = entity.BillToCard == null ? null : entity.BillToCard.EnglishName,
                            BillToLocalName = entity.BillToCard == null ? null : entity.BillToCard.LocalName,
                            BranchId = entity.BranchId,
                            CreateByUserId = entity.CreatedByUserId,
                            CreatedByUserName = entity.CreatedByUser == null ? null : (entity.CreatedByUser.Contact == null ? null : entity.CreatedByUser.Contact.EnglishName),
                            CreateDate = entity.CreateDate,
                            DebitAccountId = entity.DebitAccountId,
                            CreditAccountName = entity.DebitAccount == null ? null : entity.DebitAccount.Name,
                            ExchangeRateDate = entity.ExchangeRateDate,
                            AmountInLocalCurrency = entity.AmountInLocalCurrency,
                            AmountInPaymentCurrency = entity.AmountInPaymentCurrency,
                            Id = entity.Id,
                            InternalNotes = entity.InternalNotes,
                            IsClosed = entity.IsClosed,
                            LocalCurrencyId = entity.LocalCurrencyId,
                            LocalCurrencyCode = entity.LocalCurrency != null ? entity.LocalCurrency.Code : null,
                            PaymentCurrencyExchangeRate = entity.PaymentCurrencyExchangeRate,
                            PaidBy = entity.PaidBy,
                            PaymentCurrencyId = entity.PaymentCurrencyId,
                            PaymentCurrencyCode = entity.PaymentCurrency == null ? null : entity.PaymentCurrency.Code,
                            RegisterDate = entity.RegisterDate,
                            PaymentNo = entity.PaymentNo,
                            PrintByUserId = entity.PrintByUserId,
                            PrintDate = entity.PrintDate,
                            PrintNotes = entity.PrintNotes,
                            StatusCode = entity.StatusCode,
                            StatusName = entity.Status == null ? null : entity.Status.Name,
                            Tenant = entity.Tenant,
                            SearchFields = entity.SearchFields,
                            OpenAmount = entity.OpenAmount,
                            OpenAmountInLocalCurrency = entity.OpenAmountInLocalCurrency,
                            CreditCardTypeId = entity.CreditCardTypeId,
                            Account = entity.Account,
                            ValueDate = entity.ValueDate,
                            ChequeOrPaymentRef = entity.ChequeOrPaymentRef,
                            Bank = entity.Bank,
                            BankBranch = entity.BankBranch,
                            AmountInProfitCurrency = entity.AmountInProfitCurrency,
                            ProfitCurrencyExchangeRate = entity.ProfitCurrencyExchangeRate,
                            BankAccountId = entity.BankAccountId,
                            BankAccountLiteId = entity.BankAccountLiteId,
                            CashbookId = entity.CashbookId,
                            TransferTries = entity.TransferTries,
                            TransferError = entity.TransferError,
                            IsTransferStarted = entity.IsTransferStarted,
                            TransferStatusCode = entity.TransferStatusCode,
                            TransferStatusName = entity.TransferStatus == null ? "" : entity.TransferStatus.Name,
                            ReadyForTransfer = entity.TransferStatusCode == "RD" ? true : false,
                            InvoiceNumber = entity.InvoiceNumber,
                            ShipmentNumber = entity.ShipmentNumber,
                            SATPaymentMethodCode = entity.SATPaymentMethodCode,
                            SATTransferStatusCode = entity.SATTransferStatusCode,
                            SATTransferStatusName = entity.SATTransferStatus != null ? entity.SATTransferStatus.Name : null,
                            TransmissionError = entity.TransmissionError,
                            MetodoPagoCode = entity.MetodoPagoCode,
                            TipoCadenaPago = entity.TipoCadenaPago,
                            CadPago = entity.CadPago,
                            CertPago = entity.CertPago,
                            SelloPago = entity.SelloPago,
                            SATApprovalDate = entity.SATApprovalDate,
                            BankAccountName = entity.BankAccountLite != null ? entity.BankAccountLite.EnglishName : null,
                            ApprovedDate = entity.ApprovedDate,
                            ApprovedByUserId = entity.ApprovedByUserId,
                            FirstApproveDate = entity.FirstApproveDate,
                            IsFullAccounting = entity.IsFullAccounting,
                            FechaPago = entity.FechaPago,
                            BranchName = entity.Branch == null ? null : entity.Branch.EnglishName,
                            CreatedByPartner = entity.CreatedByPartner,
                            IsPaymentNumberManuallySet = entity.IsPaymentNumberManuallySet,
                        };

            return query;
        }

        public ARPaymentList GetPaymentByPaymentNumber(string paymentNo, int tenant)
        {
            ARPaymentList payment = (from a in repository.context.ARPayments.Include("TransferStatus").Include("BankAccountLite").Include("Branch")
                                     where a.PaymentNo == paymentNo && a.Tenant == tenant
                                     select new ARPaymentList()
                                     {
                                         Id = a.Id,
                                         Tenant = a.Tenant,
                                         ARAccountId = a.ARAccountId,
                                         AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                                         BillToAddressId = a.BillToAddressId,
                                         BillToId = a.BillToId,
                                         BranchId = a.BranchId,
                                         CreateDate = a.CreateDate,
                                         DebitAccountId = a.DebitAccountId,
                                         ExchangeRateDate = a.ExchangeRateDate,
                                         AmountInLocalCurrency = a.AmountInLocalCurrency,
                                         AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                                         InternalNotes = a.InternalNotes,
                                         IsClosed = a.IsClosed,
                                         LocalCurrencyId = a.LocalCurrencyId,
                                         PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                                         PaidBy = a.PaidBy,
                                         PaymentCurrencyId = a.PaymentCurrencyId,
                                         RegisterDate = a.RegisterDate,
                                         PaymentNo = a.PaymentNo,
                                         PrintByUserId = a.PrintByUserId,
                                         PrintDate = a.PrintDate,
                                         PrintNotes = a.PrintNotes,
                                         StatusCode = a.StatusCode,
                                         OpenAmount = a.OpenAmount,
                                         OpenAmountInLocalCurrency = a.OpenAmountInLocalCurrency,
                                         BankBranch = a.BankBranch,
                                         ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                                         ValueDate = a.ValueDate,
                                         UpdateDate = a.UpdateDate,
                                         UpdatedByUserId = a.UpdatedByUserId,
                                         CreditCardTypeId = a.CreditCardTypeId,
                                         BankAccountId = a.BankAccountId,
                                         BankAccountLiteId = a.BankAccountLiteId,
                                         CashbookId = a.CashbookId,
                                         TransferTries = a.TransferTries,
                                         TransferError = a.TransferError,
                                         IsTransferStarted = a.IsTransferStarted,
                                         TransferStatusCode = a.TransferStatusCode,
                                         TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                         ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                         InvoiceNumber = a.InvoiceNumber,
                                         ShipmentNumber = a.ShipmentNumber,
                                         SATPaymentMethodCode = a.SATPaymentMethodCode,
                                         SATTransferStatusCode = a.SATTransferStatusCode,
                                         SATTransferStatusName = a.SATTransferStatus != null ? a.SATTransferStatus.Name : null,
                                         TransmissionError = a.TransmissionError,
                                         MetodoPagoCode = a.MetodoPagoCode,
                                         TipoCadenaPago = a.TipoCadenaPago,
                                         CadPago = a.CadPago,
                                         CertPago = a.CertPago,
                                         SelloPago = a.SelloPago,
                                         SATApprovalDate = a.SATApprovalDate,
                                         BankAccountName = a.BankAccountLite != null ? a.BankAccountLite.EnglishName : null,
                                         ApprovedDate = a.ApprovedDate,
                                         ApprovedByUserId = a.ApprovedByUserId,
                                         FirstApproveDate = a.FirstApproveDate,
                                         IsFullAccounting = a.IsFullAccounting,
                                         FechaPago = a.FechaPago,
                                         BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                         CreatedByPartner = a.CreatedByPartner,
                                         IsPaymentNumberManuallySet = a.IsPaymentNumberManuallySet,
                                     }).FirstOrDefault();

            return payment;
        }
        private void BuildAllInvoicesNumbersField(ARPaymentPM payment)
        {
            if (payment.PaymentInvoices != null && payment.PaymentInvoices.Count > 0)
            {
                foreach (ARPaymentInvoicePM item in payment.PaymentInvoices)
                {
                    string aRInvoiceNumber = item.ARInvoiceNumber + ",";
                    payment.InvoiceNumbers += aRInvoiceNumber;
                }

                if (!string.IsNullOrEmpty(payment.InvoiceNumbers))
                {
                    payment.InvoiceNumbers = payment.InvoiceNumbers.Remove(payment.InvoiceNumbers.Length - 1, 1);
                }
            }
        }

      
    }
}