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
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class JournalListQueryService
    {
	    private IQueryable<JournalList> GetIqueryableList(IQueryable<Journal> iQueryable)
        {

          
            IQueryable<JournalList> query = (from a in iQueryable
                                             select new JournalList()
                                                       {
                                                           AccountingDate = a.AccountingDate,
                                                           AccountingEntityCode = a.AccountingEntityCode,
                                                        //   AccountingEntityId = a.AccountingEntityId,
                                                           AccountingEntityName = a.AccountingEntity != null ? a.AccountingEntity.LocalName : null,
                                                           AccountingEntityReference = a.AccountingEntityReference, // CurrencyCode = a.IsMultiCurrency == true ? multi : a.Currency != null ? a.Currency.Code : null,
                                                           ApproveDate = a.ApproveDate,
                                                          // ApprovedByUserId = a.ApprovedByUserId,
                                                           ApprovedByUserName = a.ApprovedByUser != null ? a.ApprovedByUser.Contact.LocalName : null,

                                                           CreateDate = a.CreateDate,
                                                         //  CreatedByUserId = a.CreatedByUserId,
                                                           CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,
                                                           ExternalNo = a.ExternalNo,                                                         
                                                           ExternalSystem = a.ExternalSystem,
                                                           Id = a.Id,
                                                           JournalNumber = a.JournalNumber,
                                                          // OriginalJournalId = a.OriginalJournalId,
                                                           OriginalJournalName = a.OriginalJournal != null ? a.OriginalJournal.JournalNumber : null,
                                                           SearchFields = a.SearchFields,
                                                          // StatusCode = a.StatusCode,
                                                           StatusName = a.JournalStatusType != null ? a.JournalStatusType.EnglishName : null,
                                                           StatusLocalName = a.JournalStatusType != null ? a.JournalStatusType.LocalName : null,
                                                           Tenant = a.Tenant,
                                                           TypeCode = a.TypeCode,
                                                           TypeName = a.JournalType !=null ? a.JournalType.EnglishName : null,
                                                           TypeLocalName = a.JournalType !=null ? a.JournalType.LocalName : null,
                                                           
                                                           UpdateDate = a.UpdateDate,
                                                         //  UpdatedByUserId = a.UpdatedByUserId,
                                                           UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,

                                                           VoidDate = a.VoidDate,
                                                           VoidedByUserName = a.VoidedByUser != null ? a.VoidedByUser.Contact.EnglishName : null,
                                                          
                                                          // VoidedByUserId = a.VoidedByUserId,

                                                           IsVoided = a.IsVoided,
                                                           IsLedgerCreated = a.IsLedgerCreated,
                                                           
                                                        
                                                          
                                                           
                                                         
                                                       

                                                         
                                                       });
            return query;
		}

		private IQueryable<Journal> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Journal> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<Journal> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Journal> iQueryable,int tenant)
        {
			return iQueryable;
		}



        public JournalList GetJournalByJournalNumber(string number, int tenant)
        {

            IQueryable<Journal> journalQuery = (from a in context.Journals
                                                where a.Tenant == tenant && a.JournalNumber == number
                                                                                  select a);

            IQueryable<JournalList> journalListQuery = GetIqueryableList(journalQuery);
            var myList = journalListQuery.FirstOrDefault();
            return myList;
        }


        public List<JournalList> GetLastActivityJournals(int tenant, string userId, string objectTableId)
        {
            List<JournalList> entityList = new List<JournalList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            JournalRepository repository = new JournalRepository(tenant);
            IQueryable<Journal> entities = entities = repository.GetAll(tenant);

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                Journal a = (from d in entities
                             .Include("JournalType")
                             .Include("JournalStatusType")
                             .Include("UpdatedByUser")
                                .Include("UpdatedByUser.Contact")
                             .Include("CreatedByUser")
                                .Include("CreatedByUser.Contact")
                             .Include("ApprovedByUser")
                                .Include("ApprovedByUser.Contact")
                             .Include("VoidedByUser")
                                .Include("VoidedByUser.Contact")
                             where d.Id == lastActivity.EntityId
                             select d).FirstOrDefault();

                if (a != null)
                {
                    JournalList list = new JournalList()
                    {
                        AccountingDate = a.AccountingDate,
                        AccountingEntityCode = a.AccountingEntityCode,
                        AccountingEntityName = a.AccountingEntity != null ? a.AccountingEntity.LocalName : null,
                        AccountingEntityReference = a.AccountingEntityReference, // CurrencyCode = a.IsMultiCurrency == true ? multi : a.Currency != null ? a.Currency.Code : null,
                        ApproveDate = a.ApproveDate,
                        ApprovedByUserName = a.ApprovedByUser != null ? a.ApprovedByUser.Contact.LocalName : null,
                        CreateDate = a.CreateDate,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,
                        ExternalNo = a.ExternalNo,
                        ExternalSystem = a.ExternalSystem,
                        Id = a.Id,
                        JournalNumber = a.JournalNumber,
                        OriginalJournalName = a.OriginalJournal != null ? a.OriginalJournal.JournalNumber : null,
                        SearchFields = a.SearchFields,
                        StatusName = a.JournalStatusType != null ? a.JournalStatusType.EnglishName : null,
                        StatusLocalName = a.JournalStatusType != null ? a.JournalStatusType.LocalName : null,
                        Tenant = a.Tenant,
                        TypeCode = a.TypeCode,
                        TypeName = a.JournalType != null ? a.JournalType.EnglishName : null,
                        TypeLocalName = a.JournalType != null ? a.JournalType.LocalName : null,
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                        VoidDate = a.VoidDate,
                        VoidedByUserName = a.VoidedByUser != null ? a.VoidedByUser.Contact.EnglishName : null,
                        IsVoided = a.IsVoided,
                        IsLedgerCreated = a.IsLedgerCreated,
                        LastActivityDate = lastActivity.ActivityDate,
                        LastActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.EnglishName,

                    };


                    entityList.Add(list);
                }
            }

            //if (entityList.Count > 0)
            //{
            //    entityList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<JournalList>(new QueryOperations(), entityList.AsQueryable<JournalList>(), tenant).ToList();
            //    entityList = ProductPermitionsFilter.AddUserProductRestrictionFilters<JournalList>(new QueryOperations(), entityList.AsQueryable<JournalList>(), tenant).ToList();
            //}

            return entityList;
        }

        public IQueryable<JournalList> GetJournalsByAccountingEntityId(string entityId, int tenant)
        {
            IQueryable<Journal> journalQuery = (from a in context.Journals
                                                where a.Tenant == tenant && a.AccountingEntityId == entityId
                                                select a);

            IQueryable<JournalList> journalListQuery = GetIqueryableList(journalQuery);
            var myList = journalListQuery;
            return myList;
        }





        public List<JournalLT_GroupItem> GetJournals_InterestTransactionsCheck(ref InterestTransactionsGetNextGroupArgs getNextGroupArgs)
        {
            getNextGroupArgs.ActualDifference = 0m;
            string old_journalId_saved = getNextGroupArgs.OldJournalId;
            string specificJournalId = getNextGroupArgs.SpecificJournalId;
            int myMAX = getNextGroupArgs.MaxPageSize; //getNextGroupArgs.LT_LinesMaximum;
            InterestTransactionsGetNextGroupArgs args = getNextGroupArgs;
            LedgerTransactionRepository repo = new LedgerTransactionRepository(this.context);
            JournalRepository journalRepository = new JournalRepository(this.context);
            JournalListQueryService journalListQueryService = new JournalListQueryService(context);
            IQueryable<IGrouping<string, LedgerTransaction>> group_query;

            if (!String.IsNullOrEmpty(specificJournalId))
                group_query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                      && String.Compare(rec.JournalId, specificJournalId) == 0).
                      GroupBy(item => item.JournalId).OrderBy(gr => gr.Key).Take(myMAX);

            else if (getNextGroupArgs.MoveOn)
            {
                group_query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                      && String.Compare(rec.JournalId, args.OldJournalId) > 0).
                      GroupBy(item => item.JournalId).OrderBy(gr => gr.Key).Take(myMAX);


            }
            else if (getNextGroupArgs.RunAgain)
            {
                group_query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                    && String.Compare(rec.JournalId, args.OldJournalId) >= 0).
                      GroupBy(item => item.JournalId).OrderBy(gr => gr.Key).Take(myMAX);



            }
            else
            {
                group_query = repo.GetAll(getNextGroupArgs.Tenant).Where(rec => rec.Tenant == args.Tenant && rec.AccountId == args.GLAccountId
                      && String.Compare(rec.JournalId, args.OldJournalId) > 0).
                      GroupBy(item => item.JournalId).OrderBy(gr => gr.Key).Take(myMAX);



            }


            List<IGrouping<string, LedgerTransaction>> group_list = group_query.ToList();
            List<string> q;
            if (group_query != null)
                q = group_query.Select(g => g.Key).ToList();
            else
                q = new List<string>();
            string long_text = "";
            int ctr = 1;
            q.ForEach(item => long_text += "#" + ctr++ + "," + item + "\n");
            List<JournalLT_GroupItem> result = new List<JournalLT_GroupItem>();

            if (group_list == null || group_list.Count == 0 || q.Count == 0)
            {
                //Nothing retrieved. Stop here!
                getNextGroupArgs.Stop = true;
            }
            else
            {
                getNextGroupArgs.OldJournalId = q.Last();
                foreach (IGrouping<String, LedgerTransaction> group in group_list)
                {
                    string jID = group.Key;
                    List<LedgerTransaction> other_LTs = new List<LedgerTransaction>();
                    Journal journal = journalRepository.GetSingle(jID, getNextGroupArgs.Tenant);
                    if (journal != null && !String.IsNullOrEmpty(journal.Id) && (String.IsNullOrEmpty(journal.ExternalSystem) || journal.ExternalSystem != "AMITAL"))
                    {
                        // now search more journals with same accounting entity
                        List<Journal> other_journals = journalRepository.GetByJournalsAccountingEntityIdAndCode(journal.AccountingEntityId, journal.AccountingEntityCode, getNextGroupArgs.Tenant)
                            .Where(j => j.Id != journal.Id && (j.StatusCode == "2" || j.StatusCode == "3")).ToList();

                        other_journals.ForEach(jr =>
                        {
                            List<LedgerTransaction> other_LTs_per_j = repo.GetAll(args.Tenant).Where(rec => rec.Tenant == args.Tenant
                                                    && rec.AccountId == args.GLAccountId
                                                    && rec.JournalId == jr.Id).ToList();
                            if (other_LTs_per_j != null && other_LTs_per_j.Count > 0)
                            {
                                other_LTs.AddRange(other_LTs_per_j);
                            }
                        });

                        string accountingEntityCode = journal.AccountingEntityCode;
                        string accountingEntityReference = journal.AccountingEntityReference;
                        if (!String.IsNullOrEmpty(accountingEntityCode) && !String.IsNullOrEmpty(accountingEntityReference))
                        {
                            JournalList list = new JournalList()
                            {
                                AccountingDate = journal.AccountingDate,
                                AccountingEntityCode = journal.AccountingEntityCode,
                                AccountingEntityName = journal.AccountingEntity != null ? journal.AccountingEntity.LocalName : null,
                                AccountingEntityReference = journal.AccountingEntityReference, // CurrencyCode = journal.IsMultiCurrency == true ? multi : journal.Currency != null ? journal.Currency.Code : null,
                                ApproveDate = journal.ApproveDate,
                                ApprovedByUserName = journal.ApprovedByUser != null ? journal.ApprovedByUser.Contact.LocalName : null,
                                CreateDate = journal.CreateDate,
                                CreatedByUserName = journal.CreatedByUser != null ? journal.CreatedByUser.Contact.LocalName : null,
                                ExternalNo = journal.ExternalNo,
                                ExternalSystem = journal.ExternalSystem,
                                Id = journal.Id,
                                JournalNumber = journal.JournalNumber,
                                OriginalJournalName = journal.OriginalJournal != null ? journal.OriginalJournal.JournalNumber : null,
                                SearchFields = journal.SearchFields,
                                StatusName = journal.JournalStatusType != null ? journal.JournalStatusType.EnglishName : null,
                                StatusLocalName = journal.JournalStatusType != null ? journal.JournalStatusType.LocalName : null,
                                Tenant = journal.Tenant,
                                TypeCode = journal.TypeCode,
                                TypeName = journal.JournalType != null ? journal.JournalType.EnglishName : null,
                                TypeLocalName = journal.JournalType != null ? journal.JournalType.LocalName : null,
                                UpdateDate = journal.UpdateDate,
                                UpdatedByUserName = journal.UpdatedByUser != null ? journal.UpdatedByUser.Contact.EnglishName : null,
                                VoidDate = journal.VoidDate,
                                VoidedByUserName = journal.VoidedByUser != null ? journal.VoidedByUser.Contact.EnglishName : null,
                                IsVoided = journal.IsVoided,
                                IsLedgerCreated = journal.IsLedgerCreated,
                            };
                            List<LedgerTransactionList> myLT_List = new List<LedgerTransactionList>();
                            group.ToList().ForEach(lt =>
                            {
                                LedgerTransactionList one_LT_List = new LedgerTransactionList()
                                {
                                    Id = lt.Id,
                                    AccountId = lt.AccountId,
                                    LocalAmountCredit = lt.LocalAmountCredit,
                                    LocalAmountDebit = lt.LocalAmountDebit,
                                };
                                myLT_List.Add(one_LT_List);
                            });
                            other_LTs.ForEach(lt =>
                            {
                                LedgerTransactionList one_LT_List_oth = new LedgerTransactionList()
                                {
                                    Id = lt.Id,
                                    AccountId = lt.AccountId,
                                    LocalAmountCredit = lt.LocalAmountCredit,
                                    LocalAmountDebit = lt.LocalAmountDebit,
                                };
                                myLT_List.Add(one_LT_List_oth);
                            });
                            JournalLT_GroupItem item = new JournalLT_GroupItem()
                            {
                                JournalList = list,
                           //     LT_Group = group,
                                LT_List = myLT_List,
                            };

                            result.Add(item);
                        }
                    }
                }

            }

            return result;
        }


}

    public class InterestTransactionsGetNextGroupArgs
    {
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }
        public int MIN { get; set; }
        public decimal OldAmount { get; set; }
        public DateTime OldDate { get; set; }
        public string OldJournalId { get; set; }
        public DateTime FromDate { get; set; }
        public string FromId { get; set; }
        public DateTime UpToDueDate { get; set; }
        public string ToId { get; set; }
        public bool RunAgain { get; set; }
        public bool MoveOn { get; set; }
        public bool Stop { get; set; }
        public int LT_LinesMaximum { get; set; }
        public int MaxPageSize { get; set; }
        public decimal MaximalDifference { get; set; }
        public decimal ActualDifference { get; set; }
        public bool OnlyZeroes { get; set; }
        public string SpecificJournalId { get; set; }

    }


    
    public class JournalLT_GroupItem
    {

        public JournalList JournalList { get; set; }
      //  public IGrouping<String, LedgerTransaction> LT_Group { get; set; }
        public List<LedgerTransactionList> LT_List { get; set; }
    }

}
