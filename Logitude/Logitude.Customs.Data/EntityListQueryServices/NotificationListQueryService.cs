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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.CustomFilters;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.Data.Utils;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class NotificationListQueryService
    {
	    private IQueryable<NotificationList> GetIqueryableList(IQueryable<Notification> iQueryable)
        {

         



            IQueryable<NotificationList> query = (from a in iQueryable.Include("Department").Include("NotificationDefinition").Include("ObjectTable").Include("Customer")

                                                  select new NotificationList()
                                                                   {
                                                                       Id = a.Id,
                                                                       AssigneToId = a.AssigneToId,
                                                     AssigneToName = a.AssigneTo != null ? (!string.IsNullOrEmpty(a.AssigneTo.Contact.LocalName) ? a.AssigneTo.Contact.LocalName : a.AssigneTo.Contact.EnglishName) : null,
                                                      AssigneToNotificationTypeCode = a.AssigneToNotificationTypeCode,
                                                                       IsClosedByAssignee = a.IsClosedByAssignee,
                                                                       IsSeenByAssignee = a.IsSeenByAssignee,
                                                                       ClosedByCustomOfficeUserId = a.ClosedByCustomOfficeUserId,
                                                                       IsHandledByCustomOffice = a.IsHandledByCustomOffice,
                                                                       CreateDate = a.CreateDate,
                                                                       CreatedByRequestID = a.CreatedByRequestID,
                                                                       DeclarationOfficeCode = a.DeclarationOfficeCode,
                                                                       DepartmentId = a.DepartmentId,
                                                                       DepartmentName = a.Department != null? a.Department.LocalName :null,
                                                                       Description = a.Description,
                                                                       DueDate = a.DueDate,
                                                                       EntityId = a.EntityId,
                                                                       IsClosedBCustomOffice = a.IsClosedBCustomOffice,
                                                                       NotificationDefinitionCode = a.NotificationDefinitionCode,
                                                                       NotificationDefinitionName = a.NotificationDefinition != null? a.NotificationDefinition.LocalName : null,
                                                                       ObjectTableId = a.ObjectTableId,
                                                                      ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                                                       Reference1Number = a.Reference1Number,
                                                                       Reference2Number = a.Reference2Number,
                                                                       SearchFields = a.SearchFields,
                                                                       ResponseNotes = a.ResponseNotes,
                                                                       Tenant = a.Tenant,
                                                                       ClosedByAssignee = a.ClosedByAssignee,
                                                                       ClosedByAssigneeName = a.User != null ? (!string.IsNullOrEmpty(a.User.Contact.LocalName) ? a.User.Contact.LocalName : a.User.Contact.EnglishName) : null,
                                                                       ClosedByCustomOfficeUserName = a.ClosedByCustomOfficeUser != null ? (!string.IsNullOrEmpty(a.ClosedByCustomOfficeUser.Contact.LocalName) ? a.ClosedByCustomOfficeUser.Contact.LocalName : a.ClosedByCustomOfficeUser.Contact.EnglishName) : null, 
                                                                       CustomerId = a.CustomerId,
                                                                       CustomerName = a.Customer != null? a.Customer.Card.LocalName : null,

                                                                   });




            


            return query;
		}

        private IQueryable<Notification> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Notification> iQueryable,int tenant)
        {
            NotificationCustomFilters filters = new NotificationCustomFilters();
             iQueryable = filters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = GetFreelancerQuery(iQueryable, tenant);

            return iQueryable;
		}

        public List<NotificationList> GetNotificationLists(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Notification> iQueryable = (from a in context.Notifications

                                                   where a.Tenant == tenant
                                                   select a);

            iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Notification>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<NotificationList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<NotificationList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(NotificationList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> NotificationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.Notification", tenant).ToList();

                ObjectField objectField = (from a in NotificationObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<NotificationList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<NotificationList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<NotificationList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<NotificationList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<NotificationList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<NotificationList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<NotificationList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderBy(d => d.IsSeenByAssignee).ThenBy(d => d.DueDate);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderBy(d => d.IsSeenByAssignee).ThenBy(d => d.DueDate);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }

            //ObjectTableRepository rep = new ObjectTableRepository(0);
            //ObjectTable objectTable = rep.GetObjectTableByName("Customs.Declaration", 0, false);
            //  List<NotificationList> result = query2.ToList();



            //List<string> EntityIds = notifications.Select(t => t.EntityId).ToList();


            //DeclarationRepository declarationRep = new DeclarationRepository(context);
            //IDictionary<string, string> declarationCutomers = declarationRep.GetCustomersByDeclarationIds(EntityIds, tenant);



            //foreach (NotificationList item in result)
            //{
            //    if(notifications.Contains(item))
            //    {
            //        if (item.EntityId != null)
            //        {
            //            if (declarationCutomers.ContainsKey(item.EntityId))
            //            {
            //                item.CustomerName = declarationCutomers[item.EntityId];
            //            }
            //        }
            //    }
            //}

            return query2.ToList();


        }

        public IQueryable<Notification> GetFreelancerQuery(IQueryable<Notification> queryableData, int tenant)
        {
            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            if (frlUtil.user.IsFreelancer)
            {
                List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
                if (customersIds.Count > 0)
                {
                    queryableData = queryableData.Where(d => customersIds.Contains(d.CustomerId));
                }
            }

            return queryableData;
        }

    }


}
	