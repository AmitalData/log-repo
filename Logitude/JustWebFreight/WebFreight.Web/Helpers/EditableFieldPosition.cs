using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class EditableFieldPosition
    {
        
        public int PageFieldIndex { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string BorderBrach { get; set; }
        public string Float { get; set; }
        public string TextColor { get; set; }
        public string TextAlgin { get; set; }

        public double FontSize { get; set; }
        public string Fontweight { get; set; }
        public string FontFamily { get; set; }
        public string NewValue { get; set; }
        public string Status { get; set; }
        public int PageCount { get; set; }
        public string ReportKey { get; set; }
        public double WidthPagePrecentage { get; set; }
        public double HeightPagePrecentage { get; set; }

        public string TextAligh { get; set; }
        public string VerticalAlign { get; set; }
        public string FieldPosition { get; set; }
        public bool IsEditedField { get; set; }
        public string OldValue { get; set; }
        public bool ReturnToOriginValue { get; set; }
        public string ControlType { get; set; }
        public int NumberOfRequest { get; set; }
        public bool IsEditField { get; set; }




    }
}