using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomerAdditionalService
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string AdditionalServiceId { get; set; }

        public int Tenant { get; set; }

        public bool Potential { get; set; }

        public string Notes { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("AdditionalServiceId")]
        public virtual AdditionalService AdditionalService { get; set; }

        public bool NotesRightToLeft { get; set; }
    }
}
