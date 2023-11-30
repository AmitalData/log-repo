using System;

namespace Logitude.DocumentTests.Models
{
    public static class DocumentAPIUrls
    {
        public static string PostSendHtmlDocument = "HtmlEditor/postsendhtmldocument";

        public static string GetCreateDocumentsFiling(DocumentsFilingArgs arguments)
        {
            return $"DocumentsFilingExtended/GetCreateDocumentsFiling?documentTypeId={arguments.DocumentTypeId}&entityId={arguments.EntityId}&childEntityId=&childReference=&objectTableId={arguments.ObjectTableId}&directionCode={arguments.DirectionCode}&tenant={arguments.Tenant}";
        }
        public static string GetCreateDocumentsOut(DocumentsFilingArgs arguments)
        {
            return $"DocumentOutExtended/getcreatedocumentout?documentTypeId={arguments.DocumentTypeId}&entityId={arguments.EntityId}&childEntityId=&childReference=&objectTableId={arguments.ObjectTableId}&directionCode={arguments.DirectionCode}&tenant={arguments.Tenant}";
        }
 
        public static string GetDocumentCopy(DocumentCopyArgs getDocumentCopy)
        {
            return $"ExportDocument?documentTypeId={getDocumentCopy.DocumentTypeId}&entityId={getDocumentCopy.EntityId}&entityObjectTableId={getDocumentCopy.EntityObjectTableId}&childEntityId=&childObjectTableId=&documentOutId={getDocumentCopy.DocumentOutId}&tenant={getDocumentCopy.Tenant}&documentTypeCopyId={getDocumentCopy.DocumentTypeCopyId}&userId={getDocumentCopy.UserId}";
        }

     

    }
}