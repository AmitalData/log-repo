using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class FTPDetailPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Folder { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool InActive { get; set; }        
        public string CreatedByUserId { get; set; }        
        public string UpdatedByUserId { get; set; }
        public bool UseSFTP { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}
