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

    public partial class ClaimImporterDeclarsP3LoiListQueryService
    {
	    private IQueryable<ClaimImporterDeclarsP3LoiList> GetIqueryableList(IQueryable<ClaimImporterDeclarsP3Loi> iQueryable)
        {
		    IQueryable<ClaimImporterDeclarsP3LoiList> query = (from a in iQueryable
                                                select new ClaimImporterDeclarsP3LoiList()
											    {
                                                    ClaimId = a.ClaimId,
                                                    Tenant = a.Tenant,
                                                    CounterKey = a.CounterKey,
                                                    LineNo = a.LineNo,
                                                    //ImporterLoiDeclarationTypeCode = a.ImporterLoiDeclarationTypeCode,
                                                    DeclarationNumber = a.DeclarationNumber,
		                    	                });
            return query;
		}

		private IQueryable<ClaimImporterDeclarsP3Loi> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ClaimImporterDeclarsP3Loi> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	