using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VatUniquePartnerTypeRepository : IRepository<VatUniquePartnerType>
    {
        ICommonDataContext Context;
        public ICommonDataContext context
        {
            get { return Context; }
        }



        public VatUniquePartnerTypeRepository(int tenant)
        {
            this.Context = CommonDataContext.GetContext(tenant);
        }

        public VatUniquePartnerTypeRepository(ICommonDataContext context)
        {
            this.Context = context;
        }

        public IQueryable<VatUniquePartnerType> GetVatUniquePartnerTypes()
        {
            return context.VatUniquePartnerTypes;
        }

        public IQueryable<VatUniquePartnerType> GetAll()
        {
            return context.VatUniquePartnerTypes;
        }

        public VatUniquePartnerType GetSingleVatUniquePartnerType(string code)
        {
            return (from a in context.VatUniquePartnerTypes where a.Code == code select a).FirstOrDefault();
        }

        public void Add(VatUniquePartnerType entity)
        {
            context.VatUniquePartnerTypes.Add(entity);
        }

        public void Remove(VatUniquePartnerType entity)
        {
            context.VatUniquePartnerTypes.Remove(entity);
        }

        public void Update(VatUniquePartnerType entity)
        {
            context.VatUniquePartnerTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VatUniquePartnerType> All()
        {
            return context.VatUniquePartnerTypes.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<VatUniquePartnerType> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public VatUniquePartnerType GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
