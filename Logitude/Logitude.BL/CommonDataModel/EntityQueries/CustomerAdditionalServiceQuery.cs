using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityDws;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerAdditionalServiceQuery
    {
        CustomerAdditionalServiceRepository repository;



        public CustomerAdditionalServiceQuery(int tenant)
        {
            repository = new CustomerAdditionalServiceRepository(tenant);
        }

        public CustomerAdditionalServiceQuery(CustomerAdditionalServiceRepository repository)
        {
            this.repository = repository;
        }

        public CustomerAdditionalServicePM GetSinglePM(string customerId, string serviceId, int tenant)
        {
            return (from a in repository.context.CustomerAdditionalServices.Include("AdditionalService").Include("Customer")
             where a.Tenant == tenant && a.CustomerId == customerId && a.AdditionalServiceId == serviceId
             select new CustomerAdditionalServicePM()
             {
                 CustomerId = a.CustomerId,
                 AdditionalServiceId = a.AdditionalServiceId,
                 Tenant = a.Tenant,
                 CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                 AdditionalServiceName = a.AdditionalService != null ? a.AdditionalService.Name : null,
                 Potential = a.Potential,
                 Notes = a.Notes,
                 NotesRightToLeft = a.NotesRightToLeft,
             }).FirstOrDefault();
        }


        public List<CustomerAdditionalServicePM> GetCustomerAdditionalServicesByListsCustomerIds(List<string> customerIds, int tenant)
        {
            List<CustomerAdditionalServicePM> result =

                    (from a in repository.context.CustomerAdditionalServices.Include("AdditionalService").Include("Customer")
                     where a.Tenant == tenant && customerIds.Contains(a.CustomerId)
                     select new CustomerAdditionalServicePM()
                     {
                         CustomerId = a.CustomerId,
                         AdditionalServiceId = a.AdditionalServiceId,
                         Tenant = a.Tenant,
                         CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                         AdditionalServiceName = a.AdditionalService != null ? a.AdditionalService.Name : null,
                         Potential = a.Potential,
                         Notes = a.Notes,
                         NotesRightToLeft = a.NotesRightToLeft,
                     }).ToList();

            return result;

        }


        public List<CustomerAdditionalServicePM> GetCustomerAdditionalServicesByCustomerId(string customerId, int tenant)
        {
            List<CustomerAdditionalServicePM> result =

                (from a in repository.context.CustomerAdditionalServices.Include("AdditionalService").Include("Customer")
                 where a.Tenant == tenant && a.CustomerId == customerId
                 select new CustomerAdditionalServicePM()
                 {
                     CustomerId = a.CustomerId,
                     AdditionalServiceId = a.AdditionalServiceId,
                     Tenant = a.Tenant,   
                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                     AdditionalServiceName = a.AdditionalService != null ? a.AdditionalService.Name : null,
                     Potential = a.Potential, 
                     Notes = a.Notes,
                     NotesRightToLeft = a.NotesRightToLeft,
                 }).ToList();

            return result;
        }

        public IQueryable<CustomerAdditionalServiceList> GetIQueryableEntityList(IQueryable<CustomerAdditionalService> iQueryable)
        {
            IQueryable<CustomerAdditionalServiceList> result =

                from a in iQueryable.Include("AdditionalService").Include("Customer")
                select new CustomerAdditionalServiceList()
                {
                    CustomerId = a.CustomerId,
                    AdditionalServiceId = a.AdditionalServiceId,
                    Tenant = a.Tenant,
                    CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                    AdditionalServiceName = a.AdditionalService != null ? a.AdditionalService.Name : null,
                    Potential = a.Potential,
                    Notes = a.Notes,
                    NotesRightToLeft = a.NotesRightToLeft,
                };

            return result;
        }

        public List<CustomerAdditionalServicePM> GetCustomerAdditionalServices(int tenant)
        {
            List<CustomerAdditionalServicePM> result =

                (from a in repository.context.CustomerAdditionalServices.Include("AdditionalService").Include("Customer").Include("Customer.Card.PrimaryContact").Include("Customer.SalesmanUser")
                 where a.Tenant == tenant
                 select new CustomerAdditionalServicePM()
                 {
                     CustomerId = a.CustomerId,
                     AdditionalServiceId = a.AdditionalServiceId,
                     Tenant = a.Tenant,
                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                     AdditionalServiceName = a.AdditionalService != null ? a.AdditionalService.Name : null,
                     Potential = a.Potential,
                     PrimaryContact = a.Customer != null ? (a.Customer.Card.PrimaryContact != null ? a.Customer.Card.PrimaryContact.EnglishName : null ) : null,
                     Salesman = a.Customer != null ? (a.Customer.SalesmanUser != null ? a.Customer.SalesmanUser.Contact.EnglishName : null) : null,
                     SalesmanUserId = a.Customer != null ? a.Customer.SalesmanUserId : null,
                     BusinessUnitId = a.Customer != null ? (a.Customer.SalesmanUser != null ? a.Customer.SalesmanUser.BusinessUnitId : null) : null,
                     Notes = a.Notes,
                     NotesRightToLeft = a.NotesRightToLeft,
                 }).ToList();

            return result;
        }


        public List<CustomerAdditionalServiceDW> GetCustomerAdditionalServicesDW(int tenant)
        {
            List<CustomerAdditionalServiceDW> result =

                (from a in repository.context.CustomerAdditionalServices.Include("AdditionalService").Include("Customer").Include("Customer.Card.PrimaryContact").Include("Customer.SalesmanUser")
                 where a.Tenant == tenant
                 select new CustomerAdditionalServiceDW()
                 {
                     CustomerId = a.CustomerId,
                     AdditionalServiceId = a.AdditionalServiceId,
                     Tenant = a.Tenant,
                     CustomerName = a.Customer != null ? a.Customer.Card.EnglishName : null,
                     AdditionalServiceName = a.AdditionalService != null ? a.AdditionalService.Name : null,
                     Potential = a.Potential,
                     PrimaryContact = a.Customer != null ? (a.Customer.Card.PrimaryContact != null ? a.Customer.Card.PrimaryContact.EnglishName : null) : null,
                     Salesman = a.Customer != null ? (a.Customer.SalesmanUser != null ? a.Customer.SalesmanUser.Contact.EnglishName : null) : null,
                     SalesmanUserId = a.Customer != null ? a.Customer.SalesmanUserId : null,
                     BusinessUnitId = a.Customer != null ? (a.Customer.SalesmanUser != null ? a.Customer.SalesmanUser.BusinessUnitId : null) : null,
                 }).ToList();

            return result;
        }
    }
}