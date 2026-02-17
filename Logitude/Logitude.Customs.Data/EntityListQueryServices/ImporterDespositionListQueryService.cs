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

    public partial class ImporterDespositionListQueryService
    {
	    private IQueryable<ImporterDespositionList> GetIqueryableList(IQueryable<ImporterDesposition> iQueryable)
        {
            IQueryable<ImporterDespositionList> query = (from a in iQueryable.Include("ImporterPeriodicDeclarStatus")
                                                         select new ImporterDespositionList()
                                                                       {
                                                                           Id = a.Id, 
                                                                           Tenant = a.Tenant,
                                                                          DepositionNumber = a.DepositionNumber,
                                                                          EndDate = a.EndDate,
                                                                          ErrorMessage = a.ErrorMessage,
                                                                          ImporterDepositionStatusCode = a.ImporterDepositionStatusCode,
                                                                          ImporterlId = a.ImporterlId,
                                                                          NotesToAgent = a.NotesToAgent,
                                                                          StartDate = a.StartDate,
                                                                          VendorID = a.VendorID,
                                                                          ImporterDepositionStatusName = a.ImporterPeriodicDeclarStatus != null? a.ImporterPeriodicDeclarStatus.LocalName : null,


                                                                       });
            return query;
		}

        private IQueryable<ImporterDesposition> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ImporterDesposition> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	