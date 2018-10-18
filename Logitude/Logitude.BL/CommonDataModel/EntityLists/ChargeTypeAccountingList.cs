using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class ChargeTypeAccountingList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string VatTypeId { get; set; }
        public string ChargeTypeId { get; set; }
        public string PayableDebitAccount { get; set; }
        public string ReceivableCreditAccount { get; set; }
        public string PayableDebitGLAcountId { get; set; }
        public string ReceivableCreditGLAccountId { get; set; }
    }
}
