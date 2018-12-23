using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class OBLTypeQuery
    {
        OBLTypeRepository repository;
        public OBLTypeQuery(int tenant)
        {
            repository = new OBLTypeRepository(tenant);
        }
        public OBLTypeQuery(OBLTypeRepository repository)
        {
            this.repository = repository;
        }

        public OBLTypePM GetSinglePM(string code)
        {
            OBLTypePM myResult = null;

            if (code != null)
            {
                myResult = (from d in repository.context.OBLTypes
                            where d.Code == code
                            select new OBLTypePM
                            {
                                Code = d.Code,
                                Name = d.Name,
                                SearchFields = d.SearchFields,
                            }).FirstOrDefault();

            }

            return myResult;
        }

        public OBLTypeList GetSingleOBLTypeList(OBLType entity)
        {
            OBLTypeList myResult = null;

            if (entity != null)
            {
                myResult = new OBLTypeList()
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    SearchFields = entity.SearchFields,
                };
            }

            return myResult;
        }

        public IQueryable<OBLTypeList> GetIQueryableEntityList(IQueryable<OBLType> entities)
        {
            var myResult = (from entity in entities
                            select new OBLTypeList()
                            {
                                Code = entity.Code,
                                Name = entity.Name,
                                SearchFields = entity.SearchFields,
                            });

            return myResult;
        }

    }
}
