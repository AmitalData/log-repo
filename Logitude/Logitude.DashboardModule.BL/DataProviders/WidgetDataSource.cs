using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public class SeriesMeasure
    {
        public string MeasureId { get; set; }
        public List<SeriesMeasureVulue> SeriesMeasureVulues = new List<SeriesMeasureVulue>();
    }
    public class SeriesMeasureVulue
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
        public string GroupById { get; set; }
    }
}
