using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class NumberFormatRepository : IRepository<NumberFormat>
    {
        ICommonDataContext commonDataContext;

        public NumberFormatRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public NumberFormatRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public NumberFormatRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public NumberFormat GetSingleNumberFormat(string code)
        {
            NumberFormat instance = (from i in context.NumberFormats
                                             where i.Code == code
                                             select i).FirstOrDefault();
            return instance;
        }

        public IQueryable<NumberFormat> GetNumberFormats()
        {
            return context.NumberFormats;
        }

        public IQueryable<NumberFormat> GetAll()
        {
            return context.NumberFormats;
        }

        public void Add(NumberFormat entity)
        {
            context.NumberFormats.Add(entity);
        }

        public void Remove(NumberFormat entity)
        {
            context.NumberFormats.Attach(entity);
            context.NumberFormats.Remove(entity);
        }

        public void Update(NumberFormat entity)
        {
            context.NumberFormats.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NumberFormat> All()
        {
            return context.NumberFormats.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        List<NumberFormat> IRepository<NumberFormat>.GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        NumberFormat IRepository<NumberFormat>.GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
