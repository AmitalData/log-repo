using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class RequiredDocumentResponseData : ResponseDataBase
    {
        public string ApplicationID { get; set; }
        public string Title { get; set; }
        public string VerificationDecisionType { get; set; }
        public string DocumentType { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeVisibility { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentWorkerName { get; set; }
        public string Remarks { get; set; }
        public string ReplacingDocumentId { get; set; }
        public string ReplacingDocumentIdVisibility { get; set; }
        public List<DocumentConnectedEntitiesResult> DocumentConnectedEntitiesList { get; set; }
    }

    public class DocumentConnectedEntitiesResult
    {
        public string EntityType { get; set; }
        public string EntityTypeName { get; set; }
        public string EntityNumber { get; set; }
    }
}
