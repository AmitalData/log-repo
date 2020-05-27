using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
   public class PaymentGatewayPartnerPM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}
