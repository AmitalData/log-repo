
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

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class TaxReportDataMapping: IMapping<TaxReportPM, TaxReport>
   {

        public void CustomPMToPOCO(TaxReportPM entityPM, TaxReport entityPOCO)
        {
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
        }

        public void CustomPOCOToPM(TaxReportPM entityPM, TaxReport entityPOCO)
        {
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
            }

            MapClosingJournalFields(entityPM, entityPOCO);
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
   