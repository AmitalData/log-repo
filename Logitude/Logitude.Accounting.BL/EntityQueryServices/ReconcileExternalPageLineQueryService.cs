 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class ReconcileExternalPageLineQueryService
    {
        public List<ReconcileExternalPageLinePM> GetPageLinesPMsByIdList(List<string> pageLinesIdList, int tenant)
        {
            List<ReconcileExternalPageLine> ledgerTransactionPOCOs = null;
            ledgerTransactionPOCOs = repository.GetPageLinesByIdList(pageLinesIdList, tenant);
            List<ReconcileExternalPageLinePM> pms = ledgerTransactionPOCOs.Select(poco => GetEntityPM(poco)).ToList();
            return pms;
        }
    }

}
	 