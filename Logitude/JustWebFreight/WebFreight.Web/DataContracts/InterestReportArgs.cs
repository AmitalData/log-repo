using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class InterestReportArgs
    {

        public bool AllSelected { get; set; }
        public List<string> SelectedIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<string> ExcludedIds { get; set; }
    }
}