
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
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ExportStorageDataMapping: IMapping<ExportStoragePM, ExportStorage>
   {

        public void CustomPMToPOCO(ExportStoragePM entityPM, ExportStorage entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ExportStoragePM entityPM, ExportStorage entityPOCO)
        {
            var declarationQueryService = new DeclarationQueryService(entityPOCO.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingleDeclarationById(entityPOCO.DeclarationId, entityPOCO.Tenant);

            var cargoTypeQueryService = new CargoTypeQueryService(entityPOCO.Tenant);
            CargoTypePM cargoTypePM = cargoTypeQueryService.GetSingle(entityPOCO.CargoType, false, true);

            var storageStatusQueryService = new StorageStatusQueryService(entityPOCO.Tenant);
            StorageStatusPM storageStatusPM = storageStatusQueryService.GetSingle(entityPOCO.StorageStatus, false, true);

            var cardQueryService = new CardQueryService(entityPOCO.Tenant);
            Card cardPM = cardQueryService.GetCardById(entityPOCO.ExporterID, entityPOCO.Tenant);

            var customsShipQueryService = new CustomsShipQueryService(entityPOCO.Tenant);
            CustomsShipPM customsShipPM = customsShipQueryService.GetSingle(entityPOCO.ShipCode, false, true);

            var cargoIdentifireTypeQueryService = new CargoIdentifireTypeQueryService(entityPOCO.Tenant);
            CargoIdentifireTypePM cargoIdentifireTypePM  = cargoIdentifireTypeQueryService.GetSingle(entityPOCO.CargoTypeCode, false, true);

            entityPM.DeclarationStatusTypeName = declarationPM.DeclarationStatusTypeName;
            entityPM.CargoTypeName = cargoTypePM.LocalName;
            entityPM.StorageStatusName = storageStatusPM.LocalName;
            entityPM.ExporterName = cardPM.LocalName;
            entityPM.ShipName = customsShipPM.LocalName;
            entityPM.CargoTypeCodeName = cargoIdentifireTypePM.LocalName;
            entityPM.DeclarationStatusTypeCode = declarationPM.DeclarationStatusTypeCode;
        }
   }
}
   