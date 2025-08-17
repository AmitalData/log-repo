using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PasswordPolicyRepository : IRepository<PasswordPolicy>
    {
        ICommonDataContext commonDataContext;



        public PasswordPolicyRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public PasswordPolicyRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PasswordPolicy> GetPasswordPolicies()
        {
            return context.PasswordPolicies;
        }

        public PasswordPolicy GetSinglePasswordPolicy(string code)
        {
            return (from record in context.PasswordPolicies where record.Code == code select record).FirstOrDefault();
        }

        public void Add(PasswordPolicy entity)
        {
            context.PasswordPolicies.Add(entity);
        }

        public void Remove(PasswordPolicy entity)
        {
            context.PasswordPolicies.Attach(entity);
            context.PasswordPolicies.Remove(entity);
        }

        public void Update(PasswordPolicy entity)
        {
            context.PasswordPolicies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PasswordPolicy> All()
        {
            return context.PasswordPolicies.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PasswordPolicy> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PasswordPolicy GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}