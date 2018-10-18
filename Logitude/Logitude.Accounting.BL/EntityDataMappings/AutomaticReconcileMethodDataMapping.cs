using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class AutomaticReconcileMethodDataMapping: IMapping<AutomaticReconcileMethodPM, AutomaticReconcileMethod>
   {

        public void CustomPMToPOCO(AutomaticReconcileMethodPM entityPM, AutomaticReconcileMethod entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.SearchFields);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            AutomaticReconcileQueryService automaticReconcileQueryService = new AutomaticReconcileQueryService(entityPM.Tenant);

            if (entityPM.AutomaticReconcile1 != null)
            {
                AutomaticReconcilePM automaticReconcile = automaticReconcileQueryService.GetSingle(entityPM.AutomaticReconcile1, false, false);
                entityPM.AutomaticReconcileName1 = automaticReconcile.EnglishName;
                entityPM.Name = automaticReconcile.EnglishName;
                entityPM.LocalName = automaticReconcile.LocalName;
            }

            if (entityPM.AutomaticReconcile2 != null)
            {
                AutomaticReconcilePM automaticReconcile = automaticReconcileQueryService.GetSingle(entityPM.AutomaticReconcile2, false, false);
                entityPM.AutomaticReconcileName2 = automaticReconcile.EnglishName;
                entityPM.Name += "+" + automaticReconcile.EnglishName;
                entityPM.LocalName += " + " + automaticReconcile.LocalName;
            }

            if (entityPM.AutomaticReconcile3 != null)
            {
                AutomaticReconcilePM automaticReconcile = automaticReconcileQueryService.GetSingle(entityPM.AutomaticReconcile3, false, false);
                entityPM.AutomaticReconcileName3 = automaticReconcile.EnglishName;
                if (entityPOCO.AutomaticReconcileField2 != null)
                {
                    entityPM.Name += "+" + automaticReconcile.EnglishName;
                    entityPM.LocalName += " + " + automaticReconcile.LocalName;

                }

            }
            entityPOCO.SearchFields = entityPM.Code + ',' + entityPM.Name + ',' + entityPM.LocalName;
        }

        public void CustomPOCOToPM(AutomaticReconcileMethodPM entityPM, AutomaticReconcileMethod entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.AutomaticReconcileName1);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AutomaticReconcileName2);
            this.CustomMappedPMProperties.Add(PMPropertyNames.AutomaticReconcileName3);


            if (entityPOCO.AutomaticReconcile1 != null)
            {
                AutomaticReconcileQueryService automaticReconcileQueryService = new AutomaticReconcileQueryService(entityPOCO.Tenant);
                AutomaticReconcilePM automaticReconcile = automaticReconcileQueryService.GetSingle(entityPOCO.AutomaticReconcile1, false, true);
                entityPM.AutomaticReconcileName1 = automaticReconcile.EnglishName;
                entityPM.Name = automaticReconcile.EnglishName;
                entityPM.LocalName = automaticReconcile.LocalName;
            }

            if (entityPOCO.AutomaticReconcile2 != null)
            {
                AutomaticReconcileQueryService automaticReconcileQueryService = new AutomaticReconcileQueryService(entityPOCO.Tenant);
                AutomaticReconcilePM automaticReconcile = automaticReconcileQueryService.GetSingle(entityPOCO.AutomaticReconcile2, false, true);
                entityPM.AutomaticReconcileName2 = automaticReconcile.EnglishName;
                entityPM.Name += "+" + automaticReconcile.EnglishName;
                entityPM.LocalName += " + " + automaticReconcile.LocalName;
            }

            if (entityPOCO.AutomaticReconcile3 != null)
            {
                AutomaticReconcileQueryService automaticReconcileQueryService = new AutomaticReconcileQueryService(entityPOCO.Tenant);
                AutomaticReconcilePM automaticReconcile = automaticReconcileQueryService.GetSingle(entityPOCO.AutomaticReconcile3, false, true);
                entityPM.AutomaticReconcileName3 = automaticReconcile.EnglishName;
                if (entityPOCO.AutomaticReconcileField2 != null)
                {
                    entityPM.Name += "+" + automaticReconcile.EnglishName;
                    entityPM.LocalName += " + " + automaticReconcile.LocalName;

                }

            }

            entityPOCO.SearchFields = entityPM.Code + ',' + entityPM.Name + ',' + entityPM.LocalName;

        }
   }


}
   