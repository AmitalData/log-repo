using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.BL
{
    public class EFIFILEMDataInExportDeclarationClosing
    {
        public ExportDeclarationClosingDataPM GetEFIFILEMDataInExportDeclarationClosingDataPM(string declarationid, int tenant)
        {
            var dec = new DeclarationQueryService(tenant).GetSingle(declarationid, true, false);
            var entityPM = new ExportDeclarationClosingDataQueryService(tenant).GetSingle(declarationid, true, false);
            if (entityPM == null)
            {
                entityPM = new ExportDeclarationClosingDataPM();
                entityPM.DeclarationId = dec.Id;
                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                entityPM.Tenant = tenant;
                foreach (var con in dec.Consignments)
                {
                    if (con.ConsignmentType == "E")
                    {
                        entityPM.FinalCargoTypeCode = con.CargoTypeCode;
                        entityPM.FinalSecondCargoId = con.SecondCargoID;
                        entityPM.FinalThirdCargoId = con.ThirdCargoID;
                        entityPM.FinalManifestNumber = con.ManifestNumber;
                        entityPM.FinalShipCode = con.ShipCode;
                        entityPM.FinalLoadingSite = con.ExportLoadingPortCode;
                    }
                }
                entityPM = SetEFIFILEMData(declarationid, tenant, entityPM, dec);
            }

            return entityPM;
        }

        public ExportDeclarationClosingDataPM SetEFIFILEMData(string declarationid, int tenant, ExportDeclarationClosingDataPM entityPM, DeclarationPM dec = null)
        {
            if (dec == null)
                dec = new DeclarationQueryService(tenant).GetSingle(declarationid, true, false);

            if (dec != null && dec.ExportFile != null && dec.Direction == "E" && dec.TransportModeId == "A")
            { 
                List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
                var tenantAmitalContext = _AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == tenant);
                if (tenantAmitalContext == null)
                {
                    tenantAmitalContext = AmitalContext.GetContext(tenant);
                    _AmitalContextList.Add(tenantAmitalContext);
                }
                int.TryParse(dec.ExportFile, out int exportFile);
                var EFIFILEMData = new EFIFILEMQueryService(tenantAmitalContext).GetSingle(exportFile, false);
                if (EFIFILEMData != null)
                {
                    entityPM.SMP = EFIFILEMData.SMP;
                    entityPM.FLIGHT_DATE = EFIFILEMData.FLIGHT_DATE;
                    if (EFIFILEMData.SPEDNO != null)
                    {
                        var ESPSPEDdata = new ESPSPEDQueryService(tenantAmitalContext).GetSingle(EFIFILEMData.SPEDNO.GetValueOrDefault(), false);
                        if (ESPSPEDdata != null)
                        {
                            entityPM.MAIN_AWB = ESPSPEDdata.MAINCARRIER + "-" + ESPSPEDdata.MAIN_AWB;
                        }
                    }
                }
            }
            return entityPM;
        }
    }
}