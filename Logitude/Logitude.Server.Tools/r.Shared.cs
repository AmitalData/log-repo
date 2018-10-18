using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Server.Tools
{
    public class r //the root class for detectig properties changes
    {
        public List<c> cs { get; set; }
    }

    public class c //property change class
    {
        [XmlAttribute]
        public string f { get; set; }// field name
        [XmlAttribute]
        public string o { get; set; }// old value
        [XmlAttribute]
        public string n { get; set; }// new value
    }
}
