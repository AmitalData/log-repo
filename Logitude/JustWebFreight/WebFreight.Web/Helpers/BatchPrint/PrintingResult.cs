using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.BatchPrint
{
    public class PrintingResult
    {
        public string DocumentId { get; set; }
        public List<PrintingRow> NotValidRows { get; set; }
        public string FileName { get; set; }
        public string SecurityId { get; set; }
    }
    public class ItemPrintingResult
    {
        public ItemPrintingResult(string entityId, string entityNumber)
        {
            EntityId = entityId;
            EntityNumber = entityNumber;
        }
        public bool IsSuccessfullyPrinted { get; set; }
        public MemoryStream DocumentStream { get; internal set; }
        public string Error { get; set; }
        public string EntityId { get; set; }
        public string EntityNumber { get; set; }
    }
    public class PrintingRow
    {
        public string Error { get; set; }
        public string EntityId { get; set; }
        public string EntityNumber { get; set; }
    }
}
