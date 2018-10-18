using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Models
{

    //[Table("WCOTranslate", Schema = "Customs")]
    public class WCOTranslate
    {
        //[Key]
        //[Required]
        //[StringLength(15, MinimumLength = 0)]
        //[Column("Id", TypeName = "varchar")]
        //public string Id { get; set; }
        public string WCOFieldCode { get; set; }
        public string PMTableId { get; set; }
        public string PMFieldCode { get; set; }
        

    }
}
