using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
        private  GLAccountCardDataService gLAccountCardDataService;
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
            UpdateGLaccountCardsData(customerOpenFilesAmountPM);


        }


        public void Update(CustomerOpenFilesAmountPM customerOpenFilesAmountPM)
        {
            this.isNewEntity = false;
           
            this.customerOpenFilesAmount = customerOpenFilesAmountRepository.GetSingleCustomerOpenFilesAmount(customerOpenFilesAmountPM.CustomerId, customerOpenFilesAmountPM.Tenant);
            
            CustomerOpenFilesAmountMapping.MapEntity(customerOpenFilesAmountPM, customerOpenFilesAmount, isNewEntity);
            customerOpenFilesAmountRepository.Update(customerOpenFilesAmount);
            customerOpenFilesAmountRepository.SubmitChanges();
            UpdateGLaccountCardsData(customerOpenFilesAmountPM);


        }
        private  void UpdateGLaccountCardsData(CustomerOpenFilesAmountPM customerOpenFilesAmountPM)
        {
            CustomerPM customer = GetCustomer(customerOpenFilesAmountPM);
            gLAccountCardDataService = new GLAccountCardDataService(customer.Id, customer.GLAccountId, customer.Tenant);
            bool GlAccountCardDataExists = CheckIfGlAccountCardDataExists();
            if (GlAccountCardDataExists)
            {
                gLAccountCardDataService.UpdateGLaccountCardsData();
            }
        }
        private  bool CheckIfGlAccountCardDataExists()
        {
            if (gLAccountCardDataService.gLAccountCardsDataPM == null)
            {
                return false;
            }
            else return true;
        }
        private  CustomerPM GetCustomer(CustomerOpenFilesAmountPM customerOpenFilesAmountPM)
        {
            CustomerQuery customerQuery = new CustomerQuery(customerOpenFilesAmountPM.Tenant);
            return customerQuery.GetSinglePM(customerOpenFilesAmountPM.CustomerId, customerOpenFilesAmountPM.Tenant);
        }
    }
}
