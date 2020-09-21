using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityPMs
{
    public class HelpResourcePM
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string VideoURL { get; set; }
        public string Duration { get; set; }
        public string FileName { get; set; }
        public string SearchFields { get; set; }
        public bool IsNew { get; set; }
        public string FeatureCode { get; set; }
        public int Tenant { get; set; }
        public string File { get; set; }
        public string FileExtension { get; set; }
        public bool Inactive { get; set; }
    }
}
