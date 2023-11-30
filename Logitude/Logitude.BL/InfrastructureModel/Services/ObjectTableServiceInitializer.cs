using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ObjectTableServiceInitializer
    {

        private ObjectTablePM entityPM;
        private TextCodeRepository textCodeRepository;
        private ObjectTableDefaultFieldsService objectTableDefaultFieldsService;
        private IWebFreightContext objectContext;
        private ObjectTableRepository objectTableRepository;
        public ObjectTableServiceInitializer(ObjectTablePM entityPM, IWebFreightContext objectContext, ObjectTableRepository objectTableRepository)
        {
            this.entityPM = entityPM;
            this.objectContext = objectContext;
            this.objectTableRepository = objectTableRepository;
            this.textCodeRepository = new TextCodeRepository(objectContext);
            this.objectTableDefaultFieldsService = new ObjectTableDefaultFieldsService(entityPM, objectContext);
        }

        public void Initialize()
        {
            InitializeEntity();
            this.objectTableDefaultFieldsService.AddDefaultFields();
        }

        private void InitializeEntity()
        {
            MapEntityName();
            this.entityPM.AvailableInCustomization = true;
            this.entityPM.HasCustomFields = true;
            this.entityPM.MaxNumberOfCustomFields = 50;
            this.entityPM.AllowCustomFields = true;
            this.entityPM.ClientModuleName = string.IsNullOrEmpty(entityPM.ClientModuleName)? "Infrastructure" : entityPM.ClientModuleName;
            if ((entityPM.IsCustom && string.IsNullOrEmpty(entityPM.ParentObjectTableId)))
            {
                this.entityPM.AvailableInDocumentTypes = true;
                return;
            }
            this.entityPM.IsComposition = true;
        }

        private void MapEntityName()
        {
            entityPM.Name = entityPM.IsCustom ? entityPM.Id : entityPM.Name;
        }

        public void InitializeTextCode(ObjectTable objectTable)
        {
            TextCode defaultTextCode = CreateDefaultTextCode();
            MapDefaultTextCode(defaultTextCode, objectTable);
            TextCode descreptionTextCode = CreateDescriptionTextCode();
            MapDescreptionTextCode(descreptionTextCode, objectTable);
            objectTableRepository.Update(objectTable);
            objectContext.SaveChanges();
        }

        private TextCode CreateDefaultTextCode()
        {
            TextCode textCode = new TextCode();
            textCode.Id = IdCounter.GetNumber("TextCode", this.entityPM.Tenant).ToString();
            textCode.ObjectTableId = this.entityPM.Id;
            textCode.Code = this.entityPM.Name;
            textCode.DefaultText = this.entityPM.DefaultText;
            textCode.DefaultTextPlural = this.entityPM.DefaultTextPlural;
            textCode.LocalDefaultText = this.entityPM.DefaultText;
            textCode.Tenant = this.entityPM.Tenant;
            textCode.TextCodeTypeCode = "T";
            textCodeRepository.Add(textCode);
            return textCode;
        }

        private TextCode CreateDescriptionTextCode()
        {
            TextCode descriptionTextCode = new TextCode();
            descriptionTextCode.Id = IdCounter.GetNumber("TextCode", this.entityPM.Tenant).ToString();
            descriptionTextCode.ObjectTableId = this.entityPM.Id;
            descriptionTextCode.Code = this.entityPM.Name + ".Description";
            descriptionTextCode.DefaultText = this.entityPM.Description;
            descriptionTextCode.LocalDefaultText = this.entityPM.Description;
            descriptionTextCode.Tenant = this.entityPM.Tenant;
            descriptionTextCode.TextCodeTypeCode = "T";
            textCodeRepository.Add(descriptionTextCode);
            return descriptionTextCode;
        }
        private void MapDescreptionTextCode (TextCode descreptionTextCode, ObjectTable objectTable)
        {
            if (descreptionTextCode == null) return;
            entityPM.DescriptionTextCodeId = objectTable.DescriptionTextCodeId = descreptionTextCode.Id;
            entityPM.DescriptionTextCodeCode = objectTable.DescriptionTextCodeCode = descreptionTextCode.Code;
        }

        private void MapDefaultTextCode(TextCode defaultTextCode, ObjectTable objectTable)
        {
            if (defaultTextCode == null) return;
            entityPM.FullNameTextCodeId = objectTable.FullNameTextCodeId = defaultTextCode.Id;
            entityPM.FullNameTextCodeCode = objectTable.FullNameTextCodeCode = defaultTextCode.Code;
        }

    }
}