using System.Xml.Serialization;

namespace AmitalCloud.Infrastructure.APITools.ApiV1
{
    public partial class User
    {


        [XmlAttribute]
        public string Id { get; set; }

        public string EnglishName { get; set; }

        public string LocalName { get; set; }

        [XmlAttribute]
        public string ExternalCode { get; set; }

        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public string PartnerCode { get; set; }

        public string ComputingPartnerCode { get; set; }

    }

}
