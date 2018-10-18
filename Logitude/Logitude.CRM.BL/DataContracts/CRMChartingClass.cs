using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.DataContracts
{
    public class CRMChartingClass
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
        public string OwnerId { get; set; }
        public int TypeIndex { get; set; }
        public int IndexOrder { get; set; }

        public string ClassificationId { get; set; }
        public string TicketStageId { get; set; }
        public string LabelColor { get; set; }

        public string SeverityId { get; set; }

        public string TicketTypeId { get; set; }

        public string DateRange { get; set; }

        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }

        public TimeSpan TimeProperty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string GroupByCode { get; set; }

    }
}
