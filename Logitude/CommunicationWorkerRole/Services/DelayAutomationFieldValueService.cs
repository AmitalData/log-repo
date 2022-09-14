using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.Services
{
   public class DelayAutomationFieldValueService
    {


        private List<Field> fields;
        private object entity;
        private ResolverAutomationObjectFieldService resolverAutomationObjectFieldService;
        private ObjectFieldRepository objectFieldRepository;
        private int tenant;
        public DelayAutomationFieldValueService(string objectTableName, string entityId , int tenant)
        {
            this.tenant = tenant;
            entity = new AutomationWorkerRole(tenant.ToString()).GetEntity(objectTableName, entityId, tenant);
            resolverAutomationObjectFieldService = new ResolverAutomationObjectFieldService(null);
            objectFieldRepository = new ObjectFieldRepository(tenant);
        }

        public List<Field> Run()
        {
            if (entity == null) return fields;
            foreach (Field field in fields)
            {
                UpdateFieldValue(field);
            }
            return fields;
        }

        private void UpdateFieldValue(Field field)
        {
            if (string.IsNullOrEmpty(field.Value)) return;
            if (string.IsNullOrEmpty(field.FieldCode)) return;
            ObjectField objectField = objectFieldRepository.GetSingleObjectFieldByObjectFieldCode(field.FieldCode);
            if (objectField == null) return;
            if (string.IsNullOrEmpty(field.PartnerObjectFieldCode))
            {
                field.Value = resolverAutomationObjectFieldService.GetValue(entity , objectField);
            }
            else
            {
                field.Value = GetValueFromParentEntity(field , objectField);
            }
        }

        private string GetValueFromParentEntity(Field field, ObjectField objectField)
        {
            var parentEntityId = fields.Where(d => d.FieldCode == field.PartnerObjectFieldCode).FirstOrDefault().Value;
            var parentEntity = new AutomationWorkerRole("").GetEntity(field.PartnerObjectFieldCode.Split('.')[0], parentEntityId, tenant);
            return  resolverAutomationObjectFieldService.GetValue(parentEntity, objectField);

        }
    }
}
