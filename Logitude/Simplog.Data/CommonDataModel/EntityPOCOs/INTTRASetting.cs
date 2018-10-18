using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class INTTRASetting
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        [ForeignKey("OutSettingsId")]
        public virtual FTPDetail OutFTPDetail { get; set; }
        public string OutSettingsId { get; set; }

        [ForeignKey("InSettingsId")]
        public virtual FTPDetail InFTPDetail { get; set; }
        public string InSettingsId { get; set; }

        [ForeignKey("INTTRASettingModeCode")]
        public virtual INTTRASettingMode INTTRASettingMode { get; set; }
        public string INTTRASettingModeCode { get; set; }

        public string INTTRAId { get; set; }
        public string INTTRAAlias { get; set; }
    }
}
