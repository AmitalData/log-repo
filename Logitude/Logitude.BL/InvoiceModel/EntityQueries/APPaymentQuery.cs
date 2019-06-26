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

            APPaymentPM payment = (from a in repository.context.APPayments.Include("LocalCurrency").Include("TransferStatus").Include("Status")
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
                                       BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       VendorName = a.VendorCard == null ? "" : (loggedContact.DontShowLocalLabels  ? a.VendorCard.EnglishName   : a.VendorCard.LocalName  ),
                                       ExternalAccountingEntityId=a.ExternalAccountingEntityId,
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
                                       AutomaticPaymentCheque = a.AutomaticPaymentCheque
                                     
                                   }).FirstOrDefault();

            payment.PaymentInvoices = apInvoicePaymentQuery.GetAPPaymentInvoicePMsForPayment(payment.Id, tenant);

            AccountingPaymentMethod method = apPaymentMethodRep.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant);
            payment.PaymentMethodName = method != null ? method.Name : null;
            payment.PaymentMethodCode = method != null ? method.Code : null;

            Currency currency = CurrencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, payment.Tenant, true);
            payment.PaymentCurrencyCode = currency != null ? currency.Code : null;

            APPaymentPM securedPM = new APPaymentPM();
            SecuredMapping.GetMappedPM(payment, securedPM, "APPayment", tenant);

            return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);
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
            APPaymentPM payment = (from a in repository.context.APPayments.Include("LocalCurrency").Include("TransferStatus").Include("Status")
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
                                       BranchName = a.Branch == null ? null : a.Branch.EnglishName,
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
                                   }).FirstOrDefault();

            payment.PaymentInvoices = apInvoicePaymentQuery.GetAPPaymentInvoicePMsForPayment(payment.Id, tenant);

            AccountingPaymentMethod method = apPaymentMethodRep.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, tenant);
            payment.PaymentMethodName = method != null ? method.Name : null;
            payment.PaymentMethodCode = method != null ? method.Code : null;

            Currency currency = CurrencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, payment.Tenant, true);
            payment.PaymentCurrencyCode = currency != null ? currency.Code : null;

            APPaymentPM securedPM = new APPaymentPM();
            SecuredMapping.GetMappedPM(payment, securedPM, "APPayment", tenant);
            
            return BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);
        }

        public IQueryable<APPaymentPM> GetAPPaymentPMsByTenant(int tenant)
        {
            IQueryable<APPaymentPM> result = (from a in repository.context.APPayments.Include("LocalCurrency").Include("PaymentMethod").Include("TransferStatus").Include("Status")
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
                        BranchName = a.Branch == null ? null : a.Branch.EnglishName,
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
                    });

            result = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APPaymentPM>(new QueryOperations(), result, tenant);
            return result;
        }

        public APPaymentPM GetSinglePaymentByPaymentNumber(string paymentNo, int tenant)
        {
            APInvoicePaymentRepository invoicePaymentRep = new APInvoicePaymentRepository(repository.context);
            AccountingPaymentMethodRepository apPaymentMethodRep = new AccountingPaymentMethodRepository(repository.context);
            APInvoicePaymentQuery apInvoicePaymentQuery = new APInvoicePaymentQuery(invoicePaymentRep);
            APPaymentPM payment = (from a in repository.context.APPayments.Include("LocalCurrency").Include("PaymentMethod").Include("TransferStatus").Include("Status")
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
                                       BranchName = a.Branch == null ? null : a.Branch.EnglishName,
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
                                                   CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                                                   CreditCardTypeId = a.CreditCardTypeId,
                                                   AmountInProfitCurrency = a.AmountInProfitCurrency,
                                                   BranchId = a.BranchId,
                                                   BranchName = a.Branch == null ? null : a.Branch.EnglishName,
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
                                               };
            return result;
        }

        public IQueryable<APPaymentList> GetOpenedAPPayments(int tenant)
        {
            var query = from a in repository.context.APPayments.Include("LocalCurrency").Include("PaymentMethod").Include("PaymentCurrency").Include("VendorCard").Include("CreatedByUser.Contact").Include("Status").Include("TransferStatus")
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
                            BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                            UpdateDate = a.UpdateDate,
                            UpdatedByUserId = a.UpdatedByUserId,
                            PaymentCurrencyCode = a.PaymentCurrency == null ? null : a.PaymentCurrency.Code,
                            ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                            AmountInProfitCurrency = a.AmountInProfitCurrency,
                            PaymentMethodName = a.PaymentMethod == null ? null : a.PaymentMethod.Name,
                            PaymentMethodCode = a.PaymentMethod == null ? null : a.PaymentMethod.Code,
                            VendorName = a.VendorCard == null ? "" : a.VendorCard.EnglishName,
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
                        };

            return query;
        }

      
    }
}