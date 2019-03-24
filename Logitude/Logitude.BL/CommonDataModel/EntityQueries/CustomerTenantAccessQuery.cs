using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;



namespace Logitude.BL.CommonDataModel.EntityQueries
{

    public class CustomerTenantAccessQuery
    {
        CustomerTenantAccessRepository repository;

    

        public CustomerTenantAccessQuery()
        {
            repository = new CustomerTenantAccessRepository();
        }

        public CustomerTenantAccessQuery(int tenant)
        {
            repository = new CustomerTenantAccessRepository(tenant);
        }

        public CustomerTenantAccessQuery(CustomerTenantAccessRepository CustomerTenantAccessRepository)
        {
            repository = CustomerTenantAccessRepository;
        }

        public IQueryable<CustomerTenantAccessList> GetIQueryableEntityList(IQueryable<CustomerTenantAccess> iQueryable)
        {
            IQueryable<CustomerTenantAccessList> entity = from a in iQueryable.Include("StatusCode").Include("UpdatedByUser.Contact").Include("UpdatedByUser")
                                                          select new CustomerTenantAccessList()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              CustomerTenant = a.CustomerTenant,
                                                              LastShipmentDate = a.LastShipmentDate,
                                                              CompanyEmail = a.CompanyEmail,
                                                              CompanyVat = a.CompanyVat,
                                                              CompanyName = a.CompanyName,
                                                              ContactMobile = a.ContactMobile,
                                                              ContactName = a.ContactName,
                                                              ContactPhone = a.ContactPhone,
                                                              LastUpdateDate = a.LastUpdateDate,
                                                              UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                                              RequestDateTime = a.RequestDateTime,
                                                              Status = a.Status,
                                                              StatusName = a.StatusCode != null ? a.StatusCode.EnglishName : null,
                                                              UpdatedByUserId = a.UpdatedByUserId,
                                                              SearchFields = a.SearchFields,
                                                              IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                                                              CustomCompanyName = a.IsPrivateLabelCustomer == true ? a.CompanyName + " (Private Label Customer)" : a.CompanyName,
                                                              StockTypeCode = a.StockTypeCode,

                                                          };


            return entity;
        }

        public CustomerTenantAccessPM GetSinglePM(string id, int tenant)
        {
            CustomerTenantAccessPM entity = (from a in repository.context.CustomerTenantAccesses.Include("UpdatedByUser.Contact").Include("UpdatedByUser").Include("StatusCode")
                                             where a.Id == id && a.Tenant == tenant
                                             select new CustomerTenantAccessPM()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 CustomerTenant = a.CustomerTenant,
                                                 LastShipmentDate = a.LastShipmentDate,
                                                 CompanyEmail = a.CompanyEmail,
                                                 CompanyVat = a.CompanyVat,
                                                 CompanyName = a.CompanyName,
                                                 ContactMobile = a.ContactMobile,
                                                 ContactName = a.ContactName,
                                                 ContactPhone = a.ContactPhone,
                                                 LastUpdateDate = a.LastUpdateDate,
                                                 RequestDateTime = a.RequestDateTime,
                                                 Status = a.Status,
                                                 StatusName = a.StatusCode != null ? a.StatusCode.EnglishName : "",
                                                 UpdatedByUserId = a.UpdatedByUserId,
                                                 SearchFields = a.SearchFields,
                                                 IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                                                 CustomCompanyName = a.IsPrivateLabelCustomer == true ? a.CompanyName + " (Private Label Customer)" : a.CompanyName,
                                                 StockTypeCode = a.StockTypeCode
                                             }).FirstOrDefault();


            CustomerTenantAccessCardRepository customerTenantAccessCardRepository = new CustomerTenantAccessCardRepository(repository.context);
            CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(customerTenantAccessCardRepository);

            entity.CustomerTenantAccessCards = customerTenantAccessCardQuery.GetCustomerTenantAccessCardPMByCustomerTenantAccessId(entity.Id, entity.Tenant);

            return entity;


        }

        public IQueryable<CustomerTenantAccessPM> GetCustomerTenantAccessPMsByTenant(int tenant)
        {

            IQueryable<CustomerTenantAccessPM> entity = from a in repository.context.CustomerTenantAccesses.Include("UpdatedByUser.Contact").Include("UpdatedByUser").Include("StatusCode")
                                                        where a.Tenant == tenant
                                                        select new CustomerTenantAccessPM()

                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            CustomerTenant = a.CustomerTenant,
                                                            LastShipmentDate = a.LastShipmentDate,
                                                            CompanyEmail = a.CompanyEmail,
                                                            CompanyVat = a.CompanyVat,
                                                            CompanyName = a.CompanyName,
                                                            ContactMobile = a.ContactMobile,
                                                            ContactName = a.ContactName,
                                                            ContactPhone = a.ContactPhone,
                                                            LastUpdateDate = a.LastUpdateDate,
                                                            RequestDateTime = a.RequestDateTime,
                                                            Status = a.Status,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            SearchFields = a.SearchFields,
                                                            IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                                                            CustomCompanyName = a.IsPrivateLabelCustomer == true ? a.CompanyName + " (Private Label Customer)" : a.CompanyName,
                                                            StockTypeCode = a.StockTypeCode
                                                        };





            return entity;
        }

        public CustomerTenantAccessPM GetSingleByIdAndCustomerTenantPM(string id, int customertenant)
        {
            CustomerTenantAccessPM entity = (from a in repository.context.CustomerTenantAccesses.Include("UpdatedByUser.Contact").Include("UpdatedByUser").Include("StatusCode")
                                             where a.Id == id && a.CustomerTenant == customertenant
                                             select new CustomerTenantAccessPM()
                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 CustomerTenant = a.CustomerTenant,
                                                 LastShipmentDate = a.LastShipmentDate,
                                                 CompanyEmail = a.CompanyEmail,
                                                 CompanyVat = a.CompanyVat,
                                                 CompanyName = a.CompanyName,
                                                 ContactMobile = a.ContactMobile,
                                                 ContactName = a.ContactName,
                                                 ContactPhone = a.ContactPhone,
                                                 LastUpdateDate = a.LastUpdateDate,
                                                 RequestDateTime = a.RequestDateTime,
                                                 Status = a.Status,
                                                 UpdatedByUserId = a.UpdatedByUserId,
                                                 SearchFields = a.SearchFields,
                                                 IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                                                 CustomCompanyName = a.IsPrivateLabelCustomer == true ? a.CompanyName + " (Private Label Customer)" : a.CompanyName,
                                                 StockTypeCode = a.StockTypeCode

                                             }).FirstOrDefault();


            CustomerTenantAccessCardRepository customerTenantAccessCardRepository = new CustomerTenantAccessCardRepository(repository.context);
            CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(customerTenantAccessCardRepository);

            entity.CustomerTenantAccessCards = customerTenantAccessCardQuery.GetCustomerTenantAccessCardPMByCustomerTenantAccessId(entity.Id, entity.Tenant);

            return entity;


        }
        public CustomerTenantAccessInfo GetCustomerTenantAccessInfo(int Tenant, string Customerid)
        {
            CustomerTenantAccessInfo customerTenantAccessInfo;
            string key = Customerid + "_" + Tenant + "_info";
            //if (CacheManager.CacheWrapper.Get(key) == null)
            //{
            CustomerTenantAccessCardQuery customerTenantAccessCardQuery = new CustomerTenantAccessCardQuery(Tenant);
            CustomerTenantAccessCardPM customerTenantAccessCardPM = customerTenantAccessCardQuery.GetSingleCustomerTenantAccessCardPMById(Customerid, Tenant);// todo: we should check this one 
            if (customerTenantAccessCardPM != null)
            {
                customerTenantAccessInfo = (from a in repository.context.CustomerTenantAccesses.Include("CustomerTenantAccessCards").Include("CustomerTenantAccessStatusType")
                                            where a.Id == customerTenantAccessCardPM.CustomerTenantAccessId && a.Tenant == Tenant //&& a.Status.ToUpper() == "H"
                                            select new CustomerTenantAccessInfo()
                                            {
                                                HasAccess = true,
                                                CustomerTenant = a.CustomerTenant

                                            }).FirstOrDefault();
            }
            else
            {
                customerTenantAccessInfo = new CustomerTenantAccessInfo() { HasAccess = false };
            }
            //CacheManager.CacheWrapper.Insert(key, customerTenantAccessInfo, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            //}
            //else
            //{
            //    customerTenantAccessInfo = (CustomerTenantAccessInfo)CacheManager.CacheWrapper.Get(key);
            //}

            return customerTenantAccessInfo;
        }


        public IQueryable<CustomerTenantAccessList> GetCustomerTenantAccessListByTenant(int tenant)
        {
            IQueryable<CustomerTenantAccessList> entity = from a in repository.context.CustomerTenantAccesses.Include("StatusCode").Include("UpdatedByUser.Contact").Include("UpdatedByUser")
                                                          where a.Tenant == tenant
                                                          select new CustomerTenantAccessList()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              CustomerTenant = a.CustomerTenant,
                                                              LastShipmentDate = a.LastShipmentDate,
                                                              CompanyEmail = a.CompanyEmail,
                                                              CompanyVat = a.CompanyVat,
                                                              CompanyName = a.CompanyName,
                                                              ContactMobile = a.ContactMobile,
                                                              ContactName = a.ContactName,
                                                              ContactPhone = a.ContactPhone,
                                                              LastUpdateDate = a.LastUpdateDate,
                                                              UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact.EnglishName : null,
                                                              RequestDateTime = a.RequestDateTime,
                                                              Status = a.Status,
                                                              StatusName = a.StatusCode != null ? a.StatusCode.EnglishName : null,
                                                              UpdatedByUserId = a.UpdatedByUserId,
                                                              SearchFields = a.SearchFields,
                                                              IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                                                              CustomCompanyName = a.IsPrivateLabelCustomer == true ? a.CompanyName + " (Private Label Customer)" : a.CompanyName,
                                                              StockTypeCode = a.StockTypeCode,

                                                          };


            return entity;

        }

        public List<CustomerTenantAccessList> GetLastCustomerRequests(int tenant)
        {
            CustomerTenantAccessRepository entityRepository = new CustomerTenantAccessRepository(tenant);
            IQueryable<CustomerTenantAccess> entities = repository.GetlasCustomerTenantAccessList(tenant);
            List<CustomerTenantAccessList> CustomerTenantAccessList = (from view in entities
                                                                       select new CustomerTenantAccessList()
                                                                       {
                                                                           Id = view.Id,
                                                                           Tenant = view.Tenant,
                                                                           CustomerTenant = view.CustomerTenant,
                                                                           LastShipmentDate = view.LastShipmentDate,
                                                                           CompanyName = view.CompanyName,
                                                                           CompanyEmail = view.CompanyEmail,
                                                                           CompanyVat = view.CompanyVat,
                                                                           ContactName = view.ContactName,
                                                                           ContactMobile = view.ContactMobile,
                                                                           ContactPhone = view.ContactPhone,
                                                                           LastUpdateDate = view.LastUpdateDate,
                                                                           UpdatedByUserName = view.UpdatedByUserId,
                                                                           RequestDateTime = view.RequestDateTime,
                                                                           Status = view.Status,
                                                                           UpdatedByUserId = view.UpdatedByUserId,
                                                                           SearchFields = view.SearchFields,
                                                                           IsPrivateLabelCustomer = view.IsPrivateLabelCustomer,
                                                                           CustomCompanyName = view.IsPrivateLabelCustomer == true ? view.CompanyName + " (Private Label Customer)" : view.CompanyName,
                                                                           StockTypeCode = view.StockTypeCode,

                                                                       }).ToList();


            return CustomerTenantAccessList;
        }

        public CustomerTenantAccessPM GetCustomerTenantAccessPMsByTenantCustomerTenant(int tenant, int CustomerTenant)
        {

            CustomerTenantAccessPM entity = (from a in repository.context.CustomerTenantAccesses.Include("UpdatedByUser.Contact").Include("UpdatedByUser").Include("StatusCode")
                                             where a.Tenant == tenant && a.CustomerTenant == CustomerTenant
                                             select new CustomerTenantAccessPM()

                                             {
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 CustomerTenant = a.CustomerTenant,
                                                 LastShipmentDate = a.LastShipmentDate,
                                                 CompanyEmail = a.CompanyEmail,
                                                 CompanyVat = a.CompanyVat,
                                                 CompanyName = a.CompanyName,
                                                 ContactMobile = a.ContactMobile,
                                                 ContactName = a.ContactName,
                                                 ContactPhone = a.ContactPhone,
                                                 LastUpdateDate = a.LastUpdateDate,
                                                 RequestDateTime = a.RequestDateTime,
                                                 Status = a.Status,
                                                 UpdatedByUserId = a.UpdatedByUserId,
                                                 SearchFields = a.SearchFields,
                                                 IsPrivateLabelCustomer = a.IsPrivateLabelCustomer,
                                                 CustomCompanyName = a.IsPrivateLabelCustomer == true ? a.CompanyName + " (Private Label Customer)" : a.CompanyName,
                                                 StockTypeCode = a.StockTypeCode
                                             }).FirstOrDefault();





            return entity;
        }


        public IQueryable<int> GetCustomerTenantAccessListsByCustomer( List<int?> customertenant , string stockTypeCode)
        {
            IQueryable<int> results = (from a in repository.context.CustomerTenantAccesses
                                                      where customertenant.Contains(a.CustomerTenant) && a.StockTypeCode == stockTypeCode
                                                      select a.CustomerTenant);
            return results;


        }

        public List<string> GetCustomerTenantAccessListsByIdsLists(List<string> ids, string stockTypeCode)
        {
            List<string> results = (from a in repository.context.CustomerTenantAccesses
                                    where ids.Contains(a.Id) && a.StockTypeCode == stockTypeCode
                                    select a.Id).ToList();
            return results;


        }

      
        public IQueryable<CustomerTenantAccessList> GetCustomerTenantAccessesByImporterVat(string ImporterVat)
        {

            IQueryable<CustomerTenantAccessList> results = (from a in repository.context.CustomerTenantAccesses
                                                            where a.CompanyVat == ImporterVat  && a.Status!= "IA"
                                                            select new CustomerTenantAccessList()
                                                            {
                                                                Id = a.Id,
                                                                CustomerTenant = a.CustomerTenant,
                                                            });
            return results;
        }


    }
}
