using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class WarehouseTypeQuery
    {
        WarehouseTypeRepository repository;

        public WarehouseTypeQuery()
        {
            repository = new WarehouseTypeRepository();
        }

        public WarehouseTypeQuery(int tenant)
        {
            repository = new WarehouseTypeRepository(tenant);
        }

        public WarehouseTypeQuery(WarehouseTypeRepository communicationLogTypeRepository)
        {
            repository = communicationLogTypeRepository;
        }
        public WarehouseTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.WarehouseTypes
                    where a.Code == code
                    select new WarehouseTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        public WarehouseTypePM GetSingleWarehouseTypePM(string code)
        {
            return (from a in repository.context.WarehouseTypes
                    where a.Code == code
                    select new WarehouseTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<WarehouseTypeList> GetIQueryableEntityList(IQueryable<WarehouseType> iQueryable)
        {
            IQueryable<WarehouseTypeList> result = from entity in iQueryable
                                                          select new WarehouseTypeList()
                                                          {
                                                              Name = entity.Name,
                                                              Code = entity.Code,
                                                              SearchFields = entity.SearchFields,
                                                          };
            return result;
        }
    }
}
