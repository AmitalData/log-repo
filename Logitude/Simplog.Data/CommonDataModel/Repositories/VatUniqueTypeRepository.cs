using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VatUniqueTypeRepository : IRepository<VatUniqueType>
    {
        ICommonDataContext Context;
        public ICommonDataContext context
        {
            get { return Context; }
        }

        public VatUniqueTypeRepository()
        {
            this.Context = new CommonDataContext();
        }

        public VatUniqueTypeRepository(int tenant)
        {
            this.Context = CommonDataContext.GetContext(tenant);
        }

        public VatUniqueTypeRepository(ICommonDataContext context)
        {
            this.Context = context;
        }

        public IQueryable<VatUniqueType> GetVatUniqueTypes()
        {
            return context.VatUniqueTypes;
        }
        public IQueryable<VatUniqueType> GetVatUniqueType()
        {
            return context.VatUniqueTypes;
        }
        
        public IQueryable<VatUniqueType> GetAll()
        {
            return context.VatUniqueTypes;
        }

        public VatUniqueType GetSingleVatUniqueType(string code)
        {
            return (from a in context.VatUniqueTypes where a.Code == code select a).FirstOrDefault();
        }

        public void Add(VatUniqueType entity)
        {
            context.VatUniqueTypes.Add(entity);
        }

        public void Remove(VatUniqueType entity)
        {
            context.VatUniqueTypes.Remove(entity);
        }

        public void Update(VatUniqueType entity)
        {
            context.VatUniqueTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VatUniqueType> All()
        {
            return context.VatUniqueTypes.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<VatUniqueType> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public VatUniqueType GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}
