//using CHAMP17;
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
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; }
        public bool IncludeOthersCountries { get; set; }
        public int? TopCountries { get; set; }
        public string DatesCode { get; set; }
        public string SelectedCurrency { get; set; }
    }
}
