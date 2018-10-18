using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer
{
    public class Entity
    {
        public string Child1Type { get; set; }
        public string Child1Sequence { get; set; }
        public string Child2Type { get; set; }
        public string Child2Sequence { get; set; }
        public string Child3Type { get; set; }
        public string Child3Sequence { get; set; }
        //public EntityError EntityErrors { get; set; }
        public List<error> EntityErrors { get; set; }
        //public FieldError FieldErrors { get; set; }
        public List<field> FieldErrors { get; set; }
    }
}
