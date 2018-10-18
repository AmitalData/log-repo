using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsCollateralQueryService
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, CustomsCollateralPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomsCollateralKeys customsCollateralKeys = entityKeys as CustomsCollateralKeys;
            CustomsCollateralsAnswerQueryService customsCollateralsAnswerQueryService = new CustomsCollateralsAnswerQueryService(context);
            entityPM.CustomsCollateralsAnswers = customsCollateralsAnswerQueryService.GetMulti(customsCollateralKeys, true);
            CustomsCollateralsConditionQueryService customsCollateralsConditionQueryService = new CustomsCollateralsConditionQueryService(context);
            entityPM.CustomsCollateralsConditions = customsCollateralsConditionQueryService.GetMulti(customsCollateralKeys, true);


            if (entityPM.CustomsCollateralsAnswers != null)
            {
                if (entityPM.CustomsCollateralsAnswers.Count > 0)
                {
                    entityPM.CustomsCollateralsAnswerLineNumber = entityPM.CustomsCollateralsAnswers.Max(m => m.LineNumber);
                }
            }
        }

        public string GetIdByCollateralRequestNumber(string collateralRequestNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(collateralRequestNumber)) return "";
            return repository.GetIdByCollateralRequestNumber(collateralRequestNumber, tenant);
        }

        public List<CustomsCollateralPM> GetDeclarationCollateralsList(string declarationId, int tenant)
        {
            List<CustomsCollateral> customsCollaterals = repository.GetDeclarationCollateralsList(declarationId, tenant);
            List<CustomsCollateralPM> customsCollateralList = new List<CustomsCollateralPM>();
            if (customsCollaterals != null)
            {
                foreach (var customsCollateralItem in customsCollaterals)
                {
                    CustomsCollateralPM customsCollateralPM = this.GetSingle(customsCollateralItem.Id,true,false);
                    customsCollateralList.Add(customsCollateralPM);
                }
            }


            return customsCollateralList;
        }

        public List<CustomsCollateralPM> GetCollateralsListByDeclarationConstraint(string customsEntityTypeCode, string entityIdKey1, string entityIdKey2, int tenant)
        {
            List<CustomsCollateral> customsCollaterals = repository.GetCollateralsListByDeclarationConstraint(customsEntityTypeCode, entityIdKey1, entityIdKey2, tenant);
            List<CustomsCollateralPM> customsCollateralList = new List<CustomsCollateralPM>();
            if (customsCollaterals != null)
            {
                foreach (var customsCollateralItem in customsCollaterals)
                {
                    CustomsCollateralPM customsCollateralPM = this.GetSingle(customsCollateralItem.Id, true, false);
                    customsCollateralList.Add(customsCollateralPM);
                }
            }
            return customsCollateralList;
        }
    }
}
