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
    public class PasswordPolicyQuery
    {
        PasswordPolicyRepository repository;

        public PasswordPolicyQuery()
        {
            repository = new PasswordPolicyRepository(); 
        }

        public PasswordPolicyQuery(int tenant)
        {
            repository = new PasswordPolicyRepository(tenant);
        }

        public PasswordPolicyQuery(PasswordPolicyRepository repository)
        {
            this.repository = repository;
        }

        public PasswordPolicyPM GetSinglePasswordPolicyPM(string code)
        {
            return (from a in repository.context.PasswordPolicies
                    where a.Code == code
                    select new PasswordPolicyPM()
                    {
                        Code = a.Code,
                        PasswordStrength = a.PasswordStrength,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public PasswordPolicyPM GetSinglePM(string code, int tenant = 0)
        {
            return (from a in repository.context.PasswordPolicies
                    where a.Code == code
                    select new PasswordPolicyPM()
                    {
                        Code = a.Code,
                        PasswordStrength = a.PasswordStrength,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<PasswordPolicyPM> GetPasswordPolicyPMs()
        {
            return from a in repository.context.PasswordPolicies
                   select new PasswordPolicyPM()
                   {
                       Code = a.Code,
                       PasswordStrength = a.PasswordStrength,
                       SearchFields = a.SearchFields,
                   };
        }

        public IQueryable<PasswordPolicyList> GetIQueryableEntityList(IQueryable<PasswordPolicy> iQueryable)
        {
            IQueryable<PasswordPolicyList> result = from entity in iQueryable
                                                    select new PasswordPolicyList()
                                                    {
                                                        Code = entity.Code,
                                                        PasswordStrength = entity.PasswordStrength,
                                                        SearchFields = entity.SearchFields,
                                                    };
            return result;
        }
    }
}