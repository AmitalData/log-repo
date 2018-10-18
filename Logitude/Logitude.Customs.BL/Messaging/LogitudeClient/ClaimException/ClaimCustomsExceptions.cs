using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.ClaimException
{
    public class ClaimCustomsExceptions
    {
        public string CustomsNotes { get; set; }
        public List<CustomsException> CustomsExceptions { get; set; }
    }
}
