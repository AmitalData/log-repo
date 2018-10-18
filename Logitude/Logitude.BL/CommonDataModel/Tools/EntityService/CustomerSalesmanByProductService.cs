using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerSalesmanByProductService
    {
        bool isNewEntity;
        private int tenant;
        public CustomerSalesmanByProduct Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private CustomerSalesmanByProductPM entityPm;
        private ICommonDataContext objectContext;
        private CustomerSalesmanByProductRepository entityRepository;

        public CustomerSalesmanByProductService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new CustomerSalesmanByProductRepository(objectContext);
        }

        public void Create(CustomerSalesmanByProductPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            this.Poco = new CustomerSalesmanByProduct();

            CustomerSalesmanByProductMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(CustomerSalesmanByProductPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleCustomerSalesmanByProduct(entityPM.ProductTypeCode, entityPm.CustomerId, entityPm.Tenant);
            CustomerSalesmanByProductMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

    }
}
