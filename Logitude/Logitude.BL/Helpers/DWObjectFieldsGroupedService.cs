using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class DWObjectFieldsGroupedService
    { 
        public string DWOTId { get; }
        public int Tenant { get; } 
        IEnumerable<IGrouping<string, DWObjectFieldPM>> CategoryGroup { get; set; }

        IEnumerable<IGrouping<string, DWObjectFieldPM>> FactGroups { get; set; }
        public DWObjectFieldsGroupedService(string dWOTId, int tenant)
        {
            DWOTId = dWOTId;
            Tenant = tenant;  
        }
        // refactoring old code
        public List<DWFactGroup> GetFactFieldsGroups()
        {

            var factFields = new DWObjectFieldAdditionalFactService(new DWObjectFieldAdditionalFactArgs() { FactTableCode = DWOTId, Tenant = Tenant, GroupedByCategory = true }).DWObjectFieldPMs;
            CategoryGroup = factFields.GroupBy(a => a.Category);
            FactGroups = factFields.GroupBy(a => a.DWObjectTableCode);

            DWHSettingRepository dWHSettingRepository = new DWHSettingRepository(Tenant);
            var isParentTenant = dWHSettingRepository.IsParentTenant(Tenant);
            List<ObjectFieldPM> objectFieldPMs = new List<ObjectFieldPM>();

            if (!isParentTenant)
            {
                objectFieldPMs = GetCustomObjectFields(DWOTId, Tenant);
            }

            List<DWFactGroup> FactFieldsGroups = BuildFactFieldsGroups(objectFieldPMs);

            return FactFieldsGroups;
        }

        private List<DWFactGroup> BuildFactFieldsGroups(List<ObjectFieldPM> objectFieldPMs)
        {
            var ParentFactIndex = 1;
            List<DWFieldsGroup> Groups = new List<DWFieldsGroup>();
            List<DWFactGroup> FactFieldsGroups = new List<DWFactGroup>();

            foreach (var fact in FactGroups)
            {
                DWFactGroup DWFactGroup = GetDWFactGroup(ParentFactIndex, fact);
                foreach (var categoryGroup in CategoryGroup)
                {
                    var factfields = categoryGroup.Where(a => a.DWObjectTableCode == fact.Key).ToList();

                    if (factfields != null && factfields.Count != 0)
                    {
                        DWFieldsGroup dwFieldsGroup = GetDWFieldsGroup(categoryGroup, factfields);
                        if (dwFieldsGroup.FieldsList != null && dwFieldsGroup.FieldsList.Count > 0)
                        {
                            ResolveCustomFields(objectFieldPMs, Groups, dwFieldsGroup);
                            DWFactGroup.FieldsGroupList.Add(dwFieldsGroup);
                        }
                    }
                }

                FactFieldsGroups.Add(DWFactGroup);
                ParentFactIndex = ++ParentFactIndex;
            }

            FactFieldsGroups = RemoveFactInvoiceCustomFieldsCategory(DWOTId, FactFieldsGroups);
            FactFieldsGroups = FactFieldsGroups.OrderBy(a => a.Index).ToList();
            return FactFieldsGroups;
        }

        private static DWFieldsGroup GetDWFieldsGroup(IGrouping<string, DWObjectFieldPM> item, List<DWObjectFieldPM> factfields)
        {
            var MyGroup = new DWFieldsGroup();
            MyGroup.Key = item.Key;
            var FirstItem = factfields.Select(a => a).FirstOrDefault();
            MyGroup.Index = FirstItem.CategoryIndex;
            MyGroup.Fact = FirstItem.DWObjectTableCode;
            MyGroup.FieldsList = factfields.Select(a => a).OrderBy(a => a.Name).ToList();
            return MyGroup;
        }

        private void ResolveCustomFields(List<ObjectFieldPM> objectFieldPMs, List<DWFieldsGroup> Groups, DWFieldsGroup dwFieldsGroup)
        {
            if (dwFieldsGroup.Key == "Custom Fields" || dwFieldsGroup.Key == "CustomFields")
            {
                ResolveDWCustomObjectFields(objectFieldPMs, dwFieldsGroup, Tenant);

                if (dwFieldsGroup.FieldsList.Where(d => d.DisplayInQueryBuilder).Any())
                {
                    Groups.Add(dwFieldsGroup);
                }
            }
            else Groups.Add(dwFieldsGroup);
        }

        private DWFactGroup GetDWFactGroup(int ParentFactIndex, IGrouping<string, DWObjectFieldPM> fact)
        {
            DWFactGroup DWFactGroup = new DWFactGroup();
              
            var factName = fact.Key.Split('_');
            DWFactGroup.Key = fact.Key.Replace('_', ' ');
            if (factName.Length > 1)
            { 
                DWFactGroup.Key = factName[1] + " " + factName[0];
            } 
            DWFactGroup.FieldsGroupList = new List<DWFieldsGroup>();
            DWFactGroup.Index = GetDWFactGroupIndex(fact, ParentFactIndex);
            return DWFactGroup;
        }

        private int GetDWFactGroupIndex(IGrouping<string, DWObjectFieldPM> fact, int parentFactIndex) 
        { 
            if (DWOTId == "Fact_MasterCharges" && fact.Key != "Fact_Shipments")
            {
                return 1;
            }
            var index = fact.Key == DWOTId ? 1 : parentFactIndex + 1;
            return index;
        }

        private List<DWFactGroup> RemoveFactInvoiceCustomFieldsCategory(string factCode, List<DWFactGroup> dWFactGroup)
        {
            List<DWFactGroup> FactGroups = dWFactGroup;

            foreach (var fact in FactGroups)
            {
                if (factCode == "Fact_Invoices")
                {
                    DWFieldsGroup customFieldsCategroy = fact.FieldsGroupList.FirstOrDefault(categroy => categroy.Key == "Custom Fields");
                    if (customFieldsCategroy != null)
                    {
                        fact.FieldsGroupList.Remove(customFieldsCategroy);
                    }
                }
            }
            return FactGroups;
        }
         
        private List<ObjectFieldPM> GetCustomObjectFields(string DWOTId, int tenant)
        {
            List<ObjectFieldPM> objectFieldPMs = new List<ObjectFieldPM>();
            ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(tenant);
            DWObjectTableQuery dWObjectTableQuery = new DWObjectTableQuery(tenant);
            DWObjectTablePM dWObjectTablePM = dWObjectTableQuery.GetSinglePM(DWOTId, tenant);
            if (dWObjectTablePM != null && !string.IsNullOrEmpty(dWObjectTablePM.ObjectTableName)) objectFieldPMs = objectFieldQuery.GetCustomObjectFieldsByTenantAndObjectTable(tenant, dWObjectTablePM.ObjectTableName);
             
            return objectFieldPMs;
        }


        private void ResolveDWCustomObjectFields(List<ObjectFieldPM> objectFieldPMs, DWFieldsGroup MyGroup, int tenant)
        {
            if (objectFieldPMs != null && objectFieldPMs.Count > 0)
            {
                foreach (var field in MyGroup.FieldsList.Where(d => d.IsCustom).ToList())
                {
                    ObjectFieldPM objectFieldPM = objectFieldPMs.Where(d => d.FieldName == field.Name).FirstOrDefault();
                    if (objectFieldPM != null)
                    {
                        ResolveDWCustomFieldsBasedDataTypeCode(field, objectFieldPM);
                    }
                }
            }
        }

        private static void ResolveDWCustomFieldsBasedDataTypeCode(DWObjectFieldPM field, ObjectFieldPM objectFieldPM)
        {
            if (objectFieldPM.DataTypeCode != "LookUp")
            {
                field.DisplayName = objectFieldPM.FullNameTextCodeDefaultText;
                field.DataTypeCode = objectFieldPM.DataTypeCode;
                if (field.DataTypeCode == "Date")
                {
                    field.DataTypeCode = "Dimension";
                    field.DimensionTableCode = "DIM_Dates";
                }
                else if (field.DataTypeCode == "PickList")
                {
                    field.DataTypeCode = "Dimension";
                    field.DimensionTableCode = "DIM_CustomPickLists";
                    field.HideTree = true;
                    field.CustomPickListCode = objectFieldPM.CustomPickListCode;
                }

                field.DisplayInQueryBuilder = true;
            }
        }
    }
     
}

public class DWFieldsGroup
{
    public string Key { get; set; }
    public int Index { get; set; }
    public string Fact { get; set; }
    public List<DWObjectFieldPM> FieldsList { get; set; }
}

public class DWFactGroup
{
    public string Key { get; set; }
    public int Index { get; set; }
    public List<DWFieldsGroup> FieldsGroupList { get; set; }
}