 
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

namespace Logitude.BL.ShipmentsModel.EntityQueries
{ 
   public class ReferenceTypeQueryService
   {
        ReferenceTypeRepository repository;

        public ReferenceTypeQueryService(int tenant)
        {
            repository = new ReferenceTypeRepository(tenant);
        }

        public ReferenceTypeQueryService(ReferenceTypeRepository repository)
        {
            this.repository = repository;
        }

        public ReferenceTypePM GetSingle(string code)
        {
            return (from a in repository.context.ReferenceTypes
                    where a.Code == code
                    select new ReferenceTypePM() { Code = a.Code, EnglishName = a.EnglishName, SearchFields = a.SearchFields }).FirstOrDefault();
        }

        public IQueryable<ContainerStatusList> GetIQueryableEntityList(IQueryable<ContainerStatusList> iQueryable)
        {
            IQueryable<ContainerStatusList> result = from entity in iQueryable
                                                     select new ContainerStatusList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
   
}
	 