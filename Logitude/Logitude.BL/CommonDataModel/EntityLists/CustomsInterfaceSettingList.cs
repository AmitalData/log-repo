using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomsInterfaceSettingList
    {
        [Key]
        public int Tenant { get; set; }
        public string LocalCustomsInterfaceCode { get; set; }
        public string ImportToUSAInterfaceCode { get; set; }
        public string ExportFromUSAInterfaceCode { get; set; }
        public string LocalCompanyId { get; set; }
        public string LocalUserId { get; set; }
        public string LocalPassword { get; set; }
        public bool ActivateCustomsManagInShipment { get; set; }
        public string ArtemusOutSettingsId { get; set; }
        public string ArtemusInSettingsId { get; set; }
        public DateTime? AMCAirStartDate { get; set; }
        public DateTime? AMCOceanStartDate { get; set; }
    }
}
