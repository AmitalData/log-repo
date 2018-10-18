using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DocumentFilingBackupSetting
    {
        [Key]
        public int Tenant { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }

        public string FTPDetailId { get; set; }
        [ForeignKey("FTPDetailId")]
        public virtual FTPDetail FTPDetail { get; set; }
    }
}
