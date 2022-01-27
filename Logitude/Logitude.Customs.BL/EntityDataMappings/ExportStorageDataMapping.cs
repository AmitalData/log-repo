
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
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ExportStorageDataMapping: IMapping<ExportStoragePM, ExportStorage>
   {

        public void CustomPMToPOCO(ExportStoragePM entityPM, ExportStorage entityPOCO)
        {
            //throw new NotImplementedException();

            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
            
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(ExportStoragePM entityPM, ExportStorage entityPOCO)
        {
            var declarationQueryService = new DeclarationQueryService(entityPOCO.Tenant);
            DeclarationPM declarationPM = entityPOCO.DeclarationId != null ? declarationQueryService.GetSingleDeclarationById(entityPOCO.DeclarationId, entityPOCO.Tenant) : null;

            var cargoTypeQueryService = new CargoTypeQueryService(entityPOCO.Tenant);
            CargoTypePM cargoTypePM = cargoTypeQueryService.GetSingle(entityPOCO.CargoType, false, true);

            var cargoStatusQueryService = new CargoStatusQueryService(entityPOCO.Tenant);
            CargoStatusPM cargoStatusPM = cargoStatusQueryService.GetSingle(entityPOCO.CustomsStatus, false, true);

            var cardQueryService = new CardQueryService(entityPOCO.Tenant);
            Card cardPM = 
             new CardRepository(entityPOCO.Tenant).GetCardsByIds(new List<string>() { entityPOCO.ExporterID }, entityPOCO.Tenant).Count() > 0 ?
             cardQueryService.GetCardById(entityPOCO.ExporterID, entityPOCO.Tenant): null;

            var customsShipQueryService = new CustomsShipQueryService(entityPOCO.Tenant);
            CustomsShipPM customsShipPM = customsShipQueryService.GetSingle(entityPOCO.ShipCode, false, true);

            var cargoIdentifireTypeQueryService = new CargoIdentifireTypeQueryService(entityPOCO.Tenant);
            CargoIdentifireTypePM cargoIdentifireTypePM  = cargoIdentifireTypeQueryService.GetSingle(entityPOCO.CargoTypeCode, false, true);

            entityPM.Declaration_ID = declarationPM?.Id;
            entityPM.DeclarationStatusTypeName = declarationPM?.DeclarationStatusTypeName;
            entityPM.CargoTypeName = cargoTypePM.LocalName;
            entityPM.CustomStatusName = cargoStatusPM?.LocalName;
            entityPM.ExporterName = cardPM?.LocalName;
            entityPM.ShipName = customsShipPM?.LocalName;
            entityPM.CargoTypeCodeName = cargoIdentifireTypePM?.LocalName;
            entityPM.DeclarationStatusTypeCode = declarationPM?.DeclarationStatusTypeCode;
            entityPM.DeclarationCustomFileNo = declarationPM?.CustomFileNo;
            entityPM.DeclarationNumber = declarationPM?.DeclarationNumber;
            entityPM.ExporterCode = cardPM?.VatNumber;
        }
   }
}
   