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
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

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

        public FilingInboxQueryResult GetFilingInboxes_LastTwoMonths(string userId, bool isShowDeleted, int pageIndex, int pageSize, int tenant)
        {
            FilingInboxQueryResult iResult = new FilingInboxQueryResult()
            {
                Count = 0,
                Data = new List<FilingInboxPM>(),
            };

            ICommonDataContext iContext = CommonDataContext.GetContext(tenant);
            ContactRepository myContactRepository = new ContactRepository(iContext);
            DocumentRepository documentRepository = new DocumentRepository(iContext);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(iContext);
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(documentsFilingRepository);
            DateTime? startDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date.AddMonths(-2);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

            IQueryable<FilingInbox> iQueryable = (from a in iContext.FilingInboxes
                                                  where a.Tenant == tenant
                                                  && System.Data.Entity.DbFunctions.TruncateTime(a.CreateDate) >= startDate
                                                  select a);

            if (!string.IsNullOrEmpty(userId))
            {
                iQueryable = iQueryable.Where(d => d.Sender == userId);
            }

            if (!isShowDeleted)
            {
                iQueryable = iQueryable.Where(a => !a.IsDeleted);
            }

            iResult.Count = iQueryable.Count();

            if (iResult.Count > 0)
            {
                //iQueryable = iQueryable.Where(d => d.Id == "1-18076");

                iResult.Data = (from f in iQueryable

                                join db_Contacts in iContext.Contacts
                                on f.Sender equals db_Contacts.Id into FilingInboxsContacts
                                from iContact in FilingInboxsContacts

                                join db_Documents in iContext.Documents
                                on f.BodyDocumentId equals db_Documents.Id into FilingInboxsDocuments
                                from iDocument in FilingInboxsDocuments

                                select new FilingInboxPM()
                                {
                                    Id = f.Id,
                                    Tenant = f.Tenant,
                                    BodyDocumentId = f.BodyDocumentId,
                                    CreateDate = f.CreateDate,
                                    IsDeleted = f.IsDeleted,
                                    Sender = f.Sender,
                                    Subject = f.Subject,
                                    UpdateDate = f.UpdateDate,
                                    UpdatedByUserId = f.UpdatedByUserId,
                                    SearchFields = f.SearchFields,
                                    SenderName = iContact == null ? null : iContact.EnglishName,

                                    FileInfo = new BlobFileInfo()
                                    {
                                        Tenant = tenant,
                                        FileName = iDocument == null ? null : iDocument.Id,
                                        FolderName = iDocument == null ? null : iDocument.Folder,
                                        Extension = iDocument == null ? null : iDocument.Extension,
                                        FileSize = iDocument == null ? null : iDocument.FileSize,
                                    },

                                    FilingInboxAttachments = (from a in iContext.FilingInboxAttachments
                                                              where a.Tenant == tenant
                                                              && a.FilingInboxId == f.Id
                                                              select new FilingInboxAttachmentPM()
                                                              {
                                                                  Id = a.Id,
                                                                  Tenant = a.Tenant,
                                                                  DocumentId = a.DocumentId,
                                                                  FileName = a.FileName,
                                                                  FilingInboxId = a.FilingInboxId,

                                                                  AttachLogs = (from d in iContext.FilingInboxAttachmentLogs

                                                                                join db_DocumentsFilings in iContext.DocumentsFilings
                                                                                on d.DocumentsFilingId equals db_DocumentsFilings.Id into DocumentsFilings
                                                                                from iDocumentsFiling in DocumentsFilings

                                                                                join db_DocumentTypes in iContext.DocumentTypes
                                                                                on iDocumentsFiling.DocumentTypeId equals db_DocumentTypes.Id into DocumentTypes
                                                                                from iDocumentType in DocumentTypes

                                                                                where d.Tenant == tenant
                                                                                && d.FilingInboxAttachmentId == a.Id
                                                                                select new FilingInboxAttachToolTip()
                                                                                {
                                                                                    DocumentName = iDocumentType == null ? null : iDocumentType.Name,
                                                                                    ObjectTableId = iDocumentType == null ? null : iDocumentType.ObjectTableId,
                                                                                    EntityNumber = iDocumentsFiling == null ? null : "# " + iDocumentsFiling.EntityNumber,
                                                                                }).ToList(),

                                                              }).ToList(),
                                })                        
                                .OrderByDescending(o => o.CreateDate)                        
                                .Skip(pageIndex)                        
                                .Take(pageSize)                        
                                .ToList();

                IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                List<ObjectTable> AllObjectTables = new List<ObjectTable>();

                foreach (FilingInboxPM item in iResult.Data)
                {
                    byte[] fileXml = storageservice.Read(item.FileInfo);

                    if (fileXml != null)
                    {
                        item.EmailBody = UTF8Encoding.UTF8.GetString(fileXml, 0, fileXml.Length);
                    }

                    foreach (FilingInboxAttachmentPM itemAttachment in item.FilingInboxAttachments)
                    {
                        foreach (FilingInboxAttachToolTip itemAttachmentToolTip in itemAttachment.AttachLogs)
                        {
                            if (itemAttachmentToolTip.ObjectTableId != null)
                            {
                                ObjectTable iObjectTable = AllObjectTables.Where(d => d.Id == itemAttachmentToolTip.ObjectTableId).FirstOrDefault();
                                if (iObjectTable == null)
                                {
                                    iObjectTable = (from s in webFreightContext.ObjectTables where s.Id == itemAttachmentToolTip.ObjectTableId select s).FirstOrDefault();
                                    if (iObjectTable != null)
                                    {
                                        AllObjectTables.Add(iObjectTable);
                                    }
                                }

                                if (iObjectTable != null)
                                {
                                    itemAttachmentToolTip.EntityNumber = iObjectTable.Name + itemAttachmentToolTip.EntityNumber;
                                }
                            }
                        }
                    }
                }
            }

            return iResult;
        }




        public IQueryable<FilingInboxPM> GetFilingInboxes_LastTwoMonths_Old(string userId, bool isShowDeleted, int tenant)
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
                    attach.AttachLogs = this.GetAttachLogsOfFilingInbox_Old(attach.Id, attach.Tenant);
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
                    if (fileXml != null)
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
        private List<FilingInboxAttachToolTip> GetAttachLogsOfFilingInbox_Old(string attachId, int tenant)
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
    public string ObjectTableId { get; set; }
    public string EntityNumber { get; set; }
    public string DocumentName { get; set; }
}

public class FilingInboxQueryResult
{
    public int Count { get; set; }
    public List<FilingInboxPM> Data { get; set; }
}

