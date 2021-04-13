using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.APIDataContract
{
    public class MilestoneData
    {
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string EstimationDate { get; set; }
        public string EstimationTime { get; set; }
        public string Remarks { get; set; }

    }
}
