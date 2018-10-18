using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class HelpResourceDetails
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string VideoURL { get; set; }
        public string Duration { get; set; }
        public string FileName { get; set; }
        public bool IsNew { get; set; }
        public string FeatureCode { get; set; }
    }
}