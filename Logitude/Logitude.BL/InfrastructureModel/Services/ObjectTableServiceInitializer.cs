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

        public ObjectTableServiceInitializer(ObjectTablePM entityPM, IWebFreightContext objectContext)
        {
            this.entityPM = entityPM;
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
            this.entityPM.AvailableInCustomization = true;
            this.entityPM.IsComposition = true;
            this.entityPM.ObjectTableTypeCode = "BR";
            this.entityPM.HasCustomFields = true;
            this.entityPM.MaxNumberOfCustomFields = 50;
            this.entityPM.AllowCustomFields = true;
        }

        private void InitializeTextCode()
        {
            CreateDefaultTextCode();
            CreateDescriptionTextCode();
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

        private void CreateDescriptionTextCode()
        {
            TextCode descriptionTextCode = new TextCode();
            descriptionTextCode.Id = IdCounter.GetNumber("TextCode", this.entityPM.Tenant).ToString();
            descriptionTextCode.ObjectTableId = this.entityPM.Id;
            descriptionTextCode.Code = this.entityPM.Name + "Description";
            descriptionTextCode.DefaultText = this.entityPM.Description;
            descriptionTextCode.LocalDefaultText = this.entityPM.Description;
            descriptionTextCode.Tenant = this.entityPM.Tenant;
            descriptionTextCode.TextCodeTypeCode = "T";
            textCodeRepository.Add(descriptionTextCode);
        }

    }
}