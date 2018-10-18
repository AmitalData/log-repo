using System;
using System.Web;
using System.Web.Services;

using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.Helpers;

using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for PerformanceLogService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PerformanceLogService : System.Web.Services.WebService
    {

        [WebMethod]
        public void InsertPerformanceLog(int tenant, string email, string modelName, string methodName,string methodParameters, bool monitoringService, int executionTime)
        {


           PerformanceLogRepository performanceLogsRepository = new PerformanceLogRepository();
            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            if (string.IsNullOrEmpty(currentIP))
            {
                currentIP = HttpContext.Current.Request.UserHostAddress;
            }
            PerformanceLog performanceLog = new PerformanceLog()
            {
                Id = Guid.NewGuid().ToString(),
                Tenant = tenant,
                Email = email,
                ModelName = modelName,
                MethodName = methodName,
                MethodParameters = methodParameters,
                MonitoringService = monitoringService,
                ExecutionTime = executionTime,
                LogDateTimeGMT = DateTime.Now,
                LogDateTimeLocal = TenantServerConfigration.GetCurrentDateTime(tenant),
                UserIP = currentIP,
            };


            performanceLogsRepository.Add(performanceLog);
            performanceLogsRepository.SubmitChanges();

        //      public string Id { get; set; }
        //public DateTime LogDateTimeGMT { get; set; }
        //public DateTime LogDateTimeLocal { get; set; }
        //public string Email { get; set; }
        //public string ModelName { get; set; }
        //public string MethodName { get; set; }
        //public bool MonitoringService { get; set; }
        //public int ExecutionTime { get; set; }
        //public string UserIP { get; set; }
        //public string MethodParameters { get; set; }
        //public int Tenant { get; set; }
        }
    }
}
