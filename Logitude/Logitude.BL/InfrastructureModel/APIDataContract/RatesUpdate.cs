using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.InfrastructureModel.APIDataContract
{
    [XmlRoot("RatesUpdate")]
    public class RatesUpdate
    {
        public string ComputingPartnerCode { get; set; }

        [XmlElement("RateUpdate")]
        public List<RateUpdate> RateUpdateList { get; set; }
        public RatesUpdate()
        {
            this.RateUpdateList = new List<RateUpdate>();
        }
    }
    public class RateUpdate
    {
        [XmlElement("Currency")]
        public Currency Currency { get; set; }
        public double? Rate { get; set; }
        public DateTime? RateDate { get; set; }
    }

    public class Currency
    {
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string PartnerCode { get; set; }
    }
}
