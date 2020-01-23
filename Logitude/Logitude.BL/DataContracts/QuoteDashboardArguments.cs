using CHAMP17;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
    public class QuoteDashboardArguments
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string OwnerId { get; set; }
        public string BusinessUnitId { get; set; }
        public string ChartCode { get; set; }

    }
}
