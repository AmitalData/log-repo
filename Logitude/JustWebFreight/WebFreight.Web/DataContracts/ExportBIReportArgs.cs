using Logitude.Infrastructure.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class ExportBIReportArgs
    {
        public  BIReportXMLData BIReportXMLData { get; set; }
        public int Tenant { get; set; }
        public  DataTable DataTable { get; set; }

    }
}