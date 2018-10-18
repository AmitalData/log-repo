using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class OBLTypeRepository : IRepository<OBLType>
    {
        IShipmentsContext shipmentsContext;

        public OBLTypeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public OBLTypeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public OBLType GetSingleOBLType(string code)
        {
            return (from a in context.OBLTypes where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<OBLType> GetAll()
        {
            return (from a in context.OBLTypes select a);
        }
        public IQueryable<OBLType> GetOBLTypes()
        {
            return (from a in context.OBLTypes select a);
        }

        public void Add(OBLType entity)
        {
            context.OBLTypes.Add(entity);
        }

        public void Remove(OBLType entity)
        {
            context.OBLTypes.Attach(entity);
            context.OBLTypes.Remove(entity);
        }

        public void Update(OBLType entity)
        {
            context.OBLTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OBLType> All()
        {
            return context.OBLTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<OBLType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public OBLType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
