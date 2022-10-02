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
                
                entityPM = SetEFIFILEMData(declarationid, tenant, entityPM, dec);
            }

            return entityPM;
        }

        public ExportDeclarationClosingDataPM SetEFIFILEMData(string declarationid, int tenant, ExportDeclarationClosingDataPM entityPM, DeclarationPM dec = null)
        {
            CustomsSettingQueryService settingService = new CustomsSettingQueryService(tenant);
            CustomsSettingPM setting = settingService.GetSettingByTenantN(tenant);

            if (setting.IsConnectedToUniFreight)
            {
                var isFromNewEntity = true;
                if (dec == null)
                {
                    isFromNewEntity = false;
                    dec = new DeclarationQueryService(tenant).GetSingle(declarationid, true, false);
                }
                if (dec != null && dec.ExportFile != null && dec.Direction == "E" && dec.TransportModeId == "A")
                {
                    if (isFromNewEntity)
                    {
                        entityPM.FinalCargoTypeCode = "36";
                        entityPM.FinalSecondCargoId = "";
                        entityPM.FinalThirdCargoId = "";
                    }

                    List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
                    var tenantAmitalContext = _AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == tenant);
                    if (tenantAmitalContext == null)
                    {
                        tenantAmitalContext = AmitalContext.GetContext(tenant);
                        _AmitalContextList.Add(tenantAmitalContext);
                    }
                    int exportFile = 0;
                    int.TryParse(dec.ExportFile, out exportFile);
                    var EFIFILEMData = new EFIFILEMQueryService(tenantAmitalContext).GetSingle(exportFile, false);
                    var EFIMMNData = new EFIMMNQueryService(tenantAmitalContext).GetByFileNo(tenant, Convert.ToInt64(exportFile));
                    if (EFIMMNData != null && EFIMMNData.Count > 0)
                    {
                        var Warehouse = EFIMMNData[EFIMMNData.Count - 1].WAREHOUSE;
                        if (Warehouse != null)
                        {
                            var ETBVENDData = new ETBVENDQueryService(tenantAmitalContext).GetSingle(Warehouse, false);
                            if (ETBVENDData != null)
                            {
                                entityPM.ChargingSite = ETBVENDData.NAMEENG;
                            }
                        }
                    }
                    if (EFIFILEMData != null)
                    {
                        entityPM.SMP = EFIFILEMData.SMP;
                        entityPM.FLIGHT_DATE = EFIFILEMData.FLIGHT_DATE;
                        if (EFIFILEMData.SPEDNO != null)
                        {
                            var ESPSPEDdata = new ESPSPEDQueryService(tenantAmitalContext).GetSingle(EFIFILEMData.SPEDNO.GetValueOrDefault(), false);
                            if (ESPSPEDdata != null && ESPSPEDdata.MAIN_AWB != null)
                            {
                                var AIRLINE_NUM = "";
                                var ETBAIRLINEData = new ETBAIRLINEQueryService(tenantAmitalContext).GetSingle(ESPSPEDdata.MAINCARRIER, false);
                                if (ETBAIRLINEData != null && ETBAIRLINEData.AIRLINENUM != null)
                                {
                                    AIRLINE_NUM = ETBAIRLINEData.AIRLINENUM + "-";
                                }
                                if (isFromNewEntity)
                                {
                                    entityPM.FinalManifestNumber = AIRLINE_NUM + ESPSPEDdata.MAIN_AWB;
                                }
                                AIRLINE_NUM = ETBAIRLINEData.AIRLINENUM + "-";
                                entityPM.MAIN_AWB = AIRLINE_NUM + ESPSPEDdata.MAIN_AWB;
                            }
                        }
                    }
                }
            }
            return entityPM;
        }
    }
}
