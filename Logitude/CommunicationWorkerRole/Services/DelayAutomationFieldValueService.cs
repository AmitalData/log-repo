using Logitude.BL.CommonDataModel.EntityQueries;
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
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole.Services
{
   public class DelayAutomationFieldValueService
    {


        private ResolverAutomationObjectFieldService resolverAutomationObjectFieldService;
        private ObjectFieldRepository objectFieldRepository;
        private int tenant;
        private List<Entity> entities;
        private List<ObjectField> objectFields;
        private string objectTableName;
        private string entityId;
        private HtmlEditorHelper htmlEditorHelper;

        public DelayAutomationFieldValueService(string objectTableId, string entityId , int tenant)
        {
            this.tenant = tenant;
            this.objectTableName = GetObjectTableName(ObjectTableRepository.GetNameById(objectTableId, tenant));
            this.entityId = entityId;
            this.entities = new List<Entity>();
            htmlEditorHelper = new HtmlEditorHelper();
            resolverAutomationObjectFieldService = new ResolverAutomationObjectFieldService(null);
            objectFieldRepository = new ObjectFieldRepository(tenant);
            objectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(this.objectTableName, tenant);

        }

        private object GetEntity(string entityId, string objectTableName)
        {
            var entity = this.entities.Where(d => d.Name == objectTableName).FirstOrDefault();
            if (entity != null) return entity.Value;
            entity = new Entity() { Value = htmlEditorHelper.GetEntity(objectTableName, entityId, tenant), Name = objectTableName };
            this.entities.Add(entity) ;
            return entity.Value;
        }
        public List<Field> Execute(List<Field> fields)
        {
            var entity = GetEntity(this.entityId , this.objectTableName); 
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
            ObjectField objectField = GetObjectField(field.FieldCode); 
            if (objectField == null) return;
            if (!string.IsNullOrEmpty(field.PartnerObjectFieldCode))
            {
                field.Value = GetPartnerFieldValue(field, objectField);
                return;
            }

            field.Value = GetFieldValue(objectField, this.objectTableName, this.entityId);

        }

        private string GetFieldValue(ObjectField objectField, string objectTableName, string entityId)
        {
            var entity = GetEntity(entityId, objectTableName);
            if (entity == null) return null;
            return resolverAutomationObjectFieldService.GetValue(entity, objectField);
        }

        private ObjectField GetObjectField(string fieldCode)
        {
            var objectField = objectFields.Where(d => d.FieldCode == fieldCode).FirstOrDefault();
            if (objectField != null) return objectField;
            objectField = objectFieldRepository.GetSingleObjectFieldByObjectFieldCode(fieldCode);
            objectFields.Add(objectField);
            return objectField;

        }

        private string GetPartnerFieldValue(Field field, ObjectField objectField)
        {
            var partnerObjectField = GetObjectField(field.PartnerObjectFieldCode);
            var partnerObjectFieldValue = GetFieldValue(partnerObjectField , this.objectTableName, this.entityId);
            var parentObjectTableName = GetObjectTableName(ObjectTableRepository.GetNameById(partnerObjectField.LookUpTableId,tenant));
            return  GetFieldValue(objectField, parentObjectTableName, partnerObjectFieldValue);

        }

        private string GetObjectTableName(string tableName)
        {
            return tableName.ToLower() == "master" ? "Shipment" : tableName;
        }
    }
    public class Entity
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
