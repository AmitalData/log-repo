using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CustomsInterfaceQuery
    {
        CustomsInterfaceRepository repository;

        public CustomsInterfaceQuery()
        {
            repository = new CustomsInterfaceRepository();
        }

        public CustomsInterfaceQuery(int tenant)
        {
            repository = new CustomsInterfaceRepository(tenant);
        }

        public CustomsInterfaceQuery(CustomsInterfaceRepository CustomsInterfaceRepository)
        {
            repository = CustomsInterfaceRepository;
        }
        
        public IQueryable<CustomsInterfaceList> GetIQueryableEntityList(IQueryable<CustomsInterface> iQueryable)
        {
            var result = from entity in iQueryable where !entity.InActive
                         select new CustomsInterfaceList()
                         {
                             Code = entity.Code,
                             Name = entity.Name,
                             SearchFields = entity.SearchFields,                             
                             InActive = entity.InActive,
                             InterfaceType = entity.InterfaceType,
                         };

            return result;
        }

       
    }
}
