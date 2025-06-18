using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public partial class DocumentPM
    {
        public DocumentPM() : base() { }
        public DocumentPM(Document entity) : base()
        {
            Id = entity.Id;
            Tenant = entity.Tenant;
            FileName = entity.FileName;
            CreateDate = entity.CreateDate;
            Extension = entity.Extension;
            FileSize = entity.FileSize;
            HasFile = entity.HasFile;
            Folder = entity.Folder;
            SmallDocumentId = entity.SmallDocumentId;
            CalculatedFileName = entity.CalculatedFileName;
            IsEncrypted = entity.IsEncrypted;
            MarkForDelete = entity.MarkForDelete;
            SmallDocument = entity.SmallDocument;
        }
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string FileName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Extension { get; set; }
        public double? FileSize { get; set; }
        public bool HasFile { get; set; }
        public string Folder { get; set; }
        public string SmallDocumentId { get; set; }
        public string CalculatedFileName { get; set; }
        public bool IsEncrypted { get; set; }
        public bool? MarkForDelete { get; set; }
        [ForeignKey("SmallDocumentId")]
        public SmallDocument SmallDocument { get; set; }
    }
}
