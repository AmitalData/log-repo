using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class AirlineMessagingRuleList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AirlineId { get; set; }
        public string MessageTypeCode { get; set; }
        public string RuleFieldId { get; set; }
        public bool IsMandatoryForSending { get; set; }
        public int? MaxSize { get; set; }
        public bool InActive { get; set; }

        //dummy
        public string RuleFieldName { get; set; }
        public string AirlineCode { get; set; }
    }
}
