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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomerSalesNoteQuery
    {
        CustomerSalesNoteRepository repository;



        public CustomerSalesNoteQuery(int tenant)
        {
            repository = new CustomerSalesNoteRepository(tenant);
        }

        public CustomerSalesNoteQuery(CustomerSalesNoteRepository myRepository)
        {
            repository = myRepository;
        }

        public CustomerSalesNotePM GetSinglePM(string customerId, int tenant)
        {
            CustomerSalesNotePM entity =

                (from a in repository.context.CustomerSalesNotes.Include("CreatedByUser").Include("UpdatedByUser")
                 where a.Tenant == tenant && a.CustomerId == customerId
                 select new CustomerSalesNotePM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     Notes = a.Notes,
                     CustomerId = a.CustomerId,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,                     
                     UpdatedByUserId = a.UpdatedByUserId,                     
                     CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                     EventLabel = a.UpdateDate == a.CreateDate ? "Created by " : "Modified by ",
                 }).FirstOrDefault();

            return entity;
        }

        public List<CustomerSalesNotePM> GetSalesNotesByCustomerId(string customerId, int tenant)
        {
            List<CustomerSalesNotePM> result =

                (from a in repository.context.CustomerSalesNotes.Include("CreatedByUser").Include("UpdatedByUser")
                 where a.Tenant == tenant && a.CustomerId == customerId
                 select new CustomerSalesNotePM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     Notes = a.Notes,
                     CustomerId = a.CustomerId,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                     EventLabel = a.UpdateDate == a.CreateDate ? "Created by " : "Modified by ",
                 }).ToList();

            return result;
        }


    }
}
