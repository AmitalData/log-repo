using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.DataContracts
{
    public class MixPanelTrackingEvent
    {
        public MixPanelTrackingEvent()
        {
        }

        public string UserAgent { get; set; }
        public string IPAddress { get; set; }
        public string SearchKeyword { get; set; }
        public int ResultsCount { get; set; } = 0;
        public string Browser { get; set; }
        public DateTime DateCreated { get; set; }
        public string EventName { get; set; }

    }
}
