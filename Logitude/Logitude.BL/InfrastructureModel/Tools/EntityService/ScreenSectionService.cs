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
    public class ScreenSectionService
    {

        bool isNewEntity;
        private int tenant;
        public ScreenSection Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ScreenSectionPM entityPM;
        private IWebFreightContext objectContext;
        private ScreenSectionRepository entityRepository;
        public ScreenSectionService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ScreenSectionRepository(objectContext);
        }

        public void Create(ScreenSectionPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ScreenSection", tenant).ToString();
            this.Poco = new ScreenSection();

            ScreenSectionMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ScreenSectionPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleScreenSection(theEntityPm.Id,entityPM.Tenant);
            if (this.Poco == null) return;
            //if (IsSectionDeleted()) this.Poco.Inactive = true;
            //else 
            ScreenSectionMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }


        private bool IsSectionDeleted()
        {
            return (this.Poco.Inactive != this.entityPM.Inactive && this.entityPM.Inactive == true); 
        }

        public void Update(List<ScreenSectionPM> screenSections, Screen screen)
        {
            if (screen != null && screen.Type == "Grid") return;

            screenSections.ForEach((screenSection) =>
            {
                switch (screenSection.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            Create(screenSection);
                            break;
                        }
                    case ChangeSetOperation.Update:
                        {
                           Update(screenSection);
                            break;
                        }
                    default: { break; }
                }
            });
        }

      
    }
}