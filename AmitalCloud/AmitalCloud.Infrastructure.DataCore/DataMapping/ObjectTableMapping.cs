using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class ObjectTableMapping
    {
        public static void MapEntity(ObjectTablePM objectTablePM, ObjectTable objectTable, bool isNewState)
        {
            objectTable.LookUp1 = objectTablePM.LookUp1;
            objectTable.LookUp2 = objectTablePM.LookUp2;
            objectTable.AutoCompleteSearchWindow = objectTablePM.AutoCompleteSearchWindow;
            objectTable.DependencyFilter1 = objectTablePM.DependencyFilter1;
            objectTable.DependencyFilter2 = objectTablePM.DependencyFilter2;
            objectTable.DependencyFilter3 = objectTablePM.DependencyFilter3;
            objectTable.EditableFromAutoCompleteWindow = objectTablePM.EditableFromAutoCompleteWindow;
            objectTable.HeaderScreenId = objectTablePM.HeaderScreenId;
            objectTable.IsClosed = objectTablePM.IsClosed;
            objectTable.IsNewWizard = objectTablePM.IsNewWizard;
            objectTable.KeyPropertyPath = objectTablePM.KeyPropertyPath;
            objectTable.LastUpdateDate = objectTablePM.LastUpdateDate;
            objectTable.Name = objectTablePM.Name;
            objectTable.NewWizardControlName = objectTablePM.NewWizardControlName;
            objectTable.Tenant = objectTablePM.Tenant;
            objectTable.CacheOnClient = objectTablePM.CacheOnClient;
            objectTable.HasCounter = objectTablePM.HasCounter;
            objectTable.EnableEditFromLOV = objectTablePM.EnableEditFromLOV;
            objectTable.EnableAddFromLOV = objectTablePM.EnableAddFromLOV;
            objectTable.IsMain = objectTablePM.IsMain;
            objectTable.IsRestrictable = objectTablePM.IsRestrictable;
            objectTable.IsAutoComplete = objectTablePM.IsAutoComplete;
            objectTable.SortingByObjectField = objectTablePM.SortingByObjectField;
            objectTable.DBTableName = objectTablePM.DBTableName;
            objectTable.DBTableShortName = objectTablePM.DBTableShortName;
            objectTable.HasCustomFields = objectTablePM.HasCustomFields;
            objectTable.CustomFieldsCount = objectTablePM.CustomFieldsCount;
            objectTable.DescriptionTextCodeId = objectTablePM.DescriptionTextCodeId;
            objectTable.SearchFields = objectTablePM.Name + "," + objectTablePM.DBTableName + "," + objectTablePM.NewWizardControlName + "," + objectTablePM.KeyPropertyPath + "," + objectTablePM.SortingByObjectField;
            objectTable.IsSaveButtonVisible = objectTablePM.IsSaveButtonVisible;
            objectTable.MainTipCode = objectTablePM.MainTipCode;
            objectTable.EnableSecurity = objectTablePM.EnableSecurity;
            objectTable.ObjectTableTypeCode = objectTablePM.ObjectTableTypeCode;
            objectTable.IsComposition = objectTablePM.IsComposition;
            objectTable.MaxNumberOfCustomFields = objectTablePM.MaxNumberOfCustomFields;
            objectTable.AllowCustomFields = objectTablePM.AllowCustomFields;
            objectTable.HasDocuments = objectTablePM.HasDocuments;
            objectTable.HasCustomValidator = objectTablePM.HasCustomValidator;
            objectTable.ClientModuleName = objectTablePM.ClientModuleName;
            objectTable.ServerModuleName = objectTablePM.ServerModuleName;
            objectTable.NewWizardComponentPath = objectTablePM.NewWizardComponentPath;
            objectTable.HasHelper = objectTablePM.HasHelper;
            objectTable.HasShortTitle = objectTablePM.HasShortTitle;
            objectTable.HasMenuButtons = objectTablePM.HasMenuButtons;
            objectTable.HasFiltersMenu = objectTablePM.HasFiltersMenu;
            objectTable.EntityResourceLastUpdate = objectTablePM.EntityResourceLastUpdate;
            objectTable.SplitComponentPath = objectTablePM.SplitComponentPath;
            objectTable.AllowedForComputingPartners = objectTablePM.AllowedForComputingPartners;
            objectTable.CodeField = objectTablePM.CodeField;
            objectTable.NameField = objectTablePM.NameField;
            objectTable.DisableSearchBox = objectTablePM.DisableSearchBox;
            objectTable.AllowedInTicket = objectTablePM.AllowedInTicket;
            objectTable.IsTabsHidden = objectTablePM.IsTabsHidden;
            objectTable.DescriptionTextCodeCode = objectTablePM.DescriptionTextCodeCode;
            objectTable.NewButtonTextCodeCode = objectTablePM.NewButtonTextCodeCode;
            objectTable.HeaderScreenCode = objectTablePM.HeaderScreenCode;
            objectTable.ParentObjectTableName = objectTablePM.ParentObjectTableName;
            objectTable.AvailableInCustomization = objectTablePM.AvailableInCustomization;
            objectTable.ParentObjectTableId = objectTablePM.ParentObjectTableId;
            objectTable.IsCustom = objectTablePM.IsCustom;
            objectTable.SupportSubEntity = objectTablePM.SupportSubEntity;
            objectTable.ApplyGenericCustomFields = objectTablePM.ApplyGenericCustomFields;
            objectTable.FullNameTextCodeId = objectTablePM.FullNameTextCodeId;
            objectTable.FullNameTextCodeCode = objectTablePM.FullNameTextCodeCode;
            objectTable.AvailableInDocumentTypes = objectTablePM.AvailableInDocumentTypes;
        }
    }
}