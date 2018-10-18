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

    public partial class DecCargoSplitConListQueryService
    {
        private IQueryable<DecCargoSplitConList> GetIqueryableList(IQueryable<DecCargoSplitCon> iQueryable)
        {
            IQueryable<DecCargoSplitConList> query = (from a in iQueryable.Include("TreatmentWay").Include("GovernmentProcedureType")
                                                      select new DecCargoSplitConList()
                                                      {
                                                          DeclarationCargoSplitId = a.DeclarationCargoSplitId,
                                                          LineNumber = a.LineNumber,
                                                          ImporterCode = a.ImporterCode,
                                                          ConditionCode = a.ConditionCode,
                                                          ConditionName = a.TreatmentWay.LocalName,
                                                          ProcedureCurrentCode = a.ProcedureCurrentCode,
                                                          ProcedureCurrentName = a.GovernmentProcedureCurrent.LocalName,
                                                      });
            return query;
        }

        private IQueryable<DecCargoSplitCon> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DecCargoSplitCon> iQueryable, int tenant)
        {
            return iQueryable;
        }
			}


}
	