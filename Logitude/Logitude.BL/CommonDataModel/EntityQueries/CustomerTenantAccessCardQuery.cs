using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerTenantAccessCardQuery
    {
        CustomerTenantAccessCardRepository repository;

        public CustomerTenantAccessCardQuery()
        {
            repository = new CustomerTenantAccessCardRepository();
        }

        public CustomerTenantAccessCardQuery(int tenant)
        {
            repository = new CustomerTenantAccessCardRepository(tenant);
        }

        public CustomerTenantAccessCardQuery(CustomerTenantAccessCardRepository CustomerTenantAccessCardRepository)
        {
            repository = CustomerTenantAccessCardRepository;
        }

        public IQueryable<CustomerTenantAccessCardList> GetIQueryableEntityList(IQueryable<CustomerTenantAccessCardList> iQueryable)
        {
            IQueryable<CustomerTenantAccessCardList> entity = from a in iQueryable.Include("User").Include("CustomerTenantAccess").Include("Customer")
                                                              select new CustomerTenantAccessCardList()
                                                              {
                                                                  CustomerId = a.CustomerId,
                                                                  CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                                  Tenant = a.Tenant,
                                                                  CreateDate = a.CreateDate,
                                                                  CreateByUserId = a.CreateByUserId,
                                                                  LastShipmentDateInQueue = a.LastShipmentDateInQueue,
                                                                  HybridStartDate = a.HybridStartDate,
                                                                  LastMappingDateTime = a.LastMappingDateTime,
                                                                  UpdateDateTime = a.UpdateDateTime,
                                                                  StatusTypeCode = a.StatusTypeCode,
                                                                  StatusType = a.StatusType,

                                                              };

            return entity;

        }

        public CustomerTenantAccessCardPM GetSinglePM(string CustomerId, string CustomerTenantAccessId, int tenant)
        {
            CustomerTenantAccessCardPM entity = (from a in repository.context.CustomerTenantAccessCards.Include("User").Include("CustomerTenantAccess").Include("Customer").Include("StatusType")
                                                 where a.CustomerId == CustomerId && a.CustomerTenantAccessId == CustomerTenantAccessId && a.Tenant == tenant
                                                 select new CustomerTenantAccessCardPM()
                                                 {
                                                     CustomerId = a.CustomerId,
                                                     CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                     Tenant = a.Tenant,
                                                     CreateDate = a.CreateDate,
                                                     CreateByUserId = a.CreateByUserId,
                                                     LastShipmentDateInQueue = a.LastShipmentDateInQueue,
                                                     //HybridStartDate = a.HybridStartDate,
                                                     LastMappingDateTime = a.LastMappingDateTime,
                                                     UpdateDateTime = a.UpdateDateTime,
                                                     StatusTypeCode = a.StatusTypeCode,
                                                     StatusType = a.StatusType != null ? a.StatusType.EnglishName : null,

                                                 }).FirstOrDefault();


            return entity;


        }

        public CustomerTenantAccessCardPM GetSingleCustomerTenantAccessCardPMById(string CustomerId, int Tenant)
        {
            CustomerTenantAccessCardPM entity = (from a in repository.context.CustomerTenantAccessCards.Include("User").Include("CustomerTenantAccess").Include("Customer").Include("StatusType")
                                                 where a.CustomerId == CustomerId && a.StatusTypeCode.ToUpper() != "IA" && a.Tenant == Tenant
                                                 select new CustomerTenantAccessCardPM()
                                                 {
                                                     CustomerId = a.CustomerId,
                                                     CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                     Tenant = a.Tenant,
                                                     CreateDate = a.CreateDate,
                                                     CreateByUserId = a.CreateByUserId,
                                                     LastShipmentDateInQueue = a.LastShipmentDateInQueue,
                                                     //HybridStartDate = a.HybridStartDate,
                                                     LastMappingDateTime = a.LastMappingDateTime,
                                                     UpdateDateTime = a.UpdateDateTime,
                                                     StatusTypeCode = a.StatusTypeCode,
                                                     StatusType = a.StatusType != null ? a.StatusType.EnglishName : null,

                                                 }).FirstOrDefault();


            return entity;


        }

        //public IQueryable<CustomerTenantAccessCardPM> GetCustomerTenantAccessCardPMsByID(string id)
        //{

        //    IQueryable<CustomerTenantAccessCardPM> entity = from a in repository.context.CustomerTenantAccessCards.Include("User").Include("CustomerTenantAccess").Include("Customer").Include("StatusType")
        //                                                    where a.CustomerId == id && a.CustomerTenantAccessId == id
        //                                                    select new CustomerTenantAccessCardPM()

        //                                                   {
        //                                                       CustomerId = a.CustomerId,
        //                                                       CustomerTenantAccessId = a.CustomerTenantAccessId,
        //                                                       CreateDate = a.CreateDate,
        //                                                       CreateByUserId = a.CreateByUserId,
        //                                                       LastShipmentDateInQueue = a.LastShipmentDateInQueue,
        //                                                       HybridStartDate = a.HybridStartDate,
        //                                                       LastMappingDateTime = a.LastMappingDateTime,
        //                                                       UpdateDateTime = a.UpdateDateTime,
        //                                                       StatusTypeCode = a.StatusTypeCode,
        //                                                       StatusType = a.StatusType != null ? a.StatusType.EnglishName : null,

        //                                                   };


        //    return entity;
        //}

        public List<CustomerTenantAccessCardPM> GetCustomerTenantAccessCardPMByCustomerTenantAccessId(string customerTenantAccessId, int Tenant)
        {
            List<CustomerTenantAccessCardPM> CustomerTenantAccessCards = (from a in repository.context.CustomerTenantAccessCards.Include("Customer").Include("Customer.Card").Include("StatusType").Include("CreateByUser.Contact").Include("CreateByUser")
                                                                          where a.CustomerTenantAccessId == customerTenantAccessId
                                                                          where a.Tenant == Tenant
                                                                          select new CustomerTenantAccessCardPM()
                                                              {
                                                                  CustomerId = a.CustomerId,
                                                                  CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                                  Tenant = a.Tenant,
                                                                  CreateByUserId = a.CreateByUserId,
                                                                  CreateDate = a.CreateDate,
                                                                  LastShipmentDateInQueue = a.LastShipmentDateInQueue,
                                                                  CustomerCode = a.Customer.Card.Code,
                                                                  CustomerName = a.Customer.Card.EnglishName,
                                                                  //HybridStartDate = a.HybridStartDate,
                                                                  LastMappingDateTime = a.LastMappingDateTime,
                                                                  UpdateDateTime = a.UpdateDateTime,
                                                                  StatusTypeCode = a.StatusTypeCode,
                                                                  StatusType = a.StatusType != null ? a.StatusType.EnglishName : null,
                                                              }).ToList();
            return CustomerTenantAccessCards.ToList();
        }

        public List<CustomerTenantAccessCardPM> GetCustomerTenantAccessCardsByTenant(int Tenant)
        {
            List<CustomerTenantAccessCardPM> CustomerTenantAccessCards = (from a in repository.context.CustomerTenantAccessCards.Include("Customer").Include("Customer.Card").Include("StatusType").Include("CreateByUser.Contact").Include("CreateByUser")
                                                                          where a.Tenant == Tenant
                                                                          select new CustomerTenantAccessCardPM()
                                                                          {
                                                                              CustomerId = a.CustomerId,
                                                                              CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                                              Tenant = a.Tenant,
                                                                              CreateByUserId = a.CreateByUserId,
                                                                              CreateDate = a.CreateDate,
                                                                              LastShipmentDateInQueue = a.LastShipmentDateInQueue,
                                                                              CustomerCode = a.Customer.Card.Code,
                                                                              CustomerName = a.Customer.Card.EnglishName,
                                                                              //HybridStartDate = a.HybridStartDate,
                                                                              LastMappingDateTime = a.LastMappingDateTime,
                                                                              UpdateDateTime = a.UpdateDateTime,
                                                                              StatusTypeCode = a.StatusTypeCode,
                                                                              StatusType = a.StatusType != null ? a.StatusType.EnglishName : null,
                                                                          }).ToList();
            return CustomerTenantAccessCards.ToList();
        }

        public CustomerTenantAccessCardPM GetIfCustomerTenantAccessCardsSelected(string CustomerId, int Tenant)
        {
            var temp = (from a in repository.context.CustomerTenantAccessCards
                        where a.Tenant == Tenant && a.CustomerId == CustomerId && a.StatusTypeCode.ToUpper() != "IA"
                        select new CustomerTenantAccessCardPM()
                                                                          {
                                                                              CustomerId = a.CustomerId,
                                                                              CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                                              Tenant = a.Tenant,
                                                                              CreateByUserId = a.CreateByUserId,
                                                                              CreateDate = a.CreateDate,
                                                                              LastShipmentDateInQueue = a.LastShipmentDateInQueue,
                                                                              CustomerCode = a.Customer.Card.Code,
                                                                              CustomerName = a.Customer.Card.EnglishName,
                                                                              //HybridStartDate = a.HybridStartDate,
                                                                              LastMappingDateTime = a.LastMappingDateTime,
                                                                              UpdateDateTime = a.UpdateDateTime,
                                                                              StatusTypeCode = a.StatusTypeCode,
                                                                              StatusType = a.StatusType != null ? a.StatusType.EnglishName : null,
                                                                          }).FirstOrDefault();
            return temp;
        }


        public bool IsCustomerTenantAccessHasCards(string customerTenantAccessId)
        {
            int CardsCount = 0;
            var Cards = repository.context.CustomerTenantAccessCards.Where(a => a.CustomerTenantAccessId == customerTenantAccessId);
            if (Cards != null)
            {
                CardsCount = Cards.Count();
            }

            if (CardsCount <= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public List<string> GetCustomerIdsByTenant(int Tenant)
        {
            List<string> customerIds = (from a in repository.context.CustomerTenantAccessCards where a.Tenant == Tenant select a.CustomerId).ToList();


            return customerIds;
        }


        public IQueryable<CustomerTenantAccessCardList> GetustomerTenantAccessCardListsByTenant(int Tenant)
        {
            IQueryable<CustomerTenantAccessCardList> customerIds = (from a in repository.context.CustomerTenantAccessCards
                                                                    where a.Tenant == Tenant && a.StatusTypeCode == "A"
                                                                    select new CustomerTenantAccessCardList()
                                                                    {
                                                                        CustomerId = a.CustomerId,
                                                                        CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                                    }
                                                             );


            return customerIds;
        }




    }
}
