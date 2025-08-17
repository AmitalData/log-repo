using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class VatFormatTypeRepository : IRepository<VatFormatType>
    {
        ICommonDataContext Context;
        public ICommonDataContext context
        {
            get { return Context; }
        }


        public VatFormatTypeRepository(int tenant)
        {
            this.Context = CommonDataContext.GetContext(tenant);
        }

        public VatFormatTypeRepository(ICommonDataContext context)
        {
            this.Context = context;
        }

        public IQueryable<VatFormatType> GetVatFormatTypes()
        {
            return context.VatFormatTypes;
        }
        public IQueryable<VatFormatType> GetVatFormatType()
        {
            return context.VatFormatTypes;
        }
        public IQueryable<VatFormatType> GetAll()
        {
            return context.VatFormatTypes;
        }

        public VatFormatType GetSingleVatFormatType(string code)
        {
            return (from a in context.VatFormatTypes where a.Code == code select a).FirstOrDefault();
        }

        public void Add(VatFormatType entity)
        {
            context.VatFormatTypes.Add(entity);
        }

        public void Remove(VatFormatType entity)
        {
            context.VatFormatTypes.Remove(entity);
        }

        public void Update(VatFormatType entity)
        {
            context.VatFormatTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VatFormatType> All()
        {
            return context.VatFormatTypes.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<VatFormatType> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public VatFormatType GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
