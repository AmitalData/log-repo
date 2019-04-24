using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.CommonDataModel;

namespace CommunicationWorkerRole
{
    public class CustomerActualDataWorkerRole : WorkerEntryPoint
    {
        public void Run_Old()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    int myTenant = 0;

                    try
                    {                        
                        DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 30, 0);
                        DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 40, 0);
                        LastActivity = DateTime.UtcNow;
                        if (DateTime.Now >= date1 && DateTime.Now <= date2)
                        {
                            TenantRepository tenantRepository = new TenantRepository(0);
                            IQueryable<Tenant> allTenants = tenantRepository.GetTenants();
                            LastActivity = DateTime.UtcNow;
                            foreach (Tenant item in allTenants)
                            {
                                myTenant = item.Id;
                                using (TransactionScope scope = TransactionFactory.GetTransaction(new TimeSpan(2,0,0)))
                                {
                                    CommonModelProcedureClass.ExecuteTenantCustomersActualData(item.Id);
                                    scope.Complete();
                                }
                            }
                            LogDoneItemInMemory();
                            Thread.Sleep(3600000); 
                        }

                        else
                        {
                            Thread.Sleep(60000);
                        }
                    }

                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Customer actual data worker role start, Tenant: " + myTenant, null, null);
                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    LastActivity = DateTime.UtcNow;
                    DateTime date1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 30, 0);
                    DateTime date2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 40, 0);

                    if (DateTime.Now >= date1 && DateTime.Now <= date2)
                    {
                        LastActivity = DateTime.UtcNow;
                        ICommonDataContext iContext = CommonDataContext.GetContext(0);
                        List<int> AllTenants = (from d in iContext.Tenants select d.Id).ToList();
                        if (AllTenants != null)
                        {
                            string CustomerId = null;
                            DateTime? StartDateTime = null;
                            DateTime? EndDateTime = null;

                            foreach (int iTenant in AllTenants)
                            {
                                try
                                {
                                    using (TransactionScope scope = TransactionFactory.GetTransaction(new TimeSpan(3, 0, 0)))
                                    {
                                        StartDateTime = DateTime.Now;

                                        bool isExists = CommonModelProcedureClass.IsExistsCustomerActualDataHistory(iTenant, StartDateTime);
                                        if (!isExists)
                                        {
                                            CommonModelProcedureClass.ExecuteSingleCustomerActualData(CustomerId, iTenant);
                                            scope.Complete();
                                            EndDateTime = DateTime.Now;
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    EndDateTime = DateTime.Now;
                                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Customer actual data worker role start, Tenant: " + iTenant, null, null);
                                    Thread.Sleep(10000);
                                }

                                finally
                                {
                                    if (StartDateTime != null && EndDateTime != null)
                                    {
                                        CommonModelProcedureClass.InsertCustomerActualDataHistory(iTenant, StartDateTime, EndDateTime);
                                    }
                                }
                            }

                            LogDoneItemInMemory();
                            Thread.Sleep(3600000);
                        }

                        else
                        {
                            Thread.Sleep(60000);
                        }
                    }

                    else
                    {
                        Thread.Sleep(60000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }


        public override bool OnStart()
        {

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CustomerActualData";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ServicePointManager.DefaultConnectionLimit = 12;
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }

    }
}
