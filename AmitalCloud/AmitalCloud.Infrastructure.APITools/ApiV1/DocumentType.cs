using System.Xml.Serialization;

namespace AmitalCloud.Infrastructure.APITools.ApiV1
{
    public class DocumentType
    {


        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Code { get; set; }

        public string Name { get; set; }

        [XmlAttribute]
        public string PartnerCode { get; set; }
    }
}
