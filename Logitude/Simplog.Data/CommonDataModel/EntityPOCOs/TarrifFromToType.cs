using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TarrifFromToType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        //public List<TarrifFromTo> TarrifFromToes { get; set; }
    }
}