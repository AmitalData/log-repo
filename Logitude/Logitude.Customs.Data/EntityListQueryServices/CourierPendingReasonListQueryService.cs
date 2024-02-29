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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CourierPendingReasonListQueryService
    {
	    private IQueryable<CourierPendingReasonList> GetIqueryableList(IQueryable<CourierPendingReason> iQueryable)
        {
		IQueryable<CourierPendingReasonList> query = (from a in iQueryable.Include("PendingErrorPlace")
                                                      select new CourierPendingReasonList()
											{
														  Id= a.Id,
					                            Code = a.Code,
					                            LocalName = a.LocalName,
					                            EnglishName = a.EnglishName,
					                            SearchFields = a.SearchFields,
					                            Inactive = a.Inactive,
                                                ErrorPlace = a.ErrorPlace,
                                                ErrorPlaceName = a.PendingErrorPlace != null ? a.PendingErrorPlace.LocalName : null,
                                                Tenant = a.Tenant,
                                                UnifreightStatusCode = a.UnifreightStatusCode,
												MamanSuspendedCode=a.MamanSuspendedCode,
												RequiresApproval = a.RequiresApproval,
												SwissportSuspendedCode=a.SwissportSuspendedCode,
												RequiresPayment = a.RequiresPayment,	
												OverseasSuspendedCode=a.OverseasSuspendedCode,
		                    	            });
            return query;
		}


		private IQueryable<CourierPendingReason> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CourierPendingReason> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public CourierPendingReasonList GetSingleByCode(string code, int tenant)
        {
            IQueryable<CourierPendingReason> CourierPendingReasonQuery = (from a in context.CourierPendingReasons
                                                                          where a.Code == code && a.Tenant == tenant
                                                                          select a);


            IQueryable<CourierPendingReasonList> CourierPendingReasonListQuery = GetIqueryableList(CourierPendingReasonQuery);
            CourierPendingReasonList CourierPendingReasonList = CourierPendingReasonListQuery.FirstOrDefault();
            return CourierPendingReasonList;

        }
    }


}
	