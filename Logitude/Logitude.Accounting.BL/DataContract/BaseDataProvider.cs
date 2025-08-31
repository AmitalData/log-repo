using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.DataContract
{
    public class BaseDataProvider
    {
        public DateTime Today_DateTime { get; set; }
        public byte[] Logo { get; set; }
        public string GeneralAddress { get; set; }
        public string CompanyName { get; set; }
        public string PrintNotes { get; set; }
        public string LocalPrintNotes { get; set; }
    }
}
