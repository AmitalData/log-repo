using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class EventTypeArgs
    {

        public List<string> EventTypeCodeList {get; set;}
        public List<EventTypeClass> EventTypeList { get; set; }

        public string EntityId { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string LoggedContactId { get; set; }



    }


    public class EventTypeClass
    {
        public string Code { get; set; }
        public DateTime? Date { get; set; }

    }

    public class PostTraceEventArgs
    {
        public int tenant { get; set; }
        public string entityId { get; set; }
        public string tableName { get; set; }
        public string eventTypeCode { get; set; }
        public string loggedUserEmail { get; set; } = "";
    }
}