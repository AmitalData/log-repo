using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VatMandatoryTypeRepository : IRepository<VatMandatoryType>
    {
        ICommonDataContext Context;
        public ICommonDataContext context
        {
            get { return Context; }
        }



        public VatMandatoryTypeRepository(int tenant)
        {
            this.Context = CommonDataContext.GetContext(tenant);
        }

        public VatMandatoryTypeRepository(ICommonDataContext context)
        {
            this.Context = context;
        }

        public IQueryable<VatMandatoryType> GetVatMandatoryTypes()
        {
            return context.VatMandatoryTypes;
        }
        public IQueryable<VatMandatoryType> GetVatMandatoryType()
        {
            return context.VatMandatoryTypes;
        }
        
        public IQueryable<VatMandatoryType> GetAll()
        {
            return context.VatMandatoryTypes;
        }

        public VatMandatoryType GetSingleVatMandatoryType(string code)
        {
            return (from a in context.VatMandatoryTypes where a.Code == code select a).FirstOrDefault();
        }

        public void Add(VatMandatoryType entity)
        {
            context.VatMandatoryTypes.Add(entity);
        }

        public void Remove(VatMandatoryType entity)
        {
            context.VatMandatoryTypes.Remove(entity);
        }

        public void Update(VatMandatoryType entity)
        {
            context.VatMandatoryTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VatMandatoryType> All()
        {
            return context.VatMandatoryTypes.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<VatMandatoryType> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public VatMandatoryType GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
