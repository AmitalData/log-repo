using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DocumentFilingBackupBatch
    {

        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string BatchNumber { get; set; }

        public DateTime? DoneDate { get; set; }

        public string Status { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? FromDatetime { get; set; }

        public DateTime? ToDatetime { get; set; }

        public int TotalDocuments { get; set; }

        public int TotalSucceeded { get; set; }

        public int TotalFailed { get; set; }

        public bool IncludeBackedUp { get; set; }



    }
}
