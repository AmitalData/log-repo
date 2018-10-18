using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class AWBOCIList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string CountryId { get; set; }
        public string AWBCustomsInformationCode { get; set; }
        public string AWBInformationCode { get; set; }
        public string SupplementaryCustomsInfo { get; set; }
    }
}