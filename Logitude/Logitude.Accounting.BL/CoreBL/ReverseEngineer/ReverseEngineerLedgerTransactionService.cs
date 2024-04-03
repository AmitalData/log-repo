using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ReverseEngineerLedgerTransactionService
    {
        private DateTime _SeedDate;
        private int _Tenant;
        private IAccountingContext _AccountingContext;

        public ReverseEngineerLedgerTransactionService(DateTime seedDate, int currTenant)
        {
            // TODO: Complete member initialization
            this._SeedDate = seedDate;
            this._Tenant = currTenant;
        }
        
        public static void CheckLastMonth(int subtractMonths)
        {
            if (subtractMonths > 0)
            {
                subtractMonths= -1 * subtractMonths;
            }
            var tenantList = new List<int>() { 1 };
            //DateTime.Now.Subtract
            var seedDate = DateTime.Now.AddMonths(subtractMonths);
            while (seedDate< DateTime.Now)
            {
                foreach (var currTenant in tenantList)
                {
                    var s = new ReverseEngineerLedgerTransactionService(seedDate, currTenant);
                    s.CheckDbIntegrity();    
                }
                
                seedDate=seedDate.AddMonths(1);
            }
        }
        
        public void CheckDbIntegrity()
        {
            var sw = Stopwatch.StartNew();
            string debugIt = "";
            string xml= "";
            try
            {


                bool stopJournalApproval = true;
                if (stopJournalApproval)
                {
                    TODO_StopJournalApproval();
                }
                var start = new DateTime(_SeedDate.Date.Year, _SeedDate.Date.Month, 1);
                var end = start.AddMonths(1).AddMinutes(-1);
                
                using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(25)))
                {
                    _AccountingContext = AccountingContext.GetContext(_Tenant);
                    (_AccountingContext as System.Data.Entity.DbContext).Database.CommandTimeout = 1200;

                    var qs = new LedgerTransactionQueryService(_AccountingContext);
                    var rowsReverseEngineerLedgerTransactionService = qs.GetReportCompareToJournalLine(start, end, _Tenant);

                    var journal_failed_notStreamedSlowQueue = GetJournal_failed_notStreamedSlowQueue(start, end);

                    var badrows = rowsReverseEngineerLedgerTransactionService.Concat(journal_failed_notStreamedSlowQueue).ToList();
                    //calcGLATotalByMonthFromLTrans = qs.GetLedgerTransactionSumFromTo(start, end, _Tenant);
                    CompareReport = new CompareReportM()
                    {
                        CompareReportName = "ReverseEngineerLedgerTransactionService",
                        Year = _SeedDate.Date.Year,
                        Month = _SeedDate.Date.Month,
                        rows = badrows,
                        Took = sw.Elapsed
                    };
                    //  xml = System.Text.Encoding.UTF8.GetString(LogitudeXmlSerializer.SerializeObject<CompareReportM>(r));
                    debugIt = xml;
                }

                
                Convert2DisplayNumber(CompareReport.rows, _Tenant);

            }
            finally
            {
               // Debug.WriteLine(debugIt);
            }
            //return xml;

        }

        private void Convert2DisplayNumber(List<JournalLineLedgerDTO> rows, int tenant)
        {
            if (rows==null)
            {
                return;
            }
            try
            {
                var AccountIdList= rows.Where(r => !string.IsNullOrWhiteSpace(r.AccountId)).Select(x => x.AccountId).Distinct().ToList();
                var repo = new GLAccountRepository(tenant);
                var res=repo.GetDisplayNumberList(AccountIdList.ToHashSet(), tenant);
                foreach (var item in rows)
                {
                    var display = res.FirstOrDefault(r => r.Key == item.AccountId); 
                    if (string.IsNullOrEmpty(display.Value)){
                        continue;
                    }
                    item.AccountDisplayNumber = display.Value;
                }
            }
            catch (Exception)
            {

                
            }
        }

        private List<JournalLineLedgerDTO> GetJournal_failed_notStreamedSlowQueue(DateTime start, DateTime end)
        {
            var JornalRepo = new JournalRepository(_AccountingContext);
            var qApprovedBetweenJournal = JornalRepo.GetQueryableBetween(_Tenant, start, end);

            ;

            var failedJournal = qApprovedBetweenJournal.Where(r => r.StatusCode == "4")
                .Select(g => new JournalLineLedgerDTO()
                {
                    CHANGE_TYPE = "העברה להנהח נכשלה",
                    JournalId = g.Id,
                    JournalLineNumber = 0,

                    AccountId = "",
                    CurrencyId = "",

                    LocalAmountCredit = 0,
                    LocalAmountDebit = 0,

                    ForeignAmountCredit = 0,
                    ForeignAmountDebit = 0,

                    AccountingDate = g.AccountingDate,
                    DueDate = g.AccountingDate,
                    DocumentDate = g.AccountingDate,

                            //DocumentDate =
                        });
            var notStreamedJournalSlowQueue = qApprovedBetweenJournal

                .Where(r => r.StatusCode == "6" || r.StatusCode == "2" || r.StatusCode == "3")//approved or Voided
                .Where(r => r.IsLedgerCreated == false)
                .Select(g => new JournalLineLedgerDTO()
                {
                    CHANGE_TYPE = "לפקודה אין תנעות",
                    JournalId = g.Id,
                    JournalLineNumber = 0,

                    AccountId = "",
                    CurrencyId = "",

                    LocalAmountCredit = 0,
                    LocalAmountDebit = 0,

                    ForeignAmountCredit = 0,
                    ForeignAmountDebit = 0,

                    AccountingDate = g.AccountingDate,
                    DueDate = g.AccountingDate,
                    DocumentDate = g.AccountingDate,

                            //DocumentDate =
                        })
                ;

            var failedJourna_notStreamedJournalSlowQueue = failedJournal.Concat(notStreamedJournalSlowQueue).ToList();
            return failedJourna_notStreamedJournalSlowQueue;
        }

        string GetSql()
        {

            return @"select * from 
( select  JournalId,JournalLineNumber ,AccountId,  CurrencyId ,Sum(LocalAmountCredit) LocalAmountCredit,sum(LocalAmountDebit) LocalAmountDebit , sum(ForeignAmountCredit) ForeignAmountCredit , sum(ForeignAmountDebit) ForeignAmountDebit from LedgerTransactions where AccountingDate >= '2016-08-01' and   AccountingDate <= '2016-08-02' group by JournalId,JournalLineNumber ,AccountId,  CurrencyId )
MyTrans ,
(

select  JournalId,line ,CreditAccountId  AccountId,  CurrencyId ,LocalAmount LocalAmountCredit,0 LocalAmountDebit , ForeignAmount ForeignAmountCredit , 0 ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where AccountingDate >= '2016-08-01' and   AccountingDate <= '2016-08-02'
and JournalActionTypes.Code =1
union
select  JournalId,line ,DebitAccountId  AccountId,  CurrencyId ,0 LocalAmountCredit,LocalAmount LocalAmountDebit , 0 ForeignAmountCredit , ForeignAmount ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where AccountingDate >= '2016-08-01' and   AccountingDate <= '2016-08-02'
and JournalActionTypes.Code =2

union
select  JournalId,line ,CreditAccountId  AccountId,  CurrencyId ,LocalAmount LocalAmountCredit,0 LocalAmountDebit , ForeignAmount ForeignAmountCredit , 0 ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where AccountingDate >= '2016-08-01' and   AccountingDate <= '2016-08-02'
and JournalActionTypes.Code =4

union
(
select  JournalId,line ,DebitAccountId  AccountId,  CurrencyId ,0 LocalAmountCredit, round(LocalAmount/1.18,2)  as LocalAmountDebit , 0 ForeignAmountCredit, round(ForeignAmount/1.18,2) ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where AccountingDate >= '2016-08-01' and   AccountingDate <= '2016-08-02'
and JournalActionTypes.Code =4
)
union
(
select  JournalId,line ,'1-30'  AccountId,  CurrencyId ,0 LocalAmountCredit, round(LocalAmount - (LocalAmount/1.18),2)  as LocalAmountDebit , 0 ForeignAmountCredit, round(ForeignAmount- (ForeignAmount/1.18),2) ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where AccountingDate >= '2016-08-01' and   AccountingDate <= '2016-08-02'
and JournalActionTypes.Code =4
) --)  a order by JournalId,line ,AccountId,  CurrencyId 
) 
MyAcc
where 
MyTrans.JournalId= MyAcc.JournalId and 
MyTrans.JournalLineNumber= MyAcc.Line and 
MyTrans.AccountId= MyAcc.AccountId and 
MyTrans.CurrencyId= MyAcc.CurrencyId 
and  
(
MyTrans.LocalAmountCredit <> MyAcc.LocalAmountCredit  
or MyTrans.LocalAmountDebit <> MyAcc.LocalAmountDebit
or MyTrans.ForeignAmountCredit <> MyAcc.ForeignAmountCredit  
or MyTrans.ForeignAmountDebit <> MyAcc.ForeignAmountDebit
)
";
            return
@"select  * from LedgerTransactions where JournalId='1-79' 

select  JournalId,JournalLineNumber ,AccountId,  CurrencyId ,Sum(LocalAmountCredit) LocalAmountCredit,sum(LocalAmountDebit) LocalAmountDebit , sum(ForeignAmountCredit) ForeignAmountCredit , sum(ForeignAmountDebit) ForeignAmountDebit from LedgerTransactions where JournalId='1-79' group by JournalId,JournalLineNumber ,AccountId,  CurrencyId 
select  JournalActionTypes.Code ,JournalActionTypes.EnglishName ,  JournalLines.*  from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id where JournalId='1-79' 


select * from (
select  JournalId,line ,CreditAccountId  AccountId,  CurrencyId ,LocalAmount LocalAmountCredit,0 LocalAmountDebit , ForeignAmount ForeignAmountCredit , 0 ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where JournalId='1-79'  
and JournalActionTypes.Code =1
union
select  JournalId,line ,DebitAccountId  AccountId,  CurrencyId ,0 LocalAmountCredit,LocalAmount LocalAmountDebit , 0 ForeignAmountCredit , ForeignAmount ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where JournalId='1-79'  
and JournalActionTypes.Code =2

union
select  JournalId,line ,CreditAccountId  AccountId,  CurrencyId ,LocalAmount LocalAmountCredit,0 LocalAmountDebit , ForeignAmount ForeignAmountCredit , 0 ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where JournalId='1-79'  
and JournalActionTypes.Code =4

union
(
select  JournalId,line ,DebitAccountId  AccountId,  CurrencyId ,0 LocalAmountCredit, round(LocalAmount/1.18,2)  as LocalAmountDebit , 0 ForeignAmountCredit, round(ForeignAmount/1.18,2) ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where JournalId='1-79'  
and JournalActionTypes.Code =4
)
union
(
select  JournalId,line ,'1-30'  AccountId,  CurrencyId ,0 LocalAmountCredit, round(LocalAmount - (LocalAmount/1.18),2)  as LocalAmountDebit , 0 ForeignAmountCredit, round(ForeignAmount- (ForeignAmount/1.18),2) ForeignAmountDebit 
from JournalLines join JournalActionTypes on JournalLines.Tenant = JournalActionTypes.Tenant and  JournalLines.ActionCode = JournalActionTypes.Id 
where JournalId='1-79'  
and JournalActionTypes.Code =4
)) a order by JournalId,line ,AccountId,  CurrencyId "
                ;
        }

        private void TODO_StopJournalApproval()
        {
            //throw new NotImplementedException();
        }


        public CompareReportM CompareReport { get; set; }
    }
    public class CompareReportM
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string CompareReportName { get; set; }
        //public TimeSpan Took { get; set; }
        [XmlIgnore] // This attribute prevents the property from being serialized by the XmlSerializer
        public TimeSpan Took
        {
            get
            {
                return XmlConvert.ToTimeSpan(Took_string);
            }
            set
            {
                Took_string = XmlConvert.ToString(value);
            }
        }

        // Substitute property
        [Browsable(false)] // Hides the property from, for example, a PropertyGrid
        [XmlElement("Took")] // Overrides the default name of property. 
        // In this case ReadTimeout_string will become ReadTimeout 
        public string Took_string { get; set; }

        public List<JournalLineLedgerDTO> rows { get; set; }

        public List<Data.Repositories.GLAccountTotalByMonthsDTO> GLAccountTotalByMonthsList { get; set; }

        public List<GLAccountBalanceDTO> GLAccountBalanceList { get; set; }
        public List<GLAccountBalanceDTO> TotalOpenReconciliation { get; set; }
        public List<InterestReportDiff> InterestReportDiffList { get; set; }
    }
    
}
