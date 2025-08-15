using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CommunicationLogTypeRepository: IRepository<CommunicationLogType>
    {
        ICommonDataContext commonDataContext;

        public CommunicationLogTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public CommunicationLogTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CommunicationLogType GetSingleCommunicationLogType(string code)
        {
            CommunicationLogType instance = (from i in context.CommunicationLogTypes
                                 where i.Code == code                                 
                                 select i).FirstOrDefault();
            return instance;
        }

        public IQueryable<CommunicationLogType> GetCommunicationLogTypes()
        {
            return context.CommunicationLogTypes;
        }

        public IQueryable<CommunicationLogType> GetAll()
        {
            return context.CommunicationLogTypes;
        }

        public void Add(CommunicationLogType entity)
        {
            context.CommunicationLogTypes.Add(entity);
        }

        public void Remove(CommunicationLogType entity)
        {
            context.CommunicationLogTypes.Attach(entity);
            context.CommunicationLogTypes.Remove(entity);
        }

        public void Update(CommunicationLogType entity)
        {
            context.CommunicationLogTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CommunicationLogType> All()
        {
            return context.CommunicationLogTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        List<CommunicationLogType> IRepository<CommunicationLogType>.GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        CommunicationLogType IRepository<CommunicationLogType>.GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}