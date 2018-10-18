using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class DocumentFilingBackupSettingList
    {
        [Key]
        public int Tenant { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }


        public string FTPDetailId { get; set; }
    }
}
