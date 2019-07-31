using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class GLAccountWithholdingTaxQueryService
    {

        public int GetMaxLineNumber(string glAccountId, int tenant)
        {
            return repository.GetMaxLineNumber(glAccountId, tenant);
        }

        public GLAccountWithholdingTaxPM GetAccountWithholdingTaxPMByglAccountAndDate(string glaccountId, DateTime? date)
        {
            GLAccountWithholdingTax poco=(from a in context.GLAccountWithholdingTax
                    where a.GLAccountId == glaccountId && !a.Inactive && ((a.FromDate < date || a.FromDate == date) && (a.ToDate > date || a.ToDate== date) )  
                    select a).FirstOrDefault();

            var result = GetEntityPM(poco);
            return result;

        }

        //public GLAccountWithholdingTaxPM GetAccountWithholdingTaxPMByGLAccountFromTo(string glaccountId, DateTime? fromDate, DateTime? toDate)
        //{
        //    GLAccountWithholdingTax poco = (from a in context.GLAccountWithholdingTax
        //                                    where a.GLAccountId == glaccountId && !a.Inactive && (fromDate.HasValue && a.FromDate == fromDate.Value) && (toDate.HasValue && a.ToDate == toDate.Value)
        //                                    select a).FirstOrDefault();

        //    var result = GetEntityPM(poco);
        //    return result;
        //}


        //public List<GLAccountWithholdingTaxPM> GetOverlappingAccountWithholdingTaxPMsByGLAccountFromTo(string glaccountId, DateTime? fromDate, DateTime? toDate)
        //{
        //    List<GLAccountWithholdingTax> pocos = (from a in context.GLAccountWithholdingTax
        //                                           where a.GLAccountId == glaccountId && !a.Inactive && !(fromDate.HasValue && a.ToDate < fromDate) && !(toDate.HasValue && a.FromDate > toDate) 
        //                                           select a).ToList<GLAccountWithholdingTax>();

        //    List<GLAccountWithholdingTaxPM> result = new List<GLAccountWithholdingTaxPM>();
        //    foreach (var poco in pocos)
        //    {
        //        result.Add(GetEntityPM(poco));
        //    }

        //    return result;

        //}


        //public List<GLAccountWithholdingTaxPM> GetAccountWithholdingTaxPMListByGLAccountFromDate(string glaccountId, DateTime? date)
        //{
        //    List<GLAccountWithholdingTax> pocos = (from a in context.GLAccountWithholdingTax
        //                                    where a.GLAccountId == glaccountId && !a.Inactive && ((a.FromDate < date && (a.ToDate > date || a.ToDate == date)) || a.FromDate == date || a.FromDate > date)
        //                                    select a).ToList<GLAccountWithholdingTax>();

        //    List<GLAccountWithholdingTaxPM> result = new List<GLAccountWithholdingTaxPM>();
        //    foreach (var poco in pocos)
        //    {
        //        result.Add(GetEntityPM(poco));
        //    }
            
        //    return result;

        //}

        //public int GetNextLineNumberByGLAccount(string glaccountId)
        //{
        //    List<int> numbers = (from a in context.GLAccountWithholdingTax
        //                   where a.GLAccountId == glaccountId
        //                   select a.LineNumber).ToList<int>();
        //    int nextLineNumber = 0;
        //    if (numbers == null || numbers.Count == 0)
        //    {
        //        nextLineNumber = 1;
        //    }
        //    else
        //    {
        //        nextLineNumber = 1 + numbers.Max();
        //    }

        //    return nextLineNumber;
        //}
    }
}
