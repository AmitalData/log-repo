using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class VatMandatoryTypeDetails
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int ViewOrder { get; set; }
        public string SearchFields { get; set; }
    }
}