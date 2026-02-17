using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class FilingInboxAttachmentQuery
    {
        FilingInboxAttachmentRepository repository;
        public FilingInboxAttachmentQuery(int tenant)
        {
            repository = new FilingInboxAttachmentRepository(tenant);
        }
        public FilingInboxAttachmentQuery(FilingInboxAttachmentRepository EntityChangeRepository)
        {
            repository = EntityChangeRepository;
        }

        public FilingInboxAttachmentPM GetSinglePM(string id, int tenant)
        {
            var query = (from a in repository.context.FilingInboxAttachments
                         where a.Tenant == tenant && a.Id == id
                         select new FilingInboxAttachmentPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             DocumentId = a.DocumentId,
                             FileName = a.FileName,
                             FilingInboxId = a.FilingInboxId,
                         }).FirstOrDefault();
            return query;
        }

        public IQueryable<FilingInboxAttachmentList> GetIQueryableEntityList(IQueryable<FilingInboxAttachment> iQueryable)
        {
            IQueryable<FilingInboxAttachmentList> result = from a in iQueryable
                                                 select new FilingInboxAttachmentList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     DocumentId = a.DocumentId,
                                                     FileName = a.FileName,
                                                     FilingInboxId = a.FilingInboxId,
                                                 };
            return result;
        }

        public List<FilingInboxAttachmentPM> GetListByFilingId(string filingId, int tenant)
        {
            var query = (from a in repository.context.FilingInboxAttachments
                         where a.Tenant == tenant && a.FilingInboxId == filingId
                         select new FilingInboxAttachmentPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             DocumentId = a.DocumentId,
                             FileName = a.FileName,
                             FilingInboxId = a.FilingInboxId,
                         }).ToList();
            return query;
        }
    }
}
