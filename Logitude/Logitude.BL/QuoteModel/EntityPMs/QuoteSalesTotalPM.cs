using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityPMs
{
    public class QuoteSalesTotalPM
    {
        [Key]
        public string CurrencyCode { get; set; }
        public double? Amount { get; set; }
    }
}
