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

    public partial class SupplierInvioceExportDefaultListQueryService
    {
	    private IQueryable<SupplierInvioceExportDefaultList> GetIqueryableList(IQueryable<SupplierInvioceExportDefault> iQueryable)
        {
		IQueryable<SupplierInvioceExportDefaultList> query = (from a in iQueryable
                                            select new SupplierInvioceExportDefaultList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          PartyRelationshipCode = a.PartyRelationshipCode,
					
					                          BuyerRoleCode = a.BuyerRoleCode,
					
					                          ProcessTypeCode = a.ProcessTypeCode,
					
					                          TransactionNatureCode = a.TransactionNatureCode,
					
					                          ClaimReasonCode = a.ClaimReasonCode,
					
		                    	            });
            return query;
		}

		private IQueryable<SupplierInvioceExportDefault> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SupplierInvioceExportDefault> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	