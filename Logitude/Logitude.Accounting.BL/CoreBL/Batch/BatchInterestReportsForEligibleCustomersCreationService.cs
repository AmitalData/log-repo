using Logitude.Accounting.BL.CoreBL.InterestReport;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchInterestReportsForEligibleCustomersCreationService : BatchTaskExecutionsService
    {
        private BatchTaskExecutionPM BatchTaskExecution;
        public BatchInterestReportsForEligibleCustomersCreationService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            BatchTaskExecution = batchTaskExecution;
        }

        public override void RunCode() 
        {
            InterestReportsCreationForCustomersBatchArgs interestReportsCreationForCustomersBatchArgs = GetInterestReportsCreationForCustomersArgs();

            IInterestReportsCreationForCustomerDataPreparation interestReportsCreationForCustomerDataPreparation =
                new InterestReportsCreationForCustomerDataPreparation(interestReportsCreationForCustomersBatchArgs);

            InterestReportsCreationForCustomersArgs interestReportsCreationForCustomersArgs = new InterestReportsCreationForCustomersArgs();
            interestReportsCreationForCustomersArgs.InterestReportsCreationForCustomerDataPreparation = interestReportsCreationForCustomerDataPreparation;
            interestReportsCreationForCustomersArgs.Tenant = interestReportsCreationForCustomersBatchArgs.Tenant;
            interestReportsCreationForCustomersArgs.InterestCalculationDate = interestReportsCreationForCustomersBatchArgs.InterestCalculationDate;
            interestReportsCreationForCustomersArgs.Email= interestReportsCreationForCustomersBatchArgs.Email;

            InterestReportsCreationForCustomersService interestReportsCreationForCustomersService = new InterestReportsCreationForCustomersService(interestReportsCreationForCustomersArgs);
            BatchTaskExecution.ErrorLog = interestReportsCreationForCustomersService.CreateReportsIfNotExistAndCalculateReportsDataForCustomers();
            
        }

        private InterestReportsCreationForCustomersBatchArgs GetInterestReportsCreationForCustomersArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(InterestReportsCreationForCustomersBatchArgs));
            InterestReportsCreationForCustomersBatchArgs interestReportsCreationForCustomersBatchArgs = serializer.Deserialize(stringReader) as InterestReportsCreationForCustomersBatchArgs;
            return interestReportsCreationForCustomersBatchArgs;
        }
    }
}
