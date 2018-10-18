using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class JournalActionTypeDetails
    {
       
            public string JournalActionTypeID { get; set; }
            public string Code { get; set; }
            public string EnglishName { get; set; }
            public string LocalName { get; set; }
            public string SearchFileds { get; set; }
            public int Tenant { get; set; }
            public bool Inactive { get; set; }

       
    }
}