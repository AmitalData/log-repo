using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityAMs
{
    public class EventTypeAM
    {
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string EntityStatusCode { get; set; }
        public string ObjectTableName { get; set; }
        public bool IsCustomerView { get; set; }
        public bool IsAgentView { get; set; }
        public bool IsFollowUp { get; set; }
        public string FollowUpEnglishName { get; set; }
        public string FollowUpLocalName { get; set; }
        public string EventTrigger { get; set; }
    }
}
