using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
    public class ExportDocumentArgs
    {

        public string DocumentTypeTemplateId { get; set; }
        public int Tenant { get; set; }
        public string CurrentDocumentOutId { get; set; }
        public string CurrentDocumentTypeCode { get; set; }
        public string DocumentTypeCopyId { get; set; }
        public string EntityId { get; set; }
        public bool IsDisplayOnly { get; set; }
        public string LoggedContactId { get; set; }
        public string ObjectTableId { get; set; }
        public int PageNumber { get; set; }
        public string ReportKey { get; set; }
        public string RequestMethodType { get; set; }
        public string ChildEntityId { get; set; }
        public string ProcessType { get; set; }
        public string ChildObjectTableId { get; set; }
        public string LoggedContactName { get; set; }
        public string AccountingCurrencyId { get; set; }
        public List<string> DocumentTypeCopyIdsList { get; set; }
        public string DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public string ObjectTableName { get; set; }
        public string DocumentTemplateEditorTool { get; set; }



    }
}
