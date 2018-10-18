using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class INTTRADocumentTypeRepository : IRepository<INTTRADocumentType>
    {
        IShipmentsContext shipmentsContext;
        public INTTRADocumentTypeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }
        public INTTRADocumentTypeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public INTTRADocumentType GetSingleINTTRADocumentType(string code)
        {
            return (from a in context.INTTRADocumentTypes where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<INTTRADocumentType> GetINTTRADocumentTypes()
        {
            return (from a in context.INTTRADocumentTypes select a);
        }

        public IQueryable<INTTRADocumentType> GetAll()
        {
            return (from a in context.INTTRADocumentTypes select a);
        }


        public void Add(INTTRADocumentType entity)
        {
            context.INTTRADocumentTypes.Add(entity);
        }

        public void Remove(INTTRADocumentType entity)
        {
            context.INTTRADocumentTypes.Attach(entity);
            context.INTTRADocumentTypes.Remove(entity);
        }

        public void Update(INTTRADocumentType entity)
        {
            context.INTTRADocumentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<INTTRADocumentType> All()
        {
            return context.INTTRADocumentTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<INTTRADocumentType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public INTTRADocumentType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
