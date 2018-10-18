using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ProductTypeDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
    }
}