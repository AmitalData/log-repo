using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class VolumeUnitQuery
    {
        VolumeUnitRepository repository;
        public VolumeUnitQuery()
        {
            repository = new VolumeUnitRepository(); 
        }

        public VolumeUnitQuery(int tenant)
        {
            repository = new VolumeUnitRepository(tenant);
        }

        public VolumeUnitQuery(VolumeUnitRepository volumeUnitRepository)
        {
            repository = volumeUnitRepository;
        }

        public VolumeUnitPM GetSingleVolumeUnitPM(string code)
        {
            return (from a in repository.context.VolumeUnits
                    where a.Code == code
                    select new VolumeUnitPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        PrintAs = a.PrintAs,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();

        }

        public VolumeUnitPM GetSinglePM(string code, int tenant= 0)
        {
            return (from a in repository.context.VolumeUnits
                    where a.Code == code
                    select new VolumeUnitPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        PrintAs = a.PrintAs,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();

        }


        public IQueryable<VolumeUnitPM> GetVolumeUnitPMs()
        {
            return from a in repository.context.VolumeUnits
                   select new VolumeUnitPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       PrintAs = a.PrintAs,
                       SearchFields = a.SearchFields,
                   };
        }


        public IQueryable<VolumeUnitList> GetIQueryableEntityList(IQueryable<VolumeUnit> iQueryable)
        {
            IQueryable<VolumeUnitList> result = from entity in iQueryable
                                                select new VolumeUnitList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    PrintAs = a.PrintAs,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }
    }
}