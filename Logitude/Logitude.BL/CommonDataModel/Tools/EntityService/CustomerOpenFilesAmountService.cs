using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
  public  class CustomerOpenFilesAmountService
    {
        bool isNewEntity;
        private int tenant;
        public CustomerOpenFilesAmount customerOpenFilesAmount { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ICommonDataContext objectContext;
        private CustomerOpenFilesAmountRepository customerOpenFilesAmountRepository;
        public CustomerOpenFilesAmountService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.customerOpenFilesAmountRepository = new CustomerOpenFilesAmountRepository(objectContext);
        }

        public void Create(CustomerOpenFilesAmountPM customerOpenFilesAmountPM)
        {
            this.isNewEntity = true;
           
            this.customerOpenFilesAmount = new CustomerOpenFilesAmount();
          
            CustomerOpenFilesAmountMapping.MapEntity(customerOpenFilesAmountPM, customerOpenFilesAmount, isNewEntity);
            customerOpenFilesAmountRepository.Add(customerOpenFilesAmount);
            customerOpenFilesAmountRepository.SubmitChanges();
        }


        public void Update(CustomerOpenFilesAmountPM customerOpenFilesAmountPM)
        {
            this.isNewEntity = false;
           
            this.customerOpenFilesAmount = customerOpenFilesAmountRepository.GetSingleCustomerOpenFilesAmount(customerOpenFilesAmountPM.CustomerId, customerOpenFilesAmountPM.Tenant);
            
            CustomerOpenFilesAmountMapping.MapEntity(customerOpenFilesAmountPM, customerOpenFilesAmount, isNewEntity);
            customerOpenFilesAmountRepository.Update(customerOpenFilesAmount);
            customerOpenFilesAmountRepository.SubmitChanges();
           
        }
    }
}
