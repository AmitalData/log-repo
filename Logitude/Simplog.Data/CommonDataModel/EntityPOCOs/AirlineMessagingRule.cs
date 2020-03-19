using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class AirlineMessagingRule
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string MessageTypeCode { get; set; }
        public string RuleFieldId { get; set; }
        public bool IsMandatoryForSending { get; set; }
        public int? MaxSize { get; set; }
        public string AirlineId { get; set; }
        public bool InActive { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string RuleFieldCode { get; set; }
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("RuleFieldId")]
        public virtual ObjectField RuleField { get; set; }

        [ForeignKey("AirlineId")]
        public virtual Card Airline { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
    }
}
