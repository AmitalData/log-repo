using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Models
{
    public class WCOTranslateSet : List<WCOTranslate>
    {
        public WCOTranslateSet()
        {
            this.Add(new WCOTranslate() { WCOFieldCode="42A",  PMFieldCode = "", PMTableId = "" });
            this.Add(new WCOTranslate() { WCOFieldCode = "05A", PMFieldCode = "", PMTableId = "" });


        }
        public WCOTranslate GetByWCOCode(string curWCOFieldCode)
        {
            return this.FirstOrDefault(rec => rec.WCOFieldCode == curWCOFieldCode);
        }
    }
}
