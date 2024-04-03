using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
    public class VisionContainerStatus
    {
        public string id { get; set; }
        public string status { get; set; }
        public string reference_id { get; set; }
        public string organization_id { get; set; }
        public Payload payload { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string parent_reference_id { get; set; }
    }


    public class Geolocation
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    

    

    public class Location
    {
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string unlocode { get; set; }
        public Geolocation geolocation { get; set; }
    }

    public class VisionMilestone
    {
        public DateTime timestamp { get; set; }
        public Location location { get; set; }
        public string description { get; set; }
        public string raw_description { get; set; }
        public string vessel { get; set; }
        public string vessel_imo { get; set; }
        public string vessel_mmsi { get; set; }
        public string voyage { get; set; }
        public bool planned { get; set; }
        public string mode { get; set; }
        public string source { get; set; }
    }


    public class Payload
    {
        public string carrier_scac { get; set; }
        public string container_id { get; set; }
        public string container_iso { get; set; }
        public string bill_of_lading { get; set; }
        public Location inland_origin { get; set; }
        public Location origin_port { get; set; }
        public Location destination_port { get; set; }
        public Location inland_destination { get; set; }
        public List<VisionMilestone> milestones { get; set; }
        public string reference_id { get; set; }
    }
}
