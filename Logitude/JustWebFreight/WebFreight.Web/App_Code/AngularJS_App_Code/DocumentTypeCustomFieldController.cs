using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code.AngularJS_App_Code
{
    public class DocumentTypeCustomFieldController : ApiController
    {
        public HttpResponseMessage GetFormCustomFieldsByDocument(int tenant, string documentTypeId, string entityId, string entityTypeId)
        {
            try
            {

                Authentication();
                FormCustomFieldRepository formCustomFieldRepository = new FormCustomFieldRepository(tenant);
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(tenant);
                DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(documentTypeCustomFieldRepository);

                List<FormCustomFieldPM> fromCustomFieldsList = new List<FormCustomFieldPM>();
                List<DocumentTypeCustomFieldPM> customFieldsList = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(documentTypeId, tenant).ToList();

                foreach (DocumentTypeCustomFieldPM customField in customFieldsList)
                {
                    FormCustomFieldQuery formCustomFieldQuery = new FormCustomFieldQuery(formCustomFieldRepository);
                    DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(customField.DocumentTypeId, tenant);
                    FormCustomFieldPM formCustomField = formCustomFieldQuery.GetSingleFormCustomFieldsPMByDocument(tenant, documentTypeId, customField.FieldCode, entityId, entityTypeId);

                    if (formCustomField != null)
                    {
                        fromCustomFieldsList.Add(formCustomField);
                    }
                    else
                    {
                        formCustomField = new FormCustomFieldPM()
                        {
                            Tenant = tenant,
                            DocumentTypeId = documentTypeId,
                            EntityId = entityId,
                            ObjectTableId = docType.ObjectTableId,
                            FieldCode = customField.FieldCode,
                            Value = customField.DefaultValue,
                            Id = customField.FieldCode,
                        };
                        fromCustomFieldsList.Add(formCustomField);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, fromCustomFieldsList);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDocumentTypeCustomFieldsByDocument(int tenant, string documentTypeId)
        {
            try
            {
                Authentication();
                DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
                List<DocumentTypeCustomFieldPM> reslut = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(documentTypeId, tenant).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, reslut);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutFormCustomField(FormCustomFieldPM formCustomFieldPM)
        {
            try
            {
                Authentication();
                if (formCustomFieldPM.Id != formCustomFieldPM.FieldCode)
                {
                    ICommonDataContext objectContext = CommonDataContext.GetContext(formCustomFieldPM.Tenant);
                    FormCustomFieldService service = new FormCustomFieldService(objectContext, formCustomFieldPM.Tenant);
                    service.Update(formCustomFieldPM);
                    TableLastUpdateClass.UpdateTableHistory(formCustomFieldPM.Tenant, "FormCustomField");
                }

                else
                {
                    InsertFormCustomField(formCustomFieldPM);
                }
                return Request.CreateResponse(HttpStatusCode.OK, formCustomFieldPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public void InsertFormCustomField(FormCustomFieldPM entityPM)
        {
            
                Authentication();
                ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                if (objectContext == null)
                {
                    objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                }

                FormCustomFieldService service = new FormCustomFieldService(objectContext, entityPM.Tenant);
                service.Create(entityPM);

                TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "FormCustomField");
           
        }




        public HttpResponseMessage PutDocumentTypeCustomField(DocumentTypeCustomFieldPM documentTypeCustomFieldPM)
        {
            try
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(documentTypeCustomFieldPM.Tenant);
                DocumentTypeCustomFieldService service = new DocumentTypeCustomFieldService(objectContext, documentTypeCustomFieldPM.Tenant);
                service.Update(documentTypeCustomFieldPM);
                TableLastUpdateClass.UpdateTableHistory(documentTypeCustomFieldPM.Tenant, "DocumentTypeCustomField");

                return Request.CreateResponse(HttpStatusCode.OK, documentTypeCustomFieldPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage PostDocumentTypeCustomField(DocumentTypeCustomFieldPM documentTypeCustomFieldPM)
        {
            try
            {
                ICommonDataContext objectContext = CommonDataContext.GetContext(documentTypeCustomFieldPM.Tenant);
                DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(objectContext);

                bool exist = (from a in documentTypeCustomFieldRepository.GetDocumentTypeCustomFields(documentTypeCustomFieldPM.Tenant)
                              where a.FieldCode == documentTypeCustomFieldPM.FieldCode && a.Tenant == documentTypeCustomFieldPM.Tenant && a.DocumentTypeId == documentTypeCustomFieldPM.DocumentTypeId
                              select a).Any();
                if (!exist)
                {
                    DocumentTypeCustomFieldService service = new DocumentTypeCustomFieldService(objectContext, documentTypeCustomFieldPM.Tenant);
                    service.Create(documentTypeCustomFieldPM);
                    TableLastUpdateClass.UpdateTableHistory(documentTypeCustomFieldPM.Tenant, "DocumentTypeCustomField");
                }

                else
                {
                    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", documentTypeCustomFieldPM.Tenant);
                    msg = msg.Replace("%Entity", "Field");
                    throw new Exception(msg);
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentTypeCustomFieldPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private static void Authentication()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            //SecurityUtility.CheckContactFeature("DocumentTypeCustomField", "READ", authToken.Tenant);
        }
    }
}