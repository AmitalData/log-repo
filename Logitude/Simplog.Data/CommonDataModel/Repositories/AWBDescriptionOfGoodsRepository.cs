using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AWBDescriptionOfGoodsRepository : IRepository<AWBDescriptionOfGoods>
    {
        ICommonDataContext commonDataContext;

        public AWBDescriptionOfGoodsRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AWBDescriptionOfGoodsRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AWBDescriptionOfGoodsRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<AWBDescriptionOfGoods> GetAWBDescriptionOfGoods()
        {
            return (from record in context.AWBDescriptionOfGoods select record);
        }

        public AWBDescriptionOfGoods GetSingleAWBDescriptionOfGoods(string id)
        {
            return (from record in context.AWBDescriptionOfGoods where record.Id == id select record).FirstOrDefault();
        }

        public void Add(AWBDescriptionOfGoods entity)
        {
            this.context.AWBDescriptionOfGoods.Add(entity);
        }

        public void Remove(AWBDescriptionOfGoods entity)
        {
            try
            {
                this.context.AWBDescriptionOfGoods.Attach(entity);
            }
            catch { }
            this.context.AWBDescriptionOfGoods.Remove(entity);

        }

        public void Update(AWBDescriptionOfGoods entity)
        {
            try
            {
                this.context.AWBDescriptionOfGoods.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);

        }

        public List<AWBDescriptionOfGoods> All()
        {
            return this.context.AWBDescriptionOfGoods.ToList<AWBDescriptionOfGoods>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<AWBDescriptionOfGoods> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBDescriptionOfGoods GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AWBDescriptionOfGoods> GetAWBDescriptionOfGoodsByAirlineCode(string airlineCode)
        {
            return (from record in context.AWBDescriptionOfGoods where record.AirlineCode == airlineCode select record);
        }
    }
}
