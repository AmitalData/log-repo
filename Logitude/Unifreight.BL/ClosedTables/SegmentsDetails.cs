using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.ClosedTables
{
    public class SegmentsDetails
    {
        static List<SegmentsDetails> _All;
        static SegmentsDetails()
        {
            _All = new List<SegmentsDetails>(){
                new SegmentsDetails() { Code = "03" , Segment="0103" , Description="כרטיס לחיוב" , Name="חשבון מכר"},
                new SegmentsDetails() { Code = "11" , Name="חישוב מיסים"},
                new SegmentsDetails() { Code = "08" , Segment="0108" , Description="צרופה" , Name="צרופות"},
                new SegmentsDetails() { Code = "121" , Segment="כללי" , Description="חשבון" , Name="שלמות נתונים"},
                new SegmentsDetails() { Code = "127" , Segment="0105" , Description="אישורים" , Name="אישורים"},
                new SegmentsDetails() { Code = "01" , Segment="0100" , Name="כללי"},
                new SegmentsDetails() { Code = "02" , Segment="0102" , Name="משגור"},
                new SegmentsDetails() { Code = "04" , Segment="0104" , Name="ערך למכס"},
                new SegmentsDetails() { Code = "05" , Segment="0105" , Description="חשבון מכר" , Name="סחורות"},
                new SegmentsDetails() { Code = "06" , Segment="1106" , Name="ריכבית"},
                new SegmentsDetails() { Code = "09" , Segment="0109" , Name="אמצעי תשלום"},
                new SegmentsDetails() { Code = "10" , Segment="0110" , Name="הסבר לאילוץ"},
                new SegmentsDetails() { Code = "123" , Segment="כללי" , Name="סגירת טיוטה"},
                new SegmentsDetails() { Code = "13" , Segment="כללי" , Name="תקלה"},
                new SegmentsDetails() { Code = "14" , Segment="כללי" , Name="הכנת רשימון"},
                new SegmentsDetails() { Code = "16" , Segment="כללי" , Name="תהליך הגשה"},
            };
        }
        
        public string Code { get; set; }
        public string Segment { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        
        public static List<SegmentsDetails> GetAll()
        {
            return _All;      
        }

        public SegmentsDetails GetSingle(string code)
        {
            return _All.FirstOrDefault(rec => rec.Code == code);
        }


    }
}
