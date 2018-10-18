using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Utils;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationTaxUpdateService //: EntityUpdateService<DeclarationTax, DeclarationTaxPM, EntityPM>
    {
        //protected override void OnCreating(ConsignmentPackagePM entityPM, ConsignmentPM entityParentPM)
        //{

        //}

        protected override void OnUpdating(DeclarationTaxPM entityPM, DeclarationTax entityPOCO)
        {
            try
            {
                base.OnUpdating(entityPM, entityPOCO);
            }
            finally
            {
                var myLogChangesService = new LogChangesService();
                myLogChangesService.
                    LogIt<DeclarationTaxPM, DeclarationTax>("20180708HD310784.LogUntilDateyyyyMMdd", entityPM, entityPOCO);
            }
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as DeclarationTaxRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
