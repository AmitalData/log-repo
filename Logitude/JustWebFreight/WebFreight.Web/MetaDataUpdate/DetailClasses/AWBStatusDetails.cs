using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class AWBStatusDetails
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}