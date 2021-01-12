using Logitude.BL.InfrastructureModel.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.ReportScheduler
{
    public interface IReportSchedulerValidator
    {
        ValidateResult validate();
    }
    public class ValidateResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class AdditionalValidate
    {
        public ReportSchedulerRecepients recepients { get; set; }
    }
}
