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
        public BatchInterestReportsForEligibleCustomersCreationService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode() 
        {
            InterestReportsCreationForCustomersArgs interestReportsCreationForCustomersArgs = GetInterestReportsCreationForCustomersArgs();
            IInterestReportsCreationForCustomerDataPreparation interestReportsCreationForCustomerDataPreparation =
                new InterestReportsCreationForCustomerDataPreparation(interestReportsCreationForCustomersArgs.InterestCalculationDate, interestReportsCreationForCustomersArgs.Tenant);
            interestReportsCreationForCustomersArgs.InterestReportsCreationForCustomerDataPreparation = interestReportsCreationForCustomerDataPreparation;
            InterestReportsCreationForCustomersService interestReportsCreationForCustomersService = new InterestReportsCreationForCustomersService(interestReportsCreationForCustomersArgs);
            interestReportsCreationForCustomersService.CreateReportsIfNotExistAndCalculateReportsDataForCustomers();
        }

        private InterestReportsCreationForCustomersArgs GetInterestReportsCreationForCustomersArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(InterestReportsCreationForCustomersArgs));
            InterestReportsCreationForCustomersArgs interestReportsCreationForCustomersArgs = serializer.Deserialize(stringReader) as InterestReportsCreationForCustomersArgs;
            return interestReportsCreationForCustomersArgs;
        }
    }
}
