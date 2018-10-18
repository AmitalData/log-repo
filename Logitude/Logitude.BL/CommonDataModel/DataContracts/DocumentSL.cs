using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.DataContracts
{
    public class DocumentSL
    {

  
        public string AgentReference { get; set; }
        public string DocumentXML { get; set; }
        public string AgentId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string AgentSharedManifestRef { get; set; }
        public List<DocumentDetails> DocumentLists { get; set; }
        
    }
    public class DocumentDetails
    {
        public string SecurityKey { get; set; }
        public string DocumentCode { get; set; }
    }
}
