using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.Repsitories
{
    public class ExternalServicesRepository
    {
        //private static readonly List<ExternalServicePM> _DBSet;
        //static ExternalServicesRepository()
        //{
        //    _DBSet = new List<ExternalServicePM>();
        //    _DBSet.Add(
        //        new ExternalServicePM() { TimeoutInSec = 360, Id = "1-1", Tenant = 1, ServiceAddressUrl = @"http://itzik7:5058/Unifreight/SignService/basic", ServiceType = ServiceTypeEnum.Sign }
        //        );
        //    _DBSet.Add(
        //        new ExternalServicePM() { TimeoutInSec = 360, Id = "1-2", Tenant = 1, ServiceAddressUrl = @"http://Tomer:5058/Unifreight/SignService/basic", ServiceType = ServiceTypeEnum.Sign }
        //        );


        //    _DBSet.Add(
        //        new ExternalServicePM() { TimeoutInSec = 360, Id = "1-3", Tenant = 2, ServiceAddressUrl = @"http://itzik7:5058/Unifreight/SignService/basic", ServiceType = ServiceTypeEnum.Sign }
        //        );
        //    _DBSet.Add(
        //        new ExternalServicePM() { TimeoutInSec = 360, Id = "1-4", Tenant = 2, ServiceAddressUrl = @"http://Tomer:5058/Unifreight/SignService/basic", ServiceType = ServiceTypeEnum.Sign }
        //        );


        //    _DBSet.Add(
        //        new ExternalServicePM() { TimeoutInSec = 360, Id = "1-5", Tenant = 1, ServiceAddressUrl = @"http://itzik7:5050/Unifreight/DCAService/Basic", ServiceType = ServiceTypeEnum.DCA }
        //        );
        //    _DBSet.Add(
        //        new ExternalServicePM() { TimeoutInSec = 360, Id = "1-6", Tenant = 1, ServiceAddressUrl = @"http://localhost:5050/Unifreight/DCAService/Basic", ServiceType = ServiceTypeEnum.DCA }
        //        );
        //}
        
        public List<ExternalServicePM> GetAll(int tenant, ServiceTypeEnum servicetype)
        {

            var repository = new CustomsSettingRepository(tenant);
            var l=repository.GetAll(tenant).ToList().Select(rec => GetMe(rec, servicetype)).ToList();
            return l;
            //return _dbset.where(rec => rec.servicetype == servicetype && rec.tenant == tenant).tolist();
        }

        private ExternalServicePM GetMe(EntityPOCOs.CustomsSetting rec, ServiceTypeEnum servicetype)
        {
            switch (servicetype)
            {
                case ServiceTypeEnum.DCA:
                    return new ExternalServicePM() { TimeoutInSec = 360, Id = rec.Id, Tenant = rec.Tenant, ServiceAddressUrl = rec.DCAServiceAddress, ServiceType = servicetype };
                    break;
                case ServiceTypeEnum.Sign:

                    return new ExternalServicePM() { TimeoutInSec = 360, Id = rec.Id, Tenant = rec.Tenant, ServiceAddressUrl = rec.SignServiceAddress, ServiceType = servicetype };
                    break;
                default:
                    return null;
                    break;
            }
        }
    }

    public enum ServiceTypeEnum { DCA = 1, Sign = 2 }
    public class ExternalServicePM
    {



        public string Id { get; set; }
        public ServiceTypeEnum ServiceType { get; set; }
        public int Tenant { get; set; }

        public string MoreParam { get; set; }

        public string ServiceAddressUrl { get; set; }
        public int TimeoutInSec { get; set; }
    }
}
