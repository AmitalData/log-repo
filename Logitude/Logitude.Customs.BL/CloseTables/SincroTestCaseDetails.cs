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
                    Code = "2755Payment",
                    Name = "  ללא חתימה הגשת תשלום",
                    Entity="DeclarationPayment",
                    AvoidSign= true
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
                    Param1= @"{ ""RequestNumber"" :""1"" ,  ""Content32"" :""1"" , 
                              ""Content29"" :""test 29"" , ""Content27"" :""test 27"", ""status"" :""13"" , ""error"" :""true"" , ""constrain"" :""false"" }",
                                                   
                                               
                    MainInterfaceCode="5117",
                },
                                  
    
                new SincroTestCaseDetail()
                {
                    Code = "190Sincro",
                    Name = "בדיקות פיזיות	",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{ """":""""}",
                    Param2= @"{ ""availability"":""2020-02-04T09:15:03.1085624"" }",
                    MainInterfaceCode="190",
                },
                new SincroTestCaseDetail()
                {
                    Code = "196Sincro",
                    Name = "סיום בדיקה פיזית ",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{ ""checkId"":""""}",
                    Param2= @"{ }",
                    MainInterfaceCode="196",
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
        //public FakeServiceEnum FakeService { get; internal set; }
        public string MainInterfaceCode { get; internal set; }
        public bool AvoidSign { get; internal set; }
    }
    public enum FakeServiceEnum
    {
        DCA190SincroService
    }
}