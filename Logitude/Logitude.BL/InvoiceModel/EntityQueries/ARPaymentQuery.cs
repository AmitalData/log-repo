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
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARPaymentQuery
    {
        const string ActionTypeCode_Credit = "1";

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
        public ARPaymentPM GetSinglePMForInterest(InterestTransactionList interestTransactionLists)
        {
            ARPaymentPM payment = (from a in repository.context.ARPayments 
                                   where a.Id == interestTransactionLists.EntityId && 
                                         a.Tenant == interestTransactionLists.Tenant
                                   select new ARPaymentPM()
                                   {
                                       Id = a.Id,
                                       PaymentNo = a.PaymentNo,
                                       Tenant = a.Tenant,

                                   }).FirstOrDefault();
            if (payment!=null)
            {
                payment = SetInterestJournalFields(payment, interestTransactionLists);
            }

            return payment;

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

                                       AccountingCancelationDate = a.AccountingCancelationDate,
                                       CancelationNotes = a.CancelationNotes,

                                       PaymentCurrencySign= a.PaymentCurrency.Sign,
                                       PartnerId = a.PartnerId,

                                   }).FirstOrDefault();


            BuildAllInvoicesNumbersField(payment);

            ARInvoicePaymentRepository entityRepository = new ARInvoicePaymentRepository(repository.context);
            ARInvoicePaymentQuery entityQuery = new ARInvoicePaymentQuery(entityRepository);
            payment.PaymentInvoices = entityQuery.GetARPaymentInvoicePMsForPayment(payment.Id, tenant);

            payment.ARPaymentChequeReplicas = GetARPaymentChequeReplicasByPaymentId(payment.Id, tenant);
            payment.ARPaymentBankTranfers = GetARPaymentBankTranfersByPaymentId(payment.Id, tenant);
            
            SetGLAccountFields(payment);
            payment =  SetJournalFields(payment);

            ARPaymentPM securedPM = new ARPaymentPM();
            SecuredMapping.GetMappedPM(payment, securedPM, "ARPayment", tenant);
            if (payment != null)
                MapCustomFieldValues(securedPM);
            if (payment.StatusCode == "VD")
            {
                JournalPM voidedByJournal = GetApprovedJournalByAccountingEntityId(payment);
                if (voidedByJournal != null)
                {
                    payment.VoidedByJournalNumber = voidedByJournal.JournalNumber;
                    securedPM.VoidedByJournalNumber = voidedByJournal.JournalNumber;
                }
            }

            return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);
        }


        private JournalPM GetApprovedJournalByAccountingEntityId(ARPaymentPM aRPaymentPM)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            return journalQuery.GetApprovedJournalByAccountingEntityId(aRPaymentPM.Id, "3", aRPaymentPM.Tenant);


        }
        private void MapCustomFieldValues(ARPaymentPM payment)
        {
            if (payment != null)
            {
                ARPayment entityPOC = (from s in repository.context.ARPayments
                                       where s.Id == payment.Id
                                       select s).FirstOrDefault();

                payment.Field1 = new CustomFieldClass("Field1", "ARPayment", entityPOC.Field1);
                payment.Field2 = new CustomFieldClass("Field2", "ARPayment", entityPOC.Field2);
                payment.Field3 = new CustomFieldClass("Field3", "ARPayment", entityPOC.Field3);
                payment.Field4 = new CustomFieldClass("Field4", "ARPayment", entityPOC.Field4);
                payment.Field5 = new CustomFieldClass("Field5", "ARPayment", entityPOC.Field5);
                payment.Field6 = new CustomFieldClass("Field6", "ARPayment", entityPOC.Field6);
                payment.Field7 = new CustomFieldClass("Field7", "ARPayment", entityPOC.Field7);
                payment.Field8 = new CustomFieldClass("Field8", "ARPayment", entityPOC.Field8);
                payment.Field9 = new CustomFieldClass("Field9", "ARPayment", entityPOC.Field9);
                payment.Field10 = new CustomFieldClass("Field10", "ARPayment", entityPOC.Field10);
            }
        }


        private List<ARPaymentChequeReplicaPM> GetARPaymentChequeReplicasByPaymentId(string paymentid, int tenant)
        {
            ARPaymentChequeReplicaQuery aRPaymentChequeReplicaQuery = new ARPaymentChequeReplicaQuery(tenant);
            return aRPaymentChequeReplicaQuery.GetARPaymentChequeReplicaPMsByPaymentId(paymentid, tenant);
        }

        private List<ARPaymentBankTranferPM> GetARPaymentBankTranfersByPaymentId(string paymentid, int tenant)
        {
            IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
            
            ARPaymentBankTranferRepository aRPaymentBankTranferRepository = new ARPaymentBankTranferRepository(tenant);
            List<ARPaymentBankTranfer> arPaymentBankTranfers = aRPaymentBankTranferRepository.GetARPaymentBankTranfers(paymentid, tenant).ToList();
            var list = (from a in arPaymentBankTranfers
                        select new ARPaymentBankTranferPM()
                        {
                            Id = a.Id,
                            Tenant = a.Tenant,
                            PaymentId = a.PaymentId,
                            PaymentRef = a.PaymentRef,
                            ValueDate = a.ValueDate,
                            BankAccountId = a.BankAccountId,
                            CurrencyId = a.CurrencyId,
                            LineNumber = a.LineNumber,
                            LocalAmount = a.LocalAmount,
                            ForeignAmount = a.ForeignAmount,
                            ExchageRate = a.ExchageRate,
                        }).OrderBy(d => d.LineNumber).ToList();

                        list.ForEach(item => {
                            BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(item.BankAccountId, tenant);
                            if (bankAccount != null)
                            {
                                item.BankAccount = new BankAccountLightPM { Id = bankAccount.Id, LocalName = bankAccount.LocalName, EnglishName = bankAccount.EnglishName, BankAccountNumber = bankAccount.AccountNumber };
                            }
                            item.BankAccountNumber = item.BankAccount!= null ? item.BankAccount.BankAccountNumber : null;
                        });
            return list;
        }

        void SetGLAccountFields(ARPaymentPM paymentPM)
        {

            if (paymentPM.IsFullAccounting)
            {
                GLAccountPM glaccount = getGLAccount(paymentPM.BillToId, paymentPM);
                if (glaccount != null)
                {
                    paymentPM.GLAccountId = glaccount.Id;
                    paymentPM.GLAccountRecoMethodCode = glaccount.ReconcileMethodCode;
                    paymentPM.GLAccountCurrencyCode = glaccount.CurrencyCode;
                }

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
            JournalPM journal = journalQuery.GetJournalByAccountingEntityIdAndCode(paymentId,"3",tenant);
            return journal;

        }

        private ARPaymentPM SetInterestJournalFields(ARPaymentPM payment, InterestTransactionList interestTransactionLists)
        {
            List<JournalPM> journals = GetJournalsWithLinesByPaymentId(payment.Id, payment.Tenant);
            if (journals != null)
            {
                SetInterestJournalFieldsForPayment(journals, payment, interestTransactionLists);
            }
            return payment;
        }


        private void SetInterestJournalFieldsForPayment(List<JournalPM> journals, ARPaymentPM payment, InterestTransactionList interestTransactionLists)
        {
            foreach (JournalPM journal in journals)
            {
                decimal? journalTotalLocalAmount= journal.JournalLines.Where(s=>s.ActionTypeCode == ActionTypeCode_Credit).Sum(s=>s.LocalAmount);
                if (interestTransactionLists.LocalAmount == journalTotalLocalAmount)
                {
                    payment.JournalId = journal.Id;
                    payment.JournalNumber = journal.JournalNumber;
                    break;
                }

            }
        }

        private List<JournalPM> GetJournalsWithLinesByPaymentId(string paymentId, int tenant)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            List<JournalPM> journals = journalQuery.GetJournalsWithLinesByAccountingEntityIdAndCode(paymentId, "3", tenant);
            return journals;
        }

        private string GetAccountIdForGLAccountCurrency(GLAccountPM gLAccount, string paymentCurrencyId)
        {
            GLAccountCurrencyRepository glAccountCurrencyRepository = new GLAccountCurrencyRepository(gLAccount.Tenant);
            GLAccountCurrency gLAccountCurrency = glAccountCurrencyRepository.GetEntityByCurrencyAndGLAccountId(gLAccount.Id, paymentCurrencyId, gLAccount.Tenant);
            if (gLAccountCurrency != null)
            {
                return gLAccountCurrency.GLAccountId;
            }
            else return gLAccount.Id;

        }
        
        private GLAccountPM getGLAccount(string billToId, ARPaymentPM payment)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(payment.Tenant);
            Card card = cardRep.GetSingleCard(billToId, payment.Tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, payment.Tenant);
                if (glaAccount != null && glaAccount.IsMultiCurrency.Value)
                {
                    string splitByCurrencyAccountId = GetAccountIdForGLAccountCurrency(glaAccount, payment.PaymentCurrencyId);
                    glaAccount = glAccountQuery.GetSingleGLAccountPM(splitByCurrencyAccountId, payment.Tenant);
                }
                else return glaAccount;
            }

            return glaAccount;
        }

        public List<ARPaymentPM> GetARpaymentsForCard(string billtoId, int tenant)
        {
            List<ARPaymentPM> payments= (from a in repository.context.ARPayments where
                   a.BillToId == billtoId && a.Tenant == tenant
                   select new ARPaymentPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                   
                   }
                   
                   ).ToList();
            foreach(ARPaymentPM paymentPM in payments)
            {
                paymentPM.ARPaymentChequeReplicas = GetARPaymentChequeReplicasByPaymentId(paymentPM.Id, paymentPM.Tenant);
            }
            return payments;
        } 


        public ARPayment GetSingleARPayment(string id, int tenant)
        {
            ARPayment payment = (from a in repository.context.ARPayments.Include("LocalCurrency").Include("Status")
                                 where a.Id == id && a.Tenant == tenant
                                 select a).FirstOrDefault();

            return payment;
        }

        public bool CheckARPaymentNumber(string number, string id, int tenant)
        {
            bool exist = (from a in repository.context.ARPayments.Include("LocalCurrency").Include("Status")
                          where a.PaymentNo == number && a.Id != id && a.Tenant == tenant
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
                                       ExternalAccountingEntityId = a.ExternalAccountingEntityId,
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
                                       AccountingCancelationDate = a.AccountingCancelationDate,
                                       CancelationNotes = a.CancelationNotes,
                                       PartnerId = a.PartnerId,
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
                payment.ARPaymentBankTranfers = GetARPaymentBankTranfersByPaymentId(payment.Id, tenant);

                if (payment.BankAccountId != null)
                {
                    payment.BankAccountNumber = GetBankAccountNumberById(payment.BankAccountId, payment.Tenant);
                }

                MapCustomFieldValues(payment);
            }

          

            return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), payment, tenant); ;
        }
        private string GetBankAccountNumberById(string id, int tenant)
        {
            IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
            BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(id, tenant);
            if (bankAccount != null)
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
                                                   Field1 = entity.Field1,
                                                   Field2 = entity.Field2,
                                                   Field3 = entity.Field3,
                                                   Field4 = entity.Field4,
                                                   Field5 = entity.Field5,
                                                   Field6 = entity.Field6,
                                                   Field7 = entity.Field7,
                                                   Field8 = entity.Field8,
                                                   Field9 = entity.Field9,
                                                   Field10 = entity.Field10,
                                                   AccountingCancelationDate = entity.AccountingCancelationDate,
                                                   CancelationNotes = entity.CancelationNotes,
                                                   PartnerId = entity.PartnerId,
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
                            Field1 = entity.Field1,
                            Field2 = entity.Field2,
                            Field3 = entity.Field3,
                            Field4 = entity.Field4,
                            Field5 = entity.Field5,
                            Field6 = entity.Field6,
                            Field7 = entity.Field7,
                            Field8 = entity.Field8,
                            Field9 = entity.Field9,
                            Field10 = entity.Field10,
                            AccountingCancelationDate = entity.AccountingCancelationDate,
                            CancelationNotes = entity.CancelationNotes,
                            PartnerId = entity.PartnerId,
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
                            Field1 = entity.Field1,
                            Field2 = entity.Field2,
                            Field3 = entity.Field3,
                            Field4 = entity.Field4,
                            Field5 = entity.Field5,
                            Field6 = entity.Field6,
                            Field7 = entity.Field7,
                            Field8 = entity.Field8,
                            Field9 = entity.Field9,
                            Field10 = entity.Field10,
                            AccountingCancelationDate = entity.AccountingCancelationDate,
                            CancelationNotes = entity.CancelationNotes,
                            PartnerId = entity.PartnerId,
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
                                         Field1 = a.Field1,
                                         Field2 = a.Field2,
                                         Field3 = a.Field3,
                                         Field4 = a.Field4,
                                         Field5 = a.Field5,
                                         Field6 = a.Field6,
                                         Field7 = a.Field7,
                                         Field8 = a.Field8,
                                         Field9 = a.Field9,
                                         Field10 = a.Field10,
                                         AccountingCancelationDate = a.AccountingCancelationDate,
                                         CancelationNotes = a.CancelationNotes,
                                         PartnerId = a.PartnerId,
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

        public string GetARPaymentNumber(string arPaymentId, int tenant)
        {
            ARPaymentRepository aRPaymentRepository = new ARPaymentRepository(tenant);
            string arPaymentNo = aRPaymentRepository.GetARPaymentNumber(arPaymentId, tenant);
            return arPaymentNo;
        }

        public User getUserByARPayment(ARPayment aRPayment)
        {
            User createByUser = repository.getUserByARPayment(aRPayment);
            return createByUser;
        }
    }
}