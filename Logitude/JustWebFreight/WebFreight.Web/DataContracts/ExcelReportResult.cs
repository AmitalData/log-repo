using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class ExcelReportResult
    {
        public List<DataProviderField> DataProviderFields { get; set; }
        public List<DataProviderField> SelectedDataProviderFields { get; set; }
    }
}