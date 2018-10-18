using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class HelpResource
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string VideoURL { get; set; }
        public string Duration { get; set; }
        public string FileName { get; set; }
        public string SearchFields { get; set; }
        public bool IsNew { get; set; }
        public string FeatureCode { get; set; }
    }
}
