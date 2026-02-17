using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.GlobalModel.EntityLists
{
    public class BluesnapTransactionList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string DocumentId { get; set; }
        public string LogitudeAmital { get; set; }
    }
}
