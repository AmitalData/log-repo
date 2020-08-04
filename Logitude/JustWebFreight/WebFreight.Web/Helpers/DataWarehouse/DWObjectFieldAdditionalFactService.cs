//using Logitude.BL.InfrastructureModel.EntityPMs;
//using Logitude.BL.InfrastructureModel.EntityQueries;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace WebFreight.Web.Helpers.DataWarehouse
//{
//    public class DWObjectFieldAdditionalFactService
//    {
//        public List<DWObjectFieldPM> DWObjectFieldPMs = new List<DWObjectFieldPM>();
//        public  bool IsHaveAddAdditionalFactFields = false;
//        public  DWObjectTablePM DwObjectTable = null;
//        private DWObjectTableQuery dWObjectTableQuery = null;
//        private DWObjectFieldQuery dWObjectFieldQuery = null;
//        private string factTableCode = string.Empty;
//        private int tenant;
        


//        public DWObjectFieldAdditionalFactService(string factTableCode, int tenant, bool loadDwObjectField = false)
//        {

//            this.factTableCode = factTableCode;
//            this.tenant = tenant;
            
//            dWObjectTableQuery = new DWObjectTableQuery(tenant);
//            dWObjectFieldQuery = new DWObjectFieldQuery(tenant);
//            DwObjectTable = dWObjectTableQuery.GetSinglePM(factTableCode, tenant);
//            IsHaveAddAdditionalFactFields = (DwObjectTable != null && !string.IsNullOrEmpty(DwObjectTable.AdditionalFactCode)) ? true : false;
//            if (loadDwObjectField) LoadDWObjectFieldsWithAdditionalFactFields();

//        }






//        public void LoadDWObjectFieldsWithAdditionalFactFields()
//        {
//            if (DwObjectTable != null)
//            {
//                DWObjectFieldPMs = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(0, factTableCode);
//                if (IsHaveAddAdditionalFactFields)
//                {
//                    var additionalFactFields = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(0, DwObjectTable.AdditionalFactCode);
//                    foreach (DWObjectFieldPM additionalFactField in additionalFactFields.Where(d => d.IsMeasurement == false && (d.DisplayInQueryBuilder || d.IsCustom)))
//                    {
//                        var dwObjectField = !string.IsNullOrEmpty(additionalFactField.OriginalObjectFieldCode) ? DWObjectFieldPMs.Where(d => d.OriginalObjectFieldCode == additionalFactField.OriginalObjectFieldCode).FirstOrDefault() : null;
//                        if (dwObjectField == null)
//                        {
//                            dwObjectField = !string.IsNullOrEmpty(additionalFactField.Code) ? DWObjectFieldPMs.Where(d => d.Code == additionalFactField.Code).FirstOrDefault() : null;
//                            if (dwObjectField == null) DWObjectFieldPMs.Add(additionalFactField);
//                        }
//                    }
//                }

//            }
//        }




//    }
//}