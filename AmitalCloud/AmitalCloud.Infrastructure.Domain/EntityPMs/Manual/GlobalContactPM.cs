using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class GlobalContactPM : BaseEntityPM
    {
        public GlobalContactPM() : base() { }
        public GlobalContactPM(GlobalContact entityPOCO) : base()
        {
            Id = entityPOCO.Id;
            GlobalTenantId = entityPOCO.GlobalTenantId;
            Email = entityPOCO.Email;
            InActive = entityPOCO.InActive;
            IsUser = entityPOCO.IsUser;
            InternetAccess = entityPOCO.InternetAccess;
            GlobalTenant = new GlobalTenantPM(entityPOCO.GlobalTenant);
        }
        public string Id { get; set; }
        public int GlobalTenantId { get; set; }
        public string Email { get; set; }
        public bool InActive { get; set; }
        public bool IsUser { get; set; }
        public bool InternetAccess { get; set; }
        public virtual GlobalTenantPM GlobalTenant { get; set; }

    }
}
