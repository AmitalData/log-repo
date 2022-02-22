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
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class BankDepositListQueryService
    {
        private IQueryable<BankDepositList> GetIqueryableList(IQueryable<BankDeposit> iQueryable)
        {
                                                 //join jr in context.Journals on a.Id equals jr.AccountingEntityId into r
                                                 //from x in r.GroupBy(d => d.AccountingEntityId).Select(d => d.FirstOrDefault())

            IQueryable<BankDepositList> query = (from a in iQueryable.Include("CashBook").Include("BankAccount")
                                                 select new BankDepositList()
                                                 {

                                                     Id = a.Id,

                                                     Tenant = a.Tenant,

                                                     CreateDate = a.CreateDate,

                                                     CreatedByUserId = a.CreatedByUserId,

                                                     UpdateDate = a.UpdateDate,

                                                     UpdatedByUserId = a.UpdatedByUserId,

                                                     SearchFields = a.SearchFields,

                                                     AccountingDate = a.AccountingDate,

                                                     CashBookId = a.CashBook != null ? a.CashBook.Id : null,

                                                     DepositBankAccountId = a.DepositBankAccountId,

                                                     DepositCurrencyId = a.Currency.Code,

                                                     DepositDate = a.DepositDate,

                                                     DepositNumber = a.DepositNumber,

                                                     ForeignAmount = a.ForeignAmount,

                                                     LocalDepositAmount = a.LocalDepositAmount,

                                                     DepositCurrencyCode = a.Currency.Code,

                                                     CashBookName = a.CashBook.EnglishName,

                                                     IsCashDeposit = a.CashBook.CashBookTypeCode == "1",

                                                     CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,

                                                     BankAccountNumber = a.BankAccount.AccountNumber,
                                                     IsCanceled = a.IsCanceled,

                                                     //JournalNumber = x.JournalNumber,
                                                 });
            return query;
        }


        private IQueryable<BankDeposit> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BankDeposit> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<BankDeposit> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BankDeposit> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<BankDepositList> GetLastActivityBankDeposits(int tenant, string userId, string objectTableId)
        {
            List<BankDepositList> entityList = new List<BankDepositList>();
            if (string.IsNullOrEmpty(userId))
            {
                return entityList;
            }

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            BankDepositRepository repository = new BankDepositRepository(tenant);
            IQueryable<BankDeposit> entities = entities = repository.GetAll(tenant);

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                BankDeposit a = (from d in entities.Include("CashBook").Include("Currency")
                                 where d.Id == lastActivity.EntityId
                                 select d).FirstOrDefault();

                if (a != null)
                {
                    BankDepositList list = new BankDepositList()
                    {
                        Id = a.Id,

                        Tenant = a.Tenant,

                        CreateDate = a.CreateDate,

                        CreatedByUserId = a.CreatedByUserId,

                        UpdateDate = a.UpdateDate,

                        UpdatedByUserId = a.UpdatedByUserId,

                        SearchFields = a.SearchFields,

                        AccountingDate = a.AccountingDate,

                        CashBookId = a.CashBook != null ? a.CashBook.Id : null,

                        DepositBankAccountId = a.DepositBankAccountId,

                        DepositCurrencyId = a.Currency.Code,

                        DepositDate = a.DepositDate,

                        DepositNumber = a.DepositNumber,

                        ForeignAmount = a.ForeignAmount,

                        LocalDepositAmount = a.LocalDepositAmount,

                        DepositCurrencyCode = a.Currency.Code,

                        CashBookName = a.CashBook.EnglishName,

                        IsCashDeposit = a.CashBook.CashBookTypeCode == "1",

                        LastActivityDate = lastActivity.ActivityDate,

                        LastActivityTypeName = lastActivity.ActivityType.Name,

                        LastActivityByUserName = lastActivity.User.Contact.EnglishName,
                    };


                    entityList.Add(list);
                }
            }

            //if (entityList.Count > 0)
            //{
            //    entityList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<BankDepositList>(new QueryOperations(), entityList.AsQueryable<BankDepositList>(), tenant).ToList();
            //    entityList = ProductPermitionsFilter.AddUserProductRestrictionFilters<BankDepositList>(new QueryOperations(), entityList.AsQueryable<BankDepositList>(), tenant).ToList();
            //}

            return entityList;
        }

        public BankDepositList GetLastBankDepositByIdList(int tenant, List<string> depositIdList)
        {
            BankDepositRepository repository = new BankDepositRepository(tenant);
            IQueryable<BankDeposit> entities = entities = repository.GetAll(tenant);

            BankDeposit a = (from d in entities.Include("CashBook").Include("Currency")
                             where depositIdList.Contains(d.Id)
                             select d).OrderByDescending(o => o.DepositDate).FirstOrDefault();
            if (a != null)
            {
                BankDepositList list = new BankDepositList()
                {
                    Id = a.Id,

                    Tenant = a.Tenant,

                    CreateDate = a.CreateDate,

                    CreatedByUserId = a.CreatedByUserId,

                    UpdateDate = a.UpdateDate,

                    UpdatedByUserId = a.UpdatedByUserId,

                    SearchFields = a.SearchFields,

                    AccountingDate = a.AccountingDate,

                    CashBookId = a.CashBook != null ? a.CashBook.Id : null,

                    DepositBankAccountId = a.DepositBankAccountId,

                    DepositCurrencyId = a.Currency.Code,

                    DepositDate = a.DepositDate,

                    DepositNumber = a.DepositNumber,

                    ForeignAmount = a.ForeignAmount,

                    LocalDepositAmount = a.LocalDepositAmount,

                    DepositCurrencyCode = a.Currency.Code,

                    CashBookName = a.CashBook.EnglishName,

                    IsCashDeposit = a.CashBook.CashBookTypeCode == "1",
                };

                return list;
            }
            else
            {
                return null;
            }


        }

    }
}
	