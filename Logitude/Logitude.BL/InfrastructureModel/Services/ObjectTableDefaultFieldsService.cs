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
        private string userObjectTableId;

        public ObjectTableDefaultFieldsService(ObjectTablePM entityPM, IWebFreightContext objectContext)
        {
            this.entityPM = entityPM;
            this.textCodeRepository = new TextCodeRepository(objectContext);
            this.objectFieldRepository = new ObjectFieldRepository(objectContext);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(objectContext);
            userObjectTableId = objectTableRepository.GetObjectTableIdByName("User");
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
            AddListTextCode("Create Date", objectField);
            return objectField;
        }
        private ObjectField GetCreatedByObjectField()
        {
            ObjectField objectField = GetNewObjectFieldInstance("CreatedBy");
            objectField.DataTypeCode = "LookUp";
            objectField.LookUpTableId = userObjectTableId;
            objectField.MaxLength = 15;
            objectField.SystemMaxLength = 15;
            objectField.CanFilter = true;
            objectField.DisplayInList = true;
            objectField.Operator = "Equals";
            objectField.ValidForQuerySection1 = this.entityPM.Name;
            objectField.DisplayInEntityVariables = true;
            objectField.AllowedinAutomationConditions = true;
            objectField.AutomationEmailRecipient = true;
            objectField.DisplayOnly = true;
            this.AddFullNameTextCode("Created By", objectField);
            AddListTextCode("Created By", objectField);
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
            AddListTextCode("Update Date", objectField);
            return objectField;
        }
        private ObjectField GetUpdatedByObjectField()
        {
            ObjectField objectField = GetNewObjectFieldInstance("UpdatedBy");
            objectField.DataTypeCode = "LookUp";
            objectField.LookUpTableId = userObjectTableId;
            objectField.MaxLength = 15;
            objectField.SystemMaxLength = 15;
            objectField.CanFilter = true;
            objectField.DisplayInList = true;
            objectField.Operator = "Equals";
            objectField.ValidForQuerySection1 = this.entityPM.Name;
            objectField.DisplayInEntityVariables = true;
            objectField.AllowedinAutomationConditions = true;
            objectField.AutomationEmailRecipient = true;
            objectField.DisplayOnly = true;
            this.AddFullNameTextCode("Updated By", objectField);
            AddListTextCode("Updated By", objectField);
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

        private void AddListTextCode(string defaultText, ObjectField objectField)
        {
            TextCode textCode = new TextCode();
            textCode.Id = IdCounter.GetNumber("TextCode", this.entityPM.Tenant).ToString();
            textCode.ObjectTableId = this.entityPM.Id;
            textCode.Code = this.entityPM.Name + ".CH." + defaultText.Replace(" ","").Trim() + "ListLable";
            textCode.DefaultText = defaultText;
            textCode.Tenant = this.entityPM.Tenant;
            textCode.TextCodeTypeCode = "CH";

            objectField.ListTextCodeId = textCode.Id;
            objectField.ListTextCodeCode = textCode.Code;
            textCodeRepository.Add(textCode);
        }

    }
}