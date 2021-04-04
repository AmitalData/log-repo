using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class BankAccountListQueryService
    {
	    private IQueryable<BankAccountList> GetIqueryableList(IQueryable<BankAccount> iQueryable)
        {
            IQueryable<BankAccountList> query = (from a in iQueryable.Include("GLAccount").Include("DeferredGLAccount").Include("TransferGLAcccount")
                                                 .Include("BankCode")
                                                 .Include("Currency")
                                                 //.DefaultIfEmpty()
                                                 select new BankAccountList()
                                                 {

                                                     Id = a.Id,

                                                     Tenant = a.Tenant,

                                                     EnglishName = a.EnglishName,

                                                     LocalName = a.LocalName,

                                                     CreateDate = a.CreateDate,

                                                     CreatedByUserId = a.CreatedByUserId,

                                                     UpdateDate = a.UpdateDate,

                                                     UpdatedByUserId = a.UpdatedByUserId,

                                                     SearchFields = a.SearchFields,

                                                     AccountNumber = a.AccountNumber,

                                                     BranchAddress = a.BranchAddress,

                                                     BranchNumber = a.BranchNumber,

                                                     DeferredGLAccountId = a.DeferredGLAccountId,

                                                     GLAccountId = a.GLAccountId,

                                                     IBAN = a.IBAN,

                                                     Inactive = a.Inactive,

                                                     SwiftCode = a.SwiftCode,

                                                     BankCode = a.BankCode.Code,

                                                     GLAccountNumber = a.GLAccount.DisplayNumber,

                                                     DeferedGLAccountNumber = a.DeferredGLAccount == null ? null : a.DeferredGLAccount.DisplayNumber,
                                                     DeferedGLAccountEnglishName = a.DeferredGLAccount == null ? null : a.DeferredGLAccount.EnglishName,
                                                     DeferedGLAccountLocalName = a.DeferredGLAccount == null ? null : a.DeferredGLAccount.LocalName,

                                                     GLAccountCurrencyId = a.GLAccount == null ? null : a.GLAccount.CurrencyId,

                                                     ChequeCounter = a.ChequeCounter,
                                                     TransferGLAcccountId = a.TransferGLAcccountId,
                                                     TransferGLAcccountNumber = a.TransferGLAcccount == null ? null : a.TransferGLAcccount.DisplayNumber,
                                                     TransferGLAcccountEnglishName = a.TransferGLAcccount == null ? null : a.TransferGLAcccount.EnglishName,
                                                     TransferGLAcccountLocalName = a.TransferGLAcccount == null ? null : a.TransferGLAcccount.LocalName,

                                                     CurrencyCode = a.Currency == null ? null : a.Currency.Code,
                                                     CurrencySign = a.Currency == null ? null : a.Currency.Sign,
                                                     CurrencyId = a.CurrencyId,

                                                 });
            return query;
		}

        private IQueryable<BankAccount> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BankAccount> iQueryable, int tenant)
        {
            return iQueryable;
		}
		private IQueryable<BankAccount> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<BankAccount> iQueryable, int tenant)
        {
			return iQueryable;
		}


        public BankAccountList GetUniqueAccount(string accountNumber, string branchNumber, string bankId, int tenant)
        {
            IQueryable<BankAccount> accountQuery = (from a in context.BankAccounts
                                                    where a.Tenant == tenant 
                                                    && a.AccountNumber == accountNumber 
                                                    && a.BranchNumber == branchNumber
                                                    && a.BankId == bankId
                                                  select a);

            IQueryable<BankAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List<BankAccountList> accountList = accountListQuery.ToList();
            BankAccountList rvList = accountList.FirstOrDefault();
            return rvList;
        }

        public BankAccountList GetByGLAccount(string accountNumber)
        {
            IQueryable<BankAccount> accountQuery = (from a in context.BankAccounts
                                                    where a.GLAccountId == accountNumber
                                                    select a);

            IQueryable<BankAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List<BankAccountList> accountList = accountListQuery.ToList();
            BankAccountList rvList = accountList.FirstOrDefault();
            return rvList;
        }

        public BankAccountList GetByDeferedGLAccount(string accountNumber)
        {
            IQueryable<BankAccount> accountQuery = (from a in context.BankAccounts
                                                    where a.DeferredGLAccountId == accountNumber
                                                    select a);

            IQueryable<BankAccountList> accountListQuery = this.GetIqueryableList(accountQuery);
            List<BankAccountList> accountList = accountListQuery.ToList();
            BankAccountList rvList = accountList.FirstOrDefault();
            return rvList;
        }

        private Contact GetLoggedContact(int tenant)
        {
            //email
            string email = "";
            if (HttpContext.Current != null)
                email = HttpContext.Current.User.Identity.Name;
            else
                email = "system@tenant" + tenant.ToString() + ".com";

            //contact
            ContactRepository contactQuery = new ContactRepository(tenant);
            Contact contactPM = contactQuery.GetSingleContactByEmailAndTenant(email, tenant);
            return contactPM;
        }


    }


}
	