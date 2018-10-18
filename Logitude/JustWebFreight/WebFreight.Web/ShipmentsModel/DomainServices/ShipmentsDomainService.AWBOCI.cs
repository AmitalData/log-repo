using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        //private AWBOCIQuery aWBOCIQuery;
        //private AWBOCIRepository aWBOCIRepository;

        //public IQueryable<AWBOCIPM> GetAWBOCIPMsByShipmentId(string shipmentId, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    aWBOCIQuery = new AWBOCIQuery(tenant);
        //    return aWBOCIQuery.GetAWBOCIPMsByShipmentId(shipmentId, tenant);
        //}

        //public AWBOCIList GetSingleAWBOCIList(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);

        //    aWBOCIQuery = new AWBOCIQuery(tenant);
        //    AWBOCIPM entity = aWBOCIQuery.GetSinglePM(id, tenant);
        //    AWBOCIList entityList = new AWBOCIList()
        //    {
        //        Id = entity.Id,
        //        Tenant = entity.Tenant,
        //        ShipmentId = entity.ShipmentId,
        //        CountryId = entity.CountryId,
        //        AWBInformationCode = entity.AWBInformationCode,
        //        SupplementaryCustomsInfo = entity.SupplementaryCustomsInfo,
        //        AWBCustomsInformationCode = entity.AWBCustomsInformationCode,
        //    };

        //    return entityList;
        //}

    }
}