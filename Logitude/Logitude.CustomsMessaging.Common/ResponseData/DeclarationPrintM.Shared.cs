using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class DeclarationPrintM
    {
        public int SequenceNumber { get; set; }
        public string DeclarationNumber { get; set; }
        public string CustomFileNo { get; set; }
        public string ErrorText { get; set; }
        public bool IsFiled { get; set; }
    }
}
