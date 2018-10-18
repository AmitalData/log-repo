using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class JournalTypeDetails
    {
        public string JournalTypeID { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string SearchFileds { get; set; }
        public bool Inactive { get; set; }
    }
}