
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

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class TenantLoginPolicyService
    {
        bool isNewEntity;
        private int tenant;
        public TenantLoginPolicy Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TenantLoginPolicyPM entityPm;
        private ICommonDataContext objectContext;
        private TenantLoginPolicyRepository entityRepository;
        public TenantLoginPolicyService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TenantLoginPolicyRepository(objectContext);
        }

        public void Create(TenantLoginPolicyPM entityPM)
        {
            if (entityPM.LoginPolicyCode == "TFAUTH" && entityPM.IsEnabledForSpecificUsers)
            {
                UserRepository userRepository = new UserRepository(tenant);

                bool hasTwoFactorEnabledUsers = userRepository.CheckUsersTwoFactorAuthenticationEnabled(tenant);
                if (!hasTwoFactorEnabledUsers)
                    throw new Exception("Please define at least one user to be enabled for two factor authentication");
            }

            this.isNewEntity = true;
            this.entityPm = entityPM;

            this.Poco = new TenantLoginPolicy();


            TenantLoginPolicyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(TenantLoginPolicyPM entityPM)
        {
            if (entityPM.LoginPolicyCode == "TFAUTH" && entityPM.IsEnabledForSpecificUsers)
            {
                UserRepository userRepository = new UserRepository(tenant);

                bool hasTwoFactorEnabledUsers = userRepository.CheckUsersTwoFactorAuthenticationEnabled(tenant);
                if (!hasTwoFactorEnabledUsers)
                    throw new Exception("Please define at least one user to be enabled for two factor authentication");
            }
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleTenantLoginPolicy(entityPm.Tenant);


            TenantLoginPolicyMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();


        }

    }
}