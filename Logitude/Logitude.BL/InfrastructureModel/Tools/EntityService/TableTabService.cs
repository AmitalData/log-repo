using System;
using System.Collections.Generic;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TableTabService
    {
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
                    UpdateEntity(tab.Changeset, MapPoco(tab));

                repository.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private void UpdateEntity(string changeset, ObjectTableTab entityPoco)
        {
            switch (changeset)
            {
                case Changeset.Insert:
                    repository.Add(entityPoco);
                    break;
                case Changeset.Update:
                    repository.Update(entityPoco);
                    break;
                case Changeset.Delete:
                    repository.Remove(entityPoco);
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
            };
        }

    }

    struct Changeset
    {
        public const string Insert = "insert";
        public const string Update = "update";
        public const string Delete = "delete";
    }
}