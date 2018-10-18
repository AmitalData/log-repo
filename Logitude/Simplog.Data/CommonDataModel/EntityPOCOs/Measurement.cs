using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Measurement
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }       
        public string Code { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string ShortName { get; set; }
        public bool InActive { get; set; }
        public bool IsContainerMeasurement { get; set; }
        public bool IsContainer { get; set; }
        public string SearchFields { get; set; }
    }
}

