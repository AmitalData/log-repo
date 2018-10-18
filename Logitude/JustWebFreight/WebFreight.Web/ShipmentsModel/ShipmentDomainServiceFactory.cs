using System;
using System.ServiceModel.DomainServices.Server;

using Simplog.Data.ShipmentsModel;

using WebFreight.Web.ShipmentsModel.DomainServices;

namespace WebFreight.Web.ShipmentsModel
{
    public class ShipmentDomainServiceFactory : IDomainServiceFactory
    {
        public DomainService CreateDomainService(Type domainServiceType, DomainServiceContext context)
        {
            DomainService domainService;
            if (typeof(ShipmentsDomainService) == domainServiceType)
            {
                ShipmentsContext shipmentContext = new ShipmentsContext();
                domainService = new ShipmentsDomainService(
                    shipmentContext);
            }
            else
            {
                domainService = (DomainService)Activator.CreateInstance(domainServiceType);
            }

            domainService.Initialize(context);
            return domainService;
        }

        public void ReleaseDomainService(DomainService domainService)
        {
            domainService.Dispose();
        }
    }
}