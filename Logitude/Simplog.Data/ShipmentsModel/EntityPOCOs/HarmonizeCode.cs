using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class HarmonizeCode
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        public string ChapterCode { get; set; }
        public string ChapterDescription { get; set; }
        public string SubChapterCode { get; set; }
        public string SubChapterDescription { get; set; }
        public string SearchFields { get; set; }
    }
}
