using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Utils
{
    public class GrantCCUTableUtil
    {
        public static string GrantCCUTo(int tenant, Func<string> GetConnetionStringFunc)
        {
            if (LogitudeSettings.DatabaseManagementSystem != "oracle")
            {
                return "";
            }
            var pm = CustomsSettingQueryService.GetSettingByTenant(tenant);
            if (pm != null && !String.IsNullOrWhiteSpace(pm.UnfConnectionString)) //pm.IsConnectedToUniFreight)
            {

                var mainConnectionString = (Logitude.Customs.Data.CustomContext.GetContext(tenant) as DbContext).Database.Connection.ConnectionString;

                var c = AmitalContext.GetContext(tenant);
                string mainServer = ""; string mainUserId = "";

                var t = typeof(Unifreight.Data.AmitalModel.EntityPOCOs.ATBPTIL);
                Type type = Type.GetType("Unifreight.Data.AmitalModel.EntityPOCOs.ATBPTIL");

                type = t.Assembly.GetType("Unifreight.Data.AmitalModel.EntityPOCOs.ATBPTIL");
                //Unifreight.Data.AmitalModel.EntityPOCOs.ATBPTIL
                //Unifreight.Data.AmitalModel.EntityPOCOs
                var unifreightTables = c.GetTableNames("Unifreight.Data.AmitalModel.EntityPOCOs");
                c.ExecuteInSys(mainConnectionString, unifreightTables, GetConnetionStringFunc);
                //var lines = unifreightTables
                //    .Select(tbl => string.Format("GRANT select ,insert ,update ,delete on  {0}  TO {1} ;", tbl, mainUserId))
                //    .ToList();

                //string script = String.Join(Environment.NewLine, lines);

            }

            return "";
        }
    }
}
