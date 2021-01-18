using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.DataContracts
{
    public class DeclarationReferantDataChartingClass
    {
        [Key]
        public string Id { get; set; }
        public string GroupedId { get; set; }
        public int IntegerProperty { get; set; }
        public string DataTypeCode { get; set; }
        public string LabelProperty { get; set; }
        public string StringProperty { get; set; }
        public double DoubleProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public string MainCarriageCarrierId { get; set; }
        public int TypeIndex { get; set; }
    }
}
