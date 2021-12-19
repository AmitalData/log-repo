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
    public enum JournalActionTypeEnum
    {
        NotValid = 0,
        Credit = 1,
        Debit = 2,
        DebitAndCredit = 3,
        DebitCreditAndVatdeduction = 4
    }
}
