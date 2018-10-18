using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class EmailAlertSettingDetails
    {
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }

        public string ObjectTableId { get; set; }
        public bool InActive { get; set; }
        public string SettingLevelCode { get; set; }
        public string To { get; set; }
        public int IndexOrder { get; set; }
    }
}