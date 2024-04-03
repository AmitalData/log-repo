using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class ExcelReportArguments
    {
        public List<DataProviderField> DataProviderFields { get; set; }
        public string ReportId { get; set; }
        public string ReportsTemplateId { get; set; }
        public bool IsNew { get; set; }
    }
}