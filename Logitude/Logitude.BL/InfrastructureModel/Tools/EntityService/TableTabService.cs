using System;
using System.Collections.Generic;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TableTabService
    {
        int tenant;
        private ObjectTableTabRepository repository;

        public TableTabService(int tenant)
        {
            repository = new ObjectTableTabRepository(tenant);
        }
        public void UpdateTabs(List<ObjectTableTabPM> tabs)
        {
            try
            {
                foreach (var tab in tabs)
                    UpdateEntity(tab.Changeset, tab);

                repository.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private void UpdateEntity(string changeset, ObjectTableTabPM tabPM)
        {
            ObjectTableTab tab = MapPoco(tabPM);
            switch (changeset)
            {
                case Changeset.Insert:
                    {
                        OnTabCreate(tab, tabPM);
                        repository.Add(tab);
                        break;
                    }
                case Changeset.Update:
                    {
                        OnTabUpdate(tab);
                        repository.Update(tab);
                        break;
                    }
                case Changeset.Delete:
                    repository.Remove(tab);
                    break;
            }
        }
        private ObjectTableTab MapPoco(ObjectTableTabPM entityPM)
        {
            return new ObjectTableTab()
            {
                Id = entityPM.Id,
                Tenant = entityPM.Tenant,
                ObjectTableId = entityPM.ObjectTableId,
                ControlPath = entityPM.ControlPath,
                TabNameTextCodeId = entityPM.TabNameTextCodeId,
                IndexOrder = entityPM.IndexOrder,
                Code = entityPM.Code,
                FeatureId = entityPM.FeatureId,
                TabNameTextCodeCode = entityPM.TabNameTextCodeCode,
                FeatureUniqeCode = entityPM.FeatureUniqeCode,
                HtmlComponentName = entityPM.HtmlComponentName,
                HtmlComponentUrl = entityPM.HtmlComponentUrl,
                ScreenCode = entityPM.ScreenCode,
                Type = entityPM.Type,
                OriginalTabCode = entityPM.OriginalTabCode,
            };
        }

        private void OnTabCreate(ObjectTableTab tab, ObjectTableTabPM tabPM)
        {
            tab.Id = IdCounter.GetNumber("ObjectTableTab", tab.Tenant);
            tab.Code = Guid.NewGuid().ToString().Substring(0, 4);
            tabPM.Code = tab.Code;
            tab.ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl";

            if(tabPM.TabNameTextCodeCode == null)
            {
                var nameTextCode = CreateTabNameTextCode(tabPM);
                tab.TabNameTextCodeId = nameTextCode.Id;
                tab.TabNameTextCodeCode = nameTextCode.Code;
            }
        }
        private void OnTabUpdate(ObjectTableTab tab)
        {
        }

        private TextCodePM CreateTabNameTextCode(ObjectTableTabPM tab)
        {

            TextCodeService textCodeService = new TextCodeService(WebFreightContext.GetContext(tenant), tenant);

            TextCodePM textCode = textCodeService.Create(new TextCodePM()
            {
                Code = "user-tab." + tab.Code,
                DefaultText = tab.Name,
                DefaultTextPlural = tab.Name,
                ObjectTableId = tab.ObjectTableId,
                Tenant = tab.Tenant,
                TextCodeTypeCode = "O",
                LocalDefaultText = tab.Name
            });

            return textCode;
        }


    }

    struct Changeset
    {
        public const string Insert = "insert";
        public const string Update = "update";
        public const string Delete = "delete";
    }
}