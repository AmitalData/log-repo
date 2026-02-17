
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TwoFactorAuthenticationDeviceService
    {
        bool isNewEntity;
        private int tenant;
        public TwoFactorAuthenticationDevice Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TwoFactorAuthenticationDevicePM entityPm;
        private ICommonDataContext objectContext;
        private TwoFactorAuthenticationDeviceRepository entityRepository;
        public TwoFactorAuthenticationDeviceService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TwoFactorAuthenticationDeviceRepository(objectContext);
        }

        public void Create(TwoFactorAuthenticationDevicePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.TwoFactorkey = "";
            this.Poco = new TwoFactorAuthenticationDevice();
            this.Poco.TwoFactorkey  = PasswordGenerator.Generate(20);

            TwoFactorAuthenticationDeviceMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(TwoFactorAuthenticationDevicePM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleTwoFactorAuthenticationDevice(entityPM.Id, entityPm.Tenant);


            TwoFactorAuthenticationDeviceMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }

    }
}