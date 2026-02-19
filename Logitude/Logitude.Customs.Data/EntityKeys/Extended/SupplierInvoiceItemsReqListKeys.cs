using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.EntityKeys.Extended
{
    public class SiiSelectedRowDto
    {

        public string DeclarationId { get; set; }
        public string SIIRequestID { get; set; }
        public int InvoiceCounterKey { get; set; }
        public int InvoiceItemLineNumber { get; set; }
        public int UiIndex { get; set; }

    }
}
