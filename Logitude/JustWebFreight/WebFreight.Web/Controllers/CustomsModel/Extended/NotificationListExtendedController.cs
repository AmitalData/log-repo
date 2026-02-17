using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using WebFreight.Web.CustomModel;
using Logitude.Customs.BL.EntityQueryServices;
using System.Transactions;
using Logitude.Customs.Data.DataContracts;
using System.Data.SqlClient;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;
using System.Data.Entity.Infrastructure;
using Logitude.Customs.Data.Repsitories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class NotificationListExtendedController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
       {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Customs.Notification", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Customs.Notification",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.Notifications",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };


                List<ObjectField> NotificationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.Notification", tenant);
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
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
                        ObjectField field = NotificationObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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
                        ObjectField field = NotificationObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
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

               ICustomContext MyContext = CustomContext.GetContext(tenant);
                NotificationListQueryService notificationQuery = new NotificationListQueryService(MyContext);

                List<NotificationList> entityLists = notificationQuery.GetNotificationLists(queryOperations, tenant);

                //UserQuery userQuery = new UserQuery(tenant);
                //List<string> Ids = (from a in entityLists select a.AssigneToId).ToList();
                //if (Ids.Count > 0)
                //{
                //    List<UserPM> users = userQuery.GetUserPMsByUserIds(Ids, tenant);
                //    foreach (NotificationList item in entityLists)
                //    {
                //        string UserName = (from a in users
                //                          where a.Id == item.AssigneToId
                //                          select a.LocalName).FirstOrDefault();
                //        item.AssigneToName = UserName;


                //    }
                //}

                ServiceResponse response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = notificationQuery.GetListCount(queryOperations, tenant);
                    response.Count = count;
                }

                response.Result = entityLists;//.OrderBy(d => d.IsSeenByAssignee).ThenBy(d => d.DueDate);
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        [HttpGet]
        public HttpResponseMessage getCountByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Customs.Notification", "READ", authToken.Tenant);

                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Customs.Notification",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.Notifications",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };


                List<ObjectField> NotificationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.Notification", tenant);
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
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
                        ObjectField field = NotificationObjectFields.FirstOrDefault(f => f.FieldName == filterName);
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
                        ObjectField field = NotificationObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
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

                ICustomContext MyContext = CustomContext.GetContext(tenant);
                List<NotificationFiltersDataCount> countsList = new List<NotificationFiltersDataCount>();
                NotificationListQueryService listService = new NotificationListQueryService(MyContext);
                NotificationFiltersDataCount count = null;

                QueryFilterItem notificationTypeFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "AssigneToNotificationTypeCode" && d.Operator == "Equals").FirstOrDefault();
                string notificationType = null;
                if (notificationTypeFilterItem != null)
                {
                    notificationType = notificationTypeFilterItem.FieldValue.ToString();
                    queryOperations.QueryFilterItems.Remove(notificationTypeFilterItem);
                }

                QueryFilterItem IsClosedByAssigneeFilterItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IsClosedByAssignee" && d.Operator == "Equals").FirstOrDefault();
                bool IsClosedByAssignee = false;
                if (IsClosedByAssigneeFilterItem != null)
                {
                    IsClosedByAssignee = (bool)IsClosedByAssigneeFilterItem.FieldValue;
                    queryOperations.QueryFilterItems.Remove(IsClosedByAssigneeFilterItem);
                }



                // int notificationCount = listService.GetListCount(queryOperations, tenant);
                queryOperations.GetAll = true;
                // List<NotificationList> allNotifications = listService.GetList(queryOperations, tenant);


                //All Count notification Type
                if (IsClosedByAssigneeFilterItem != null)
                {
                    queryOperations.QueryFilterItems.Add(IsClosedByAssigneeFilterItem);
                }
                int allTypeNotifications = listService.GetListCount(queryOperations, tenant);
                QueryFilterItem IsSeenByAssigneeFilterItem = new QueryFilterItem() { FieldName = "IsSeenByAssignee", Operator = "Equals", FieldValue = true };
                QueryFilterItem IsUnReadByAssigneeFilterItem = new QueryFilterItem() { FieldName = "IsSeenByAssignee", Operator = "Equals", FieldValue = false };

                if (IsSeenByAssigneeFilterItem != null)
                {
                    queryOperations.QueryFilterItems.Add(IsSeenByAssigneeFilterItem);
                }
                int allReadNotifications = listService.GetListCount(queryOperations, tenant);
                queryOperations.QueryFilterItems.Remove(IsSeenByAssigneeFilterItem);

                if (IsUnReadByAssigneeFilterItem != null)
                {
                    queryOperations.QueryFilterItems.Add(IsUnReadByAssigneeFilterItem);
                }
                int allUnReadNotifications = listService.GetListCount(queryOperations, tenant);
                queryOperations.QueryFilterItems.Remove(IsUnReadByAssigneeFilterItem);

                // info notifications
                QueryFilterItem infoFilterItem = new QueryFilterItem() { FieldName = "AssigneToNotificationTypeCode", Operator = "Equals", FieldValue = "I" };
                queryOperations.QueryFilterItems.Add(infoFilterItem);
                int allInfoNotifications = listService.GetListCount(queryOperations, tenant);
                queryOperations.QueryFilterItems.Remove(infoFilterItem);
                //int allInfoNotifications = allNotifications.Where(d => d.AssigneToNotificationTypeCode == "I" && d.IsClosedByAssignee == IsClosedByAssignee).Count();

                //Action notifications

                QueryFilterItem actionFilterItem = new QueryFilterItem() { FieldName = "AssigneToNotificationTypeCode", Operator = "Equals", FieldValue = "A" };
                queryOperations.QueryFilterItems.Add(actionFilterItem);
                int allActionNotifications = listService.GetListCount(queryOperations, tenant);
                queryOperations.QueryFilterItems.Remove(actionFilterItem);


                queryOperations.QueryFilterItems.Remove(IsClosedByAssigneeFilterItem);
                //int allActionNotifications = allNotifications.Where(d => d.AssigneToNotificationTypeCode == "A" && d.IsClosedByAssignee == IsClosedByAssignee).Count();
                int OpenCount = 0;
                int closedCount = 0;
                if (notificationType == null)
                {
                    //open count
                    QueryFilterItem openFilterItem = new QueryFilterItem() { FieldName = "IsClosedByAssignee", Operator = "Equals", FieldValue = false };
                    queryOperations.QueryFilterItems.Add(openFilterItem);
                    OpenCount = listService.GetListCount(queryOperations, tenant);
                    queryOperations.QueryFilterItems.Remove(openFilterItem);

                    //closed Count
                    QueryFilterItem closedFilterItem = new QueryFilterItem() { FieldName = "IsClosedByAssignee", Operator = "Equals", FieldValue = true };
                    queryOperations.QueryFilterItems.Add(closedFilterItem);
                    closedCount = listService.GetListCount(queryOperations, tenant);
                    queryOperations.QueryFilterItems.Remove(closedFilterItem);
                    //OpenCount = allNotifications.Where(d => !d.IsClosedByAssignee ).Count();
                }
                else
                {
                    QueryFilterItem openFilterItem = new QueryFilterItem() { FieldName = "IsClosedByAssignee", Operator = "Equals", FieldValue = false };
                    queryOperations.QueryFilterItems.Add(openFilterItem);
                    queryOperations.QueryFilterItems.Add(notificationTypeFilterItem);
                    OpenCount = listService.GetListCount(queryOperations, tenant);
                    queryOperations.QueryFilterItems.Remove(openFilterItem);

                    QueryFilterItem closedFilterItem = new QueryFilterItem() { FieldName = "IsClosedByAssignee", Operator = "Equals", FieldValue = true };
                    queryOperations.QueryFilterItems.Add(closedFilterItem);
                    queryOperations.QueryFilterItems.Add(notificationTypeFilterItem);
                    closedCount = listService.GetListCount(queryOperations, tenant);

                    queryOperations.QueryFilterItems.Remove(closedFilterItem);
                    queryOperations.QueryFilterItems.Remove(notificationTypeFilterItem);

                    // OpenCount = allNotifications.Where(d => d.AssigneToNotificationTypeCode == notificationType && !d.IsClosedByAssignee ).Count();
                }



                count = new NotificationFiltersDataCount() { Id = Guid.NewGuid().ToString(), OpenCount = OpenCount, AllCount = allTypeNotifications, ActionCount = allActionNotifications, InfoCount = allInfoNotifications, ClosedCount = closedCount, AllReadCount= allReadNotifications, AllUnreadCount= allUnReadNotifications };
                countsList.Add(count);
              
               ServiceResponse response = new ServiceResponse();

                  response.Result = count;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);

                return reponseMessage;

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage PutNotificationsStatus(NotificationList notification)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                UserRepository userRepository = new UserRepository(tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                NotificationQueryService notificationQuery = new NotificationQueryService(customContext);

                NotificationUpdateService service = new NotificationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
            

                NotificationPM notificationPM = notificationQuery.GetSingle(notification.Id, false,false);

                notificationPM.IsClosedByAssignee = true;
                notificationPM.ClosedByAssignee = loggedUser.Id;

                notificationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                service.Update(notificationPM, true);

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTopTenNotifications(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
             
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                NotificationQueryService queryService = new NotificationQueryService(customContext);
                List<NotificationPM> list = queryService.GetTopTenNotificationPMs(userId, tenant);
                ServiceResponse response = new ServiceResponse();
                response.Result = list;
                return Request.CreateResponse(HttpStatusCode.OK, response.Result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetOpenNotificationsCount(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                NotificationQueryService queryService = new NotificationQueryService(customContext);
                int count= queryService.GetOpenNotificationCountForUser(userId, tenant);
                ServiceResponse response = new ServiceResponse();
                response.Result = count;
                return Request.CreateResponse(HttpStatusCode.OK, response.Result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetNotificationsBadjCount(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                NotificationQueryService queryService = new NotificationQueryService(customContext);
                int count= queryService.GetBadjCount(userId, tenant);
                ServiceResponse response = new ServiceResponse();
                response.Result = count;
                return Request.CreateResponse(HttpStatusCode.OK, response.Result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        
        public HttpResponseMessage PutNotificationBadjCount(NotificationPM notification)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                UserRepository userRepository = new UserRepository(tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                NotificationQueryService queryService = new NotificationQueryService(customContext);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    List<NotificationPM> result = queryService.GetNotificationsWithBadj(loggedUser.Id, tenant);
                    foreach (NotificationPM item in result)
                    {
                        item.BadjCount = false;
                        NotificationUpdateService service = new NotificationUpdateService(customContext, new Dictionary<string, IContext>(), item.Tenant);
                        item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                        service.Update(item, false);
                    }

                    customContext.SaveChanges();
                    scope.Complete();
                }
                ServiceResponse response = new ServiceResponse();
              
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutNotificationStatus(SelectedNotifications selected)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                UserRepository userRepository = new UserRepository(tenant);
                User loggedUser = userRepository.GetSingleUserByCodeOrEmail(null, loggedUserEmail, tenant, true);
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                NotificationQueryService notificationQuery = new NotificationQueryService(customContext);
                NotificationRepository notificationRep = new NotificationRepository(customContext);
                NotificationUpdateService service = new NotificationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                List<NotificationPM> notifications = new List<NotificationPM>();
                IQueryable<NotificationPM> NotificationPMs;
                DateTime? fromDate = null;
                DateTime? toDate = null;
              
                if (selected.IsAllSelected && string.IsNullOrEmpty(selected.SearchFields))
                {
                    NotificationPMs = notificationQuery.GetAllNotifications(tenant);

                    if (selected.IsClosedByAssignee == "closed")
                    {
                        NotificationPMs = NotificationPMs.Where(d => d.IsClosedByAssignee);
                    }
                    else if (selected.IsClosedByAssignee == "open")
                    {

                        NotificationPMs = NotificationPMs.Where(d => !d.IsClosedByAssignee);
                    }

                   
                    if (!string.IsNullOrEmpty(selected.SeenByAssigneeStatus))
                    {
                        if (selected.SeenByAssigneeStatus == "read")
                        {
                            NotificationPMs= NotificationPMs.Where(d => d.IsSeenByAssignee);
                        }
                        else if (selected.SeenByAssigneeStatus == "unread")
                        {
                            NotificationPMs = NotificationPMs.Where(d => !d.IsSeenByAssignee);
                        }


                    }

                    if (selected.DueDate != "0")
                    {
                        switch (selected.DueDate)
                        {
                            case "1":
                                {
                                    fromDate = DateTime.Today.Date;
                                    NotificationPMs = NotificationPMs.Where(d => d.DueDate < fromDate);
                                    break;
                                }

                            case "2":
                                {
                                    fromDate = DateTime.Today.Date;
                                    toDate = DateTime.Today.Date.AddDays(3);
                                    NotificationPMs = NotificationPMs.Where(d => d.DueDate > fromDate && d.DueDate < toDate);
                                    break;
                                }

                            case "3":
                                {
                                    fromDate = DateTime.Today.Date;
                                    toDate = DateTime.Today.Date.AddDays(7);
                                    NotificationPMs = NotificationPMs.Where(d => d.DueDate < fromDate && d.DueDate > toDate);
                                    break;
                                }

                            case "4":
                                {
                                    fromDate = DateTime.Today.Date;
                                    toDate = DateTime.Today.Date.AddDays(30);
                                    NotificationPMs = NotificationPMs.Where(d => d.DueDate < fromDate && d.DueDate > toDate);
                                    break;
                                }
                        }
                    }
                    if (selected.IsHandledByCustomOffice)
                    {
                        NotificationPMs = NotificationPMs.Where(d => d.IsHandledByCustomOffice);
                    }

                    if(selected.ObjectTableName == "Customs.Declaration")
                    {
                     
                     NotificationPMs = NotificationPMs.Where(d => d.EntityId == selected.DeclarationId || d.Reference1Number == selected.CustomFileNo);
                        
                    }
                   
                    if (!string.IsNullOrEmpty(selected.AssigneToId))
                    {
                        NotificationPMs = NotificationPMs.Where(d => d.AssigneToId == selected.AssigneToId);
                    }
                    if (!string.IsNullOrEmpty(selected.DepartmentId))
                    {
                        NotificationPMs = NotificationPMs.Where(d => d.DepartmentId == selected.DepartmentId);
                    }

                    if (!string.IsNullOrEmpty(selected.DeclarationOfficeCode))
                    {
                        NotificationPMs = NotificationPMs.Where(d => d.DeclarationOfficeCode == selected.DeclarationOfficeCode);
                    }

                    if (!string.IsNullOrEmpty(selected.AssigneToNotificationTypeCode))
                    {
                        NotificationPMs = NotificationPMs.Where(d => d.AssigneToNotificationTypeCode == selected.AssigneToNotificationTypeCode);
                    }
                    if(selected.ExcludedIds != null) {
                        NotificationPMs = (from a in NotificationPMs
                                           where !selected.ExcludedIds.Contains(a.Id)
                                           select a);
                        selected.dataCount = selected.dataCount - selected.ExcludedIds.Count();

                    }
                   // NotificationPMs = NotificationPMs.OrderBy(d => d.IsSeenByAssignee).ThenBy(d => d.DueDate);
                    if (NotificationPMs.Count() == selected.dataCount)
                    {
                        notifications = NotificationPMs.Take(1000).ToList();

                        using (TransactionScope scope = TransactionFactory.GetTransaction())
                        {

                            foreach (NotificationPM notificationPM in notifications)
                            {

                                if (selected.Status == "Read")
                                {
                                    notificationPM.IsSeenByAssignee = true;
                                }

                                else if (selected.Status == "Unread")
                                {
                                    notificationPM.IsSeenByAssignee = false;
                                }

                                else if (selected.Status == "Close")
                                {

                                    notificationPM.IsClosedByAssignee = true;
                                    notificationPM.ClosedByAssignee = loggedUser.Id;

                                }

                                else if (selected.Status == "Open")
                                {
                                    notificationPM.IsClosedByAssignee = false;
                                }
                                notificationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                                service.Update(notificationPM, false);
                            }

                            customContext.SaveChanges();
                            scope.Complete();
                        }

               


                    }



                 
                }
                else if(selected.IsAllSelected && !string.IsNullOrEmpty(selected.SearchFields))
                {
                    notifications = notificationQuery.GetNotificationsPMsBySearchField(selected.SearchFields, tenant);

                    foreach (NotificationPM notificationPM in notifications)
                    {

                        if (selected.Status == "Read")
                        {
                            notificationPM.IsSeenByAssignee = true;
                        }

                        else if (selected.Status == "Unread")
                        {
                            notificationPM.IsSeenByAssignee = false;
                        }

                        else if (selected.Status == "Close")
                        {
                            notificationPM.IsClosedByAssignee = true;
                            notificationPM.ClosedByAssignee = loggedUser.Id;
                        }

                        else if (selected.Status == "Open")
                        {
                            notificationPM.IsClosedByAssignee = false;
                        }
                        notificationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                        service.Update(notificationPM, true);
                    }
                }
                else {
                    notifications = notificationQuery.GetNotificationsPMsByIds(selected.SelectedIds, tenant);
                 
                    foreach (NotificationPM notificationPM in notifications)
                    {
                       
                        if (selected.Status == "Read")
                        {
                            notificationPM.IsSeenByAssignee = true;
                        }

                        else if (selected.Status == "Unread")
                        {
                            notificationPM.IsSeenByAssignee = false;
                        }

                        else if (selected.Status == "Close")
                        {
                            notificationPM.IsClosedByAssignee = true;
                            notificationPM.ClosedByAssignee = loggedUser.Id;
                        }

                        else if (selected.Status == "Open")
                        {
                            notificationPM.IsClosedByAssignee = false;
                        }
                        notificationPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                        service.Update(notificationPM, true);
                    }
                }


            

                ServiceResponse response = new ServiceResponse();

                if (selected.IsAllSelected)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "ok");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, notifications);
                }
            }
             
            catch (Exception ex)
            {
            
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }
    }

    //string strConnString = GetConnection(tenant);
    //string wherestring = "  Customs.Notifications.IsClosedByAssignee = '" + selected.IsClosedByAssignee + "'";

    //if (!string.IsNullOrEmpty(selected.SeenByAssigneeStatus))
    //{
    //    if (selected.SeenByAssigneeStatus == "read")
    //    {
    //        wherestring += " and Customs.Notifications.IsSeenByAssignee = '" + true + "'";
    //    }
    //    else if (selected.SeenByAssigneeStatus == "unread")
    //    {
    //        wherestring += " and Customs.Notifications.IsSeenByAssignee = '" + false + "'";
    //    }


    //}
    //if (selected.DueDate != "0")
    //{
    //    switch (selected.DueDate)
    //    {
    //        case "1":
    //            {
    //                fromDate = DateTime.Today.Date;
    //                wherestring += " and Customs.Notifications.DueDate < '" + fromDate + "'";
    //                break;
    //            }

    //        case "2":
    //            {
    //                fromDate = DateTime.Today.Date;
    //                toDate = DateTime.Today.Date.AddDays(3);
    //                wherestring += " and Customs.Notifications.DueDate between '" + fromDate + "' and '" + toDate + "'" ;
    //                break;
    //            }

    //        case "3":
    //            {
    //                fromDate = DateTime.Today.Date;
    //                toDate = DateTime.Today.Date.AddDays(7);
    //                wherestring += " and Customs.Notifications.DueDate between '" + fromDate + "' and '" + toDate + "'";
    //                break;
    //            }

    //        case "4":
    //            {
    //                fromDate = DateTime.Today.Date;
    //                toDate = DateTime.Today.Date.AddDays(30);
    //                wherestring += " and Customs.Notifications.DueDate between '" + fromDate + "' and '" + toDate + "'";
    //                break;
    //            }
    //    }
    //}
    //if (selected.IsHandledByCustomOffice)
    //{
    //    wherestring += " and Customs.Notifications.IsHandledByCustomOffice = '" + true + "'";
    //}
    //if (!string.IsNullOrEmpty(selected.DeclarationId))
    //{
    //    wherestring += " and Customs.Notifications.EntityId = '" + selected.DeclarationId + "'";
    //}
    //if (!string.IsNullOrEmpty(selected.AssigneToId))
    //{
    //    wherestring += " and Customs.Notifications.AssigneToId = '" + selected.AssigneToId + "'";
    //}
    //if (!string.IsNullOrEmpty(selected.DepartmentId))
    //{
    //    wherestring += " and Customs.Notifications.DepartmentId = '" + selected.DepartmentId + "'";
    //}

    //if (!string.IsNullOrEmpty(selected.DeclarationOfficeCode))
    //{
    //    wherestring += " and Customs.Notifications.DeclarationOfficeCode = '" + selected.DeclarationOfficeCode + "'";
    //}

    //if (!string.IsNullOrEmpty(selected.AssigneToNotificationTypeCode))
    //{
    //    wherestring += " and Customs.Notifications.AssigneToNotificationTypeCode = '" + selected.AssigneToNotificationTypeCode + "'";
    //}

    //if (selected.ExcludedPMs != null)
    //{
    //    foreach (NotificationPM item in selected.ExcludedPMs)
    //    {
    //        wherestring += " and Customs.Notifications.Id != '" + item.Id + "' or ";

    //    }
    //    char[] chars = { 'o', 'r' };
    //    wherestring = wherestring.TrimEnd(chars);
    //}
    //int count = 0;
    //using (SqlConnection cn = new SqlConnection(strConnString))
    //{
    //    string cmd = "select * from Customs.Notifications where "+ wherestring;

    //    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

    //    cn.Open();
    //    sqlCommand.ExecuteNonQuery();
    //    cn.Close();

    //    iQueryable = activeContext.Database.SqlQuery<Notification>(cmd, parameters);
    //    count = iQueryable.Count();
    //}

    //if (count == selected.dataCount)
    //{
    //    using (SqlConnection cn = new SqlConnection(strConnString))
    //    {



    //        string UpdateCmd = "";
    //        if (selected.Status == "Read")
    //        {
    //            UpdateCmd = "Update Customs.Notifications set IsSeenByAssignee= '" + true + "' where " + wherestring; ;

    //        }

    //        else if (selected.Status == "Unread")
    //        {
    //            UpdateCmd = "Update Customs.Notifications set IsSeenByAssignee= '" + false + "' where " + wherestring; ;
    //        }

    //        else if (selected.Status == "Close")
    //        {
    //            UpdateCmd = "Update Customs.Notifications set IsClosedByAssignee= '" + true + "' where " + wherestring; ;

    //        }

    //        else if (selected.Status == "Open")
    //        {
    //            UpdateCmd = "Update Customs.Notifications set IsClosedByAssignee= '" + false + "' where " + wherestring; ;


    //        }



    //        SqlCommand sqlCommand = new SqlCommand(UpdateCmd, cn);

    //        cn.Open();
    //        sqlCommand.ExecuteNonQuery();
    //        cn.Close();
    //    }
    //}
}