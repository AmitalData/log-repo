using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Helpers;
using System.Diagnostics;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting.Contexts;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Accounting.BL.CloseTables;
using Microsoft.SqlServer.Server;
using System.Transactions;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Accounting.BL.Utils
{
    public class GLAccountRecalculateBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        private bool _retry;
        private bool _errors = false;

        public GLAccountRecalculateBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void RunGLAccountRecalculate(GLAccountRecalculateArg gLAccountRecalculateArg)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                try
                {

                    int tenant = gLAccountRecalculateArg.Tenant;
                    string myGLAccountId = gLAccountRecalculateArg.AccountId;
                    if (gLAccountRecalculateArg.BatchTask != null)
                    {
                        BatchTaskExecutionPM batchTaskExecutionPM = gLAccountRecalculateArg.BatchTask;
                        BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = null;
                        if (batchTaskExecutionPM != null)
                        {
                            batchTaskExecutionUpdateService = GetBatchTaskUpdateServiceInstance(tenant);
                        }
                    }


                    var revengtotalsvc = new ReverseEngineerTotalByMonthService(DateTime.Today, tenant, myGLAccountId);
                    List<Data.Repositories.GLAccountTotalByMonthsDTO> changedList = new List<GLAccountTotalByMonthsDTO>();
                    try
                    {
                        revengtotalsvc.FixDbIntegrityFromLedgeToAllMonths(ref changedList);
                    }
                    catch (Exception ex) 
                    { 
                    }


                    var revengbalancesvc = new ReverseEngineerGLAccountBalance(tenant);
                    try 
                    { 
                        revengbalancesvc.FIXCheckDbIntegrity();
                    }
                    catch (Exception ex)
                    {
                    }

                    string userid = "";
                    if (gLAccountRecalculateArg.BatchTask != null && !String.IsNullOrEmpty(gLAccountRecalculateArg.BatchTask.CreatedByUserId))
                        userid = gLAccountRecalculateArg.BatchTask.CreatedByUserId;
                    else
                    {
                        ContactRepository contactRep = new ContactRepository(tenant);
                        string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
                        Simplog.Data.CommonDataModel.EntityPOCOs.Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, tenant);
                    }

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = myGLAccountId,
                        Tenant = tenant,
                        UserId = userid,
                        ObjectTableName = "GLAccount",
                        IsAddedManually = false,
                        EventTypeCode = "RCLC",
                        Notes = "",
                    });

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }
            }
        }



        private BatchTaskExecutionUpdateService GetBatchTaskUpdateServiceInstance(int tenant)
        {
            IInfrastructureContext context = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService batchTaskExecutionUpdateService = new BatchTaskExecutionUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            return batchTaskExecutionUpdateService;
        }


    }

    public class GLAccountRecalculateArg
    {
        public int Tenant { get; set; }
        public string AccountId { get; set; }
        public bool Batch { get; set; }
        public BatchTaskExecutionPM BatchTask { get; set; }
    }
}