using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public class MainCarriageLeg
    {
        public string Id { get; set; }
        public int LegIndex { get; set; }
        public Card Carrier { get; set; }
        public Port FromPort { get; set; }
        public Port ToPort { get; set; }
        public Vessel Vessel { get; set; }
        public string VesselName { get; set; }
        public string CarrierNumber { get; set; }
        public string MasterNumber { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ATA { get; set; }
    }
}

