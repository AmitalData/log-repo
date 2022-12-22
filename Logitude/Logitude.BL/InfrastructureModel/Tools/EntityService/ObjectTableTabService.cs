using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ObjectTableTabService
    {
        bool isNewEntity;
        private int tenant;
        private int tenantZero = 0;
        private ObjectTableTab Poco { get; set; }
        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private ObjectTableTabPM entityPM;

        private IWebFreightContext objectContext;
        private ObjectTableTabRepository entityRepository;

        public ObjectTableTabService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ObjectTableTabRepository(objectContext);

            
        }

        public void Create(ObjectTableTabPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            SetDefultProperties();
            this.Poco = new ObjectTableTab();
            this.Poco.Id = this.entityPM.Id;
            ObjectTableTabValidating.Validate(theEntityPm);
            ObjectTableTabTracing.Trace(theEntityPm, Poco, isNewEntity);
            ObjectTableTabMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

  

        public void Update(ObjectTableTabPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectTableTab(theEntityPm.Id);
            ObjectTableTabValidating.Validate(theEntityPm);
            ObjectTableTabTracing.Trace(theEntityPm, Poco, isNewEntity);
            UpdateTabModification(theEntityPm);
            UpdateTextCode();
            ObjectTableTabMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }


        public void Updates(List<ObjectTableTabPM> objectTableTabs)
        {
            foreach (ObjectTableTabPM objectTableTabPM in objectTableTabs)
            {
                switch (objectTableTabPM.Changeset)
                {
                    case "insert":
                        {
                            Create(objectTableTabPM);
                            break;
                        }
                    case "update":
                        {
                            Update(objectTableTabPM);
                            break;
                        }
                    case "delete":
                        {
                            Delete(objectTableTabPM);
                            break;
                        }
                }
            }

        }


        public void Delete(ObjectTableTabPM theEntityPm)
        {
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleObjectTableTab(theEntityPm.Id);
            if (this.Poco == null) return;
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();
        }



        private void SetDefultProperties()
        {
            this.entityPM.Id = IdCounter.GetNumber("ObjectTableTab", tenant).ToString();
            this.entityPM.Code = Guid.NewGuid().ToString().Substring(0, 4);
            this.entityPM.ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl";
            if (this.entityPM.TabNameTextCodeCode == null || this.entityPM.CreateDefaultTextCode) new ObjectTableTabTextCodeService(this.entityPM, this.ObjectContext, tenant).Create();
        }

        private void UpdateTextCode()
        {
            if (this.entityPM.Tenant == tenantZero) return;
            new ObjectTableTabTextCodeService(this.entityPM, this.ObjectContext, tenant).Update();

        }

        private void UpdateTabModification(ObjectTableTabPM theEntityPm)
        {
            new TableTabModificationService(tenant, objectContext).Update(theEntityPm);
        }



    }
}