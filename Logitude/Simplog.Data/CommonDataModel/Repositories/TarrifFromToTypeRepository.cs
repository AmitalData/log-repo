using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TarrifFromToTypeRepository:IRepository<TarrifFromToType>
    {
        ICommonDataContext commonDataContext;


        public TarrifFromToTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TarrifFromToTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public TarrifFromToType GetSingleTarrifFromToType(string code)
        {
            return (from a in context.TarrifFromToTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public IQueryable<TarrifFromToType> GetTarrifFromToTypes()
        {
            return from a in context.TarrifFromToTypes
                   
                   select a;
        }

        public IQueryable<TarrifFromToType> GetAll()
        {
            return from a in context.TarrifFromToTypes

                   select a;
        }

        public void Add(TarrifFromToType entity)
        {
            context.TarrifFromToTypes.Add(entity);
        }

        public void Remove(TarrifFromToType entity)
        {
            context.TarrifFromToTypes.Attach(entity);
            context.TarrifFromToTypes.Remove(entity);
        }

        public void Update(TarrifFromToType entity)
        {
            context.TarrifFromToTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TarrifFromToType> All()
        {
            return context.TarrifFromToTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext ; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TarrifFromToType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TarrifFromToType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}