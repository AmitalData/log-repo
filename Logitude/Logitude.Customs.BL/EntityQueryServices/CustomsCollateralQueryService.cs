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
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.Repsitories;

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

        public List<CustomsCollateral> UpdateMulti(string[] ids, string declarationId, bool selectAll, CustomsCollateralsAnswerPM customsCollateralsAnswerPM, int tenant)
        {
            CustomsCollateralUpdateService updateService = new CustomsCollateralUpdateService(context, new Dictionary<string, IContext>(), tenant);
            var ccaRepo = new CustomsCollateralsAnswerRepository(context);
            List<CustomsCollateral> customsCollateralList =
                selectAll ?
                    repository.GetDeclarationCollateralsList(declarationId, tenant).FindAll(x => !ids.Contains(x.Id) && x.IsClosed == false) :
                    repository.GetDeclarationCollateralsList(ids);

            AddCustomsCollateralsAnswerToCustomsCollaterals(customsCollateralsAnswerPM, tenant, updateService, ccaRepo, customsCollateralList);

            return customsCollateralList;
        }

        private void AddCustomsCollateralsAnswerToCustomsCollaterals(CustomsCollateralsAnswerPM customsCollateralsAnswerPM, int tenant, CustomsCollateralUpdateService updateService, CustomsCollateralsAnswerRepository ccaRepo, List<CustomsCollateral> customsCollateralList)
        {
            customsCollateralList.ForEach(customsCollateralItem =>
            {
                CustomsCollateralPM customsCollateralPM = GetEntityPM(customsCollateralItem, true, new CustomsCollateralKeys() { Id = customsCollateralItem.Id });
                customsCollateralsAnswerPM.CustomsCollateralId = customsCollateralPM.Id;
                customsCollateralsAnswerPM.AllocatedAmount = customsCollateralPM.CustomsCollateralsConditions.Sum(x=> x.RequestedAmount);
                var ccaList = customsCollateralPM.CustomsCollateralsAnswers;
                customsCollateralsAnswerPM.LineNumber = ccaList.Count == 0 ? 1 : ccaList.Max(m => m.LineNumber) + 1; 

                customsCollateralsAnswerPM.ChangeSetOp = ChangeSetOperation.Insert;
                customsCollateralPM.CustomsCollateralsAnswers.Add(customsCollateralsAnswerPM);

                customsCollateralPM.ChangeSetOp = ChangeSetOperation.Update;
                updateService.Update(customsCollateralPM, true);
            });
        }
    }
}
