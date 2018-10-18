using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityPMs
{
    public partial class DepositPM : ITapagPM
    {
        [DataMember]
        public string CustomsTapagFile { get; set; }

        [DataMember]
        public int CustomsNumeral { get; set; }

        [DataMember]
        public int PaymentOrderNumber { get; set; }

        [DataMember]
        public decimal CustomsAmount { get; set; }

        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public string ConnectedDeclarationId { get; set; }

        [DataMember]
        public string DecisionCode { get; set; }

    }
}
