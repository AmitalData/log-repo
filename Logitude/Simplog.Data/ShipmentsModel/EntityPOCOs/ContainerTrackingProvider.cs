using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ContainerTrackingProvider
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string SourceCode { get; set; }
        [ForeignKey("SourceCode")]
        public virtual ContainerStatusSource Source { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
        public string CallbackURL { get; set; }
        public string APIKey { get; set; }
        public string ProviderURL { get; set; }
        public string LogitudeToken { get; set; }

    }
}
