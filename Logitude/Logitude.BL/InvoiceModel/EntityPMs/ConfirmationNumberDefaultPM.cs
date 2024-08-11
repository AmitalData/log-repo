using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InvoiceModel.EntityPMs
{
    public class ConfirmationNumberDefaultPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }
        public DateTime FromDate { get; set; }
        public string SearchFields { get; set; }

        public int AmountForConfirmationNumber { get; set; }
        public bool InActive { get; set; }

    }
}