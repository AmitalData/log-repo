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

    public partial class DeclarationStatusListQueryService
    {
	    private IQueryable<DeclarationStatusList> GetIqueryableList(IQueryable<DeclarationStatus> iQueryable)
        {
		IQueryable<DeclarationStatusList> query = (from a in iQueryable
                                            select new DeclarationStatusList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          FieldC1 = a.FieldC1,
					
					                          FieldC2 = a.FieldC2,
					
					                          FieldC3 = a.FieldC3,
					
					                          FieldC4 = a.FieldC4,
					
					                          FieldC5 = a.FieldC5,
					
					                          FieldC6 = a.FieldC6,
					
					                          FieldC7 = a.FieldC7,
					
					                          FieldC8 = a.FieldC8,
					
					                          FieldC9 = a.FieldC9,
					
					                          FieldC10 = a.FieldC10,
					
					                          FieldC11 = a.FieldC11,
					
					                          FieldC12 = a.FieldC12,
					
					                          FieldC13 = a.FieldC13,
					
					                          FieldD1 = a.FieldD1,
					
					                          FieldD2 = a.FieldD2,
					
					                          FieldD3 = a.FieldD3,
					
					                          FieldD4 = a.FieldD4,
					
					                          FieldD5 = a.FieldD5,
					
					                          FieldR1 = a.FieldR1,
					
					                          FieldR2 = a.FieldR2,
					
					                          FieldR3 = a.FieldR3,
					
					                          FieldR4 = a.FieldR4,
					
					                          FieldR5 = a.FieldR5,
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationStatus> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	