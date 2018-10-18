using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer
{
    public class field : error
    {
        public string Fieldcode { get; set; }
        public string Code { get; set; }
        public string MessageError { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
