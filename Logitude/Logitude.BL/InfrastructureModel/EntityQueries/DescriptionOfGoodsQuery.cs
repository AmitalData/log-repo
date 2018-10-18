using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityLists;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DescriptionOfGoodsQuery
    {
        DescriptionOfGoodsRepository repository;
        public DescriptionOfGoodsQuery()
        {
            repository = new DescriptionOfGoodsRepository(); 
        }

        public DescriptionOfGoodsQuery(int tenant)
        {
            repository = new DescriptionOfGoodsRepository(tenant);
        }

        public DescriptionOfGoodsQuery(DescriptionOfGoodsRepository descriptionOfGoodsRepository)
        {
            repository = descriptionOfGoodsRepository;
        }

        public DescriptionOfGoodsPM GetSingleDescriptionOfGoodsPM(string id)
        {
            return (from a in repository.context.DescriptionOfGoods
                    where a.Id == id
                    select new DescriptionOfGoodsPM()
                    {
                        AddedManually = a.AddedManually,
                        DescriptionOfGood = a.DescriptionOfGood,
                        Id = a.Id,
                        InActive = a.InActive,
                        Name = a.Name,
                        Tenant = a.Tenant,
                    }).FirstOrDefault();
        }

        public DescriptionOfGoodsPM GetSinglePM(string id,int tenant)
        {
            return (from a in repository.context.DescriptionOfGoods
                    where a.Id == id
                    select new DescriptionOfGoodsPM()
                    {
                        AddedManually = a.AddedManually,
                        DescriptionOfGood = a.DescriptionOfGood,
                        Id = a.Id,
                        InActive = a.InActive,
                        Name = a.Name,
                        Tenant = a.Tenant,
                    }).FirstOrDefault();
        }

        public IQueryable<DescriptionOfGoodsList> GetIQueryableEntityList(IQueryable<DescriptionOfGoods> iQueryable)
        {
            IQueryable<DescriptionOfGoodsList> result = from entity in iQueryable
                                                        select new DescriptionOfGoodsList()
                                                        {
                                                            AddedManually = entity.AddedManually,
                                                            Name = entity.Name,
                                                            Id = entity.Id,
                                                            InActive = entity.InActive,
                                                            DescriptionOfGood = entity.DescriptionOfGood,
                                                            Tenant = entity.Tenant,
                                                        };
            return result;
        }
    }
}