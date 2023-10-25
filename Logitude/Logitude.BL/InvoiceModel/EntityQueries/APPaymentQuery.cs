using System.Linq;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class APPaymentQuery
    {
        APPaymentRepository repository;

        public APPaymentQuery()
        {
            repository = new APPaymentRepository(); 
        }

        public APPaymentQuery(int tenant)
        {
            repository = new APPaymentRepository(tenant);
        }

        public APPaymentQuery(APPaymentRepository apPaymentRepository)
        {
            repository = apPaymentRepository;
        }

        public APPaymentPM GetSinglePM(string id, int tenant)
        {
            APInvoicePaymentRepository apInvoicePaymentRepository = new APInvoicePaymentRepository(repository.context);
            APInvoicePaymentQuery apInvoicePaymentQuery = new APInvoicePaymentQuery(apInvoicePaymentRepository);
            AccountingPaymentMethodRepository apPaymentMethodRep = new AccountingPaymentMethodRepository(repository.context);
            Contact loggedContact = GetLogContact(tenant);

            APPaymentPM payment = (from a in repository.context.APPayments.Include("LocalCurrency").Include("TransferStatus").Include("Status").Include("Branch")
                                   where a.Id == id && a.Tenant == tenant
                                   select new APPaymentPM()
                                   {
                                       CreateDate = a.CreateDate,
                                       Id = a.Id,
                                       InternalNotes = a.InternalNotes,
                                       IsClosed = a.IsClosed,
                                       LocalCurrencyId = a.LocalCurrencyId,
                                       LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                       PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                                       PaymentCurrencyId = a.PaymentCurrencyId,
                                       RegisterDate = a.RegisterDate,
                                       PaymentNo = a.PaymentNo,
                                       PrintDate = a.PrintDate,
                                       PrintNotes = a.PrintNotes,
                                       StatusCode = a.StatusCode,
                                       OpenAmount = a.OpenAmount,
                                       Tenant = a.Tenant,
                                       Account = a.Account,
                                       AmountInLocalCurrency = a.AmountInLocalCurrency,
                                       AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                                       Bank = a.Bank,
                                       BankBranch = a.BankBranch,
                                       ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                                       CreatedByUserId = a.CreatedByUserId,
                                       PaymentCurrencyExchangeRateDate = a.PaymentCurrencyExchangeRateDate,
                                       AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                                       PrintedByUserId = a.PrintedByUserId,
                                       VendorAddressId = a.VendorAddressId,
                                       VendorId = a.VendorId,
                                       ValueDate = a.ValueDate,
                                       StatusName = a.Status.Name,
                                       CreditCardTypeId = a.CreditCardTypeId,
                                       BranchId = a.BranchId,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       VendorName = a.VendorCard == null ? "" : (loggedContact.DontShowLocalLabels ? a.VendorCard.EnglishName : a.VendorCard.LocalName),
                                       VendorLocalName = a.VendorCard == null ? "" : a.VendorCard.LocalName,
                                       VendorCode = a.VendorCard == null ? "" : a.VendorCard.Code,
                                       ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                       TransferError = a.TransferError,
                                       TransferStatusCode = a.TransferStatusCode,
                                       TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                       ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                       VendorPartnerTypeId = a.VendorCard == null ? "" : a.VendorCard.PartnerTypeId,
                                       TaxDeductionLocalAmount = a.TaxDeductionLocalAmount,
                                       TaxDeductionPercentage = a.TaxDeductionPercentage,
                                       ApprovedByUserId = a.ApprovedByUserId,
                                       ApprovedDateTime = a.ApprovedDateTime,
                                       BankAccountId = a.BankAccountId,
                                       FirstApproveDate = a.FirstApproveDate,
                                       AutomaticPaymentCheque = a.AutomaticPaymentCheque,
                                       BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                       VendorBankAccountNumber = a.VendorBankAccountNumber,
                                       VendorBankAddress = a.VendorBankAddress,
                                       VendorBankName = a.VendorBankName,
                                       VendorIBANNumber = a.VendorIBANNumber,
                                       VendorSwift = a.VendorSwift,
                                       AccountingCancelationDate = a.AccountingCancelationDate,
                                       CancelationNotes = a.CancelationNotes,
                                       DontIncludeInDeductionReport = a.DontIncludeInDeductionReport,
                                       ExternalPaymentAmount = a.ExternalPaymentAmount,
                                       ExternalPaymentDate = a.ExternalPaymentDate,
                                       ExternalPaymentNotes = a.ExternalPaymentNotes,
                                       ConnectedInvoicesNumbers = a.ConnectedInvoicesNumbers,
                                       VendorGLAccountId = a.VendorCard != null ? a.VendorCard.GLAccountId : null,
                                   }).FirstOrDefault();

            payment.PaymentInvoices = apInvoicePaymentQuery.GetAPPaymentInvoicePMsForPayment(payment.Id, tenant);

            AccountingPaymentMethod method = apPaymentMethodRep.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant);
            payment.PaymentMethodName = method != null ? method.Name : null;
            payment.PaymentMethodCode = method != null ? method.Code : null;

            Currency currency = CurrencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, payment.Tenant, true);
            payment.PaymentCurrencyCode = currency != null ? currency.Code : null;
            if (payment.StatusCode == "VD")
            {
                JournalPM voidedByJournal = GetApprovedJournalByAccountingEntityId(payment);
                if (voidedByJournal != null)
                {
                    payment.VoidedByJournalNumber = voidedByJournal.JournalNumber;
                }
            }
            APPaymentPM securedPM = new APPaymentPM();
            SecuredMapping.GetMappedPM(payment, securedPM, "APPayment", tenant);

            MapCustomFieldValues(payment, securedPM);

            if (IsFullAccountingActivated(tenant))
                MapJournalFields(securedPM);

            return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);
        }

        private void MapCustomFieldValues(APPaymentPM payment, APPaymentPM securedPM)
        {
            if (securedPM != null && payment != null)
            {
                APPayment entityPOC = (from s in repository.context.APPayments
                                       where s.Id == securedPM.Id
                                       select s).FirstOrDefault();

                securedPM.Field1 = new CustomFieldClass("Field1", "APPayment", entityPOC.Field1);
                securedPM.Field2 = new CustomFieldClass("Field2", "APPayment", entityPOC.Field2);
                securedPM.Field3 = new CustomFieldClass("Field3", "APPayment", entityPOC.Field3);
                securedPM.Field4 = new CustomFieldClass("Field4", "APPayment", entityPOC.Field4);
                securedPM.Field5 = new CustomFieldClass("Field5", "APPayment", entityPOC.Field5);
                securedPM.Field6 = new CustomFieldClass("Field6", "APPayment", entityPOC.Field6);
                securedPM.Field7 = new CustomFieldClass("Field7", "APPayment", entityPOC.Field7);
                securedPM.Field8 = new CustomFieldClass("Field8", "APPayment", entityPOC.Field8);
                securedPM.Field9 = new CustomFieldClass("Field9", "APPayment", entityPOC.Field9);
                securedPM.Field10 = new CustomFieldClass("Field10", "APPayment", entityPOC.Field10);
            }
        }

        private JournalPM GetApprovedJournalByAccountingEntityId(APPaymentPM aPPaymentPM)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            return journalQuery.GetApprovedJournalByAccountingEntityId(aPPaymentPM.Id, "5", aPPaymentPM.Tenant);


        }

        private void MapJournalFields(APPaymentPM entityPM)
        {
            Journal journal = GetJournalOfAPPayment(entityPM);
            if (journal != null)
            {
                entityPM.JournalId = journal.Id;
                entityPM.JournalNumber = journal.JournalNumber;
            }
        }
        private Journal GetJournalOfAPPayment(APPaymentPM entityPM)
        {
            JournalRepository rep = new JournalRepository(entityPM.Tenant);
            Journal journal = rep.GetByAccountingEntityId(entityPM.Id, "5", entityPM.Tenant); // 5- APPayment
            return journal;
        }
        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPoco = tenantRepository.GetSingleTenant(tenant);
            return (tenantPoco != null && tenantPoco.AccountingActivated);
        }
        public Contact  GetLogContact(int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + tenant + ".com";
            }
           
            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);
           

            return contact;



        }
        public APPaymentPM GetSingleAPPaymentPM(string id, int tenant)
        {
            APInvoicePaymentRepository apInvoicePaymentRepository = new APInvoicePaymentRepository(repository.context);
            APInvoicePaymentQuery apInvoicePaymentQuery = new APInvoicePaymentQuery(apInvoicePaymentRepository);
            AccountingPaymentMethodRepository apPaymentMethodRep = new AccountingPaymentMethodRepository(repository.context);
            APPaymentPM payment = (from a in repository.context.APPayments.Include("LocalCurrency").Include("TransferStatus").Include("Status").Include("Branch")
                                   where a.Id == id && a.Tenant == tenant
                                   select new APPaymentPM()
                                   {
                                       CreateDate = a.CreateDate,
                                       Id = a.Id,
                                       InternalNotes = a.InternalNotes,
                                       IsClosed = a.IsClosed,
                                       LocalCurrencyId = a.LocalCurrencyId,
                                       LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                       PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                                       PaymentCurrencyId = a.PaymentCurrencyId,
                                       RegisterDate = a.RegisterDate,
                                       PaymentNo = a.PaymentNo,
                                       PrintDate = a.PrintDate,
                                       PrintNotes = a.PrintNotes,
                                       StatusCode = a.StatusCode,
                                       OpenAmount = a.OpenAmount,
                                       Tenant = a.Tenant,
                                       //SearchFields = a.SearchFields,
                                       Account = a.Account,
                                       AmountInLocalCurrency = a.AmountInLocalCurrency,
                                       AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                                       Bank = a.Bank,
                                       BankBranch = a.BankBranch,
                                       ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                                       CreatedByUserId = a.CreatedByUserId,
                                       PaymentCurrencyExchangeRateDate = a.PaymentCurrencyExchangeRateDate,
                                       AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                                       PrintedByUserId = a.PrintedByUserId,
                                       VendorAddressId = a.VendorAddressId,
                                       VendorId = a.VendorId,
                                       ValueDate = a.ValueDate,
                                       StatusName = a.Status.Name,
                                       CreditCardTypeId = a.CreditCardTypeId,
                                       BranchId = a.BranchId,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                       TransferError = a.TransferError,
                                       TransferStatusCode = a.TransferStatusCode,
                                       TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                       ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                       VendorPartnerTypeId = a.VendorCard == null ? "" : a.VendorCard.PartnerTypeId,
                                       TaxDeductionLocalAmount = a.TaxDeductionLocalAmount,
                                       TaxDeductionPercentage = a.TaxDeductionPercentage,
                                       ApprovedByUserId = a.ApprovedByUserId,
                                       ApprovedDateTime = a.ApprovedDateTime,
                                       BankAccountId = a.BankAccountId,
                                       FirstApproveDate = a.FirstApproveDate,
                                       BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                       VendorBankAccountNumber = a.VendorBankAccountNumber,
                                       VendorBankAddress = a.VendorBankAddress,
                                       VendorBankName = a.VendorBankName,
                                       VendorIBANNumber = a.VendorIBANNumber,
                                       VendorSwift = a.VendorSwift,
                                       ExternalPaymentAmount = a.ExternalPaymentAmount,
                                       ExternalPaymentDate = a.ExternalPaymentDate,
                                       ExternalPaymentNotes = a.ExternalPaymentNotes,
                                       ConnectedInvoicesNumbers = a.ConnectedInvoicesNumbers,
                                   }).FirstOrDefault();
            if (payment != null)
            {
                payment.PaymentInvoices = apInvoicePaymentQuery.GetAPPaymentInvoicePMsForPayment(payment.Id, tenant);

                AccountingPaymentMethod method = apPaymentMethodRep.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant);
                payment.PaymentMethodName = method != null ? method.Name : null;
                payment.PaymentMethodCode = method != null ? method.Code : null;

                Currency currency = CurrencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, payment.Tenant, true);
                payment.PaymentCurrencyCode = currency != null ? currency.Code : null;

                APPaymentPM securedPM = new APPaymentPM();
                SecuredMapping.GetMappedPM(payment, securedPM, "APPayment", tenant);
                MapCustomFieldValues(payment, securedPM);

                return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);
            }
            else return null;
        }

        public IQueryable<APPaymentPM> GetAPPaymentPMsByTenant(int tenant)
        {
            IQueryable<APPaymentPM> result = (from a in repository.context.APPayments.Include("LocalCurrency").Include("PaymentMethod").Include("TransferStatus").Include("Status").Include("Branch")
                                              where a.Tenant == tenant
                    select new APPaymentPM()
                    {
                        CreateDate = a.CreateDate,
                        Id = a.Id,
                        InternalNotes = a.InternalNotes,
                        IsClosed = a.IsClosed,
                        LocalCurrencyId = a.LocalCurrencyId,
                        LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                        PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                        PaymentCurrencyId = a.PaymentCurrencyId,
                        RegisterDate = a.RegisterDate,
                        PaymentNo = a.PaymentNo,
                        PrintDate = a.PrintDate,
                        PrintNotes = a.PrintNotes,
                        StatusCode = a.StatusCode,
                        OpenAmount = a.OpenAmount,
                        Tenant = a.Tenant,
                        //SearchFields = a.SearchFields,
                        Account = a.Account,
                        AmountInLocalCurrency = a.AmountInLocalCurrency,
                        AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                        Bank = a.Bank,
                        BankBranch = a.BankBranch,
                        ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                        CreatedByUserId = a.CreatedByUserId,
                        PaymentCurrencyExchangeRateDate = a.PaymentCurrencyExchangeRateDate,
                        AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                        PrintedByUserId = a.PrintedByUserId,
                        VendorAddressId = a.VendorAddressId,
                        VendorId = a.VendorId,
                        ValueDate = a.ValueDate,
                        BranchId = a.BranchId,
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserId = a.UpdatedByUserId,
                        //PaymentMethodName = a.PaymentMethod == null ? null : a.PaymentMethod.Name,
                        CreditCardTypeId = a.CreditCardTypeId,
                        ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                        TransferError = a.TransferError,
                        TransferStatusCode = a.TransferStatusCode,
                        TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                        ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                        TaxDeductionLocalAmount = a.TaxDeductionLocalAmount,
                        TaxDeductionPercentage = a.TaxDeductionPercentage,
                        ApprovedByUserId = a.ApprovedByUserId,
                        ApprovedDateTime = a.ApprovedDateTime,
                        BankAccountId = a.BankAccountId,
                        FirstApproveDate = a.FirstApproveDate,
                        BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                        VendorBankAccountNumber = a.VendorBankAccountNumber,
                        VendorBankAddress = a.VendorBankAddress,
                        VendorBankName = a.VendorBankName,
                        VendorIBANNumber = a.VendorIBANNumber,
                        VendorSwift = a.VendorSwift,
                        ExternalPaymentAmount = a.ExternalPaymentAmount,
                        ExternalPaymentDate = a.ExternalPaymentDate,
                        ExternalPaymentNotes = a.ExternalPaymentNotes,
                    });

            result = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APPaymentPM>(new QueryOperations(), result, tenant);
            return result;
        }

        public APPaymentPM GetSinglePaymentByPaymentNumber(string paymentNo, int tenant)
        {
            APInvoicePaymentRepository invoicePaymentRep = new APInvoicePaymentRepository(repository.context);
            AccountingPaymentMethodRepository apPaymentMethodRep = new AccountingPaymentMethodRepository(repository.context);
            APInvoicePaymentQuery apInvoicePaymentQuery = new APInvoicePaymentQuery(invoicePaymentRep);
            APPaymentPM payment = (from a in repository.context.APPayments.Include("LocalCurrency").Include("PaymentMethod").Include("TransferStatus").Include("Status").Include("Branch")
                                   where a.PaymentNo == paymentNo && a.Tenant == tenant
                                   select new APPaymentPM()
                                   {
                                       CreateDate = a.CreateDate,
                                       Id = a.Id,
                                       InternalNotes = a.InternalNotes,
                                       IsClosed = a.IsClosed,
                                       LocalCurrencyId = a.LocalCurrencyId,
                                       LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                       PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                                       PaymentCurrencyId = a.PaymentCurrencyId,
                                       RegisterDate = a.RegisterDate,
                                       PaymentNo = a.PaymentNo,
                                       PrintDate = a.PrintDate,
                                       PrintNotes = a.PrintNotes,
                                       StatusCode = a.StatusCode,
                                       OpenAmount = a.OpenAmount,
                                       Tenant = a.Tenant,
                                       //SearchFields = a.SearchFields,
                                       Account = a.Account,
                                       AmountInLocalCurrency = a.AmountInLocalCurrency,
                                       AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                                       Bank = a.Bank,
                                       BankBranch = a.BankBranch,
                                       ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                                       CreatedByUserId = a.CreatedByUserId,
                                       PaymentCurrencyExchangeRateDate = a.PaymentCurrencyExchangeRateDate,
                                       AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                                       PrintedByUserId = a.PrintedByUserId,
                                       VendorAddressId = a.VendorAddressId,
                                       VendorId = a.VendorId,
                                       ValueDate = a.ValueDate,
                                       BranchId = a.BranchId,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       CreditCardTypeId = a.CreditCardTypeId,
                                       ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                       TransferError = a.TransferError,
                                       TransferStatusCode = a.TransferStatusCode,
                                       TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                       ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                       TaxDeductionLocalAmount = a.TaxDeductionLocalAmount,
                                       TaxDeductionPercentage = a.TaxDeductionPercentage,
                                       ApprovedByUserId = a.ApprovedByUserId,
                                       ApprovedDateTime = a.ApprovedDateTime,
                                       BankAccountId = a.BankAccountId,
                                       FirstApproveDate = a.FirstApproveDate,
                                       BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                       VendorBankAccountNumber = a.VendorBankAccountNumber,
                                       VendorBankAddress = a.VendorBankAddress,
                                       VendorBankName = a.VendorBankName,
                                       VendorIBANNumber = a.VendorIBANNumber,
                                       VendorSwift = a.VendorSwift,
                                       ExternalPaymentAmount = a.ExternalPaymentAmount,
                                       ExternalPaymentDate = a.ExternalPaymentDate,
                                       ExternalPaymentNotes = a.ExternalPaymentNotes,
                                       ConnectedInvoicesNumbers = a.ConnectedInvoicesNumbers,
                                   }).FirstOrDefault();

            payment.PaymentInvoices = apInvoicePaymentQuery.GetAPPaymentInvoicePMsForPayment(payment.Id, tenant).ToList();

            AccountingPaymentMethod method = apPaymentMethodRep.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant);
            payment.PaymentMethodName = method != null ? method.Name : null;
            payment.PaymentMethodCode = method != null ? method.Code : null;

            Currency currency = CurrencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, payment.Tenant, true);
            payment.PaymentCurrencyCode = currency != null ? currency.Code : null;
            return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), payment, tenant);;
        }

        public IQueryable<APPaymentList> GetIQueryableEntityList(IQueryable<APPayment> iQueryable)
        {
            IQueryable<APPaymentList> result = from a in iQueryable.Include("LocalCurrency").Include("AccountingPaymentMethod").Include("PaymentCurrency").Include("VendorCard").Include("CreatedByUser.Contact").Include("Status").Include("TransferStatus").Include("Branch")
                                               select new APPaymentList()
                                               {
                                                   CreateDate = a.CreateDate,
                                                   Id = a.Id,
                                                   InternalNotes = a.InternalNotes,
                                                   IsClosed = a.IsClosed,
                                                   LocalCurrencyId = a.LocalCurrencyId,
                                                   LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                                   PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                                                   PaymentCurrencyId = a.PaymentCurrencyId,
                                                   RegisterDate = a.RegisterDate,
                                                   PaymentNo = a.PaymentNo,
                                                   PrintDate = a.PrintDate,
                                                   PrintNotes = a.PrintNotes,
                                                   StatusCode = a.StatusCode,
                                                   OpenAmount = a.OpenAmount,
                                                   Tenant = a.Tenant,
                                                   SearchFields = a.SearchFields,
                                                   Account = a.Account,
                                                   AmountInLocalCurrency = a.AmountInLocalCurrency,
                                                   AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                                                   Bank = a.Bank,
                                                   BankBranch = a.BankBranch,
                                                   ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                                                   CreatedByUserId = a.CreatedByUserId,
                                                   PaymentCurrencyExchangeRateDate = a.PaymentCurrencyExchangeRateDate,
                                                   AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                                                   PrintedByUserId = a.PrintedByUserId,
                                                   VendorAddressId = a.VendorAddressId,
                                                   VendorId = a.VendorId,
                                                   ValueDate = a.ValueDate,
                                                   StatusName = a.Status.Name,
                                                   PaymentMethodName = a.AccountingPaymentMethod == null ? null : a.AccountingPaymentMethod.Name,
                                                   PaymentMethodCode = a.AccountingPaymentMethod == null ? null : a.AccountingPaymentMethod.Code,
                                                   PaymentCurrencyCode = a.PaymentCurrency == null ? "" : a.PaymentCurrency.Code,
                                                   VendorName = a.VendorCard == null ? "" : a.VendorCard.EnglishName,
                                                   VendorLocalName = a.VendorCard == null ? "" : a.VendorCard.LocalName,
                                                   VendorCode = a.VendorCard == null ? "" : a.VendorCard.Code,
                                                   CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                                                   CreditCardTypeId = a.CreditCardTypeId,
                                                   AmountInProfitCurrency = a.AmountInProfitCurrency,
                                                   BranchId = a.BranchId,
                                                   ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                                   ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                                   TransferError = a.TransferError,
                                                   TransferStatusCode = a.TransferStatusCode,
                                                   TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                                   ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                                   TaxDeductionLocalAmount = a.TaxDeductionLocalAmount,
                                                   TaxDeductionPercentage = a.TaxDeductionPercentage,
                                                   ApprovedByUserId = a.ApprovedByUserId,
                                                   ApprovedDateTime = a.ApprovedDateTime,
                                                   BankAccountId = a.BankAccountId,
                                                   FirstApproveDate = a.FirstApproveDate,
                                                   BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                                   VendorBankAccountNumber = a.VendorBankAccountNumber,
                                                   VendorBankAddress = a.VendorBankAddress,
                                                   VendorBankName = a.VendorBankName,
                                                   VendorIBANNumber = a.VendorIBANNumber,
                                                   VendorSwift = a.VendorSwift,
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
                                                   ExternalPaymentAmount = a.ExternalPaymentAmount,
                                                   ExternalPaymentDate = a.ExternalPaymentDate,
                                                   ExternalPaymentNotes = a.ExternalPaymentNotes,
                                                   ConnectedInvoicesNumbers = a.ConnectedInvoicesNumbers,
                                               };
            return result;
        }

        public IQueryable<APPaymentList> GetOpenedAPPayments(int tenant)
        {
            var query = from a in repository.context.APPayments.Include("LocalCurrency").Include("PaymentMethod").Include("PaymentCurrency").Include("VendorCard").Include("CreatedByUser.Contact").Include("Status").Include("TransferStatus").Include("Branch")
                        where a.Tenant == tenant && a.StatusCode != "DR" && a.StatusCode != "VD" && a.StatusCode != "LL" && !a.IsClosed
                        select new APPaymentList()
                        {
                            CreateDate = a.CreateDate,
                            Id = a.Id,
                            InternalNotes = a.InternalNotes,
                            IsClosed = a.IsClosed,
                            LocalCurrencyId = a.LocalCurrencyId,
                            LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                            PaymentCurrencyExchangeRate = a.PaymentCurrencyExchangeRate,
                            PaymentCurrencyId = a.PaymentCurrencyId,
                            RegisterDate = a.RegisterDate,
                            PaymentNo = a.PaymentNo,
                            PrintDate = a.PrintDate,
                            PrintNotes = a.PrintNotes,
                            StatusCode = a.StatusCode,
                            OpenAmount = a.OpenAmount,
                            Tenant = a.Tenant,
                            SearchFields = a.SearchFields,
                            Account = a.Account,
                            AmountInLocalCurrency = a.AmountInLocalCurrency,
                            AmountInPaymentCurrency = a.AmountInPaymentCurrency,
                            Bank = a.Bank,
                            BankBranch = a.BankBranch,
                            ChequeOrPaymentRef = a.ChequeOrPaymentRef,
                            CreatedByUserId = a.CreatedByUserId,
                            PaymentCurrencyExchangeRateDate = a.PaymentCurrencyExchangeRateDate,
                            AccountingPaymentMethodId = a.AccountingPaymentMethodId,
                            PrintedByUserId = a.PrintedByUserId,
                            VendorAddressId = a.VendorAddressId,
                            VendorId = a.VendorId,
                            ValueDate = a.ValueDate,
                            StatusName = a.Status.Name,
                            CreditCardTypeId = a.CreditCardTypeId,
                            BranchId = a.BranchId,
                            UpdateDate = a.UpdateDate,
                            UpdatedByUserId = a.UpdatedByUserId,
                            PaymentCurrencyCode = a.PaymentCurrency == null ? null : a.PaymentCurrency.Code,
                            ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                            AmountInProfitCurrency = a.AmountInProfitCurrency,
                            PaymentMethodName = a.AccountingPaymentMethod == null ? null : a.AccountingPaymentMethod.Name,
                            PaymentMethodCode = a.AccountingPaymentMethod == null ? null : a.AccountingPaymentMethod.Code,
                            VendorName = a.VendorCard == null ? "" : a.VendorCard.EnglishName,
                            VendorLocalName = a.VendorCard == null ? "" : a.VendorCard.LocalName,
                            VendorCode = a.VendorCard == null ? "" : a.VendorCard.Code,
                            ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                            TransferError=a.TransferError,
                            TransferStatusCode=a.TransferStatusCode,
                            TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                            ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                            TaxDeductionLocalAmount = a.TaxDeductionLocalAmount,
                            TaxDeductionPercentage = a.TaxDeductionPercentage,
                            ApprovedByUserId = a.ApprovedByUserId,
                            ApprovedDateTime = a.ApprovedDateTime,
                            BankAccountId = a.BankAccountId,
                            FirstApproveDate = a.FirstApproveDate,
                            BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                            VendorBankAccountNumber = a.VendorBankAccountNumber,
                            VendorBankAddress = a.VendorBankAddress,
                            VendorBankName = a.VendorBankName,
                            VendorIBANNumber = a.VendorIBANNumber,
                            VendorSwift = a.VendorSwift,
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
                            ExternalPaymentAmount = a.ExternalPaymentAmount,
                            ExternalPaymentDate = a.ExternalPaymentDate,
                            ExternalPaymentNotes = a.ExternalPaymentNotes,
                            ConnectedInvoicesNumbers = a.ConnectedInvoicesNumbers,
                        };

            return query;
        }      
    }
}