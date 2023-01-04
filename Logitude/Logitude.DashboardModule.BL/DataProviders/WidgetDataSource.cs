using System;
using System.Collections.Generic;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public class SeriesMeasure
    {
        public SeriesMeasure()
        {
            Values = new List<SeriesMeasureValue>();
        }
        public string Name { get; set; }
        public string RenderAs { get; set; }
        public string MeasureFieldId { get; set; }
        public List<SeriesMeasureValue> Values { get; set; }
    }
    public class SeriesMeasureValue
    {
        public decimal Value { get; set; }
        public string Label { get; set; }
        public string GroupById { get; set; }
        public string LabelSec { get; set; }
        public string GroupByIdSec { get; set; }
    }
}
