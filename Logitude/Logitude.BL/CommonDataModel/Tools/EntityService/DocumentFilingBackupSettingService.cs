using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DocumentFilingBackupSettingService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentFilingBackupSetting Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentFilingBackupSettingPM entityPM;
        private ICommonDataContext objectContext;
        private DocumentFilingBackupSettingRepository entityRepository;
        public DocumentFilingBackupSettingService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentFilingBackupSettingRepository(objectContext);
        }

        public void Create(DocumentFilingBackupSettingPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.Poco = new DocumentFilingBackupSetting();
            if (this.entityPM.IsActive)
            {
                this.entityPM.ActivationDate = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
            }

            DocumentFilingBackupSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(DocumentFilingBackupSettingPM theEntityPm)
        {
            string entityPmName = "DocumentFilingBackupSettingPM" + theEntityPm.Tenant;
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentFilingBackupSetting(entityPM.Tenant, 0);

            if (this.entityPM.IsActive && !this.Poco.IsActive)
            {
                this.entityPM.ActivationDate = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
            }
            else if (!theEntityPm.IsActive && this.Poco.IsActive)
            {
                this.entityPM.DeactivationDate = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
            }

            DocumentFilingBackupSettingMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

        }
    }
}
