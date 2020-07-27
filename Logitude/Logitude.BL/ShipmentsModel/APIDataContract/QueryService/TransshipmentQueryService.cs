using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.APIDataContract.ApiV1
{
    public class TransshipmentQueryService
    {
        public TransshipmentQueryService(int tenant)
        {
            
        }

        public List<Transshipment> TransshipmentCustomDataMapping(List<TransshipmentLeg> MyEntityPMs, int Tenant)
        {
            try
            {

                var MyList = new List<Transshipment>();

                return MyList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<TransshipmentLeg> TransshipmentDataMappingAndValidatin(List<Transshipment> MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var MyList = new List<TransshipmentLeg>();
                
                return MyList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
