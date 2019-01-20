
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
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class TaxReportLineDataMapping: IMapping<TaxReportLinePM, TaxReportLine>
   {
       
        public void CustomPMToPOCO(TaxReportLinePM entityPM, TaxReportLine entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.TaxReportId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.Line);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.TaxReportId = entityPM.TaxReportId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Line = entityPM.Line;
            }
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;

        }
        private static void BuildSearchFields(TaxReportLinePM entityPM, TaxReportLine poco, bool isNewEntity)
        {
            string result = "";


            if (!string.IsNullOrEmpty(entityPM.VatNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.VatNumber : result + "," + entityPM.VatNumber;

            }
            if (!string.IsNullOrEmpty(entityPM.Reference))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.Reference : result + "," + entityPM.Reference;

            }
            //if (!string.IsNullOrEmpty(entityPM.ReferecneGroup))
            //{
            //    result = string.IsNullOrEmpty(result) ? entityPM.ReferecneGroup : result + "," + entityPM.ReferecneGroup;

            //}
            //if (!string.IsNullOrEmpty(entityPM.JournalNumber))
            //{
            //    result = string.IsNullOrEmpty(result) ? entityPM.JournalNumber : result + "," + entityPM.JournalNumber;


            //}
            //if (!string.IsNullOrEmpty(entityPM.StatusEnglishName))
            //{
            //    result = string.IsNullOrEmpty(result) ? entityPM.StatusEnglishName : result + "," + entityPM.StatusEnglishName;


            //}
            //if (!string.IsNullOrEmpty(entityPM.StatusLocalName))
            //{
            //    result = string.IsNullOrEmpty(result) ? entityPM.StatusLocalName : result + "," + entityPM.StatusLocalName;


            //}

            entityPM.SearchFields = result;
            poco.SearchFields = result;

        }






        public void CustomPOCOToPM(TaxReportLinePM entityPM, TaxReportLine entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.StatusCode);

            if (entityPOCO.StatusCode != null)
            {
                TaxReportLineStatusQueryService queryService = new TaxReportLineStatusQueryService(entityPOCO.Tenant);
                TaxReportLineStatusPM status = queryService.GetSingle(entityPOCO.StatusCode, false, false);
                if (status != null)
                {
                    entityPM.StatusEnglishName = status.EnglishName;
                    entityPM.StatusLocalName = status.LocalName;
                }
            }

            //Journal
            CustomMappedPOCOProperties.Add(POCOPropertyNames.JournalId);
            if (entityPOCO.JournalId != null)
            {
                JournalQueryService queryService = new JournalQueryService(entityPOCO.Tenant);
                JournalPM jr = queryService.GetSingle(entityPOCO.JournalId, false, false);
                if (jr != null)
                {
                    entityPM.JournalNumber = jr.JournalNumber;
                }
            }

            //entityPM.SearchFields = entityPM.VatNumber + "," + entityPM.Reference + "," + entityPM.ReferecneGroup + "," + entityPM.JournalNumber + "," + entityPM.StatusEnglishName + "," + entityPM.StatusLocalName;


        }
   }


}
   