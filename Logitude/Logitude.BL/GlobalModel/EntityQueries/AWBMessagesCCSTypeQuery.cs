using Logitude.BL.GlobalModel.EntityLists;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class AWBMessagesCCSTypeQuery
    {
        AWBMessagesCCSTypeRepository repository;

        public AWBMessagesCCSTypeQuery()
        {
            this.repository = new AWBMessagesCCSTypeRepository();
        }

        public AWBMessagesCCSTypeQuery(int tenant)
        {
            this.repository = new AWBMessagesCCSTypeRepository(tenant);
        }

        public AWBMessagesCCSTypeQuery(AWBMessagesCCSTypeRepository myrepository)
        {
            this.repository = myrepository;
        }

        public IQueryable<AWBMessagesCCSTypeList> GetIQueryableEntityList(IQueryable<AWBMessagesCCSType> iQueryable)
        {
            IQueryable<AWBMessagesCCSTypeList> result = from a in iQueryable
                                                        select new AWBMessagesCCSTypeList()
                                                        {
                                                            Code = a.Code,
                                                            Name = a.Name,
                                                            SearchFields = a.SearchFields,
                                                        };
            return result;
        }
    }
}
