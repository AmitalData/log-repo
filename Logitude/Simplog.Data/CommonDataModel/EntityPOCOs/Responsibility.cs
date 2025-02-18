using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Responsibility
    {
        [Key]
        [Column("Code")]
        public string Code { get; set; }
        [Column("LocalName")]
        public string LocalName { get; set; }
        [Column("EnglishName")]
        public string EnglishName { get; set; }
        [Column("SearchFields")]
        public string SearchFields { get; set; }
        [Column("Inactive")]
        public bool Inactive { get; set; }
    }
}
