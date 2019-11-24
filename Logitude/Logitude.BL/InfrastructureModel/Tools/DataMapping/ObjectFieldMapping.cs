using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ObjectFieldMapping
    {
        public static void MapEntity(ObjectFieldPM objectFieldPM, ObjectField objectField, bool isNewState, ObjectFieldModification objectFieldModification)
        {
            if(isNewState)
                objectField.Code = objectFieldPM.Code;

            if (string.IsNullOrEmpty(objectField.Code))
            {
                objectField.Code = objectFieldPM.FieldName;
            }
            objectField.FieldCode = objectFieldPM.FieldCode;
            objectField.AutomaticField = objectFieldPM.AutomaticField;
            objectField.CanFilter = objectFieldPM.CanFilter;
            objectField.ConverterName = objectFieldPM.ConverterName;
            objectField.DataTemplateName = objectFieldPM.DataTemplateName;
            objectField.DataTypeCode = objectFieldPM.DataTypeCode;
            objectField.DependencyFilter1Type = objectFieldPM.DependencyFilter1Type;
            objectField.DependencyFilter1Value = objectFieldPM.DependencyFilter1Value;
            objectField.DependencyFilter2Type = objectFieldPM.DependencyFilter2Type;
            objectField.DependencyFilter2Value = objectFieldPM.DependencyFilter2Value;
            objectField.DependencyFilter3Type = objectFieldPM.DependencyFilter3Type;
            objectField.DependencyFilter3Value = objectFieldPM.DependencyFilter3Value;
            objectField.DisplayInList = objectFieldPM.DisplayInList;
            objectField.DisplayInLookUpIndex = objectFieldPM.DisplayInLookUpIndex;
            objectField.DisplayInSearchWindowFilters = objectFieldPM.DisplayInSearchWindowFilters;
            objectField.DisplayInSearchWindowFiltersIndex = objectFieldPM.DisplayInSearchWindowFiltersIndex;
            objectField.DisplayInSearchWindowList = objectFieldPM.DisplayInSearchWindowList;
            objectField.DisplayInSearchWindowListIndex = objectFieldPM.DisplayInSearchWindowListIndex;
            objectField.DisplayOnLookUp = objectFieldPM.DisplayOnLookUp;
            objectField.DisplayOnly = objectFieldPM.DisplayOnly;
            objectField.FullNameTextCodeId = objectFieldPM.FullNameTextCodeId;
            objectField.FieldName = objectFieldPM.FieldName;
            objectField.ShortNameTextCodeId = objectFieldPM.ShortNameTextCodeId;
            objectField.HelpTextCodeId = objectFieldPM.HelpTextCodeId;
            objectField.AgentPermissionTypeCode = objectFieldPM.AgentPermissionTypeCode;
            objectField.CustomerPermissionTypeCode = objectFieldPM.CustomerPermissionTypeCode;
            objectField.IsCustomFilter = objectFieldPM.IsCustomFilter;
            objectField.IsMulti = objectFieldPM.IsMulti;
            objectField.IsTimeFrameFilter = objectFieldPM.IsTimeFrameFilter;
            objectField.ListTextCodeId = objectFieldPM.ListTextCodeId;
            objectField.ListPropertyPath = objectFieldPM.ListPropertyPath;
            objectField.LookUpControlName = objectFieldPM.LookUpControlName;
            objectField.LookUpTableId = objectFieldPM.LookUpTableId;
            objectField.MultiLine = objectFieldPM.MultiLine;
            objectField.MultiTableId = objectFieldPM.MultiTableId;
            objectField.ObjectTableId = objectFieldPM.ObjectTableId;
            objectField.Operator = objectFieldPM.Operator;
            objectField.PMPropertyPath = objectFieldPM.PMPropertyPath;
            objectField.SystemMaxLength = objectFieldPM.SystemMaxLength;
            objectField.SystemRequired = objectFieldPM.SystemRequired;
            objectField.Tenant = objectFieldPM.Tenant;
            objectField.UniqueField = objectFieldPM.UniqueField;
            objectField.ValidForQuerySection1 = objectFieldPM.ValidForQuerySection1;
            objectField.ValidForQuerySection2 = objectFieldPM.ValidForQuerySection2;
            objectField.IsRestrictable = objectFieldPM.IsRestrictable;
            objectField.DisplayInEntityVariables = objectFieldPM.DisplayInEntityVariables;
            objectField.TextCase = objectFieldPM.TextCase;
            objectField.DigitsAfterPoint = objectFieldPM.DigitsAfterPoint;
            objectField.SearchFields = objectFieldPM.FieldName + "," + objectFieldPM.ObjectTableName + "," + objectFieldPM.FullNameTextCodeDefaultText + "," + objectFieldPM.PMPropertyPath + "," + objectFieldPM.ListPropertyPath + "," + objectFieldPM.ListTextCodeDefaultText + "," + objectFieldPM.MinLength + "," + objectFieldPM.MaxLength;
            objectField.DisplayInLookupColumnSize = objectFieldPM.DisplayInLookupColumnSize;
            objectField.ColumnHeaderTemplateName = objectFieldPM.ColumnHeaderTemplateName;
            objectField.ControlField1 = objectFieldPM.ControlField1;
            objectField.ControlField2 = objectFieldPM.ControlField2;
            objectField.ControlField3 = objectFieldPM.ControlField3;
            objectField.CustomPickListCode = objectFieldPM.CustomPickListCode;
            objectField.DependencyFilter1IsList = objectFieldPM.DependencyFilter1IsList;
            objectField.DependencyFilter2IsList = objectFieldPM.DependencyFilter2IsList;
            objectField.DependencyFilter3IsList = objectFieldPM.DependencyFilter3IsList;
            objectField.IsMaxLength = objectFieldPM.IsMaxLength;
            objectField.AllowedinAutomationConditions = objectFieldPM.AllowedinAutomationConditions;
            objectField.AutomationEmailRecipient = objectFieldPM.AutomationEmailRecipient;
            objectField.AllowedInAirlineMessaging = objectFieldPM.AllowedInAirlineMessaging;   
            objectField.CanAutomateSetValue = objectFieldPM.CanAutomateSetValue;
            objectField.HasTemplate = objectFieldPM.HasTemplate;
            objectField.HtmlHeaderComponentUrl = objectFieldPM.HtmlHeaderComponentUrl;
            objectField.HtmlListComponentUrl = objectFieldPM.HtmlListComponentUrl;
            objectField.HtmlHeaderComponentName = objectFieldPM.HtmlHeaderComponentName;
            objectField.HtmlListComponentName = objectFieldPM.HtmlListComponentName;
            objectField.AllowedInCustomerFieldsSettings = objectFieldPM.AllowedInCustomerFieldsSettings;
            objectField.GeneratedComponentPath = objectFieldPM.GeneratedComponentPath;
            objectField.DisplayInDocumentReferences = objectFieldPM.DisplayInDocumentReferences;
            objectField.CopyToDW = objectField.CopyToDW;
            objectField.EnableFullscreenTextBox = objectField.EnableFullscreenTextBox;
            objectField.DisplayInAutomationAsEnitity = objectField.DisplayInAutomationAsEnitity;
            objectField.RecordType = objectField.RecordType;

            if (objectFieldModification != null)
            {
                objectFieldModification.IsRequired = objectFieldPM.IsRequiered;
                objectFieldModification.MaxLength = objectFieldPM.MaxLength;
                objectFieldModification.MinLength = objectFieldPM.MinLength;
                objectFieldModification.UpdateDateGMT = DateTime.UtcNow;
            }

            else
            {
                objectField.IsRequiered = objectFieldPM.IsRequiered;
                objectField.MaxLength = objectFieldPM.MaxLength;
                objectField.MinLength = objectFieldPM.MinLength;
            }
        }
    }
}