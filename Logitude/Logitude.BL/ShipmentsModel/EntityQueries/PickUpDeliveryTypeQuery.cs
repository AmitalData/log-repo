using System.Linq;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class PickUpDeliveryTypeQuery
    {
        PickUpDeliveryTypeRepository repository;
         
        public PickUpDeliveryTypeQuery(int tenant)
        {
            repository = new PickUpDeliveryTypeRepository(tenant);
        }

        public PickUpDeliveryTypeQuery(PickUpDeliveryTypeRepository repository)
        {
            this.repository = repository;
        }

        public PickUpDeliveryTypePM GetSinglePickUpDeliveryTypePM(string code)
        {
            return (from a in repository.context.PickUpDeliveryTypes
                    where a.Code == code
                    select new PickUpDeliveryTypePM() { Code = a.Code, Name = a.Name }).FirstOrDefault();
        }
    }
}