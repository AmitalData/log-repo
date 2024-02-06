using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class ReleaseArgs
    {
        [Key]
        public string Id { get; set; }
        public string ReleaseDateString { get; set; }
        public bool IsDeleteRelease { get; set; }
        public string ReleaseCode { get; set; }

    }
}
