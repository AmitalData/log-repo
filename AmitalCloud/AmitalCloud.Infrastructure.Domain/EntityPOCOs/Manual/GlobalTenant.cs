using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class GlobalTenant
    {
        [Key]
        public int Id { get; set; }
        public string GlobalDBId { get; set; }
        public string CompanyName { get; set; }
        int _Version;

        public int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        //public int Version { get; set; }
        public bool IsActive { get; set; }
        public string TTY { get; set; }


        public string computed { get { return CompanyName + '(' + Id + ')'; } }

        [Include]
        [Association("GlobalDBGlobalTenant", "GlobalDBId", "Id", IsForeignKey = true)]
        [ForeignKey("GlobalDBId")]
        public  GlobalDB GlobalDB { get; set; }

        public string PrivateLabelId { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        [ForeignKey("PrivateLabelId")]
        public TenantManagmentPrivateLabels TenantManagmentPrivateLabel { get; set; }
        //public List<GlobalContact> GlobalContacts { get; set; }

        public virtual TenantManagement TenantManagement { get; set; }

    }
}