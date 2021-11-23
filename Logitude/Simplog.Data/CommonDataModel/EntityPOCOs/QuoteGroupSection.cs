using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class QuoteGroupSection
    {
        [Key]
        [Column("Code")]
        public string Code { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Searchfields")]
        public string Searchfields { get; set; }


    }
}