using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class CRMSummary
    {
        [Key]
        public int Id { get; set; }

        public int OpenCallsToday { get; set; }
        public int OpenTasksToday { get; set; }
        public int OpenMeetingsToday { get; set; }
        
        public int MyOpenDataCount { get; set; }
        public int MyOpenAsAccountManagerDataCount { get; set; }
        public int AllOpenDataCount { get; set; }
        public int MyClosedDataCount { get; set; }
        public int AllClosedDataCount { get; set; }
        public int OpenByStageCount { get; set; }

        public int Quotes_All { get; set; }
        public int Quotes_My { get; set; }
        public int Quotes_InProgress { get; set; }
        public int Quotes_Created { get; set; }
        public int Quotes_Draft { get; set; }
        public int Quotes_Expired { get; set; }
        public int Quotes_Accepted { get; set; }
        public int Quotes_AcceptedNOShip { get; set; }
        public int Quotes_Cancelled { get; set; }
        public int Quotes_Sent { get; set; }
        public int Quotes_AllFollowups { get; set; }
        public int Quotes_MyFollowups { get; set; }

        public int Customers_Waiting { get; set; }
        public int Customers_Potential { get; set; }
        public int Customers_Active { get; set; }
        public int Customers_Inactive { get; set; }

        public int Unassigned_Tickets { get; set; }
        public int SLA_Failures { get; set; }
        public int RecentlyUpdated_Tickets { get; set; }

        public int Tickets_ExternalLines { get; set; }
        public int Tickets_InternalLines { get; set; }
        public int Tickets_Escalations { get; set; }
        public int Tickets_Activities { get; set; }
    }
}
