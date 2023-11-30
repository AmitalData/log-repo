using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders.Models
{
    public class KpiChart
    {
        public string MeasureLabel { get; set; }
        public object Value { get; set; }
        public string Unit { get; set; }
        public object ComparisonValue { get; set; }
        public int Ratio { get; set; }
        public string AbbreviationSymbol { get; set; }
        public string ComparisonAbbreviationSymbol { get; set; }
        public bool CompareWithPrevious { get; set; }
        public string Increase { get; set; }
        public string CompareFromDate { get; set; }
        public string CompareToDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
