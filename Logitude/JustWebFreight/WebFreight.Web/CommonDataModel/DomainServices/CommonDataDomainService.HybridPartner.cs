using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {

        public void UpdateHybridPartnerList(HybridPartnerList currentEntity, int tenant)
        {
        }

        public IQueryable<HybridPartner> HybridPartneresByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            hybridPartnerRepository = new HybridPartnerRepository(tenant);
            return hybridPartnerRepository.GetHybridPartnersByTenant();
        }

        public HybridPartnerPM GetSingleHybridPartner(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            hybridPartnerQuery = new HybridPartnerQuery(tenant);
            return hybridPartnerQuery.GetSinglePM(id);
        }

        public HybridPartnerList GetSingleHybridPartnerList(string id)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("HybridPartner", "READ", 0);

           hybridPartnerRepository = new HybridPartnerRepository(0);
            HybridPartnerList HybridPartnerList = null;
            HybridPartner HybridPartner = hybridPartnerRepository.GetSingleHybridPartner(id);

            if (HybridPartner != null)
            {
                List<HybridPartner> singleEntityList = new List<HybridPartner>();
                singleEntityList.Add(HybridPartner);

                hybridPartnerQuery = new HybridPartnerQuery(hybridPartnerRepository);
                IQueryable<HybridPartner> iQueryable = singleEntityList.AsQueryable();
                IQueryable<HybridPartnerList> iQueryableEntityList = hybridPartnerQuery.GetIQueryableEntityList(iQueryable);
                HybridPartnerList = iQueryableEntityList.FirstOrDefault();
            }
            return HybridPartnerList;
        }


        public List<HybridPartnerList> GetHybridPartnerLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            hybridPartnerRepository = new HybridPartnerRepository(tenant);

            var requestsList = (from a in hybridPartnerRepository.context.CustomerTenantAccessRequests.Include("RequestStatusCode")
                                join b in hybridPartnerRepository.context.HybridPartners on a.ForwarderId equals b.Id
                                where a.ForwarderId == b.Id && a.Tenant == tenant
                                select new HybridPartnerList
                                {
                                    Id = b.Id,
                                    IsHasRequest = a.RequestStatusCode.Code != "N",
                                    LocalName = b.LocalName,
                                    LogoId = b.LogoId,
                                    Name = b.Name,
                                    PartnerTenant = b.PartnerTenant,
                                    SearchFields = b.SearchFields,
                                    SmallLogoId = b.SmallLogoId,
                                    StatusName = a.RequestStatusCode.EnglishName,
                                    ReqId = a.Id
                                }).ToList();

            //foreach (HybridPartnerList list in result)
            //{
            //    list.IsHasRequest = requestsList.Any(r => r.ForwarderId == list.Id);
            //    list.StatusName = requestsList.Any(r => r.ForwarderId == list.Id) ? requestsList.First(r => r.ForwarderId == list.Id).RequestStatusCode.EnglishName : null;
            //} 
            return requestsList;
        }

        public List<HybridPartnerList> GetHybridPartnerListWithNoRequest(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            hybridPartnerRepository = new HybridPartnerRepository(tenant);

            var requestsList = (from a in hybridPartnerRepository.context.CustomerTenantAccessRequests.Include("RequestStatusCode")
                                where a.Tenant == tenant
                                select a.ForwarderId).ToList();

            var hybridList = (from a in hybridPartnerRepository.context.HybridPartners
                              where !requestsList.Contains(a.Id)
                              select new HybridPartnerList
            {
                                  Id = a.Id,
                                  IsHasRequest = false,
                                  LocalName = a.LocalName,
                                  LogoId = a.LogoId,
                                  Name = a.Name,
                                  PartnerTenant = a.PartnerTenant,
                                  SearchFields = a.SearchFields,
                                  SmallLogoId = a.SmallLogoId,
                                  StatusName = "New"
                              }).ToList();



            return hybridList;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<HybridPartnerList> GetHybridPartnerFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

           hybridPartnerRepository = new HybridPartnerRepository(tenant);
            hybridPartnerQuery = new HybridPartnerQuery(hybridPartnerRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<HybridPartner> iQueryable = hybridPartnerRepository.GetHybridPartnersByTenant();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<HybridPartner>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<HybridPartnerList> query2 = hybridPartnerQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<HybridPartnerList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(HybridPartnerList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("HybridPartner", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<HybridPartnerList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<HybridPartnerList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<HybridPartnerList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<HybridPartnerList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<HybridPartnerList, bool>(queryOperations, query2);
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

        public int GetHybridPartnerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

           hybridPartnerRepository = new HybridPartnerRepository(tenant);
            hybridPartnerQuery = new HybridPartnerQuery(hybridPartnerRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<HybridPartner> iQueryable = hybridPartnerRepository.GetHybridPartnersByTenant();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<HybridPartner>(nonListQueryOperation, iQueryable);

            IQueryable<HybridPartnerList> query2 = hybridPartnerQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<HybridPartnerList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }
  
        public void InsertHybridPartnerPM(HybridPartnerPM entityPM, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("HybridPartner", "NEW", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }
           
            hybridPartnerRepository = new HybridPartnerRepository(objectContext);
           
            bool exist = hybridPartnerRepository.GetHybridPartnersByPartnerTenant((int)entityPM.PartnerTenant).Any();

            if (!exist)
            {
                HybridPartnerService service = new HybridPartnerService(objectContext);
                service.Create(entityPM);
                

            }
            else
            {
                string msg = "Hybrid Parnter already exists on this Partner Tenant";//TranslateTextsClass.Translate("General.M.EntityAlreadyexists", 0);
                //msg = msg.Replace("%Entity", "HybridPartner");
                throw new Exception(msg);
            }

            //IWebFreightContext webfreightcontext = WebFreightContext.GetContext(tenant);
            //TableLastUpdateClass.UpdateTableHistory(0, "HybridPartner", webfreightcontext);
        }


         
  

        public void UpdateHybridPartner(HybridPartnerPM entityPM, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            SecurityUtility.CheckContactFeature("HybridPartner", "UPDATE", 0);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            HybridPartnerService service = new HybridPartnerService(objectContext);

            service.Update(entityPM);

            IWebFreightContext webfreightcontext = WebFreightContext.GetContext(tenant);
            TableLastUpdateClass.UpdateTableHistory(0, "HybridPartner", webfreightcontext);
        }
    }
}