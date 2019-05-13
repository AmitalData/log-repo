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

    }


}
	