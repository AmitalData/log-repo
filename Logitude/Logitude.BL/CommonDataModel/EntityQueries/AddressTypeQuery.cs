using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AddressTypeQuery
    {
        AddressTypeRepository repository;

        public AddressTypeQuery(int tenant)
        {
            repository = new AddressTypeRepository(tenant);
        }

        public AddressTypeQuery(AddressTypeRepository myRepository)
        {
            repository = myRepository;
        }

        public IQueryable<AddressTypeList> GetIQueryableEntityList(IQueryable<AddressType> iQueryable)
        {
            IQueryable<AddressTypeList> result = from entityPoco in iQueryable
                                                        select new AddressTypeList()
                                                        {
                                                            Id = entityPoco.Id,
                                                            Name = entityPoco.Name,
                                                            SearchFields = entityPoco.SearchFields,
                                                        };

            return result;
        }

        public AddressTypePM GetSinglePM(string Id)
        {
            AddressTypePM entityPM = (from a in repository.context.AddressTypes
                                     where a.Id == Id
                                     select new AddressTypePM()
                                     {
                                         Id = a.Id,
                                         Name = a.Name
                                     }).FirstOrDefault();
            return entityPM;
        }
        public AddressTypePM GetSinglePM(string Id,int tenant)
        {
            AddressTypePM entityPM = (from a in repository.context.AddressTypes
                                      where a.Id == Id
                                      select new AddressTypePM()
                                      {
                                          Id = a.Id,
                                          Name = a.Name
                                      }).FirstOrDefault();
            return entityPM;
        }
    }
}
