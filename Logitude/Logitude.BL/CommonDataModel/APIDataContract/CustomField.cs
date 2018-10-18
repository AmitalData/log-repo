using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public class CustomField
    {
        [XmlAttribute]
        public string Code { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
    }
}
