using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class PackageFeature
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PackageCode { get; set; }
        public string FeatureId { get; set; }
        public string FeatureUniqeCode { get; set; }


        //[ForeignKey("FeatureId")]
        public Feature Feature { get; set; }

        [ForeignKey("PackageCode")]
        public Package Package { get; set; }
    }
}
