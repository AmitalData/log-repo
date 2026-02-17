using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.GlobalModel.CustomFilters;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Security;

namespace WebFreight.Web.GlobalModel
{
    public partial class GlobalDomainService
    {
        private TenantManagmentPrivateLabelsRepository tenantManagmentPrivateLabelsRepository;
        private TenantManagmentPrivateLabelsQuery tenantManagmentPrivateLablesQuery;

        //[RequiresAuthentication]
        //[Query(IsDefault = true)]
        //public IQueryable<TenantManagement> GetTenants()
        //{
        //    SecurityUtility.CheckContactFeature("TenantManagement", "READ", 0);
        //    return tenantManagementRepository.GetTenants();
        //}

        public TenantManagmentPrivateLabelsPM GetSingleTenantManagmentPrivateLabelsPM(string id)
        {
            tenantManagmentPrivateLablesQuery = new TenantManagmentPrivateLabelsQuery(0);
            return tenantManagmentPrivateLablesQuery.GetSinglePM(id);
        }

        public IQueryable<TenantManagmentPrivateLabelsPM> GetTenantManagmentPrivateLablesPMs()
        {
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("TenantManagement", "READ", tenant);
            tenantManagmentPrivateLablesQuery = new TenantManagmentPrivateLabelsQuery(0);
            return tenantManagmentPrivateLablesQuery.GetTenantManagmentPrivateLablesPMs();
        }

        public void InsertTenantManagmentPrivateLabel(TenantManagmentPrivateLabelsPM entityPM)
        {
            var Context = GlobalContext.GetContext();
            TenantManagmentPrivateLablesService service = new TenantManagmentPrivateLablesService(Context);
            service.Create(entityPM); 
        }

        public void UpdateTenantManagmentPrivateLabel(TenantManagmentPrivateLabelsPM entityPM)
        {
            var Context = GlobalContext.GetContext();
            TenantManagmentPrivateLablesService service = new TenantManagmentPrivateLablesService(Context);
            service.Update(entityPM); 

        }

        [Query(HasSideEffects = true)]
        public IQueryable<TenantManagmentPrivateLabelsList> GetTenantManagmentPrivateLabelsFilters(byte[] xmlFilters, int tenant)
        {
            var Context = GlobalContext.GetContext();
            tenantManagmentPrivateLabelsRepository = new TenantManagmentPrivateLabelsRepository(Context);
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("ApiCredintials", "READ", tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TenantManagmentPrivateLabels> iQueryable = tenantManagmentPrivateLabelsRepository.GetTenantManagmentPrivateLabels();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TenantManagmentPrivateLabels>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from a in iQueryable
                         select new TenantManagmentPrivateLabelsList()
                         {
                             Id = a.Id,
                             PrivateLabelName = a.PrivateLabelName,
                             PrivateLabelShortName = a.PrivateLabelShortName,
                             PrivateLabelUrl = a.PrivateLabelUrl,
                             ReceiveAllStatuses = a.ReceiveAllStatuses,
                             MainLogo = a.MainLogo,
                             InActive = a.InActive,
                             HybridPartnerId = a.HybridPartnerId,
                             ContactUsEmail = a.ContactUsEmail,
                             SearchFields = a.SearchFields
                         };

            query2 = filter.GetFilteredQuery<TenantManagmentPrivateLabelsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TenantManagmentPrivateLabelsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TenantManagmentPrivateLabels", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentPrivateLabelsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentPrivateLabelsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentPrivateLabelsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentPrivateLabelsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TenantManagmentPrivateLabelsList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTenantManagmentPrivateLabelsFiltersCount(byte[] xmlFilters, int tenant)
        {
            tenantManagmentPrivateLabelsRepository = new TenantManagmentPrivateLabelsRepository(objectContext);
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("TenantManagmentPrivateLabels", "READ", tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<TenantManagmentPrivateLabels> iQueryable = tenantManagmentPrivateLabelsRepository.GetTenantManagmentPrivateLabels();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TenantManagmentPrivateLabels>(nonListQueryOperation, iQueryable);

            var query2 = from a in iQueryable
                         select new TenantManagmentPrivateLabelsList()
                         {
                             Id = a.Id,
                             PrivateLabelName = a.PrivateLabelName,
                             PrivateLabelShortName = a.PrivateLabelShortName,
                             PrivateLabelUrl = a.PrivateLabelUrl,
                             ReceiveAllStatuses = a.ReceiveAllStatuses,
                             MainLogo = a.MainLogo,
                             InActive = a.InActive,
                             HybridPartnerId = a.HybridPartnerId,
                             ContactUsEmail = a.ContactUsEmail,
                             SearchFields = a.SearchFields
                         };

            query2 = filter.GetFilteredQuery<TenantManagmentPrivateLabelsList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }


        public IQueryable<TenantManagmentPrivateLabelsList> GetTenantManagmentPrivateLabelsLists()
        {
            
            SecurityUtility.AuthenticationOnTenant(0);
            tenantManagmentPrivateLablesQuery = new TenantManagmentPrivateLabelsQuery(0);
            return tenantManagmentPrivateLablesQuery.GetTenantManagmentPrivateLablesLists();
        }

        public TenantManagmentPrivateLabelsList GetSingleTenantManagmentPrivateLabelList(string id)
        {
            tenantManagmentPrivateLablesQuery = new TenantManagmentPrivateLabelsQuery(0);
            return tenantManagmentPrivateLablesQuery.GetSingleList(id);
        }

        public TenantManagmentPrivateLabelsPM GetTenantManagmentPrivateLabelById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("APILogs", "READ", tenant);

            TenantManagmentPrivateLabelsQuery Query = new TenantManagmentPrivateLabelsQuery(tenant);
            TenantManagmentPrivateLabelsPM entityPM = Query.GetSinglePM(id);
            return entityPM;

        }
    }
}