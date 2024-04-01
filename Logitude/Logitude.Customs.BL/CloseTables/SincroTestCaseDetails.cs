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
                     Param1= @"{ ""withSignature"":""false"" , ""amount"":""99""}",

                },

                  new SincroTestCaseDetail()
                {
                    Code = "1234",
                    Name = "תשובה להמכלה",
                    Entity="CONT",
                     Param1= @"{ ""IsValid"":""true""}",

                },
                 new SincroTestCaseDetail()
                {
                    Code = "8373",
                    Name = "(8373)שחזור נתוני הצהרה",
                    Entity="RetrieveDeclaration",

                },
                new SincroTestCaseDetail()
                {
                    Code = "2754Payment",
                    Name = "  הגשת תשלום",
                    Entity="DeclarationPayment",
                    AvoidSign= true

                },
                new SincroTestCaseDetail()
                {
                    Code = "2755",
                    Name = "  ללא חתימה הגשת תשלום",
                    Entity="DeclarationPayment",
                    AvoidSign= true
                },

                new SincroTestCaseDetail()
                {
                    Code = "2754Constraint",
                    Name = "הצהרת יבוא אילוץ",
                    Entity="Declaration",
                    Param1= @"{ ""code"":""4589"" , ""documentSectionCode"" : ""30B"", ""tagId"" :""D024""}",
                },

                new SincroTestCaseDetail()
                {
                    Code = "2892",
                    Name = "(2892)תיקון הצהרה",
                    Entity="Declaration",
                    Param1= @"{ ""Error"":""false""}",
MainInterfaceCode ="2892"
                },
                               new SincroTestCaseDetail()
                {
                    Code = "1170",
                    Name = "(1170)מסר מצהר- מניפסט",
                    Entity="Manifest",
                    Param1= @"{ ""manifestCargoStatusCode"":""1""  }",
                    MainInterfaceCode ="1170"
                },
                        new SincroTestCaseDetail()
                {
                    Code = "5002",
                    Name = "(5002)ביטול הצהרה",
                    Entity="DeclarationCancellation",
                    Param1= @"{ ""Error"":""false""}",
MainInterfaceCode ="5002"
                },
                 new SincroTestCaseDetail()
                {
                    Code = "5117",
                    Name = "(הצהרת תקן (5117 ",
                    IsDCA= true,
                    Entity="Declaration",
                    Param1= @"{ ""RequestNumber"" :""1"" ,  ""Content32"" :""1"" , 
                              ""Content29"" :""test 29"" , ""Content27"" :""test 27"",  ""Content16"" :""PaymentID"" ,""status"" :""13"" , ""error"" :""true"" ,
                               ""constrain"" :""false"" , ""amendmentDocumentDetails"" :""false""  }",


                    MainInterfaceCode="5117",
                },


                    new SincroTestCaseDetail()
                {
                    Code = "5118",
                    Name = "(מענה לביטול הצהרה (5118 ",
                    IsDCA= true,
                    Entity="Declaration",
                    Param1= @"{ ""RequestNumber"" :""1"" ,  ""Content33"" :""5"" , 
                              ""Content22"" :""test 22"" , ""Content36"" :""test 36"" , ""Content37"" :""10/01/2020 16:30:30"",
                                 ""DeclarationStatusID"" :""13"" , ""ProceduralFaultMsg "" :""false"" ,
                              ""RequiredDocumentDetails "" :""false""  }",


                    MainInterfaceCode="5118",
                },

                    new SincroTestCaseDetail()
                {
                    Code = "8227",
                    Name = "(מסמך נדרש (8227 ",
                    IsDCA= true,
                    Entity="Declaration",
                    Param1= @"{ ""documentId"" :"""" ,  ""requiredDocumentMessageType"" :""1"" , 
                              ""typeId"" :""380"" , ""entityType"" :""12414"" , ""entityIdKey2"" :"""",
                                 ""entityIdKey3"" :""""  }",


                    MainInterfaceCode="8227",
                },
                new SincroTestCaseDetail()
                {
                    Code = "8228",
                    Name = "(אימות מסמך נדרש (8228",
                    IsDCA= true,
                    Entity="Declaration",
                    Param1= @"{ ""documentId"" :"""" ,  ""rejectVerificationReason"" :"""" , ""verificationDecisionType"" :""2"" , ""rejectVerificationRemark"" :""FAKE"" , 
                              ""typeId"" :""380"" , ""entityType"" :""12414"" , ""entityIdKey2"" :"""",
                                 ""entityIdKey3"" :""""  }",


                    MainInterfaceCode="8228",
                },
                new SincroTestCaseDetail()
                {
                    Code = "190",
                    Name = "בדיקות פיזיות	",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{ ""checkId"":"""",""ActionTypeCode"" :"""" }",
                    Param2= @"{}",
                    MainInterfaceCode="190",
                },
                new SincroTestCaseDetail()
                {
                    Code = "8215SincroConstraintApprove",
                    Name = "אישור אילוץ-אישור ללא תנאי",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""constraintId"":""18""}",
                    Param2= @"{}",
                    MainInterfaceCode="8215",
                },
                   new SincroTestCaseDetail()
                {
                    Code = "8215SincroConstraintDeny",
                    Name = "אישור אילוץ-דחייה",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""constraintId"":""18""}",
                    Param2= @"{}",
                    MainInterfaceCode="8215",
                },
                new SincroTestCaseDetail()
                {
                    Code = "196",
                    Name = "סיום בדיקה פיזית ",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{ ""checkId"":""""}",
                    Param2= @"{ }",
                    MainInterfaceCode="196",
                },
                new SincroTestCaseDetail()
                {
                    Code = "2470",
                    Name = "מסר התרה",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""MasterLevel"":""true"",""CourierLevel"":""false"",""ReleaseMessageCode"":""1""}",
                    Param2= @"{}",
                    MainInterfaceCode="2470",
                },
                new SincroTestCaseDetail()
                {
                    Code = "3050",
                    Name = "הודעה לסוכן על הוראת תשלום",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""paymentStatus"":""3"",  ""paymentProcess"" :""1""}",
                    Param2= @"{}",
                    MainInterfaceCode="3050",
                },

                   new SincroTestCaseDetail()
                {
                    Code = "8211",
                    Name = "מסר פתיחת בטוחה",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""Test"":""""}",
                    Param2= @"{}",
                    MainInterfaceCode="8211",
                },
                   new SincroTestCaseDetail()
                {
                    Code = "3052",
                    Name = " תשלום הוראה ",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""PaymentId"":""1""}",
                    Param2= @"{}",
                    MainInterfaceCode="3052",
                },
                   new SincroTestCaseDetail()
                {
                    Code = "5110",
                    Name = "יידוע על קבלת פיקדון ופתיחת תיק פיקדון",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""numeral"":""1""}",
                    Param2= @"{}",
                    MainInterfaceCode="5110",
                },
                   new SincroTestCaseDetail()
                {
                    Code = "2030",
                    Name = " דרישה לתשלום פיקדון",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""numeral"":""1""}",
                    Param2= @"{}",
                    MainInterfaceCode="2030",
                },
                   new SincroTestCaseDetail()
                {
                    Code = "8251",
                    Name = "8251",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""Test"":""""}",
                    Param2= @"{}",
                    MainInterfaceCode="8251",
                },
                         new SincroTestCaseDetail()
                {
                    Code = "5101O_I",
                    Name = "הודעה לסוכן",
                    IsDCA = true,
                    Entity="Declaration",
                    Param1= @"{""entityType"" :"""" ,  ""entityIdKey1"" :"""" , ""msgCode"" :"""" , ""msgString"" :""""}",
                    Param2= @"{}",
                    MainInterfaceCode="5101O_I",

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