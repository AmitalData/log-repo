using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderTests.Models
{
    public class Card
    {
        public string Id { get; set; }

        public string EnglishName { get; set; }

        public string LocalName { get; set; }

        public string Code { get; set; }

        public string VatNumber { get; set; }

        public string PartnerCode { get; set; }

        public bool IsDisconnectedFromGLAccount { get; set; }

        public string ReceivablesAccountingCard { get; set; }

        public string PayablesAccountingCard { get; set; }

        public string ICAO { get; set; }

        public string ComputingPartnerCode { get; set; }
    }
}
