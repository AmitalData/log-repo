using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AWBDescriptionOfGoodsQuery
    {
        AWBDescriptionOfGoodsRepository repository;

        public AWBDescriptionOfGoodsQuery()
        {
            this.repository = new AWBDescriptionOfGoodsRepository(); 
        }

        public AWBDescriptionOfGoodsQuery(int tenant)
        {
            this.repository = new AWBDescriptionOfGoodsRepository(tenant);
        }

        public AWBDescriptionOfGoodsQuery(AWBDescriptionOfGoodsRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<AWBDescriptionOfGoodsList> GetIQueryableEntityList(IQueryable<AWBDescriptionOfGoods> iQueryable)
        {
            IQueryable<AWBDescriptionOfGoodsList> result = from entity in iQueryable
                                                           select new AWBDescriptionOfGoodsList()
                                                             {
                                                                 Id = entity.Id,
                                                                 Name = entity.Name,
                                                                 ShortDescriptionOfGoods = entity.ShortDescriptionOfGoods,
                                                                 SearchFields = entity.SearchFields,
                                                                 AirlineCode = entity.AirlineCode,
                                                                 ProductCode = entity.ProductCode,
                                                                 Service = entity.Service,
                                                                 IsTemperatureSensitive = entity.IsTemperatureSensitive,
                                                             };
            return result;
        }
    }
}
