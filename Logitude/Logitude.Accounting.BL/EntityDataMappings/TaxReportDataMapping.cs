
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
            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "TaxReportDataMapping.CustomPMToPOCO(*1L*): " + entityPM.Id + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode;
            ULog(text, stopLogAt);

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
            text = "TaxReportDataMapping.CustomPMToPOCO(*9L*): " + entityPM.Id + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode;//, false, "TaxReportPMToPOCO_1L");
            ULog(text, stopLogAt);
        }


        public void CustomPOCOToPM(TaxReportPM entityPM, TaxReport entityPOCO)
        {
            DateTime stopLogAt = new DateTime(2023, 06, 01);
            string text = "TaxReportDataMapping.CustomPOCOToPM(*1*): " + entityPM.Id + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode;
            ULog(text, stopLogAt);

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
                if (entityPM.StatusCode == "D" || entityPM.StatusCode == "E")
                {
                    entityPM.CanRecalculate = true;
                }
            }
           
            text = "TaxReportDataMapping.CustomPOCOToPM(*7*): " + entityPM.Id + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode;
            ULog(text, stopLogAt);

            MapClosingJournalFields(entityPM, entityPOCO);

            text = "TaxReportDataMapping.CustomPOCOToPM(*9*): " + entityPM.Id + " PM.StatusCode : " + entityPM.StatusCode + ",  POCO.StatusCode : " + entityPOCO.StatusCode;
            ULog(text, stopLogAt);
        }

        private static void ULog(string text, DateTime stopLogAt)
        {
            string log_text = text + System.Environment.NewLine;
            log_text = log_text + String.Format("{0:HH:mm:ss.ffff}", DateTime.Now.ToString()) + System.Environment.NewLine;
            System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
            log_text = log_text + t.ToString();
            LogitudeSettings.HandleLogMe(log_text, false, "TaxReportPMToPOCO", new DateTime(2023, 6, 1));
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
   