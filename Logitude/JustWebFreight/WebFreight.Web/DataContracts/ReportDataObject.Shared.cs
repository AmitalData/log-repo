using System.Collections.Generic;

namespace WebFreight.Web.DataContracts
{
    public class ReportDataObject
    {
        public string Name { get; set; }

        public List<ReportDataField> Fields { get; set; }

        public Dictionary<int,Dictionary<int,object>> ListOfRows { get; set; }
    }
}