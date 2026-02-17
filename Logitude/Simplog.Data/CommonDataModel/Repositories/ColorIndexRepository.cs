using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ColorIndexRepository : IRepository<ColorIndex>
    {
        ICommonDataContext commonDataContext;

        public ColorIndexRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ColorIndexRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ColorIndexRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<ColorIndex> GetColorIndexs()
        {
            return context.ColorIndexs;
        }

        public ColorIndex GetSingleColorIndex(int index)
        {
            return (from record in context.ColorIndexs where record.IndexNumber == index select record).FirstOrDefault();
        }

        public string GetSingleHasColor(int? index)
        {
            return (from record in context.ColorIndexs where record.IndexNumber == index select record.Color).FirstOrDefault();
        }


        public void Add(ColorIndex entity)
        {
            context.ColorIndexs.Add(entity);
        }

        public void Remove(ColorIndex entity)
        {
            context.ColorIndexs.Attach(entity);
            context.ColorIndexs.Remove(entity);
        }

        public void Update(ColorIndex entity)
        {
            context.ColorIndexs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ColorIndex> All()
        {
            return context.ColorIndexs.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ColorIndex> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ColorIndex GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
