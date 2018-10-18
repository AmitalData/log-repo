using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ContactDoneMethodQuery
    {
        ContactDoneMethodRepository repository;

        public ContactDoneMethodQuery()
        {
            repository = new ContactDoneMethodRepository(); 
        }

        public ContactDoneMethodQuery(int tenant)
        {
            repository = new ContactDoneMethodRepository(tenant);
        }

        public ContactDoneMethodQuery(ContactDoneMethodRepository repository)
        {
            this.repository = repository;
        }

        public ContactDoneMethodPM GetSingleContactDoneMethodPM(string code)
        {
            return (from a in repository.context.ContactDoneMethods
                    where a.Code == code
                    select new ContactDoneMethodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public ContactDoneMethodPM GetSinglePM(string code)
        {
            return (from a in repository.context.ContactDoneMethods
                    where a.Code == code
                    select new ContactDoneMethodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public ContactDoneMethodPM GetSinglePM(string code, int tenant)
        {
            return (from a in repository.context.ContactDoneMethods
                    where a.Code == code
                    select new ContactDoneMethodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<ContactDoneMethodPM> GetContactDoneMethodPMs()
        {
            return (from a in repository.context.ContactDoneMethods

                    select new ContactDoneMethodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, });
        }

        public IQueryable<ContactDoneMethodList> GetIQueryableEntityList(IQueryable<ContactDoneMethod> iQueryable)
        {
            IQueryable<ContactDoneMethodList> result = from entity in iQueryable
                                             select new ContactDoneMethodList()
                                             {
                                                 Name = entity.Name,
                                                 Code = entity.Code,
                                                 SearchFields = entity.SearchFields,
                                             };
            return result;
        }
    }
}