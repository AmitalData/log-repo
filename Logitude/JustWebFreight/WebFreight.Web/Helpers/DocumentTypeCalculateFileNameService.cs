using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DocumentTypeCalculateFileNameService
    {
        private DocumentFileNameParameter documentFileNameParameter;
        private string fileName = string.Empty;
        public DocumentTypeCalculateFileNameService(DocumentFileNameParameter documentFileNameParameter)
        {
            this.documentFileNameParameter = documentFileNameParameter;
        }


        public DocumentTypeCalculateFileNameService(DocumentsFiling documentsFiling , string fileName)
        {

                this.fileName = fileName;
                documentFileNameParameter = GetDocumentFileNameParameter(documentsFiling.Tenant, documentsFiling);
        }



        private static DocumentFileNameParameter GetDocumentFileNameParameter(int tenant, DocumentsFiling externalDocument)
        {
            return new DocumentFileNameParameter
            {
                EntityId = externalDocument.EntityId,
                EntityObjectTableId = externalDocument.ObjectTableId,
                ChildEntityId = externalDocument.ChildEntityId,
                Tenant = tenant,
                UserId = externalDocument.CreatedByUserId,
                DocumentType = externalDocument.DocumentType,
            };
        }


        public string Calculate()
        {

            if (string.IsNullOrEmpty(documentFileNameParameter.DocumentType.FileName)) return null;
            if (documentFileNameParameter.DocumentType.FileName.Contains("[")) return GetDocumentFileNameFromDataFields();

            return documentFileNameParameter.DocumentType.FileName.Length > 120 ? documentFileNameParameter.DocumentType.FileName.Substring(0, 119): documentFileNameParameter.DocumentType.FileName;
           
        }

        private  string GetDocumentFileNameFromDataFields()
        {
            string calculatedFileName = ResolveMainObjectTableFields();
            calculatedFileName = ResolveDocumentTypeFields(calculatedFileName);
            calculatedFileName = ResolveDocumentFilingFields(calculatedFileName);
            return calculatedFileName.Length > 120 ? calculatedFileName.Substring(0, 119) : calculatedFileName;
 
            
        }

        private string ResolveMainObjectTableFields()
        {
            HtmlEditorResolveArgs htmlResolveArgs = new HtmlEditorResolveArgs();
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();

            string id = documentFileNameParameter.EntityId;
            if (!string.IsNullOrEmpty(documentFileNameParameter.ChildEntityId))
                id = documentFileNameParameter.ChildEntityId;

            htmlResolveArgs.EntityId = id;
            htmlResolveArgs.ObjectTableId = documentFileNameParameter.DocumentType.ObjectTableId;
            htmlResolveArgs.HtmlString = documentFileNameParameter.DocumentType.FileName;
            htmlResolveArgs.UserId = documentFileNameParameter.UserId;
            htmlResolveArgs.Tenant = documentFileNameParameter.Tenant;

            string htmlResolve = htmlEditorHelper.ResolveHtmlString(htmlResolveArgs);
            return htmlResolve;
        }


        private  string ResolveDocumentFilingFields( string documentFileName)
        {
            string calculatedDocumentFileName = documentFileName.Replace("[DocumentsFilingFileName]" , fileName);
            if (calculatedDocumentFileName.Contains("[DocumentsFiling"))
            {
                calculatedDocumentFileName = ResolvemDocumentsFilingsFields(calculatedDocumentFileName);
            }

            return calculatedDocumentFileName;
        }

        private  string ResolveDocumentTypeFields(string documentFileName)
        {
            string calculatedDocumentFileName = documentFileName.Replace("[DocumentTypeCopyName]", documentFileNameParameter?.DocumentTypeCopy?.Name);
            if (calculatedDocumentFileName.Contains("[DocumentType"))
            {
                DocumentFileNameFromObjectTableParameter documentFileNameFromObjectTableParameter = new DocumentFileNameFromObjectTableParameter
                {
                    DocumentFileNameParameter = documentFileNameParameter,
                    DocumentFileName = calculatedDocumentFileName,
                    EntityId = documentFileNameParameter.DocumentType.Id,
                    ObjectTableName = "DocumentType",
                };
                calculatedDocumentFileName = GetDocumentFileNameFromDataFieldsByObjectTableName(documentFileNameFromObjectTableParameter);
            }

            return calculatedDocumentFileName;
        }

        private  string ResolvemDocumentsFilingsFields ( string documentFileName)
        {
            string calculatedDocumentFileName = documentFileName;

            string id = documentFileNameParameter.EntityId;
            if (!string.IsNullOrEmpty(documentFileNameParameter.ChildEntityId))
                id = documentFileNameParameter.ChildEntityId;

            Logitude.BL.CommonDataModel.EntityQueries.DocumentsFilingQuery documentsFilingQuery = new Logitude.BL.CommonDataModel.EntityQueries.DocumentsFilingQuery(documentFileNameParameter.Tenant);
            DocumentsFilingPM documentsFilingPM = documentsFilingQuery.GetDocumentsFilingByDocumentType(documentFileNameParameter.DocumentType.Id, documentFileNameParameter.DocumentType.ObjectTableId, id, documentFileNameParameter.Tenant);
            if (documentsFilingPM != null)
            {
                DocumentFileNameFromObjectTableParameter documentFileNameFromObjectTableParameter = new DocumentFileNameFromObjectTableParameter
                {
                    DocumentFileNameParameter = documentFileNameParameter,
                    DocumentFileName = calculatedDocumentFileName,
                    EntityId = documentsFilingPM.Id,
                    ObjectTableName = "DocumentsFiling",
                };
                calculatedDocumentFileName = GetDocumentFileNameFromDataFieldsByObjectTableName(documentFileNameFromObjectTableParameter);
            }

            return calculatedDocumentFileName;
        }

        private  string GetDocumentFileNameFromDataFieldsByObjectTableName(DocumentFileNameFromObjectTableParameter documentFileNameFromObjectTableParameter)
        {
            HtmlEditorResolveArgs htmlResolveArgs = new HtmlEditorResolveArgs();
            HtmlEditorHelper htmlEditorHelper = new HtmlEditorHelper();

            htmlResolveArgs.EntityId = documentFileNameFromObjectTableParameter.EntityId;
            htmlResolveArgs.ObjectTableName = documentFileNameFromObjectTableParameter.ObjectTableName;
            htmlResolveArgs.HtmlString = documentFileNameFromObjectTableParameter.DocumentFileName.Replace("[" + documentFileNameFromObjectTableParameter.ObjectTableName, "[");
            htmlResolveArgs.UserId = documentFileNameFromObjectTableParameter.DocumentFileNameParameter.UserId;
            htmlResolveArgs.Tenant = documentFileNameFromObjectTableParameter.DocumentFileNameParameter.Tenant;

            string resolveDocumentFileName = htmlEditorHelper.ResolveHtmlString(htmlResolveArgs);
            return resolveDocumentFileName;
        }


    }
}