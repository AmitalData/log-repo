using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerProductService
    {
        bool isNewEntity;
        private int tenant;
        public CustomerProduct Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomerProductPM entityPm;
        private ICommonDataContext objectContext;
        private CustomerProductRepository entityRepository;

        public CustomerProductService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerProductRepository(objectContext);
        }

        public void Create(CustomerProductPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.Poco = new CustomerProduct();
            this.Poco.CustomerId = this.entityPm.CustomerId;
            this.Poco.ProductTypeCode = this.entityPm.ProductTypeCode;
        
            CustomerProductMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomerProductPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCustomerProduct(entityPM.CustomerId, entityPm.ProductTypeCode, entityPm.Tenant);
           
            CustomerProductMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }   
    }
}
