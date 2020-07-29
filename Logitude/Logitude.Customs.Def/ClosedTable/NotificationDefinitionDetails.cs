using Logitude.Customs.Data.EntityPOCOs;
using System.Collections.Generic;

namespace Logitude.Customs.Def.ClosedTable
{// moran 24.7.14 - Task 6922
    public class NotificationDefinitionDetails : NotificationDefinition, ICloseTable<NotificationDefinition, NotificationDefinitionDetails>
    {
        public enum NotificationEnum
        {
            POR, POC, POU, POP, _02
        }

        public NotificationDefinitionDetails()
        {
        }
        public NotificationDefinitionDetails(NotificationDefinition notificationDefinition)
        {
            this.Code = notificationDefinition.Code;
            this.EnglishName = notificationDefinition.EnglishName;
            this.LocalName = notificationDefinition.LocalName;
            this.AssigneeNotificationTypeCode = notificationDefinition.AssigneeNotificationTypeCode;

        }



        public List<NotificationDefinitionDetails> GetAll()
        {

            var all = new List<NotificationDefinitionDetails>();
            all.Add(new NotificationDefinitionDetails()
            {
                Code = "3050N", //NotificationEnum.POR.ToString(),
                EnglishName = "Payment Order Created",
                LocalName = "הוראת תשלום נוצרה",
                AssigneeNotificationTypeCode = "A",
                // IsCustomerView = true

            });
            all.Add(new NotificationDefinitionDetails()
            {
                Code = "3050C", //NotificationEnum.POC.ToString() ,
                EnglishName = "Payment Order Cancelled",
                LocalName = "הוראת תשלום בוטלה",
                AssigneeNotificationTypeCode = "I",
                //IsCustomerView = true
            });
            all.Add(new NotificationDefinitionDetails()
            {
                Code = "3050U", //NotificationEnum.POU.ToString(),
                EnglishName = "Payment Order Updated",
                LocalName = "הוראת תשלום עודכנה",
                AssigneeNotificationTypeCode = "A",
                //IsCustomerView = true
            });
            all.Add(new NotificationDefinitionDetails()
            {
                Code = "3052P", //NotificationEnum.POP.ToString(),
                EnglishName = "Payment Order Paid",
                LocalName = "הוראת תשלום שולמה",
                AssigneeNotificationTypeCode = "I",
                // IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // moran 11.8.14 - Task 7086 
            {
                Code = "2470N",
                EnglishName = "Declaration Release",
                LocalName = "הצהרה הותרה",
                AssigneeNotificationTypeCode = "I",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // moran 11.8.14 - Task 7086 
            {
                Code = "2470C",
                EnglishName = "Declaration Release Cancelation",
                LocalName = "להצהרה בוטלה ההתרה",
                AssigneeNotificationTypeCode = "A",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // moran 07.07.15 - Task 14260 
            {
                Code = "2470P",
                EnglishName = "Pre clearance",
                LocalName = "הודעה מוקדמת לסוכן מכס",
                AssigneeNotificationTypeCode = "I",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // Yuval Chalup 17.01.2018 - Task 36118 
            {
                Code = "2470A",
                EnglishName = "Release When Arrived",
                LocalName = "מאושר להתרה לאחר הגשת טובין",
                AssigneeNotificationTypeCode = "I",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // moran 11.8.14 - Task 7092 
            {
                Code = "190N",
                EnglishName = "Physical Checks Created",
                LocalName = "בדיקה פיזית נוצרה",
                AssigneeNotificationTypeCode = "A",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // moran 11.8.14 - Task 7092 
            {
                Code = "190U",
                EnglishName = "Physical Checks Updated",
                LocalName = "בדיקה פיזית עודכנה",
                AssigneeNotificationTypeCode = "A",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // moran 11.8.14 - Task 7092 
            {
                Code = "196E",
                EnglishName = "Physical Checks Ended",
                LocalName = "בדיקה פיזית הסתיימה",
                AssigneeNotificationTypeCode = "I",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // moran 11.8.14 - Task 7092 
            {
                Code = "190C",
                EnglishName = "Physical Checks Cancelled",
                LocalName = "בדיקה פיזית בוטלה",
                AssigneeNotificationTypeCode = "I",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // moran 9.9.14 - Task 7885 
            {
                Code = "70N",
                EnglishName = "Warehouse Approved Storage",
                LocalName = "בקשת אחסנה אושרה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 31/05/15 Task 13700
            {
                Code = "70C",
                EnglishName = "Warehouse Rejected Storage",
                LocalName = "בקשת אחסנה נדחתה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // moran 15.9.14 - Task 7918 
            {
                Code = "8215A",
                EnglishName = "Constraint Approved by Customs",
                LocalName = "אילוץ אושר במכס",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // moran 18.9.14 - Task 7918 
            {
                Code = "8215D",
                EnglishName = "Constraint Declined by Customs",
                LocalName = "אילוץ נדחה עʺי המכס",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // moran 18.9.14 - Task 7918 
            {
                Code = "8215C",
                EnglishName = "Constraint Conditional Approval",
                LocalName = "אילוץ מאושר בתנאי",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // moran 16.9.14 - Task 7995 
            {
                Code = "8227N",
                EnglishName = "Document Request By Customs",
                LocalName = "מסמך נדרש עʺי המכס",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // moran 16.9.14 - Task 7995 
            {
                Code = "8227D",
                EnglishName = "Delete Document",
                LocalName = "ביטול דרישת מסמך",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // moran 7.10.14 - Task 8066 
            {
                Code = "5101N",
                EnglishName = "Agent Notification",
                LocalName = "הודעה לסוכן",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // moran 28.4.19 - Task 51282 
            {
                Code = "5101A",
                EnglishName = "Correspondence to Cargo Split Rejected",
                LocalName = "התכתבות דחיית פיצול מטען",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // moran 28.4.19 - Task 51282 
            {
                Code = "5101E",
                EnglishName = "Correspondence to legality ransom",
                LocalName = "התכתבות כופר חוקיות",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // moran 28.4.19 - Task 51282 
            {
                Code = "5101R",
                EnglishName = "Correspondence to request for document",
                LocalName = "התכתבות לדרישת מסמך",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // mirit 25.8.19 - Task 55928 
            {
                Code = "5101F",
                EnglishName = "Collateral Demand",
                LocalName = "עמידה/אי עמידה בדרישה לבטוחה",
                AssigneeNotificationTypeCode = "I",
            });

            //all.Add(new NotificationDefinitionDetails() // Mirit 22.04.15 - Task 12713 //delete Task 20106
            //{
            //    Code = "5101I",
            //    EnglishName = "Agent Notification",
            //    LocalName = "הודעה לסוכן",
            //    AssigneeNotificationTypeCode = "I",
            //});

            all.Add(new NotificationDefinitionDetails() // Mirit 10.05.15 - Task 13106 
            {
                Code = "5101D",
                EnglishName = "Agent Notification - Docs Inspection",
                LocalName = "הודעה לסוכן - בקרת מסמכים",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 10.05.15 - Task 13106 
            {
                Code = "5101T",
                EnglishName = "Agent Notification - Check",
                LocalName = "הודעה לסוכן - תור בחינה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 10.05.15 - Task 13106 
            {
                Code = "5101C",
                EnglishName = "Agent Notification - Check",
                LocalName = "הודעה לסוכן - תור בחינת רשות",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 10.05.15 - Task 13106 
            {
                Code = "5101S",
                EnglishName = "Storage Request created",
                LocalName = "נוצרה בקשת אחסנה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 11.07.15 - Task 14662 
            {
                Code = "5101G",
                EnglishName = "Storage Request cancelled",
                LocalName = "בוטלה בקשת אחסנה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 11.07.15 - Task 14662 
            {
                Code = "5101U",
                EnglishName = "Storage Request updated",
                LocalName = "עודכנה בקשת אחסנה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // moran 17.1.17 - Task 21101 
            {
                Code = "5101B",
                EnglishName = "Agent Notification - Security Check",
                LocalName = "הודעה לסוכן - בדיקה בטחונית",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 13.11.17 - Task 34198 
            {
                Code = "5101M",
                EnglishName = "Agent Notification - BOL",
                LocalName = "הודעה לסוכן - התקבל מסר שטר מטען מאסטר",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 19.11.17 - Task 34182 
            {
                Code = "5101P",
                EnglishName = "Agent Notification - Load/Unload",
                LocalName = "הודעה לסוכן - אישור פריקה/טעינה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // moran 30.10.14 - Task 8327 
            {
                Code = "5009N",
                EnglishName = "Notification Regarding a Deficit",
                LocalName = "התראת מכס לגבי תיק גרעון",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 09/11/14 Task 8950 
            {
                Code = "3681C",
                EnglishName = "Vendor Defect",
                LocalName = "ליקוי ספק",
                AssigneeNotificationTypeCode = "A",
                //IsCustomerView = true
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 09/11/14 Task 8950 
            {
                Code = "3681I",
                EnglishName = "Vendor Update",
                LocalName = "עדכון ספק",
                AssigneeNotificationTypeCode = "I",
            });

            //<--- Yuval Chalup 17.11.2014 TASK-9089
            //all.Add(new NotificationDefinitionDetails()
            //{
            //    Code = "5107N",
            //    EnglishName = "Deficit Customs Answer",
            //    LocalName = "תשובת מכס בגין גרעון עצמי",
            //    AssigneeNotificationTypeCode = "A",
            //    //IsCustomerView = true
            //});
            //Yuval Chalup 17.11.2014 TASK-9089 --->

            all.Add(new NotificationDefinitionDetails() // Mirit 09/11/14 Task 1505 
            {
                Code = "8213N",
                EnglishName = "Approval/Denial of Reply to Cllateral",
                LocalName = "אישור/דחיה של מענה לדרישה לבטוחה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 09/11/14 Task 9119 
            {
                Code = "2020N",
                EnglishName = "Deposit Refund",
                LocalName = "הודעה על החזר פיקדון",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 09/11/14 Task 9121 
            {
                Code = "2000N",
                EnglishName = "Deposit Forfiet",
                LocalName = "הודעה על חילוט פיקדון",
                AssigneeNotificationTypeCode = "I",
            });



            all.Add(new NotificationDefinitionDetails() // Mirit 12/12/14 Task 8849 
            {
                Code = "3700N",
                EnglishName = "New Importer Declaration",
                LocalName = "תצהיר תקופתי חדש",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 12/12/14 Task 8849 
            {
                Code = "3700U",
                EnglishName = "Importer Declaration Updated",
                LocalName = "תצהיר תקופתי עודכן",
                AssigneeNotificationTypeCode = "I",
            });




            //<--- Yuval Chalup 17.11.2014 TASK-9089
            all.Add(new NotificationDefinitionDetails()
            {
                Code = "5107N",
                EnglishName = "Deficit Customs Answer",
                LocalName = "תשובת מכס בגין גרעון עצמי",
                AssigneeNotificationTypeCode = "A",
                //IsCustomerView = true
            });
            //Yuval Chalup 17.11.2014 TASK-9089 --->

            all.Add(new NotificationDefinitionDetails() // Mirit 06/01/14 Task 1788 
            {
                Code = "1812U",
                EnglishName = "Update Custom Guarantee Notification",
                LocalName = "עדכון ההודעה על ערבות",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 06/01/14 Task 1788 
            {
                Code = "1812N",
                EnglishName = "Custom Guarantee Notification",
                LocalName = "בקשה להמצאת ערבות",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // moran 11.1.15 - Task 9921  
            {
                Code = "3720N",
                EnglishName = "POA to Agent",
                LocalName = "הודעה על יפוי כוח שניתן לסוכן",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // moran 25.1.15 - Task 9967  
            {
                Code = "8400A",
                EnglishName = "Customs Approved Logistic Permit",
                LocalName = "מכס אישר היתר לוגיסטי",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // moran 25.1.15 - Task 9967  
            {
                Code = "8400C",
                EnglishName = "Customs Cancelled Logistic Permit",
                LocalName = "מכס ביטל היתר לוגיסטי",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 15.2.15 - Task 10780  
            {
                Code = "8218N",
                EnglishName = "New ProceduralFault",
                LocalName = "התקבל ליקוי מכס",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 15.2.15 - Task 10780  
            {
                Code = "8218U",
                EnglishName = "ProceduralFault Updated",
                LocalName = "עודכן ליקוי מכס",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 15.2.15 - Task 10781  
            {
                Code = "8219C",
                EnglishName = "ProceduralFault Cancelled",
                LocalName = "בוטל ליקוי מכס",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 23.2.15 - Task 11349  
            {
                Code = "5110N",
                EnglishName = "New Deposit Notice",
                LocalName = "הודעה על פתיחת תיק פיקדון",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 31.3.15 - Task 11405  
            {
                Code = "5018N",
                EnglishName = "Declaration Cancelled",
                LocalName = "ההצהרת יבוא בוטלה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 08/04/15 - Task 12278  
            {
                Code = "8211N",
                EnglishName = "Collerterals Request",
                LocalName = "דרישה לבטוחה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 08/04/15 - Task 12278  
            {
                Code = "8211U",
                EnglishName = "Collerterals Request",
                LocalName = "דרישה לבטוחה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 07/07/15 - Task 11406  
            {
                Code = "5117N",
                EnglishName = "Declaration Changed By Customs",
                LocalName = "בוצע תיקון הצהרה ע'י המכס",
                AssigneeNotificationTypeCode = "A",
            });


            all.Add(new NotificationDefinitionDetails()  
            {
                Code = "5117A",
                EnglishName = "Declaration Amendment Approved",
                LocalName = "תיקון הצהרה אושרה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails()
            {
                Code = "5117P",
                EnglishName = "Declaration Amendment Partial Approval",
                LocalName = "תיקון הצהרה אושרה חלקית",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails()
            {
                Code = "5117D",
                EnglishName = "Declaration Amendment Denial",
                LocalName = "תיקון הצהרה נדחתה",
                AssigneeNotificationTypeCode = "A",
            });


            all.Add(new NotificationDefinitionDetails()
            {
                Code = "5117C",
                EnglishName = "Declaration Amendment Denial",
                LocalName = "תיקון הצהרה בוטלה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails()
            {
                Code = "5117W",
                EnglishName = "Amendment Waiting for customs response",
                LocalName = "תיקון הצהרה ממתינה לטיפול מכס",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // moran 13.7.15 - Task 14521  
            {
                Code = "2754C",
                EnglishName = "Declaration Amendment Cancelled",
                LocalName = "יש להגיש הצהרה מחדש",
                AssigneeNotificationTypeCode = "A",
            });


            all.Add(new NotificationDefinitionDetails() // Mirit 10.08.15 - Task 15423 
            {
                Code = "8228A",
                EnglishName = "Required Document Verified",
                LocalName = "מסמך נדרש אומת",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 10.08.15 - Task 15423 
            {
                Code = "8228D",
                EnglishName = "Required Document Rejected",
                LocalName = "מסמך נדרש נדחה",
                AssigneeNotificationTypeCode = "A",
            });


            all.Add(new NotificationDefinitionDetails() // Mirit 10/11/15 Task 17714
            {
                Code = "60A",
                EnglishName = "Special activity Execution By Warehouse",
                LocalName = "פעולה מיוחדת בוצעה במחסן",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 10/01/16 Task 19722
            {
                Code = "10A",
                EnglishName = "Bonded Special Request Approved",
                LocalName = "אושרה בקשה לפעולה מיוחדת במחסן",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 10/01/16 Task 19722
            {
                Code = "10D",
                EnglishName = "Bonded Special Request Deny",
                LocalName = "נדחתה בקשה לפעולה מיוחדת במחסן",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 07/03/16 Task 20546
            {
                Code = "5115N",
                EnglishName = "Claim Message",
                LocalName = "הודעה לגבי תיק תביעה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 07/03/16 Task 20547
            {
                Code = "2300N",
                EnglishName = "Requested Doc for Claim",
                LocalName = "מסמך נדרש לתיק תביעה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() 
            {
                Code = "8374A",
                EnglishName = "Cargo Split Approved",
                LocalName = "בקשת פיצול מטען אושרה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails()
            {
                Code = "8374J",
                EnglishName = "Cargo Split Rejected",
                LocalName = "בקשת פיצול מטען נדחתה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails()
            {
                Code = "8374C",
                EnglishName = "Cargo Split Canceled",
                LocalName = "בקשת פיצול מטען בוטלה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails()
            {
                Code = "8374D",
                EnglishName = "Cargo Split Done",
                LocalName = "בוצע פיצול מטען",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails()
            {
                Code = "2753A",
                EnglishName = "Deposit Request bank account to refund",
                LocalName = "בקשה להשלמת פרטי החזר פקדון",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 07/03/16 Task 20546
            {
                Code = "5114N",
                EnglishName = "Acceptance/Rejection Claim Message",
                LocalName = "אישור/דחיית תביעה",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails() // Mirit 04/08/19 Task 53620
            {
                Code = "5108N",
                EnglishName = "Deficit Customs Answer",
                LocalName = "החלטה בגין גרעון",
                AssigneeNotificationTypeCode = "A",
            });

            all.Add(new NotificationDefinitionDetails()  
            {
                Code = "5117A",
                EnglishName = "Declaration Amendment Approved",
                LocalName = "תיקון הצהרה אושרה",
                AssigneeNotificationTypeCode = "I",
            });

            all.Add(new NotificationDefinitionDetails()
            {
                Code = "5117P",
                EnglishName = "Declaration Amendment Partial Approval",
                LocalName = "תיקון הצהרה אושרה חלקית",
                AssigneeNotificationTypeCode = "I",
            });
            all.Add(new NotificationDefinitionDetails()
            {
                Code = "5117D",
                EnglishName = "Declaration Amendment Denial",
                LocalName = "תיקון הצהרה נדחתה",
                AssigneeNotificationTypeCode = "I",
            });
            all.Add(new NotificationDefinitionDetails()
            {
                Code = "5117C",
                EnglishName = "Declaration Amendment Cancelled",
                LocalName = "תיקון הצהרה בוטלה",
                AssigneeNotificationTypeCode = "I",
            });
            return all;
        }

        public void MapPoco(NotificationDefinition poco)
        {
            poco.Code = this.Code;
            poco.EnglishName = this.EnglishName;
            poco.LocalName = this.LocalName;
            poco.AssigneeNotificationTypeCode = this.AssigneeNotificationTypeCode;
            poco.SearchFields = GetSearchFields(this);

        }




        public string GetSearchFields(NotificationDefinition rec)
        {
            return string.Concat(rec.Code + "," + rec.EnglishName + ",", rec.LocalName).ToLower();
        }
    }
}
