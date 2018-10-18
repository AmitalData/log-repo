using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.DataContracts
{
    public class InvoiceEntityFields
    {
        [Key]
        public string Id { get; set; }
        public string EntityTransportModeId { get; set; }
        public string EntityLevelCode { get; set; }
    }
}