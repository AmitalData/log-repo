using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer
{
    public class error
    {
        public string Code { get; set; }
        public string ListVersionID { get; set; } // 1517
        public string MessageError { get; set; }
        public string ConstraintID { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string AmendmentFieldStatus { get; set; }

    }
}
