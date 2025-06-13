using System.Xml.Serialization;

namespace AmitalCloud.Infrastructure.APITools.ApiV1
{
    public class ObjectTable
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
    }

}
