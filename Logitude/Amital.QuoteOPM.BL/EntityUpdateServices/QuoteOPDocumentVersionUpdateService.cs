using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.BL.QuoteModel.EntityOtherServices;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
      public partial  class QuoteOPDocumentVersionUpdateService
    {
        protected override void OnCreating(QuoteOPDocumentVersionPM itemPM, QuoteOPPM entityParentPM)
        {
            var lastVersion = (this.Repository as QuoteOPDocumentVersionRepository).GetQuoteDocumentVersionsByQuoteId(itemPM.QuoteOPId, itemPM.Tenant).OrderBy(s => s.VersionNumber).ToList().LastOrDefault();
            if (lastVersion != null)
            {
                itemPM.VersionNumber = lastVersion.VersionNumber + 1;
            }
            else
            {
                itemPM.VersionNumber = 1;
            }
            itemPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            itemPM.UpdateDate = itemPM.CreateDate;

            CreateQuoutePDFDocument(itemPM);
#warning CreateQuoutePDFDocument(itemPM);
           


            base.OnCreating(itemPM, entityParentPM);
        }

        private static void CreateQuoutePDFDocument(QuoteOPDocumentVersionPM itemPM)
        {
#if false


            QuoteTemplateEntityService quoteTemplateService = new QuoteTemplateEntityService();
            byte[] pdfData = new byte[] { };
            if (itemPM.VersionType == "G")
            {
                pdfData = quoteTemplateService.GetQuoteTemplatePdfReport(itemPM.QuoteId, itemPM.QuoteTemplateId, itemPM.CreatedByUserId, itemPM.Tenant, null, null, itemPM.VersionNumber);
            }

            DocumentRepository documentRep = new DocumentRepository(myCommonContext);
            Document document = new Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "pdf",
                FileSize = Convert.ToInt32(pdfData.Length),
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", itemPM.Tenant).ToString(),
                Folder = "quotetemplatesectionfiles",
                CalculatedFileName = "Quotation-" + entityPM.QuoteNumber + "-" + itemPM.VersionNumber,
                HasFile = true,
            };
            documentRep.Add(document);
            documentRep.SubmitChanges();

            itemPM.DocumentId = document.Id;


            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = document.FileSize,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(pdfData, fileInfo);
             this.entityPM.LastVersionNumber = itemPM.VersionNumber;
            this.entityPM.QuoteTemplateId = itemPM.QuoteTemplateId;

#endif
        }

        protected override void OnUpdating(QuoteOPDocumentVersionPM itemPM, Data.EntityPOCOs.QuoteOPDocumentVersion itemPoco)
        {
            
            

            
            string entityName = "QuoteDocumentVersion" + itemPM.QuoteOPId + itemPM.Tenant + itemPM.VersionNumber;
            string entityPmName = "QuoteDocumentVersionPM" + itemPM.QuoteOPId + itemPM.Tenant + itemPM.VersionNumber;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            itemPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            if (itemPM.IsSent && !itemPoco.IsSent)
            {
                itemPM.SendDate = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant);
            }


            base.OnUpdating(itemPM, itemPoco);
        }
    }
}
