using CWXSD;
using Intuit.Ipp.LinqExtender.Ast;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.Controllers.CommonDataModel.Generated.ListControllers;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using static Logitude.CustomsMessaging.Helpers.CloneUtil;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class ComputingPartnerViewsExtendedController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetByFiltersGrouping([FromUri] CustomApiQueryFilters filters)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                if (filters.Tenant != null)
                {
                    SecurityUtility.AuthenticationOnTenant(filters.Tenant.Value);
                }

                String SearchFields = null;
                int Tenant = authToken.Tenant;
                SecurityUtility.CheckContactFeature("ComputingPartnerTranslation", "READ", 0);
                ApiQueryFilters TableFilters = new ApiQueryFilters();
                TableFilters.GetAll = true;
                var ClientModuleName = filters.ClinetName;
                Object TableController;
                Type magicType;
                if (!string.IsNullOrWhiteSpace(filters.objectTableName)) filters.objectTableName = filters.objectTableName.Replace("Customs.", "");

                TableController = GetInstance("Logitude." + ClientModuleName + ".BL.EntityQueryServices." + (!string.IsNullOrEmpty(filters.ParentObjectTableName) ? filters.ParentObjectTableName : filters.objectTableName) + "QueryService," + "Logitude." + ClientModuleName + ".BL", Tenant);
                if (TableController == null)
                {
                    TableController = GetInstance("Logitude.BL." + ClientModuleName + "DataModel.EntityQueries." + (!string.IsNullOrEmpty(filters.ParentObjectTableName) ? filters.ParentObjectTableName : filters.objectTableName) + "Query,Logitude.BL", Tenant);

                }

                if (TableController == null)
                {
                    TableController = GetInstance("Logitude.BL." + ClientModuleName + "Model.EntityQueries." + (!string.IsNullOrEmpty(filters.ParentObjectTableName) ? filters.ParentObjectTableName : filters.objectTableName) + "Query,Logitude.BL", Tenant);

                }

                magicType = Type.GetType("Logitude." + ClientModuleName + ".BL.EntityQueryServices." + (!string.IsNullOrEmpty(filters.ParentObjectTableName) ? filters.ParentObjectTableName : filters.objectTableName) + "QueryService," + "Logitude." + ClientModuleName + ".BL");
                if (magicType == null)
                {
                    magicType = Type.GetType("Logitude.BL." + ClientModuleName + "DataModel.EntityQueries." + (!string.IsNullOrEmpty(filters.ParentObjectTableName) ? filters.ParentObjectTableName : filters.objectTableName) + "Query,Logitude.BL");
                }

                if (magicType == null)
                {
                    magicType = Type.GetType("Logitude.BL." + ClientModuleName + "Model.EntityQueries." + (!string.IsNullOrEmpty(filters.ParentObjectTableName) ? filters.ParentObjectTableName : filters.objectTableName) + "Query,Logitude.BL");
                }



                MethodInfo magicMethod;
                var myTableDataList = new List<object>();
                if (!filters.IsClosedTable)
                {
                    magicMethod = magicType.GetMethod("Get" + filters.objectTableName + "PMsByTenant");
                    object[] param = new object[] { 100 };
                    param[0] = Tenant;
                    myTableDataList = ((IQueryable<object>)magicMethod.Invoke(TableController, param)).ToList();
                }
                else
                {
                    magicMethod = magicType.GetMethod("Get" + filters.objectTableName + "PMs");
                    myTableDataList = ((IQueryable<object>)magicMethod.Invoke(TableController, null)).ToList();
                }
                //    myTableDataList = this.GetFilterQuery(filters, myTableDataList);
                List<ComputingPartnerTranslationList> TranslationList = TranslationList = GetByFilters(filters, Tenant);
                if (TranslationList == null)
                    TranslationList = new List<ComputingPartnerTranslationList>();

                List<ComputingPartnerTranslationList> DefaultTranslations = DefaultTranslations = GetByFilters(filters, 0);
                if (DefaultTranslations == null)
                    DefaultTranslations = new List<ComputingPartnerTranslationList>();

                List<TranslateItemClass> Obslist = new List<TranslateItemClass>();

                ObjectTableQuery query = new ObjectTableQuery(Tenant);
                string objectTableName = (!string.IsNullOrEmpty(filters.ParentObjectTableName) ? filters.ParentObjectTableName : filters.objectTableName);
                ObjectTablePM objectTablePM = query.GetObjectTableByName(objectTableName, Tenant);
                if (myTableDataList.Count > 0)
                {
                    string prop = "Code";
                    string propName = "Name";

                    if (objectTablePM != null)
                    {
                        prop = objectTablePM.CodeField;
                        propName = objectTablePM.NameField;
                        // prop = "Code";
                        //propName = "Name";

                    }
                    Type type = myTableDataList.First().GetType();
                    PropertyInfo property = type.GetProperty(prop);
                    PropertyInfo property2 = propName != null ? type.GetProperty(propName) : null;
                    PropertyInfo Active = type.GetProperty("InActive");

                    if (property != null)
                    {
                        foreach (object item in myTableDataList)
                        {
                            object itemCode = property.GetValue(item, null);
                            bool IsActive = true;
                            if (Active != null)
                            {
                                IsActive = !(bool)Active.GetValue(item, null);

                            }
                            if (IsActive)
                            {
                                object itemName = null;
                                if (property2 != null)
                                    itemName = property2.GetValue(item, null);

                                if (itemCode != null)
                                {
                                    string itemCodeString = itemCode.ToString();
                                    string itemNameString = itemName != null ? itemName.ToString() : "";
                                    ComputingPartnerTranslationList myTranslationPM = TranslationList.Where(d => d.OurCode == itemCodeString).FirstOrDefault();
                                    TranslateItemClass MYItem;
                                    if (myTranslationPM == null)
                                    {
                                        DateTime todayDateTime = DateTime.UtcNow;

                                        MYItem = new TranslateItemClass()
                                        {
                                            Tenant = Tenant,
                                            OurCode = itemCodeString,
                                            ComputingPartnerId = filters.computingPartnerId,
                                            ComputingPartnerName = filters.ComputingPartnerName,
                                            ObjectTableId = (!string.IsNullOrEmpty(filters.ParentObjectTableId) ? filters.ParentObjectTableId : filters.objectTableId),
                                            ObjectTableName = (!string.IsNullOrEmpty(filters.ParentObjectTableName) ? filters.ParentObjectTableName : filters.objectTableName),
                                            CreateDate = todayDateTime,
                                            UpdateDate = todayDateTime,
                                            CreatedByUserId = filters.LoggedContactId,
                                            UpdatedByUserId = filters.LoggedContactId,
                                            SearchFields = itemCodeString + ",",
                                            Name = itemNameString,
                                        };
                                    }
                                    else
                                    {
                                        MYItem = new TranslateItemClass()
                                        {
                                            Tenant = myTranslationPM.Tenant,
                                            OurCode = myTranslationPM.OurCode,
                                            PartnerCode = myTranslationPM.PartnerCode,
                                            ComputingPartnerId = myTranslationPM.ComputingPartnerId,
                                            ComputingPartnerName = myTranslationPM.ComputingPartnerName,
                                            ObjectTableId = myTranslationPM.ObjectTableId,
                                            ObjectTableName = myTranslationPM.ObjectTableName,
                                            CreateDate = myTranslationPM.CreateDate,
                                            UpdateDate = myTranslationPM.UpdateDate,
                                            CreatedByUserId = myTranslationPM.CreatedByUserId,
                                            UpdatedByUserId = myTranslationPM.UpdatedByUserId,
                                            Id = myTranslationPM.Id,
                                            SearchFields = myTranslationPM.SearchFields,
                                            Name = itemNameString,
                                        };

                                    }

                                    ComputingPartnerTranslationList myDefaultTranslationPM = null;

                                    if (Tenant != 0)
                                    {
                                        myDefaultTranslationPM = DefaultTranslations.Where(d => d.OurCode == itemCodeString).FirstOrDefault();
                                        if (myDefaultTranslationPM != null)
                                        {
                                            MYItem.DefaultTranslationCode = myDefaultTranslationPM.OurCode;
                                            MYItem.DefaultTranslationPartnerCode = myDefaultTranslationPM.PartnerCode;
                                            MYItem.CreatedDateDefault = myDefaultTranslationPM.CreateDate;
                                            MYItem.UpdatedDateDefault = myDefaultTranslationPM.UpdateDate;
                                            MYItem.CreatedByUserNameDefault = myDefaultTranslationPM.CreatedByUserName;
                                            MYItem.UpdatedByUserNameDefault = myDefaultTranslationPM.UpdatedByUserName;
                                            MYItem.DefaultId = myDefaultTranslationPM.Id;
                                            MYItem.SearchFields = myDefaultTranslationPM.SearchFields;
                                            MYItem.Name = itemNameString;
                                        }
                                    }

                                    Obslist.Add(MYItem);
                                }
                            }
                        }
                    }
                }





                ServiceResponse response = new ServiceResponse();
                Obslist.Sort(new Comparison<TranslateItemClass>((x, y) => String.Compare(y.PartnerCode, x.PartnerCode)));
                if (filters.SearchingFields != null && filters.SearchingFields != "null" && filters.SearchingFields != "undefined")

                    Obslist = Obslist.Where(fl => (fl.OurCode.ToLower().StartsWith(filters.SearchingFields.ToLower())) || (fl.PartnerCode != null && fl.PartnerCode.ToLower().StartsWith(filters.SearchingFields.ToLower()))).ToList();

                response.Result = Obslist;
                response.Count = Obslist.Count;

                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;


            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public List<ComputingPartnerTranslationList> GetByFilters(CustomApiQueryFilters filters, int Tenant)
        {
            try
            {
                string computingPartnerId = filters.computingPartnerId;
                string objectTableId = (!string.IsNullOrEmpty(filters.ParentObjectTableId) ? filters.ParentObjectTableId : filters.objectTableId);


                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "ComputingPartnerTranslation",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "ComputingPartnerTranslations",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> ComputingPartnerTranslationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ComputingPartnerTranslation", Tenant);
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
                        ObjectField field = ComputingPartnerTranslationObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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
                        ObjectField field = ComputingPartnerTranslationObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
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


                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();

                ICommonDataContext MyContext = CommonDataContext.GetContext(Tenant);
                ComputingPartnerTranslationRepository computingPartnerRepository = new ComputingPartnerTranslationRepository(MyContext);
                IQueryable<ComputingPartnerTranslation> entityPocos = computingPartnerRepository.GetComputingPartnerTranslations(Tenant);

                ComputingPartnerTranslationQuery computingPartnerQuery = new ComputingPartnerTranslationQuery(computingPartnerRepository);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                entityPocos = genericFilter.GetFilteredQuery<ComputingPartnerTranslation>(nonListQueryOperation, entityPocos);
                int skippedEntities = queryOperations.PageIndex;
                IQueryable<ComputingPartnerTranslationList> entityLists = computingPartnerQuery.GetIQueryableEntityList(entityPocos, computingPartnerId, objectTableId, Tenant);
                entityLists = genericFilter.GetFilteredQuery<ComputingPartnerTranslationList>(listQueryOperation, entityLists);
                if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
                {
                    PropertyInfo propInfo = typeof(ComputingPartnerTranslationList).GetProperty(queryOperations.SortByColumnName);


                    ObjectField objectField = (from a in ComputingPartnerTranslationObjectFields
                                               where a.FieldName == queryOperations.SortByColumnName
                                               select a).FirstOrDefault();

                    if (objectField != null)
                    {
                        if (objectField.IsCustom)
                        {
                            entityLists = sortClass.GetSorterQuery<ComputingPartnerTranslationList, string>(queryOperations, entityLists);
                        }
                        else
                        {
                            switch (objectField.DataTypeCode.ToLower())
                            {
                                case "ntext":
                                case "text":
                                case "lookup":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ComputingPartnerTranslationList, string>(queryOperations, entityLists);
                                        break;
                                    }
                                case "sigdouble":
                                case "double":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ComputingPartnerTranslationList, double>(queryOperations, entityLists);
                                        break;
                                    }
                                case "date":
                                case "datetime":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ComputingPartnerTranslationList, DateTime>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsinteger":
                                case "integer":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ComputingPartnerTranslationList, int>(queryOperations, entityLists);
                                        break;
                                    }
                                case "boolean":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ComputingPartnerTranslationList, bool>(queryOperations, entityLists);
                                        break;
                                    }
                                case "unsdecimal":
                                case "decimal":
                                    {
                                        entityLists = sortClass.GetSorterQuery<ComputingPartnerTranslationList, decimal>(queryOperations, entityLists);
                                        break;
                                    }
                                default:
                                    {
                                        entityLists = entityLists.OrderBy(d => d.Id);
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                {
                    entityLists = entityLists.OrderBy(d => d.Id);
                }

                if (!queryOperations.GetAll)
                {

                    entityLists = entityLists.Skip(skippedEntities);
                    entityLists = entityLists.Take(queryOperations.PageSize);

                }
                List<ComputingPartnerTranslationList> listResult = entityLists.ToList();
                return listResult;

            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public object GetInstance(string strFullyQualifiedName, int Tenant)
        {
            object[] param = new object[] { 100 };
            param[0] = Tenant;
            Type t = Type.GetType(strFullyQualifiedName);
            if (t == null)
                return null;
            return Activator.CreateInstance(t, param);
        }

        private List<object> GetFilterQuery(CustomApiQueryFilters filters, List<object> myTableDataList, int Tenant)
        {
            if (!string.IsNullOrEmpty(filters.AdditionalFilters))
            {
                JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);
                List<ObjectField> ComputingPartnerTranslationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(filters.objectTableName, Tenant);

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = filters.objectTableName,
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = filters.objectTableName,
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                foreach (QueryFilterItem filter in filters_list)
                {
                    ObjectField field = ComputingPartnerTranslationObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
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
                GenericFilter genericFilter = new GenericFilter();
                QueryOperations nonListQueryOperation = new QueryOperations();
                var TableController = GetInstance("Logitude.BL.CommonDataModel.EntityLists." + filters.objectTableName + ",Logitude.BL", Tenant);

                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                Type type = Type.GetType("Logitude.BL.CommonDataModel.EntityLists." + filters.objectTableName + ",Logitude.BL");
                myTableDataList = genericFilter.GetFilteredQuery<object>(nonListQueryOperation, myTableDataList.AsQueryable()).ToList();


            }
            return myTableDataList;

        }
    }



    public class CustomApiQueryFilters
    {

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public bool GetCount { get; set; }
        public int? Tenant { get; set; }
        public bool IsClosedTable { get; set; }
        public string ClinetName { get; set; }
        public string LoggedContactId { get; set; }
        public string ComputingPartnerName { get; set; }
        public string computingPartnerId { get; set; }
        public string objectTableId { get; set; }
        public string SearchingFields { get; set; }
        public string Filter1Name { get; set; }
        public string Filter1Value { get; set; }
        public string Filter1Operator { get; set; }
        public string objectTableName { get; set; }
        public string Filter2Name { get; set; }
        public string Filter2Value { get; set; }
        public string Filter2Operator { get; set; }
        public string SearchFields { get; set; }
        public string Filter3Name { get; set; }
        public string Filter3Value { get; set; }
        public string Filter3Operator { get; set; }

        public string Filter4Name { get; set; }
        public string Filter4Value { get; set; }
        public string Filter4Operator { get; set; }

        public string Filter5Name { get; set; }
        public string Filter5Value { get; set; }
        public string Filter5Operator { get; set; }

        public string Filter6Name { get; set; }
        public string Filter6Value { get; set; }
        public string Filter6Operator { get; set; }

        public string Filter7Name { get; set; }
        public string Filter7Value { get; set; }
        public string Filter7Operator { get; set; }

        public string Filter8Name { get; set; }
        public string Filter8Value { get; set; }
        public string Filter8Operator { get; set; }

        public string Filter9Name { get; set; }
        public string Filter9Value { get; set; }
        public string Filter9Operator { get; set; }

        public string Filter10Name { get; set; }
        public string Filter10Value { get; set; }
        public string Filter10Operator { get; set; }

        public string AdditionalFilters { get; set; }


        public bool GetAll { get; set; }
        public string ParentObjectTableName { get; set; }
        public string ParentObjectTableId { get; set; }
    }

    public class TranslateItemClass
    {
        public string Id { get; set; }
        public string DefaultId { get; set; }
        public string OurCode { get; set; }
        public string PartnerCode { get; set; }
        public string DefaultTranslationCode { get; set; }
        public string DefaultTranslationPartnerCode { get; set; }
        public int Tenant { get; set; }
        public string ComputingPartnerId { get; set; }
        public string ComputingPartnerName { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime? CreatedDateDefault { get; set; }
        public DateTime? UpdatedDateDefault { get; set; }
        public string SearchFields { get; set; }
        public string CreatedByUserNameDefault { get; set; }
        public string UpdatedByUserNameDefault { get; set; }

    }
}
