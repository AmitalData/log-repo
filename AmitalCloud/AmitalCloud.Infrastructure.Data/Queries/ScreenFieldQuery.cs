using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class ScreenFieldQuery
    {
        private readonly Repository<ScreenField> repository;
        public ScreenFieldQuery(int tenant)
        {
            repository = new Repository<ScreenField>(AmitalCloudContext.GetContext(tenant));
        }
        public List<ScreenFieldPM> GetScreenFieldPMsByTenant(int tenant)
        {
            List<ScreenFieldPM> screenFieldPMs = repository.GetMulti(a => a.Tenant == tenant || a.Tenant == 0, a => new ScreenFieldPM(a)
            {
                ObjectFieldName = a.ObjectField != null ? a.ObjectField.FieldName : null,
                ObjectFieldObjectTableName = a.ObjectField != null ? a.ObjectField.ObjectTable != null ? a.ObjectField.ObjectTable.Name : "" : "",
            }, "ObjectField,ObjectField.ObjectTable");

            List<ScreenFieldPM> selectedScreenFields = new List<ScreenFieldPM>();
            foreach (ScreenFieldPM field in screenFieldPMs)
            {
                ScreenFieldPM existedField = (from a in selectedScreenFields
                                              where a.ScreenId == field.ScreenId && a.ObjectFieldCode == field.ObjectFieldCode
                                              select a).FirstOrDefault();

                if (existedField == null)
                {
                    selectedScreenFields.Add(field);
                }
                else if (existedField.Tenant == 0)
                {
                    selectedScreenFields.Remove(existedField);
                    selectedScreenFields.Add(field);
                }
            }

            return selectedScreenFields;
        }
    }
}
