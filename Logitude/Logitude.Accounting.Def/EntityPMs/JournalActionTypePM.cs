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

    public partial class JournalActionTypePM : EntityPM
    {
        
    }
    public enum MyJournalActionTypeEnum
    {
        // JournalActionTypeRepository journalActionTypeRepository = new JournalActionTypeRepository(accountingContext);
        //AddClosedTables.AddJournalActionType(new JournalActionTypeDetails() { Code = "1", JournalActionTypeID = "1", Tenant = 0, EnglishName = "Credit", LocalName = "זכות" }, journalActionTypeRepository);
        //AddClosedTables.AddJournalActionType(new JournalActionTypeDetails() { Code = "2", JournalActionTypeID = "2", Tenant = 0, EnglishName = "Debit ", LocalName = "חובה" }, journalActionTypeRepository);
        //AddClosedTables.AddJournalActionType(new JournalActionTypeDetails() { Code = "3", JournalActionTypeID = "3", Tenant = 0, EnglishName = "Debit And Credit", LocalName = "חובה+זכות" }, journalActionTypeRepository);
        //AddClosedTables.AddJournalActionType(new JournalActionTypeDetails() { Code = "4", JournalActionTypeID = "4", Tenant = 0, EnglishName = "Debit, Credit And Vat deduction ", LocalName = "חובה + זכות + חילוץ מעמ" }, journalActionTypeRepository);
        NotValid = 0,
        Credit = 1,
        Debit = 2,
        DebitAndCredit = 3,
        DebitCreditAndVatdeduction = 4
    }
}
