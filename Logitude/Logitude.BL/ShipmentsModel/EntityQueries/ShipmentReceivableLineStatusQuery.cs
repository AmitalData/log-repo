using System.Linq;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class ShipmentReceivableLineStatusQuery
    {
        ShipmentReceivableLineStatusRepository repository;
         
        public ShipmentReceivableLineStatusQuery(int tenant)
        {
            repository = new ShipmentReceivableLineStatusRepository(tenant);
        }

        public ShipmentReceivableLineStatusQuery(ShipmentReceivableLineStatusRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<ShipmentReceivableLineStatusPM> GetShipmentReceivableLineStatusPMs()
        {
            return from a in repository.context.ShipmentReceivableLineStatus
                   select new ShipmentReceivableLineStatusPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, };
        }
    }
}