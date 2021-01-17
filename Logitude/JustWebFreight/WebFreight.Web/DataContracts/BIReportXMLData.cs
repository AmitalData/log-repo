using Logitude.Infrastructure.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.DataContracts
{
    public class BIReportXMLData
    {
        public string BIReportId { get; set; }
        public string UserId { get;set; }
        public DWQueryData DWQueryData { get; set; }
        public BIReportPM BIReportPM { get; set; }
        public BITabularViewSettings BITabularViewSettings { get; set; }
        public string BIReportKey { get; set; }
        public bool IncludeTotals { get; set; }
        public string ExportDataType { get; set; }
        public string BIReportsExecutionLogId { get; set; }

        
    }

    [XmlRoot("BITabularViewSettings")]
    public class BITabularViewSettings
    {
        [XmlArray("Columns")]
        [XmlArrayItem("Column")]
        public List<Column> Columns { get; set; }
    }

    public class Column
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SortDirction { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int SortOrder { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Width { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Index { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsChecked { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DataTypeCode { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FieldCode { get; set; }


    }
}