
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
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ExportDeclarationClosingDataDataMapping: IMapping<ExportDeclarationClosingDataPM, ExportDeclarationClosingData>
   {

        public void CustomPMToPOCO(ExportDeclarationClosingDataPM entityPM, ExportDeclarationClosingData entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(ExportDeclarationClosingDataPM entityPM, ExportDeclarationClosingData entityPOCO)
        {
            if(entityPOCO.FinalCargoType != null)
            {
                entityPM.FinalCargoTypeName = entityPOCO.FinalCargoType.LocalName;
            }
            if(entityPOCO.FinalLoadingSiteType != null)
            {
                entityPM.FinalLoadingSiteName = entityPOCO.FinalLoadingSiteType.LocalName;
            }
            if(entityPOCO.FinalCustomsShip != null)
            {
                entityPM.FinalShipCodeName = entityPOCO.FinalCustomsShip.LocalName;
            }
            var dec = new DeclarationQueryService(entityPOCO.Tenant).GetSingle(entityPOCO.DeclarationId, false, false);
            if (dec != null && dec.ExportFile != null && dec.Direction == "E" && dec.TransportModeId == "A") 
            { // only if transportmode= A and direction = export 
                List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
                var tenantAmitalContext = _AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == entityPM.Tenant);
                if (tenantAmitalContext == null)
                {
                    tenantAmitalContext = AmitalContext.GetContext(entityPM.Tenant);
                    _AmitalContextList.Add(tenantAmitalContext);
                }
                int.TryParse(dec.ExportFile, out int exportFile);
                var EFIFILEMData = new EFIFILEMQueryService(tenantAmitalContext).GetSingle(exportFile, false);
                if(EFIFILEMData != null)
                {
                    entityPM.SMP = EFIFILEMData.SMP;
                    entityPM.FLIGHT_DATE=EFIFILEMData.FLIGHT_DATE;
                    if(EFIFILEMData.SPEDNO != null)
                    {
                        var ESPSPEDdata = new ESPSPEDQueryService(tenantAmitalContext).GetSingle(EFIFILEMData.SPEDNO.GetValueOrDefault(), false);
                        if(ESPSPEDdata != null)
                        {
                            entityPM.MAIN_AWB = ESPSPEDdata.MAIN_AWB;
                        }
                    }
                }
            }
        }
   }


}
   