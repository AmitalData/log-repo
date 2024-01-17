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

    public partial class CB_RegularityRequiredCertificateListQueryService
    {
	    private IQueryable<CB_RegularityRequiredCertificateList> GetIqueryableList(IQueryable<CB_RegularityRequiredCertificate> iQueryable)
        {
		IQueryable<CB_RegularityRequiredCertificateList> query = (from a in iQueryable
                                            select new CB_RegularityRequiredCertificateList()
											{
                     
					                          ID = a.ID,
					
					                          RegularityInceptionID = a.RegularityInceptionID,
					
					                          ConfirmationTypeID = a.ConfirmationTypeID,
					
					                          Number = a.Number,
					
					                          TextualCondition = a.TextualCondition,
					
					                          TrNumber = a.TrNumber,
					
					                          AuthorityID = a.AuthorityID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_RegularityRequiredCertificate> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_RegularityRequiredCertificate> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	