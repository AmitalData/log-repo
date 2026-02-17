using System;
using System.Linq;
using Logitude.BL.QuoteModel.EntityOtherServices;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.DataMapping;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteDocumentVersionService
    {
        bool isNewEntity;
        private int tenant;
        public QuoteDocumentVersion Poco { get; set; }

        public IQuotesContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuoteDocumentVersionPM entityPm;
        private IQuotesContext objectContext;
        private QuoteDocumentVersionRepository entityRepository;

        public QuoteDocumentVersionService(IQuotesContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuoteDocumentVersionRepository(objectContext);
        }

        public void Create(QuoteDocumentVersionPM entityPM)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            this.isNewEntity = true;
            this.entityPm = entityPM;

            this.Poco = new QuoteDocumentVersion();

            QuoteDocumentVersion lastVersion = entityRepository.GetQuoteDocumentVersionsByQuoteId(entityPM.QuoteId, entityPM.Tenant).OrderBy(s => s.VersionNumber).ToList().LastOrDefault();
            if (lastVersion != null)
            {
                entityPM.VersionNumber = lastVersion.VersionNumber + 1;
            }
            else
            {
                entityPM.VersionNumber = 1;
            }

            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.UpdateDate = entityPM.CreateDate;

            QuoteTemplateEntityService quoteTemplateService = new QuoteTemplateEntityService();

            byte[] pdfData = new byte[] { };
            if (entityPM.VersionType == "G")
            {
                pdfData = quoteTemplateService.GetQuoteTemplatePdfReport(entityPM.QuoteId, entityPM.QuoteTemplateId, entityPM.CreatedByUserId, entityPM.Tenant,null);
            }

            DocumentRepository documentRep = new DocumentRepository(commonContext);
            Document document = new Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "pdf",
                FileSize = Convert.ToInt32(pdfData.Length),
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", entityPM.Tenant).ToString(),
                Folder = "quotetemplatesectionfiles",
                HasFile = true,
            };
            documentRep.Add(document);
            documentRep.SubmitChanges();

            entityPM.DocumentId = document.Id;

            //string localPath = "quotetemplatesectionfiles/" + document.Id + "." + document.Extension;
            //var blobContainer = StorageAcountDetails.GetCurrentContainer(tenant);
            //var blobfile = blobContainer.GetBlockBlobReference(localPath);
            //using (Stream blobstream = blobfile.OpenWrite())
            //{
            //    blobstream.Write(pdfData, 0, (int)pdfData.Length);
            //}

            string filename = document.Id + "." + document.Extension;
            string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(filename.ToLower(), document.Folder);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = pdfData.Length,

            };
            storageservice.Write(pdfData, fileInfo);
            

            QuoteRepository quoteRep = new QuoteRepository(objectContext);
            Quote quote = quoteRep.GetSingleQuote(entityPM.QuoteId, entityPM.Tenant);
            quote.LastVersionNumber = entityPM.VersionNumber;
            quote.QuoteTemplateId = entityPM.QuoteTemplateId;


            QuoteDocumentVersionMapping.MappingQuoteDocumentVersion(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();


            ObjectTableRepository tableRepository = new ObjectTableRepository(tenant);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
            DocumentOutRepository documentOutRepository = new DocumentOutRepository(commonContext);
            DocumentTypeCopyRepository documentTypeCopyRepository = new DocumentTypeCopyRepository(commonContext);
            DocumentOutCopyRepository documentOutCopyRepository = new DocumentOutCopyRepository(commonContext);
            DocumentsFilingRepository documentsFilingRepository = new DocumentsFilingRepository(commonContext);
            DocumentType documentType = documentTypeRepository.GetSingleDocumentTypeByCode("QUOTE", tenant);
            if (documentType != null)
            {
                DocumentOut documentout = documentOutRepository.GetDocumentOutByDocumentTypeAndEntity(entityPm.QuoteId, documentType.Id, tenant);
                if (documentout == null)
                {
                    ObjectTable table = tableRepository.GetObjectTableByName("Quote", 0, true);

                    DocumentsFiling newDocumentFiling = new DocumentsFiling() { DocumentTypeId = documentType.Id, EntityId = entityPm.QuoteId, Tenant = tenant, ObjectTableId = table.Id, ChildEntityId = null, ChildEntityReference = null, DirectionCode = "O" };
                    newDocumentFiling.SearchFields = newDocumentFiling.Code + "," + newDocumentFiling.DirectionCode;
                    newDocumentFiling.Id = IdCounter.GetNumber("Document", tenant).ToString();
                    newDocumentFiling.Code = CodeCounter.GetNumber("DocumentsFiling", tenant).ToString();
                    newDocumentFiling.CreatedByUserId = entityPM.UpdatedByUserId;
                    newDocumentFiling.OwnerId = entityPM.UpdatedByUserId;
                    newDocumentFiling.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.UpdatedByUserId = entityPM.UpdatedByUserId;
                    newDocumentFiling.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newDocumentFiling.HasCopies = true;

                    documentsFilingRepository.Add(newDocumentFiling);

                    documentout = new DocumentOut()
                    {
                        Id = newDocumentFiling.Id,
                        EmailTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId,
                        DocumentTemplateId = documentType.DocumentTypeDefaultReportTemplateId,
                        Tenant = tenant,
                        Issued = true,
                        IsBlobExist = true,
                    };


                    //documentout.Id = IdCounter.GetNumber("Document", tenant).ToString();
                    documentOutRepository.Add(documentout);

                    DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopyByDocumentTypeId(documentType.Id, tenant);
                    if (documentTypeCopy == null)
                    {
                        documentTypeCopy = new DocumentTypeCopy()
                        {
                            Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                            DocumentTypeId = documentType.Id,
                            Code = documentType.Code,
                            Name = documentType.Name,
                            Tenant = tenant,

                        };

                        documentTypeCopyRepository.Add(documentTypeCopy);
                    }

                    DocumentOutCopy docoutCopy = new DocumentOutCopy()
                    {
                        Id = document.Id,
                        DocumentId = document.Id,
                        DocumentOutId = documentout.Id,
                        DocumentTypeCopyId = documentTypeCopy.Id,
                        Tenant = tenant,

                    };

                    documentOutCopyRepository.Add(docoutCopy);
                }
                else
                {
                    //DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.context.DocumentTypeCopies.Where(d => d.DocumentTypeId == documentType.Id).FirstOrDefault();
                    DocumentOutCopy docoutCopy = (from a in commonContext.DocumentOutCopies
                                                  where a.DocumentOutId == documentout.Id && a.Tenant == tenant
                                                  select a).FirstOrDefault();//documentOutCopyRepository.GetDocumentOutCopyByDocumentOutAndType(documentout.Id, documentTypeCopy.Id, tenant);
                    if (docoutCopy != null)
                    {
                        docoutCopy.DocumentId = document.Id;
                    }
                    else
                    {
                        DocumentTypeCopy documentTypeCopy = documentTypeCopyRepository.GetSingleDocumentTypeCopyByDocumentTypeId(documentType.Id, tenant);
                        if (documentTypeCopy == null)
                        {
                            documentTypeCopy = new DocumentTypeCopy()
                            {
                                Id = IdCounter.GetNumber("DocumentTypeCopy", tenant).ToString(),
                                DocumentTypeId = documentType.Id,
                                Code = documentType.Code,
                                Name = documentType.Name,
                                Tenant = tenant,

                            };

                            documentTypeCopyRepository.Add(documentTypeCopy);
                        }

                        docoutCopy = new DocumentOutCopy()
                        {
                            Id = document.Id,
                            DocumentId = document.Id,
                            DocumentOutId = documentout.Id,
                            DocumentTypeCopyId = documentTypeCopy.Id,
                            Tenant = tenant,

                        };

                        documentOutCopyRepository.Add(docoutCopy);
                    }
                }

                commonContext.SaveChanges();

            }
            else
            {
                throw new Exception("Document Type with code 'QUOTE' is not found!");
            }


        }




        public void Update(QuoteDocumentVersionPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleQuoteDocumentVersion(entityPm.QuoteId, entityPm.Tenant, entityPm.VersionNumber);

            string entityName = "QuoteDocumentVersion" + entityPM.QuoteId + entityPM.Tenant + entityPM.VersionNumber;
            string entityPmName = "QuoteDocumentVersionPM" + entityPM.QuoteId + entityPM.Tenant + entityPM.VersionNumber;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }
            //QuoteTemplateSettingValidating.Validate(entityPM);
            //if (!entityPM.IsHybrid)
            //{
            //    QuoteTemplateSettingTracing.Trace(entityPM, Poco, isNewEntity);
            //}


            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            QuoteDocumentVersionMapping.MappingQuoteDocumentVersion(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }


    }
}
