using Logitude.Accounting.Data;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchYearlyFIXService : BatchTaskExecutionsService
    {
        public BatchYearlyFIXService(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution) { }

        public void TestIt(BatchTaskExecutionPM batchTaskExecution)
        {
           
            base.BatchTaskExecution=batchTaskExecution;
            RunCode();
        }
        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            var serializer = new XmlSerializer(typeof(BatchYearlyFIXParams));
            var parameterArgs = serializer.Deserialize(stringReader) as BatchYearlyFIXParams;
            var accountingContext = AccountingContext.GetContext(parameterArgs.Tenant);
            try
            {
                int maxMonth = 12;
                if (false && parameterArgs.Year == DateTime.Now.Year)
                {
                    maxMonth = DateTime.Now.Month;
                }
                for (int month = 1; month <= maxMonth; month++)
                {
                    DateTime dateTime = new DateTime(parameterArgs.Year, month, 1);
                    switch (parameterArgs.MyFixType)
                    {
                        case "ReverseEngineerTotalByMonthServiceControl":
                            {
                                try
                                {
                                    var s = new ReverseEngineerTotalByMonth_ControlAccountService(dateTime, parameterArgs.Tenant, null);
                                    s.FixDbIntegrityFromLedgeToTotal(/*param.ChangeSupplier2Customer*/);
                                }
                                catch (Exception E) when (E.Message == ReverseEngineerTotalByMonth_ControlAccountService.const_isokNothingDone)
                                {

                                    Debug.WriteLine("const_isokNothingDone");
                                    //throw;
                                }
                                break;
                            }
                        case "ReverseEngineerTotalByMonthService":
                            {
                                try
                                {
                                    var s = new ReverseEngineerTotalByMonthService(dateTime, parameterArgs.Tenant, null);
                                    s.FixDbIntegrityFromLedgeToTotal();
                                }
                                catch (Exception E) when (E.Message == ReverseEngineerTotalByMonth_ControlAccountService.const_isokNothingDone)
                                {

                                    Debug.WriteLine("const_isokNothingDone");
                                    //throw;
                                }
                                break;
                            }
                            default:
                            {
                                throw new ArgumentOutOfRangeException("");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "BatchYearlyFIXService", new DateTime(2021, 7, 1));
                throw;

            }

        }
    }

    public class BatchYearlyFIXParams
    {
        public int Tenant { get; set; }
        public int Year { get; set; }

        public string MyFixType { get; set; }
        public string UserId { get; set; }
        public enum FixType
        {
            ReverseEngineerTotalByMonthService
        }
    }

}
