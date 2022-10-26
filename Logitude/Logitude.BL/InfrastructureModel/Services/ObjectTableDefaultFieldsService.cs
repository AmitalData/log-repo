using System.Collections.Generic;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ObjectTableDefaultFieldsService
    {

        private ObjectTablePM entityPM;
        private TextCodeRepository textCodeRepository;
        private ObjectFieldRepository objectFieldRepository;

        public ObjectTableDefaultFieldsService(ObjectTablePM entityPM, IWebFreightContext objectContext)
        {
            this.entityPM = entityPM;
            this.textCodeRepository = new TextCodeRepository(objectContext);
            this.objectFieldRepository = new ObjectFieldRepository(objectContext);
        }

        public void AddDefaultFields()
        {
            List<ObjectField> defaultObjectFields = FillDefaultObjectFields() ;

            foreach(ObjectField objectField in defaultObjectFields)
            {
                this.objectFieldRepository.Add(objectField);
            }

        }
        private List<ObjectField> FillDefaultObjectFields()
        {
            var defaultObjectFields = new List<ObjectField>();

            ObjectField objectField = GetIdObjectField();
            defaultObjectFields.Add(objectField);

            objectField = GetTenantObjectField();
            defaultObjectFields.Add(objectField);

            objectField = GetCreateDateObjectField();
            defaultObjectFields.Add(objectField);

            objectField = GetCreatedByObjectField();
            defaultObjectFields.Add(objectField);

            objectField = GetUpdateDateObjectField();
            defaultObjectFields.Add(objectField);

            objectField = GetUpdatedByObjectField();
            defaultObjectFields.Add(objectField);

            return defaultObjectFields;
        }
        private ObjectField GetIdObjectField()
        {
            ObjectField objectField = GetNewObjectFieldInstance("Id");
            objectField.DataTypeCode = "Text";
            objectField.MaxLength = 15;
            objectField.SystemMaxLength = 15;
            objectField.ForMetaDataOnly = true;
            this.AddFullNameTextCode("Id", objectField);
            return objectField;
        }
        private ObjectField GetTenantObjectField()
        {
            ObjectField objectField = GetNewObjectFieldInstance("Tenant");
            objectField.DataTypeCode = "Integer";
            objectField.SystemRequired = true;
            objectField.ForMetaDataOnly = true;
            this.AddFullNameTextCode("Tenant", objectField);
            return objectField;
        }
        private ObjectField GetCreateDateObjectField()
        {
            ObjectField objectField = GetNewObjectFieldInstance("CreateDate");
            objectField.DataTypeCode = "DateTime";
            objectField.CanFilter = true;
            objectField.DisplayInList = true;
            objectField.Operator = "Equals";
            objectField.ValidForQuerySection1 = this.entityPM.Name;
            objectField.DisplayInEntityVariables = true;
            objectField.AllowedinAutomationConditions = true;
            objectField.HasTemplate = true;
            objectField.DisplayOnly = true;
            this.AddFullNameTextCode("Create Date", objectField);
            return objectField;
        }
        private ObjectField GetCreatedByObjectField()
        {
            ObjectField objectField = GetNewObjectFieldInstance("CreatedBy");
            objectField.DataTypeCode = "Text";
            objectField.MaxLength = 15;
            objectField.SystemMaxLength = 15;
            objectField.CanFilter = true;
            objectField.Operator = "Equals";
            objectField.ValidForQuerySection1 = this.entityPM.Name;
            objectField.DisplayInEntityVariables = true;
            objectField.AllowedinAutomationConditions = true;
            objectField.AutomationEmailRecipient = true;
            objectField.DisplayOnly = true;
            this.AddFullNameTextCode("Created By", objectField);
            return objectField;
        }
        private ObjectField GetUpdateDateObjectField()
        {
            ObjectField objectField = GetNewObjectFieldInstance("UpdateDate");
            objectField.DataTypeCode = "DateTime";
            objectField.CanFilter = true;
            objectField.DisplayInList = true;
            objectField.Operator = "Equals";
            objectField.ValidForQuerySection1 = this.entityPM.Name;
            objectField.DisplayInEntityVariables = true;
            objectField.AllowedinAutomationConditions = true;
            objectField.HasTemplate = true;
            objectField.DisplayOnly = true;
            this.AddFullNameTextCode("Update Date", objectField);
            return objectField;
        }
        private ObjectField GetUpdatedByObjectField()
        {
            ObjectField objectField = GetNewObjectFieldInstance("UpdatedBy");
            objectField.DataTypeCode = "Text";
            objectField.MaxLength = 15;
            objectField.SystemMaxLength = 15;
            objectField.CanFilter = true;
            objectField.Operator = "Equals";
            objectField.ValidForQuerySection1 = this.entityPM.Name;
            objectField.DisplayInEntityVariables = true;
            objectField.AllowedinAutomationConditions = true;
            objectField.AutomationEmailRecipient = true;
            objectField.DisplayOnly = true;
            this.AddFullNameTextCode("Updated By", objectField);
            return objectField;
        }
        private ObjectField GetNewObjectFieldInstance(string objectFieldName)
        {
            return new ObjectField()
            {
                Id = IdCounter.GetNumber("ObjectField", this.entityPM.Tenant).ToString(),
                ObjectTableId = this.entityPM.Id,
                FieldName = objectFieldName,
                Tenant = this.entityPM.Tenant,
                IsRequiered = true,
                Code = objectFieldName,
                PMPropertyPath = objectFieldName,
                ListPropertyPath = objectFieldName,
                FieldCode = this.entityPM.Name + ".F." + objectFieldName,
                IsCustom = false,
            };

        }
        private void AddFullNameTextCode(string defaultText, ObjectField objectField)
        {
            TextCode textCode = new TextCode();
            textCode.Id = IdCounter.GetNumber("TextCode", this.entityPM.Tenant).ToString();
            textCode.ObjectTableId = this.entityPM.Id;
            textCode.Code = objectField.FieldCode;
            textCode.DefaultText = defaultText;
            textCode.Tenant = this.entityPM.Tenant;
            textCode.TextCodeTypeCode = "F";

            objectField.FullNameTextCodeId = textCode.Id;
            objectField.FullNameTextCodeCode = textCode.Code;
            textCodeRepository.Add(textCode);
        }
       
    }
}