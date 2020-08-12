using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public class Transshipment
    {
        public string Id { get; set; }
        public int LegIndex { get; set; }
        public Card Carrier { get; set; }
        public Port Port { get; set; }
        public Vessel Vessel { get; set; }
        public string CarrierNumber { get; set; }
        public string MasterNumber { get; set; }
    }
}

