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
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerTenantAccessCardBatchQuery
    {
        CustomerTenantAccessCardsBatchRepository repository;

        public CustomerTenantAccessCardBatchQuery()
        {
            repository = new CustomerTenantAccessCardsBatchRepository();
        }

        public CustomerTenantAccessCardBatchQuery(int tenant)
        {
            repository = new CustomerTenantAccessCardsBatchRepository(tenant);
        }

        public CustomerTenantAccessCardBatchQuery(CustomerTenantAccessCardsBatchRepository CustomerTenantAccessCardsBatchRepository)
        {
            repository = CustomerTenantAccessCardsBatchRepository;
        }

        public IQueryable<CustomerTenantAccessCardsBatchList> GetIQueryableEntityList(IQueryable<CustomerTenantAccessCardsBatch> iQueryable)
        {
            IQueryable<CustomerTenantAccessCardsBatchList> entity = from a in iQueryable
                                                                    select new CustomerTenantAccessCardsBatchList()
                                                                    {
                                                                        CustomerId = a.CustomerId,
                                                                        CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                                        Tenant = a.Tenant,
                                                                        CreateDateTime = a.CreateDateTime,
                                                                         DoneDate= a.DoneDate,
                                                                        FromDatetime = a.FromDatetime,
                                                                        BatchNumber = a.BatchNumber,
                                                                        ToDatetime = a.ToDatetime,
                                                                        Status = a.Status,
                                                                        TotalFailed = a.TotalFailed,
                                                                        TotalShipment = a.TotalShipment,
                                                                        Totalsucceeded = a.Totalsucceeded

                                                                    };

            return entity;

        }

        public CustomerTenantAccessCardsBatchPM GetSinglePM(string CustomerId, string CustomerTenantAccessId, string BatchNumber, int tenant)
        {
            CustomerTenantAccessCardsBatchPM entity = (from a in repository.context.CustomerTenantAccessCardsBatches
                                                       where a.CustomerId == CustomerId && a.CustomerTenantAccessId == CustomerTenantAccessId && a.BatchNumber == BatchNumber && a.Tenant == tenant
                                                       select new CustomerTenantAccessCardsBatchPM()
                                                 {
                                                     CustomerId = a.CustomerId,
                                                     CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                     Tenant = a.Tenant,
                                                     CreateDateTime = a.CreateDateTime,
                                                     DoneDate = a.DoneDate,
                                                     FromDatetime = a.FromDatetime,
                                                     BatchNumber = a.BatchNumber,
                                                     ToDatetime = a.ToDatetime,
                                                     Status = a.Status,
                                                     TotalFailed = a.TotalFailed,
                                                     TotalShipment = a.TotalShipment,
                                                     Totalsucceeded = a.Totalsucceeded

                                                 }).FirstOrDefault();


            return entity;


        }

        public CustomerTenantAccessCardsBatchPM GetSinglePM(string BatchNumber,int Tenant)
        {
            CustomerTenantAccessCardsBatchPM entity = (from a in repository.context.CustomerTenantAccessCardsBatches
                                                       where a.BatchNumber == BatchNumber && a.Tenant == Tenant
                                                       select new CustomerTenantAccessCardsBatchPM()
                                                       {
                                                           CustomerId = a.CustomerId,
                                                           CustomerTenantAccessId = a.CustomerTenantAccessId,
                                                           Tenant = a.Tenant,
                                                           CreateDateTime = a.CreateDateTime,
                                                           DoneDate = a.DoneDate,
                                                           FromDatetime = a.FromDatetime,
                                                           BatchNumber = a.BatchNumber,
                                                           ToDatetime = a.ToDatetime,
                                                           Status = a.Status,
                                                           TotalFailed = a.TotalFailed,
                                                           TotalShipment = a.TotalShipment,
                                                           Totalsucceeded = a.Totalsucceeded

                                                       }).FirstOrDefault();


            return entity;


        }

        public IQueryable<CustomerTenantAccessCardsBatchPM> GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId(string CustomerId, string CustomerTenantAccessId, int tenant)
        {
            

            var query = from a in repository.context.CustomerTenantAccessCardsBatches
                        where a.Tenant == tenant && a.CustomerId == CustomerId && a.CustomerTenantAccessId == CustomerTenantAccessId
                        select new CustomerTenantAccessCardsBatchPM()
                        {
                            CustomerId = a.CustomerId,
                            CustomerTenantAccessId = a.CustomerTenantAccessId,
                            Tenant = a.Tenant,
                            CreateDateTime = a.CreateDateTime,
                            DoneDate = a.DoneDate,
                            FromDatetime = a.FromDatetime,
                            BatchNumber = a.BatchNumber,
                            ToDatetime = a.ToDatetime,
                            Status = a.Status,
                            TotalFailed = a.TotalFailed,
                            TotalShipment = a.TotalShipment,
                            Totalsucceeded = a.Totalsucceeded
                        };

                return query;
        }

        public CustomerTenantAccessCardsBatchPM GetOldestCustomerTenantAccessCardsBatch(string CustomerId, string CustomerTenantAccessId, int tenant)
        {


            var query = (from a in repository.context.CustomerTenantAccessCardsBatches
                        where a.Tenant == tenant && a.CustomerId == CustomerId && a.CustomerTenantAccessId == CustomerTenantAccessId
                        orderby a.CreateDateTime ascending
                        select new CustomerTenantAccessCardsBatchPM()
                        {
                            CustomerId = a.CustomerId,
                            CustomerTenantAccessId = a.CustomerTenantAccessId,
                            Tenant = a.Tenant,
                            CreateDateTime = a.CreateDateTime,
                            DoneDate = a.DoneDate,
                            FromDatetime = a.FromDatetime,
                            BatchNumber = a.BatchNumber,
                            ToDatetime = a.ToDatetime,
                            Status = a.Status,
                            TotalFailed = a.TotalFailed,
                            TotalShipment = a.TotalShipment,
                            Totalsucceeded = a.Totalsucceeded
                        }).FirstOrDefault();

            return query;
        }

        public CustomerTenantAccessCardsBatchPM GetOldestCustomerTenantAccessCardsBatch(string CustomerId, int tenant,int ImporterTenant)
        {

            var CustomerTenantAccess = (from a in repository.context.CustomerTenantAccesses where a.Tenant == tenant && a.CustomerTenant == ImporterTenant select a).FirstOrDefault();
            var query = (from a in repository.context.CustomerTenantAccessCardsBatches
                         where a.Tenant == tenant && a.CustomerId == CustomerId && a.CustomerTenantAccessId == CustomerTenantAccess.Id
                         orderby a.CreateDateTime ascending
                         select new CustomerTenantAccessCardsBatchPM()
                         {
                             CustomerId = a.CustomerId,
                             CustomerTenantAccessId = a.CustomerTenantAccessId,
                             Tenant = a.Tenant,
                             CreateDateTime = a.CreateDateTime,
                             DoneDate = a.DoneDate,
                             FromDatetime = a.FromDatetime,
                             BatchNumber = a.BatchNumber,
                             ToDatetime = a.ToDatetime,
                             Status = a.Status,
                             TotalFailed = a.TotalFailed,
                             TotalShipment = a.TotalShipment,
                             Totalsucceeded = a.Totalsucceeded
                         }).FirstOrDefault();

            return query;
        }

        //public CustomerTenantAccessCardsBatchPM GetSingleCustomerTenantAccessCardPMById(string CustomerId, int Tenant)
        //{
        //    CustomerTenantAccessCardsBatchPM entity = (from a in repository.context.CustomerTenantAccessCards
        //                                         where a.CustomerId == CustomerId && a.StatusTypeCode.ToUpper() != "IA" && a.Tenant == Tenant
        //                                               select new CustomerTenantAccessCardsBatchPM()
        //                                         {
        //                                             CustomerId = a.CustomerId,
        //                                             CustomerTenantAccessId = a.CustomerTenantAccessId,
        //                                             Tenant = a.Tenant,
        //                                             CreateDateTime = a.CreateDateTime,
        //                                             LastShipmentDateInQueue = a.LastShipmentDateInQueue,
        //                                             StartDatetime = a.StartDatetime,
        //                                             BatchNumber = a.BatchNumber,
        //                                             EndDatetime = a.EndDatetime,
        //                                             Status = a.Status,
        //                                             TotalFailed = a.TotalFailed,
        //                                             TotalShipment = a.TotalShipment,
        //                                             Totalsucceeded = a.Totalsucceeded

        //                                         }).FirstOrDefault();


        //    return entity;


        //}



        //public List<CustomerTenantAccessCardPM> GetCustomerTenantAccessCardPMByCustomerTenantAccessId(string customerTenantAccessId, int Tenant)
        //{
        //    List<CustomerTenantAccessCardPM> CustomerTenantAccessCards = (from a in repository.context.CustomerTenantAccessCards.Include("Customer").Include("Customer.Card").Include("StatusType").Include("CreateByUser.Contact").Include("CreateByUser")
        //                                                                  where a.CustomerTenantAccessId == customerTenantAccessId
        //                                                                  where a.Tenant == Tenant
        //                                                                  select new CustomerTenantAccessCardPM()
        //                                                      {
        //                                                          CustomerId = a.CustomerId,
        //                                                          CustomerTenantAccessId = a.CustomerTenantAccessId,
        //                                                          Tenant = a.Tenant,
        //                                                          CreateByUserId = a.CreateByUserId,
        //                                                          CreateDate = a.CreateDate,
        //                                                          LastShipmentDateInQueue = a.LastShipmentDateInQueue,
        //                                                          CustomerCode = a.Customer.Card.Code,
        //                                                          CustomerName = a.Customer.Card.EnglishName,
        //                                                          HybridStartDate = a.HybridStartDate,
        //                                                          LastMappingDateTime = a.LastMappingDateTime,
        //                                                          UpdateDateTime = a.UpdateDateTime,
        //                                                          StatusTypeCode = a.StatusTypeCode,
        //                                                          StatusType = a.StatusType != null ? a.StatusType.EnglishName : null,
        //                                                      }).ToList();
        //    return CustomerTenantAccessCards.ToList();
        //}


        //public bool IsCustomerTenantAccessHasCards(string customerTenantAccessId)
        //{
        //    int CardsCount = 0;
        //    var Cards = repository.context.CustomerTenantAccessCards.Where(a => a.CustomerTenantAccessId == customerTenantAccessId);
        //    if (Cards != null)
        //    {
        //        CardsCount = Cards.Count();
        //    }

        //    if (CardsCount <= 1)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
