using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using System.Transactions;
using Logitude.BL.InfrastructureModel.EntityLists;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ObjectFieldQuery
    {
        ObjectFieldRepository repository;
        public ObjectFieldQuery()
        {
            repository = new ObjectFieldRepository();
        }

        public ObjectFieldQuery(int tenant)
        {
            repository = new ObjectFieldRepository(tenant);
        }

        public ObjectFieldQuery(ObjectFieldRepository objectFieldsRepository)
        {
            repository = objectFieldsRepository;
        }

        public ObjectFieldPM GetSinglePM(string fieldId, int tenant)
        {
            ObjectFieldPM objectField = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                         where (a.Tenant == tenant || a.Tenant == 0)
                                         && a.Id == fieldId
                                         && a.InActive == false
                                         select new ObjectFieldPM()
                                         {  FieldCode = a.FieldCode,
                                             IsMaxLength = a.IsMaxLength,
                                             AutomaticField = a.AutomaticField,
                                             CanFilter = a.CanFilter,
                                             ConverterName = a.ConverterName,
                                             DataTemplateName = a.DataTemplateName,
                                             DataTypeCode = a.DataTypeCode,
                                             DependencyFilter1Type = a.DependencyFilter1Type,
                                             DependencyFilter1Value = a.DependencyFilter1Value,
                                             DependencyFilter2Type = a.DependencyFilter2Type,
                                             DependencyFilter2Value = a.DependencyFilter2Value,
                                             DisplayInList = a.DisplayInList,
                                             DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                             DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                             DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                             DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                             DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                             DisplayOnLookUp = a.DisplayOnLookUp,
                                             DisplayOnly = a.DisplayOnly,
                                             FullNameTextCodeId = a.FullNameTextCodeId,
                                             FieldName = a.FieldName,
                                             ShortNameTextCodeId = a.ShortNameTextCodeId,
                                             HelpTextCodeId = a.HelpTextCodeId,
                                             Id = a.Id,
                                             IsCustom = a.IsCustom,
                                             IsCustomFilter = a.IsCustomFilter,
                                             IsMulti = a.IsMulti,
                                             IsRequiered = a.IsRequiered,
                                             IsTimeFrameFilter = a.IsTimeFrameFilter,
                                             ListTextCodeId = a.ListTextCodeId,
                                             ListPropertyPath = a.ListPropertyPath,
                                             LookUpControlName = a.LookUpControlName,
                                             LookUpTableId = a.LookUpTableId,
                                             MaxLength = a.MaxLength,
                                             MinLength = a.MinLength,
                                             MultiLine = a.MultiLine,
                                             MultiTableId = a.MultiTableId,
                                             ObjectTableId = a.ObjectTableId,
                                             ObjectTableName = a.ObjectTable.Name,
                                             Operator = a.Operator,
                                             PMPropertyPath = a.PMPropertyPath,
                                             SystemMaxLength = a.SystemMaxLength,
                                             SystemRequired = a.SystemRequired,
                                             Tenant = a.Tenant,
                                             UniqueField = a.UniqueField,
                                             ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                             FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                             ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                             FullNameTextCodeCode = a.FullNameTextCodeCode,
                                             ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                             HelpTextCodeCode = a.HelpTextCodeCode,
                                             ListTextCodeCode = a.ListTextCodeCode,
                                             ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                             ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                             HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                             ValidForQuerySection2 = a.ValidForQuerySection2,
                                             ValidForQuerySection1 = a.ValidForQuerySection1,
                                             IsRestrictable = a.IsRestrictable,
                                             DisplayInEntityVariables = a.DisplayInEntityVariables,
                                             DigitsAfterPoint = a.DigitsAfterPoint,
                                             TextCase = a.TextCase,
                                             SearchFields = a.SearchFields,
                                             DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                             ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                             TenantZeroIsRequired = a.IsRequiered,
                                             TenantZeroMaxLength = a.MaxLength,
                                             TenantZeroMinLength = a.MinLength,
                                             UserTenant = tenant,
                                             DisplayLongName = a.DisplayLongName,
                                             AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                             CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                             ControlField1 = a.ControlField1,
                                             ControlField2 = a.ControlField2,
                                             CustomPickListCode = a.CustomPickListCode,
                                             NumberOfDigits = a.NumberOfDigits,
                                             DependencyFilter1IsList = a.DependencyFilter1IsList,
                                             DependencyFilter2IsList = a.DependencyFilter2IsList,
                                             FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                             AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                             AutomationEmailRecipient = a.AutomationEmailRecipient,
                                             AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                             CanAutomateSetValue = a.CanAutomateSetValue,
                                             HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                             HtmlListComponentUrl = a.HtmlListComponentUrl,
                                             HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                             HtmlListComponentName = a.HtmlListComponentName,
                                             HasTemplate = a.HasTemplate,
                                             AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                             GeneratedComponentPath = a.GeneratedComponentPath,
                                             DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                             Code = a.Code,
                                             ControlField3 = a.ControlField3,
                                             DependencyFilter3Value = a.DependencyFilter3Value,
                                             DependencyFilter3Type = a.DependencyFilter3Type,
                                             DependencyFilter3IsList = a.DependencyFilter3IsList,
                                             CopyToDW = a.CopyToDW,
                                             DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                             EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                             RecordType =a.RecordType,
                                             DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                             AdditionalQuerySections = a.AdditionalQuerySections,

                                             

                                         }).FirstOrDefault();

            ObjectFieldValidationQuery objectFieldValidationQuery = new ObjectFieldValidationQuery(tenant);
            objectField.ObjectFieldValidations = objectFieldValidationQuery.GetObjectFieldValidationPMsByObjectFieldCode(objectField.FieldCode, objectField.Tenant).ToList();
            string fieldCode = repository.GetObjectFieldCodeById(fieldId, tenant);

            ObjectFieldModification mod = (from a in repository.context.ObjectFieldModifications
                                           where a.ObjectFieldCode == fieldCode && a.Tenant == tenant
                                           select a).FirstOrDefault();
            if (mod != null)
            {
                objectField.IsRequiered = mod.IsRequired;
                objectField.MaxLength = mod.MaxLength;
                objectField.MinLength = mod.MinLength;
            }

            return objectField;
        }

        public IQueryable<ObjectFieldList> GetIQueryableEntityList(IQueryable<ObjectField> iQueryable)
        {
            IQueryable<ObjectFieldList> result = from a in iQueryable
                                                 select new ObjectFieldList()
                                                 {
                                                     IsMaxLength = a.IsMaxLength,
                                                     AutomaticField = a.AutomaticField,
                                                     CanFilter = a.CanFilter,
                                                     ConverterName = a.ConverterName,
                                                     DataTemplateName = a.DataTemplateName,
                                                     DataTypeCode = a.DataTypeCode,
                                                     DependencyFilter1Type = a.DependencyFilter1Type,
                                                     DependencyFilter1Value = a.DependencyFilter1Value,
                                                     DependencyFilter2Type = a.DependencyFilter2Type,
                                                     DependencyFilter2Value = a.DependencyFilter2Value,
                                                     DisplayInList = a.DisplayInList,
                                                     DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                     DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                     DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                     DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                     DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                     DisplayOnLookUp = a.DisplayOnLookUp,
                                                     DisplayOnly = a.DisplayOnly,
                                                     FullNameTextCodeId = a.FullNameTextCodeId,
                                                     FieldName = a.FieldName,
                                                     ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                     HelpTextCodeId = a.HelpTextCodeId,
                                                     Id = a.Id,
                                                     IsCustom = a.IsCustom,
                                                     IsCustomFilter = a.IsCustomFilter,
                                                     IsMulti = a.IsMulti,
                                                     IsRequiered = a.IsRequiered,
                                                     IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                     ListTextCodeId = a.ListTextCodeId,
                                                     ListPropertyPath = a.ListPropertyPath,
                                                     LookUpControlName = a.LookUpControlName,
                                                     LookUpTableId = a.LookUpTableId,
                                                     MaxLength = a.MaxLength,
                                                     MinLength = a.MinLength,
                                                     MultiLine = a.MultiLine,
                                                     MultiTableId = a.MultiTableId,
                                                     ObjectTableId = a.ObjectTableId,
                                                     ListTextCodeCode = a.ListTextCodeCode,
                                                     Operator = a.Operator,
                                                     PMPropertyPath = a.PMPropertyPath,
                                                     SystemMaxLength = a.SystemMaxLength,
                                                     SystemRequired = a.SystemRequired,
                                                     Tenant = a.Tenant,
                                                     UniqueField = a.UniqueField,
                                                     FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                     ValidForQuerySection2 = a.ValidForQuerySection2,
                                                     ValidForQuerySection1 = a.ValidForQuerySection1,
                                                     IsRestrictable = a.IsRestrictable,
                                                     DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                     SearchFields = a.SearchFields,
                                                     DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                     ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                     DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                     DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                     AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                     AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                     CanAutomateSetValue = a.CanAutomateSetValue,
                                                     HasTemplate = a.HasTemplate,
                                                     AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                     DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                     Code = a.Code,
                                                     DependencyFilter3Value = a.DependencyFilter3Value,
                                                     DependencyFilter3Type = a.DependencyFilter3Type,
                                                     DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                     CopyToDW = a.CopyToDW,
                                                     DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                     EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                     RecordType = a.RecordType,
                                                     DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                     FieldCode = a.FieldCode,
                                                     FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                     ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                     HelpTextCodeCode = a.HelpTextCodeCode,
                                                     AdditionalQuerySections = a.AdditionalQuerySections,


                                                 };
            return result;
        }

        public List<ObjectFieldPM> GetCustomFieldsBytableID(string tableId, int tenant, int currenttenant)
        {
            List<ObjectFieldPM> objectFields = (from a in repository.context.ObjectFields.Include("ObjectTable")
                                                where a.Tenant == tenant
                                                && a.ObjectTableId == tableId
                                                && a.IsCustom == true && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.DisplayLongName,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    IsMaxLength = a.IsMaxLength,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).ToList();

            return Get_List_Of_ObjectFields_With_Modifications_And_Validations(objectFields, tenant);
        }

        public List<ObjectFieldPM> GetCustomFieldsByTableIdForCTool(string tableId, int tenant)
        {
            List<ObjectFieldPM> objectFields = (from a in repository.context.ObjectFields.Include("ObjectTable")
                                                                                         .Include("TextCode")
                                                where a.Tenant == tenant
                                                && a.ObjectTableId == tableId
                                                && (a.DataTypeCode != "LookUp")
                                                && a.IsCustom == true && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.DisplayLongName,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    IsMaxLength = a.IsMaxLength,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode.DefaultText

                                                }).ToList();

            return objectFields;
        }

        public ObjectFieldPM GetCustomFieldsByFieldId(string fieldId, int tenant)
        {
            ObjectFieldPM objectField = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                         where a.Tenant == tenant
                                         && a.Id == fieldId
                                         && a.IsCustom == true && a.InActive == false
                                         select new ObjectFieldPM()
                                         {
                                             IsMaxLength = a.IsMaxLength,
                                             AutomaticField = a.AutomaticField,
                                             CanFilter = a.CanFilter,
                                             ConverterName = a.ConverterName,
                                             DataTemplateName = a.DataTemplateName,
                                             DataTypeCode = a.DataTypeCode,
                                             DependencyFilter1Type = a.DependencyFilter1Type,
                                             DependencyFilter1Value = a.DependencyFilter1Value,
                                             DependencyFilter2Type = a.DependencyFilter2Type,
                                             DependencyFilter2Value = a.DependencyFilter2Value,
                                             DisplayInList = a.DisplayInList,
                                             DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                             DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                             DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                             DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                             DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                             DisplayOnLookUp = a.DisplayOnLookUp,
                                             DisplayOnly = a.DisplayOnly,
                                             FullNameTextCodeId = a.FullNameTextCodeId,
                                             FieldName = a.FieldName,
                                             ShortNameTextCodeId = a.ShortNameTextCodeId,
                                             HelpTextCodeId = a.HelpTextCodeId,
                                             Id = a.Id,
                                             IsCustom = a.IsCustom,
                                             IsCustomFilter = a.IsCustomFilter,
                                             IsMulti = a.IsMulti,
                                             IsRequiered = a.IsRequiered,
                                             IsTimeFrameFilter = a.IsTimeFrameFilter,
                                             ListTextCodeId = a.ListTextCodeId,
                                             ListPropertyPath = a.ListPropertyPath,
                                             LookUpControlName = a.LookUpControlName,
                                             LookUpTableId = a.LookUpTableId,
                                             MaxLength = a.MaxLength,
                                             MinLength = a.MinLength,
                                             MultiLine = a.MultiLine,
                                             MultiTableId = a.MultiTableId,
                                             ObjectTableId = a.ObjectTableId,
                                             ObjectTableName = a.ObjectTable.Name,
                                             Operator = a.Operator,
                                             PMPropertyPath = a.PMPropertyPath,
                                             SystemMaxLength = a.SystemMaxLength,
                                             SystemRequired = a.SystemRequired,
                                             Tenant = a.Tenant,
                                             UniqueField = a.UniqueField,
                                             ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                             FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                             ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                             FullNameTextCodeCode = a.FullNameTextCodeCode,
                                             ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                             HelpTextCodeCode = a.HelpTextCodeCode,
                                             ListTextCodeCode = a.ListTextCodeCode,
                                             ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                             ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                             HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                             ValidForQuerySection2 = a.ValidForQuerySection2,
                                             ValidForQuerySection1 = a.ValidForQuerySection1,
                                             IsRestrictable = a.IsRestrictable,
                                             DisplayInEntityVariables = a.DisplayInEntityVariables,
                                             DigitsAfterPoint = a.DigitsAfterPoint,
                                             TextCase = a.TextCase,
                                             SearchFields = a.SearchFields,
                                             DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                             ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                             TenantZeroIsRequired = a.IsRequiered,
                                             TenantZeroMaxLength = a.MaxLength,
                                             TenantZeroMinLength = a.MinLength,
                                             UserTenant = tenant,
                                             DisplayLongName = a.DisplayLongName,
                                             AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                             CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                             ControlField1 = a.ControlField1,
                                             ControlField2 = a.ControlField2,
                                             CustomPickListCode = a.CustomPickListCode,
                                             NumberOfDigits = a.NumberOfDigits,
                                             DependencyFilter1IsList = a.DependencyFilter1IsList,
                                             DependencyFilter2IsList = a.DependencyFilter2IsList,
                                             FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                             AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                             AutomationEmailRecipient = a.AutomationEmailRecipient,
                                             AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                             CanAutomateSetValue = a.CanAutomateSetValue,
                                             HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                             HtmlListComponentUrl = a.HtmlListComponentUrl,
                                             HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                             HtmlListComponentName = a.HtmlListComponentName,
                                             HasTemplate = a.HasTemplate,
                                             AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                             GeneratedComponentPath = a.GeneratedComponentPath,
                                             DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                             Code = a.Code,
                                             ControlField3 = a.ControlField3,
                                             DependencyFilter3Value = a.DependencyFilter3Value,
                                             DependencyFilter3Type = a.DependencyFilter3Type,
                                             DependencyFilter3IsList = a.DependencyFilter3IsList,
                                             CopyToDW = a.CopyToDW,
                                             DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                             EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                             RecordType = a.RecordType,
                                             DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                             FieldCode = a.FieldCode,
                                             AdditionalQuerySections = a.AdditionalQuerySections,


                                         }).FirstOrDefault();

            ObjectFieldValidationQuery objectFieldValidationQuery = new ObjectFieldValidationQuery(tenant);
            objectField.ObjectFieldValidations = objectFieldValidationQuery.GetObjectFieldValidationPMsByObjectFieldCode(objectField.FieldCode, objectField.Tenant).ToList();

            return objectField;
        }

        public List<ObjectFieldPM> GetStandardFieldsFortableID(string tableId, int tenant, int currenttenant)
        {
            List<ObjectFieldPM> objectFields = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                                where (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTableId == tableId && a.IsCustom == false && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.IsMaxLength,
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.DisplayLongName,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).OrderBy(o => o.FieldName).ToList();

            return Get_List_Of_ObjectFields_With_Modifications_And_Validations(objectFields, tenant);
        }

        public ObjectFieldPM GetStandardFieldsByFieldId(string fieldId, int tenant)
        {
            string fieldCode = repository.GetObjectFieldCodeById(fieldId, tenant);
            ObjectFieldModification mod = (from a in repository.context.ObjectFieldModifications
                                           where a.ObjectFieldCode == fieldCode && a.Tenant == tenant
                                           select a).FirstOrDefault();

            ObjectFieldPM objectField = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                         where a.Id == fieldId && (a.Tenant == tenant || a.Tenant == 0) && a.InActive == false
                                         select new ObjectFieldPM()
                                         {
                                             IsMaxLength = a.IsMaxLength,
                                             AutomaticField = a.AutomaticField,
                                             CanFilter = a.CanFilter,
                                             ConverterName = a.ConverterName,
                                             DataTemplateName = a.DataTemplateName,
                                             DataTypeCode = a.DataTypeCode,
                                             DependencyFilter1Type = a.DependencyFilter1Type,
                                             DependencyFilter1Value = a.DependencyFilter1Value,
                                             DependencyFilter2Type = a.DependencyFilter2Type,
                                             DependencyFilter2Value = a.DependencyFilter2Value,
                                             DisplayInList = a.DisplayInList,
                                             DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                             DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                             DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                             DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                             DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                             DisplayOnLookUp = a.DisplayOnLookUp,
                                             DisplayOnly = a.DisplayOnly,
                                             FullNameTextCodeId = a.FullNameTextCodeId,
                                             FieldName = a.FieldName,
                                             ShortNameTextCodeId = a.ShortNameTextCodeId,
                                             HelpTextCodeId = a.HelpTextCodeId,
                                             Id = a.Id,
                                             IsCustom = a.IsCustom,
                                             IsCustomFilter = a.IsCustomFilter,
                                             IsMulti = a.IsMulti,
                                             IsRequiered = a.IsRequiered,
                                             IsTimeFrameFilter = a.IsTimeFrameFilter,
                                             ListTextCodeId = a.ListTextCodeId,
                                             ListPropertyPath = a.ListPropertyPath,
                                             LookUpControlName = a.LookUpControlName,
                                             LookUpTableId = a.LookUpTableId,
                                             MaxLength = a.MaxLength,
                                             MinLength = a.MinLength,
                                             MultiLine = a.MultiLine,
                                             MultiTableId = a.MultiTableId,
                                             ObjectTableId = a.ObjectTableId,
                                             ObjectTableName = a.ObjectTable.Name,
                                             Operator = a.Operator,
                                             PMPropertyPath = a.PMPropertyPath,
                                             SystemMaxLength = a.SystemMaxLength,
                                             SystemRequired = a.SystemRequired,
                                             Tenant = a.Tenant,
                                             UniqueField = a.UniqueField,
                                             ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                             FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                             ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                             FullNameTextCodeCode = a.FullNameTextCodeCode,
                                             ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                             HelpTextCodeCode = a.HelpTextCodeCode,
                                             ListTextCodeCode = a.ListTextCodeCode,
                                             ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                             ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                             HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                             ValidForQuerySection2 = a.ValidForQuerySection2,
                                             ValidForQuerySection1 = a.ValidForQuerySection1,
                                             IsRestrictable = a.IsRestrictable,
                                             DisplayInEntityVariables = a.DisplayInEntityVariables,
                                             DigitsAfterPoint = a.DigitsAfterPoint,
                                             TextCase = a.TextCase,
                                             SearchFields = a.SearchFields,
                                             DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                             ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                             TenantZeroIsRequired = a.IsRequiered,
                                             TenantZeroMaxLength = a.MaxLength,
                                             TenantZeroMinLength = a.MinLength,
                                             UserTenant = tenant,
                                             DisplayLongName = a.DisplayLongName,
                                             AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                             CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                             ControlField1 = a.ControlField1,
                                             ControlField2 = a.ControlField2,
                                             CustomPickListCode = a.CustomPickListCode,
                                             NumberOfDigits = a.NumberOfDigits,
                                             DependencyFilter1IsList = a.DependencyFilter1IsList,
                                             DependencyFilter2IsList = a.DependencyFilter2IsList,
                                             FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                             AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                             AutomationEmailRecipient = a.AutomationEmailRecipient,
                                             AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                             CanAutomateSetValue = a.CanAutomateSetValue,
                                             HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                             HtmlListComponentUrl = a.HtmlListComponentUrl,
                                             HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                             HtmlListComponentName = a.HtmlListComponentName,
                                             HasTemplate = a.HasTemplate,
                                             AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                             GeneratedComponentPath = a.GeneratedComponentPath,
                                             DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                             Code = a.Code,
                                             ControlField3 = a.ControlField3,
                                             DependencyFilter3Value = a.DependencyFilter3Value,
                                             DependencyFilter3Type = a.DependencyFilter3Type,
                                             DependencyFilter3IsList = a.DependencyFilter3IsList,
                                             CopyToDW = a.CopyToDW,
                                             DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                             EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                             RecordType = a.RecordType,
                                             DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                             FieldCode = a.FieldCode,
                                             AdditionalQuerySections = a.AdditionalQuerySections,


                                         }).FirstOrDefault();
            if (mod != null)
            {
                objectField.IsRequiered = mod.IsRequired;
                objectField.MaxLength = mod.MaxLength;
                objectField.MinLength = mod.MinLength;
            }
            ObjectFieldValidationQuery objectFieldValidationQuery = new ObjectFieldValidationQuery(tenant);
            objectField.ObjectFieldValidations = objectFieldValidationQuery.GetObjectFieldValidationPMsByObjectFieldCode(objectField.FieldCode, objectField.Tenant).ToList();


            return objectField;
        }

        public List<ObjectFieldPM> GetFilteredObjectFields(int tenant, int currenttenant)
        {
            List<ObjectFieldPM> objectFields = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                                where a.Tenant == tenant && a.CanFilter == true && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.IsMaxLength,
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.DisplayLongName,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).ToList();

            return Get_List_Of_ObjectFields_With_Modifications_And_Validations(objectFields, tenant);
        }


        public List<ObjectFieldPM> GetObjectFieldPMsByObjectTableName(string objectTableName, int tenant)
        {
            List<ObjectFieldPM> objectfields = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                                where (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTable.Name == objectTableName && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.IsMaxLength,
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.DisplayLongName,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).ToList();
            return objectfields;
        }

        public List<ObjectFieldPM> GetObjectFieldsAllowedinAutomationConditionsPMsByObjectTableId(string objectTableId, int tenant)
        {
            return (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                    where (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTableId == objectTableId && a.InActive == false && (a.AllowedinAutomationConditions == true || a.AutomationEmailRecipient == true || a.IsCustom)
                    select new ObjectFieldPM()
                    {
                        IsMaxLength = a.IsMaxLength,
                        AutomaticField = a.AutomaticField,
                        CanFilter = a.CanFilter,
                        ConverterName = a.ConverterName,
                        DataTemplateName = a.DataTemplateName,
                        DataTypeCode = a.DataTypeCode,
                        DependencyFilter1Type = a.DependencyFilter1Type,
                        DependencyFilter1Value = a.DependencyFilter1Value,
                        DependencyFilter2Type = a.DependencyFilter2Type,
                        DependencyFilter2Value = a.DependencyFilter2Value,
                        DisplayInList = a.DisplayInList,
                        DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                        DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                        DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                        DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                        DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                        DisplayOnLookUp = a.DisplayOnLookUp,
                        DisplayOnly = a.DisplayOnly,
                        FullNameTextCodeId = a.FullNameTextCodeId,
                        FieldName = a.FieldName,
                        ShortNameTextCodeId = a.ShortNameTextCodeId,
                        HelpTextCodeId = a.HelpTextCodeId,
                        Id = a.Id,
                        IsCustom = a.IsCustom,
                        IsCustomFilter = a.IsCustomFilter,
                        IsMulti = a.IsMulti,
                        IsRequiered = a.IsRequiered,
                        IsTimeFrameFilter = a.IsTimeFrameFilter,
                        ListTextCodeId = a.ListTextCodeId,
                        ListPropertyPath = a.ListPropertyPath,
                        LookUpControlName = a.LookUpControlName,
                        LookUpTableId = a.LookUpTableId,
                        MaxLength = a.MaxLength,
                        MinLength = a.MinLength,
                        MultiLine = a.MultiLine,
                        MultiTableId = a.MultiTableId,
                        ObjectTableId = a.ObjectTableId,
                        ObjectTableName = a.ObjectTable.Name,
                        Operator = a.Operator,
                        PMPropertyPath = a.PMPropertyPath,
                        SystemMaxLength = a.SystemMaxLength,
                        SystemRequired = a.SystemRequired,
                        Tenant = a.Tenant,
                        UniqueField = a.UniqueField,
                        ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                        FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                        ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                        FullNameTextCodeCode = a.FullNameTextCodeCode,
                        ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                        HelpTextCodeCode = a.HelpTextCodeCode,
                        ListTextCodeCode = a.ListTextCodeCode,
                        ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                        ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                        HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                        ValidForQuerySection2 = a.ValidForQuerySection2,
                        ValidForQuerySection1 = a.ValidForQuerySection1,
                        IsRestrictable = a.IsRestrictable,
                        DisplayInEntityVariables = a.DisplayInEntityVariables,
                        DigitsAfterPoint = a.DigitsAfterPoint,
                        TextCase = a.TextCase,
                        SearchFields = a.SearchFields,
                        DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                        ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                        TenantZeroIsRequired = a.IsRequiered,
                        TenantZeroMaxLength = a.MaxLength,
                        TenantZeroMinLength = a.MinLength,
                        UserTenant = tenant,
                        DisplayLongName = a.DisplayLongName,
                        AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                        CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                        ControlField1 = a.ControlField1,
                        ControlField2 = a.ControlField2,
                        CustomPickListCode = a.CustomPickListCode,
                        NumberOfDigits = a.NumberOfDigits,
                        DependencyFilter1IsList = a.DependencyFilter1IsList,
                        DependencyFilter2IsList = a.DependencyFilter2IsList,
                        FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                        AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                        AutomationEmailRecipient = a.AutomationEmailRecipient,
                        AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                        CanAutomateSetValue = a.CanAutomateSetValue,
                        HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                        HtmlListComponentUrl = a.HtmlListComponentUrl,
                        HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                        HtmlListComponentName = a.HtmlListComponentName,
                        HasTemplate = a.HasTemplate,
                        AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                        GeneratedComponentPath = a.GeneratedComponentPath,
                        DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                        Code = a.Code,
                        ControlField3 = a.ControlField3,
                        DependencyFilter3Value = a.DependencyFilter3Value,
                        DependencyFilter3Type = a.DependencyFilter3Type,
                        DependencyFilter3IsList = a.DependencyFilter3IsList,
                        CopyToDW = a.CopyToDW,
                        DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                        EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                        RecordType = a.RecordType,
                        DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                        FieldCode = a.FieldCode,
                        AdditionalQuerySections = a.AdditionalQuerySections,


                    }).ToList();
        }


        public List<ObjectFieldPM> GetCustomObjectFieldPMsByObjectTableName(string objectTableName, int tenant)
        {
            List<ObjectFieldPM> objectfields = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                                where a.Tenant == tenant && a.ObjectTable.Name == objectTableName && a.IsCustom == true && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.IsMaxLength,
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.DisplayLongName,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).ToList();
            return objectfields;
        }

        public List<ObjectFieldPM> GetAdvanceFilteredObjectFields(int tenant, string queryCode, int currenttenant)
        {
            List<ObjectFieldPM> objectFields = (from q in repository.context.AdvancedQueryFilters.Include("ObjectField").Include("ObjectField.ObjectTable_LookUpTable").Include("ObjectField.FullNameTextCode").Include("ObjectField.ShortNameTextCode").Include("ObjectField.FullNameTextCode").Include("ObjectField.ShortNameTextCode").Include("ObjectField.HelpTextCode").Include("ObjectField.ListTextCode").Include("ObjectField.ObjectTable_MultiTable")
                                                where q.Tenant == tenant && q.Query.UniqueCode == queryCode
                                                select q).Select(a => new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.ObjectField.IsMaxLength,
                                                    AutomaticField = a.ObjectField.AutomaticField,
                                                    CanFilter = a.ObjectField.CanFilter,
                                                    ConverterName = a.ObjectField.ConverterName,
                                                    DataTemplateName = a.ObjectField.DataTemplateName,
                                                    DataTypeCode = a.ObjectField.DataTypeCode,
                                                    DependencyFilter1Type = a.ObjectField.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.ObjectField.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.ObjectField.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.ObjectField.DependencyFilter2Value,
                                                    DisplayInList = a.ObjectField.DisplayInList,
                                                    DisplayInLookUpIndex = a.ObjectField.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.ObjectField.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.ObjectField.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.ObjectField.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.ObjectField.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.ObjectField.DisplayOnLookUp,
                                                    DisplayOnly = a.ObjectField.DisplayOnly,
                                                    FullNameTextCodeId = a.ObjectField.FullNameTextCodeId,
                                                    FieldName = a.ObjectField.FieldName,
                                                    ShortNameTextCodeId = a.ObjectField.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.ObjectField.HelpTextCodeId,
                                                    Id = a.ObjectField.Id,
                                                    IsCustom = a.ObjectField.IsCustom,
                                                    IsCustomFilter = a.ObjectField.IsCustomFilter,
                                                    IsMulti = a.ObjectField.IsMulti,
                                                    IsRequiered = a.ObjectField.IsRequiered,
                                                    IsTimeFrameFilter = a.ObjectField.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ObjectField.ListTextCodeId,
                                                    ListPropertyPath = a.ObjectField.ListPropertyPath,
                                                    LookUpControlName = a.ObjectField.LookUpControlName,
                                                    LookUpTableId = a.ObjectField.LookUpTableId,
                                                    MaxLength = a.ObjectField.MaxLength,
                                                    MinLength = a.ObjectField.MinLength,
                                                    MultiLine = a.ObjectField.MultiLine,
                                                    MultiTableId = a.ObjectField.MultiTableId,
                                                    ObjectTableId = a.ObjectField.ObjectTableId,
                                                    ObjectTableName = a.ObjectField.ObjectTable.Name,
                                                    Operator = a.ObjectField.Operator,
                                                    PMPropertyPath = a.ObjectField.PMPropertyPath,
                                                    SystemMaxLength = a.ObjectField.SystemMaxLength,
                                                    SystemRequired = a.ObjectField.SystemRequired,
                                                    Tenant = a.ObjectField.Tenant,
                                                    UniqueField = a.ObjectField.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectField.ObjectTable_LookUpTable != null ? a.ObjectField.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.ObjectField.FullNameTextCode != null ? a.ObjectField.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ObjectField.ShortNameTextCode != null ? a.ObjectField.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.ObjectField.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ObjectField.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.ObjectField.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ObjectField.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectField.ObjectTable_MultiTable != null ? a.ObjectField.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ObjectField.ListTextCode != null ? a.ObjectField.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.ObjectField.HelpTextCode != null ? a.ObjectField.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ObjectField.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ObjectField.ValidForQuerySection1,
                                                    IsRestrictable = a.ObjectField.IsRestrictable,
                                                    DisplayInEntityVariables = a.ObjectField.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.ObjectField.DigitsAfterPoint,
                                                    TextCase = a.ObjectField.TextCase,
                                                    DisplayInLookupColumnSize = a.ObjectField.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ObjectField.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.ObjectField.IsRequiered,
                                                    TenantZeroMaxLength = a.ObjectField.MaxLength,
                                                    TenantZeroMinLength = a.ObjectField.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.ObjectField.DisplayLongName,
                                                    AgentPermissionTypeCode = a.ObjectField.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.ObjectField.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ObjectField.ControlField1,
                                                    ControlField2 = a.ObjectField.ControlField2,
                                                    CustomPickListCode = a.ObjectField.CustomPickListCode,
                                                    NumberOfDigits = a.ObjectField.NumberOfDigits,
                                                    DependencyFilter1IsList = a.ObjectField.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.ObjectField.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.ObjectField.FullNameTextCode != null ? a.ObjectField.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.ObjectField.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.ObjectField.AutomationEmailRecipient,
                                                    CanAutomateSetValue = a.ObjectField.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.ObjectField.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.ObjectField.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.ObjectField.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.ObjectField.HtmlListComponentName,
                                                    HasTemplate = a.ObjectField.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.ObjectField.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.ObjectField.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.ObjectField.DisplayInDocumentReferences,
                                                    Code = a.ObjectField.Code,
                                                    ControlField3 = a.ObjectField.ControlField3,
                                                    DependencyFilter3Value = a.ObjectField.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.ObjectField.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.ObjectField.DependencyFilter3IsList,
                                                    CopyToDW = a.ObjectField.CopyToDW,
                                                    DisplayOnLookUpLocal = a.ObjectField.DisplayOnLookUpLocal,
                                                    RecordType = a.ObjectField.RecordType,
                                                    DisplayInAutomationAsEnitity = a.ObjectField.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.ObjectField.FieldCode,
                                                    AdditionalQuerySections = a.ObjectField.AdditionalQuerySections,

                                                }).ToList();


            return Get_List_Of_ObjectFields_With_Modifications_And_Validations(objectFields, tenant);
        }

        public IQueryable<ObjectFieldPM> GetObjectFieldPMsByTenant2(int tenant, int currenttenant)
        {
            var result = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                          where a.Tenant == tenant && a.InActive == false
                          select new ObjectFieldPM()
                          {
                              IsMaxLength = a.IsMaxLength,
                              AutomaticField = a.AutomaticField,
                              CanFilter = a.CanFilter,
                              ConverterName = a.ConverterName,
                              DataTemplateName = a.DataTemplateName,
                              DataTypeCode = a.DataTypeCode,
                              DependencyFilter1Type = a.DependencyFilter1Type,
                              DependencyFilter1Value = a.DependencyFilter1Value,
                              DependencyFilter2Type = a.DependencyFilter2Type,
                              DependencyFilter2Value = a.DependencyFilter2Value,
                              DisplayInList = a.DisplayInList,
                              DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                              DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                              DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                              DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                              DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                              DisplayOnLookUp = a.DisplayOnLookUp,
                              DisplayOnly = a.DisplayOnly,
                              FullNameTextCodeId = a.FullNameTextCodeId,
                              FieldName = a.FieldName,
                              ShortNameTextCodeId = a.ShortNameTextCodeId,
                              HelpTextCodeId = a.HelpTextCodeId,
                              Id = a.Id,
                              IsCustom = a.IsCustom,
                              IsCustomFilter = a.IsCustomFilter,
                              IsMulti = a.IsMulti,
                              IsRequiered = a.IsRequiered,
                              IsTimeFrameFilter = a.IsTimeFrameFilter,
                              ListTextCodeId = a.ListTextCodeId,
                              ListPropertyPath = a.ListPropertyPath,
                              LookUpControlName = a.LookUpControlName,
                              LookUpTableId = a.LookUpTableId,
                              MaxLength = a.MaxLength,
                              MinLength = a.MinLength,
                              MultiLine = a.MultiLine,
                              MultiTableId = a.MultiTableId,
                              ObjectTableId = a.ObjectTableId,
                              ObjectTableName = a.ObjectTable.Name,
                              Operator = a.Operator,
                              PMPropertyPath = a.PMPropertyPath,
                              SystemMaxLength = a.SystemMaxLength,
                              SystemRequired = a.SystemRequired,
                              Tenant = a.Tenant,
                              UniqueField = a.UniqueField,
                              ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                              FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                              ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                              FullNameTextCodeCode = a.FullNameTextCodeCode,
                              ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                              HelpTextCodeCode = a.HelpTextCodeCode,
                              ListTextCodeCode = a.ListTextCodeCode,
                              ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                              ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                              HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                              ValidForQuerySection2 = a.ValidForQuerySection2,
                              ValidForQuerySection1 = a.ValidForQuerySection1,
                              IsRestrictable = a.IsRestrictable,
                              DisplayInEntityVariables = a.DisplayInEntityVariables,
                              DigitsAfterPoint = a.DigitsAfterPoint,
                              TextCase = a.TextCase,
                              SearchFields = a.SearchFields,
                              DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                              ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                              TenantZeroIsRequired = a.IsRequiered,
                              TenantZeroMaxLength = a.MaxLength,
                              TenantZeroMinLength = a.MinLength,
                              DisplayLongName = a.DisplayLongName,
                              UserTenant = tenant,
                              AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                              CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                              ControlField1 = a.ControlField1,
                              ControlField2 = a.ControlField2,
                              CustomPickListCode = a.CustomPickListCode,
                              NumberOfDigits = a.NumberOfDigits,
                              DependencyFilter1IsList = a.DependencyFilter1IsList,
                              DependencyFilter2IsList = a.DependencyFilter2IsList,
                              FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                              AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                              AutomationEmailRecipient = a.AutomationEmailRecipient,
                              AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                              CanAutomateSetValue = a.CanAutomateSetValue,
                              HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                              HtmlListComponentUrl = a.HtmlListComponentUrl,
                              HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                              HtmlListComponentName = a.HtmlListComponentName,
                              HasTemplate = a.HasTemplate,
                              AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                              GeneratedComponentPath = a.GeneratedComponentPath,
                              DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                              Code = a.Code,
                              ControlField3 = a.ControlField3,
                              DependencyFilter3Value = a.DependencyFilter3Value,
                              DependencyFilter3Type = a.DependencyFilter3Type,
                              DependencyFilter3IsList = a.DependencyFilter3IsList,
                              CopyToDW = a.CopyToDW,
                              DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                              EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                              RecordType = a.RecordType,
                              DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                              FieldCode = a.FieldCode,
                              AdditionalQuerySections = a.AdditionalQuerySections,


                          });
            return result;
        }

        public IQueryable<ObjectFieldPM> GetTenantZeroObjectFieldPMs()
        {
            return (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                                where a.Tenant == 0 && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.IsMaxLength,
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    DisplayLongName = a.DisplayLongName,
                                                   
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                });

            
        }

        public List<ObjectFieldPM> GetObjectFieldPMsByTenant(int tenant, int currenttenant)
        {
            List<ObjectFieldPM> objectFields = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                                where a.Tenant == tenant && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.IsMaxLength,
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    DisplayLongName = a.DisplayLongName,
                                                    UserTenant = tenant,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).ToList();

            return Get_List_Of_ObjectFields_With_Modifications_And_Validations(objectFields, tenant);//.Take(800).ToList();
        }

        private static List<ObjectFieldPM> Get_List_Of_ObjectFields_With_Modifications_And_Validations(List<ObjectFieldPM> objectfields, int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                IWebFreightContext context = WebFreightContext.GetContext(tenant);

                TextCodeRepository textcodesRepository = new TextCodeRepository(context);
                Dictionary<string, ObjectFieldValidationPM> objectFieldValidationsDictionary = new Dictionary<string, ObjectFieldValidationPM>();
                ObjectFieldValidationQuery objectFieldValidationQuery = new ObjectFieldValidationQuery(tenant);
                objectFieldValidationsDictionary = objectFieldValidationQuery.GetObjectFieldValidationPMsByTenant(tenant).ToDictionary(objv => objv.Id, objv => objv);

                Dictionary<string, ObjectFieldModification> objectFieldModificationsDictionary = new Dictionary<string, ObjectFieldModification>();
                objectFieldModificationsDictionary = context.ObjectFieldModifications.Where(te => te.Tenant == tenant).ToDictionary(objm => objm.ObjectFieldCode, objm => objm);

                ObjectTableRepository tableRep = new ObjectTableRepository(context);

                List<TextCode> textCodes = TextCodeRepository.GetTenantTextCodesWithTenantZero(tenant);
                List<ObjectTable> objectTables = tableRep.GetObjectsByTenant(tenant).ToList();

                foreach (ObjectFieldPM objectField in objectfields)
                {
					if (objectFieldModificationsDictionary.Count != 0 && tenant != 0)
					{
						if (objectFieldModificationsDictionary.Keys.Contains(objectField.FieldCode))
						{
							ObjectFieldModification mod = objectFieldModificationsDictionary[objectField.FieldCode];

							if (mod != null)
							{
								objectField.IsRequiered = mod.IsRequired;
								objectField.MaxLength = mod.MaxLength;
								objectField.MinLength = mod.MinLength;
							}
						}
					}

                    if (objectFieldValidationsDictionary.Count != 0)
                    {
                        objectField.ObjectFieldValidations = (from d in objectFieldValidationsDictionary
                                                              where d.Value.ObjectFieldCode == objectField.FieldCode
                                                              select d.Value).ToList();
                    }

                    if (objectField.LookUpTableId != null)
                    {
                        objectField.ObjectTable_LookUpTableName = objectTables.Where(t => t.Id == objectField.LookUpTableId).FirstOrDefault().Name;
                    }

                    if (objectField.MultiTableId != null)
                    {
                        objectField.ObjectTable_MultiTableName = objectTables.Where(t => t.Id == objectField.MultiTableId).FirstOrDefault().Name;//tableRep.GetSingleObjectTable(objectField.MultiTableId, objectField.Tenant, true).Name;
                    }

                    if (objectField.FullNameTextCodeCode != null)
                    {
                        TextCode fullnamecode = textCodes.Where(t => t.Code == objectField.FullNameTextCodeCode).FirstOrDefault();
                        fullnamecode = fullnamecode ?? textcodesRepository.GetSingleTextCodeByCode(objectField.FullNameTextCodeCode);

                        if (fullnamecode != null)
                        {
                            objectField.FullNameTextCodeDefaultText = fullnamecode.DefaultText;
                            objectField.FullNameTextCodeLocalDefaultText = fullnamecode.LocalDefaultText;
                            //objectField.FullNameTextCodeCode = fullnamecode.Code;
                        }
                       
                    }

                    if (objectField.ShortNameTextCodeCode != null)
                    {
                        TextCode shortnamecode = textCodes.Where(t => t.Code == objectField.ShortNameTextCodeCode).FirstOrDefault();
                        shortnamecode = shortnamecode ?? textcodesRepository.GetSingleTextCodeByCode(objectField.ShortNameTextCodeCode);

                        if (shortnamecode != null)
                        {
                            objectField.ShortNameTextCodeDefaultText = shortnamecode.DefaultText;
                            //objectField.ShortNameTextCodeCode = shortnamecode.Code;
                        }
                       
                    }

                    if (objectField.HelpTextCodeCode != null)
                    {
                        TextCode helpcode = textCodes.Where(t => t.Id == objectField.HelpTextCodeCode).FirstOrDefault();
                        helpcode = helpcode ?? textcodesRepository.GetSingleTextCodeByCode(objectField.HelpTextCodeCode);

                        if (helpcode != null)
                        {
                            //objectField.HelpTextTextCodeCode = helpcode.Code;
                            objectField.HelpTextCodeDefaultText = helpcode.DefaultText;
                        }
                       
                    }

                    if (objectField.ListTextCodeCode != null)
                    {
                        TextCode listcode = textCodes.Where(t => t.Id == objectField.ListTextCodeCode).FirstOrDefault();
                        listcode = listcode ?? textcodesRepository.GetSingleTextCodeByCode(objectField.ListTextCodeCode);

                        if (listcode != null)
                        {
                            //objectField.ListTextCodeCode = listcode.Code;
                            objectField.ListTextCodeDefaultText = listcode.DefaultText;
                        }
                       
                    }
                }

                scope.Complete();
            }

            return objectfields;
        }

        public List<ObjectFieldPM> GetObjectFieldsByTenantAndObjectTable(int tenant, string objecttableName)
        {
            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(objecttableName, tenant);
            List<ObjectFieldPM> objectFields = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                                where (a.Tenant == tenant || a.Tenant == 0) && a.InActive == false && a.ObjectTableId == table.Id
                                                select new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.IsMaxLength,
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.DisplayLongName,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).ToList();

            return Get_List_Of_ObjectFields_With_Modifications_And_Validations(objectFields, tenant);
        }

        public static List<ObjectFieldPM> GetObjectFieldsByTenantStep(int tenant, int skip, int take)
        {
            List<ObjectFieldPM> result = new List<ObjectFieldPM>();
            List<ObjectFieldPM> allObjectFields = new List<ObjectFieldPM>();

            allObjectFields = GetTenantObjectFieldsWithTenantZero(tenant);
            result = allObjectFields.Skip(skip).Take(take).ToList();

            return result;
        }

        public static List<ObjectFieldPM> GetTenantObjectFieldsWithTenantZero(int tenant)
        {

            string listName = "tenantzeroobjectfields";
            string tenantListName = "tenantobjectfields" + tenant;

            List<ObjectFieldPM> result = new List<ObjectFieldPM>();

            List<ObjectFieldPM> currentTenantObjectFields = new List<ObjectFieldPM>();
            List<ObjectFieldPM> zeroTenantObjectFields = new List<ObjectFieldPM>();

            #region Current Tenant Fields
            if (tenant != 0)
            {
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(tenantListName) == null)
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IWebFreightContext context = WebFreightContext.GetContext(tenant);
                            currentTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable")
                                                         where (a.Tenant == tenant) && a.InActive == false
                                                         select new ObjectFieldPM()
                                                         {
                                                             IsMaxLength = a.IsMaxLength,
                                                             AutomaticField = a.AutomaticField,
                                                             CanFilter = a.CanFilter,
                                                             ConverterName = a.ConverterName,
                                                             DataTemplateName = a.DataTemplateName,
                                                             DataTypeCode = a.DataTypeCode,
                                                             DependencyFilter1Type = a.DependencyFilter1Type,
                                                             DependencyFilter1Value = a.DependencyFilter1Value,
                                                             DependencyFilter2Type = a.DependencyFilter2Type,
                                                             DependencyFilter2Value = a.DependencyFilter2Value,
                                                             DisplayInList = a.DisplayInList,
                                                             DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                             DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                             DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                             DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                             DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                             DisplayOnLookUp = a.DisplayOnLookUp,
                                                             DisplayOnly = a.DisplayOnly,
                                                             FullNameTextCodeId = a.FullNameTextCodeId,
                                                             FieldName = a.FieldName,
                                                             ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                             HelpTextCodeId = a.HelpTextCodeId,
                                                             Id = a.Id,
                                                             IsCustom = a.IsCustom,
                                                             IsCustomFilter = a.IsCustomFilter,
                                                             IsMulti = a.IsMulti,
                                                             IsRequiered = a.IsRequiered,
                                                             IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                             ListTextCodeId = a.ListTextCodeId,
                                                             ListPropertyPath = a.ListPropertyPath,
                                                             LookUpControlName = a.LookUpControlName,
                                                             LookUpTableId = a.LookUpTableId,
                                                             MaxLength = a.MaxLength,
                                                             MinLength = a.MinLength,
                                                             MultiLine = a.MultiLine,
                                                             MultiTableId = a.MultiTableId,
                                                             ObjectTableId = a.ObjectTableId,
                                                             ObjectTableName = a.ObjectTable.Name,
                                                             Operator = a.Operator,
                                                             PMPropertyPath = a.PMPropertyPath,
                                                             SystemMaxLength = a.SystemMaxLength,
                                                             SystemRequired = a.SystemRequired,
                                                             Tenant = a.Tenant,
                                                             UniqueField = a.UniqueField,
                                                             ValidForQuerySection2 = a.ValidForQuerySection2,
                                                             ValidForQuerySection1 = a.ValidForQuerySection1,
                                                             IsRestrictable = a.IsRestrictable,
                                                             DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                             DigitsAfterPoint = a.DigitsAfterPoint,
                                                             TextCase = a.TextCase,
                                                             SearchFields = a.SearchFields,
                                                             DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                             ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                             TenantZeroIsRequired = a.IsRequiered,
                                                             TenantZeroMaxLength = a.MaxLength,
                                                             TenantZeroMinLength = a.MinLength,
                                                             UserTenant = tenant,
                                                             DisplayLongName = a.DisplayLongName,
                                                             AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                             CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                             ControlField1 = a.ControlField1,
                                                             ControlField2 = a.ControlField2,
                                                             CustomPickListCode = a.CustomPickListCode,
                                                             NumberOfDigits = a.NumberOfDigits,
                                                             DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                             DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                             FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                             AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                             CanAutomateSetValue = a.CanAutomateSetValue,
                                                             HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                             HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                             HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                             HtmlListComponentName = a.HtmlListComponentName,
                                                             HasTemplate = a.HasTemplate,
                                                             AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                             GeneratedComponentPath = a.GeneratedComponentPath,
                                                             DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                             Code = a.Code,
                                                             ControlField3 = a.ControlField3,
                                                             DependencyFilter3Value = a.DependencyFilter3Value,
                                                             DependencyFilter3Type = a.DependencyFilter3Type,
                                                             DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                             CopyToDW = a.CopyToDW,
                                                             DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                             EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                             RecordType = a.RecordType,
                                                             DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                             FieldCode = a.FieldCode,
                                                             FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                             ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                             HelpTextCodeCode = a.HelpTextCodeCode,
                                                             ListTextCodeCode = a.ListTextCodeCode,
                                                             AdditionalQuerySections = a.AdditionalQuerySections,


                                                         }).ToList();

                            currentTenantObjectFields = Get_List_Of_ObjectFields_With_Modifications_And_Validations(currentTenantObjectFields, tenant);
                            scope.Complete();
                        }

                        CacheManager.CacheWrapper.Insert(tenantListName, currentTenantObjectFields, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                    else
                    {
                        currentTenantObjectFields = (List<ObjectFieldPM>)CacheManager.CacheWrapper.Get(tenantListName);
                    }
                }
                else
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(tenant);
                        currentTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable")
                                                     where (a.Tenant == tenant) && a.InActive == false
                                                     select new ObjectFieldPM()
                                                     {
                                                         IsMaxLength = a.IsMaxLength,
                                                         AutomaticField = a.AutomaticField,
                                                         CanFilter = a.CanFilter,
                                                         ConverterName = a.ConverterName,
                                                         DataTemplateName = a.DataTemplateName,
                                                         DataTypeCode = a.DataTypeCode,
                                                         DependencyFilter1Type = a.DependencyFilter1Type,
                                                         DependencyFilter1Value = a.DependencyFilter1Value,
                                                         DependencyFilter2Type = a.DependencyFilter2Type,
                                                         DependencyFilter2Value = a.DependencyFilter2Value,
                                                         DisplayInList = a.DisplayInList,
                                                         DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                         DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                         DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                         DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                         DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                         DisplayOnLookUp = a.DisplayOnLookUp,
                                                         DisplayOnly = a.DisplayOnly,
                                                         FullNameTextCodeId = a.FullNameTextCodeId,
                                                         FieldName = a.FieldName,
                                                         ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                         HelpTextCodeId = a.HelpTextCodeId,
                                                         Id = a.Id,
                                                         IsCustom = a.IsCustom,
                                                         IsCustomFilter = a.IsCustomFilter,
                                                         IsMulti = a.IsMulti,
                                                         IsRequiered = a.IsRequiered,
                                                         IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                         ListTextCodeId = a.ListTextCodeId,
                                                         ListPropertyPath = a.ListPropertyPath,
                                                         LookUpControlName = a.LookUpControlName,
                                                         LookUpTableId = a.LookUpTableId,
                                                         MaxLength = a.MaxLength,
                                                         MinLength = a.MinLength,
                                                         MultiLine = a.MultiLine,
                                                         MultiTableId = a.MultiTableId,
                                                         ObjectTableId = a.ObjectTableId,
                                                         ObjectTableName = a.ObjectTable.Name,
                                                         Operator = a.Operator,
                                                         PMPropertyPath = a.PMPropertyPath,
                                                         SystemMaxLength = a.SystemMaxLength,
                                                         SystemRequired = a.SystemRequired,
                                                         Tenant = a.Tenant,
                                                         UniqueField = a.UniqueField,
                                                         ValidForQuerySection2 = a.ValidForQuerySection2,
                                                         ValidForQuerySection1 = a.ValidForQuerySection1,
                                                         IsRestrictable = a.IsRestrictable,
                                                         DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                         DigitsAfterPoint = a.DigitsAfterPoint,
                                                         TextCase = a.TextCase,
                                                         SearchFields = a.SearchFields,
                                                         DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                         ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                         TenantZeroIsRequired = a.IsRequiered,
                                                         TenantZeroMaxLength = a.MaxLength,
                                                         TenantZeroMinLength = a.MinLength,
                                                         UserTenant = tenant,
                                                         DisplayLongName = a.DisplayLongName,
                                                         AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                         CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                         ControlField1 = a.ControlField1,
                                                         ControlField2 = a.ControlField2,
                                                         CustomPickListCode = a.CustomPickListCode,
                                                         NumberOfDigits = a.NumberOfDigits,
                                                         DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                         DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                         FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                         AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                         CanAutomateSetValue = a.CanAutomateSetValue,
                                                         HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                         HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                         HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                         HtmlListComponentName = a.HtmlListComponentName,
                                                         HasTemplate = a.HasTemplate,
                                                         AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                         GeneratedComponentPath = a.GeneratedComponentPath,
                                                         DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                         Code = a.Code,
                                                         ControlField3 = a.ControlField3,
                                                         DependencyFilter3Value = a.DependencyFilter3Value,
                                                         DependencyFilter3Type = a.DependencyFilter3Type,
                                                         DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                         CopyToDW = a.CopyToDW,
                                                         DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                         EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                         RecordType = a.RecordType,
                                                         DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                         FieldCode = a.FieldCode,
                                                         FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                         ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                         HelpTextCodeCode = a.HelpTextCodeCode,
                                                         ListTextCodeCode = a.ListTextCodeCode,
                                                         AdditionalQuerySections = a.AdditionalQuerySections,


                                                     }).ToList();

                        currentTenantObjectFields = Get_List_Of_ObjectFields_With_Modifications_And_Validations(currentTenantObjectFields, tenant);
                        scope.Complete();
                    }
                }
            }
            #endregion

            #region Tenant Zero Fields
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(listName) == null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(0);
                        zeroTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable")
                                                  where (a.Tenant == 0) && a.InActive == false
                                                  select new ObjectFieldPM()
                                                  {
                                                      IsMaxLength = a.IsMaxLength,
                                                      AutomaticField = a.AutomaticField,
                                                      CanFilter = a.CanFilter,
                                                      ConverterName = a.ConverterName,
                                                      DataTemplateName = a.DataTemplateName,
                                                      DataTypeCode = a.DataTypeCode,
                                                      DependencyFilter1Type = a.DependencyFilter1Type,
                                                      DependencyFilter1Value = a.DependencyFilter1Value,
                                                      DependencyFilter2Type = a.DependencyFilter2Type,
                                                      DependencyFilter2Value = a.DependencyFilter2Value,
                                                      DisplayInList = a.DisplayInList,
                                                      DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                      DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                      DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                      DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                      DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                      DisplayOnLookUp = a.DisplayOnLookUp,
                                                      DisplayOnly = a.DisplayOnly,
                                                      FullNameTextCodeId = a.FullNameTextCodeId,
                                                      FieldName = a.FieldName,
                                                      ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                      HelpTextCodeId = a.HelpTextCodeId,
                                                      Id = a.Id,
                                                      IsCustom = a.IsCustom,
                                                      IsCustomFilter = a.IsCustomFilter,
                                                      IsMulti = a.IsMulti,
                                                      IsRequiered = a.IsRequiered,
                                                      IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                      ListTextCodeId = a.ListTextCodeId,
                                                      ListPropertyPath = a.ListPropertyPath,
                                                      LookUpControlName = a.LookUpControlName,
                                                      LookUpTableId = a.LookUpTableId,
                                                      MaxLength = a.MaxLength,
                                                      MinLength = a.MinLength,
                                                      MultiLine = a.MultiLine,
                                                      MultiTableId = a.MultiTableId,
                                                      ObjectTableId = a.ObjectTableId,
                                                      ObjectTableName = a.ObjectTable.Name,
                                                      Operator = a.Operator,
                                                      PMPropertyPath = a.PMPropertyPath,
                                                      SystemMaxLength = a.SystemMaxLength,
                                                      SystemRequired = a.SystemRequired,
                                                      Tenant = a.Tenant,
                                                      UniqueField = a.UniqueField,
                                                      ValidForQuerySection2 = a.ValidForQuerySection2,
                                                      ValidForQuerySection1 = a.ValidForQuerySection1,
                                                      IsRestrictable = a.IsRestrictable,
                                                      DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                      DigitsAfterPoint = a.DigitsAfterPoint,
                                                      TextCase = a.TextCase,
                                                      SearchFields = a.SearchFields,
                                                      DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                      ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                      TenantZeroIsRequired = a.IsRequiered,
                                                      TenantZeroMaxLength = a.MaxLength,
                                                      TenantZeroMinLength = a.MinLength,
                                                      UserTenant = tenant,
                                                      DisplayLongName = a.DisplayLongName,
                                                      AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                      CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                      ControlField1 = a.ControlField1,
                                                      ControlField2 = a.ControlField2,
                                                      CustomPickListCode = a.CustomPickListCode,
                                                      NumberOfDigits = a.NumberOfDigits,
                                                      DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                      DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                      FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                      AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                      CanAutomateSetValue = a.CanAutomateSetValue,
                                                      HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                      HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                      HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                      HtmlListComponentName = a.HtmlListComponentName,
                                                      HasTemplate = a.HasTemplate,
                                                      AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                      GeneratedComponentPath = a.GeneratedComponentPath,
                                                      DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                      Code = a.Code,
                                                      ControlField3 = a.ControlField3,
                                                      DependencyFilter3Value = a.DependencyFilter3Value,
                                                      DependencyFilter3Type = a.DependencyFilter3Type,
                                                      DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                      CopyToDW = a.CopyToDW,
                                                      DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                      EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                      RecordType = a.RecordType,
                                                      DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                      FieldCode = a.FieldCode,
                                                      FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                      ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                      HelpTextCodeCode = a.HelpTextCodeCode,
                                                      ListTextCodeCode = a.ListTextCodeCode,

                                                      AdditionalQuerySections = a.AdditionalQuerySections,

                                                  }).ToList();

                        zeroTenantObjectFields = Get_List_Of_ObjectFields_With_Modifications_And_Validations(zeroTenantObjectFields, 0);

                        scope.Complete();
                    }

                    CacheManager.CacheWrapper.Insert(listName, zeroTenantObjectFields, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    zeroTenantObjectFields = (List<ObjectFieldPM>)CacheManager.CacheWrapper.Get(listName);
                }
            }
            else
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    IWebFreightContext context = WebFreightContext.GetContext(0);
                    zeroTenantObjectFields = (from a in context.ObjectFields.Include("ObjectTable")
                                              where (a.Tenant == 0) && a.InActive == false
                                              select new ObjectFieldPM()
                                              {
                                                  IsMaxLength = a.IsMaxLength,
                                                  AutomaticField = a.AutomaticField,
                                                  CanFilter = a.CanFilter,
                                                  ConverterName = a.ConverterName,
                                                  DataTemplateName = a.DataTemplateName,
                                                  DataTypeCode = a.DataTypeCode,
                                                  DependencyFilter1Type = a.DependencyFilter1Type,
                                                  DependencyFilter1Value = a.DependencyFilter1Value,
                                                  DependencyFilter2Type = a.DependencyFilter2Type,
                                                  DependencyFilter2Value = a.DependencyFilter2Value,
                                                  DisplayInList = a.DisplayInList,
                                                  DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                  DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                  DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                  DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                  DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                  DisplayOnLookUp = a.DisplayOnLookUp,
                                                  DisplayOnly = a.DisplayOnly,
                                                  FullNameTextCodeId = a.FullNameTextCodeId,
                                                  FieldName = a.FieldName,
                                                  ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                  HelpTextCodeId = a.HelpTextCodeId,
                                                  Id = a.Id,
                                                  IsCustom = a.IsCustom,
                                                  IsCustomFilter = a.IsCustomFilter,
                                                  IsMulti = a.IsMulti,
                                                  IsRequiered = a.IsRequiered,
                                                  IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                  ListTextCodeId = a.ListTextCodeId,
                                                  ListPropertyPath = a.ListPropertyPath,
                                                  LookUpControlName = a.LookUpControlName,
                                                  LookUpTableId = a.LookUpTableId,
                                                  MaxLength = a.MaxLength,
                                                  MinLength = a.MinLength,
                                                  MultiLine = a.MultiLine,
                                                  MultiTableId = a.MultiTableId,
                                                  ObjectTableId = a.ObjectTableId,
                                                  ObjectTableName = a.ObjectTable.Name,
                                                  Operator = a.Operator,
                                                  PMPropertyPath = a.PMPropertyPath,
                                                  SystemMaxLength = a.SystemMaxLength,
                                                  SystemRequired = a.SystemRequired,
                                                  Tenant = a.Tenant,
                                                  UniqueField = a.UniqueField,
                                                  ValidForQuerySection2 = a.ValidForQuerySection2,
                                                  ValidForQuerySection1 = a.ValidForQuerySection1,
                                                  IsRestrictable = a.IsRestrictable,
                                                  DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                  DigitsAfterPoint = a.DigitsAfterPoint,
                                                  TextCase = a.TextCase,
                                                  SearchFields = a.SearchFields,
                                                  DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                  ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                  TenantZeroIsRequired = a.IsRequiered,
                                                  TenantZeroMaxLength = a.MaxLength,
                                                  TenantZeroMinLength = a.MinLength,
                                                  UserTenant = tenant,
                                                  DisplayLongName = a.DisplayLongName,
                                                  AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                  CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                  ControlField1 = a.ControlField1,
                                                  ControlField2 = a.ControlField2,
                                                  CustomPickListCode = a.CustomPickListCode,
                                                  NumberOfDigits = a.NumberOfDigits,
                                                  DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                  DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                  FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                  AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                  CanAutomateSetValue = a.CanAutomateSetValue,
                                                  HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                  HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                  HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                  HtmlListComponentName = a.HtmlListComponentName,
                                                  HasTemplate = a.HasTemplate,
                                                  AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                  GeneratedComponentPath = a.GeneratedComponentPath,
                                                  DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                  Code = a.Code,
                                                  ControlField3 = a.ControlField3,
                                                  DependencyFilter3Value = a.DependencyFilter3Value,
                                                  DependencyFilter3Type = a.DependencyFilter3Type,
                                                  DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                  CopyToDW = a.CopyToDW,
                                                  DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                  EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                  RecordType = a.RecordType,
                                                  DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                  FieldCode = a.FieldCode,
                                                  FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                  ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                  HelpTextCodeCode = a.HelpTextCodeCode,
                                                  ListTextCodeCode = a.ListTextCodeCode,
                                                  AdditionalQuerySections = a.AdditionalQuerySections,


                                              }).ToList();

                    zeroTenantObjectFields = Get_List_Of_ObjectFields_With_Modifications_And_Validations(zeroTenantObjectFields, 0);
                    scope.Complete();
                }
            }
            #endregion

            result = zeroTenantObjectFields.Concat(currentTenantObjectFields).ToList();
            return result;
        }

        public ObjectFieldPM GetSinglePMByNameAndTable(string fieldName, string tableId, int tenant)
        {
            ObjectFieldPM objectField = (from a in repository.context.ObjectFields.Include("FullNameTextCode")
                                         where a.Tenant == tenant
                                         && a.FieldName == fieldName
                                         && a.ObjectTableId == tableId
                                         select new ObjectFieldPM()
                                         {
                                             IsMaxLength = a.IsMaxLength,
                                             AutomaticField = a.AutomaticField,
                                             CanFilter = a.CanFilter,
                                             ConverterName = a.ConverterName,
                                             DataTemplateName = a.DataTemplateName,
                                             DataTypeCode = a.DataTypeCode,
                                             DependencyFilter1Type = a.DependencyFilter1Type,
                                             DependencyFilter1Value = a.DependencyFilter1Value,
                                             DependencyFilter2Type = a.DependencyFilter2Type,
                                             DependencyFilter2Value = a.DependencyFilter2Value,
                                             DisplayInList = a.DisplayInList,
                                             DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                             DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                             DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                             DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                             DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                             DisplayOnLookUp = a.DisplayOnLookUp,
                                             DisplayOnly = a.DisplayOnly,
                                             FullNameTextCodeId = a.FullNameTextCodeId,
                                             FieldName = a.FieldName,
                                             ShortNameTextCodeId = a.ShortNameTextCodeId,
                                             HelpTextCodeId = a.HelpTextCodeId,
                                             Id = a.Id,
                                             IsCustom = a.IsCustom,
                                             IsCustomFilter = a.IsCustomFilter,
                                             IsMulti = a.IsMulti,
                                             IsRequiered = a.IsRequiered,
                                             IsTimeFrameFilter = a.IsTimeFrameFilter,
                                             ListTextCodeId = a.ListTextCodeId,
                                             ListPropertyPath = a.ListPropertyPath,
                                             LookUpControlName = a.LookUpControlName,
                                             LookUpTableId = a.LookUpTableId,
                                             MaxLength = a.MaxLength,
                                             MinLength = a.MinLength,
                                             MultiLine = a.MultiLine,
                                             MultiTableId = a.MultiTableId,
                                             ObjectTableId = a.ObjectTableId,
                                             ObjectTableName = a.ObjectTable.Name,
                                             Operator = a.Operator,
                                             PMPropertyPath = a.PMPropertyPath,
                                             SystemMaxLength = a.SystemMaxLength,
                                             SystemRequired = a.SystemRequired,
                                             Tenant = a.Tenant,
                                             UniqueField = a.UniqueField,
                                             FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                             FullNameTextCodeCode = a.FullNameTextCodeCode,
                                             ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                             HelpTextCodeCode = a.HelpTextCodeCode,
                                             ListTextCodeCode = a.ListTextCodeCode,
                                             ValidForQuerySection2 = a.ValidForQuerySection2,
                                             ValidForQuerySection1 = a.ValidForQuerySection1,
                                             IsRestrictable = a.IsRestrictable,
                                             DisplayInEntityVariables = a.DisplayInEntityVariables,
                                             DigitsAfterPoint = a.DigitsAfterPoint,
                                             TextCase = a.TextCase,
                                             SearchFields = a.SearchFields,
                                             DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                             ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                             TenantZeroIsRequired = a.IsRequiered,
                                             TenantZeroMaxLength = a.MaxLength,
                                             TenantZeroMinLength = a.MinLength,
                                             UserTenant = tenant,
                                             DisplayLongName = a.DisplayLongName,
                                             AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                             CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                             ControlField1 = a.ControlField1,
                                             ControlField2 = a.ControlField2,
                                             CustomPickListCode = a.CustomPickListCode,
                                             NumberOfDigits = a.NumberOfDigits,
                                             DependencyFilter1IsList = a.DependencyFilter1IsList,
                                             DependencyFilter2IsList = a.DependencyFilter2IsList,
                                             FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                             DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                             EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                             RecordType = a.RecordType,
                                             DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                             FieldCode = a.FieldCode,
                                             AdditionalQuerySections = a.AdditionalQuerySections,

                                         }).FirstOrDefault();

            return objectField;
        }


        public List<ObjectFieldPM> GetCustomObjectFieldsByTenantAndObjectTable(int tenant, string objecttableName)
        {
            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(objecttableName, tenant);
            List<ObjectFieldPM> objectFields = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode")
                                                where a.Tenant == tenant  && a.InActive == false && a.ObjectTableId == table.Id && a.IsCustom == true
                                                select new ObjectFieldPM()
                                                {
                                                   
                                                    DataTypeCode = a.DataTypeCode,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Tenant = a.Tenant,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    DisplayLongName = a.DisplayLongName,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    Code = a.Code,
                                                    FieldCode = a.FieldCode,
                                                    CustomPickListCode =a.CustomPickListCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).ToList();

            return objectFields;
        }



        public List<ObjectFieldPM> GetEntityAuomationAllowedinAutomationConditionsObjectFieldPMsByEntityTableIds(List<string> entityTableIds, int tenant)
        {
            List<ObjectFieldPM> objectfields = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                                where (a.Tenant == tenant || a.Tenant == 0) && a.AllowedinAutomationConditions == true && entityTableIds.Contains(a.ObjectTableId) && a.InActive == false
                                                select new ObjectFieldPM()
                                                {
                                                    IsMaxLength = a.IsMaxLength,
                                                    AutomaticField = a.AutomaticField,
                                                    CanFilter = a.CanFilter,
                                                    ConverterName = a.ConverterName,
                                                    DataTemplateName = a.DataTemplateName,
                                                    DataTypeCode = a.DataTypeCode,
                                                    DependencyFilter1Type = a.DependencyFilter1Type,
                                                    DependencyFilter1Value = a.DependencyFilter1Value,
                                                    DependencyFilter2Type = a.DependencyFilter2Type,
                                                    DependencyFilter2Value = a.DependencyFilter2Value,
                                                    DisplayInList = a.DisplayInList,
                                                    DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                                    DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                                    DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                                    DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                                    DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                                    DisplayOnLookUp = a.DisplayOnLookUp,
                                                    DisplayOnly = a.DisplayOnly,
                                                    FullNameTextCodeId = a.FullNameTextCodeId,
                                                    FieldName = a.FieldName,
                                                    ShortNameTextCodeId = a.ShortNameTextCodeId,
                                                    HelpTextCodeId = a.HelpTextCodeId,
                                                    Id = a.Id,
                                                    IsCustom = a.IsCustom,
                                                    IsCustomFilter = a.IsCustomFilter,
                                                    IsMulti = a.IsMulti,
                                                    IsRequiered = a.IsRequiered,
                                                    IsTimeFrameFilter = a.IsTimeFrameFilter,
                                                    ListTextCodeId = a.ListTextCodeId,
                                                    ListPropertyPath = a.ListPropertyPath,
                                                    LookUpControlName = a.LookUpControlName,
                                                    LookUpTableId = a.LookUpTableId,
                                                    MaxLength = a.MaxLength,
                                                    MinLength = a.MinLength,
                                                    MultiLine = a.MultiLine,
                                                    MultiTableId = a.MultiTableId,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ObjectTableName = a.ObjectTable.Name,
                                                    Operator = a.Operator,
                                                    PMPropertyPath = a.PMPropertyPath,
                                                    SystemMaxLength = a.SystemMaxLength,
                                                    SystemRequired = a.SystemRequired,
                                                    Tenant = a.Tenant,
                                                    UniqueField = a.UniqueField,
                                                    ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                                    FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                                    ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                    ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                                    HelpTextCodeCode = a.HelpTextCodeCode,
                                                    ListTextCodeCode = a.ListTextCodeCode,
                                                    ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                                    ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                                    HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                                    ValidForQuerySection2 = a.ValidForQuerySection2,
                                                    ValidForQuerySection1 = a.ValidForQuerySection1,
                                                    IsRestrictable = a.IsRestrictable,
                                                    DisplayInEntityVariables = a.DisplayInEntityVariables,
                                                    DigitsAfterPoint = a.DigitsAfterPoint,
                                                    TextCase = a.TextCase,
                                                    SearchFields = a.SearchFields,
                                                    DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                                    ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                                    TenantZeroIsRequired = a.IsRequiered,
                                                    TenantZeroMaxLength = a.MaxLength,
                                                    TenantZeroMinLength = a.MinLength,
                                                    UserTenant = tenant,
                                                    DisplayLongName = a.DisplayLongName,
                                                    AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                                    CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                                    ControlField1 = a.ControlField1,
                                                    ControlField2 = a.ControlField2,
                                                    CustomPickListCode = a.CustomPickListCode,
                                                    NumberOfDigits = a.NumberOfDigits,
                                                    DependencyFilter1IsList = a.DependencyFilter1IsList,
                                                    DependencyFilter2IsList = a.DependencyFilter2IsList,
                                                    FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                                    AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                                    AutomationEmailRecipient = a.AutomationEmailRecipient,
                                                    AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                                    CanAutomateSetValue = a.CanAutomateSetValue,
                                                    HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                                    HtmlListComponentUrl = a.HtmlListComponentUrl,
                                                    HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                                    HtmlListComponentName = a.HtmlListComponentName,
                                                    HasTemplate = a.HasTemplate,
                                                    AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                                    GeneratedComponentPath = a.GeneratedComponentPath,
                                                    DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                                    Code = a.Code,
                                                    ControlField3 = a.ControlField3,
                                                    DependencyFilter3Value = a.DependencyFilter3Value,
                                                    DependencyFilter3Type = a.DependencyFilter3Type,
                                                    DependencyFilter3IsList = a.DependencyFilter3IsList,
                                                    CopyToDW = a.CopyToDW,
                                                    DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                                    EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                                    RecordType = a.RecordType,
                                                    DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,
                                                    FieldCode = a.FieldCode,
                                                    AdditionalQuerySections = a.AdditionalQuerySections,


                                                }).ToList();
            return objectfields;
        }

        public ObjectFieldPM GetObjectFieldByFieldCode(string fieldCode, int tenant)
        {
            ObjectFieldPM objectField = (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                                         where (a.Tenant == tenant || a.Tenant == 0)
                                         && a.FieldCode == fieldCode
                                         && a.InActive == false
                                         select new ObjectFieldPM()
                                         {
                                             FieldCode = a.FieldCode,
                                             IsMaxLength = a.IsMaxLength,
                                             AutomaticField = a.AutomaticField,
                                             CanFilter = a.CanFilter,
                                             ConverterName = a.ConverterName,
                                             DataTemplateName = a.DataTemplateName,
                                             DataTypeCode = a.DataTypeCode,
                                             DependencyFilter1Type = a.DependencyFilter1Type,
                                             DependencyFilter1Value = a.DependencyFilter1Value,
                                             DependencyFilter2Type = a.DependencyFilter2Type,
                                             DependencyFilter2Value = a.DependencyFilter2Value,
                                             DisplayInList = a.DisplayInList,
                                             DisplayInLookUpIndex = a.DisplayInLookUpIndex,
                                             DisplayInSearchWindowFilters = a.DisplayInSearchWindowFilters,
                                             DisplayInSearchWindowFiltersIndex = a.DisplayInSearchWindowFiltersIndex,
                                             DisplayInSearchWindowList = a.DisplayInSearchWindowList,
                                             DisplayInSearchWindowListIndex = a.DisplayInSearchWindowListIndex,
                                             DisplayOnLookUp = a.DisplayOnLookUp,
                                             DisplayOnly = a.DisplayOnly,
                                             FullNameTextCodeId = a.FullNameTextCodeId,
                                             FieldName = a.FieldName,
                                             ShortNameTextCodeId = a.ShortNameTextCodeId,
                                             HelpTextCodeId = a.HelpTextCodeId,
                                             Id = a.Id,
                                             IsCustom = a.IsCustom,
                                             IsCustomFilter = a.IsCustomFilter,
                                             IsMulti = a.IsMulti,
                                             IsRequiered = a.IsRequiered,
                                             IsTimeFrameFilter = a.IsTimeFrameFilter,
                                             ListTextCodeId = a.ListTextCodeId,
                                             ListPropertyPath = a.ListPropertyPath,
                                             LookUpControlName = a.LookUpControlName,
                                             LookUpTableId = a.LookUpTableId,
                                             MaxLength = a.MaxLength,
                                             MinLength = a.MinLength,
                                             MultiLine = a.MultiLine,
                                             MultiTableId = a.MultiTableId,
                                             ObjectTableId = a.ObjectTableId,
                                             ObjectTableName = a.ObjectTable.Name,
                                             Operator = a.Operator,
                                             PMPropertyPath = a.PMPropertyPath,
                                             SystemMaxLength = a.SystemMaxLength,
                                             SystemRequired = a.SystemRequired,
                                             Tenant = a.Tenant,
                                             UniqueField = a.UniqueField,
                                             ObjectTable_LookUpTableName = a.ObjectTable_LookUpTable != null ? a.ObjectTable_LookUpTable.Name : null,
                                             FullNameTextCodeDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.DefaultText : null,
                                             ShortNameTextCodeDefaultText = a.ShortNameTextCode != null ? a.ShortNameTextCode.DefaultText : null,
                                             FullNameTextCodeCode = a.FullNameTextCodeCode,
                                             ShortNameTextCodeCode = a.ShortNameTextCodeCode,
                                             HelpTextCodeCode = a.HelpTextCodeCode,
                                             ListTextCodeCode = a.ListTextCodeCode,
                                             ObjectTable_MultiTableName = a.ObjectTable_MultiTable != null ? a.ObjectTable_MultiTable.Name : null,
                                             ListTextCodeDefaultText = a.ListTextCode != null ? a.ListTextCode.DefaultText : null,
                                             HelpTextCodeDefaultText = a.HelpTextCode != null ? a.HelpTextCode.DefaultText : null,
                                             ValidForQuerySection2 = a.ValidForQuerySection2,
                                             ValidForQuerySection1 = a.ValidForQuerySection1,
                                             IsRestrictable = a.IsRestrictable,
                                             DisplayInEntityVariables = a.DisplayInEntityVariables,
                                             DigitsAfterPoint = a.DigitsAfterPoint,
                                             TextCase = a.TextCase,
                                             SearchFields = a.SearchFields,
                                             DisplayInLookupColumnSize = a.DisplayInLookupColumnSize,
                                             ColumnHeaderTemplateName = a.ColumnHeaderTemplateName,
                                             TenantZeroIsRequired = a.IsRequiered,
                                             TenantZeroMaxLength = a.MaxLength,
                                             TenantZeroMinLength = a.MinLength,
                                             UserTenant = tenant,
                                             DisplayLongName = a.DisplayLongName,
                                             AgentPermissionTypeCode = a.AgentPermissionTypeCode,
                                             CustomerPermissionTypeCode = a.CustomerPermissionTypeCode,
                                             ControlField1 = a.ControlField1,
                                             ControlField2 = a.ControlField2,
                                             CustomPickListCode = a.CustomPickListCode,
                                             NumberOfDigits = a.NumberOfDigits,
                                             DependencyFilter1IsList = a.DependencyFilter1IsList,
                                             DependencyFilter2IsList = a.DependencyFilter2IsList,
                                             FullNameTextCodeLocalDefaultText = a.FullNameTextCode != null ? a.FullNameTextCode.LocalDefaultText : null,
                                             AllowedinAutomationConditions = a.AllowedinAutomationConditions,
                                             AutomationEmailRecipient = a.AutomationEmailRecipient,
                                             AllowedInAirlineMessaging = a.AllowedInAirlineMessaging,
                                             CanAutomateSetValue = a.CanAutomateSetValue,
                                             HtmlHeaderComponentUrl = a.HtmlHeaderComponentUrl,
                                             HtmlListComponentUrl = a.HtmlListComponentUrl,
                                             HtmlHeaderComponentName = a.HtmlHeaderComponentName,
                                             HtmlListComponentName = a.HtmlListComponentName,
                                             HasTemplate = a.HasTemplate,
                                             AllowedInCustomerFieldsSettings = a.AllowedInCustomerFieldsSettings,
                                             GeneratedComponentPath = a.GeneratedComponentPath,
                                             DisplayInDocumentReferences = a.DisplayInDocumentReferences,
                                             Code = a.Code,
                                             ControlField3 = a.ControlField3,
                                             DependencyFilter3Value = a.DependencyFilter3Value,
                                             DependencyFilter3Type = a.DependencyFilter3Type,
                                             DependencyFilter3IsList = a.DependencyFilter3IsList,
                                             CopyToDW = a.CopyToDW,
                                             DisplayOnLookUpLocal = a.DisplayOnLookUpLocal,
                                             EnableFullscreenTextBox = a.EnableFullscreenTextBox,
                                             RecordType = a.RecordType,
                                             DisplayInAutomationAsEnitity = a.DisplayInAutomationAsEnitity,

                                             AdditionalQuerySections = a.AdditionalQuerySections,


                                         }).FirstOrDefault();

            ObjectFieldValidationQuery objectFieldValidationQuery = new ObjectFieldValidationQuery(tenant);
            objectField.ObjectFieldValidations = objectFieldValidationQuery.GetObjectFieldValidationPMsByObjectFieldCode(objectField.FieldCode, objectField.Tenant).ToList();

            ObjectFieldModification mod = (from a in repository.context.ObjectFieldModifications
                                           where a.ObjectFieldCode == fieldCode && a.Tenant == tenant
                                           select a).FirstOrDefault();
            if (mod != null)
            {
                objectField.IsRequiered = mod.IsRequired;
                objectField.MaxLength = mod.MaxLength;
                objectField.MinLength = mod.MinLength;
            }

            return objectField;
        }



        public List<ObjectFieldList> GetObjectFieldsForAutomations()
        {
            return (from a in repository.context.ObjectFields.Include("ObjectTable_LookUpTable").Include("FullNameTextCode").Include("ShortNameTextCode").Include("ListTextCode").Include("HelpTextCode").Include("ObjectTable").Include("ObjectTable_MultiTable")
                    where  (a.AllowedinAutomationConditions == true || a.DisplayInAutomationAsEnitity == true || a.AutomationEmailRecipient == true || a.IsCustom || a.FieldName == "DescriptionOfGoods" || a.FieldName == "MainCarriageFinalDestinationETA" || a.FieldName == "MainCarriageFinalDestinationATA" || a.FieldName == "MainCarriageETD" || a.FieldName == "MainCarriageATD")
                    select new ObjectFieldList()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        FieldCode = a.FieldCode,
                    }).ToList();
        }





        public List<ObjectFieldList> GetObjectFieldsUsedInDWData(int tenant)
        {
            List<ObjectFieldList> objectfields = (from a in repository.context.ObjectFields
                                                where (a.Tenant == tenant || a.Tenant == 0) &&   a.CopyToDW

                                                select new ObjectFieldList()
                                                {
                                                    FieldCode = a.FieldCode , 
                                                    FullNameTextCodeCode = a.FullNameTextCodeCode,
                                                }).ToList();
            return objectfields;
        }




    }
}