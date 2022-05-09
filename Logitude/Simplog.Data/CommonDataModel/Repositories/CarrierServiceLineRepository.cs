using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CarrierServiceLineRepository : IRepository<CarrierServiceLine>
    {
        ICommonDataContext commonDataContext;

        public CarrierServiceLineRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CarrierServiceLineRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CarrierServiceLineRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IQueryable<CarrierServiceLine> GetCarrierServiceLines(int tenant)
        {
            return (from record in context.CarrierServiceLines
                    where record.Tenant == tenant
                    select record);
        }

        public IQueryable<CarrierServiceLine> GetProductsByCardId(string cardId, int tenant)
        {
            return (from d in context.CarrierServiceLines
                    where d.Tenant == tenant && d.CardId == cardId
                    select d);
        }

        public CarrierServiceLine GetSingleCarrierServiceLine(string id, int tenant)
        {
            return (from a in commonDataContext.CarrierServiceLines
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public void Add(CarrierServiceLine entity)
        {
            context.CarrierServiceLines.Add(entity);
        }

        public void Remove(CarrierServiceLine entity)
        {
            context.CarrierServiceLines.Attach(entity);
            context.CarrierServiceLines.Remove(entity);
        }

        public void Update(CarrierServiceLine entity)
        {
            try
            {
                context.CarrierServiceLines.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CarrierServiceLine> All()
        {
            return context.CarrierServiceLines.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CarrierServiceLine> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CarrierServiceLine GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
