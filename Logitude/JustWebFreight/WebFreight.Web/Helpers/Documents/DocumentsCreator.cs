using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.Documents
{
    public class DocumentsCreator
    {
        public static string Create(DocumentsCreatorArgs documentsCreatorArgs)
        {
            if (!string.IsNullOrEmpty(documentsCreatorArgs.DocumentId)) return documentsCreatorArgs.DocumentId;
            DocumentRepository documentRepository = new DocumentRepository(documentsCreatorArgs.Tenant);
            Document newDocument = GetNewDocument(documentsCreatorArgs);

            documentRepository.Add(newDocument);
            documentRepository.SubmitChanges();

            return newDocument.Id;

        }

        private static Document GetNewDocument(DocumentsCreatorArgs documentsCreatorArgs)
        {
            return new Document
            {
                Id = IdCounter.GetNumber("Document", documentsCreatorArgs.Tenant).ToString(),
                Tenant = documentsCreatorArgs.Tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(documentsCreatorArgs.Tenant),
                FileName = documentsCreatorArgs.FileName,
                FileSize = documentsCreatorArgs.FileData.Length,
                Extension = documentsCreatorArgs.FileExtension,
                HasFile = true,
                CalculatedFileName = documentsCreatorArgs.FileName,
                Folder = documentsCreatorArgs.FileFolder,

            };
        }
    }
    public class DocumentsCreatorArgs
    {
        public int Tenant { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileFolder { get; set; }
        public byte[] FileData { get; set; }
        public string DocumentId { get; set; }
    }
}