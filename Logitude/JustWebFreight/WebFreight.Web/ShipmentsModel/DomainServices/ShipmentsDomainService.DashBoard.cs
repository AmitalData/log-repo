using System.Collections.Generic;
using System.Linq;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Data.QuoteModel.Repositories;
using WebFreight.Web.DataContracts;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Logitude.BL.CommonDataModel.BusinessUnitFilters;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        public IQueryable<DashBoardClass> GetTop10DashBoard(int lastMonths,int lastDays, int measurment, int currentTenant, int top, bool includeOthers,string directionId,string TransportmodeId)
        {
            shipmentQuery = new ShipmentQuery(currentTenant);
            SecurityUtility.AuthenticationOnTenant(currentTenant);
            return shipmentQuery.GetTop10DashBoard(null,lastMonths,lastDays, measurment, currentTenant, top, includeOthers, directionId, TransportmodeId);
        }

        public List<DashBoardClass> GetShipmentsByTop10CountriesDashBoard(int lastMonths,int lastDays, int measurment, int currentTenant, int top, bool includeOthers, string customerid,string directionId,string transmodeId)
        {
            shipmentQuery = new ShipmentQuery(currentTenant);
            SecurityUtility.AuthenticationOnTenant(currentTenant);
            return shipmentQuery.GetShipmentsByTop10CountriesDashBoard(null,lastMonths, lastDays, measurment, currentTenant, top, includeOthers, customerid, directionId, transmodeId);
        }

        public List<DashBoardClass> GetShipmentsByMonthDashBoard(int lastMonths,int lastDays, int currentTenant, string customerid)
        {
            shipmentQuery = new ShipmentQuery(currentTenant);
            SecurityUtility.AuthenticationOnTenant(currentTenant);
            return shipmentQuery.GetShipmentsByMonthDashBoard(null,lastMonths, lastDays, currentTenant, customerid);
        }

        public IQueryable<DashBoardClass> GetShipmentsByCountryDashBoard(int last, int currentTenant)
        {
            shipmentQuery = new ShipmentQuery(currentTenant);
            SecurityUtility.AuthenticationOnTenant(currentTenant);
            return shipmentQuery.GetShipmentsByCountryDashBoard(last, currentTenant);
        }

        public IQueryable<DashBoardClass> GetShipmentsByActivitesDashBoard(int last, int currentTenant)
        {
            shipmentQuery = new ShipmentQuery(currentTenant);
            SecurityUtility.AuthenticationOnTenant(currentTenant);
            return shipmentQuery.GetShipmentsByActivitesDashBoard(last, currentTenant);
        }

        public IQueryable<DashBoardClass> GetShipmentByDirectionAndTransmode(int lastMonths,int lastDays, int tenant, string customerid)
        {
            shipmentQuery = new ShipmentQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentQuery.GetShipmentsByDirectionAndTransMode(null,lastMonths, lastDays, tenant, customerid);
        }

        public IQueryable<DashBoardClass> GetShipmentByDirectionForCustomer(int last, int tenant, string customerid)
        {
            shipmentQuery = new ShipmentQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentQuery.GetShipmentsByDirectionForCustomer(last, tenant, customerid);
        }

        public IQueryable<DashBoardClass> GetShipmentByTransmodeForCustomer(int last, int tenant, string customerid)
        {
            shipmentQuery = new ShipmentQuery(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            return shipmentQuery.GetShipmentsByTransModeForCustomer(last, tenant, customerid);
        }
    }
}