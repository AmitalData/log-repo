using Logitude.BL.CommonDataModel.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class ShareDocumentArgs
    {
        public string AgentId { get; set; }
        public string AgentReference { get; set; }
        public string ShipmentLevelCode { get; set; }
        public List<ShipmentShareDocumentsData> ShareDocumentsLists { get; set; }

    }
}