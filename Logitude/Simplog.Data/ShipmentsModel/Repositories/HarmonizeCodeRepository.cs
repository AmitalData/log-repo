using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class HarmonizeCodeRepository : IRepository<HarmonizeCode>
    {
        IShipmentsContext iContext;

        public HarmonizeCodeRepository(int tenant)
        {
            iContext = ShipmentsContext.GetContext(tenant);
        }

        public HarmonizeCodeRepository(IShipmentsContext context)
        {
            iContext = context;
        }

        public HarmonizeCode GetSingleHarmonizeCode(string code)
        {
            return (from a in Context.HarmonizeCodes where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<HarmonizeCode> GetHarmonizeCodes()
        {
            return (from a in Context.HarmonizeCodes select a);
        }

        public IQueryable<HarmonizeCode> GetAll()
        {
            return (from a in Context.HarmonizeCodes select a);
        }

        public void Add(HarmonizeCode entity)
        {
            Context.HarmonizeCodes.Add(entity);
        }

        public void Remove(HarmonizeCode entity)
        {
            Context.HarmonizeCodes.Attach(entity);
            Context.HarmonizeCodes.Remove(entity);
        }

        public void Update(HarmonizeCode entity)
        {
            Context.HarmonizeCodes.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<HarmonizeCode> All()
        {
            return Context.HarmonizeCodes.ToList();
        }

        public IShipmentsContext Context
        {
            get { return iContext; }
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public List<HarmonizeCode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public HarmonizeCode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
