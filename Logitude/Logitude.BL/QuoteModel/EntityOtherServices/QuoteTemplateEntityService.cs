using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.Practices.Unity;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.EntityOtherServices
{
    public class QuoteTemplateEntityService
    {
        public string UploadQuoteTemplateSectionDataFile(byte[] data, string documentId, int tenant)
        {
           
            DocumentRepository docRepository = new DocumentRepository(tenant);
            long fileSize = data.Length;
            Simplog.Data.CommonDataModel.EntityPOCOs.Document document = null;
            if (string.IsNullOrEmpty(documentId))
            {
                document = new Simplog.Data.CommonDataModel.EntityPOCOs.Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "html",
                    FileSize = Convert.ToInt32(fileSize),
                    Tenant = tenant,
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = "quotetemplatesectionfiles",
                };

                docRepository.Add(document);
                docRepository.SubmitChanges();
            }
            else
            {
                document = docRepository.GetSingleDocument(tenant, documentId);
            }
            
            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = document.Tenant,
                FileSize = document.FileSize,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(data, fileInfo);
            
            return document.Id;
        }

        public byte[] GetQuoteTemplatePdfReport(string quoteId, string quoteTemplateId, string userId, int tenant, List<QuoteTemplateSectionPM> templateSections, int? userTenant = null)
        {
            IQuoteTemplateReportHelper helper = ContainerAccessor.Container.Resolve(typeof(IQuoteTemplateReportHelper), "QuoteTemplateReportHelper", new ParameterOverride("", 1)) as IQuoteTemplateReportHelper;
            return helper.BuildQuoteTemplatePdfReport(quoteId, quoteTemplateId, userId, tenant, templateSections , userTenant);
            
        }
    }
}
