using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Simplog.Data.Helpers
{
   [XmlRoot(ElementName = "Field")]
    public class Field
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public bool IsChange { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        [XmlAttribute]
        public string PropertyName { get; set; }
        
    }
}
