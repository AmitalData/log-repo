using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityDws
{
    public class ActivitiyDW
    {
        public string ActivityId { get; set; }
        public string ActivityTypeCode { get; set; }
        public string ActivityTypeName { get; set; }
        public string OpportunityId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string CreatedbyUserId { get; set; }
        public string CreatedbyUserName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string ActivityStatusCode { get; set; }
        public string ActivityStatusName { get; set; }
        public string MeetingSummary { get; set; }
        public bool IsOpen { get; set; }
        public string QuoteId { get; set; }
        public string Notes { get; set; }


        public DateTime? DueDate { get; set; }
        public string SalesmanId{ get; set; }
        public string SalesmanName { get; set; } 

    }
}
