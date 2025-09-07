
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class LoginPolicyRepository : IRepository<LoginPolicy>
    {
        ICommonDataContext commonDataContext;



        public LoginPolicyRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public LoginPolicyRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<LoginPolicy> GetLoginPolicies()
        {
            return context.LoginPolicies;
        }

        public IQueryable<LoginPolicy> GetAll()
        {
            return context.LoginPolicies;
        }

        public LoginPolicy GetSingleLoginPolicy(string code)
        {
            return (from record in context.LoginPolicies where record.Code == code select record).FirstOrDefault();
        }


        public void Add(LoginPolicy entity)
        {
            context.LoginPolicies.Add(entity);
        }

        public void Remove(LoginPolicy entity)
        {
            context.LoginPolicies.Attach(entity);
            context.LoginPolicies.Remove(entity);
        }

        public void Update(LoginPolicy entity)
        {
            context.LoginPolicies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LoginPolicy> All()
        {
            return context.LoginPolicies.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<LoginPolicy> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public LoginPolicy GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
