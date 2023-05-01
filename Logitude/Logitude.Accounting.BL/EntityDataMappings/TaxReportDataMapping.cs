
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Server.Tools.Utils;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class TaxReportDataMapping: IMapping<TaxReportPM, TaxReport>
   {

        public void CustomPMToPOCO(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            //DateTime stopLogAt = new DateTime(2023, 06, 01);
            //LogitudeSettings.HandleLogMe("TaxReportDataMapping.CustomPMToPOCO(*1*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPMToPOCO", stopLogAt);
            //Simplog.Server.Infrastructure.LogitudeSettings.HandleLogMe?.Invoke("TaxReportPMToPOCO_1", false, "", DateTime.MaxValue);
            Logger.LogMe("TaxReportDataMapping.CustomPMToPOCO(*1L*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPMToPOCO_1L");

            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;

            if (!this.CustomMappedPOCOProperties.Contains(POCOPropertyNames.StatusCode))
            {
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.StatusCode);
            }
            entityPOCO.StatusCode = entityPM.StatusCode;
            //LogitudeSettings.HandleLogMe("TaxReportDataMapping.CustomPMToPOCO(*9*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPMToPOCO", stopLogAt);
            Logger.LogMe("TaxReportDataMapping.CustomPMToPOCO(*9L*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPMToPOCO_1L");

        }

        public void CustomPOCOToPM(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            //DateTime stopLogAt = new DateTime(2023, 06, 01);
            //LogitudeSettings.HandleLogMe("TaxReportDataMapping.CustomPOCOToPM(*1*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPOCOToPM", stopLogAt);
            //Simplog.Server.Infrastructure.LogitudeSettings.HandleLogMe?.Invoke("TaxReportPOCOToPM_1", false, "", DateTime.MaxValue);
            Logger.LogMe("TaxReportDataMapping.CustomPOCOToPM(*1L*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPOCOToPM_1L");

            CustomMappedPOCOProperties.Add(POCOPropertyNames.StatusCode);

            if (entityPOCO.StatusCode != null)
            {
                VatReportStatusQueryService queryService = new VatReportStatusQueryService(entityPOCO.Tenant);
                VatReportStatusPM status = queryService.GetSingle(entityPOCO.StatusCode, false, false);
                if (status != null)
                {
                    entityPM.StatusEnglishName = status.EnglishName;
                    entityPM.StatusLocalName = status.LocalName;
                }
                entityPM.StatusCode = entityPOCO.StatusCode;
            }
            //LogitudeSettings.HandleLogMe("TaxReportDataMapping.CustomPOCOToPM(*9*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPOCOToPM", stopLogAt);
            Logger.LogMe("TaxReportDataMapping.CustomPOCOToPM(*9L*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPOCOToPM_1L");

            MapClosingJournalFields(entityPM, entityPOCO);

            //LogitudeSettings.HandleLogMe("TaxReportDataMapping.CustomPOCOToPM(*9*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPOCOToPM", stopLogAt);
            Logger.LogMe("TaxReportDataMapping.CustomPOCOToPM(*9L*): " + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode, false, "TaxReportPOCOToPM_1L");
        }

        private void MapClosingJournalFields(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            JournalPM closingJournal = GetClosingJournalOfTaxReport(entityPOCO);
            if (closingJournal != null)
            {
                entityPM.ClosingJournalNumber = closingJournal.JournalNumber;
                entityPM.ClosingJournalId = closingJournal.Id;
            }
        }

        private JournalPM GetClosingJournalOfTaxReport(TaxReport entityPOCO)
        {
            JournalQueryService journalQueryService = new JournalQueryService(entityPOCO.Tenant);
            return journalQueryService.GetSingleWithLinesByEntityIdAndCode(entityPOCO.Id, AccountingEntityValues.TaxReport, entityPOCO.Tenant);
        }

        private static void BuildSearchFields(TaxReportPM entityPM, TaxReport poco, bool isNewEntity)
        {
            string result = "";



            if (!string.IsNullOrEmpty(entityPM.TaxReportNumber))
            {
                if (!(result.Split(',').Contains(entityPM.TaxReportNumber)))
                {
                    result = string.IsNullOrEmpty(result) ? entityPM.TaxReportNumber : result + "," + entityPM.TaxReportNumber;
                }
            }


            //if (entityPM.TaxReportMonth != null)
            //{
             
            //        result =  (string.IsNullOrEmpty(result) ? entityPM.TaxReportMonth.ToString() : result + "," + entityPM.TaxReportMonth).ToString();
                
            //}

            //if (!string.IsNullOrEmpty(entityPM.VatNumber))
            //{
            //    result = string.IsNullOrEmpty(result) ? entityPM.VatNumber : result + "," + entityPM.VatNumber;

            //}

           

         

            entityPM.SearchFields = result;
            poco.SearchFields = result;

        }
    }


}
   