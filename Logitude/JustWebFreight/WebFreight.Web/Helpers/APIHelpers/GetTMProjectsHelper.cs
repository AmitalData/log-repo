using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Controllers.WebDomainControllers;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class GetTMProjectsHelper : BatchTaskExecutionsService
    {
        public GetTMProjectsHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(TMProjectDataArgs));
            TMProjectDataArgs parameterArgs = serializer.Deserialize(stringReader) as TMProjectDataArgs;

            int tenant;
            string employeeUserId = "";
            DateTime? fromDate;
            DateTime? toDate;
            if (parameterArgs != null)
            {
                tenant = parameterArgs.Tenant;
                employeeUserId = parameterArgs.EmployeeUserId;
                fromDate = parameterArgs.FromDate;
                toDate = parameterArgs.ToDate;

                ITimeManagementContext myContext = TimeManagementContext.GetContext(tenant);
                IQueryable<TMEmployeeTime> iQueryable = (from d in myContext.TMEmployeeTimes
                                                         where d.Tenant == tenant && d.DateOfWork != null && d.WINumber != null
                                                         select d);

                List<TMProject> allProjects = (from d in myContext.TMProjects where d.Tenant == tenant select d).ToList();
                if (!string.IsNullOrEmpty(employeeUserId))
                {
                    iQueryable = iQueryable.Where(d => d.EmployeeUserId == employeeUserId);
                }

                if(fromDate != null)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));
                }
                if (toDate != null)
                {
                    iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                }

                TFSParseWebhook myTFSParseWebhook = new TFSParseWebhook();
                List<TMEmployeeTime> projectsList = myTFSParseWebhook.GetProjects(iQueryable.ToList(), tenant);
            }
        }

        public string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }
    }
}