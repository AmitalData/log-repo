using Logitude.Accounting.Def.Validators;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    //[CustomValidation(typeof(AccountingClassLevelValidator), "ValidateClass")]
    //[DataContract]
    public partial class JournalStatusTypePM : EntityPM
    {
        public enum StatusCodeEnum
        {
            //AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "0", EnglishName = "Draft", LocalName = "פתוח" }, journalStatusTypeRepository);
            //AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "1", EnglishName = "Waiting for Approve", LocalName = "סגור" }, journalStatusTypeRepository);
            //AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "2", EnglishName = "Approved", LocalName = "מאושר" }, journalStatusTypeRepository);
            //AddClosedTables.AddJournalStatusType(new JournalStatusTypeDetails() { JournalStatusID = "3", EnglishName = "Voided", LocalName = "מבוטל" }, journalStatusTypeRepository);
            Draft = 0,
            WaitingforApprove = 1,
            Approved = 2,
            Voided = 3,
            Failed=4

        }
    }
}
