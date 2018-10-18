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
    public class INTTRADocumentTypeQuery
    {
        INTTRADocumentTypeRepository repository;
        public INTTRADocumentTypeQuery(int tenant)
        {
            repository = new INTTRADocumentTypeRepository(tenant);
        }
        public INTTRADocumentTypeQuery(INTTRADocumentTypeRepository repository)
        {
            this.repository = repository;
        }

        public INTTRADocumentTypeList GetSingleINTTRADocumentTypeList(INTTRADocumentType entity)
        {
            INTTRADocumentTypeList myResult = null;

            if (entity != null)
            {
                myResult = new INTTRADocumentTypeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<INTTRADocumentTypeList> GetIQueryableEntityList(IQueryable<INTTRADocumentType> iQueryable)
        {
            IQueryable<INTTRADocumentTypeList> result = from entity in iQueryable
                                                     select new INTTRADocumentTypeList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}
