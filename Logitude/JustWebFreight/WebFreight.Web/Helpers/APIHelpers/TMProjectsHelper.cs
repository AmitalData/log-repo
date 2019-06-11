using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
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
    public class TMProjectsHelper : BatchTaskExecutionsService
    {
        public TMProjectsHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(TMProjectDataArgs));
            TMProjectDataArgs args = serializer.Deserialize(stringReader) as TMProjectDataArgs;

            if (args != null)
            {
                ITimeManagementContext myContext = TimeManagementContext.GetContext(args.Tenant);
                TMEmployeeTimeRepository iTMEmployeeTimeRepository = new TMEmployeeTimeRepository(myContext);
                TMProjectRepository iTMProjectRepository = new TMProjectRepository(myContext);
                IQueryable<TMEmployeeTime> iQueryable = iTMEmployeeTimeRepository.GetAll(args.Tenant);
                List<TMEmployeeTime> employeeTimes = this.GetFilteredList(iQueryable, args).ToList();

                if (employeeTimes.Count > 0)
                {
                    TFSParseWebhook webhook = new TFSParseWebhook();

                    int count = 0;
                    foreach (TMEmployeeTime item in employeeTimes)
                    {
                        int iWorkItemNumber;

                        if (Int32.TryParse(item.WINumber, out iWorkItemNumber))
                        {
                            string iProjectNumber = webhook.GetWorkItemById(iWorkItemNumber, false);
                            if (!string.IsNullOrEmpty(iProjectNumber))
                            {
                                string iProjectId = iTMProjectRepository.GetTMActiveProjectByNumber(iProjectNumber, args.Tenant);

                                if (!string.IsNullOrEmpty(iProjectId))
                                {
                                    if (item.ProjectId != iProjectId)
                                    {
                                        item.ProjectId = iProjectId;
                                        iTMEmployeeTimeRepository.Update(item);
                                        count++;
                                    }
                                }
                            }
                        }

                        if (count >= 100)
                        {
                            count = 0;
                            iTMEmployeeTimeRepository.SubmitChanges();
                        }
                    }

                    if (count > 0)
                    {
                        iTMEmployeeTimeRepository.SubmitChanges();
                    }
                }
            }
        }

        private IQueryable<TMEmployeeTime> GetFilteredList(IQueryable<TMEmployeeTime> iQueryable, TMProjectDataArgs args)
        {
            iQueryable = iQueryable.Where(d => d.DateOfWork != null && d.WINumber != null);

            if (!string.IsNullOrEmpty(args.EmployeeUserId))
            {
                iQueryable = iQueryable.Where(d => d.EmployeeUserId == args.EmployeeUserId);
            }

            if (args.FromDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(args.FromDate));
            }

            if (args.ToDate != null)
            {
                iQueryable = iQueryable.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(args.ToDate));
            }

            return iQueryable;
        }
    }
}