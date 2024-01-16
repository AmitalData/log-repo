import { CacheLogComponent } from './Components/Maintenance/CacheLogComponent';
// Directives
import {FocusMeDirective} from './Utilities/FocusMeDirective';
import {LocationDirective} from './Utilities/LocationDirective';
import {FixedPositionDirective} from './Utilities/FixedPositionDirective';
import { ChildDirective } from './Directives/ChildDirective';

//rtl directives
import {FloatStartDirective} from './Utilities/RTLDirectives/FloatStartDirective';
import { FloatEndDirective } from './Utilities/RTLDirectives/FloatEndDirective';
import { TextAlignStart } from './Utilities/RTLDirectives/TextAlignStart';
import { TextAlignEnd } from './Utilities/RTLDirectives/TextAlignEnd';

// Pipes.
import {DateTimeToColorPipe} from './Pipes/DateTimePipes/DateTimeToColorPipe';
import {NumbersPipe} from './Pipes/NumbersPipe';
import {PaddingPipe} from './Pipes/PaddingPipe';
import { ReplacePipe } from './Pipes/ReplacePipe';
import { HighlightSearch } from './Pipes/HighlightSearch'; 
import {StringToColorPipe} from './Pipes/StringToColorPipe';
import {DateTimeToTimePipe} from './Pipes/DateTimeToTimePipe';
import {AttatchmentIconPipe} from './Pipes/AttatchmentIconPipe';
import {GroupByPipe} from './Pipes/GroupByPipe';
import {MenuButtonsItemsPipe} from './Pipes/MenuButtonsItemsPipe';
import {StageAgePipe} from './Pipes/StageAgePipe';
import {DateTimeToBackgroundPipe} from './Pipes/DateTimeToBackgroundPipe';
import {ExchangeRateDatePipe} from './Pipes/ExchangeRateDatePipe';
import {LogBoxStatusForegroundPipe} from './Pipes/LogBoxStatusForegroundPipe';
import {InvoiceDueDateForegroundPipe} from './Pipes/InvoiceDueDateForegroundPipe';
import {CustomNumbersPipe} from './Pipes/CustomNumbersPipe';
import {RatesNumbersPipe} from './Pipes/RatesNumbersPipe';
import {DateTimeToMSDYDatePipe} from './Pipes/DateTimeToMSDYDatePipe';
import {FollowUpDatePipe} from './Pipes/FollowUpDatePipe';
import {DateTimeToShortDatePipe} from './Pipes/DateTimeToShortDatePipe';
import {SafePipe} from './Pipes/SafePipe';
import {LogBoxStatusDatePipe} from './Pipes/LogBoxStatusDatePipe';
import {TimeToHoursMinutesPipe} from './Pipes/TimeToHoursMinutesPipe';
import {CustomFieldResolverPipe} from './Pipes/CustomFieldResolverPipe';

// Controls Components
import { GeneralSendComponent } from './Components/LogitudeComponents/GeneralSendComponent';
import { ImageLibraryComponent } from './Components/LogitudeComponents/ImageLibraryComponent';
import {LogLabelComponent} from './Components/LogitudeComponents/LogLabelComponent';
import {LogToolTipComponent} from './Components/LogitudeComponents/LogToolTip/LogToolTipComponent';
import {LogTextBoxComponent} from './Components/LogitudeComponents/LogTextBoxComponent';
import {MultilineTextBoxWindow} from './Components/LogitudeComponents/MultilineTextBoxWindow';
import {LogTextBoxV2Component} from './Components/LogitudeComponents/LogTextBoxV2Component';
import {LogLovComponent} from './Components/LogitudeComponents/LogLovComponent';
import {LogLovV2Component} from './Components/LogitudeComponents/LogLovV2Component';
import {LogTextBoxComponentV3} from './Components/LogitudeComponents/LogTextBoxComponentV3';
import {DWLovComponent} from './Components/LogitudeComponents/DWLovComponent';
import {DWDateComponent} from './Components/LogitudeComponents/DWDateComponent'

import {LogDatePickerComponent} from './Components/LogitudeComponents/LogDatePickerComponent';
import { AdvancedDatePickerComponent } from './Components/LogitudeComponents/AdvancedDatePickerComponent';
import {LogCalendarComponent} from './Components/LogitudeComponents/LogCalendarComponent';
import {TimeSelectComponent} from './Components/LogitudeComponents/TimeSelectComponent';
import {LogCheckboxComponent} from './Components/LogitudeComponents/LogCheckboxComponent';
import {CustomDatePickerComponent} from './Components/CustomControls/CustomDatePickerComponent';
import { ObjectFieldTemplate } from './Components/Templates/ObjectFieldTemplate';
import { MyTemplate } from './Components/Templates/MyTemplate';

import {StimulsoftViewerComponent} from './Components/StimulsoftComponent/StimulsoftViewerComponent';
import {ListComponent} from './Components/ListComponent/ListComponent';
import { LogGridComponent } from './Components/LogitudeComponents/LogGridComponent/LogGridComponent';
import { LogGridComponentV2 } from './Components/LogitudeComponents/LogGridComponent/LogGridComponentV2';

import {ListHeaderTemplateComponent} from './Components/LogitudeComponents/LogGridComponent/ListHeaderTemplateComponent';
import {ListTemplateComponent} from './Components/LogitudeComponents/LogGridComponent/ListTemplateComponent';
import {UsersQueryList} from './Components/CustomControls/UsersQueryList';
import {ChooseDatesComponent} from './Components/CustomControls/ChooseDatesComponent';
import { SearchBox } from './Components/CustomControls/SearchBox';
import { RatioBoxComponent } from './Components/CustomControls/RatioBoxComponent';

import {LogCellTemplateComponent} from './Components/LogitudeComponents/EditableLogGridComponent/LogCellTemplateComponent';
import {LogColumnComponent} from './Components/LogitudeComponents/EditableLogGridComponent/LogColumnComponent';
import {EditableLogGridComponent} from './Components/LogitudeComponents/EditableLogGridComponent/EditableLogGridComponent';
import {InnerComponent} from './Components/LogitudeComponents/EditableLogGridComponent/InnerComponent';
import {InnerSpanComponent} from './Components/LogitudeComponents/EditableLogGridComponent/InnerSpanComponent';
import {LogRowDetailsTemplateComponent} from './Components/LogitudeComponents/EditableLogGridComponent/LogRowDetailsTemplateComponent';
import {LogRowDetailsComponent} from './Components/LogitudeComponents/EditableLogGridComponent/LogRowDetailsComponent';
import {EditableListTemplateComponent} from './Components/LogitudeComponents/EditableLogGridComponent/EditableListTemplateComponent';
import {LogHeaderTemplateComponent} from './Components/LogitudeComponents/EditableLogGridComponent/LogHeaderTemplateComponent';
import {LogFooterTemplateComponent} from './Components/LogitudeComponents/EditableLogGridComponent/LogFooterTemplateComponent';
import {LogFooterComponent} from './Components/LogitudeComponents/EditableLogGridComponent/LogFooterComponent';
import {ImageComponent} from './Components/LogitudeCustomComponents/ImageComponent';
import {LogBooleanComponent} from './Components/LogitudeComponents/LogBooleanComponent';
import {PickListComponent} from './Components/LogitudeComponents/PickListComponent';
import {TextDesignComponent} from './Components/LogitudeCustomComponents/TextDesignComponent';
import {SocialComponent} from './Components/LogitudeCustomComponents/SocialComponent';
import {TimeInput} from './Components/LogitudeComponents/TimeInput';
import {EntityFollowComponent} from './Components/LogitudeCustomComponents/EntityFollowComponent';
import {AutomationsConditionAreaComponent} from './Components/Maintenance/Automation/AutomationsConditionAreaComponent';

// Infrastructure Components
import {RootComponent} from './RootComponent';
import {RootComponent_Cust} from './RootComponent_Cust';
import {LoginComponent} from './Components/LoginComponent/LoginComponent';
import {DSVLoginProcessComponent} from './Components/LoginComponent/PrivateLabelComponents/DSVLoginProcessComponent';
import {DSVMobileLoginProcessComponent} from './Components/LoginComponent/PrivateLabelComponents/DSVMobileLoginProcessComponent';
import {BlockScreenComponent} from './Components/LoginComponent/BlockScreenComponent';
import {HomeComponent} from './Components/HomeComponent/HomeComponent';
import {SessionComponent} from './Components/Session/SessionComponent';
import {MainMenuComponent} from './Components/MainMenuComponent/MainMenuComponent';
import {EditComponent} from './Components/EditComponent/EditComponent';
import {EditTabComponent} from './Components/EditComponent/EditTabComponent';
import {MaintenanceComponent} from './Components/Maintenance/MaintenanceComponent';
import {MenuButtonsComponent} from './Components/LogitudeComponents/MenuButtonsComponent/MenuButtonsComponent';
import {AdvanceSearchComponent} from './Components/AdvanceSearchComponent/AdvanceSearchComponent';
import {QueryListComponent} from './Components/LogitudeComponents/QueryListComponent/QueryListComponent';
import {LogitudeListBoxComponent} from './Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent';
import {GeneralTabComponent} from './GenericComponents/GeneralTabComponent';
import {GeneratedComponent} from './GenericComponents/GeneratedComponent';
import { LastSuccessfulLoginComponent } from './Components/LogitudeCustomComponents/LastSuccessfulLoginComponent';

import {TipsComponent} from './Components/LogitudeComponents/TipsComponent/TipsComponent';

import {MainMenuAutomationComponent} from './Components/Maintenance/Automation/MainMenuAutomationComponent';
import {AutomationsSettingsComponent} from './Components/Maintenance/Automation/AutomationsSettingsComponent';
import {AddEditAutomationsComponent} from './Components/Maintenance/Automation/AddEditAutomationsComponent';
import { ChooseSpecificUserComponent } from './Components/Maintenance/Automation/ChooseSpecificUserComponent';
import {DelayAutomationconditionsComponent} from './Components/Maintenance/Automation/DelayAutomationconditionsComponent';
import {ViewAutomationHistoryComponent} from './Components/Maintenance/Automation/ViewAutomationHistoryComponent';
import {AuditAutomationTabComponent} from './Components/Maintenance/Automation/AuditAutomationTabComponent';


import {SelectDocumentTypesComponent} from './Components/Maintenance/Automation/SelectDocumentTypesComponent';

import {NewViewComponent} from './Components/NewViewComponent/NewViewComponent';
import { Export2ExcelControl } from './Components/Export2ExcelControl/Export2ExcelControl';
import { ExportBI2ExcelControl } from './Components/ExportBI2ExcelControl/ExportBI2ExcelControl';
import {QueryColumnsEditComponent} from './Components/QueryColumnsComponents/QueryColumnsEditComponent';
import { btnComponent } from './Components/QueryColumnsComponents/btnComponent';
import { btnUpdateComponent } from './Components/QueryColumnsComponents/btnUpdateComponent';

import {ToComponent} from './Components/QueryColumnsComponents/ToComponent';
import {NewEntityComponent} from './GenericComponents/NewEntityComponent';
import {LogSearchWindowComponent} from './Components/LogitudeComponents/LogSearchWindowComponent';
import {DWLogSearchWindowComponent} from './Components/LogitudeComponents/DWLogSearchWindowComponent';

import {InfrastructureFieldTemplateComponent} from './Components/Templates/InfrastructureFieldTemplateComponent';
import {LogSearchWindowButtonsComponent} from './Components/QueryColumnsComponents/LogSearchWindowButtonsComponent';
import {LogTabsComponent} from './Components/LogitudeComponents/LogTabsComponent';
import { MenuButtonsComponentLoader } from './Components/LogitudeComponents/MenuButtonsComponent/MenuButtonsComponentLoader';
import { LogWaterMarkComponent } from './Components/Templates/LogWaterMarkComponent';
import {DWLogSearchWindowFieldsComponent} from './Components/QueryColumnsComponents/DWLogSearchWindowFieldsComponent';
import { ChooseUserComponent } from './Components/NewViewComponent/ChooseUserComponent';

import {DWLogSearchAddFieldsComponent} from './Components/QueryColumnsComponents/DWLogSearchAddFieldsComponent';




// Followups
import {HelperFollowups} from './Components/LogitudeComponents/Followups/HelperFollowups';
import {MainMenuFollowups} from './Components/LogitudeComponents/Followups/MainMenuFollowups';
import {FollowupButton} from './Components/LogitudeComponents/Followups/FollowupButton';
import {AddFollowupComponent} from './Components/LogitudeComponents/Followups/AddFollowupComponent';
import {AddDocumentFollowupComponent} from './Components/LogitudeComponents/Followups/AddDocumentFollowupComponent';

import {DropBoxLogin} from './Components/HomeComponent/DropBoxLogin';
import { StimulsoftDesigner } from './Components/StimulsoftDesigner/StimulsoftDesigner';
import {ExportSettingAdvanceComponent} from './Components/StimulsoftComponent/ExportSettingAdvanceComponent';



import {ObjectFieldComponent} from './Components/LogitudeComponents/ObjectFieldComponent';
import {WizardBaseComponent} from './Components/Maintenance/Wizard/WizardBaseComponent';
import {WizardAddressCompnent} from './Components/Maintenance/Wizard/WizardAddressCompnent';
import {WizardAccountingComponent} from './Components/Maintenance/Wizard/WizardAccountingComponent';
import {StimulsoftDesignerComponent} from './Components/StimulsoftComponent/StimulsoftDesignerComponent';
import {EraseTenantManagementDataComponent} from './Components/MenuButtons/EraseTenantManagementDataComponent';
import {FroalaEditorComponent} from './Components/FroalaEditorComponent/FroalaEditorComponent';
import {DocsOutTabComponent} from './Components/Documents/DocsOutTabComponent';
import {DocsInTabComponent} from './Components/Documents/DocsInTabComponent';
import { BTEGeneralTabComponent } from './Components/Maintenance/BatchTaskExecution/BTEGeneralTabComponent';
import { BTELogTabComponent } from './Components/Maintenance/BatchTaskExecution/BTELogTabComponent';
import { BTEParameterTabComponent } from './Components/Maintenance/BatchTaskExecution/BTEParameterTabComponent/BTEParameterTabComponent';
import { TaskSchedulerHistoryComponent } from '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/TaskSchedulerHistoryComponent';
import { DropdownMenuComponent } from './Components/LogitudeComponents/DropdownMenuComponent';
import { MultiSelectLOVComponent } from './Components/LogitudeComponents/MultiSelectLOVComponent';

import { FTBSchedulerTemplateComponent } from '../InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/SchedulerTemplates/FTBSchedulerTemplateComponent';
import { SendInterfaceResultComponent } from './Components/Maintenance/Automation/AutomationResult/SendInterfaceResultComponent';
import { FTPAutomationDetailsComponent } from './Components/Maintenance/Automation/AutomationResult/FTPAutomationDetailsComponent';
import { ToolTipFloatDirective } from './Utilities/RTLDirectives/ToolTipFloatDirective';
import { SendDocumentResultComponent } from './Components/Maintenance/Automation/AutomationResult/SendDocumentResultComponent';

import { PrivateLabelLoginProcessComponent } from './Components/LoginComponent/PrivateLabelComponents/PrivateLabelLoginProcessComponent';
import { AutomationsConditionsViewDetailsComponent } from './Components/Maintenance/Automation/AutomationsConditionsViewDetailsComponent';
import { AutomationConditionsDetailsComponent } from './Components/Maintenance/Automation/AutomationConditionsDetailsComponent';
import { CreateTaskResultComponent } from './Components/Maintenance/Automation/AutomationResult/CreateTaskResultComponent';
import { IdentityShaamLandingPageComponent } from './Components/IdentityShaamLandingPage/IdentityShaamLandingPage';
 
 




//import { ScrollingModule } from '@angular/cdk/scrolling';
export const Directives =
    [
        FocusMeDirective,
        LocationDirective,
        FixedPositionDirective,
        ChildDirective,
        FloatStartDirective,
        ToolTipFloatDirective,
        FloatEndDirective,
        TextAlignStart,
        TextAlignEnd,
    ];
export const Pipes =
    [
        NumbersPipe,
        PaddingPipe,
        ReplacePipe,
        HighlightSearch,
        StringToColorPipe,
        DateTimeToTimePipe,
        AttatchmentIconPipe,
        GroupByPipe,
        MenuButtonsItemsPipe,
        StageAgePipe,
        DateTimeToColorPipe,
        DateTimeToBackgroundPipe,
        ExchangeRateDatePipe,
        LogBoxStatusForegroundPipe,
        InvoiceDueDateForegroundPipe,
        RatesNumbersPipe,
        DateTimeToMSDYDatePipe,
        CustomNumbersPipe,
        FollowUpDatePipe,
        DateTimeToShortDatePipe,
        SafePipe,
        LogBoxStatusDatePipe,
        TimeToHoursMinutesPipe,
        CustomFieldResolverPipe,
    ];
export const ControlsComponents =
    [
        GeneralSendComponent,
        LogLabelComponent,
        LogToolTipComponent,
        LogTextBoxComponent,
        MultilineTextBoxWindow,
        LogTextBoxV2Component,
        LogLovComponent,
        LogLovV2Component,
        DWLovComponent,
        DWDateComponent,
        LogDatePickerComponent,
        AdvancedDatePickerComponent,
        LogCalendarComponent,
        TimeSelectComponent,
        LogCheckboxComponent,
        CustomDatePickerComponent,
        ObjectFieldTemplate,
        MyTemplate,
        StimulsoftViewerComponent,

        ListComponent,
        LogGridComponent,
        LogGridComponentV2,
        ListHeaderTemplateComponent,
        ListTemplateComponent,
        UsersQueryList,
        ChooseDatesComponent,
        SearchBox,
        RatioBoxComponent,
        LogCellTemplateComponent,
        LogColumnComponent,
        EditableLogGridComponent,
        InnerComponent,
        InnerSpanComponent,
        LogRowDetailsTemplateComponent,
        LogRowDetailsComponent,
        EditableListTemplateComponent,
        LogHeaderTemplateComponent,
        LogFooterTemplateComponent,
        LogFooterComponent,
        TipsComponent,
        CacheLogComponent,
        ImageComponent,
        LogTabsComponent,
        LogBooleanComponent,
        LogWaterMarkComponent,
        TextDesignComponent,
        PickListComponent,
        HelperFollowups,
        MainMenuFollowups,
        FollowupButton,
        ObjectFieldComponent,
        AuditAutomationTabComponent,
        SocialComponent,
        TimeInput,
        EntityFollowComponent,
        StimulsoftDesignerComponent,
        AutomationsConditionAreaComponent,
        LogitudeListBoxComponent,
        FroalaEditorComponent,
        DocsOutTabComponent,
        DocsInTabComponent,
        LastSuccessfulLoginComponent,
        LogTextBoxComponentV3,
        ImageLibraryComponent,
        TaskSchedulerHistoryComponent,
        DropdownMenuComponent,
        MultiSelectLOVComponent,
        
        FTBSchedulerTemplateComponent,
        SendInterfaceResultComponent,
        FTPAutomationDetailsComponent,
        SendDocumentResultComponent,
        AuditAutomationTabComponent,
        AutomationsConditionsViewDetailsComponent,
        CreateTaskResultComponent,
    ];
export const Components =
    [
        RootComponent,
        RootComponent_Cust,
        LoginComponent,
        DSVLoginProcessComponent,
        DSVMobileLoginProcessComponent,
        BlockScreenComponent,
        HomeComponent,
        SessionComponent,
        MainMenuComponent,
        EditComponent,
        EditTabComponent,
        MaintenanceComponent,
        MenuButtonsComponent,
        MenuButtonsComponentLoader,
        AdvanceSearchComponent,
        QueryListComponent,
        LogitudeListBoxComponent,
        GeneralTabComponent,
        GeneratedComponent,
        MultilineTextBoxWindow,

        MainMenuAutomationComponent,
        AutomationsSettingsComponent,
        AddEditAutomationsComponent,
        ChooseSpecificUserComponent,
        DelayAutomationconditionsComponent,
        ViewAutomationHistoryComponent,


        NewViewComponent,
        Export2ExcelControl,
        ExportBI2ExcelControl,
        QueryColumnsEditComponent,
        btnComponent,
        btnUpdateComponent,
        ToComponent,
        NewEntityComponent,
        LogSearchWindowComponent,
        DWLogSearchWindowComponent,
        ChooseUserComponent,
        DWLogSearchAddFieldsComponent,
        InfrastructureFieldTemplateComponent,

        LogSearchWindowButtonsComponent,
        DWLogSearchWindowFieldsComponent,
        AddFollowupComponent,
        AddDocumentFollowupComponent,
        DropBoxLogin,

        StimulsoftDesigner,

        ExportSettingAdvanceComponent,

        AuditAutomationTabComponent,

        WizardBaseComponent,
        WizardAddressCompnent,
        WizardAccountingComponent,
        SelectDocumentTypesComponent,

        AutomationsConditionAreaComponent,

        EraseTenantManagementDataComponent,

        FroalaEditorComponent,
        DocsOutTabComponent,
        DocsInTabComponent,
        BTEGeneralTabComponent,
        BTELogTabComponent,
        BTEParameterTabComponent,
        LastSuccessfulLoginComponent,
        FTBSchedulerTemplateComponent,

        PrivateLabelLoginProcessComponent,
         
        AutomationConditionsDetailsComponent,
        AutomationsConditionsViewDetailsComponent,
        IdentityShaamLandingPageComponent,        
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        let myResult: any = null;

        switch (name) {
            case 'RootComponent': { myResult = RootComponent; break; }
            case 'RootComponent_Cust': { myResult = RootComponent_Cust; break; }
            case 'LoginComponent': { myResult = LoginComponent; break; }
            case 'DSVLoginProcessComponent': { myResult = DSVLoginProcessComponent; break; }
            case 'DSVMobileLoginProcessComponent': { myResult = DSVMobileLoginProcessComponent; break; }
            case 'BlockScreenComponent': { myResult = BlockScreenComponent; break; }
            case 'HomeComponent': { myResult = HomeComponent; break; }
            case 'SessionComponent': { myResult = SessionComponent; break; }
            case 'MainMenuComponent': { myResult = MainMenuComponent; break; }
            case 'EditComponent': { myResult = EditComponent; break; }
            case 'EditTabComponent': { myResult = EditTabComponent; break; }
            case 'ListComponent': { myResult = ListComponent; break; }
            case 'MaintenanceComponent': { myResult = MaintenanceComponent; break; }
            case 'MenuButtonsComponent': { myResult = MenuButtonsComponent; break; }
            case 'MenuButtonsComponentLoader': { myResult = MenuButtonsComponentLoader; break; }
            case 'AdvanceSearchComponent': { myResult = AdvanceSearchComponent; break; }
            case 'QueryListComponent': { myResult = QueryListComponent; break; }
            case 'LogitudeListBoxComponent': { myResult = LogitudeListBoxComponent; break; }
            case 'GeneralTabComponent': { myResult = GeneralTabComponent; break; }
            case 'GeneratedComponent': { myResult = GeneratedComponent; break; }



            case 'ImageComponent': { myResult = ImageComponent; break; }
            case 'TextDesignComponent': { myResult = TextDesignComponent; break; }

            case 'MainMenuAutomationComponent': { myResult = MainMenuAutomationComponent; break; }
            case 'AutomationsSettingsComponent': { myResult = AutomationsSettingsComponent; break; }
            case 'AddEditAutomationsComponent': { myResult = AddEditAutomationsComponent; break; }
            case 'ChooseSpecificUserComponent': { myResult = ChooseSpecificUserComponent; break; }
            case 'DelayAutomationconditionsComponent': { myResult = DelayAutomationconditionsComponent; break; }
            case 'ViewAutomationHistoryComponent': { myResult = ViewAutomationHistoryComponent; break; }
            case 'AutomationConditionsDetailsComponent': { myResult = AutomationConditionsDetailsComponent; break; }
                 

            case 'NewViewComponent': { myResult = NewViewComponent; break; }
            case 'Export2ExcelControl': { myResult = Export2ExcelControl; break; }
            case 'ExportBI2ExcelControl': { myResult = ExportBI2ExcelControl; break; }
            case 'QueryColumnsEditComponent': { myResult = QueryColumnsEditComponent; break; }
            case 'btnComponent': { myResult = btnComponent; break; }
            case 'btnUpdateComponent': { myResult = btnUpdateComponent; break; }
            case 'ToComponent': { myResult = ToComponent; break; }
            case 'DWLogSearchAddFieldsComponent': { myResult = DWLogSearchAddFieldsComponent; break; }

            case 'NewEntityComponent': { myResult = NewEntityComponent; break; }
            case 'LogSearchWindowComponent': { myResult = LogSearchWindowComponent; break; }
            case 'DWLogSearchWindowComponent': { myResult = DWLogSearchWindowComponent; break; }
            case 'StimulsoftViewerComponent': { myResult = StimulsoftViewerComponent; break; }
            case 'TipsComponent': { myResult = TipsComponent; break; }
            case 'CacheLogComponent': { myResult = CacheLogComponent; break; }
            case 'ChooseUserComponent': { myResult = ChooseUserComponent; break; }

            case 'ChooseDatesComponent': { myResult = ChooseDatesComponent; break; }
            case 'InfrastructureFieldTemplateComponent': { myResult = InfrastructureFieldTemplateComponent; break; }

            case 'GeneralSendComponent': { myResult = GeneralSendComponent; break; }
            case 'LogSearchWindowButtonsComponent': { myResult = LogSearchWindowButtonsComponent; break; }
            case 'DWLogSearchWindowFieldsComponent': { myResult = DWLogSearchWindowFieldsComponent; break; }
            case 'AddFollowupComponent': { myResult = AddFollowupComponent; break; }
            case 'AddDocumentFollowupComponent': { myResult = AddDocumentFollowupComponent; break; }
            case 'DropBoxLogin': { myResult = DropBoxLogin; break; }
              case 'StimulsoftDesigner': { myResult = StimulsoftDesigner; break; }




            case 'ObjectFieldComponent': { myResult = ObjectFieldComponent; break; }
            case 'ExportSettingAdvanceComponent': { myResult = ExportSettingAdvanceComponent; break; }
            case 'AuditAutomationTabComponent': { myResult = AuditAutomationTabComponent; break; }
            case 'SocialComponent': { myResult = SocialComponent; break; }
            case 'LogTabsComponent': { myResult = LogTabsComponent; break; }
            case 'LogWaterMarkComponent': { myResult = LogWaterMarkComponent; break; }

            case 'EntityFollowComponent': { myResult = EntityFollowComponent; break; }
            case 'WizardBaseComponent': { myResult = WizardBaseComponent; break; }
            case 'WizardAddressCompnent': { myResult = WizardAddressCompnent; break; }
            case 'WizardAccountingComponent': { myResult = WizardAccountingComponent; break; }
            case 'SelectDocumentTypesComponent': { myResult = SelectDocumentTypesComponent; break; }
            case 'StimulsoftDesignerComponent': { myResult = StimulsoftDesignerComponent; break; }
            case 'AutomationsConditionAreaComponent': { myResult = AutomationsConditionAreaComponent; break; }
            case 'AutomationsConditionAreaComponent': { myResult = AutomationsConditionsViewDetailsComponent; break; }
                 

            case 'EraseTenantManagementDataComponent': { myResult = EraseTenantManagementDataComponent; break; }

            case 'FroalaEditorComponent': { myResult = FroalaEditorComponent; break; }
            case 'DocsInTabComponent': { myResult = DocsInTabComponent; break; }
            case 'DocsOutTabComponent': { myResult = DocsOutTabComponent; break; }
            case 'BTEGeneralTabComponent': { myResult = BTEGeneralTabComponent; break; }
            case 'BTELogTabComponent': { myResult = BTELogTabComponent; break; }
            case 'BTEParameterTabComponent': { myResult = BTEParameterTabComponent; break; }
            case 'LastSuccessfulLoginComponent': { myResult = LastSuccessfulLoginComponent; break; }
            case 'MultilineTextBoxWindow': { myResult = MultilineTextBoxWindow; break; }
            case 'ImageLibraryComponent': { myResult = ImageLibraryComponent; break; }
            case 'FTBSchedulerTemplateComponent': { myResult = FTBSchedulerTemplateComponent; break; }
            case 'SendInterfaceResultComponent': { myResult = SendInterfaceResultComponent; break; }
            case 'FTPAutomationDetailsComponent': { myResult = FTPAutomationDetailsComponent; break; }
            case 'SendDocumentResultComponent': { myResult = SendDocumentResultComponent; break; } 
            case 'PrivateLabelLoginProcessComponent': { myResult = PrivateLabelLoginProcessComponent; break; } 
            case 'CreateTaskResultComponent': { myResult = CreateTaskResultComponent; break; }
            case 'IdentityShaamLandingPageComponent': { myResult = IdentityShaamLandingPageComponent; break; }
                 
                 

                 
        }

        return myResult;
    }
}
