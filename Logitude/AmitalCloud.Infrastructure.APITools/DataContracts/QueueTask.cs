using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace AmitalCloud.Infrastructure.APITools.DataContracts
{
    public class QueueTask
    {
        [XmlAttribute("action")]
        public string Action { get; set; }

        public List<Parameter> Parameters { get; set; }

    }

}
