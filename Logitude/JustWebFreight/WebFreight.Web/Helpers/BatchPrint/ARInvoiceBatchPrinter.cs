using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class ARInvoiceBatchPrinter : BatchPrinter
    {
        public ARInvoiceBatchPrinter(BatchPrintManagerArgs batchPrintManagerArgs) :base(batchPrintManagerArgs)
        {

        }
        public override void CustomeValidation()
        {
            //_batchPrintManagerArgs;
        }
    }
}