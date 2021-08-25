using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class QuoteGroupSection
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; }
        [Column("Tenant")]
        public int Tenant { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Name")]
        public string Name { get; set; }


    }
}