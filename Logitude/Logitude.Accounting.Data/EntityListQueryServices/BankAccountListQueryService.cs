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
using System.Runtime.InteropServices;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.CommonDataModel;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class BankAccountListQueryService
    {
        private List<BankAccountList> GetIqueryableList(IQueryable<BankAccount> iQueryable)
        {
            var resPartOne = (from a in iQueryable.Include("GLAccount").Include("DeferredGLAccount").Include("TransferGLAcccount")
                                                 .Include("BankCode")
                                                 .Include("Currency")
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
                                                                  CurrencyId = a.CurrencyId


                                                              }).ToList();

            if (resPartOne != null && resPartOne.Count > 0)
            {
                var idsPartOne = resPartOne.Select(v => v.Id).ToList();
                var tenantPartOne = resPartOne.First().Tenant;

                var resPartTwo = (from a in context.TotalOpenTransInBankViews
                                  where a.Tenant == tenantPartOne && idsPartOne.Contains(a.Id)
                                  select a).ToList();
                foreach (var r in resPartOne)
                {


                    var match = resPartTwo.FirstOrDefault(x => x.Id == r.Id);
                    if (match != null)
                    {
                        r.TotalOpenExternalTransactions = match.TotalLedgerTransactionsCount.ToString();
                        r.TotalOpenPagesLines = match.TotalReconcileExternalPageLinesCount.ToString();
                    }
                }
            }
            return resPartOne;
        }

        private IQueryable<BankAccount> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BankAccount> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<BankAccount> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BankAccount> iQueryable, int tenant)
        {
            return iQueryable;
        }


        public BankAccountList GetUniqueAccount(string accountNumber, string branchNumber, string bankId, string currencyId, int tenant)
        {
            IQueryable<BankAccount> accountQuery = (from a in context.BankAccounts
                                                    where a.Tenant == tenant
                                                    && a.AccountNumber == accountNumber
                                                    && a.BranchNumber == branchNumber
                                                    && a.BankId == bankId
                                                    && a.CurrencyId == currencyId
                                                    select a);

            List<BankAccountList> accountList = this.GetIqueryableList(accountQuery);
            BankAccountList rvList = accountList.FirstOrDefault();
            return rvList;
        }

        public BankAccountList GetByGLAccount(string accountNumber)
        {
            IQueryable<BankAccount> accountQuery = (from a in context.BankAccounts
                                                    where a.GLAccountId == accountNumber
                                                    select a);

            List<BankAccountList> accountList =  this.GetIqueryableList(accountQuery);
            BankAccountList rvList = accountList.FirstOrDefault();
            return rvList;
        }

        public BankAccountList GetByDeferedGLAccount(string accountNumber)
        {
            IQueryable<BankAccount> accountQuery = (from a in context.BankAccounts
                                                    where a.DeferredGLAccountId == accountNumber
                                                    select a);

            List<BankAccountList> accountList = this.GetIqueryableList(accountQuery);
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
