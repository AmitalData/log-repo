using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables
{
    public class SincroTestCaseDetails
    {
        public List<SincroTestCaseDetail> GetAllSincroTestCaseDetails()
        {
            var all = new List<SincroTestCaseDetail>()
            {
                new SincroTestCaseDetail()
                {
                    Code = "2754Valid",
                    Name = "הצהרת יבוא תקינה",
                    Entity="Declaration",
                },
                new SincroTestCaseDetail()
                {
                    Code = "2754Payment",
                    Name = "  הגשת תשלום",
                    Entity="Declaration",
                },
                new SincroTestCaseDetail()
                {
                    Code = "2754Constraint",
                    Name = "הצהרת יבוא אילוץ",
                    Entity="Declaration",
                    Param1= @"{ ""ConstraintName"":""MissingX"", ""MissingX"": ""Hello Word"" }"
                },
                 new SincroTestCaseDetail()
                {
                    Code = "5117SincroFix",
                    Name = "הצהרת תקן5117 ",
                    IsDCA= true,
                    Entity="Declaration",
                    Param1= @"{ ""Fix"":""AAA"" }"
                },
                                  new SincroTestCaseDetail()
                {
                    Code = "5117SincroCancel",
                    Name = "הצהרת בטל5117 ",
                    IsDCA= true,
                    Entity="Declaration",
                    Param1= @"{ ""cancel"":""AAA"" }"
                }


            };
            return all;

        }
    }
    public class SincroTestCaseDetail
    {
        public string Code { get; set; }
        public string Name { get; set; }    
        public string Entity { get; set; }
        public bool IsDCA { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
    }
}