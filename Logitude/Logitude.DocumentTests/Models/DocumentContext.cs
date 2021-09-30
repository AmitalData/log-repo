using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Models
{
    public class DocumentContext
    {
        public List<DocumentTypeList> DocumentTypes { get; internal set; }
        public DocumentsFilingPM Document { get; internal set; }
        public string FileSizeFormat { get; internal set; }
        public int FileSize { get; internal set; }
        public int UploadedFileSize { get; internal set; }
        public string DocumentSecurityId { get; internal set; }
        public HttpStatusCode? StatusCode { get; internal set; }
        public DocumentsFilingPM DocumentCopy { get; internal set; }
        public DocumentOutPM DocumentOut { get; internal set; }
        public string DocumentOutCopyId { get; internal set; }
        public string DocumentOutSecurityId { get; internal set; }
        public string SendDocumentOutResult { get; internal set; }
    }
}
