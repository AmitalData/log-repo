using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;


namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class VatFormatTypeQuery
    {
       VatFormatTypeRepository repository;

        public VatFormatTypeQuery()
        {
            repository = new VatFormatTypeRepository();
        }
        public VatFormatTypeQuery(int tenant)
        {
            repository = new VatFormatTypeRepository(tenant);
        }
        public VatFormatTypeQuery(VatFormatTypeRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<VatFormatTypeList> GetIQueryableEntityList(IQueryable<VatFormatType> iQueryable)
        {
            IQueryable<VatFormatTypeList> result = from f in iQueryable
                                                      select new VatFormatTypeList()
                                                      {
                                                          Code = f.Code,
                                                          Name = f.Name,
                                                          ViewOrder = f.ViewOrder,
                                                          SearchFields = f.SearchFields,

                                                      };
            return result;
        }
    }
}