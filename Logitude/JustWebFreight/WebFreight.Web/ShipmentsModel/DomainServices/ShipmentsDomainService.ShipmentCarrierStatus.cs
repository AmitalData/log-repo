using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using System.ServiceModel.DomainServices.Server;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Reflection;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.ShipmentsModel;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        //private ShipmentCarrierStatusRepository shipmentCarrierStatusRepository;

        public List<ShipmentCarrierStatusList> GetShipmentCarrierStatusLists(string shipmentId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ShipmentCarrierStatusRepository entityRepository = new ShipmentCarrierStatusRepository(tenant);
            ShipmentCarrierStatusQuery entityQuery = new ShipmentCarrierStatusQuery(entityRepository);

            List<ShipmentCarrierStatusList> myResult = entityQuery.GetShipmentCarrierStatusLists(shipmentId, tenant).ToList();

            return myResult;
        }
    
    }
}