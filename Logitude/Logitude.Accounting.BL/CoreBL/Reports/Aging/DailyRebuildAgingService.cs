using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reports.Aging
{
    class DailyRebuildAgingService
    {
        public void RunAllAgingTenants()
        {

            var tenantRepository = new TenantRepository(0);
            var tenantList = tenantRepository.GetAccountingActivatedTenants();

            foreach (var tenant in tenantList)
            {
                LogMessagingUtil.Instance.Clear();
                try
                {
                    this.ReBuild(tenant);
                    //ExceptionHandler.HandleException(new Exception(LogMessagingUtil.Instance.ToString()), DateTime.Now, 0, "", "DailyRebuildAgingService" + this.GetType().Name, " : ReBuild()", null);
                }
                catch (Exception e)
                {

                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "DailyRebuildAgingService" + this.GetType().Name, " : Run() Method", null);
                    //Thread.Sleep(TimeSpan.FromSeconds(5));
                }

            }
        }

        public void ReBuild(int tenant)
        {
            RebuildAging4AccountTypeCode(tenant, "Customer2", null);
            RebuildAging4AccountTypeCode(tenant, "Vendor3", null);
        }

        public void ReBuildByType(int tenant, string type)
        {
            RebuildAging4AccountTypeCode(tenant, type, null);
        }

        public List<GLAccountAgingDataPM> RebuildAging4AccountTypeCode(int tenant, string aging4AccountTypeCode, string MyGLAccId)
        {
            var agingReportRebulidService = new AgingReportRebulidService(new AgingReportRebulidParam()
            {
                Tenant = tenant,

                NumberOfmonthsbackwards = 6,
                AgingForDate = DateTime.Now.Date,
                VendorCustomerId = MyGLAccId,
                Aging4AccountTypeCode = aging4AccountTypeCode == "Vendor3" ? AgingReportParam.Aging4AccountTypeCodeEnum.Vendor3 : AgingReportParam.Aging4AccountTypeCodeEnum.Customer2,




            });
            var stopwatch = Stopwatch.StartNew();

            agingReportRebulidService.RunReport();
            LogMessagingUtil.Instance.AppendLine($"RunReport took:{stopwatch.Elapsed}");
            LogMessagingUtil.Instance.AppendLine($"Account.count {agingReportRebulidService.MyPeriodList.Count()}");
            stopwatch.Restart();
            var res = agingReportRebulidService.RebuildGLAccountAgingData();
            LogMessagingUtil.Instance.AppendLine($"update diff took:{stopwatch.Elapsed}");
            return res;
        }
    }
}
