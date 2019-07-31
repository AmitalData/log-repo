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
using Logitude.TimeManagement.BL.EntityPMs;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class TMMBProjectsHelper : BatchTaskExecutionsService
    {
        public TMMBProjectsHelper(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(TMMBProjectDataArgs));
            TMMBProjectDataArgs args = serializer.Deserialize(stringReader) as TMMBProjectDataArgs;

            if (args != null)
            {
                ITimeManagementContext myContext = TimeManagementContext.GetContext(args.Tenant);
                TMEmployeeTimeRepository iTMEmployeeTimeRepository = new TMEmployeeTimeRepository(myContext);
                TMProjectRepository iTMProjectRepository = new TMProjectRepository(myContext);
                IQueryable<TMEmployeeTime> iQueryable = iTMEmployeeTimeRepository.GetAll(args.Tenant).Where(d=>d.ProjectId == args.FromProject) ;
                List<TMEmployeeTime> employeeTimes = this.GetFilteredList(iQueryable, args).ToList();


                if (employeeTimes.Count > 0)
                {
                    int count = 0;
                    foreach (TMEmployeeTime item in employeeTimes)
                    {
                        item.ProjectId = args.ToProject;
                        item.NeedsProrating = true;
                        iTMEmployeeTimeRepository.Update(item);
                        count += 1;

                        if(count == 20 || (count == employeeTimes.Count()) || (employeeTimes.IndexOf(item)== employeeTimes.IndexOf(employeeTimes.Last())))
                        {
                            iTMEmployeeTimeRepository.SubmitChanges();
                            count = 0;
                        }
                    }

                }

                    
            }
        }

        private IQueryable<TMEmployeeTime> GetFilteredList(IQueryable<TMEmployeeTime> iQueryable, TMMBProjectDataArgs args)
        {
            if (!string.IsNullOrEmpty(args.EmployeeUserId))
            {
                iQueryable = iQueryable.Where(d => d.EmployeeUserId == args.EmployeeUserId);
            }
            if (args.FromDate != null && args.ToDate != null)
            {
                iQueryable = iQueryable.Where(d => (d.DateOfWork >= args.FromDate) && (d.DateOfWork <= args.ToDate));
            }
            else if (args.FromDate != null) {
                iQueryable = iQueryable.Where(d => (d.DateOfWork >= args.FromDate));
            }

            else if (args.ToDate != null)
            {
                iQueryable = iQueryable.Where(d => (d.DateOfWork <= args.ToDate));
            }


            return iQueryable;
        }
    }
}