	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{

    public partial class BatchTaskExecutionListQueryService
    {
        private IQueryable<BatchTaskExecutionList> GetIqueryableList(IQueryable<BatchTaskExecution> iQueryable)
        {
            IQueryable<BatchTaskExecutionList> query = (from a in iQueryable
                                                        select new BatchTaskExecutionList()
                                                        {

                                                            Id = a.Id,

                                                            Tenant = a.Tenant,

                                                            CreateDate = a.CreateDate,

                                                            CreatedByUserId = a.CreatedByUserId,

                                                            SearchFields = a.SearchFields,

                                                            ClassName = a.ClassName,

                                                            DoneDateTime = a.DoneDateTime,

                                                            ErrorLog = a.ErrorLog,

                                                            PrametersXml = a.PrametersXml,

                                                            ProgressMessage = a.ProgressMessage,

                                                            ProgressPercentage = a.ProgressPercentage,

                                                            StartDateTime = a.StartDateTime,

                                                            StatusCode = a.StatusCode,

                                                            StatusName = a.BatchTaskExecutionStatus.Name,

                                                            CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                            
                                                            Subject = a.Subject,

                                                        });
            return query;
        }

        private IQueryable<BatchTaskExecution> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<BatchTaskExecution> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<BatchTaskExecution> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<BatchTaskExecution> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	