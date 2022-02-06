using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class RegimenFiscalRepository : IRepository<RegimenFiscal>
    {
        ICommonDataContext commonDataContext;

        public RegimenFiscalRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public RegimenFiscalRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public RegimenFiscalRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<RegimenFiscal> GetRegimenFiscals()
        {
            return context.RegimenFiscals;
        }

        public IQueryable<RegimenFiscal> GetAll()
        {
            return context.RegimenFiscals;
        }

        public RegimenFiscal GetSingleRegimenFiscal(string code, int tenant = 0)
        {
            return (from record in context.RegimenFiscals where record.Code == code select record).FirstOrDefault();
        }

        public RegimenFiscal GetSingleRegimenFiscalUpdate(string code, int tenant)
        {
            return (from record in context.RegimenFiscals where record.Code == code select record).FirstOrDefault();
        }

        public void Add(RegimenFiscal entity)
        {
            context.RegimenFiscals.Add(entity);
        }

        public void Remove(RegimenFiscal entity)
        {
            try
            {
                context.RegimenFiscals.Attach(entity);
            }
            catch { }
            context.RegimenFiscals.Remove(entity);
        }

        public void Update(RegimenFiscal entity)
        {
            try
            {
                context.RegimenFiscals.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<RegimenFiscal> All()
        {
            return context.RegimenFiscals.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<RegimenFiscal> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public RegimenFiscal GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
