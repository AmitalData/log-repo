using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CustomerStatusLogsService
    {
        CustomerTenantAccessCardRepository customerTenantAccessCardRepository;
        public Customer customer;
        private CustomerPM customerPM;
        public CustomerStatusLogsService(CustomerTenantAccessCardRepository customerTenantAccessCardRepository, Customer customer,CustomerPM customerPM)
        { 
            this.customerTenantAccessCardRepository = customerTenantAccessCardRepository;
            this.customer = customer;
            this.customerPM = customerPM;
        }
        public List<QueueTask> CreateCustomerStatusLogs()
        {
            CustomerTenantAccessCard customerTenantAccessCards = customerTenantAccessCardRepository.GetByCustomerId(customerPM.Id, customerPM.Tenant);
            return GetCustomerStatusLogsParameters(customerTenantAccessCards);
        }
        private List<QueueTask> GetCustomerStatusLogsParameters(CustomerTenantAccessCard customerTenantAccessCards)
        {

            List<QueueTask> tasks = new List<QueueTask>();

            string action = GetCustomerStatusLogsAction(customerPM);

            tasks.Add(new QueueTask()
            {
                Action = action,
                Parameters = new List<Parameter>() {
                new Parameter { Order = 1, Value = customerPM.Code },
                new Parameter { Order = 2, Value = customerPM.LogBoxActivated.ToString() },
                new Parameter { Order = 3, Value = customerPM.IsPrivateLabelCustomer.ToString() },
                new Parameter { Order = 4, Value = customerPM.CustomerTenant.ToString() },
                new Parameter { Order = 5, Value = customerTenantAccessCards?.IsExportActivated.ToString() },
                new Parameter { Order = 6, Value = customerTenantAccessCards?.IsCustomsActivated.ToString() },
            }
            });

            return tasks;
        }

        private string GetCustomerStatusLogsAction(CustomerPM entityPM)
        {
            if (entityPM.IsPrivateLabelCustomer == true || customer.IsPrivateLabelCustomer == true)
            {
                return "Customer.PrivateLabel";
            }
            return "Customer.LogBoxActivated";
        }
         
    }
}