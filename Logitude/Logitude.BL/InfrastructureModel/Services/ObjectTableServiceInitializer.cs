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
        public ObjectTableServiceInitializer(ObjectTablePM entityPM, IWebFreightContext objectContext)
        {
            this.entityPM = entityPM;
            this.objectContext = objectContext;
            this.textCodeRepository = new TextCodeRepository(objectContext);
            this.objectTableDefaultFieldsService = new ObjectTableDefaultFieldsService(entityPM, objectContext);
        }

        public void Initialize()
        {
            InitializeEntity();
            InitializeTextCode();
            this.objectTableDefaultFieldsService.AddDefaultFields();
        }

        private void InitializeEntity()
        {
            MapEntityName();
            this.entityPM.AvailableInCustomization = true;
            this.entityPM.HasCustomFields = true;
            this.entityPM.MaxNumberOfCustomFields = 50;
            this.entityPM.AllowCustomFields = true;
            if ((entityPM.IsCustom && string.IsNullOrEmpty(entityPM.ParentObjectTableId))) return;
            this.entityPM.IsComposition = true;
        }
        private void MapEntityName()
        {
            if (entityPM.IsCustom && !string.IsNullOrEmpty(entityPM.ParentObjectTableId))
            {
                MapCustomSubEntityName();
                return;
            }
            if (entityPM.IsCustom && string.IsNullOrEmpty(entityPM.ParentObjectTableId))
            {
                MapCustomEntityName();
                return;
            }  
        }

        private void MapCustomSubEntityName()
        {
            ObjectTableRepository entityRepository = new ObjectTableRepository(objectContext);
            ObjectTable  parentObjectTable = entityRepository.GetSingleObjectTable(entityPM.ParentObjectTableId, entityPM.Tenant, false);
            if (entityPM.Name.StartsWith(parentObjectTable.Id + "." + entityPM.Tenant + ".")) return;
            entityPM.Name = parentObjectTable.Id + "." + entityPM.Tenant + "." + entityPM.Name;
        }
        private void MapCustomEntityName()
        {
            if (entityPM.Name.StartsWith("C." + entityPM.Tenant + ".")) return;
            entityPM.Name = "C." + entityPM.Tenant + "." + entityPM.Name;
        }
        private void InitializeTextCode()
        {
            CreateDefaultTextCode();
            TextCode descreptionTextCode = CreateDescriptionTextCode();
            MapDescreptionTextCode(descreptionTextCode);
        }

        private void CreateDefaultTextCode()
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
        private void MapDescreptionTextCode (TextCode descreptionTextCode)
        {
            if (descreptionTextCode == null) return;
            //entityPM.DescriptionTextCodeId = descreptionTextCode.Id;
            entityPM.DescriptionTextCodeCode = descreptionTextCode.Code;
        }

    }
}