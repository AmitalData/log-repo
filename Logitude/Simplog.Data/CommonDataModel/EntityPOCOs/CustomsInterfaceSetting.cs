using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomsInterfaceSetting
    {
        [Key]
        public int Tenant { get; set; }
        public string LocalCustomsInterfaceCode { get; set; }
        public string ImportToUSAInterfaceCode { get; set; }
        public string ExportFromUSAInterfaceCode { get; set; }
        public string LocalCompanyId { get; set; }
        public string LocalUserId { get; set; }
        public string LocalPassword { get; set; }       
        public bool ActivateCustomsManagementInShipments { get; set; }
        public string ArtemusOutSettingsId { get; set; }
        public string ArtemusInSettingsId { get; set; }

        [ForeignKey("LocalCustomsInterfaceCode")]
        public virtual CustomsInterface LocalCustomsInterface { get; set; }

        [ForeignKey("ImportToUSAInterfaceCode")]
        public virtual CustomsInterface ImportToUSAInterface { get; set; }

        [ForeignKey("ExportFromUSAInterfaceCode")]
        public virtual CustomsInterface ExportFromUSAInterface { get; set; }

        [ForeignKey("ArtemusOutSettingsId")]
        public virtual FTPDetail ArtemusOutSettings { get; set; }

        [ForeignKey("ArtemusInSettingsId")]
        public virtual FTPDetail ArtemusInSettings { get; set; }
    }
}
