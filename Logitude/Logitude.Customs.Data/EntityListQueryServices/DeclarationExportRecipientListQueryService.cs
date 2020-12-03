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

    public partial class DeclarationExportRecipientListQueryService
    {
	    private IQueryable<DeclarationExportRecipientList> GetIqueryableList(IQueryable<DeclarationExportRecipient> iQueryable)
        {
		IQueryable<DeclarationExportRecipientList> query = (from a in iQueryable
                                            select new DeclarationExportRecipientList()
											{
                     
					                          RecipientName = a.RecipientName,
					
					                          RecipientAddress = a.RecipientAddress,
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationExportRecipient> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationExportRecipient> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	