using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
   public class CustomerTenantAccessRequestQuery
    {
         CustomerTenantAccessRequestRepository repository;
     
        public CustomerTenantAccessRequestQuery()
        {
            repository = new CustomerTenantAccessRequestRepository(); 
        }

        public CustomerTenantAccessRequestQuery(int tenant)
        {         
            repository = new CustomerTenantAccessRequestRepository(tenant);
        }

        public CustomerTenantAccessRequestQuery(CustomerTenantAccessRequestRepository CustomerTenantAccessRequestRepository)
        {
            repository = CustomerTenantAccessRequestRepository;
        }

        public IQueryable<CustomerTenantAccessRequestList> GetIQueryableEntityList(IQueryable<CustomerTenantAccessRequest> iQueryable)
        {
            IQueryable<CustomerTenantAccessRequestList> entity = from a in iQueryable.Include("RequestStatusCode").Include("HybridPartnerId")
                                                          select new CustomerTenantAccessRequestList()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              ForwarderId=a.ForwarderId,
                                                              RequestDateTime=a.RequestDateTime,
                                                              RequestStatus=a.RequestStatus,                                                       
                                                              StatusName = a.RequestStatusCode != null ?a.RequestStatusCode.EnglishName: null,
                                                              ForwarderName=a.HybridPartnerId !=null?a.HybridPartnerId.Name:null,
                                                              IsExport = a.IsExport,
                                                              IsCustoms = a.IsCustoms,
                                                          };


            return entity;
                                   
        }

      

        public CustomerTenantAccessRequestPM GetSinglePM(string id, int tenant)
        {
            CustomerTenantAccessRequestPM entity = (from a in repository.context.CustomerTenantAccessRequests.Include("CustomerTenantAccessStatusType").Include("HybridPartner")                     
                                             where a.Id == id && a.Tenant==tenant
                                             select new CustomerTenantAccessRequestPM()
                                             {
                                                 Id=a.Id,
                                                 Tenant=a.Tenant,
                                                 ForwarderId=a.ForwarderId,
                                                 RequestDateTime=a.RequestDateTime,
                                                 RequestStatus=a.RequestStatus,
                                                 IsExport = a.IsExport,
                                                 IsCustoms = a.IsCustoms,
                                                  
                                             }).FirstOrDefault();


                        return entity;
                                   
            
        }

        public CustomerTenantAccessRequestPM GetByTenantAndForwarderId(int tenant, string forwarderId)
        {
            CustomerTenantAccessRequestPM entity = (from a in repository.context.CustomerTenantAccessRequests.Include("CustomerTenantAccessStatusType").Include("HybridPartner")
                                                    where a.ForwarderId == forwarderId && a.Tenant == tenant
                                                    select new CustomerTenantAccessRequestPM()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        ForwarderId = a.ForwarderId,
                                                        RequestDateTime = a.RequestDateTime,
                                                        RequestStatus = a.RequestStatus,
                                                        IsExport = a.IsExport,
                                                        IsCustoms = a.IsCustoms,

                                                    }).FirstOrDefault();


            return entity;


        }



        public CustomerTenantAccessRequestPM GetSinglePMByCustomerTenantAndPartnerTenant(string PartnerId, int CustomerTenant)
        {
            CustomerTenantAccessRequestPM entity = (from a in repository.context.CustomerTenantAccessRequests.Include("CustomerTenantAccessStatusType").Include("HybridPartner")
                                                    where a.ForwarderId == PartnerId && a.Tenant == CustomerTenant
                                                    select new CustomerTenantAccessRequestPM()
                                                    {
                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        ForwarderId = a.ForwarderId,
                                                        RequestDateTime = a.RequestDateTime,
                                                        RequestStatus = a.RequestStatus,
                                                        IsExport = a.IsExport,
                                                        IsCustoms = a.IsCustoms,


                                                    }).FirstOrDefault();


            return entity;


        }

        public IQueryable<CustomerTenantAccessRequestPM> GetCustomerTenantAccessRequestPMsByTenant(int tenant)
        {

            IQueryable<CustomerTenantAccessRequestPM> entity = from a in repository.context.CustomerTenantAccessRequests.Include("CustomerTenantAccessStatusType").Include("HybridPartner")
                                                        where a.Tenant == tenant
                                                       select new CustomerTenantAccessRequestPM()
                                
                                                       {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              RequestDateTime=a.RequestDateTime,
                                                              RequestStatus=a.RequestStatus,
                                                              ForwarderId=a.ForwarderId,
                                                             IsExport = a.IsExport,
                                                             IsCustoms = a.IsCustoms,

                                                       };

                                           
                                   


            return entity;
        }

        public List<string> GetCustomerTenantAccessRequestForwarderIdsByTenant(int tenant)
        {
            var IdsList = from a in repository.context.CustomerTenantAccessRequests.Include("CustomerTenantAccessStatusType").Include("HybridPartner")
                    where a.Tenant == tenant
                    select a.ForwarderId;

            return IdsList.ToList() ;
         
        }


        public IQueryable<CustomerTenantAccessRequestPM> GetCustomerTenantAccessRequestByTenantAndForwarderIds(int tenant, List<string> forwarderIds)
        {

            IQueryable<CustomerTenantAccessRequestPM> entity = from a in repository.context.CustomerTenantAccessRequests.Include("CustomerTenantAccessStatusType").Include("HybridPartner")
                                                               where a.Tenant == tenant && forwarderIds.Contains(a.ForwarderId)
                                                               select new CustomerTenantAccessRequestPM()

                                                               {
                                                                   Id = a.Id,
                                                                   Tenant = a.Tenant,
                                                                   RequestDateTime = a.RequestDateTime,
                                                                   RequestStatus = a.RequestStatus,
                                                                   ForwarderId = a.ForwarderId,
                                                                   IsExport = a.IsExport,
                                                                   IsCustoms = a.IsCustoms,

                                                               };





            return entity;
        }

         
    }
}
