

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
    public class LoginPolicyQuery
    {

        LoginPolicyRepository repository;

        public LoginPolicyQuery()
        {
            repository = new LoginPolicyRepository();
        }

        public LoginPolicyQuery(int tenant)
        {
            repository = new LoginPolicyRepository(tenant);
        }

        public LoginPolicyQuery(LoginPolicyRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<LoginPolicyList> GetIQueryableEntityList(IQueryable<LoginPolicy> iQueryable)
        {
            IQueryable<LoginPolicyList> result = from a in iQueryable
                                                 select new LoginPolicyList()
                                                 {

                                                     Code = a.Code,
                                                     Name = a.Name,
                                                     SearchFields = a.SearchFields,


                                                 };


            return result;
        }

        public LoginPolicyPM GetSinglePM(string code)
        {
            LoginPolicyPM entity = (from a in repository.context.LoginPolicies
                                    where a.Code == code

                                    select new LoginPolicyPM()
                                    {
                                        Code = a.Code,
                                        Name = a.Name,
                                        SearchFields = a.SearchFields,
                                    }).FirstOrDefault();
            return entity;
        }

        public IQueryable<LoginPolicyPM> GetLoginPolicyPMs()
        {
            IQueryable<LoginPolicyPM> LoginPolicyPMs = from a in repository.context.LoginPolicies
                                                                         select new LoginPolicyPM()
                                                                         {
                                                                             Code = a.Code,
                                                                             Name = a.Name,
                                                                             SearchFields = a.SearchFields,
                                                                         };
            return LoginPolicyPMs;
        }

        public IQueryable<LoginPolicyList> GetLoginPolicyLists(int tenant)
        {
            IQueryable<LoginPolicyList> LoginPolicyLists = from a in repository.context.LoginPolicies
                                                                             select new LoginPolicyList()
                                                                             {
                                                                                 Code = a.Code,
                                                                                 Name = a.Name,
                                                                                 SearchFields = a.SearchFields,

                                                                             };
            return LoginPolicyLists;
        }



    }
}