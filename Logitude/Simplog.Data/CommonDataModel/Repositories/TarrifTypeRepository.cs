using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TarrifTypeRepository:IRepository<TarrifType>
    {
        ICommonDataContext commonDataContext;

        public TarrifTypeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TarrifTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TarrifTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public TarrifType GetSingleTarrifType(string code)
        {
            return (from a in context.TarrifTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public IQueryable<TarrifType> GetTarrifTypes()
        {
            return from a in context.TarrifTypes
                   
                   select a;
        }

        public IQueryable<TarrifType> GetAll()
        {
            return from a in context.TarrifTypes

                   select a;
        }

        public void Add(TarrifType entity)
        {
            context.TarrifTypes.Add(entity);
        }

        public void Remove(TarrifType entity)
        {
            context.TarrifTypes.Attach(entity);
            context.TarrifTypes.Remove(entity);
        }

        public void Update(TarrifType entity)
        {
            context.TarrifTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TarrifType> All()
        {
            return context.TarrifTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext ; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TarrifType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TarrifType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}