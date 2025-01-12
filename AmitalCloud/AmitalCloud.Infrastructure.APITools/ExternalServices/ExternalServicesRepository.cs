using System;
using System.Collections.Generic;

namespace AmitalCloud.Infrastructure.APITools.ExternalServices
{
    public class ExternalServicesRepository
    {
        public List<ExternalServicePM> GetAll(int tenant, ServiceTypeEnum servicetype)
        {
            throw new NotImplementedException();
            //todo  vladi -  implement Customs Settings as generic settings
            //var repository = new CustomsSettingRepository(tenant);
            //var l=repository.GetAll(tenant).ToList().Select(rec => GetMe(rec, servicetype)).ToList();
            //return l;
        }

        //private ExternalServicePM GetMe(EntityPOCOs.CustomsSetting rec, ServiceTypeEnum servicetype)
        //{
        //    switch (servicetype)
        //    {
        //        case ServiceTypeEnum.DCA:
        //            return new ExternalServicePM() { TimeoutInSec = 360, Id = rec.Id, Tenant = rec.Tenant, ServiceAddressUrl = rec.DCAServiceAddress, ServiceType = servicetype };
        //            break;
        //        case ServiceTypeEnum.Sign:

        //            return new ExternalServicePM() { TimeoutInSec = 360, Id = rec.Id, Tenant = rec.Tenant, ServiceAddressUrl = rec.SignServiceAddress, ServiceType = servicetype };
        //            break;
        //        default:
        //            return null;
        //            break;
        //    }
        //}
    }
}
