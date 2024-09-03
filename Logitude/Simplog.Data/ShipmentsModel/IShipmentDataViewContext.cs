using System.Data.Entity;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel
{
    public interface IShipmentDataViewContext : IContext
    {
        IDbSet<ShipmentDataView> ShipmentDataViews { get; }
        IDbSet<DigitalShipmentsDataView> DigitalShipmentsDataView { get; }
        IDbSet<ShipmentCountryDashboardView> ShipmentCountryDashboardViews { get; }
        IDbSet<ShipmentDirectionTransmodeView> ShipmentDirectionTransmodeViews { get; }
        IDbSet<ShipmentsCustomersDashboardView> ShipmentsCustomersDashboardViews { get; }
        IDbSet<CustomsShipmentDataView> CustomsShipmentDataView { get; }

        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}