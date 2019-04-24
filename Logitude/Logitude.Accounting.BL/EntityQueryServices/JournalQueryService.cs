using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{

    public partial class JournalQueryService : EntityQueryService<Journal, JournalKeys, JournalPM, object, JournalKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, JournalPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            JournalKeys journalKeys = entityKeys as JournalKeys;

            JournalLineQueryService journalLineQueryService = new JournalLineQueryService(context);



            //******getting all compositionTables for response service purposes only *****///

            entityPM.JournalLines = journalLineQueryService.GetMulti(journalKeys, true);

            // entityPM.DeclarationErrorViews = this.GetDeclarationErrors(declarationKeys.Id, entityPM.Tenant, null);
            //****************************************************************************//



            var journalReconcileQueryService = new JournalReconcileQueryService(context);



            //******getting all compositionTables for response service purposes only *****///

            entityPM.JournalReconciles = journalReconcileQueryService.GetMulti(journalKeys, true);

        }


        //public JournalPM  GetJournalPMByJournalNumber(string journalNumber, int tenant)
        //{
        
        //}

        public string GetJournalMaxNumber(int tenant)
        {
            string max =  this.repository.GetJournalMaxNumber(tenant);

            int maxJournalNumber;

            int.TryParse(max, out maxJournalNumber);

            maxJournalNumber++;

            return maxJournalNumber.ToString();

        }

        public List<string> GetApprovedJournalWithoutLedgerTransaction(int tenant, 
            string seedJournalId,
            List<string> ExcludeJournalKeys,int top=15)
        {

            var ledgerTransactionRepository = new LedgerTransactionRepository(this.MainContext as IAccountingContext);
            var qPendingApprovedOrderedQueryable = repository.GetQueryablePending2ApproveOrdered(tenant);
            if (ExcludeJournalKeys != null && ExcludeJournalKeys.Count > 0)
            //exclude last executed buffer//
            {
                qPendingApprovedOrderedQueryable = qPendingApprovedOrderedQueryable.Where(rec => !ExcludeJournalKeys.Contains(rec.Id));
            }
            if (!string.IsNullOrWhiteSpace(seedJournalId))
            {
                qPendingApprovedOrderedQueryable = 
                    qPendingApprovedOrderedQueryable
                    .Where(r => r.Id == seedJournalId);
            }
            var q = (from j in qPendingApprovedOrderedQueryable
                     join t in ledgerTransactionRepository.GetAll(tenant)
                     on j.Id equals t.JournalId into myJoin
                     from jt in myJoin.DefaultIfEmpty()
                     where jt == null
                     select j.Id);
            var keys = q.Take(top).ToList();
            return keys;
        }


        public List<string> GetJournalWhileStreamingHadErrorWithStatus4(int tenant,
            string seedJournalId,
            List<string> ExcludeJournalKeys, int top = 15)
        {

            var ledgerTransactionRepository = new LedgerTransactionRepository(this.MainContext as IAccountingContext);
            var qPendingApprovedOrderedQueryable = repository.GetJournalWhileStreamingHadError(tenant);
            if (ExcludeJournalKeys != null && ExcludeJournalKeys.Count > 0)
            //exclude last executed buffer//
            {
                qPendingApprovedOrderedQueryable = qPendingApprovedOrderedQueryable.Where(rec => !ExcludeJournalKeys.Contains(rec.Id));
            }
            if (!string.IsNullOrWhiteSpace(seedJournalId))
            {
                qPendingApprovedOrderedQueryable =
                    qPendingApprovedOrderedQueryable
                    .Where(r => r.Id == seedJournalId);
            }
            var q = (from j in qPendingApprovedOrderedQueryable
                     join t in ledgerTransactionRepository.GetAll(tenant)
                     on j.Id equals t.JournalId into myJoin
                     from jt in myJoin.DefaultIfEmpty()
                     where jt == null
                     select j.Id);
            var keys = q.Take(top).ToList();
            return keys;
        }


        public bool GetAnyPendingApprovedDev(IQueryable<string> GLAccountIDList, int tenant)
        {

            var journalLineRepository = new JournalLineRepository(this.MainContext as IAccountingContext);
            var have = (from j in repository.GetQueryablePending2ApproveOrdered(tenant)
                        join jl in journalLineRepository.GetQueryContainsAccId(GLAccountIDList, tenant)
                        on j.Id equals jl.JournalId
                        select jl).Any();
            return have;
        }
        public JournalPM GetSinglePendingApprovedDev(int tenant, bool getComposition = false)
        {
            var ledgerTransactionRepository = new LedgerTransactionRepository(tenant);
            var q = (from j in repository.GetQueryablePending2ApproveOrdered(tenant)
                     join t in ledgerTransactionRepository.GetAll(1)
                     on j.Id equals t.JournalId into myJoin
                     from jt in myJoin.DefaultIfEmpty()
                     where jt == null
                     select j);
            var poco = q.FirstOrDefault();
            if (poco != null)
            {
                return base.GetEntityPM(poco, getComposition, new JournalKeys() { Id = poco.Id });
            }
            else
            {
                return base.GetEntityPM(poco);
            }
        }
        public JournalPM GetSinglePendingApproved(int tenant)
        {
            Journal poco = repository.GetSinglePendingApproved(tenant);
            return base.GetEntityPM(poco);
        }
        

        public List<JournalPM> GetJournalByJournalNumber(string number, int tenant)
        {
            List<Journal> journal = repository.GetJournalByJournalNumber(number, tenant);


            List<JournalPM> ratepms = (from a in journal
                                       select new JournalPM()
                                                   {
                                                       AccountingDate = a.AccountingDate,
                                                       AccountingEntityCode = a.AccountingEntityCode,
                                                       AccountingEntityId = a.AccountingEntityId,
                                                       AccountingEntityName = a.AccountingEntityReference != null ? a.AccountingEntityReference : null,
                                                       AccountingEntityReference = a.AccountingEntityReference, // CurrencyCode = a.IsMultiCurrency == true ? multi : a.Currency != null ? a.Currency.Code : null,
                                                       ApproveDate = a.ApproveDate,
                                                       ApprovedByUserId = a.ApprovedByUserId,
                                                       ApprovedByUserName = a.ApprovedByUser != null ? a.ApprovedByUser.Contact.EnglishName : null,
                                                       CreateDate = a.CreateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                       ExternalNo = a.ExternalNo,
                                                       Id = a.Id,
                                                       JournalNumber = a.JournalNumber,
                                                       OriginalJournalId = a.OriginalJournalId,
                                                       OriginalJournalName = a.OriginalJournal != null ? a.OriginalJournal.JournalNumber : null,
                                                       SearchFields = a.SearchFields,
                                                       StatusCode = a.StatusCode,
                                                       StatusName = a.JournalStatusType != null ? a.JournalStatusType.EnglishName : null,
                                                       Tenant = a.Tenant,
                                                       TypeCode = a.TypeCode,
                                                       TypeName = a.JournalType != null ? a.JournalType.EnglishName : null,

                                                       UpdateDate = a.UpdateDate,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                       UpdatedByUserName = a.UpdatedByUser != null ? a.VoidedByUser.Contact.EnglishName : null,
                                                       VoidDate = a.VoidDate,
                                                       VoidedByUserId = a.VoidedByUserId,
                                                      
                                                   }).ToList();

            return ratepms;
        }

        public JournalPM GetSingleJournalByNumber(string number, int tenant)
        {
            Journal a = repository.GetSingleJournalByNumber(number, tenant);


            JournalPM journal = new JournalPM()
            {
                AccountingDate = a.AccountingDate,
                AccountingEntityCode = a.AccountingEntityCode,
                AccountingEntityId = a.AccountingEntityId,
                AccountingEntityName = a.AccountingEntityReference != null ? a.AccountingEntityReference : null,
                AccountingEntityReference = a.AccountingEntityReference, // CurrencyCode = a.IsMultiCurrency == true ? multi : a.Currency != null ? a.Currency.Code : null,
                ApproveDate = a.ApproveDate,
                ApprovedByUserId = a.ApprovedByUserId,
                ApprovedByUserName = a.ApprovedByUser != null ? a.ApprovedByUser.Contact.EnglishName : null,
                CreateDate = a.CreateDate,
                CreatedByUserId = a.CreatedByUserId,
                CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                ExternalNo = a.ExternalNo,
                Id = a.Id,
                JournalNumber = a.JournalNumber,
                OriginalJournalId = a.OriginalJournalId,
              
                SearchFields = a.SearchFields,
                StatusCode = a.StatusCode,
                StatusName = a.JournalStatusType != null ? a.JournalStatusType.EnglishName : null,
                Tenant = a.Tenant,
                TypeCode = a.TypeCode,
                TypeName = a.JournalType != null ? a.JournalType.EnglishName : null,
                ExternalSystem = a.ExternalSystem,
                UpdateDate = a.UpdateDate,
                UpdatedByUserId = a.UpdatedByUserId,
                UpdatedByUserName = a.UpdatedByUser != null ? a.VoidedByUser.Contact.EnglishName : null,
                VoidDate = a.VoidDate,
                VoidedByUserId = a.VoidedByUserId,

            };

            if (journal.OriginalJournalId != null)
            {
              
                JournalQueryService journalQueryService = new JournalQueryService(tenant);
                JournalPM parent = journalQueryService.GetSingle(journal.OriginalJournalId, false, false);
                journal.OriginalJournalName = parent.JournalNumber;

            }
            JournalLineQueryService journalLineQueryService = new JournalLineQueryService(tenant);
            List<JournalLinePM> lines = journalLineQueryService.GetJournalLinesByJournalId(journal.Id, tenant);
            journal.JournalLines = lines;
            return journal;
        }

        public JournalPM GetByAccountingEntityId(string entityId, int tenant)
        {
            Journal poco = repository.GetByAccountingEntityId(entityId, tenant);
            return base.GetEntityPM(poco);
        }

        public bool CheckIfExternalNoAndSystemExist(string externalNo, string externalSystem, out string journalNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(externalNo) || String.IsNullOrWhiteSpace(externalSystem))
            {
                journalNumber = null;
                return false;
            }
            else
            {
                return this.repository.CheckIfExternalNoAndSystemExist(externalNo, externalSystem, out journalNumber, tenant);
            }
        }

        public IQueryable<JournalPM> GetJournalsByAccountingEntityId(string entityId, int tenant)
        {
            IQueryable<Journal> journalQuery = repository.GetByJournalsAccountingEntityId(entityId,tenant);

            IQueryable<JournalPM> journals = from a in journalQuery
                                             select new JournalPM()
                                             {
                                                 JournalNumber = a.JournalNumber,
                                                 AccountingDate = a.AccountingDate,
                                                 StatusName = a.JournalStatusType != null? a.JournalStatusType.LocalName :null,
                                                 Id = a.Id
                                             };
          
            return journals;
        }
        public IQueryable<JournalPM> GetJournalsByAccountingEntityIdAndCode(string entityId, string entityCode, int tenant)
        {
            IQueryable<Journal> journalQuery = repository.GetByJournalsAccountingEntityIdAndCode(entityId, entityCode, tenant);

            IQueryable<JournalPM> journals = from a in journalQuery
                                             select new JournalPM()
                                             {
                                                 JournalNumber = a.JournalNumber,
                                                 AccountingDate = a.AccountingDate,
                                                 StatusName = a.JournalStatusType != null ? a.JournalStatusType.LocalName : null,
                                                 Id = a.Id
                                             };

            return journals;
        }


        public JournalPM GetSinglePM(string id, int tenant)
        {
            Journal poco = null;
            poco = repository.GetJournalByIdAndTenant(id, tenant);
            JournalPM pm = this.GetEntityPM(poco);

            if (pm.OriginalJournalId != null)
            {

                JournalQueryService journalQueryService = new JournalQueryService(tenant);
                JournalPM parent = journalQueryService.GetSingle(pm.OriginalJournalId, false, false);
                pm.OriginalJournalName = parent.JournalNumber;

            }
            JournalLineQueryService journalLineQueryService = new JournalLineQueryService(tenant);
            List<JournalLinePM> lines = journalLineQueryService.GetJournalLinesByJournalId(id,tenant);
            pm.JournalLines = lines;

            return pm;
        }
        public JournalPM GetSingleJournalByExternalNoAndExternalSystem(string externalNo,string externalSystem, int tenant)
        {
            Journal poco = null;
            poco = repository.GetSingleJournalByExternalNoAndExternalSystem(externalNo, externalSystem, tenant);
            JournalPM journal = this.GetEntityPM(poco);
            if (journal != null)
            {
                if (journal.OriginalJournalId != null)
                {

                    JournalQueryService journalQueryService = new JournalQueryService(tenant);
                    JournalPM parent = journalQueryService.GetSingle(journal.OriginalJournalId, false, false);
                    journal.OriginalJournalName = parent.JournalNumber;

                }
                JournalLineQueryService journalLineQueryService = new JournalLineQueryService(tenant);
                List<JournalLinePM> lines = journalLineQueryService.GetJournalLinesByJournalId(journal.Id, tenant);
                journal.JournalLines = lines;
            }
            return journal;
        }

        public List<JournalPM> GetJournalsByIds(List<string> ids, int tenant)
        {
            IQueryable<Journal> journalQuery = repository.GetByJournalsAccountingIds(ids, tenant);

            IQueryable<JournalPM> journals = from a in journalQuery
                                             select new JournalPM()
                                             {
                                                 JournalNumber = a.JournalNumber,
                                                 AccountingDate = a.AccountingDate,
                                                 StatusName = a.JournalStatusType != null ? a.JournalStatusType.LocalName : null,
                                                 Id = a.Id
                                                 
                                             };
            //foreach(JournalPM journal in journals)
            //{

            //    journal.JournalLines =  GetJournalLines(journal, tenant);
            //}
            return journals.ToList();
        }

      //public List<JournalLinePM>  GetJournalLines(JournalPM journal, int tenant)
      //  {
      //      List<JournalLinePM> journalLines = new List<JournalLinePM>();
      //      JournalLineQueryService journalLineQueryService = new JournalLineQueryService(tenant);
      //     journalLines  = journalLineQueryService.GetJournalLinesByJournalId(journal.Id, tenant);

      //      return journalLines;
      //  }
    }
}
