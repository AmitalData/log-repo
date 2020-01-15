using Logitude.SystemLogs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers
{
    public class TableQueryReflector
    {
        private static Dictionary<string, string> LogitudeModelNames
        {
            get
            {
                Dictionary<string, string> modelName = new Dictionary<string, string>();
                modelName.Add("Logitude.Accounting", "AccountingContext");
                modelName.Add("Logitude.BookingLib", "BookingContext");
                modelName.Add("Logitude.CRM", "CRMContext");
                modelName.Add("Logitude.Customs", "CustomContext");
                modelName.Add("Logitude.Social", "SocialContext");
                modelName.Add("Logitude.TimeManagement", "TimeManagementContext");

                return modelName;
            }
        }

        public static object GetTableListData(string tableName, int tenant = 0,string modelName = null)
        {

            if (tableName == "DescriptionOfGoods")
            {
                tableName = "DescriptionOfGood";
            }

            ObjectTableRepository obRepository = new ObjectTableRepository(0);
            ObjectTable table = obRepository.GetObjectTableByName(tableName, 0, true);

            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            FilterSerializer filterSerializer = new FilterSerializer();
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = new QueryOperations();
            queryOperations.QueryFilterItems = new List<QueryFilterItem>();
            queryOperations.GetAll = true;
            queryOperations.PageIndex = 0;
            queryOperations.GetAll = true;
            queryOperations.SortDirectin = "ascending";
            queryOperations.SortByColumnName = table.SortingByObjectField;
            if (string.IsNullOrEmpty(queryOperations.SortByColumnName))
                queryOperations.SortByColumnName = table.LookUp1;

            MethodInfo getListMethodInfo = null;
            MethodInfo getCountMethodInfo = null;
            System.Linq.IQueryable querableEntities = null;

            byte[] xmlFilters = null;
            object context = null;

            var MethodsInfo = getMethodsInfo("WebFreight.Web.ShipmentsModel.DomainServices.ShipmentsDomainService", tableName);
            bool stop = false;
            if (MethodsInfo != null && LogitudeSettings.WorkEnvironment!="customs")
            {
                getListMethodInfo = MethodsInfo.ListMethodInfo;
                getCountMethodInfo = MethodsInfo.CountMethodInfo;
                context = MethodsInfo.context;
                stop = true;
            }
            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.CommonDataModel.DomainServices.CommonDataDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }
            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.InfrastructureModel.DomainServices.WebFreightDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }
            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.QuoteModel.DomainServices.QuotesDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }
            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.GlobalModel.GlobalDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }
            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.CommonDataModel.DomainServices.ContactDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }
            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.CommonDataModel.DomainServices.PartnersDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
                MethodsInfo = getMethodsInfo("WebFreight.Web.CommonDataModel.DomainServices.PaymentTermDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }

            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.InvoiceModel.DomainServices.InvoiceDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }

            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.CRMModel.DomainServices.CRMDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }

            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.BookingModel.DomainServices.BookingsDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }

            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.AccountingModel.DomainServices.AccountingDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }


            if (stop == false)
            {
                if (tableName.Contains("."))
                    tableName = tableName.Split('.')[1];

                MethodsInfo = getMethodsInfo("WebFreight.Web.CustomModel.DomainServices.CustomDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }


            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.AccountingModel.DomainServices." + tableName + "DomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }

            if (stop == false)
            {
                MethodsInfo = getMethodsInfo("WebFreight.Web.WarehouseModel.DomainServices.WarehousesDomainService", tableName);
                if (MethodsInfo != null)
                {
                    getListMethodInfo = MethodsInfo.ListMethodInfo;
                    getCountMethodInfo = MethodsInfo.CountMethodInfo;
                    context = MethodsInfo.context;
                    stop = true;
                }
            }

            if (stop == false)
            {

                querableEntities = GetTableListForGeneratedQueries(table, tenant);
                
            }

            else
            {
                if (getListMethodInfo != null && getCountMethodInfo != null)
                {
                    //Get data count
                    xmlFilters = filterSerializer.SerializeFilterItems(queryOperations);
                    object[] parameters = new object[] { xmlFilters, tenant };
                    if (getCountMethodInfo.GetParameters().Count() == 1)
                    {
                        parameters = new object[] { xmlFilters };
                        int count = (int)getCountMethodInfo.Invoke(context, parameters);
                        queryOperations.PageSize = count;

                    }
                    else
                    {
                        int count = (int)getCountMethodInfo.Invoke(context, parameters);
                        queryOperations.PageSize = count;
                    }

                    // Get dataList

                    xmlFilters = filterSerializer.SerializeFilterItems(queryOperations);

                    parameters = new object[] { xmlFilters, tenant };

                    try
                    {
                        querableEntities = getListMethodInfo.Invoke(context, parameters) as IQueryable;
                        if (querableEntities == null)
                        {
                            var queryResult = getListMethodInfo.Invoke(context, parameters);
                            if (queryResult != null)
                            {
                                IList list = queryResult as IList;
                                if (list != null)
                                {
                                    querableEntities = list.AsQueryable();
                                }

                            }
                        }

                    }
                    catch (Exception e)
                    {
                        string ip = "";
                        if (HttpContext.Current != null && HttpContext.Current.Request != null)
                        {
                            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                            if (string.IsNullOrEmpty(currentIP))
                            {
                                currentIP = HttpContext.Current.Request.UserHostAddress;
                            }
                            ip = currentIP;
                        }
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "worker role", "build zip files for table:" + table.Name, ip);
                        //AzureLog.SaveLogsInStorage("Before adding document filing queue (Id:" + entityPM.Id + ",Tenant:" + entityPM.Tenant + ")", "L", DateTime.Now, "", "", 0, loggedUserId, loggedUserId, null);
                        //ErrorsLog
                    }
                    //if (querableEntities != null)
                    // {

                    //   IEnumerator datalist = querableEntities.GetEnumerator();

                    // return datalist;
                    // }

                    //return querableEntities;
                }

            }

            return querableEntities;
        }

        private static System.Linq.IQueryable GetTableListForGeneratedQueries(ObjectTable table, int tenant)
        {
            System.Linq.IQueryable querableEntities = null;

            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = new QueryOperations();
            queryOperations.QueryFilterItems = new List<QueryFilterItem>();
            queryOperations.GetAll = true;
            queryOperations.PageIndex = 0;
            queryOperations.GetAll = true;
            queryOperations.SortDirectin = "ascending";
            queryOperations.SortByColumnName = table.SortingByObjectField;
            if (string.IsNullOrEmpty(queryOperations.SortByColumnName))
                queryOperations.SortByColumnName = table.LookUp1;
            foreach (var k in LogitudeModelNames.Keys)
            {
                string modelName = k;
                string contextName = LogitudeModelNames[k];

                string listQueryServiceName = Assembly.CreateQualifiedName(modelName + ".Data", modelName + ".Data" + ".EntityListQueryServices." + table.Name + "ListQueryService");
                var tableQueryType = System.Type.GetType(listQueryServiceName);

                if (string.IsNullOrEmpty(contextName))
                    contextName = modelName + "Context";
                if (tableQueryType != null)
                {
                    string contextClassName = Assembly.CreateQualifiedName(modelName + ".Data", modelName + ".Data." + contextName);
                    Type contextType = Type.GetType(contextClassName);
                    if (contextType != null)
                    {
                        var getContextMethodInfo = contextType.GetMethod("GetContext");
                        object[] contextParams = new object[] { tenant };
                        object datacontext = getContextMethodInfo.Invoke(contextType, contextParams);
                        object[] parameters1 = new object[] { datacontext };
                        object listQuery = Activator.CreateInstance(tableQueryType, parameters1);

                        Type[] parameterstypes = new Type[] { typeof(QueryOperations), typeof(int) };

                        MethodInfo getListMethodInfo = listQuery.GetType().GetMethod("GetList", parameterstypes);
                        MethodInfo getCountMethodInfo = listQuery.GetType().GetMethod("GetListCount");


                        if (getListMethodInfo != null && getCountMethodInfo != null)
                        {
                            object[] parameters = new object[] { queryOperations, tenant };
                            if (getCountMethodInfo.GetParameters().Count() == 1)
                            {
                                parameters = new object[] { queryOperations };
                                int count = (int)getCountMethodInfo.Invoke(listQuery, parameters);
                                queryOperations.PageSize = count;
                            }
                            else
                            {
                                int count = (int)getCountMethodInfo.Invoke(listQuery, parameters);
                                queryOperations.PageSize = count;
                            }
                            object queryResult = null;
                            if (getListMethodInfo.GetParameters().Count() == 1)
                            {
                                parameters = new object[] { queryOperations };
                                queryResult = getListMethodInfo.Invoke(listQuery, parameters);
                            }
                            else
                            {
                                parameters = new object[] { queryOperations, tenant };
                                queryResult = getListMethodInfo.Invoke(listQuery, parameters);
                            }

                            if (queryResult != null)
                            {
                                IList list = queryResult as IList;
                                if (list != null)
                                {
                                    querableEntities = list.AsQueryable();
                                }

                            }

                            break;
                        }
                    }
                }
            }

            return querableEntities;
        }

        private static ReflectionProperties getMethodsInfo(string ContextName, string ObjectTableName)
        {
            Type contextType = Type.GetType(ContextName);
            ReflectionProperties ReturnData = null;
            if (contextType != null)
            {
                object context = Activator.CreateInstance(contextType);

                DomainServiceContext con = new DomainServiceContext(new MockServiceProvider(), DomainOperationType.Query);
                MethodInfo methodInfo = context.GetType().GetMethod("Initialize");
                object[] parameters1 = new object[] { con };
                methodInfo.Invoke(context, parameters1);
                MethodInfo getListMethodInfo = context.GetType().GetMethod("Get" + ObjectTableName + "Filters");
                MethodInfo getCountMethodInfo = context.GetType().GetMethod("Get" + ObjectTableName + "FiltersCount");
                //MethodInfo getListMethodInfo = ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + ObjectTableName.Split('.')[1] + "Filters") : context.GetType().GetMethod("Get" + ObjectTableName + "Filters");
                //MethodInfo getCountMethodInfo = ObjectTableName.Contains("Customs.") ? context.GetType().GetMethod("Get" + ObjectTableName.Split('.')[1] + "FiltersCount") : context.GetType().GetMethod("Get" + ObjectTableName + "FiltersCount");
                //if (getCountMethodInfo == null)
                //{
                //    getCountMethodInfo =  context.GetType().GetMethod("Get" + ObjectTableName + "Count");

                //}
                if (getCountMethodInfo == null)
                {
                    getCountMethodInfo = context.GetType().GetMethod("Get" + ObjectTableName + "Count");
                }


                if (getCountMethodInfo == null)
                {
                    getCountMethodInfo = context.GetType().GetMethod("Get" + ObjectTableName + "sFiltersCount");
                }

                if (getListMethodInfo == null)
                {
                    getListMethodInfo = context.GetType().GetMethod("Get" + ObjectTableName + "sFilters");
                }


                if (getListMethodInfo != null && getCountMethodInfo != null)
                {
                    ReturnData = new ReflectionProperties();
                    ReturnData.ListMethodInfo = getListMethodInfo;
                    ReturnData.CountMethodInfo = getCountMethodInfo;
                    ReturnData.context = context;
                }
            }
            return ReturnData;
        }
    }
}
