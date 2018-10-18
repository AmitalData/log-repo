using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class MoveTypeDetails
    {
        public int Tenant { get; set; }
        public string MoveTypeEnglishName { get; set; }
        public string MoveTypeLocalName { get; set; }
        public string TransportModeId { get; set; }
        public bool AddedManually { get; set; }
        public bool InActive { get; set; }
        public string Code { get; set; }
        public string SearchFields { get; set; }
    }
}