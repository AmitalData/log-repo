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
    public class MenuButtonGroupService
    {
        bool isNewEntity;
        private int tenant;
        public MenuButtonGroup Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private MenuButtonGroupPM entityPM;
        private IWebFreightContext objectContext;
        private MenuButtonGroupRepository entityRepository;
        private MenuButtonService service;
        private MenuButtonRepository MenuButtonRepository;
        public MenuButtonGroupService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new MenuButtonGroupRepository(objectContext);
        }

        public void Create(MenuButtonGroupPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("MenuButtonGroup", tenant).ToString();
            this.Poco = new MenuButtonGroup();
            this.Poco.Id = this.entityPM.Id;

            MenuButtonRepository menuButtonRepository = new MenuButtonRepository(ObjectContext);

            if (theEntityPm.MenuButtons != null)
            {
                foreach (MenuButtonPM button in theEntityPm.MenuButtons)
                {
                    MenuButton newButton = new MenuButton();
                    newButton.Id = IdCounter.GetNumber("MenuButton", theEntityPm.Tenant).ToString();
                    button.Id = newButton.Id;
                    newButton.MenuButtonGroupId = theEntityPm.Id;
                    MenuButtonMapping.MapEntity(button, newButton, isNewEntity);
                    menuButtonRepository.Add(newButton);
                }
            }

            MenuButtonGroupValidating.Validate(theEntityPm);
            MenuButtonGroupTracing.Trace(theEntityPm, Poco, isNewEntity);
            MenuButtonGroupMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(MenuButtonGroupPM theEntityPm , List<MenuButtonPM> menuButtonList)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleMenuButtonGroup(theEntityPm.Id);

             service = new MenuButtonService(objectContext, entityPM.Tenant);
             MenuButtonRepository = new MenuButtonRepository(ObjectContext);
   
            foreach (MenuButtonPM r in menuButtonList)
            {
            
                switch (r.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            service.Create(r);
                            //r.Id = IdCounter.GetNumber("MenuButton", theEntityPm.Tenant).ToString();
                            //MenuButton newMenuButton = new MenuButton();
                            //newMenuButton.Id = r.Id;
                            //MenuButtonMapping.MapEntity(r, newMenuButton, isNewEntity);
                            //MenuButtonRepository.Add(newMenuButton);
                            break;
                        }
                    case ChangeSetOperation.Update:
                        {
                            service.Update(r);
                            //MenuButton menuButton = MenuButtonRepository.GetSingleMenuButton(r.Id);
                            //MenuButtonMapping.MapEntity(r, menuButton, isNewEntity);
                            //MenuButtonRepository.Update(menuButton);
                            break;
                        }
                    case ChangeSetOperation.Delete:
                        {
                            MenuButton menuButton = MenuButtonRepository.GetSingleMenuButton(r.Id);
                            MenuButtonRepository.Remove(menuButton);

                            break;
                        }
                    case ChangeSetOperation.None:
                        {

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }


            MenuButtonGroupValidating.Validate(theEntityPm);
            MenuButtonGroupTracing.Trace(theEntityPm, Poco, isNewEntity);
            MenuButtonGroupMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

          
          
        }

    }
}