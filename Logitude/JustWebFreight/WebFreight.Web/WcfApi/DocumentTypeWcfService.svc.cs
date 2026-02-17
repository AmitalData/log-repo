using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using WebFreight.Web.Security;
using Logitude.BL.Validators;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DocumentTypeWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DocumentTypeWcfService.svc or DocumentTypeWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DocumentTypeWcfService : IDocumentTypeWcfService
    {

        public Response Upsert(DocumentTypePM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                ICommonDataContext commoncontext = CommonDataContext.GetContext(entityPM.Tenant);
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(entityPM.Tenant);
                ObjectTableRepository objecttableRepository = new ObjectTableRepository(webFreightContext);
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commoncontext);
                DocumentTypeService service = new DocumentTypeService(commoncontext, entityPM.Tenant);


                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (!string.IsNullOrEmpty(entityPM.ObjectTableName))
                    {
                        var objectTable = objecttableRepository.GetObjectTableByName(entityPM.ObjectTableName, 0, false);
                        if (objectTable != null)
                        {
                            entityPM.ObjectTableId = objectTable.Id;
                        }
                        if (String.IsNullOrWhiteSpace(entityPM.ObjectTableName))
                        {
                            throw new Exception("entityPM.ObjectTableName ( " + entityPM.ObjectTableName + " ) not exist in tenant 0 ");
                        }
                    }
                    else
                    {
                        throw new Exception("entityPM.ObjectTableName ( " + entityPM.ObjectTableName + " ) is Required");
                    }


                    ClassLevelValidator validationClass = new ClassLevelValidator("DocumentType", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }


                    entityPM.IsHybrid = true;
                    DocumentType entity = documentTypeRepository.GetSingleDocumentTypeByCode(entityPM.Code, entityPM.Tenant);
                    if (entity == null)
                    {
                        service.Create(entityPM);
                    }
                    else
                    {
                        entityPM.Id = entity.Id;
                        service.Update(entityPM, entityPM.DocumentTypeCopies);
                    }
                    response.Result = entityPM.Id;
                    scope.Complete();
                    return response;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }
                return response;
            }
        }

        public List<DocumentTypeList> GetDocumentTypes(string objectTableName, int tenant, int skip, int take, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("DocumentType", "READ", tenant);

                List<DocumentTypeList> result = new List<DocumentTypeList>();
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);
                ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

                ObjectTable table = null;
                if (!string.IsNullOrEmpty(objectTableName))
                {
                    table = objecttableRepository.GetObjectTableByName(objectTableName, 0, false);

                    if (table == null)
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Object table not found!";
                        return null;
                    }
                }

                IQueryable<DocumentType> queryList = null;
                if (table != null)
                {
                    queryList = documentTypeRepository.GetDocumentTypesByObjectTableId(tenant, table.Id).OrderBy(c => c.Code).Skip(skip).Take(take);
                }
                else
                {
                    queryList = documentTypeRepository.GetDocumentTypes(tenant).OrderBy(c => c.Code).Skip(skip).Take(take);
                }


                result = documentTypeQuery.GetIQueryableEntityList(queryList).ToList();



                return result;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }



        public DocumentTypePM GetDocumentTypeByCode(string code, int tenant, ref Response response)
        {
            response = new Response();
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("DocumentType", "READ", tenant);

                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                IWebFreightContext webfreightContext = WebFreightContext.GetContext(tenant);
                DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commoncontext);
                ObjectTableRepository objecttableRepository = new ObjectTableRepository(webfreightContext);
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);

                ObjectTable table = null;

                DocumentTypePM result = documentTypeQuery.GetSinglePMByCodeAndTenant(code, tenant);

                if (result != null)
                {
                    table = objecttableRepository.GetSingleObjectTable(result.ObjectTableId, result.Tenant, false);
                    result.ObjectTableName = table.Id;

                }

                return result;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }
    }
}

