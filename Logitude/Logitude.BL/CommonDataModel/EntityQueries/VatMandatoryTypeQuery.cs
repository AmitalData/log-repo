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
    public class VatMandatoryTypeQuery
    {
        VatMandatoryTypeRepository repository;

        public VatMandatoryTypeQuery()
        {
            repository = new VatMandatoryTypeRepository();
        }
        public VatMandatoryTypeQuery(int tenant)
        {
            repository = new VatMandatoryTypeRepository(tenant);
        }
        public VatMandatoryTypeQuery(VatMandatoryTypeRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<VatMandatoryTypeList> GetIQueryableEntityList(IQueryable<VatMandatoryType> iQueryable)
        {

            IQueryable<VatMandatoryTypeList> result = from f in iQueryable
                                                      select new VatMandatoryTypeList()
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