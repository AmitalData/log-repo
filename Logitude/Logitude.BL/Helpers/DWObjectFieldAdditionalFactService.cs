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
            var factDWObjectFieldPMs = groupedByCategory ? dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(0, factTableCode) : dWObjectFieldQuery.GetDWObjectFieldByDWObjectTableCode(0, factTableCode).Where(d => d.DisplayInQueryBuilder == true).ToList();
            DWObjectFieldPMs = new List<DWObjectFieldPM>();
            if (IsHaveAddAdditionalFactFields)
            {
                var additionalFactDWObjectFieldPMs = groupedByCategory ? dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(0, DwObjectTable.AdditionalFactCode) : dWObjectFieldQuery.GetDWObjectFieldByDWObjectTableCode(0, DwObjectTable.AdditionalFactCode).ToList(); ;
                foreach (DWObjectFieldPM additionalFactField in additionalFactDWObjectFieldPMs.Where(d => d.IsMeasurement == false && (d.DisplayInQueryBuilder || d.IsCustom)))
                {
                    var dwObjectField = !string.IsNullOrEmpty(additionalFactField.OriginalObjectFieldCode) ? factDWObjectFieldPMs.Where(d => d.OriginalObjectFieldCode == additionalFactField.OriginalObjectFieldCode).FirstOrDefault() : null;
                    if (dwObjectField == null)
                    {
                        dwObjectField = !string.IsNullOrEmpty(additionalFactField.Code) ? factDWObjectFieldPMs.Where(d => d.Code == additionalFactField.Code).FirstOrDefault() : null;
                        if (dwObjectField == null) DWObjectFieldPMs.Add(additionalFactField);
                    }
                }
            }

            DWObjectFieldPMs = DWObjectFieldPMs.Concat(factDWObjectFieldPMs).ToList();
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