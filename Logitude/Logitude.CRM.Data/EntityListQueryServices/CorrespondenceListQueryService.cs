using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{

    public partial class CorrespondenceListQueryService
    {
        private IQueryable<CorrespondenceList> GetIqueryableList(IQueryable<Correspondence> iQueryable)
        {
            IQueryable<CorrespondenceList> query = (from a in iQueryable.Include("CreatedByContact")
                                                    select new CorrespondenceList()
                                                    {
                                                        Id = a.Id,

                                                        Tenant = a.Tenant,

                                                        CreatedByContactId = a.CreatedByContactId,

                                                        CreateDate = a.CreateDate,

                                                        Description = a.Description,

                                                        IsInternal = a.IsInternal,

                                                        ObjectTableId = a.ObjectTableId,

                                                        EntityId = a.EntityId,

                                                        ContactName = a.CreatedByContact != null ? a.CreatedByContact.EnglishName : null,

                                                        ActivityId = a.ActivityId, 

                                                        ActivitySubject = a.ActivitySubject, 

                                                        ActivityTypeCode = a.ActivityTypeCode,

                                                        CCs = a.CCs , 

                                                        Bcc = a.Bcc,

                                                        NotifyMe = a.NotifyMe,

                                                        NotifyOwner = a.NotifyOwner,

                                                        InternalUsers = a.InternalUsers,

                                                        ContactEmail = a.CreatedByContact != null ? a.CreatedByContact.Email : null,

                                                        Direction = a.Direction,

                                                        HTMLFullBody = a.HTMLFullBody,

                                                        RightToLeft = a.RightToLeft,
                                                    });
            return query;
        }

        private IQueryable<Correspondence> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Correspondence> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<Correspondence> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Correspondence> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<CorrespondenceList> GetCorrespondencesByTicketId(string ticketId, int tenant)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Ticket", 0, true);

            IQueryable<CorrespondenceList> query = (from a in context.Correspondences.Include("CreatedByContact")
                                                    where a.Tenant == tenant && a.EntityId == ticketId && a.ObjectTableId == objectTable.Id

                                                    select new CorrespondenceList()
                                                     {
                                                         Id = a.Id,

                                                         Tenant = a.Tenant,

                                                         CreatedByContactId = a.CreatedByContactId,

                                                         CreateDate = a.CreateDate,

                                                         Description = a.Description,

                                                         IsInternal = a.IsInternal,

                                                         ObjectTableId = a.ObjectTableId,

                                                         EntityId = a.EntityId,

                                                         ContactName = a.CreatedByContact != null ? a.CreatedByContact.EnglishName : null,

                                                         ActivityId = a.ActivityId,

                                                         ActivitySubject = a.ActivitySubject,

                                                         ActivityTypeCode = a.ActivityTypeCode,

                                                         CCs = a.CCs,

                                                         Bcc = a.Bcc,

                                                         NotifyMe = a.NotifyMe,

                                                         NotifyOwner = a.NotifyOwner,

                                                         InternalUsers = a.InternalUsers,

                                                         ContactEmail = a.CreatedByContact != null ? a.CreatedByContact.Email : null,

                                                         Direction = a.Direction,

                                                         HTMLFullBody = a.HTMLFullBody,

                                                         RightToLeft = a.RightToLeft,
                                                     });
            return query.ToList();
        }
    }
}
