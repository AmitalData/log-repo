using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class PortTimeZoneQuery
    {
        PortTimeZoneRepository repository;
        public PortTimeZoneQuery()
        {
            repository = new PortTimeZoneRepository();
        }
        public PortTimeZoneQuery(int tenant)
        {
            repository = new PortTimeZoneRepository(tenant);
        }
        public PortTimeZoneQuery(PortTimeZoneRepository PortTimeZoneRepository)
        {
            repository = PortTimeZoneRepository;
        }

        public PortTimeZonePM GetSinglePM(string code, int tenant)
        {
            PortTimeZonePM entity = (from a in repository.context.PortTimeZones
                                     where a.Code == code
                                     select new PortTimeZonePM()
                                     {
                                         Name = a.Name,
                                         Inactive = a.Inactive,
                                         Notes = a.Notes,
                                         SearchFields = a.SearchFields,                                         
                                         Code = a.Code,
                                         UTCOffset = a.UTCOffset,
                                         UTCDSTOffset = a.UTCDSTOffset,
                                     }).FirstOrDefault();


            return entity;
        }
        
        public IQueryable<PortTimeZoneList> GetIQueryableEntityList(IQueryable<PortTimeZone> iQueryable)
        {
            IQueryable<PortTimeZoneList> result = from a in iQueryable
                                            select new PortTimeZoneList()
                                            {
                                                Name = a.Name,
                                                Inactive = a.Inactive,
                                                Notes = a.Notes,
                                                SearchFields = a.SearchFields,
                                                Code = a.Code,
                                                UTCOffset = a.UTCOffset,
                                                UTCDSTOffset = a.UTCDSTOffset,
                                            };
            return result;
        }
    }
}
