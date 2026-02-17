using System.Linq;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class PickUpDeliveryFromToTypeQuery
    {
        PickUpDeliveryFromToTypeRepository repository;
         
        public PickUpDeliveryFromToTypeQuery(int tenant)
        {
            repository = new PickUpDeliveryFromToTypeRepository(tenant);
        }

        public PickUpDeliveryFromToTypeQuery(PickUpDeliveryFromToTypeRepository repository)
        {
            this.repository = repository;
        }

        public PickUpDeliveryFromToTypePM GetSinglePickUpDeliveryFromToTypePM(string code)
        {
            return (from a in repository.context.PickUpDeliveryFromToTypes
                    where a.Code == code
                    select new PickUpDeliveryFromToTypePM() { Code = a.Code, Name = a.Name }).FirstOrDefault();
        }
    }
}