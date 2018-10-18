using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class AWBOCIQuery
    {
        AWBOCIRepository repository;

        public AWBOCIQuery(int tenant)
        {
            repository = new AWBOCIRepository(tenant);
        }

        public AWBOCIQuery(AWBOCIRepository repository)
        {
            this.repository = repository;
        }

        public AWBOCIPM GetSinglePM(string id,int tenant)
        {
            return (from a in repository.Context.AWBOCIs
                    where a.Id == id && a.Tenant == tenant
                    select new AWBOCIPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ShipmentId = a.ShipmentId,
                        CountryId = a.CountryId,
                        AWBInformationCode = a.AWBInformationCode,
                        SupplementaryCustomsInfo = a.SupplementaryCustomsInfo,
                        AWBCustomsInformationCode = a.AWBCustomsInformationCode,
                    }).FirstOrDefault();
        }

        public IQueryable<AWBOCIPM> GetAWBOCIPMsByShipmentId(string shipmentId, int tenant)
        {
            return (from a in repository.Context.AWBOCIs
                    where a.ShipmentId == shipmentId && a.Tenant == tenant
                    select new AWBOCIPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        ShipmentId = a.ShipmentId,
                        CountryId = a.CountryId,
                        AWBInformationCode = a.AWBInformationCode,
                        SupplementaryCustomsInfo = a.SupplementaryCustomsInfo,
                        AWBCustomsInformationCode = a.AWBCustomsInformationCode,
                    });
        }

        public IQueryable<AWBOCIList> GetIQueryableEntityList(IQueryable<AWBOCIList> iQueryable)
        {
            IQueryable<AWBOCIList> result =
                from a in iQueryable
                select new AWBOCIList()
                               {
                                   Id = a.Id,
                                   Tenant = a.Tenant,
                                   ShipmentId = a.ShipmentId,
                                   CountryId = a.CountryId,
                                   AWBInformationCode = a.AWBInformationCode,
                                   SupplementaryCustomsInfo = a.SupplementaryCustomsInfo,
                                   AWBCustomsInformationCode = a.AWBCustomsInformationCode,
                               };
            return result;
        }

    }
}