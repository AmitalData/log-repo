using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Tariff.Models
{
    public class TariffVersionPM
    {
        public int Tenant { get; set; }
        public int Version { get; set; }
        public bool IsDraft { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ChangeSetOp { get; set; }
    }
}
