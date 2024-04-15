using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityLists
{
    public class ConfirmationNumberDefaultList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime FromDate { get; set; }
        public int AmountForConfirmationNumber { get; set; }

    }
}