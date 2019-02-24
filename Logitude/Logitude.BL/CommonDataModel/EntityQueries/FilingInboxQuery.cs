using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class FilingInboxQuery
    {
        FilingInboxRepository repository;
        public FilingInboxQuery(int tenant)
        {
            repository = new FilingInboxRepository(tenant);
        }
        public FilingInboxQuery(FilingInboxRepository EntityChangeRepository)
        {
            repository = EntityChangeRepository;
        }
        public FilingInboxPM GetSinglePM(string id, int tenant)
        {
            var query = (from a in repository.context.FilingInboxes
                         where a.Tenant == tenant && a.Id == id
                         select new FilingInboxPM()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             BodyDocumentId = a.BodyDocumentId,
                             CreateDate = a.CreateDate,
                             IsDeleted = a.IsDeleted,
                             Sender = a.Sender,
                             Subject = a.Subject,
                             UpdateDate = a.UpdateDate,
                             UpdatedByUserId = a.UpdatedByUserId,
                             SearchFields = a.SearchFields,
                         }).FirstOrDefault();
            return query;
        }
        public IQueryable<FilingInboxList> GetIQueryableEntityList(IQueryable<FilingInbox> iQueryable)
        {
            IQueryable<FilingInboxList> result = from a in iQueryable
                                                 select new FilingInboxList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     BodyDocumentId = a.BodyDocumentId,
                                                     CreateDate = a.CreateDate,
                                                     IsDeleted = a.IsDeleted,
                                                     Sender = a.Sender,
                                                     Subject = a.Subject,
                                                     UpdateDate = a.UpdateDate,
                                                     UpdatedByUserId = a.UpdatedByUserId,
                                                     SearchFields = a.SearchFields,
                                                 };
            return result;
        }

        public IQueryable<FilingInboxPM> GetFilingInboxes_LastTwoMonths(string userId, bool isShowDeleted, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository myContactRepository = new ContactRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            Document document;
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date.AddMonths(-2);
            IQueryable<FilingInboxPM> result;

            if (string.IsNullOrEmpty(userId))
            {
                result = (from a in repository.context.FilingInboxes
                          where a.Tenant == tenant && System.Data.Entity.DbFunctions.TruncateTime(a.CreateDate) >= todayDate
                          select new FilingInboxPM()
                          {
                              Id = a.Id,
                              Tenant = a.Tenant,
                              BodyDocumentId = a.BodyDocumentId,
                              CreateDate = a.CreateDate,
                              IsDeleted = a.IsDeleted,
                              Sender = a.Sender,
                              Subject = a.Subject,
                              UpdateDate = a.UpdateDate,
                              UpdatedByUserId = a.UpdatedByUserId,
                              SearchFields = a.SearchFields,
                          });
            }
            else
            {
                result = (from a in repository.context.FilingInboxes
                          where a.Tenant == tenant && System.Data.Entity.DbFunctions.TruncateTime(a.CreateDate) >= todayDate
                          && a.Sender == userId
                          select new FilingInboxPM()
                          {
                              Id = a.Id,
                              Tenant = a.Tenant,
                              BodyDocumentId = a.BodyDocumentId,
                              CreateDate = a.CreateDate,
                              IsDeleted = a.IsDeleted,
                              Sender = a.Sender,
                              Subject = a.Subject,
                              UpdateDate = a.UpdateDate,
                              UpdatedByUserId = a.UpdatedByUserId,
                              SearchFields = a.SearchFields,
                          });
            }

            if (!isShowDeleted)
            {
                result = result.Where(a => !a.IsDeleted);
            }

            var list = result.OrderByDescending(a => a.CreateDate).ToList();

            foreach (var item in list)
            {
                var attachments = (from a in repository.context.FilingInboxAttachments
                                   where a.Tenant == tenant && a.FilingInboxId == item.Id
                                   select new FilingInboxAttachmentPM()
                                   {
                                       Id = a.Id,
                                       Tenant = a.Tenant,
                                       DocumentId = a.DocumentId,
                                       FileName = a.FileName,
                                       FilingInboxId = a.FilingInboxId,

                                   }).ToList();
                item.FilingInboxAttachments = attachments;

                foreach (var attach in item.FilingInboxAttachments)
                {
                    attach.AttachLogs = this.GetAttachLogsOfFilingInbox(attach.Id, attach.Tenant);
                }

                document = documentRepository.GetSingleDocument(tenant, item.BodyDocumentId);
                if (document != null)
                {
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = document.FileSize,
                    };
                    byte[] fileXml = storageservice.Read(fileInfo);
                    if (fileXml.Length > 0)
                    {
                        item.EmailBody = UTF8Encoding.UTF8.GetString(fileXml, 0, fileXml.Length);
                    } 
                }

                var contact = myContactRepository.GetSingleContact(item.Sender, tenant);
                item.SenderName = contact != null ? contact.EnglishName : "";
            }

            IQueryable<FilingInboxPM> iQueryableList;
            if (list != null)
            {
                iQueryableList = list.AsQueryable();
            }
            else
            {
                iQueryableList = null;
            }
            return iQueryableList;
        }

        private List<FilingInboxAttachToolTip> GetAttachLogsOfFilingInbox(string attachId, int tenant)
        {
            List<FilingInboxAttachToolTip> list = null;
            DocumentsFilingQuery DocumentsFilingQuery = new DocumentsFilingQuery(tenant);
            List<string> documentsFilingIds = (from a in repository.context.FilingInboxAttachmentLogs
                                               where a.Tenant == tenant && a.FilingInboxAttachmentId == attachId
                                               select a.DocumentsFilingId).ToList();
            if (documentsFilingIds != null && documentsFilingIds.Count() > 0)
            {
                List<DocumentsFilingPM> documentsFilings = DocumentsFilingQuery.GetDocumentsFilingsByIds(String.Join(",", documentsFilingIds), tenant);
                list = (from a in documentsFilings
                        select new FilingInboxAttachToolTip
                        {
                            EntityNumber = a.ObjectTableName + "# " + a.EntityNumber,
                            DocumentName = a.DocumentTypeName,
                        }).ToList();
            }
            return list;
        }
    }
}

public class FilingInboxAttachToolTip
{
    public string EntityNumber { get; set; }
    public string DocumentName { get; set; }
}

