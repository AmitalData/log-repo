using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityDataMappings;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Configuration;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.Repsitories;

namespace Unifreight.BL.EntityUpdateServices
{
    public class GGGQUpdateService : EntityUpdateService<GGGQ, GGGQPM, EntityPM>
    {      
        public GGGQUpdateService(AmitalContext context)
        {
            MainContext = context;
            Repository = new GGGQRepository(context);

            Mapping = new GGGQDataMapping();
            AdditionalContexts = new Dictionary<string, Simplog.Server.Infrastructure.IContext>();
        }
        
        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(GGGQPM entityPM)
        {
            return new GGGQKeys() { QUEID = entityPM.QUEID };
        }

        protected override void OnCreating(GGGQPM entityPM, EntityPM entityParentPM)
        {
            if (IsGGGQExist(entityPM))
            {
                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.None;
                return;
            }

            entityPM.CREATEDATE = DateTime.Now;
            if (string.IsNullOrWhiteSpace(entityPM.QUEID))
            {
                entityPM.QUEID = CommCounterUtil.GetUnique30(entityPM.CREATEDATE);
            }
            ///entityPM.COMPUTERID = Environment.MachineName;            
        }

        private bool IsGGGQExist(GGGQPM entityPM)
        {
            string val = "";
            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["20220216.CheckIfGGGQExist"]))
            {
                val = ConfigurationManager.AppSettings["20220216.CheckIfGGGQExist"].ToString();
            }
            DateTime stopLogAt = new DateTime(2022, 06, 01);
            string logData = "";
            logData = $"entityPM.PRIMARYNUM={entityPM.PRIMARYNUM}, ConfigurationManager.AppSettings[20220216.CheckIfGGGQExist]={val}, before check";
            LogitudeSettings.HandleLogMe("Check IsGGGQExist " + logData, false, "IsGGGQExist", stopLogAt);
            if (val != "1") return false;

            var context = this.MainContext as AmitalContext;
            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            var myGGGQQueryService = new GGGQQueryService(context);
            GGGQPM ExistGGGQPM = myGGGQQueryService.GetByPrimary(entityPM.PRIMARYNUM, entityPM.ENTNAME, entityPM.ORIGINQUE, entityPM.FORMID, entityPM.STATUS);
            if (ExistGGGQPM != null && !string.IsNullOrWhiteSpace(ExistGGGQPM.QUEID))
            {
                logData = $"entityPM.PRIMARYNUM={entityPM.PRIMARYNUM}, ExistGGGQPM.QUEID={ExistGGGQPM.QUEID}, queue found ";
                LogitudeSettings.HandleLogMe("Check IsGGGQExist " + logData, false, "IsGGGQExist", stopLogAt);
                return true;
            }
            return false;
        }
        protected override void OnUpdating(GGGQPM entityPM)
        {
            if (entityPM.Tenant != 0)
            {
                CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(entityPM.Tenant);
                bool isConnectedToUnifreight = custSettingsRepo.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight;
                if (!isConnectedToUnifreight)
                {
                    entityPM.IS_SYNCH = false;
                    entityPM.LAST_UPDATE_DT = DateTime.Now;
                }
            }

        }

        protected override void UpdateComposition(GGGQPM entityPM)
        {
            var myGGGQCUpdateService = new GGGQCUpdateService(this.MainContext as AmitalContext);
            myGGGQCUpdateService.UpdateMulti(entityPM.GGGQCPMs, new List<GGGQCPM>(), entityPM, true);
        }
    }
}
