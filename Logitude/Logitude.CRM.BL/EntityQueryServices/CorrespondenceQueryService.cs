using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class CorrespondenceQueryService
    {
        public List<CorrespondencePM> GetCorrespondencesByEntityId(string entityId, int tenant)
        {
            List<CorrespondencePM> query = (from a in context.Correspondences
                                            where a.EntityId == entityId
                                            && a.Tenant == tenant && a.IsInternal == false
                                            select new CorrespondencePM()
                                                {
                                                    Id = a.Id, 
                                                    Tenant = a.Tenant,
                                                    CreateDate = a.CreateDate,
                                                    CreatedByContactId = a.CreatedByContactId,
                                                    Description = a.Description,
                                                    IsInternal = a.IsInternal,
                                                    ObjectTableId = a.ObjectTableId,
                                                    EntityId = a.EntityId,
                                                    ActivityId = a.ActivityId,
                                                    ActivitySubject = a.ActivitySubject,
                                                    ActivityTypeCode = a.ActivityTypeCode,
                                                    CCs = a.CCs,
                                                    Bcc = a.Bcc,
                                                    InternalUsers = a.InternalUsers,
                                                    ContactName = a.CreatedByContact != null ? a.CreatedByContact.EnglishName : null,
                                                    ContactEmail = a.CreatedByContact != null ? a.CreatedByContact.Email : null,
                                                    NotifyMe = a.NotifyMe,
                                                    NotifyOwner = a.NotifyOwner,
                                                    Direction = a.Direction,
                                                    HTMLFullBody = a.HTMLFullBody,
                                                    RightToLeft = a.RightToLeft,

                                                }).OrderByDescending(a=>a.CreateDate).ToList();
            return query;
        }

        public List<CorrespondencePM> GetAllCorrespondencesByEntityIdAndTenant(string entityId, int tenant)
        {
            List<CorrespondencePM> query = (from a in context.Correspondences
                                            where a.EntityId == entityId
                                            && a.Tenant == tenant
                                            select new CorrespondencePM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                CreateDate = a.CreateDate,
                                                CreatedByContactId = a.CreatedByContactId,
                                                Description = a.Description,
                                                IsInternal = a.IsInternal,
                                                ObjectTableId = a.ObjectTableId,
                                                EntityId = a.EntityId,
                                                ActivityId = a.ActivityId,
                                                ActivitySubject = a.ActivitySubject,
                                                ActivityTypeCode = a.ActivityTypeCode,
                                                CCs = a.CCs,
                                                Bcc = a.Bcc,
                                                InternalUsers = a.InternalUsers,
                                                ContactName = a.CreatedByContact != null ? a.CreatedByContact.EnglishName : null,
                                                ContactEmail = a.CreatedByContact != null ? a.CreatedByContact.Email : null,
                                                NotifyMe = a.NotifyMe,
                                                NotifyOwner = a.NotifyOwner,
                                                Direction = a.Direction,
                                                HTMLFullBody = a.HTMLFullBody,
                                                RightToLeft = a.RightToLeft,
                  
                                            }).ToList();
            return query;
        }

        public CorrespondencePM GetLastCorrespondenceByEntityId(string entityId, int tenant)
        {
            CorrespondencePM lastCorrespondenceLine = (from a in context.Correspondences
                                            where a.EntityId == entityId
                                            && a.Tenant == tenant
                                            select new CorrespondencePM()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                CreateDate = a.CreateDate,
                                                CreatedByContactId = a.CreatedByContactId,
                                                Description = a.Description,
                                                IsInternal = a.IsInternal,
                                                ObjectTableId = a.ObjectTableId,
                                                EntityId = a.EntityId,
                                                ActivityId = a.ActivityId,
                                                ActivitySubject = a.ActivitySubject,
                                                ActivityTypeCode = a.ActivityTypeCode,
                                                CCs = a.CCs,
                                                Bcc = a.Bcc,
                                                InternalUsers = a.InternalUsers,
                                                ContactName = a.CreatedByContact != null ? a.CreatedByContact.EnglishName : null,
                                                ContactEmail = a.CreatedByContact != null ? a.CreatedByContact.Email : null,
                                                NotifyMe = a.NotifyMe,
                                                NotifyOwner = a.NotifyOwner,
                                                Direction = a.Direction,
                                                HTMLFullBody = a.HTMLFullBody,
                                                RightToLeft = a.RightToLeft,

                                            }).OrderByDescending(a => a.CreateDate).FirstOrDefault();
            return lastCorrespondenceLine;
        }
    }
}
