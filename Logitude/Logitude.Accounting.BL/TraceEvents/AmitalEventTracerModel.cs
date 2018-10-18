using Logitude.Accounting.BL.Messaging.Amital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accouting.BL.TraceEvents
{
    public class AmitalEventTracerModel : AmitalCommunicationModelBase
    {
        public AmitalEventTracerModel()
            : base(OperationMethod.AnalyzeStandard, "", "")
        {

        }
        public string EventCode { get; set; }

        public string notes { get; set; }

        public string currentStatusId { get; set; }
        public string newStatusId { get; set; }
        public bool manually { get; set; }



        public FUStatus MyFUStatus { get; set; }


        public class FUStatus
        {
            public string xml_status { get; set; }
            public string status { get; set; }// "new";
            public string entname { get; set; }
            public string primary_number { get; set; }
            public string status_save { get; set; }//= "no_fail";
            public DateTime status_DateTime { get; set; }
            public string status_id { get; set; }//"PUI";
            public string comments { get; set; }
            public string status_place { get; set; }//= "FRA";
        }
    }
}
