


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
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerDepositionQuery
    {
        CustomerDepositionRepository repository;

        public CustomerDepositionQuery()
        {
            repository = new CustomerDepositionRepository();
        }

        public CustomerDepositionQuery(int tenant)
        {
            repository = new CustomerDepositionRepository(tenant);
        }

        public CustomerDepositionQuery(CustomerDepositionRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<CustomerDepositionList> GetIQueryableEntityList(IQueryable<CustomerDeposition> iQueryable)
        {
            IQueryable<CustomerDepositionList> result = from a in iQueryable
                                                     select new CustomerDepositionList()
                                                     {
                                                         Tenant = a.Tenant,
                                                         Id = a.Id,
                                                         CustomsShipperId = a.CustomsShipperId,
                                                         DepositionNumber = a.DepositionNumber,
                                                         ValidityStartDate = a.ValidityStartDate,
                                                         ValidityEndDate = a.ValidityEndDate,
                                                         CreateDate = a.CreateDate,
                                                         

                                                     };


            return result;
        }


        public CustomerDepositionPM GetSinglePM(string id, int tenant)
        {
            CustomerDepositionPM entity = (from a in repository.context.CustomerDepositions
                                        where a.Tenant == tenant
                                        && a.Id == id
                                        select new CustomerDepositionPM()
                                        {
                                            Tenant = a.Tenant,
                                            Id = a.Id,
                                            CustomsShipperId = a.CustomsShipperId,
                                            DepositionNumber = a.DepositionNumber,
                                            ValidityStartDate = a.ValidityStartDate,
                                            ValidityEndDate = a.ValidityEndDate,
                                            CreateDate = a.CreateDate,
                                        }).FirstOrDefault();


         
            return entity;
        }

        public IQueryable<CustomerDepositionPM> GetCustomerDepositionPMsByTenant(int tenant)
        {
            IQueryable<CustomerDepositionPM> CustomerDepositionPMs = from a in repository.context.CustomerDepositions
                                                               where a.Tenant == tenant
                                                               select new CustomerDepositionPM()
                                                               {
                                                                   Tenant = a.Tenant,
                                                                   Id = a.Id,
                                                                   CustomsShipperId = a.CustomsShipperId,
                                                                   DepositionNumber = a.DepositionNumber,
                                                                   ValidityStartDate = a.ValidityStartDate,
                                                                   ValidityEndDate = a.ValidityEndDate,
                                                                   CreateDate = a.CreateDate,
                                                               };
            return CustomerDepositionPMs;
        }

        public IQueryable<CustomerDepositionList> GetCustomerDepositionListsByTenant(int tenant)
        {
            IQueryable<CustomerDepositionList> CustomerDepositionLists = from a in repository.context.CustomerDepositions
                                                                   where a.Tenant == tenant
                                                                   select new CustomerDepositionList()
                                                                   {
                                                                       Tenant = a.Tenant,
                                                                       Id = a.Id,
                                                                       CustomsShipperId = a.CustomsShipperId,
                                                                       DepositionNumber = a.DepositionNumber,
                                                                       ValidityStartDate = a.ValidityStartDate,
                                                                       ValidityEndDate = a.ValidityEndDate,
                                                                       CreateDate = a.CreateDate,
                                                                   };
            return CustomerDepositionLists;
        }


        
        public List<CustomerDepositionList> GetCustomerDepositionListsByCustomsShipperId(string customsShipperId, int tenant)
        {
            List<CustomerDepositionList> customerDepositionLists = (from a in repository.context.CustomerDepositions
                                                                         where a.Tenant == tenant && a.CustomsShipperId == customsShipperId
                                                                         select new CustomerDepositionList()
                                                                         {
                                                                             Tenant = a.Tenant,
                                                                             Id = a.Id,
                                                                             CustomsShipperId = a.CustomsShipperId,
                                                                             DepositionNumber = a.DepositionNumber,
                                                                             ValidityStartDate = a.ValidityStartDate,
                                                                             ValidityEndDate = a.ValidityEndDate,
                                                                             CreateDate = a.CreateDate,
                                                                         }).OrderByDescending(d=>d.ValidityStartDate).ToList();

            foreach (CustomerDepositionList item in customerDepositionLists)
            {
                string color = "Green";

                if (item.ValidityEndDate!=null)
                {
                    var todayDate = DateTime.Now;

                    if (item.ValidityEndDate <= todayDate)
                    {
                        color = "Red";
                    }
                    else if (todayDate.AddDays(30) > item.ValidityEndDate)
                    {
                        color = "Orange"; 
                    }
                }

                item.ValidityEndDateColor = color;

            }
            

            return customerDepositionLists;
        }

    }
}