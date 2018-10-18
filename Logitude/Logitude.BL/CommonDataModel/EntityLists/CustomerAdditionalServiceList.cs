using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerAdditionalServiceList
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string AdditionalServiceId { get; set; }

        public int Tenant { get; set; }

        public bool Potential { get; set; }
        public string CustomerName { get; set; }

        public string AdditionalServiceName { get; set; }
        public string AdditionalServiceCode { get; set; }

        public string Notes { get; set; }

        public bool NotesRightToLeft { get; set; }
        
    }
}