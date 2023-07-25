using Devart.Data.Oracle;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Utils;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvioceExportDefaultUpdateService
    {

        protected override void OnCreating(SupplierInvioceExportDefaultPM entityPM, Server.Tools.EntityPM entityParentPM)
        {

            entityPM.Id = IdCounter.GetNumber("Customs.SupplierInvioceExportDefault", entityPM.Tenant);
            base.OnCreating(entityPM, entityParentPM);
        }



    }
}
