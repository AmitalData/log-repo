 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{ 
   public class ResponsibilityQuery
   {
        ResponsibilityRepository repository;

        public ResponsibilityQuery(int tenant)
        {
            repository = new ResponsibilityRepository(tenant);
        }

        public ResponsibilityQuery(ResponsibilityRepository repository)
        {
            this.repository = repository;
        }

        public ResponsibilityList GetSingle(string code)
        {
            return (from a in repository.context.Responsibilities
                    where a.Code == code
                    select new ResponsibilityList() { Code = a.Code, EnglishName = a.EnglishName, SearchFields = a.SearchFields }).FirstOrDefault();
        }

        public IQueryable<ResponsibilityList> GetIQueryableEntityList(IQueryable<Responsibility> iQueryable)
        {
            IQueryable<ResponsibilityList> result = from entity in iQueryable
                                                     select new ResponsibilityList()
                                                     {
                                                         EnglishName = entity.EnglishName,
                                                         LocalName = entity.LocalName,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
   
}
	 