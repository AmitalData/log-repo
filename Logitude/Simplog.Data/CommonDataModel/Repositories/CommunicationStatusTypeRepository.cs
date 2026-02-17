using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CommunicationStatusTypeRepository: IRepository<CommunicationStatusType>
    {
        ICommonDataContext commonDataContext;

        public CommunicationStatusTypeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CommunicationStatusTypeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CommunicationStatusTypeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CommunicationStatusType GetSingleCommunicationStatusType(string code)
        {
            CommunicationStatusType instance = (from i in context.CommunicationStatusTypes
                                 where i.Code == code                                 
                                 select i).FirstOrDefault();
            return instance;
        }

        public IQueryable<CommunicationStatusType> GetCommunicationStatusTypes()
        {
            return context.CommunicationStatusTypes;
        }

        public IQueryable<CommunicationStatusType> GetAll()
        {
            return context.CommunicationStatusTypes;
        }

        public void Add(CommunicationStatusType entity)
        {
            context.CommunicationStatusTypes.Add(entity);
        }

        public void Remove(CommunicationStatusType entity)
        {
            context.CommunicationStatusTypes.Attach(entity);
            context.CommunicationStatusTypes.Remove(entity);
        }

        public void Update(CommunicationStatusType entity)
        {
            context.CommunicationStatusTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CommunicationStatusType> All()
        {
            return context.CommunicationStatusTypes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CommunicationStatusType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CommunicationStatusType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}