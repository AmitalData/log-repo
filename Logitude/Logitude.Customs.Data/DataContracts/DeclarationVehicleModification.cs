using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts
{
    public  class DeclarationVehicleModification
    {

        [Key]
        public Guid Id { get; set; }
        public string ChassisNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string AdjustmentType { get; set; }
        public decimal? DeductAmount { get; set; }
    }
}
