using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Linq;

namespace Logitude.BL
{
    public class ShipmentDigitalFieldQuery
    {
        ShipmentDigitalFieldRepository repository;

        public ShipmentDigitalFieldQuery(int tenant)
        {
            repository = new ShipmentDigitalFieldRepository(tenant);
        }

        public ShipmentDigitalFieldQuery(ShipmentDigitalFieldRepository repository)
        {
            this.repository = repository;
        }
        public ShipmentDigitalFieldPM GetSinglePM(string id)
        {
            return (from a in repository.context.ShipmentDigitalFields
                    where a.Id == id
                    select new ShipmentDigitalFieldPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        IsCustomerArchived = a.IsCustomerArchived,
                    }).FirstOrDefault();
        }

        public IQueryable<ShipmentDigitalFieldList> GetIQueryableEntityList(IQueryable<ShipmentDigitalField> iQueryable)
        {
            IQueryable<ShipmentDigitalFieldList> result = (from a in iQueryable
                                                           select new ShipmentDigitalFieldList()
                                                           {
                                                               Id = a.Id,
                                                               Tenant = a.Tenant,
                                                               IsCustomerArchived = a.IsCustomerArchived,

                                                           });
            return result;
        }

    }
}
