using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ManifestStatusDetails
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}