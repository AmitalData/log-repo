using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class GeneralDomainController : ApiController
    {
        public HttpResponseMessage GetTranslationsByParam(string typeCode, string tableId, string translationLanguageCode)
        {
            try
            {
                typeCode = this.FixFilter(typeCode);
                tableId = this.FixFilter(tableId);

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                GeneralDomainService service = new GeneralDomainService();
                List<FieldsTranslations> result = service.GetTranslationsByParamForCustomization(tenant, typeCode, tableId, translationLanguageCode);

                return Request.CreateResponse(HttpStatusCode.OK, result.OrderBy(d => d.ObjectTableName));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetStandardFieldsByTableId(string tableId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ObjectFieldRepository objectFieldsRepository = new ObjectFieldRepository(tenant);
                ObjectFieldQuery objectFieldsQuery = new ObjectFieldQuery(objectFieldsRepository);
                List<ObjectFieldPM> result = objectFieldsQuery.GetStandardFieldsFortableID(tableId, tenant, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result.OrderBy(d => d.FullNameTextCodeDefaultText));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomFieldsByTableId(string tableId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
               
                ObjectFieldRepository ObjectFieldsRepository = new ObjectFieldRepository(tenant);
                //this.ChangeConnectionString(tenant);
                ObjectFieldQuery objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
                List<ObjectFieldPM> result = objectFieldsQuery.GetCustomFieldsBytableID(tableId, tenant, tenant).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetFieldDataTypes()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
             
                DataTypeRepository dataTypeRepository = new DataTypeRepository(tenant);
                List<FieldDataType> result = dataTypeRepository.GetDataTypes().Where(d => d.Code != "Byte[]" && d.Code != "Emails" && d.Code != "Constant" && d.Code != "List" && d.Code != "SigDouble" && d.Code != "UnsDecimal" && d.Code != "UnsInteger").ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostObjectField(ObjectFieldPM objectField)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext ObjectContext = WebFreightContext.GetContext(objectField.Tenant);
                ObjectFieldService service = new ObjectFieldService(ObjectContext, objectField.Tenant);
                service.Create(objectField);
                ObjectTableRepository rep = new ObjectTableRepository(ObjectContext);
                rep.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, objectField);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutObjectField(ObjectFieldPM objectField)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext ObjectContext = WebFreightContext.GetContext(objectField.Tenant);
                ObjectFieldService service = new ObjectFieldService(ObjectContext, objectField.Tenant);
                service.Update(objectField, false);
                ObjectTableRepository rep = new ObjectTableRepository(ObjectContext);
                rep.SubmitChanges();
                return Request.CreateResponse(HttpStatusCode.OK, objectField);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTranslationsList(string typeCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
               
                GeneralDomainService domain = new GeneralDomainService();
                List<FieldsTranslations> result = domain.GetTranslationsForExport(tenant).Where(a => a.TypeCode == typeCode).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTranslationsByFilters_Old([FromUri] CustomApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string queryId = filters.queryId;
                int tenant = (int)filters.Tenant;
                string userid = filters.userid;
                string ObjectTableName = filters.ObjectTableName;

                QueryRepository queryRep = new QueryRepository(tenant);
                QueryQuery queryQuery = new QueryQuery(queryRep);
                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = ObjectTableName,
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                };

                List<ObjectField> ObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(ObjectTableName, tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                        ObjectField field = ObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }
                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = ObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                string codeLanguage = queryOperations.QueryFilterItems.Where(a => a.FieldName == "TranslationHeaderCode").Select(d => d.FieldValue).FirstOrDefault() as string;

                GeneralDomainService service = new GeneralDomainService();
                TranslationArgs result = service.GetAllFieldsTranslationsArgs(tenant, codeLanguage, queryOperations.PageIndex, queryOperations.PageSize);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCustomPickListsByCode(string code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

                var customPickListsRepository = new CustomPickListRepository(objectContext);
                var customPickListQuery = new CustomPickListQuery(customPickListsRepository);
                var temp = customPickListQuery.GetCustomPickListPMsByCode(tenant, code);
                return Request.CreateResponse(HttpStatusCode.OK, temp);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostPickList(PickListGeneralEntitiesArgs args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(authToken.Tenant);
                if (args.CustomPickListPMs != null && args.CustomPickListPMs.Count > 0)
                {
                    CustomPickListService customPickListService = new CustomPickListService(objectContext, authToken.Tenant);
                    foreach (var entityPM in args.CustomPickListPMs)
                    {
                        customPickListService.Create(entityPM);
                    }

                }

                TableLastUpdateClass.UpdateTableHistory(authToken.Tenant, "CustomPickList");

                return Request.CreateResponse(HttpStatusCode.OK, args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutPickList(PickListGeneralEntitiesArgs args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(authToken.Tenant);
                if (args.CustomPickListPMs != null && args.CustomPickListPMs.Count > 0)
                {
                    CustomPickListService customPickListService = new CustomPickListService(objectContext, authToken.Tenant);
                    foreach (var entityPM in args.CustomPickListPMs)
                    {
                        SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                        if (!string.IsNullOrEmpty(entityPM.Id))
                        {
                            customPickListService.Update(entityPM);
                        }
                        else
                        {
                            customPickListService.Create(entityPM);
                        }
                    }

                }

                if (args.RemovedCustomPickListPMs != null && args.RemovedCustomPickListPMs.Count > 0)
                {
                    CustomPickListService customPickListService = new CustomPickListService(objectContext, authToken.Tenant);
                    CustomPickListRepository repo = new CustomPickListRepository(authToken.Tenant);
                    foreach (var entityPM in args.RemovedCustomPickListPMs)
                    {
                        SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                        var Column = repo.GetSingleCustomPickList(entityPM.Id, entityPM.Tenant);
                        if (Column != null)
                        {
                            repo.Remove(Column);
                            repo.SubmitChanges();
                            customPickListService.ProcessPickListCToolMessage(entityPM, "DeleteCustomPickListValue");
                        }
                    }
                }

                TableLastUpdateClass.UpdateTableHistory(authToken.Tenant, "CustomPickList");

                return Request.CreateResponse(HttpStatusCode.OK, args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutFieldsTranslations(FieldsUpdateHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    
                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if (args != null)
                    {
                        if (args.Items.Count > 0)
                        {
                            GeneralDomainService service = new GeneralDomainService();

                            foreach (FieldsTranslations entity in args.Items)
                            {
                                service.UpdateFieldTranslation(entity);
                            }
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTextCodeTypes()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                TextCodeTypesRepository repository = new TextCodeTypesRepository(tenant);
                IQueryable<TextCodeType> result = repository.GetTextCodeTypes();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetAllFieldsTranslations(string language, string objectTableId, string textCodeType)
        {
            try
            {
                language = this.FixFilter(language);
                objectTableId = this.FixFilter(objectTableId);
                textCodeType = this.FixFilter(textCodeType);

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                GeneralDomainService service = new GeneralDomainService();
                List<FieldsTranslations> result = service.GetAllFieldsTranslationsByFilters(tenant, language, objectTableId, textCodeType);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string FixFilter(string filter)
        {
            string myResult = filter;

            if (myResult != null)
            {
                switch (myResult.ToLower())
                {
                    case "all":
                    case "null":
                    case "undefined":
                        {
                            myResult = null;
                            break;
                        }
                }
            }

            return myResult;
        }

        public HttpResponseMessage PutScreenFields(ScreenLayoutArgs args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                IWebFreightContext objectContext = WebFreightContext.GetContext(authToken.Tenant);
                IWebFreightContext ModobjectContext = WebFreightContext.GetContext(authToken.Tenant);


                ScreenFieldsQuery MyQuery = new ScreenFieldsQuery(authToken.Tenant);
                var MyTenantFields = MyQuery.GetScreenFieldPMsByTenant(authToken.Tenant).Where(a => a.Tenant != 0);
                ScreenFieldService MyService = new ScreenFieldService(objectContext, authToken.Tenant);
                ScreensRepository myRepo = new ScreensRepository(authToken.Tenant);
                var ScreenModification = myRepo.GetScreenModificationByScreen(args.ScreenCode, authToken.Tenant);
                if (ScreenModification == null)
                {
                    ScreenModification = new ScreenModification()
                    {
                        Tenant = authToken.Tenant,
                        ScreenId = args.ScreenId,
                        ScreenCode = args.ScreenCode,
                        NumberOfColumns = args.Columns,
                        NumberOfRows = args.Rows,
                        Id = IdCounter.GetNumber("ScreenModification", authToken.Tenant)
                    };
                    objectContext.ScreenModifications.Add(ScreenModification);
                    objectContext.SaveChanges();
                }
                else
                {
                    ScreenModification.NumberOfRows = args.Rows;
                    ScreenModification.NumberOfColumns = args.Columns;

                    myRepo.context.ScreenModifications.Attach(ScreenModification);
                    myRepo.context.SetAsModified(ScreenModification);
                    myRepo.context.SaveChanges();
                }
                if (args.ScreenFields != null && args.ScreenFields.Count > 0)
                {
                    foreach (var item in args.ScreenFields)
                    {
                        var temp = MyTenantFields.Where(a => a.ObjectFieldCode == item.ObjectFieldCode && a.ScreenCode == item.ScreenCode).FirstOrDefault();
                        if (temp != null)
                        {
                            item.Id = temp.Id;
                            MyService.Update(item);
                        }
                        else
                        {
                            item.Id = null;
                            item.Tenant = authToken.Tenant;
                            MyService.Create(item);
                        }
                    }
                }
                if (args.RemovedScreenFields != null && args.RemovedScreenFields.Count > 0)
                {
                    ScreenFieldsRepository repo = new ScreenFieldsRepository(authToken.Tenant);
                    foreach (var entityPM in args.RemovedScreenFields)
                    {
                        var field = repo.GetSingleScreenField(entityPM.Id);
                        if (field != null && field.Tenant != 0)
                        {
                            repo.Remove(field);
                            repo.SubmitChanges();
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetScreenModificationByScreenCode(string ScreenCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ScreensRepository myRepo = new ScreensRepository(authToken.Tenant);
                var temp = myRepo.GetScreenModificationByScreen(ScreenCode, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, temp);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSingleObjectFieldFromZeroTenant(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(0);
                ObjectFieldPM objectFieldPM = objectFieldQuery.GetSinglePM(id, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, objectFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSingleObjectFieldByFieldCodeFromZeroTenant(string fieldCode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(0);
                ObjectFieldPM objectFieldPM = objectFieldQuery.GetObjectFieldByFieldCode(fieldCode, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, objectFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSingleObjectFieldByFieldNameAndTableId(string fieldName, string tableId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(0);
                ObjectFieldPM objectFieldPM = objectFieldQuery.GetSinglePMByNameAndTable(fieldName, tableId, 0);

                return Request.CreateResponse(HttpStatusCode.OK, objectFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage GetObjectFieldModificationForLoggedTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                ObjectFieldRepository objectFieldsRepository = new ObjectFieldRepository(tenant);

                List<ObjectFieldModification> result = objectFieldsRepository.GetAllObjectFieldModificationByTenant(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}