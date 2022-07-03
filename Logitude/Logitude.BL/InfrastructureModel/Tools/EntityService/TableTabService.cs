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
        ObjectTableTabRepository repository;
        IWebFreightContext context;

        public TableTabService(int tenant)
        {
            this.tenant = tenant;
            context = WebFreightContext.GetContext(tenant);
            repository = new ObjectTableTabRepository(context);
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
            TableTabMappingService mappingService = new TableTabMappingService(repository);
            ObjectTableTab tab = mappingService.MapPoco(tabPM);

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
                        OnTabUpdate(tab, tabPM);
                        
                        repository.Update(tab);
                        break;
                    }
                case Changeset.Delete:
                    {
                        OnTabDelete(tab, tabPM);
                        repository.Remove(tab);
                        break;
                    }
            }
        }

        private void OnTabCreate(ObjectTableTab tab, ObjectTableTabPM tabPM)
        {
            tab.Id = IdCounter.GetNumber("ObjectTableTab", tab.Tenant);
            tab.Code = Guid.NewGuid().ToString().Substring(0, 4);
            tabPM.Code = tab.Code;
            tab.ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl";

            if(tabPM.TabNameTextCodeCode == null)
                AddNewTextCodeForTab(tab, tabPM);
        }
        private void OnTabUpdate(ObjectTableTab tab, ObjectTableTabPM tabPM)
        {
            if (tab.Tenant == 0)
                new TableTabModificationService(tenant, context).UpdateModification(tabPM);
            else
                CheckNameTextCodeChange(tab, tabPM);

        }

        private void CheckNameTextCodeChange(ObjectTableTab tab, ObjectTableTabPM tabPM)
        {
            var textCodeRepository = new TextCodeRepository(context);
            var textCode = textCodeRepository.GetSingleTextCodeByCode(tab.TabNameTextCodeCode);
            if (textCode.DefaultText != tabPM.Name)
            {
                textCode.DefaultText = tabPM.Name;
                textCodeRepository.Update(textCode);
                textCodeRepository.SubmitChanges();
            }
        }

        private void OnTabDelete(ObjectTableTab tab, ObjectTableTabPM tabPM)
        {
        }

        private void AddNewTextCodeForTab(ObjectTableTab tab, ObjectTableTabPM tabPM)
        {
            var nameTextCode = CreateTabNameTextCode(tabPM);
            tab.TabNameTextCodeId = nameTextCode.Id;
            tab.TabNameTextCodeCode = nameTextCode.Code;
        }

        private TextCodePM CreateTabNameTextCode(ObjectTableTabPM tab)
        {

            TextCodeService textCodeService = new TextCodeService(WebFreightContext.GetContext(tenant), tenant);

            TextCodePM textCode = textCodeService.Create(new TextCodePM()
            {
                Code = "ObjectTableTab.O." + tab.Code,
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