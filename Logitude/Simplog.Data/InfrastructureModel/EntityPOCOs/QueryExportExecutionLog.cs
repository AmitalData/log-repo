using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class QueryExportExecutionLog
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }

        public string StatusCode { get; set; }
        public string ExceptionMessage { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public string QueryFilterXML { get; set; }

        public string QueryCode { get; set; }
        
        [ForeignKey("StatusCode")]
        public virtual CommunicationStatusType CommunicationStatusType { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }



    }
}
