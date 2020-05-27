using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class CustomsTransferTypeQuery
    {
        CustomsTransferTypeRepository repository;
        public CustomsTransferTypeQuery(int tenant)
        {
            repository = new CustomsTransferTypeRepository(tenant);
        }
        public CustomsTransferTypeQuery(CustomsTransferTypeRepository repository)
        {
            this.repository = repository;
        }

        public CustomsTransferTypeList GetSingleCustomsTransferTypeList(CustomsTransferType entity)
        {
            CustomsTransferTypeList myResult = null;

            if (entity != null)
            {
                myResult = new CustomsTransferTypeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<CustomsTransferTypeList> GetIQueryableEntityList(IQueryable<CustomsTransferType> iQueryable)
        {
            IQueryable<CustomsTransferTypeList> result = from entity in iQueryable
                                                              select new CustomsTransferTypeList()
                                                              {
                                                                  Name = entity.Name,
                                                                  Code = entity.Code,
                                                                  SearchFields = entity.SearchFields,
                                                              };
            return result;
        }
    }
}
