using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.DataContracts
{
    public class FHLShipmentValidator
    {
        [Key]
        public int Id { get; set; }
        public bool IsFHLValid { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string Shipper { get; set; }
        public string FNAReason { get; set; }
        public string FHLStatusCode { get; set; }
        public string FHLStatusName { get; set; }
        public string CargonautFHLStatusCode { get; set; }
        public string CargonautFHLStatusName { get; set; }
        public List<string> FHLErrors { get; set; }
    }
}
