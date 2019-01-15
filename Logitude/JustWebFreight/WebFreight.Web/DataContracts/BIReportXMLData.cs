using Logitude.Infrastructure.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class BIReportXMLData
    {
        public string BIReportId { get; set; }
        public BIReportPM BIReportPM { get; set; }
        public List<BIReportColumnData> Columns { get; set; }
    }

    public class BIReportColumnData
    {
        public string SortColId { get; set; }
        public string SortDirction { get; set; }
        public int Width { get; set; }
        public int Index { get; set; }
    }
}