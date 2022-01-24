
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{

    public partial class DeclarationCargoSplitDataMapping : IMapping<DeclarationCargoSplitPM, DeclarationCargoSplit>
    {

        public void CustomPMToPOCO(DeclarationCargoSplitPM entityPM, DeclarationCargoSplit entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;

            }

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(DeclarationCargoSplitPM entityPM, DeclarationCargoSplit entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ActionTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.CargoTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.RequestReasonName);
            CustomMappedPMProperties.Add(PMPropertyNames.ResponseStatusName);

            if (entityPOCO.CargoTypeCode != null)
            {
                CargoIdentifireTypeQueryService cargoIdentifireTypeQueryService = new CargoIdentifireTypeQueryService(entityPOCO.Tenant);
                CargoIdentifireTypePM cargoIdentifireType = cargoIdentifireTypeQueryService.GetSingle(entityPOCO.CargoTypeCode, false, true);
                entityPM.CargoTypeName = cargoIdentifireType != null ? cargoIdentifireType.EnglishName : null;
            }

            if (entityPOCO.ActionTypeCode != null)
            {
                ActionCodeQueryService actionCodeQueryService = new ActionCodeQueryService(entityPOCO.Tenant);
                ActionCodePM actionCode = actionCodeQueryService.GetSingle(entityPOCO.ActionTypeCode, false, true);
                entityPM.ActionTypeName = actionCode != null ? actionCode.LocalName : null;
            }


            if (entityPOCO.RequestReason != null)
            {
                SplitOrMergeReasonQueryService splitOrMergeReasonQueryService = new SplitOrMergeReasonQueryService(entityPOCO.Tenant);
                SplitOrMergeReasonPM splitOrMergeReason = splitOrMergeReasonQueryService.GetSingle(entityPOCO.RequestReason, false, true);
                entityPM.RequestReasonName = splitOrMergeReason != null ? splitOrMergeReason.LocalName : null;
            }

            if (entityPOCO.ResponseStatusCode != null)
            {
                CargoSplitRequestStatusQueryService cargoSplitRequestStatusQueryService = new CargoSplitRequestStatusQueryService(entityPOCO.Tenant);
                CargoSplitRequestStatusPM cargoSplitRequestStatus = cargoSplitRequestStatusQueryService.GetSingle(entityPOCO.ResponseStatusCode, false, true);
                entityPM.ResponseStatusName = cargoSplitRequestStatus != null ? cargoSplitRequestStatus.LocalName : null;
            }

            if (!string.IsNullOrEmpty(entityPOCO.DeclarationId))
            {
                DeclarationQueryService declarationQuery = new DeclarationQueryService(entityPOCO.Tenant);
                DeclarationPM declaration = declarationQuery.GetSingle(entityPOCO.DeclarationId, false, false);
                entityPM.CustomFileNo = declaration != null ? declaration.CustomFileNo : null;
                entityPM.Direction= declaration != null ? declaration.Direction : null;
                entityPM.TransportModeId = declaration != null ? declaration.TransportModeId : null;

            }
        }

        private static void BuildSearchFields(DeclarationCargoSplitPM entityPM, DeclarationCargoSplit poco, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.ManifestNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ManifestNumber : result + "," + entityPM.ManifestNumber;

            }


            if (!string.IsNullOrEmpty(entityPM.RequestNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.RequestNumber : result + "," + entityPM.RequestNumber;

            }

            if (!string.IsNullOrEmpty(entityPM.SecondCargoID))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.SecondCargoID : result + "," + entityPM.SecondCargoID;

            }

            if (!string.IsNullOrEmpty(entityPM.ThirdCargoID))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ThirdCargoID : result + "," + entityPM.ThirdCargoID;

            }

            DeclarationQueryService declarationQuery = new DeclarationQueryService(poco.Tenant);

            DeclarationPM declaration = declarationQuery.GetSingle(entityPM.DeclarationId, false, false);

            if (declaration != null)
            {
                if (!string.IsNullOrEmpty(declaration.DeclarationNumber))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.DeclarationNumber : result + "," + declaration.DeclarationNumber;
                }

                if (!string.IsNullOrEmpty(declaration.CustomFileNo))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.CustomFileNo : result + "," + declaration.CustomFileNo;
                }

            }
            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
    }
}
   