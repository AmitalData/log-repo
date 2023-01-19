using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ExportDeclarationClosingDataUpdateService : EntityUpdateService<ExportDeclarationClosingData, ExportDeclarationClosingDataPM, EntityPM>
    {
        

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.ExportDeclarationClosingDataRepository).FastDeleteMulti(entityKeyFields);
        }
        protected override void OnUpdating(ExportDeclarationClosingDataPM entityPM, ExportDeclarationClosingData entityPOCO)
        {

            entityPM.FinalManifestNumber = !string.IsNullOrEmpty(entityPM.FinalManifestNumber) ? entityPM.FinalManifestNumber.Trim() : "";
            entityPM.FinalSecondCargoId = !string.IsNullOrEmpty(entityPM.FinalSecondCargoId) ? entityPM.FinalSecondCargoId.Trim() : "";
            entityPM.FinalThirdCargoId = !string.IsNullOrEmpty(entityPM.FinalThirdCargoId) ? entityPM.FinalThirdCargoId.Trim() : "";
            if (entityPM.FinalLoadingSite != null)
            {
                ICustomContext context = MainContext as CustomContext;
                var query = new LoadingSiteTypeQueryService(context);
                var site = query.GetSingle(entityPM.FinalLoadingSite,false,true);
                if(site == null)
                {
                    entityPM.FinalLoadingSite = null;
                }

            }
        }
    }
}
