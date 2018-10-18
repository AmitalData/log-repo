using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class FormCustomFieldService
    {
        bool isNewEntity;
        private int tenant;
        public FormCustomField Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private FormCustomFieldPM entityPM;
        private ICommonDataContext objectContext;
        private FormCustomFieldRepository entityRepository;
        public FormCustomFieldService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new FormCustomFieldRepository(objectContext);
        }

        public void Create(FormCustomFieldPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("FormCustomField", tenant).ToString();
            this.Poco = new FormCustomField();
            this.Poco.Id = this.entityPM.Id;

            FormCustomFieldValidating.Validate(theEntityPm);
            FormCustomFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            FormCustomFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(FormCustomFieldPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleFormCustomField(theEntityPm.Id, theEntityPm.Tenant);

            FormCustomFieldValidating.Validate(theEntityPm);
            FormCustomFieldTracing.Trace(theEntityPm, Poco, isNewEntity);
            FormCustomFieldMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
