"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CacheLogComponent_1 = require("./Components/Maintenance/CacheLogComponent");
// Directives
var FocusMeDirective_1 = require("./Utilities/FocusMeDirective");
var LocationDirective_1 = require("./Utilities/LocationDirective");
var FixedPositionDirective_1 = require("./Utilities/FixedPositionDirective");
//rtl directives
var FloatStartDirective_1 = require("./Utilities/RTLDirectives/FloatStartDirective");
var FloatEndDirective_1 = require("./Utilities/RTLDirectives/FloatEndDirective");
var TextAlignStart_1 = require("./Utilities/RTLDirectives/TextAlignStart");
var TextAlignEnd_1 = require("./Utilities/RTLDirectives/TextAlignEnd");
// Pipes.
var DateTimeToColorPipe_1 = require("./Pipes/DateTimePipes/DateTimeToColorPipe");
var NumbersPipe_1 = require("./Pipes/NumbersPipe");
var PaddingPipe_1 = require("./Pipes/PaddingPipe");
var ReplacePipe_1 = require("./Pipes/ReplacePipe");
var StringToColorPipe_1 = require("./Pipes/StringToColorPipe");
var DateTimeToTimePipe_1 = require("./Pipes/DateTimeToTimePipe");
var AttatchmentIconPipe_1 = require("./Pipes/AttatchmentIconPipe");
var GroupByPipe_1 = require("./Pipes/GroupByPipe");
var MenuButtonsItemsPipe_1 = require("./Pipes/MenuButtonsItemsPipe");
var StageAgePipe_1 = require("./Pipes/StageAgePipe");
var DateTimeToBackgroundPipe_1 = require("./Pipes/DateTimeToBackgroundPipe");
var ExchangeRateDatePipe_1 = require("./Pipes/ExchangeRateDatePipe");
var LogBoxStatusForegroundPipe_1 = require("./Pipes/LogBoxStatusForegroundPipe");
var InvoiceDueDateForegroundPipe_1 = require("./Pipes/InvoiceDueDateForegroundPipe");
var CustomNumbersPipe_1 = require("./Pipes/CustomNumbersPipe");
var RatesNumbersPipe_1 = require("./Pipes/RatesNumbersPipe");
var DateTimeToMSDYDatePipe_1 = require("./Pipes/DateTimeToMSDYDatePipe");
var FollowUpDatePipe_1 = require("./Pipes/FollowUpDatePipe");
var DateTimeToShortDatePipe_1 = require("./Pipes/DateTimeToShortDatePipe");
var SafePipe_1 = require("./Pipes/SafePipe");
var LogBoxStatusDatePipe_1 = require("./Pipes/LogBoxStatusDatePipe");
var TimeToHoursMinutesPipe_1 = require("./Pipes/TimeToHoursMinutesPipe");
var CustomFieldResolverPipe_1 = require("./Pipes/CustomFieldResolverPipe");
// Controls Components
var GeneralSendComponent_1 = require("./Components/LogitudeComponents/GeneralSendComponent");
var LogLabelComponent_1 = require("./Components/LogitudeComponents/LogLabelComponent");
var LogToolTipComponent_1 = require("./Components/LogitudeComponents/LogToolTip/LogToolTipComponent");
var LogTextBoxComponent_1 = require("./Components/LogitudeComponents/LogTextBoxComponent");
var MultilineTextBoxWindow_1 = require("./Components/LogitudeComponents/MultilineTextBoxWindow");
var LogTextBoxV2Component_1 = require("./Components/LogitudeComponents/LogTextBoxV2Component");
var LogLovComponent_1 = require("./Components/LogitudeComponents/LogLovComponent");
var LogLovV2Component_1 = require("./Components/LogitudeComponents/LogLovV2Component");
var DWLovComponent_1 = require("./Components/LogitudeComponents/DWLovComponent");
var DWDateComponent_1 = require("./Components/LogitudeComponents/DWDateComponent");
var LogDatePickerComponent_1 = require("./Components/LogitudeComponents/LogDatePickerComponent");
var LogCalendarComponent_1 = require("./Components/LogitudeComponents/LogCalendarComponent");
var TimeSelectComponent_1 = require("./Components/LogitudeComponents/TimeSelectComponent");
var LogCheckboxComponent_1 = require("./Components/LogitudeComponents/LogCheckboxComponent");
var CustomDatePickerComponent_1 = require("./Components/CustomControls/CustomDatePickerComponent");
var ObjectFieldTemplate_1 = require("./Components/Templates/ObjectFieldTemplate");
var StimulsoftViewerComponent_1 = require("./Components/StimulsoftComponent/StimulsoftViewerComponent");
var ListComponent_1 = require("./Components/ListComponent/ListComponent");
var LogGridComponent_1 = require("./Components/LogitudeComponents/LogGridComponent/LogGridComponent");
var ListHeaderTemplateComponent_1 = require("./Components/LogitudeComponents/LogGridComponent/ListHeaderTemplateComponent");
var ListTemplateComponent_1 = require("./Components/LogitudeComponents/LogGridComponent/ListTemplateComponent");
var UsersQueryList_1 = require("./Components/CustomControls/UsersQueryList");
var ChooseDatesComponent_1 = require("./Components/CustomControls/ChooseDatesComponent");
var SearchBox_1 = require("./Components/CustomControls/SearchBox");
var RatioBoxComponent_1 = require("./Components/CustomControls/RatioBoxComponent");
var LogCellTemplateComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/LogCellTemplateComponent");
var LogColumnComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/LogColumnComponent");
var EditableLogGridComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/EditableLogGridComponent");
var InnerComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/InnerComponent");
var InnerSpanComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/InnerSpanComponent");
var LogRowDetailsTemplateComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/LogRowDetailsTemplateComponent");
var LogRowDetailsComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/LogRowDetailsComponent");
var EditableListTemplateComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/EditableListTemplateComponent");
var LogHeaderTemplateComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/LogHeaderTemplateComponent");
var LogFooterTemplateComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/LogFooterTemplateComponent");
var LogFooterComponent_1 = require("./Components/LogitudeComponents/EditableLogGridComponent/LogFooterComponent");
var ImageComponent_1 = require("./Components/LogitudeCustomComponents/ImageComponent");
var LogBooleanComponent_1 = require("./Components/LogitudeComponents/LogBooleanComponent");
var PickListComponent_1 = require("./Components/LogitudeComponents/PickListComponent");
var TextDesignComponent_1 = require("./Components/LogitudeCustomComponents/TextDesignComponent");
var SocialComponent_1 = require("./Components/LogitudeCustomComponents/SocialComponent");
var TimeInput_1 = require("./Components/LogitudeComponents/TimeInput");
var EntityFollowComponent_1 = require("./Components/LogitudeCustomComponents/EntityFollowComponent");
var AutomationsConditionAreaComponent_1 = require("./Components/Maintenance/Automation/AutomationsConditionAreaComponent");
// Infrastructure Components
var RootComponent_1 = require("./RootComponent");
var RootComponent_Cust_1 = require("./RootComponent_Cust");
var LoginComponent_1 = require("./Components/LoginComponent/LoginComponent");
var DSVLoginProcessComponent_1 = require("./Components/LoginComponent/CustomLoginComponents/DSVLoginProcessComponent");
var DSVMobileLoginProcessComponent_1 = require("./Components/LoginComponent/CustomLoginComponents/DSVMobileLoginProcessComponent");
var BlockScreenComponent_1 = require("./Components/LoginComponent/BlockScreenComponent");
var HomeComponent_1 = require("./Components/HomeComponent/HomeComponent");
var SessionComponent_1 = require("./Components/Session/SessionComponent");
var MainMenuComponent_1 = require("./Components/MainMenuComponent/MainMenuComponent");
var EditComponent_1 = require("./Components/EditComponent/EditComponent");
var EditTabComponent_1 = require("./Components/EditComponent/EditTabComponent");
var MaintenanceComponent_1 = require("./Components/Maintenance/MaintenanceComponent");
var MenuButtonsComponent_1 = require("./Components/LogitudeComponents/MenuButtonsComponent/MenuButtonsComponent");
var AdvanceSearchComponent_1 = require("./Components/AdvanceSearchComponent/AdvanceSearchComponent");
var QueryListComponent_1 = require("./Components/LogitudeComponents/QueryListComponent/QueryListComponent");
var LogitudeListBoxComponent_1 = require("./Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent");
var GeneralTabComponent_1 = require("./GenericComponents/GeneralTabComponent");
var GeneratedComponent_1 = require("./GenericComponents/GeneratedComponent");
var LastSuccessfulLoginComponent_1 = require("./Components/LogitudeCustomComponents/LastSuccessfulLoginComponent");
var TipsComponent_1 = require("./Components/LogitudeComponents/TipsComponent/TipsComponent");
var MainMenuAutomationComponent_1 = require("./Components/Maintenance/Automation/MainMenuAutomationComponent");
var AutomationsSettingsComponent_1 = require("./Components/Maintenance/Automation/AutomationsSettingsComponent");
var AddEditAutomationsComponent_1 = require("./Components/Maintenance/Automation/AddEditAutomationsComponent");
var DelayAutomationconditionsComponent_1 = require("./Components/Maintenance/Automation/DelayAutomationconditionsComponent");
var ViewAutomationHistoryComponent_1 = require("./Components/Maintenance/Automation/ViewAutomationHistoryComponent");
var AuditAutomationTabComponent_1 = require("./Components/Maintenance/Automation/AuditAutomationTabComponent");
var SelectDocumentTypesComponent_1 = require("./Components/Maintenance/Automation/SelectDocumentTypesComponent");
var NewViewComponent_1 = require("./Components/NewViewComponent/NewViewComponent");
var Export2ExcelControl_1 = require("./Components/Export2ExcelControl/Export2ExcelControl");
var ExportBI2ExcelControl_1 = require("./Components/ExportBI2ExcelControl/ExportBI2ExcelControl");
var QueryColumnsEditComponent_1 = require("./Components/QueryColumnsComponents/QueryColumnsEditComponent");
var btnComponent_1 = require("./Components/QueryColumnsComponents/btnComponent");
var btnUpdateComponent_1 = require("./Components/QueryColumnsComponents/btnUpdateComponent");
var ToComponent_1 = require("./Components/QueryColumnsComponents/ToComponent");
var NewEntityComponent_1 = require("./GenericComponents/NewEntityComponent");
var LogSearchWindowComponent_1 = require("./Components/LogitudeComponents/LogSearchWindowComponent");
var DWLogSearchWindowComponent_1 = require("./Components/LogitudeComponents/DWLogSearchWindowComponent");
var InfrastructureFieldTemplateComponent_1 = require("./Components/Templates/InfrastructureFieldTemplateComponent");
var LogSearchWindowButtonsComponent_1 = require("./Components/QueryColumnsComponents/LogSearchWindowButtonsComponent");
var LogTabsComponent_1 = require("./Components/LogitudeComponents/LogTabsComponent");
var MenuButtonsComponentLoader_1 = require("./Components/LogitudeComponents/MenuButtonsComponent/MenuButtonsComponentLoader");
var LogWaterMarkComponent_1 = require("./Components/Templates/LogWaterMarkComponent");
var DWLogSearchWindowFieldsComponent_1 = require("./Components/QueryColumnsComponents/DWLogSearchWindowFieldsComponent");
var ChooseUserComponent_1 = require("./Components/NewViewComponent/ChooseUserComponent");
var DWLogSearchAddFieldsComponent_1 = require("./Components/QueryColumnsComponents/DWLogSearchAddFieldsComponent");
// Followups
var HelperFollowups_1 = require("./Components/LogitudeComponents/Followups/HelperFollowups");
var MainMenuFollowups_1 = require("./Components/LogitudeComponents/Followups/MainMenuFollowups");
var FollowupButton_1 = require("./Components/LogitudeComponents/Followups/FollowupButton");
var AddFollowupComponent_1 = require("./Components/LogitudeComponents/Followups/AddFollowupComponent");
var AddDocumentFollowupComponent_1 = require("./Components/LogitudeComponents/Followups/AddDocumentFollowupComponent");
var DropBoxLogin_1 = require("./Components/HomeComponent/DropBoxLogin");
var StimulsoftDesigner_1 = require("./Components/StimulsoftDesigner/StimulsoftDesigner");
var ExportSettingAdvanceComponent_1 = require("./Components/StimulsoftComponent/ExportSettingAdvanceComponent");
var ObjectFieldComponent_1 = require("./Components/LogitudeComponents/ObjectFieldComponent");
var WizardBaseComponent_1 = require("./Components/Maintenance/Wizard/WizardBaseComponent");
var WizardAddressCompnent_1 = require("./Components/Maintenance/Wizard/WizardAddressCompnent");
var WizardAccountingComponent_1 = require("./Components/Maintenance/Wizard/WizardAccountingComponent");
var StimulsoftDesignerComponent_1 = require("./Components/StimulsoftComponent/StimulsoftDesignerComponent");
var EraseTenantManagementDataComponent_1 = require("./Components/MenuButtons/EraseTenantManagementDataComponent");
var FroalaEditorComponent_1 = require("./Components/FroalaEditorComponent/FroalaEditorComponent");
var DocsOutTabComponent_1 = require("./Components/Documents/DocsOutTabComponent");
var DocsInTabComponent_1 = require("./Components/Documents/DocsInTabComponent");
var BTEGeneralTabComponent_1 = require("./Components/Maintenance/BatchTaskExecution/BTEGeneralTabComponent");
var BTELogTabComponent_1 = require("./Components/Maintenance/BatchTaskExecution/BTELogTabComponent");
var BTEParameterTabComponent_1 = require("./Components/Maintenance/BatchTaskExecution/BTEParameterTabComponent/BTEParameterTabComponent");
exports.Directives = [
    FocusMeDirective_1.FocusMeDirective,
    LocationDirective_1.LocationDirective,
    FixedPositionDirective_1.FixedPositionDirective,
    FloatStartDirective_1.FloatStartDirective,
    FloatEndDirective_1.FloatEndDirective,
    TextAlignStart_1.TextAlignStart,
    TextAlignEnd_1.TextAlignEnd,
];
exports.Pipes = [
    NumbersPipe_1.NumbersPipe,
    PaddingPipe_1.PaddingPipe,
    ReplacePipe_1.ReplacePipe,
    StringToColorPipe_1.StringToColorPipe,
    DateTimeToTimePipe_1.DateTimeToTimePipe,
    AttatchmentIconPipe_1.AttatchmentIconPipe,
    GroupByPipe_1.GroupByPipe,
    MenuButtonsItemsPipe_1.MenuButtonsItemsPipe,
    StageAgePipe_1.StageAgePipe,
    DateTimeToColorPipe_1.DateTimeToColorPipe,
    DateTimeToBackgroundPipe_1.DateTimeToBackgroundPipe,
    ExchangeRateDatePipe_1.ExchangeRateDatePipe,
    LogBoxStatusForegroundPipe_1.LogBoxStatusForegroundPipe,
    InvoiceDueDateForegroundPipe_1.InvoiceDueDateForegroundPipe,
    RatesNumbersPipe_1.RatesNumbersPipe,
    DateTimeToMSDYDatePipe_1.DateTimeToMSDYDatePipe,
    CustomNumbersPipe_1.CustomNumbersPipe,
    FollowUpDatePipe_1.FollowUpDatePipe,
    DateTimeToShortDatePipe_1.DateTimeToShortDatePipe,
    SafePipe_1.SafePipe,
    LogBoxStatusDatePipe_1.LogBoxStatusDatePipe,
    TimeToHoursMinutesPipe_1.TimeToHoursMinutesPipe,
    CustomFieldResolverPipe_1.CustomFieldResolverPipe,
];
exports.ControlsComponents = [
    GeneralSendComponent_1.GeneralSendComponent,
    LogLabelComponent_1.LogLabelComponent,
    LogToolTipComponent_1.LogToolTipComponent,
    LogTextBoxComponent_1.LogTextBoxComponent,
    MultilineTextBoxWindow_1.MultilineTextBoxWindow,
    LogTextBoxV2Component_1.LogTextBoxV2Component,
    LogLovComponent_1.LogLovComponent,
    LogLovV2Component_1.LogLovV2Component,
    DWLovComponent_1.DWLovComponent,
    DWDateComponent_1.DWDateComponent,
    LogDatePickerComponent_1.LogDatePickerComponent,
    LogCalendarComponent_1.LogCalendarComponent,
    TimeSelectComponent_1.TimeSelectComponent,
    LogCheckboxComponent_1.LogCheckboxComponent,
    CustomDatePickerComponent_1.CustomDatePickerComponent,
    ObjectFieldTemplate_1.ObjectFieldTemplate,
    StimulsoftViewerComponent_1.StimulsoftViewerComponent,
    ListComponent_1.ListComponent,
    LogGridComponent_1.LogGridComponent,
    ListHeaderTemplateComponent_1.ListHeaderTemplateComponent,
    ListTemplateComponent_1.ListTemplateComponent,
    UsersQueryList_1.UsersQueryList,
    ChooseDatesComponent_1.ChooseDatesComponent,
    SearchBox_1.SearchBox,
    RatioBoxComponent_1.RatioBoxComponent,
    LogCellTemplateComponent_1.LogCellTemplateComponent,
    LogColumnComponent_1.LogColumnComponent,
    EditableLogGridComponent_1.EditableLogGridComponent,
    InnerComponent_1.InnerComponent,
    InnerSpanComponent_1.InnerSpanComponent,
    LogRowDetailsTemplateComponent_1.LogRowDetailsTemplateComponent,
    LogRowDetailsComponent_1.LogRowDetailsComponent,
    EditableListTemplateComponent_1.EditableListTemplateComponent,
    LogHeaderTemplateComponent_1.LogHeaderTemplateComponent,
    LogFooterTemplateComponent_1.LogFooterTemplateComponent,
    LogFooterComponent_1.LogFooterComponent,
    TipsComponent_1.TipsComponent,
    CacheLogComponent_1.CacheLogComponent,
    ImageComponent_1.ImageComponent,
    LogTabsComponent_1.LogTabsComponent,
    LogBooleanComponent_1.LogBooleanComponent,
    LogWaterMarkComponent_1.LogWaterMarkComponent,
    TextDesignComponent_1.TextDesignComponent,
    PickListComponent_1.PickListComponent,
    HelperFollowups_1.HelperFollowups,
    MainMenuFollowups_1.MainMenuFollowups,
    FollowupButton_1.FollowupButton,
    ObjectFieldComponent_1.ObjectFieldComponent,
    AuditAutomationTabComponent_1.AuditAutomationTabComponent,
    SocialComponent_1.SocialComponent,
    TimeInput_1.TimeInput,
    EntityFollowComponent_1.EntityFollowComponent,
    StimulsoftDesignerComponent_1.StimulsoftDesignerComponent,
    AutomationsConditionAreaComponent_1.AutomationsConditionAreaComponent,
    LogitudeListBoxComponent_1.LogitudeListBoxComponent,
    FroalaEditorComponent_1.FroalaEditorComponent,
    DocsOutTabComponent_1.DocsOutTabComponent,
    DocsInTabComponent_1.DocsInTabComponent,
    LastSuccessfulLoginComponent_1.LastSuccessfulLoginComponent,
];
exports.Components = [
    RootComponent_1.RootComponent,
    RootComponent_Cust_1.RootComponent_Cust,
    LoginComponent_1.LoginComponent,
    DSVLoginProcessComponent_1.DSVLoginProcessComponent,
    DSVMobileLoginProcessComponent_1.DSVMobileLoginProcessComponent,
    BlockScreenComponent_1.BlockScreenComponent,
    HomeComponent_1.HomeComponent,
    SessionComponent_1.SessionComponent,
    MainMenuComponent_1.MainMenuComponent,
    EditComponent_1.EditComponent,
    EditTabComponent_1.EditTabComponent,
    MaintenanceComponent_1.MaintenanceComponent,
    MenuButtonsComponent_1.MenuButtonsComponent,
    MenuButtonsComponentLoader_1.MenuButtonsComponentLoader,
    AdvanceSearchComponent_1.AdvanceSearchComponent,
    QueryListComponent_1.QueryListComponent,
    LogitudeListBoxComponent_1.LogitudeListBoxComponent,
    GeneralTabComponent_1.GeneralTabComponent,
    GeneratedComponent_1.GeneratedComponent,
    MultilineTextBoxWindow_1.MultilineTextBoxWindow,
    MainMenuAutomationComponent_1.MainMenuAutomationComponent,
    AutomationsSettingsComponent_1.AutomationsSettingsComponent,
    AddEditAutomationsComponent_1.AddEditAutomationsComponent,
    DelayAutomationconditionsComponent_1.DelayAutomationconditionsComponent,
    ViewAutomationHistoryComponent_1.ViewAutomationHistoryComponent,
    NewViewComponent_1.NewViewComponent,
    Export2ExcelControl_1.Export2ExcelControl,
    ExportBI2ExcelControl_1.ExportBI2ExcelControl,
    QueryColumnsEditComponent_1.QueryColumnsEditComponent,
    btnComponent_1.btnComponent,
    btnUpdateComponent_1.btnUpdateComponent,
    ToComponent_1.ToComponent,
    NewEntityComponent_1.NewEntityComponent,
    LogSearchWindowComponent_1.LogSearchWindowComponent,
    DWLogSearchWindowComponent_1.DWLogSearchWindowComponent,
    ChooseUserComponent_1.ChooseUserComponent,
    DWLogSearchAddFieldsComponent_1.DWLogSearchAddFieldsComponent,
    InfrastructureFieldTemplateComponent_1.InfrastructureFieldTemplateComponent,
    LogSearchWindowButtonsComponent_1.LogSearchWindowButtonsComponent,
    DWLogSearchWindowFieldsComponent_1.DWLogSearchWindowFieldsComponent,
    AddFollowupComponent_1.AddFollowupComponent,
    AddDocumentFollowupComponent_1.AddDocumentFollowupComponent,
    DropBoxLogin_1.DropBoxLogin,
    StimulsoftDesigner_1.StimulsoftDesigner,
    ExportSettingAdvanceComponent_1.ExportSettingAdvanceComponent,
    AuditAutomationTabComponent_1.AuditAutomationTabComponent,
    WizardBaseComponent_1.WizardBaseComponent,
    WizardAddressCompnent_1.WizardAddressCompnent,
    WizardAccountingComponent_1.WizardAccountingComponent,
    SelectDocumentTypesComponent_1.SelectDocumentTypesComponent,
    AutomationsConditionAreaComponent_1.AutomationsConditionAreaComponent,
    EraseTenantManagementDataComponent_1.EraseTenantManagementDataComponent,
    FroalaEditorComponent_1.FroalaEditorComponent,
    DocsOutTabComponent_1.DocsOutTabComponent,
    DocsInTabComponent_1.DocsInTabComponent,
    BTEGeneralTabComponent_1.BTEGeneralTabComponent,
    BTELogTabComponent_1.BTELogTabComponent,
    BTEParameterTabComponent_1.BTEParameterTabComponent,
    LastSuccessfulLoginComponent_1.LastSuccessfulLoginComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case 'RootComponent': {
                myResult = RootComponent_1.RootComponent;
                break;
            }
            case 'RootComponent_Cust': {
                myResult = RootComponent_Cust_1.RootComponent_Cust;
                break;
            }
            case 'LoginComponent': {
                myResult = LoginComponent_1.LoginComponent;
                break;
            }
            case 'DSVLoginProcessComponent': {
                myResult = DSVLoginProcessComponent_1.DSVLoginProcessComponent;
                break;
            }
            case 'DSVMobileLoginProcessComponent': {
                myResult = DSVMobileLoginProcessComponent_1.DSVMobileLoginProcessComponent;
                break;
            }
            case 'BlockScreenComponent': {
                myResult = BlockScreenComponent_1.BlockScreenComponent;
                break;
            }
            case 'HomeComponent': {
                myResult = HomeComponent_1.HomeComponent;
                break;
            }
            case 'SessionComponent': {
                myResult = SessionComponent_1.SessionComponent;
                break;
            }
            case 'MainMenuComponent': {
                myResult = MainMenuComponent_1.MainMenuComponent;
                break;
            }
            case 'EditComponent': {
                myResult = EditComponent_1.EditComponent;
                break;
            }
            case 'EditTabComponent': {
                myResult = EditTabComponent_1.EditTabComponent;
                break;
            }
            case 'ListComponent': {
                myResult = ListComponent_1.ListComponent;
                break;
            }
            case 'MaintenanceComponent': {
                myResult = MaintenanceComponent_1.MaintenanceComponent;
                break;
            }
            case 'MenuButtonsComponent': {
                myResult = MenuButtonsComponent_1.MenuButtonsComponent;
                break;
            }
            case 'MenuButtonsComponentLoader': {
                myResult = MenuButtonsComponentLoader_1.MenuButtonsComponentLoader;
                break;
            }
            case 'AdvanceSearchComponent': {
                myResult = AdvanceSearchComponent_1.AdvanceSearchComponent;
                break;
            }
            case 'QueryListComponent': {
                myResult = QueryListComponent_1.QueryListComponent;
                break;
            }
            case 'LogitudeListBoxComponent': {
                myResult = LogitudeListBoxComponent_1.LogitudeListBoxComponent;
                break;
            }
            case 'GeneralTabComponent': {
                myResult = GeneralTabComponent_1.GeneralTabComponent;
                break;
            }
            case 'GeneratedComponent': {
                myResult = GeneratedComponent_1.GeneratedComponent;
                break;
            }
            case 'ImageComponent': {
                myResult = ImageComponent_1.ImageComponent;
                break;
            }
            case 'TextDesignComponent': {
                myResult = TextDesignComponent_1.TextDesignComponent;
                break;
            }
            case 'MainMenuAutomationComponent': {
                myResult = MainMenuAutomationComponent_1.MainMenuAutomationComponent;
                break;
            }
            case 'AutomationsSettingsComponent': {
                myResult = AutomationsSettingsComponent_1.AutomationsSettingsComponent;
                break;
            }
            case 'AddEditAutomationsComponent': {
                myResult = AddEditAutomationsComponent_1.AddEditAutomationsComponent;
                break;
            }
            case 'DelayAutomationconditionsComponent': {
                myResult = DelayAutomationconditionsComponent_1.DelayAutomationconditionsComponent;
                break;
            }
            case 'ViewAutomationHistoryComponent': {
                myResult = ViewAutomationHistoryComponent_1.ViewAutomationHistoryComponent;
                break;
            }
            case 'NewViewComponent': {
                myResult = NewViewComponent_1.NewViewComponent;
                break;
            }
            case 'Export2ExcelControl': {
                myResult = Export2ExcelControl_1.Export2ExcelControl;
                break;
            }
            case 'ExportBI2ExcelControl': {
                myResult = ExportBI2ExcelControl_1.ExportBI2ExcelControl;
                break;
            }
            case 'QueryColumnsEditComponent': {
                myResult = QueryColumnsEditComponent_1.QueryColumnsEditComponent;
                break;
            }
            case 'btnComponent': {
                myResult = btnComponent_1.btnComponent;
                break;
            }
            case 'btnUpdateComponent': {
                myResult = btnUpdateComponent_1.btnUpdateComponent;
                break;
            }
            case 'ToComponent': {
                myResult = ToComponent_1.ToComponent;
                break;
            }
            case 'DWLogSearchAddFieldsComponent': {
                myResult = DWLogSearchAddFieldsComponent_1.DWLogSearchAddFieldsComponent;
                break;
            }
            case 'NewEntityComponent': {
                myResult = NewEntityComponent_1.NewEntityComponent;
                break;
            }
            case 'LogSearchWindowComponent': {
                myResult = LogSearchWindowComponent_1.LogSearchWindowComponent;
                break;
            }
            case 'DWLogSearchWindowComponent': {
                myResult = DWLogSearchWindowComponent_1.DWLogSearchWindowComponent;
                break;
            }
            case 'StimulsoftViewerComponent': {
                myResult = StimulsoftViewerComponent_1.StimulsoftViewerComponent;
                break;
            }
            case 'TipsComponent': {
                myResult = TipsComponent_1.TipsComponent;
                break;
            }
            case 'CacheLogComponent': {
                myResult = CacheLogComponent_1.CacheLogComponent;
                break;
            }
            case 'ChooseUserComponent': {
                myResult = ChooseUserComponent_1.ChooseUserComponent;
                break;
            }
            case 'ChooseDatesComponent': {
                myResult = ChooseDatesComponent_1.ChooseDatesComponent;
                break;
            }
            case 'InfrastructureFieldTemplateComponent': {
                myResult = InfrastructureFieldTemplateComponent_1.InfrastructureFieldTemplateComponent;
                break;
            }
            case 'GeneralSendComponent': {
                myResult = GeneralSendComponent_1.GeneralSendComponent;
                break;
            }
            case 'LogSearchWindowButtonsComponent': {
                myResult = LogSearchWindowButtonsComponent_1.LogSearchWindowButtonsComponent;
                break;
            }
            case 'DWLogSearchWindowFieldsComponent': {
                myResult = DWLogSearchWindowFieldsComponent_1.DWLogSearchWindowFieldsComponent;
                break;
            }
            case 'AddFollowupComponent': {
                myResult = AddFollowupComponent_1.AddFollowupComponent;
                break;
            }
            case 'AddDocumentFollowupComponent': {
                myResult = AddDocumentFollowupComponent_1.AddDocumentFollowupComponent;
                break;
            }
            case 'DropBoxLogin': {
                myResult = DropBoxLogin_1.DropBoxLogin;
                break;
            }
            case 'StimulsoftDesigner': {
                myResult = StimulsoftDesigner_1.StimulsoftDesigner;
                break;
            }
            case 'ObjectFieldComponent': {
                myResult = ObjectFieldComponent_1.ObjectFieldComponent;
                break;
            }
            case 'ExportSettingAdvanceComponent': {
                myResult = ExportSettingAdvanceComponent_1.ExportSettingAdvanceComponent;
                break;
            }
            case 'AuditAutomationTabComponent': {
                myResult = AuditAutomationTabComponent_1.AuditAutomationTabComponent;
                break;
            }
            case 'SocialComponent': {
                myResult = SocialComponent_1.SocialComponent;
                break;
            }
            case 'LogTabsComponent': {
                myResult = LogTabsComponent_1.LogTabsComponent;
                break;
            }
            case 'LogWaterMarkComponent': {
                myResult = LogWaterMarkComponent_1.LogWaterMarkComponent;
                break;
            }
            case 'EntityFollowComponent': {
                myResult = EntityFollowComponent_1.EntityFollowComponent;
                break;
            }
            case 'WizardBaseComponent': {
                myResult = WizardBaseComponent_1.WizardBaseComponent;
                break;
            }
            case 'WizardAddressCompnent': {
                myResult = WizardAddressCompnent_1.WizardAddressCompnent;
                break;
            }
            case 'WizardAccountingComponent': {
                myResult = WizardAccountingComponent_1.WizardAccountingComponent;
                break;
            }
            case 'SelectDocumentTypesComponent': {
                myResult = SelectDocumentTypesComponent_1.SelectDocumentTypesComponent;
                break;
            }
            case 'StimulsoftDesignerComponent': {
                myResult = StimulsoftDesignerComponent_1.StimulsoftDesignerComponent;
                break;
            }
            case 'AutomationsConditionAreaComponent': {
                myResult = AutomationsConditionAreaComponent_1.AutomationsConditionAreaComponent;
                break;
            }
            case 'EraseTenantManagementDataComponent': {
                myResult = EraseTenantManagementDataComponent_1.EraseTenantManagementDataComponent;
                break;
            }
            case 'FroalaEditorComponent': {
                myResult = FroalaEditorComponent_1.FroalaEditorComponent;
                break;
            }
            case 'DocsInTabComponent': {
                myResult = DocsInTabComponent_1.DocsInTabComponent;
                break;
            }
            case 'DocsOutTabComponent': {
                myResult = DocsOutTabComponent_1.DocsOutTabComponent;
                break;
            }
            case 'BTEGeneralTabComponent': {
                myResult = BTEGeneralTabComponent_1.BTEGeneralTabComponent;
                break;
            }
            case 'BTELogTabComponent': {
                myResult = BTELogTabComponent_1.BTELogTabComponent;
                break;
            }
            case 'BTEParameterTabComponent': {
                myResult = BTEParameterTabComponent_1.BTEParameterTabComponent;
                break;
            }
            case 'LastSuccessfulLoginComponent': {
                myResult = LastSuccessfulLoginComponent_1.LastSuccessfulLoginComponent;
                break;
            }
            case 'MultilineTextBoxWindow': {
                myResult = MultilineTextBoxWindow_1.MultilineTextBoxWindow;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map