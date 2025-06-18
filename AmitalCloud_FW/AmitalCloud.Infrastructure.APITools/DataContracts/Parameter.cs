using System.Xml.Serialization;

namespace AmitalCloud.Infrastructure.APITools.DataContracts
{
    public class Parameter
    {
        [XmlAttribute("order")]
        public int Order { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }

        public string Value { get; set; }
    }

}
