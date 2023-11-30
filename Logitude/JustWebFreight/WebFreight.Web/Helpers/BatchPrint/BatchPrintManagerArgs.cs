using Logitude.Infrastructure.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class BatchPrintManagerArgs
    {
        public string DocumentTypeId { get; set; }
        public string TemplateId { get; set; }
        public string CopyId { get; set; }
        public string ObjectTableId { get; set; }
        public int Tenant { get; set; }
        public List<PrintEntityKeys> EntityIds { get; set; }
        public string Email { get; set; }
    }
    public class PrintEntityKeys
    {
        public string EntityId { get; set; }
        public string EntityNumber { get; set; }
        public string ChildEntityId { get; set; }
        public string ObjectTableId { get; set; }
        public bool IsAlreadyPrinted { get; set; }
    }
    public class BatchPrinterArgs: BatchPrintManagerArgs
    {
        public BatchTaskExecutionPM BatchTaskExecution { get; set; }
        public BatchPrinterArgs(BatchPrintManagerArgs batchPrintManagerArgs, BatchTaskExecutionPM BatchTaskExecution)
        {
            this.CopyId = batchPrintManagerArgs.CopyId;
            this.DocumentTypeId = batchPrintManagerArgs.DocumentTypeId;
            this.Email = batchPrintManagerArgs.Email;
            this.EntityIds = batchPrintManagerArgs.EntityIds;
            this.ObjectTableId = batchPrintManagerArgs.ObjectTableId;
            this.TemplateId = batchPrintManagerArgs.TemplateId;
            this.Tenant = batchPrintManagerArgs.Tenant;
            this.BatchTaskExecution = BatchTaskExecution;
        }
    }
}
