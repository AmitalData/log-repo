using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class FTPDetail
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
        public bool UseSFTP { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }
        public string CreatedByUserId { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
        public string UpdatedByUserId { get; set; }
        public byte[] PrivateKey { get; set; }
		public string Port { get; set; }

	}
}
