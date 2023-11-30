using Logitude.CommonDataTests.Models;
using Logitude.CommonDataTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CommonDataTests.Services
{
    public class VesselDataPreparation
    {

        public void Prepar()
        {
            try
            {
                ApiResponse<VesselPM> response = APICaller.CallPost<VesselPM>(GetValidVesselPM(), Urls.VesselsController, UserTenant.Token);
                VesselDataMap(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating vessel Before Feature Run :" + e.InnerException);
            }
        }

        private VesselPM GetValidVesselPM()
        {
            return new VesselBuilder()
                 .WithDefualtValues()
                 .Tenant(UserTenant.Tenant)
                 .EnglishName("specflow name")
                 .IMOCode("SPcode")
                 .LocalName("specflow local name")
                 .Notes("specflow note")
                 .Build();
        }

        private void VesselDataMap(VesselPM vessel)
        {
            CommonData.VesselId = vessel.Id;
        }


    }
}
