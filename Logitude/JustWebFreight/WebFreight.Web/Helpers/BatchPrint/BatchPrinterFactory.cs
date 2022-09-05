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
        private const string ARInvoiceObjectTableName = "ARInvoice";
        private const string ShipmentObjectTableName = "Shipment";
        public static BatchPrinter GetBatchPrinter(BatchPrinterArgs batchPrinterArgs)
        {
            ObjectTable objectTable = ObjectTableRepository.GetSingleObjectTableById(batchPrinterArgs.ObjectTableId, 0);
            switch (objectTable.Name)
            {
                case ARInvoiceObjectTableName:
                    return new ARInvoiceBatchPrinter(batchPrinterArgs);

                case ShipmentObjectTableName:
                    return new ShipmentBatchPrinter(batchPrinterArgs);

                default:
                    throw new Exception("there is no BatchPrinter for this object table");
            }
        }
    }
}