using Logitude.BL.CommonDataModel.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
  public  class AgentSharedDocumentPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AgentReference { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string DocumentXML { get; set; }
        public string StatusCode { get; set; }
        public string AgentId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string AgentSharedManifestRef { get; set; }
        public DocumentSL DocumentSL { get; set; }
    }
}
