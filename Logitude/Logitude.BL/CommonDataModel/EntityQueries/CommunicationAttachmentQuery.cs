using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CommunicationAttachmentQuery
    {
        CommunicationAttachmentRepository repository;



        public CommunicationAttachmentQuery(int tenant)
        {
            repository = new CommunicationAttachmentRepository(tenant);
        }

        public CommunicationAttachmentQuery(CommunicationAttachmentRepository communicationAttachmentRepository)
        {
            repository = communicationAttachmentRepository;
        }

        public CommunicationAttachmentPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.CommunicationAttachments
                    where a.Id == id && a.Tenant == tenant
                    select new CommunicationAttachmentPM()
                    {
                        CommunicationLogId = a.CommunicationLogId,
                        DocumentId = a.DocumentId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                    }).FirstOrDefault();
        }

        public IQueryable<CommunicationAttachmentPM> GetCommunicationAttachmentPMsByTenant(int tenant)
        {
            return (from a in repository.context.CommunicationAttachments
                    where a.Tenant == tenant
                    select new CommunicationAttachmentPM()
                    {
                        CommunicationLogId = a.CommunicationLogId,
                        DocumentId = a.DocumentId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                    });
        }

        public IQueryable<CommunicationAttachmentPM> GetCommunicationAttachmentPMsByLogId(string id, int tenant)
        {
            return (from a in repository.context.CommunicationAttachments
                    where a.Tenant == tenant && a.CommunicationLogId == id
                    select new CommunicationAttachmentPM()
                    {
                        CommunicationLogId = a.CommunicationLogId,
                        DocumentId = a.DocumentId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                    });
        }

        public IQueryable<CommunicationAttachmentList> GetIQueryableEntityList(IQueryable<CommunicationAttachment> iQueryable)
        {
            IQueryable<CommunicationAttachmentList> result = from entity in iQueryable
                                                             select new CommunicationAttachmentList()
                                                            {
                                                                 Id = entity.Id,
                                                                 Tenant = entity.Tenant,
                                                                 CommunicationLogId = entity.CommunicationLogId,
                                                                 DocumentId = entity.DocumentId,
                                                            };
            return result;
        }

        public IQueryable<CommunicationAttachmentPM> GetCommunicationAttachmentsByCommunicationLogId(string communicationLogId, int tenant)
        {
      
            return (from a in repository.context.CommunicationAttachments
                    where a.Tenant == tenant && a.CommunicationLogId == communicationLogId
                    select new CommunicationAttachmentPM()
                    {
                        CommunicationLogId = a.CommunicationLogId,
                        DocumentId = a.DocumentId,
                        Id = a.Id,
                        Tenant = a.Tenant,
                    });
     
        }
    }
}