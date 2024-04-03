using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.Helpers
{
    public class DWObjectFieldAdditionalFactService
    {
        public List<DWObjectFieldPM> DWObjectFieldPMs = new List<DWObjectFieldPM>();
        public bool IsHaveAddAdditionalFactFields = false;
        public DWObjectTablePM DwObjectTable = null;
        private DWObjectTableQuery dWObjectTableQuery = null;
        private DWObjectFieldQuery dWObjectFieldQuery = null;
        private string factTableCode = string.Empty;
        private int tenant;
        private bool groupedByCategory = false;

        public DWObjectFieldAdditionalFactService(DWObjectFieldAdditionalFactArgs dWObjectFieldAdditionalFactArgs)
        {

            this.factTableCode = dWObjectFieldAdditionalFactArgs.FactTableCode;
            this.tenant = dWObjectFieldAdditionalFactArgs.Tenant;
            this.groupedByCategory = dWObjectFieldAdditionalFactArgs.GroupedByCategory;
            dWObjectTableQuery = new DWObjectTableQuery(tenant);
            dWObjectFieldQuery = new DWObjectFieldQuery(tenant);
            DwObjectTable = dWObjectTableQuery.GetSinglePM(factTableCode, tenant);
            IsHaveAddAdditionalFactFields = (DwObjectTable != null && !string.IsNullOrEmpty(DwObjectTable.AdditionalFactCode)) ? true : false;
            if (!dWObjectFieldAdditionalFactArgs.DontLoadDwObjectField) LoadDWObjectFieldsWithAdditionalFactFields();

        }


        public void LoadDWObjectFieldsWithAdditionalFactFields()
        {
            string factCode = !string.IsNullOrEmpty(DwObjectTable.ParentFactCode) ? DwObjectTable.ParentFactCode : factTableCode;



            var factDWObjectFieldPMs = groupedByCategory ? dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(0, factCode, DwObjectTable.RecordType) : dWObjectFieldQuery.GetDWObjectFieldByDWObjectTableCode(0, factTableCode).Where(d => d.DisplayInQueryBuilder == true && (string.IsNullOrEmpty(d.RecordType) || (!string.IsNullOrEmpty(d.RecordType) && d.RecordType.IndexOf(DwObjectTable.RecordType) > -1))).ToList();
            DWObjectFieldPMs = new List<DWObjectFieldPM>();
            if (IsHaveAddAdditionalFactFields)
            {
                var additionalFactDWObjectFieldPMs = groupedByCategory ? dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(0, DwObjectTable.AdditionalFactCode, DwObjectTable.RecordType) : dWObjectFieldQuery.GetDWObjectFieldByDWObjectTableCode(0, DwObjectTable.AdditionalFactCode).Where(d => string.IsNullOrEmpty(d.RecordType) || (!string.IsNullOrEmpty(d.RecordType) && d.RecordType.IndexOf(DwObjectTable.RecordType) > -1)).ToList();
                foreach (DWObjectFieldPM additionalFactField in additionalFactDWObjectFieldPMs.Where(d => (d.IsMeasurement == false || IsShipmentProfitField(d)) && (d.DisplayInQueryBuilder || (d.IsCustom && !DwObjectTable.HasCustomFields))))
                {
                    if (IsShipmentProfitField(additionalFactField))
                        additionalFactField.AggregationTypeCode = "MAX";

                    var dwObjectField = !string.IsNullOrEmpty(additionalFactField.OriginalObjectFieldCode) ? factDWObjectFieldPMs.Where(d => d.OriginalObjectFieldCode == additionalFactField.OriginalObjectFieldCode && d.DisplayInQueryBuilder).FirstOrDefault() : null;
                    if (dwObjectField == null)
                    {
                        dwObjectField = !string.IsNullOrEmpty(additionalFactField.Code) ? factDWObjectFieldPMs.Where(d => d.Code == additionalFactField.Code && d.DisplayInQueryBuilder).FirstOrDefault() : null;
                        if (dwObjectField == null && ContainTableRecordType(additionalFactField))
                        {
                            DWObjectFieldPMs.Add(additionalFactField);
                        }
                    }
                }
            }



            DWObjectFieldPMs = DWObjectFieldPMs.Concat(factDWObjectFieldPMs).ToList();
        }

        private bool IsShipmentProfitField(DWObjectFieldPM Field)
        {
            return (Field.Code == "[Profit]" || Field.Code == "[Profit ( Local )]"|| Field.Code == "[Accounted Profit]" || Field.Code == "[Accounted Profit(Local)]") && factTableCode == "Fact_ARInvoices" && Field.DWObjectTableCode == "Fact_Shipments";
        }

        private bool ContainTableRecordType(DWObjectFieldPM additionalFactField) 
        {
            if (string.IsNullOrEmpty(additionalFactField.RecordType)) return true;
            string[] recordTypes = additionalFactField.RecordType.Split(',');

            foreach (var recordType in recordTypes)
            {
                if (recordType.Trim() == DwObjectTable.RecordType) return true;
            } 
            return false; 
        }
    } 

    public class DWObjectFieldAdditionalFactArgs
    {

        public string FactTableCode { get; set; }
        public int Tenant { get; set; }
        public bool DontLoadDwObjectField { get; set; }
        public bool GroupedByCategory { get; set; }

    }
}