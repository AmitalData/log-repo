using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.StimulReport
{
    public class SupplierInvoiceModificationM
    {
        public string ModificationAndDiscountTypeLocalName { get; set; }

        public string ModificationAndDiscountTypeCode { get; set; }

        public string CurrencyTypeCode { get; set; }

        public double? Amount { get; set; }
    }
}
