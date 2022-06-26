using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class BatchPrinterFactory
    {
        const string ARInvoiceId = "1-71";
        public static BatchPrinter GetBatchPrinter(BatchPrintManagerArgs batchPrintManagerArgs)
        {
            switch (batchPrintManagerArgs.ObjectTableId)
            {
                case ARInvoiceId:
                    return new ARInvoiceBatchPrinter(batchPrintManagerArgs);
                default:
                    throw new Exception("there is no BatchPrinter for this object table");
            }
        }
    }
}