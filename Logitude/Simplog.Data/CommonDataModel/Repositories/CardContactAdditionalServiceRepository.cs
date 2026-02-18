using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CardContactAdditionalServiceRepository : IRepository<CardContactAdditionalService>
    {
        ICommonDataContext commonDataContext;

        public CardContactAdditionalServiceRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CardContactAdditionalServiceRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CardContactAdditionalServiceRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CardContactAdditionalService> GetCardContactAdditionalServices(int tenant)
        {
            return (from record in context.CardContactAdditionalServices
                    where record.Tenant == tenant
                    select record);
        }

        public List<CardContactAdditionalService> GetCardContactServicesByServicesList(List<string> myList, int tenant)
        {
            return (from record in context.CardContactAdditionalServices.Include("CardContact")
                    where record.Tenant == tenant && myList.Contains(record.AdditionalServiceId)
                    select record).ToList();
        }

        public IQueryable<CardContactAdditionalService> GetAdditionalServicesByCardContactIdd(string cardContactId, int tenant)
        {
            return (from d in context.CardContactAdditionalServices
                    where d.Tenant == tenant && d.CardContactId == cardContactId
                    select d);
        }

        public CardContactAdditionalService GetSingleCardContactAdditionalService(string id, int tenant)
        {
            return (from a in commonDataContext.CardContactAdditionalServices
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(CardContactAdditionalService entity)
        {
            context.CardContactAdditionalServices.Add(entity);
        }

        public void Remove(CardContactAdditionalService entity)
        {
            context.CardContactAdditionalServices.Attach(entity);
            context.CardContactAdditionalServices.Remove(entity);
        }

        public void Update(CardContactAdditionalService entity)
        {
            try
            {
                context.CardContactAdditionalServices.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CardContactAdditionalService> All()
        {
            return context.CardContactAdditionalServices.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CardContactAdditionalService> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CardContactAdditionalService GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}