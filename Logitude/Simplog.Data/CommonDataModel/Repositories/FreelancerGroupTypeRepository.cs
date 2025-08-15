using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FreelancerGroupTypeRepository : IRepository<FreelancerGroupType>
    {
        ICommonDataContext commonDataContext;


        public FreelancerGroupTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public FreelancerGroupTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public FreelancerGroupType GetSingleFreelancerGroupType(string code, int tenant)
        {
            return (from d in context.FreelancerGroupTypes where d.Code == code && d.Tenant == tenant select d).FirstOrDefault();
        }
        public FreelancerGroupType GetSingle(string id,int tenant)
        {
            return (from d in context.FreelancerGroupTypes where d.Id == id && d.Tenant == tenant select d).FirstOrDefault();
        }


 
        
        public void Add(FreelancerGroupType entity)
        {
            context.FreelancerGroupTypes.Add(entity);
        }

        public void Remove(FreelancerGroupType entity)
        {
            context.FreelancerGroupTypes.Attach(entity);
            context.FreelancerGroupTypes.Remove(entity);
        }

        public void Update(FreelancerGroupType entity)
        {
            context.FreelancerGroupTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FreelancerGroupType> All()
        {
            return context.FreelancerGroupTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        List<FreelancerGroupType> IRepository<FreelancerGroupType>.GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        FreelancerGroupType IRepository<FreelancerGroupType>.GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
