using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.APIDataContract
{
    public class ReferenceViaCarrierCodeRequest
    {
        public string container_id { get; set; }
        public string carrier_code { get; set; }
        public string bill_of_lading { get; set; }
        public string callback_url { get; set; }
    }
    public class CreateReferenceViaBillOfLadingRequest
    {
        public string bill_of_lading { get; set; }
        public string carrier_code { get; set; }
        public string callback_url { get; set; }
    }
    public class VizionReferenceResponce
    {
        public string message { get; set; }
        public Reference reference { get; set; }

    }
    public class Reference
    {
        public string callback_url { get; set; }
        public string container_id { get; set; }
        public string carrier_id { get; set; }
        public string carrier_scac { get; set; }
        public string organization_id { get; set; }
        public bool auto_carrier { get; set; }
        public object bill_of_lading { get; set; }
        public string parent_reference_id { get; set; }
        public object last_update_status { get; set; }
        public object last_update_attempted_at { get; set; }
        public string id { get; set; }
        public bool active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int retry_count { get; set; }

    }
    public class VizionCarrier
    {
        public string scac { get; set; }
        public string name { get; set; }
        public string carrier_code { get; set; }
    }
    public class UnsubscribeResult
    {
        public string message { get; set; }

    }

    public class Organization
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class ActiveRequest
    {
        public string id { get; set; }
        public string container_id { get; set; }
        public object bill_of_lading { get; set; }
        public string carrier_scac { get; set; }
        public string callback_url { get; set; }
        public string organization_id { get; set; }
        public object parent_reference_id { get; set; }
        public bool active { get; set; }
        public string last_update_status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime last_update_attempted_at { get; set; }
        public int retry_count { get; set; }
        public Organization organization { get; set; }
    }
}
