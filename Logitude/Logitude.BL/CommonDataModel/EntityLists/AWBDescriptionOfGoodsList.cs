using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class AWBDescriptionOfGoodsList
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDescriptionOfGoods { get; set; }
        public string AirlineCode { get; set; }
        public string ProductCode { get; set; }
        public string SearchFields { get; set; }
        public string Service { get; set; }
        public bool IsTemperatureSensitive { get; set; } 
    }
}
