using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentAssemblyQuery
    {
        ShipmentAssemblyRepository repository;

        public ShipmentAssemblyQuery(int tenant)
        {
            repository = new ShipmentAssemblyRepository(tenant);
        }

        public ShipmentAssemblyQuery(ShipmentAssemblyRepository repository)
        {
            this.repository = repository;
        }

        public ShipmentAssemblyPM GetSinglePM(string shipmentId, int tenant)
        {
            ShipmentAssemblyPM myResult
                = (from a in repository.context.ShipmentAssemblies.Include("Shipper").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentAssemblyPM()
                   {                       
                       Id = a.Id,                       
                       Tenant = a.Tenant,                       
                       ShipmentId = a.ShipmentId,
                       ShipperId = a.ShipperId,
                       ShipperName = a.Shipper == null ? null : a.Shipper.EnglishName,
                       House = a.House,
                       CreateDate = a.CreateDate,
                       UpdateDate = a.UpdateDate,
                       CreatedByUserId = a.CreatedByUserId,
                       CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                       UpdatedByUserId = a.UpdatedByUserId,
                       UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                   }).FirstOrDefault();
            
            return myResult;
        }

        public List<ShipmentAssemblyPM> GetShipmentAssemblies(string shipmentId, int tenant)
        {
            List<ShipmentAssemblyPM> shipmentAssembleies
                = (from a in repository.context.ShipmentAssemblies.Include("Shipper").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                   select new ShipmentAssemblyPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ShipmentId = a.ShipmentId,
                       ShipperId = a.ShipperId,
                       ShipperName = a.Shipper == null ? null : a.Shipper.EnglishName,
                       House = a.House,
                       CreateDate = a.CreateDate,
                       UpdateDate = a.UpdateDate,
                       CreatedByUserId = a.CreatedByUserId,
                       CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                       UpdatedByUserId = a.UpdatedByUserId,
                       UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                   }).ToList();

            return shipmentAssembleies;
        }
    }
}
