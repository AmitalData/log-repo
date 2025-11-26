using Simplog.Data.ShipmentsModel.Repositories;


namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class OceanCarrierStatusAPIconfigQuery
    {

        OceanCarrierStatusAPIconfigRepository repository;

        public OceanCarrierStatusAPIconfigQuery(int tenant)
        {
            repository = new OceanCarrierStatusAPIconfigRepository(tenant);
        }

        public OceanCarrierStatusAPIconfigQuery(OceanCarrierStatusAPIconfigRepository repository)
        {
            this.repository = repository;
        }

        public bool HasActiveConfig(string scacCode, int tenant)
        {
            return repository.HasActiveConfig(scacCode, tenant);
        }

    }
}
