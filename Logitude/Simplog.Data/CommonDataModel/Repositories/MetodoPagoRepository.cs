using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class MetodoPagoRepository : IRepository<MetodoPago>
    {
        ICommonDataContext commonDataContext;

        public MetodoPagoRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public MetodoPagoRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public MetodoPagoRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<MetodoPago> GetMetodoPagos()
        {
            return context.MetodoPagos;
        }

        public IQueryable<MetodoPago> GetAll()
        {
            return context.MetodoPagos;
        }

        public MetodoPago GetSingleMetodoPago(string code, int tenant = 0)
        {
            return (from record in context.MetodoPagos where record.Code == code select record).FirstOrDefault();
        }

        public MetodoPago GetSingleMetodoPagoUpdate(string code, int tenant)
        {
            return (from record in context.MetodoPagos where record.Code == code select record).FirstOrDefault();
        }

        public void Add(MetodoPago entity)
        {
            context.MetodoPagos.Add(entity);
        }

        public void Remove(MetodoPago entity)
        {
            try
            {
                context.MetodoPagos.Attach(entity);
            }
            catch { }
            context.MetodoPagos.Remove(entity);
        }

        public void Update(MetodoPago entity)
        {
            try
            {
                context.MetodoPagos.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<MetodoPago> All()
        {
            return context.MetodoPagos.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<MetodoPago> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MetodoPago GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
