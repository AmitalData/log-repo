using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class CustomerCurrencyCode
    {
        [Key]
        public string InvoiceCurrencyId { get; set; }
    }
}