using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Test.Base.Models.BillingsPreparation
{
    public class CreditCardTypePM
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public string BankAccountId { get; set; }
    }
}
