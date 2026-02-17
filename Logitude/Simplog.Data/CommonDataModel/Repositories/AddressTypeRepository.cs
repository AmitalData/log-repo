using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AddressTypeRepository : IRepository<AddressType>
    {
        ICommonDataContext Context;
        public AddressTypeRepository()
        {
            Context = new CommonDataContext();

        }
        public AddressTypeRepository(ICommonDataContext context)
        {
            Context = context;

        }
        public AddressTypeRepository(int tenant)
        {
            Context = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<AddressType> GetAddressTypes()
        {
            return context.AddressTypes;
        }

        public AddressType GetSingleAddressType(string id)
        {
            return (from a in context.AddressTypes
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public void Add(AddressType entity)
        {
            context.AddressTypes.Add(entity);
        }

        public void Remove(AddressType entity)
        {
            context.AddressTypes.Remove(entity);
        }

        public void Update(AddressType entity)
        {
            context.AddressTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AddressType> All()
        {
            return context.AddressTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return Context; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AddressType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AddressType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<AddressType> GetAll()
        {
            return context.AddressTypes;
        }

    }
}