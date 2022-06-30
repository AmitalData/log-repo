using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class BatchPrinterFactory
    {
        const string ARInvoiceObjectTableName = "ARInvoice";
        public static BatchPrinter GetBatchPrinter(BatchPrinterArgs batchPrinterArgs)
        {
            ObjectTable objectTable = ObjectTableRepository.GetSingleObjectTableById(batchPrinterArgs.ObjectTableId, 0);
            switch (objectTable.Name)
            {
                case ARInvoiceObjectTableName:
                    return new ARInvoiceBatchPrinter(batchPrinterArgs);
                default:
                    throw new Exception("there is no BatchPrinter for this object table");
            }
        }
    }
}