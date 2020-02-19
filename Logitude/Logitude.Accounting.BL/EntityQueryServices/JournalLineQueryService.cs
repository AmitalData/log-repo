using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class JournalLineQueryService : EntityQueryService<JournalLine, JournalLineKeys, JournalLinePM, JournalPM, JournalKeys>
    {
        private IQueryable<IGrouping<string, JournalLineLedgerTransactionDTO>> _1;

        public IQueryable<JournalLine> GetQJournalLineByAcountingDate(DateTime fromDate, DateTime toDate, int tenant)
        {
            return this.repository.GetAll(tenant)
                .Where(rec => EntityFunctions.TruncateTime(rec.AccountingDate) >= fromDate.Date && EntityFunctions.TruncateTime(rec.AccountingDate) <= toDate.Date);
        }



        //public IQueryable<IGrouping<String, JournalLineLedgerTransactionDTO>>
        //    GetQGJournalLinesByExternalRecoFromTo(int tenant, string fromExtNum, string toExtNum)
        //{

        //    var q = (from jl in this.repository.GetAll(tenant)
        //                 .Where(rec => rec.ExternalReconcileNumber != null && rec.ExternalReconcileNumber != ""
        //                 && rec.ExternalReconcileNumber != "0"
        //                 && rec.ExternalReconcileNumber != "0.00"
        //                 && rec.ExternalReconcileNumber.CompareTo(fromExtNum) >= 0
        //                 && rec.ExternalReconcileNumber.CompareTo(toExtNum) <= 0)
        //             join trans in (context as AccountingContext).LedgerTransactions.Where(r => r.Tenant == tenant && r.IsReconciled == false)
        //             on new { jl.JournalId, jl.Line }
        //             equals new { trans.JournalId, Line = trans.JournalLineNumber }
        //             into joinT
        //             from joinr in joinT
        //             select new JournalLineLedgerTransactionDTO
        //             {
        //                 JournalLine = jl,
        //                 LedgerTransaction = joinr,
        //             });
        //    return q.OrderBy(rec => rec.JournalLine.ExternalReconcileNumber).GroupBy(rec => rec.JournalLine.ExternalReconcileNumber);

        //}

 //       public IQueryable<JournalLineLedgerTransactionDTO>
        public IEnumerable<JournalLineLedgerTransactionDTO>
            GetQGJournalLinesByExternalRecoFromTo(int tenant, string fromExtNum, string toExtNum)
        {
            var pre_q = from j_lines in this.repository.GetAll(tenant)
                        .Where(rec => rec.ExternalReconcileNumber != null && rec.ExternalReconcileNumber != ""
                         && rec.ExternalReconcileNumber != "0"
                         && rec.ExternalReconcileNumber != "0.00" && rec.ExternalReconcileNumber != "000000000000000"
                         && rec.ExternalReconcileNumber.CompareTo(fromExtNum) >= 0
                         && rec.ExternalReconcileNumber.CompareTo(toExtNum) <= 0)
                        select j_lines;
            List<JournalLine> jl_list = pre_q.ToList();
            List<String> jL_Id_list = jl_list.Select(i => i.JournalId).ToList<String>();

            var q = (from jl in jl_list
//                         .Where(rec => rec.ExternalReconcileNumber != null && rec.ExternalReconcileNumber != ""
//                         && rec.ExternalReconcileNumber != "0"
//                         && rec.ExternalReconcileNumber != "0.00" && rec.ExternalReconcileNumber != "000000000000000"
//                         && rec.ExternalReconcileNumber.CompareTo(fromExtNum) >= 0
//                         && rec.ExternalReconcileNumber.CompareTo(toExtNum) <= 0)
                     join trans in (context as AccountingContext).LedgerTransactions.Where(r => r.Tenant == tenant && jL_Id_list.Contains(r.JournalId) && r.IsReconciled == false)
                     on new { jl.JournalId, jl.Line }
                     equals new { trans.JournalId, Line = trans.JournalLineNumber }
                     into joinT
                     from joinr in joinT
                     select new JournalLineLedgerTransactionDTO
                     {
                         JournalLine = jl,
                         LedgerTransaction = joinr,
                     });
            return q;//.OrderBy(rec => rec.JournalLine.ExternalReconcileNumber).GroupBy(rec => rec.JournalLine.ExternalReconcileNumber);

        }


        public string GetMaxExternalRecoNum(int tenant)
        {
            string rv;
            var pre_q = this.repository.GetAll(tenant)
                        .Where(rec => rec.ExternalReconcileNumber != null && rec.ExternalReconcileNumber != ""
                         && rec.ExternalReconcileNumber != "0"
                         && rec.ExternalReconcileNumber != "0.00" && rec.ExternalReconcileNumber != "000000000000000");
            if (pre_q != null)
            {
                rv = pre_q.Max(i => i.ExternalReconcileNumber);
            }
            else
            {
                rv = "";
            }
            return rv;
        }


        public IQueryable<JournalLineLedgerTransactionAccDTO>
            GetQJournalLinesByExternalNo_NotReconciled(int tenant)
        {
            IQueryable<JournalLine> q1 = (from jline in this.repository.GetAll(tenant)
                     join journals in (context as AccountingContext).Journals.Where(r => r.Tenant == tenant && r.ExternalNo != null)
                     on jline.JournalId equals journals.Id
                     select jline);

            IQueryable<JournalLineLedgerTransactionAccDTO> q = (from jl in q1
                         .Where(rec => rec.ExternalReconcileNumber != null )
                     join trans in (context as AccountingContext).LedgerTransactions.Where(r => r.Tenant == tenant && r.IsReconciled == false)
                     on new { jl.JournalId, jl.Line }
                     equals new { trans.JournalId, Line = trans.JournalLineNumber }
                     into joinT
                     from joinr in joinT
                     select new JournalLineLedgerTransactionAccDTO
                     {
                         JournalLine = jl,
                         LedgerTransaction = joinr,
                         AccId = jl.ActionCode == "1" ? jl.CreditAccountId : jl.DebitAccountId,
                     });
            return q;
        }



        public IQueryable<JournalLineLedgerDTO> GetJournalLineAsLedgerTransaction(DateTime fromTruncateTime, DateTime toTruncateTime, int tenant
            //, JournalLineQueryService qsJournalLine
            , string JournalId = null, int? JournalLine = null
            , String glAccountId = null
            )
        {

            var myTaxCard = (new AccountingSettingResolver()).ResolveVATOutputGLAccountId(tenant);
            var JornalRepo = new JournalRepository(this.MainContext as IAccountingContext);
            var qApprovedBetweenJournal = JornalRepo.GetQueryableApprovedBetween(tenant, fromTruncateTime, toTruncateTime);


            var defFromDate = DateTime.Now;

            //var full = new FullAccountingSettingQueryService.Get(tenant);
            var fullPm = //full.GetSingleFullAccountingSetting(tenant);
                FullAccountingSettingQueryService.Get(tenant);
            //var vatTypePercentageQuery = new VatTypePercentageQuery(tenant);
            var percentagesQueryOrderDescByFromDate =
                //vatTypePercentageQuery
                //.GetVatTypePercentagesForVatType(tenant, fullPm.DefaultVATTypeId)
                //.Where(d => d.FromDate !=null)
                (from a in (this.context as AccountingContext).VatTypePercentages
                 where a.Tenant == tenant && a.VatTypeId == fullPm.DefaultVATTypeId
                 select new VatTypePercentageDTO()
                 {
                     FromDate = a.FromDate ?? defFromDate,
                     //Id = a.Id,
                     //myVat =  a.Percentage==null ? 100: (decimal)((a.Percentage + 100) / 100) 
                     myVat =
                     (a.Percentage == null || a.Percentage == 0)
                     ?
                     (double)100.0m
                     : (double)((a.Percentage + 100) / 100)

                     //Tenant = a.Tenant,
                     //VatTypeId = a.VatTypeId,
                 })
                .OrderByDescending(d => d.FromDate);
            percentagesQueryOrderDescByFromDate.ToList();
            //List<string> myDateVatList = percentagesQueryOrderDescByFromDate
            //    .Select(r => String.Concat(r.FromDate.Value.Date.ToShortDateString(), " ", ((decimal)(100 + r.Percentage) / 100).ToString()))
            //    .ToList();
            //var myVat = 1.18m; //AccountingSettingResolver.ResolveVat();    

            //var qJournalLineByAcountingDate = this.GetQJournalLineByAcountingDate(fromTruncateTime, toTruncateTime, tenant);

            var qJournalLineByAcountingDate = (
                from j in qApprovedBetweenJournal
                join jl in this.repository.GetAll(tenant)
                on j.Id equals jl.JournalId
                select jl);

            if (!String.IsNullOrWhiteSpace(JournalId))
            {
                qJournalLineByAcountingDate = qJournalLineByAcountingDate.Where(rec => rec.JournalId == JournalId);
                if (JournalLine.HasValue)
                {
                    int jline = JournalLine.GetValueOrDefault();
                    qJournalLineByAcountingDate = qJournalLineByAcountingDate.Where(rec => rec.Line == jline);

                }
            }

            if (!String.IsNullOrWhiteSpace(glAccountId))
            {
                qJournalLineByAcountingDate = qJournalLineByAcountingDate.Where(rec =>
                rec.CreditAccountId == glAccountId || rec.DebitAccountId == glAccountId);
            }

            var qJournalLineLedgerDTO= GetJournalLineLedgerDTO(myTaxCard, percentagesQueryOrderDescByFromDate, qJournalLineByAcountingDate);
            if (!String.IsNullOrWhiteSpace(glAccountId))
            {
                qJournalLineLedgerDTO =
                    qJournalLineLedgerDTO
                    .Where(rec => rec.AccountId == glAccountId);
            }
            return qJournalLineLedgerDTO;
        }

        private static IQueryable<JournalLineLedgerDTO> GetJournalLineLedgerDTO(string myTaxCard, IOrderedQueryable<VatTypePercentageDTO> percentagesQueryOrderDescByFromDate, IQueryable<JournalLine> qJournalLineByAcountingDate)
        {
            var crditList = new List<string>(){
        ((int)MyJournalActionTypeEnum.Credit).ToString(),
        ((int)MyJournalActionTypeEnum.DebitAndCredit).ToString(),
        ((int)MyJournalActionTypeEnum.DebitCreditAndVatdeduction).ToString(),
        };
            var qJLCredit =
                qJournalLineByAcountingDate.Where(rec => crditList.Contains(rec.JournalActionType.Code))

                .Select(rec => new JournalLineLedgerDTO()
                {
                    CHANGE_TYPE = "",
                    JournalId = rec.JournalId,
                    JournalLineNumber = rec.Line,
                    AccountId = rec.CreditAccountId,
                    CurrencyId = rec.CurrencyId,
                    LocalAmountCredit = (double)rec.LocalAmount,
                    LocalAmountDebit = 0,
                    ForeignAmountCredit = (double)rec.ForeignAmount,
                    ForeignAmountDebit = 0,
                    AccountingDate = rec.AccountingDate,
                    DueDate = rec.DueDate,
                    DocumentDate = rec.DocumentDate,

                });
            var debitList = new List<string>(){
        ((int)MyJournalActionTypeEnum.Debit).ToString(),
        ((int)MyJournalActionTypeEnum.DebitAndCredit).ToString(),
        };
            var qJLDebit =
                 qJournalLineByAcountingDate.Where(rec => debitList.Contains(rec.JournalActionType.Code))
                 .Select(rec => new JournalLineLedgerDTO()
                 {
                     CHANGE_TYPE = "",
                     JournalId = rec.JournalId,
                     JournalLineNumber = rec.Line,
                     AccountId = rec.DebitAccountId,
                     CurrencyId = rec.CurrencyId,
                     LocalAmountCredit = 0,
                     LocalAmountDebit = (double)rec.LocalAmount,
                     ForeignAmountCredit = 0,
                     ForeignAmountDebit = (double)rec.ForeignAmount,
                     AccountingDate = rec.AccountingDate,
                     DueDate = rec.DueDate,
                     DocumentDate = rec.DocumentDate,

                 });


            var qJLDebitVat =
                (from jl in
                     qJournalLineByAcountingDate.Where(rec => rec.JournalActionType.Code == ((int)MyJournalActionTypeEnum.DebitCreditAndVatdeduction).ToString())
                 from vl in
                     (from v in percentagesQueryOrderDescByFromDate.OrderByDescending(r => r.FromDate)
                      where v.FromDate <= jl.DocumentDate
                      select v).Take(1)
                 select new { jl, vl.myVat }
                )
                .Select(rec => new JournalLineLedgerDTO()
                {
                    CHANGE_TYPE = "",
                    JournalId = rec.jl.JournalId,
                    JournalLineNumber = rec.jl.Line,
                    AccountId = rec.jl.DebitAccountId,
                    CurrencyId = rec.jl.CurrencyId,
                    LocalAmountCredit = 0,
                    LocalAmountDebit =
                    Math.Round((double)((double)rec.jl.LocalAmount / (double)rec.myVat), 2),
                    //Math.Round(((rec.jl.LocalAmount / ((rec.Percentage + 100) / 100))), 2),
                    //rec.Percentage ==null ?  
                    //Math.Round(((double)(rec.jl.LocalAmount / (double)(( 200) / 100))), 2) :
                    //Math.Round(((double)(rec.jl.LocalAmount / (double)((rec.Percentage.GetValueOrDefault() + 100) / 100))), 2) 

                    ForeignAmountCredit = 0,
                    ForeignAmountDebit =
                    Math.Round((double)((double)rec.jl.ForeignAmount / (double)rec.myVat), 2)
                    //Math.Round(((decimal)(rec.jl.ForeignAmount / (decimal)((rec.Percentage  + 100) / 100))), 2),
                    //(decimal)Expression.Divide((decimal)rec.ForeignAmount, (decimal)myVat)
                                        
                    ,AccountingDate = rec.jl.AccountingDate,
                    DueDate = rec.jl.DueDate,
                    DocumentDate = rec.jl.DocumentDate,

                });

            var qJLVat =
                (
                from jl in
                    qJournalLineByAcountingDate.Where(rec => rec.JournalActionType.Code == ((int)MyJournalActionTypeEnum.DebitCreditAndVatdeduction).ToString())
                from vl in
                    (from v in percentagesQueryOrderDescByFromDate.OrderByDescending(r => r.FromDate)
                     where v.FromDate <= jl.DocumentDate
                     select v).Take(1)
                select new { jl, myVat = vl.myVat }
                 )
                 .Select(rec => new JournalLineLedgerDTO()
                 {
                     CHANGE_TYPE = "",
                     JournalId = rec.jl.JournalId,
                     JournalLineNumber = rec.jl.Line,

                     AccountId = myTaxCard,//rec.CreditAccountId,
                     CurrencyId = rec.jl.CurrencyId,

                     LocalAmountCredit = 0,
                     LocalAmountDebit = Math.Round((double)rec.jl.LocalAmount - (1 * (double)rec.jl.LocalAmount / (double)rec.myVat), 2),

                     ForeignAmountCredit = 0,
                     ForeignAmountDebit = Math.Round((double)rec.jl.ForeignAmount - 1 * ((double)rec.jl.ForeignAmount / (double)rec.myVat), 2)
                     //ForeignAmountDebit =
                     //Math.Truncate(
                     //Math.Truncate(
                     // (double)(
                     // (double)rec.jl.ForeignAmount - (1 * (double)rec.jl.ForeignAmount / (double)rec.myVat)
                     // )
                     // *1000
                     // )
                     // /100)
                                         ,
                     AccountingDate = rec.jl.AccountingDate,
                     DueDate = rec.jl.DueDate,
                     DocumentDate = rec.jl.DocumentDate,

                 });
            //var qJLVat_260_3 = qJLVat.First(r => r.JournalId == "1-260" && r.JournalLineNumber == 3);

            var qJLAll = qJLCredit.Union(qJLDebit).Union(qJLDebitVat).Union(qJLVat);
            bool UnionreturnsDistinctvalues = true;
            if (UnionreturnsDistinctvalues)
            {
                qJLAll = qJLCredit.Concat(qJLDebit).Concat(qJLDebitVat).Concat(qJLVat);
            }
                //qJLAll.ToList();
                return qJLAll;
        }

        //public IQueryable<JournalLineLedgerDTO> GetJournalLineAsLedgerTransactionByAccId(string glAccountId, int tenant)
        //{
        //    var journalRepository = new JournalRepository(this.MainContext as IAccountingContext);
            
        //    var qJournal = journalRepository.GetQueryablesApprovedStreamed(tenant);
        //    var qLines = repository.GetQueryContainsAccId(new List<string>() { glAccountId }, tenant);
        //    var qq = (from j in qJournal
        //              join jl in qLines
        //              on j.Id equals jl.JournalId
        //              select jl
        //              );
        //    return GetJournalLineLedgerDTO(null,null,qq);
        //}


        public List<JournalLinePM> GetJournalLinesByJournalId(string JournalId, int tenant)
        {
           
            List<JournalLine> journals = repository.GetJournalLines(JournalId, tenant);

            return journals.Select(rec => this.GetEntityPM(rec)).ToList();
        }

        public JournalLinePM GetSingleJournalLine(string JournalId, int line, int tenant)
        {
            JournalLine poco = repository.GetSingleJournalLine(JournalId, line, tenant);
            return GetEntityPM(poco);
        }
    }
}
