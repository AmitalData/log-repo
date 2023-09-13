 
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
using System.Data.Entity;

namespace Logitude.Accounting.Data.Repositories
{
    public partial class GLAccountTotalByMonthRepository : IRepository<GLAccountTotalByMonth>
   {
        
        
		public List<GLAccountTotalByMonth> GetMulti(EntityKeyFields entityKeys)
        {
            GLAccountTotalByMonthKeys gLAccountTotalByMonthKeys = entityKeys as GLAccountTotalByMonthKeys;

            return (from a in context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == "1") //GLAccountTotalDateTypeValues.Accoutingdate)
                    where a.AccountId == gLAccountTotalByMonthKeys.AccountId
                    select a).ToList();
        }

        //public decimal? GLAccountTotalLocalAmountBalanceTillMonth(string accountId, int year, int month, int tenant)
        //{           
        //    var mylist = (from a in context.GLAccountTotalByMonths
        //                  where a.AccountId == accountId && a.Year == year && a.Month < month && a.Tenant == tenant
        //            select new {a.Currency,a.AccountId,a.LocalAmountCredit,a.LocalAmountDebit}).ToList();          
        //    var totalSum  = mylist.Sum(c => c.LocalAmountCredit - c.LocalAmountDebit);

        //    return totalSum;
        //}

        public List<GLAccountTotalByMonth> GetMounthTotals(int year, int month, int tenant)
        {
            var pocos = GetQuaryableMonthTotals(year, month, tenant, "1"/*GLAccountTotalDateTypeValues.Accoutingdate*/).ToList();
            return pocos;
        }
        public decimal GetLocalOpenBalanceForYearDateTypeCode(string DateTypeCode, string accountId, int year, int tenant)
        {
            decimal LocalOpenBalanceForYear = (from tot in
                                                   context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == DateTypeCode) //GLAccountTotalDateTypeValues.Accoutingdate)
                        .Where(a => a.Tenant == tenant && a.Year < year && a.AccountId == accountId)
                                               group tot by 1 into g
                                               select g.Sum(tot => tot.LocalAmountDebit - tot.LocalAmountCredit)

                        ).FirstOrDefault();
            return LocalOpenBalanceForYear;
        }
        public decimal GetLocalOpenBalanceForYear(string accountId, int year, int tenant)
        {
            decimal LocalOpenBalanceForYear = (from tot in
                                                   context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == "1" ) //GLAccountTotalDateTypeValues.Accoutingdate)
                        .Where(a => a.Tenant == tenant && a.Year < year && a.AccountId == accountId)
                        group tot by 1 into g
                        select g.Sum( tot=> tot.LocalAmountDebit -tot.LocalAmountCredit)

                        ).FirstOrDefault();
            return LocalOpenBalanceForYear;
        }

        public IQueryable<EntityPOCOs.GLAccountTotalByMonth> GetQuaryableMonthTotals(int year, int month, int tenant,string DateTypeCode)
        {
            return context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == DateTypeCode) //GLAccountTotalDateTypeValues.Accoutingdate)
                .Where(a => a.Tenant == tenant && a.Year == year && a.Month == month);
        }

        public decimal? GLAccountTotalLocalAmountBalanceByCurrency(string accountId, int year, int month, int tenant, string currencyId)
        {
            IQueryable<GLAccountTotalByMonth> GLAccountTotalByMonthsQuery;

            GLAccountTotalByMonthsQuery = context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == "1") //GLAccountTotalDateTypeValues.Accoutingdate)
                .Where(a => a.Tenant == tenant && a.AccountId == accountId && a.Year == year && a.Month < month);

            if (!string.IsNullOrWhiteSpace(currencyId))
            {
                GLAccountTotalByMonthsQuery = GLAccountTotalByMonthsQuery.Where(a => a.CurrencyId == currencyId);
            }

            var mylist = GLAccountTotalByMonthsQuery.Select(a => new { a.LocalAmountCredit, a.LocalAmountDebit }).ToList();

            var totalSum = mylist.Sum(c => c.LocalAmountCredit - c.LocalAmountDebit);
             
          
            return totalSum;
        }



        public List<CurrencySum> GLAccountTotalByMonth(string accountId, int year, int month, int tenant)
        {
            var mysumlist = (from r in context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == "1") //GLAccountTotalDateTypeValues.Accoutingdate)
                             where r.AccountId == accountId && (r.Year == year && r.Month <= month || r.Year < year) && r.Tenant == tenant
                             group r by new
                             {
                                 r.CurrencyId
                             } into g
                             select new CurrencySum
                             {
                                AccountId = accountId,
                                CurrencyId = g.Key.CurrencyId,
                                ForeignAmountCredit = g.Sum(x => x.ForeignAmountCredit),
                                ForeignAmountDebit = g.Sum(x => x.ForeignAmountDebit),
                                LocalAmountCredit = g.Sum(x => x.LocalAmountCredit),
                                LocalAmountDebit = g.Sum(x => x.LocalAmountDebit)
                             }).ToList();
            return mysumlist;
        }

        public IQueryable<GLAccountAndMoreDTO> QAsGLAccountBalanceInLocalCurrency(int tenant)
        {
            return (
                from tot in
                    context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == "1") // GLAccountTotalDateTypeValues.Accoutingdate
                where tot.Tenant == tenant
                group tot by tot.AccountId into g
                select new GLAccountAndMoreDTO
                {
                    Id = g.Key,
                    BalanceInLocalCurrency = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit)
                }
                 );
        }

        public IQueryable<GLAccountTotalByMonth> GetQTotalByDateTypeCodeUntil(int tenant, DateTime utillMonth,
            //IEnumerable<string> listBatch,
            IQueryable<string> listOfAccId, string dateTypeCode)
        {

            var qTotalByDateTypeCodeUntil =
                (from totDueDate in context.GLAccountTotalByMonths
                 join accId in listOfAccId on totDueDate.AccountId equals accId
                 //where listBatch.Contains(totDueDate.AccountId)
                 where totDueDate.Tenant == tenant
                 
                 where totDueDate.DateTypeCode == dateTypeCode
                 where (totDueDate.Year < utillMonth.Year || (totDueDate.Year == utillMonth.Year && totDueDate.Month <= utillMonth.Month))
                 select totDueDate);
            return qTotalByDateTypeCodeUntil;

        }

        
            public List<CurrencySum> GetCurrencySumUntillNotIncludeDateType(IQueryable<string> accountIdList, string DateTypeCode, int year, int month, int tenant)
        {
            var aggr = new List<CurrencySum>();
            foreach (var accountIdListOf100 in accountIdList.Batch(100))
            {


                var mysumlist =
                    GetQAllCurrencySumUntillNotIncludeGByAccIdCurrId(year, month, tenant, DateTypeCode)
                    .Where(r => accountIdListOf100.Contains(r.AccountId))
                    .ToList();
                aggr.AddRange(mysumlist);
            }
            return aggr;
        }
        public List<CurrencySum> GetAllCurrencySumUntillNotInclude(IQueryable<string> accountIdList, int year, int month, int tenant)
        {
            var aggr = new List<CurrencySum>();
            foreach (var accountIdListOf100 in accountIdList.Batch(100))
            {


                var mysumlist = 
                    GetQAllCurrencySumUntillNotIncludeGByAccIdCurrId(year, month, tenant)
                    .Where( r=> accountIdListOf100.Contains(r.AccountId))
                    .ToList();
                aggr.AddRange(mysumlist);
            }
            return aggr;
        }

        public IQueryable<CurrencySum> GetQAllCurrencySumUntillNotIncludeGByAccIdCurrId(int year, int month, int tenant,string DateTypeCode ="1")
        {
            return (from r in context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == DateTypeCode) //GLAccountTotalDateTypeValues.Accoutingdate
                    //where accountIdListOf100.Contains(r.AccountId)
                    where r.Tenant == tenant
                    where r.Year < year ||
                    (r.Year == year && //r.Month <= month) 
                       r.Month < month)

                    group r by new
                    {
                        r.AccountId,
                        r.CurrencyId
                    } into g
                    select new CurrencySum
                    {
                        AccountId = g.Key.AccountId,
                        CurrencyId = g.Key.CurrencyId,
                        ForeignAmountCredit =g.Sum(x => x.ForeignAmountCredit),
                        ForeignAmountDebit = g.Sum(x => x.ForeignAmountDebit),
                        LocalAmountCredit = g.Sum(x => x.LocalAmountCredit),
                        LocalAmountDebit = g.Sum(x => x.LocalAmountDebit)
                    });
        }

        


        public IQueryable<CurrencySum> GetQAllCurrencySumRange(
            int fromYear, int fromMonth,
            int tillYear, int tillMonth,
            int tenant)
        {
            return (from r in context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == "1" ) //GLAccountTotalDateTypeValues.Accoutingdate)
                    //where accountIdListOf100.Contains(r.AccountId)
                    where r.Tenant == tenant


                    where r.Year > fromYear ||
                    (r.Year == fromYear && r.Month > fromMonth)

                    where r.Year < tillYear ||
                    (r.Year == tillYear && r.Month < tillMonth)


                    group r by new
                    {
                        r.AccountId,
                        r.CurrencyId
                    } into g
                    select new CurrencySum
                    {
                        AccountId = g.Key.AccountId,
                        CurrencyId = g.Key.CurrencyId,
                        ForeignAmountCredit = g.Sum(x => x.ForeignAmountCredit),
                        ForeignAmountDebit = g.Sum(x => x.ForeignAmountDebit),
                        LocalAmountCredit = g.Sum(x => x.LocalAmountCredit),
                        LocalAmountDebit = g.Sum(x => x.LocalAmountDebit)
                    });
        }

        public DateTime GLAccountMonthTotalsUpToDate(string accountId, DateTime toDate, int tenant)
        {
            int month = toDate.Month;
            int year = toDate.Year;

            GLAccountTotalByMonth query = (from r in context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == "1" ) //GLAccountTotalDateTypeValues.Accoutingdate)
                             where r.AccountId == accountId && (r.Year == year && r.Month <= month || r.Year < year) && r.Tenant == tenant
                                          select r).OrderByDescending(k => k.Year).ThenByDescending(k => k.Month).FirstOrDefault(); //new DateTime { Date = new DateTime(r.Year, r.Month, 1) });
            DateTime rv;
            if (query != null)
            {
                rv = new DateTime(query.Year, query.Month, 1); 
                rv = rv.AddMonths(1).AddDays(-1d); // end of month 
            }
            else
            {
                rv = DateTime.MinValue; // no months computed 
            }
            return rv;
        }

        public List<int> GetActiveTenantPerYear(int year)
        {
            var q=
            context
                .GLAccountTotalByMonths
                .Where(r => r.Year == year)
                .Select(r => r.Tenant).Distinct();
            return q.ToList();
             
        }

        public void DeleteControlByTanent(int tenant)
        {            
            List<GLAccountTotalByMonth> controller = 
                (from x in context. GLAccountTotalByMonths.Include("GLAccount")
                where  x.Tenant == tenant && x.GLAccount.IsControlAccount.Value && x.DateTypeCode == "1"
                select x).ToList();

            controller.ForEach(x => context.GLAccountTotalByMonths.Remove(x));
            SubmitChanges();
        }

        public void AddRange(List<GLAccountTotalByMonthsDTO> gLAccountTotalByMonths)
        {
            foreach (var  g in gLAccountTotalByMonths)
                context.GLAccountTotalByMonths.Add(new GLAccountTotalByMonth()
                {
                    AccountId = g.AccountId,
                    CurrencyId = g.CurrencyId,
                    DateTypeCode = g.DateTypeValue,
                    ForeignAmountCredit = g.ForeignAmountCredit,
                    ForeignAmountDebit = g.ForeignAmountDebit,
                    LocalAmountCredit = g.LocalAmountCredit,
                    LocalAmountDebit = g.LocalAmountDebit,
                    Month = g.Month,
                    Year = g.Year,
                    Tenant = g.Tenant
                });

            SubmitChanges();
        }

   }
    public class CurrencySum
   {
       public string AccountId { get; set; }

        public string AccountDisplayNumber { get; set; }
        
       public string CurrencyId { get; set; }

       public decimal ForeignAmountCredit { get; set; }
       public decimal ForeignAmountDebit { get; set; }
       public decimal LocalAmountCredit { get; set; }
       public decimal LocalAmountDebit { get; set; }
       
       

    }
   

    public class GLAccountTotalByMonthsDTO
    {
        public GLAccountTotalByMonthsDTO()
        {

        }
     
       

        

        public int Tenant { get; set; }
        
        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public string AccountDisplayNumber { get; set; }
        

        public string CurrencyId { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        
        
        public decimal LocalAmountDebit { get; set; }
        public decimal LocalAmountCredit { get; set; }
        
        public decimal ForeignAmountDebit { get; set; }
        public decimal ForeignAmountCredit { get; set; }

        public string CHANGE_TYPE { get; set; }
        public string DateTypeValue { get; set; }
    }


    public class GLAccountTotalByMonthsDTOAging///: GLAccountTotalByMonthsDTO
    {



        public int Tenant { get; set; }

        public string AccountId { get; set; }
        public string CurrencyId { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }


        public decimal LocalAmountDebit { get; set; }
        public decimal LocalAmountCredit { get; set; }

        public decimal ForeignAmountDebit { get; set; }
        public decimal ForeignAmountCredit { get; set; }

        public string CHANGE_TYPE { get; set; }



        public string GLAccountCurrencyId { get; set; }
        public int TotalOpenTransactions { get;  set; }
    }

    public class CurrencySumOpenAmount
    {
        public string AccountId { get; set; }
        public string OpenAmountCurrencyId { get; set; }
        public decimal OpenAmount { get; set; }
    }

}


