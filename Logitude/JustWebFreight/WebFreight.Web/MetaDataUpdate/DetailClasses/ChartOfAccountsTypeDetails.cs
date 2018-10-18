using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class ChartOfAccountsTypeDetails
    {
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string SearchFileds { get; set; }
        public bool Inactive { get; set; }
        public string ParentName { get; set; }
        public string TypeName { get; set; }
    }
}