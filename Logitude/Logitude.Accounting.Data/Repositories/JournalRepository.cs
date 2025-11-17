 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Data.Entity;
using Logitude.Server.Tools;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Logitude.Accounting.Data.DataContract;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Utils;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Simplog.Data.Helpers;

namespace Logitude.Accounting.Data.Repositories
{
    public partial class JournalRepository : IRepository<Journal>
    {

        public void LockNoWaitUpdateQueueId(int tenant, string journalId, string QueueId)
        {
            if (string.IsNullOrWhiteSpace(QueueId))
            {
                throw new Exception("QueueId is must");
            }
            var myList = LockByJournal_forUpdateNOWAIT(journalId, tenant);// lock it !!!!
            if (!myList.Any())
            {
                var mess =("JournalApproveService:Failed ... LockByJournal_forUpdateNOWAIT");
                throw new Exception(mess);
            }
            var poco = myList.First();
            if (poco.QueueId != null)
            {
                throw new Exception("JournalApproveService:Failed(poco.QueueId != null) already Streamed !!!");
            }
            
            poco.QueueId = QueueId;

            this.Update(poco);
        }


        public void UpdateWhileStreaming(int tenant, string journalId,Action<Journal> updatePoco)
        {
            var poco = GetSingle(journalId, tenant);
            if (poco == null)
            {
                var mess = ("JournalApproveService:Failed ... LockByJournal_forUpdateNOWAIT");
                throw new Exception(mess);
            }

            updatePoco(poco);
            this.Update(poco);
        }
        public List<Journal> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }
        public List<Journal> LockByJournal_forUpdateNOWAIT(string id, int tenant)
        {
            var list = (context as DbContext)
                //.FirstOrDefaultFUNOWAITWhere
                .GetListNOWAITWhere
                <Journal>(a => a.Id == id && a.Tenant == tenant);
            return list;
        }
        
        partial void onUpdate()//Partial Methods
        {
            InsureUsingOnlyByUpdateService();
        }

        partial void onAdd()//Partial Methods
        {
            InsureUsingOnlyByUpdateService();
        }
        private  void InsureUsingOnlyByUpdateService()
        {
            //return;//mohammad temp fix until itzik is back
            //int iFrame = 3;
            //var mth = new StackTrace().GetFrame(iFrame).GetMethod();
            //var cls = mth.ReflectedType.Name;
            //if (IsClassValid(mth, cls))
            //{
            //    return;
            //}
            //iFrame = 4;
            //mth = new StackTrace().GetFrame(iFrame).GetMethod();
            //cls = mth.ReflectedType.Name;
            //if (IsClassValid(mth, cls))
            //{
            //    return;
            //}
            var myName = this.NameOf();
            if (myName != "JournalRepositoryPriv")
            {
                AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);

                //var checkInsureUsingOnlyByUpdateService = System.Configuration.ConfigurationManager.AppSettings.Get("InsureUsingOnlyByUpdateService");
                //if (!string.IsNullOrWhiteSpace(checkInsureUsingOnlyByUpdateService))
                {
                    throw new Exception("InsureUsingOnlyByUpdateService");
                }
            }
        }

        private bool IsClassValid(System.Reflection.MethodBase mth, string cls)
        {
            if (cls == "JournalUpdateService") //never happen 
            {
                return true;
            }
            if (cls == "JournalApproveService")
            {
                return true;
            }

            if (cls == "EntityUpdateService`3" && mth.Name == "PerformUpdate")
            {
                return true;
            }
            return false;
        }

        public string GetJournalMaxNumber(string JournalId, int tenant)
        {

            var Journals = (from a in context.Journals
                            where a.Tenant == tenant
                            select a);


            var maxValue = Journals.Max(x => x.JournalNumber);
            var result = Journals.First(x => x.JournalNumber == maxValue);




            return maxValue;

        }

        public string GetJournalMaxNumber(int tenant)
        {

            var Journals = (from a in context.Journals
                            where a.Tenant == tenant
                            select a);


            var maxValue = Journals.Max(x => x.JournalNumber);
            var result = Journals.First(x => x.JournalNumber == maxValue);




            return maxValue;

        }


        public Journal GetSinglePendingApproved(int tenant)
        {
            var q = GetQueryablePending2ApproveOrdered(tenant);
            return q.FirstOrDefault();
        }

        
            public IQueryable<Journal> GetJournalWhileStreamingHadError(int tenant)
        {
            var q = (from a in context.Journals
                     where a.Tenant == tenant
                     where a.QueueId == null
                     where (a.StatusCode == "4" )//  4== error while streaming
                     //3 voided 
                     //2	Approved	מאושר	2,Approved,מאושר	0
                     //to be continue ... a new field have to create !!!
                     //where IsNull( a.Transaction)
                     orderby a.ApproveDate, a.AccountingDate descending
                     select a);
            return q;
        }
        public IQueryable<Journal> GetQueryablePending2Approve_LedgerNotCreated(int tenant)
        {
            var q = (from a in context.Journals
                     where a.Tenant == tenant
                     where a.IsLedgerCreated== false//index 
                     where (a.StatusCode == "2" || a.StatusCode == "6" || a.StatusCode == "3")
                     //3 voided 
                     //2	Approved	מאושר	2,Approved,מאושר	0
                     //to be continue ... a new field have to create !!!
                     //where IsNull( a.Transaction)
                     
                     select a);
            return q;
        }

        public IQueryable<Journal> GetQueryableFailedJournals()
        {
            string failedJournalStatus = "4";
            var q = (from a in context.Journals
                     where (a.StatusCode == failedJournalStatus)
                     select a);
            return q;
        }

        public IQueryable<Journal> GetJournalsWithoutTransactionsForToday()
        {
            string approvedJournalStatus = "2";
            var q = (from a in context.Journals
                     where (a.IsLedgerCreated == false && (a.StatusCode == approvedJournalStatus|| a.StatusCode=="6"))
                     select a);
            return q;
        }
        
        public IQueryable<Journal> GetQueryablePending2ApproveOrdered(int tenant)
        {
            var q = (from a in context.Journals
                     where a.Tenant == tenant
                     //where a.QueueId == null
                     where !a.IsLedgerCreated//index 
                     where (a.StatusCode == "6" || a.StatusCode == "3")
                     //3 voided 
                     //2	Approved	מאושר	2,Approved,מאושר	0
                     //to be continue ... a new field have to create !!!
                     //where IsNull( a.Transaction)
                     orderby a.ApproveDate, a.AccountingDate descending
                     select a);
            return q;
        }


        public IQueryable<Journal> GetQueryablePending6ApproveOrdered(int tenant, bool allTenants = false)
        {
            var q = (from a in context.Journals
                     where (allTenants || a.Tenant == tenant)
                                         && (a.StatusCode == "6" || a.StatusCode == "4" || (a.StatusCode == "2" && !a.IsLedgerCreated))
                     select a);
            return q;
        }


        //public IQueryable<Journal> GetQueryablesApprovedStreamed(int tenant)
        //{
        //    var q = (from a in context.Journals
        //             where a.Tenant == tenant
        //             where a.QueueId == null
        //             where (a.StatusCode == "2" || a.StatusCode == "3")

        //             //2	Approved	מאושר	2,Approved,מאושר	0
        //             //to be continue ... a new field have to create !!!
        //             //where !IsNull( a.Transaction)

        //             select a);
        //    return q;
        //}

        public IQueryable<Journal> GetQueryableBetween(int tenant, DateTime fromTruncateTime, DateTime toTruncateTime)
        {
            fromTruncateTime = fromTruncateTime.Date;
            toTruncateTime = toTruncateTime.Date;
            var q = (from a in context.Journals
                     where a.Tenant == tenant
                     where EntityFunctions.TruncateTime(a.AccountingDate) >= fromTruncateTime && EntityFunctions.TruncateTime(a.AccountingDate) <= toTruncateTime
                     select a);
            return q;

        }
        public IQueryable<Journal> GetQueryableApprovedBetween(int tenant, DateTime fromTruncateTime, DateTime toTruncateTime)
        {
            var q = (from a in context.Journals
                     where a.Tenant == tenant
                     //where (a.StatusCode == "2" || a.StatusCode == "3")
                     //where !String.IsNullOrWhiteSpace(a.QueueId)
                     where  !(a.QueueId == null || a.QueueId.Trim() == string.Empty)
                     where EntityFunctions.TruncateTime(a.AccountingDate) >= fromTruncateTime && EntityFunctions.TruncateTime(a.AccountingDate) <= toTruncateTime
                     //2	Approved	מאושר	2,Approved,מאושר	0
                     //to be continue ... a new field have to create !!!
                     //where !IsNull( a.Transaction)

                     select a);
            return q;
        }
        public List<Journal> GetJournalByJournalNumber(string number, int tenant)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            List<Journal> Journal = (from a in context.Journals
                                     where a.JournalNumber == number && a.Tenant == tenant
                                     select a).ToList();
            return Journal;
        }

        public Journal GetSingleJournalByNumber(string number, int tenant)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            Journal Journal = (from a in context.Journals
                                     where a.JournalNumber == number && a.Tenant == tenant
                                     select a).FirstOrDefault();
            return Journal;
        }

        public List<string> GetJournalNumbersByTransactionsList(List<InterestTransactionList> interestTransactionLists, int tenant)
        {
            List<string> entityIdsWithCodes = interestTransactionLists.Select(d => d.EntityId+ "," +( d.InterestEntityTypeCode == InterestEntities.ARInvoice ? AccountingEntities.ARInvoice :
                                                                                                      d.InterestEntityTypeCode == InterestEntities.ARPayment ? AccountingEntities.ARPayment : AccountingEntities.Journal)).ToList();
            List<string> JournalNumbers = (from a in context.Journals
                                     where entityIdsWithCodes.Contains(a.AccountingEntityId+","+a.AccountingEntityCode)  && a.Tenant == tenant
                                     select a.JournalNumber+","+ a.AccountingEntityId + ","  +(a.AccountingEntityCode == AccountingEntities.ARInvoice ? InterestEntities.ARInvoice :
                                                                                               a.AccountingEntityCode == AccountingEntities.ARPayment ? InterestEntities.ARPayment : InterestEntities.Journal)).ToList();
            return JournalNumbers;
        }

        public Journal GetSingleJournalByExternalNoAndExternalSystem(string externalNo,string externalSystem ,int tenant)
        {
           

            Journal Journal = (from a in context.Journals
                               where a.ExternalNo == externalNo  && a.ExternalSystem == externalSystem && a.Tenant == tenant
                               select a).FirstOrDefault();
            return Journal;
        }

        public List<Journal> GetJournalsByAccountingEntityIdAndTypeCode(string entityId, string entityTypeCode, int tenant)
        {


            List<Journal> journals = (from a in context.Journals where
                                                   a.Tenant == tenant &&
                                                   a.AccountingEntityId == entityId &&
                                                   a.AccountingEntityCode == entityTypeCode
                               select a).ToList();
            return journals;
        }
        public Journal GetJournalByAccountingEntity(string entityId, string entityTypeCode, int tenant)
        {
            return (from a in context.Journals.Include("AccountingEntity")
                                      where
                        a.Tenant == tenant &&
                        a.AccountingEntityId == entityId &&
                        a.AccountingEntityCode == entityTypeCode
                                      select a).FirstOrDefault();
        }
        public bool CheckIfExternalNoAndSystemExist(string externalNo, string externalSystem, out string journalNumber, int tenant)
        {
            bool exist;
            journalNumber = "";
            if (String.IsNullOrWhiteSpace(externalNo) || String.IsNullOrWhiteSpace(externalSystem))
            {
                //exist = false;
            }
            else
            {
                IQueryable<Journal> x = (from a in context.Journals
                                         where a.ExternalNo == externalNo && a.Tenant == tenant && a.ExternalSystem == externalSystem
                                         select a);
                //exist = x.Any();
                //if (exist)
                {
                    Journal journal = x.FirstOrDefault<Journal>();
                    if (journal != null)
                    {
                        journalNumber = journal.JournalNumber;
                    }
                }
            }
            return !String.IsNullOrWhiteSpace(journalNumber);
        }



        public JournalEntity GetJournalByARInvoiceEntity(string entityId, int tenant)
        {
            var entity = (from a in context.Journals
                          where a.Tenant == tenant
                          where a.AccountingEntityId == entityId && a.AccountingEntityCode == AccountingEntities.ARInvoice
                          select new JournalEntity
                          {
                              JournalId = a.Id,
                              JournalNumber = a.JournalNumber,

                          }).FirstOrDefault();

            return entity;
        }

        public JournalEntity GetJournalByAccountingEntityId(string entityId, int tenant)
        {
            var entity = (from a in context.Journals
                          where a.Tenant == tenant
                          where a.AccountingEntityId == entityId
                          select new JournalEntity
                          {
                              JournalId = a.Id,
                              JournalNumber = a.JournalNumber,

                          }).FirstOrDefault();

            return entity;
        }
        public JournalEntity GetJournalByAccountingEntityIdAndTypeCode(string entityId,string entityTypeCode, int tenant)
        {
            var entity = (from a in context.Journals
                          where a.Tenant == tenant
                          where a.AccountingEntityId == entityId
                          && a.AccountingEntityCode == entityTypeCode
                          select new JournalEntity
                          {
                              JournalId = a.Id,
                              JournalNumber = a.JournalNumber,

                          }).FirstOrDefault();

            return entity;
        }

        public Journal GetByAccountingEntityId(string entityId,string accountingEntityCode, int tenant)
        {
            var entity = (from a in context.Journals
                          where a.Tenant == tenant
                          && a.AccountingEntityId == entityId
                          && a.AccountingEntityCode == accountingEntityCode
                          select a).FirstOrDefault();

            return entity;
        }
        public Journal GetApprovedJournalByAccountingEntityId(string entityId, string accountingEntityCode, int tenant)
        {
            var entity = (from a in context.Journals
                          where a.Tenant == tenant
                          && a.AccountingEntityId == entityId
                          && a.AccountingEntityCode == accountingEntityCode &&a.StatusCode =="2"
                          select a).FirstOrDefault();

            return entity;
        }

        public Journal GetByAccountingEntityId(string entityId, int tenant)
        {
            var entity = (from a in context.Journals
                          where a.Tenant == tenant
                          && a.AccountingEntityId == entityId
                          select a).FirstOrDefault();

            return entity;
        }

        public IQueryable<Journal> GetByJournalsAccountingEntityId(string entityId, int tenant)
        {
            var journals = (from a in context.Journals.Include("JournalStatusType")
                          where a.Tenant == tenant
                          where a.AccountingEntityId == entityId  
                          select a);

            return journals;
        }
        public IQueryable<Journal> GetByJournalsAccountingEntityIds(List<string> entityIdS, int tenant, string accountingEntityCode)
        {
            var journals = (from a in context.Journals.Include("JournalStatusType")
                            where a.Tenant == tenant
                            where entityIdS.Contains(a.AccountingEntityId)
                                    && a.AccountingEntityCode == accountingEntityCode
                            select a);

            return journals;
        }
        public IQueryable<Journal> GetByJournalsAccountingEntityIdAndCode(string entityId, string entityCode, int tenant)
        {
            var journals = (from a in context.Journals.Include("JournalStatusType")
                            where a.Tenant == tenant
                            where a.AccountingEntityId == entityId && a.AccountingEntityCode == entityCode
                            select a);

            return journals;
        }
        public IQueryable<Journal> GetByOriginal(string originalId, int tenant)
        {
            var journals = (from a in context.Journals.Include("JournalStatusType")
                            where a.Tenant == tenant
                            where a.OriginalJournalId == originalId
                            select a);

            return journals;
        }
        public IQueryable<Journal> GetByJournalsAccountingEntityCodeAndDate(string entityCode, DateTime accountingDate, int tenant)
        {
            var journals = (from a in context.Journals.Include("JournalStatusType")
                            where a.Tenant == tenant
                            where a.AccountingEntityCode == entityCode && a.AccountingDate == accountingDate
                            select a);

            return journals;
        }

        public IQueryable<Journal> GetJournalsNotLTByAccDate(DateTime accountingDateFrom, DateTime accountingDateTo, int tenant)
        {
            var journals = (from a in context.Journals.Include("JournalStatusType")
                            where a.Tenant == tenant
                            where a.StatusCode != "0" && a.StatusCode != "5" && a.StatusCode != "3" && a.AccountingDate >= accountingDateFrom && a.AccountingDate <= accountingDateTo && a.IsLedgerCreated == false
                            select a);

            return journals;
        }

        public IQueryable<Journal> GetJournalsNotLTByAccDateAccEntity(DateTime accountingDateFrom, DateTime accountingDateTo, string entityCode, int tenant)
        {
            var journals = (from a in context.Journals.Include("JournalStatusType")
                            where a.Tenant == tenant
                            where a.StatusCode != "0" && a.StatusCode != "5" && a.StatusCode != "3" && a.AccountingDate >= accountingDateFrom && a.AccountingDate <= accountingDateTo && a.IsLedgerCreated == false
                                    && a.AccountingEntityCode == entityCode
                            select a);

            return journals;
        }


        public List<string> CheckIfThereNonTranslatedJournalsByMonth(int year, int month, int tenant)
        {
            string DraftStatus = "0";
            string CancelledStatus = "5";
            return (from record in context.Journals
                    where
                        record.Tenant == tenant
                        && record.AccountingDate.Year == year
                        && record.AccountingDate.Month == month
                        && record.QueueId == null
                        && record.StatusCode != DraftStatus && record.StatusCode != CancelledStatus
                    select record.JournalNumber).ToList();
        }



        public bool ExistsAccEntCodeByJIds(string accEntCode, IQueryable<string> jIds, int tenant)
        {
            bool rv = false;

            var query = from record in context.Journals
                        where record.Tenant == tenant
                        && jIds.Contains(record.Id)
                        && record.AccountingEntityCode == accEntCode
                        select record.Id;

            rv = query.Any();

            return rv;
        }

        public string GetFirstIdByAccEntCodeByJIds(string accEntCode, IQueryable<string> jIds, int tenant)
        {
            string rv = "";

            var query = from record in context.Journals
                        where record.Tenant == tenant
                        && jIds.Contains(record.Id)
                        && record.AccountingEntityCode == accEntCode
                        select record.Id;

            rv = query.FirstOrDefault();

            return rv;
        }

        public List<CustomTaxReportData> GetARInvoiceJournals(DateTime? taxReportMonth, int tenant)
        {
            int days= DateTime.DaysInMonth(taxReportMonth.Value.Year, taxReportMonth.Value.Month);
            DateTime date = new DateTime(taxReportMonth.Value.Year, taxReportMonth.Value.Month, days);

            IInvoiceContext invoicecontext = InvoiceContext.GetContext(tenant);
            List<string> invoiceIds = (from a in invoicecontext.ARInvoices
                                        where a.InvoiceDate <= date && (a.TotalAmountForTaxReport != null && a.TotalAmountForTaxReport != 0)   && a.Tenant == tenant 
                                        select a.Id).ToList();


            List<CustomTaxReportData> list = new List<CustomTaxReportData>();
            const int sqlLimit = 5000;
            int iterations = invoiceIds.Count() / sqlLimit;
            for (int i = 0; i <= iterations; i++)
            {
                var tempInvoiceIds = invoiceIds.Skip(i * sqlLimit).Take(sqlLimit).ToList();
                List<CustomTaxReportData> tempList = (from j in context.Journals
                                                      join jl in context.JournalLines on j.Id equals jl.JournalId
                                                      join ledger in context.LedgerTransactions on new { JournalId = j.Id, JournalLineNumber = jl.Line } equals new { ledger.JournalId, ledger.JournalLineNumber }
                                                      join adt in context.JournalAdditionalDatas on j.Id equals adt.JournalId
                                                      where j.AccountingEntityCode == "2" && (adt.TaxReportId == null || adt.TaxReportTransmitStatusCode == "2" || adt.TaxReportTransmitStatusCode == null) && j.Tenant == tenant
                                                            && jl.DocumentDate <= date && adt.Tenant == tenant
                                                            && tempInvoiceIds.Contains(j.AccountingEntityId)
                                                      select new CustomTaxReportData()
                                                      {
                                                          Id = j.Id,
                                                          AccountingEntityId = j.AccountingEntityId,
                                                          IsLedgerReconciled = (ledger == null ? false : ledger.IsReconciled),
                                                          LedgerTransactionId = (ledger == null ? null : ledger.Id)

                                                      }).ToList();
                list.AddRange(tempList);
            }


            //List<CustomTaxReportData> data = (from j in journals
            //                                  join jl in context.JournalLines on j.Id equals jl.JournalId

            //                                  where invoiceIds.Contains(j.AccountingEntityId)
            //                                  select new CustomTaxReportData()
            //                                  {
            //                                      Id = j.Id,
            //                                      AccountingEntityId = j.AccountingEntityId,
            //                                      IsLedgerReconciled = (ledger == null ? false : ledger.IsReconciled)

            //                                  }).ToList();


            return list;

           
        }

        public Journal GetJournalByIdAndTenant(string id, int tenant)
        {
            return (from a in context.Journals

                    where a.Id == id && a.Tenant == tenant


                    select a).FirstOrDefault();
        }

        public IQueryable<Journal> GetJournalsByIds(List<string> ids, int tenant)
        {
            var journals = (from a in context.Journals 
                            where a.Tenant == tenant
                            && ids.Contains(a.Id)
                            select a);

            return journals;
        }
        public IQueryable<Journal> GetByJournalsAccountingIds(List<string> accountingEntityIds,string accountingEntityCode, int tenant)
        {
            var journals = (from a in context.Journals
                            where a.Tenant == tenant
                            && a.AccountingEntityCode == accountingEntityCode
                            && accountingEntityIds.Contains(a.AccountingEntityId)
                            select a);

            return journals;
        }


        public List<Journal> GetFailedJournalsInReconcileProcess(string accountId, int tenant)
        {
           
            var internalReconciles = from r in context.JournalReconciles
                                     join j in context.Journals on r.JournalId equals j.Id
                                     join l in context.LedgerTransactions on r.LedgerTransactionId equals l.Id
                                     where l.InReconcileProgress && !l.IsReconciled && j.StatusCode == "4"
                                           && l.AccountId == accountId && l.Tenant == tenant && r.Tenant == tenant && j.Tenant == tenant
                                     select j;

            var externalReconciles = from r in context.JournalExternalReconciles
                                     join j in context.Journals on r.JournalId equals j.Id
                                     join l in context.LedgerTransactions on r.LedgerTransactionId equals l.Id
                                     where l.InProgressExternalReconcile && !l.IsExternalReconcile && j.StatusCode == "4"
                                           && l.AccountId == accountId && l.Tenant == tenant && r.Tenant == tenant && j.Tenant == tenant
                                     select j;

            return internalReconciles.Union(externalReconciles).Distinct().ToList();
        }

        public void FixFailedReconcileJournals(int tenant)
        {
            string sqlConnectionString = TenantServerConfigration.GetDbConnection(tenant);

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                try
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand("dbo.usp_FixFailedReconcileJournals", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Tenant", tenant);

                        command.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlConnection.Close();                 }
            }
        }
    }




}


public class JournalEntity
{
    public string JournalId  { get; set; }
    public string JournalNumber { get; set; }
}
