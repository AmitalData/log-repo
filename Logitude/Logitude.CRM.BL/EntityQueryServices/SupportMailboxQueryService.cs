using Logitude.CRM.BL.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class SupportMailboxQueryService
    {
        public List<SupportMailboxPM> GetSupportMailboxsByTenant(int tenant)
        {
            List<SupportMailboxPM> query = (from a in context.SupportMailboxes
                                       where a.Tenant == tenant
                                       select new SupportMailboxPM()
                                       {
                                           Id = a.Id,
                                           Tenant = a.Tenant,
                                           CreateDate = a.CreateDate,
                                           CreatedByUserId = a.CreatedByUserId,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           Mailbox = a.Mailbox,
                                           IsDefault = a.IsDefault,
                                           Inactive = a.Inactive,
                                           UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                           CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                       }).ToList();
            return query;
        }
    }
}
