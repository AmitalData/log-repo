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
    public class FilingInboxAttachmentLogQuery
    {
        FilingInboxAttachmentLogRepository repository;
        public FilingInboxAttachmentLogQuery(int tenant)
        {
            repository = new FilingInboxAttachmentLogRepository(tenant);
        }
        public FilingInboxAttachmentLogQuery(FilingInboxAttachmentLogRepository EntityChangeRepository)
        {
            repository = EntityChangeRepository;
        }

        public FilingInboxAttachmentLogPM GetSinglePM(string id, int tenant)
        {
            var query = (from a in repository.context.FilingInboxAttachmentLogs
                         where a.Tenant == tenant && a.Id == id
                         select new FilingInboxAttachmentLogPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             DocumentsFilingId = a.DocumentsFilingId,
                             FilingInboxAttachmentId = a.FilingInboxAttachmentId,
                         }).FirstOrDefault();
            return query;
        }
        public IQueryable<FilingInboxAttachmentLogList> GetIQueryableEntityList(IQueryable<FilingInboxAttachmentLog> iQueryable)
        {
            IQueryable<FilingInboxAttachmentLogList> result = from a in iQueryable
                                                           select new FilingInboxAttachmentLogList()
                                                           {
                                                               Id = a.Id,
                                                               Tenant = a.Tenant,
                                                               DocumentsFilingId = a.DocumentsFilingId,
                                                               FilingInboxAttachmentId = a.FilingInboxAttachmentId,
                                                           };
            return result;
        }
    }
}
