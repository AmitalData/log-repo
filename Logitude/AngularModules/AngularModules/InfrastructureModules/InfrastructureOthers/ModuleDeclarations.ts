import {ApiCredintialsComponent} from './Components/ApiCredintials/ApiCredintialsComponent';
import {DatabaseBackupComponent} from './Components/CustomizeLogitude/DatabaseBackupComponent';
import {TermsOfUseStartupComponent} from './Components/TermsOfUse/TermsOfUseStartupComponent';
import {TermsofUseSignatureComponent} from './Components/TermsOfUse/TermsofUseSignatureComponent';
import {DSVTermsOfUseStartupComponent} from './Components/TermsOfUse/CustomTermsOfUse/DSVTermsOfUseStartupComponent';
import {SystemInfoComponent} from './Components/CustomizeLogitude/SystemInfoComponent';
import {ActivationWizardComponent} from './Components/ActivationWizard/ActivationWizardComponent';
import {HybridTenantThresholdComponent} from './Components/CustomizeLogitude/HybridTenantThresholdComponent';
import {DropBoxConnectionComponent} from './Components/DropBox/DropBoxConnectionComponent';
import { DropBoxTestFileComponent } from './Components/DropBox/DropBoxTestFileComponent';
import { EmailNotificationsSettingsComponent } from './Components/EmailNotifications/EmailNotificationsSettingsComponent';
import { IntegrationSystemsSetting} from './Components/IntegrationSystemsSetting/IntegrationSystemsSetting';
import {CreateTenantPackageSelectionComponent} from './Components/CreateTenant/CreateTenantPackageSelectionComponent';
import {CreateTenantValidationScreenComponent} from './Components/CreateTenant/CreateTenantValidationScreenComponent';
import {TenantLoginPolicyComponent} from './Components/TenantSecurityPolicy/TenantLoginPolicyComponent';
import {ErrorLogExceptionComponent} from './Components/ErrorLog/ErrorLogExceptionComponent';
import {MoveTypeGeneralTabComponent} from './Components/MoveType/MoveTypeGeneralTabComponent';
import { NewMoveTypeComponent } from './Components/MoveType/NewMoveTypeComponent';
import { WebhookKeysComponent } from './Components/WebhookKeys/WebhookKeysComponent';
import { WebhookTesterComponent } from './Components/WebhookKeys/WebhookTesterComponent';
import { CreateTenantComponent } from './Components/CreateTenant/CreateTenantComponent'; 
import { PriceStepsGeneralTabComponent } from './Components/PriceSteps/PriceStepsGeneralTabComponent';
import { CustomEventTypeComponent } from './Components/EventType/CustomEventTypeComponent';
import { EventRemarksComponent } from './Components/EventType/EventRemarksComponent';
import { FeatureToggleGeneralTabComponent } from './Components/FeatureToggle/FeatureToggleGeneralTabComponent';
import { NewFeatureToggleComponent } from './Components/FeatureToggle/NewFeatureToggleComponent';
import { OceanInsightsSettingsComponent } from './Components/OceanInsightsSetting/OceanInsightsSettingsComponent';
import { NewImageLibraryComponent } from './Components/ImageLibrary/NewImageLibraryComponent';
import { ImageLibraryGeneralTabComponent } from './Components/ImageLibrary/ImageLibraryGeneralTabComponent';
import { VizionAutomaticRequestComponent } from './Components/Vizion/VizionAutomaticRequestComponent';
import { TermsofUseComponent } from './Components/TermsOfUse/TermsofUseComponent';
import { CustomsCloudComponent } from './Components/CustomsCloud/CustomsCloudComponent';
import { LogitudeGridSimpleComponent } from './AmitalAPI/components/LogitudeGridSimpleComponent';
import { APISettingsComponent } from './AmitalAPI/APISettingsComponent';
import { AmitalAPIRequestsComponent } from './AmitalAPI/AmitalAPIRequestsComponent';
import { AmitalAPISettingsComponent } from './AmitalAPI/AmitalAPISettingsComponent';
import { AmitalAPIAddApiWindowComponent } from './AmitalAPI/WindowsComponent/AmitalAPIAddApiWindowComponent';
import { AmitalAPIAddClientWindowComponent } from './AmitalAPI/WindowsComponent/AmitalAPIAddClientWindowComponent';
import { AmitalAPIAddSchemaWindowComponent } from './AmitalAPI/WindowsComponent/AmitalAPIAddSchemaWindowComponent';
import { AmitalAPISchemaTable } from './AmitalAPI/components/AmitalAPISchemaTable';
import { CloseSaveButtonsComponent } from './AmitalAPI/components/CloseSaveButtonsComponent';
import { LogTexBoxFormComponent } from './AmitalAPI/components/LogTexBoxFormComponent';

export const Components =
    [
        ApiCredintialsComponent,
        DatabaseBackupComponent,
        TermsOfUseStartupComponent,
        DSVTermsOfUseStartupComponent,
        TermsofUseSignatureComponent,
        SystemInfoComponent,
        ActivationWizardComponent,
        MoveTypeGeneralTabComponent,
        DropBoxConnectionComponent,
        DropBoxTestFileComponent,
        EmailNotificationsSettingsComponent,
        IntegrationSystemsSetting,       
        CreateTenantPackageSelectionComponent,
        CreateTenantValidationScreenComponent,
        TenantLoginPolicyComponent,
        HybridTenantThresholdComponent,
        ErrorLogExceptionComponent,
        NewMoveTypeComponent,
        WebhookKeysComponent,
        WebhookTesterComponent,
        CreateTenantComponent,
        PriceStepsGeneralTabComponent,
        CustomEventTypeComponent,
        EventRemarksComponent,
        FeatureToggleGeneralTabComponent,
        NewFeatureToggleComponent,
        OceanInsightsSettingsComponent,
        NewImageLibraryComponent,
        ImageLibraryGeneralTabComponent,
        VizionAutomaticRequestComponent,
        TermsofUseComponent,
        CustomsCloudComponent,
        APISettingsComponent,
        AmitalAPISettingsComponent,
        AmitalAPIAddApiWindowComponent,
        AmitalAPIAddClientWindowComponent,
        AmitalAPIAddSchemaWindowComponent,
        AmitalAPISchemaTable,
        LogTexBoxFormComponent,
        CloseSaveButtonsComponent,
        AmitalAPIRequestsComponent,
        LogitudeGridSimpleComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ApiCredintialsComponent": { myResult = ApiCredintialsComponent; break; }
            case "DatabaseBackupComponent": { myResult = DatabaseBackupComponent; break; }
            case "TermsOfUseStartupComponent": { myResult = TermsOfUseStartupComponent; break; }
            case "DSVTermsOfUseStartupComponent": { myResult = DSVTermsOfUseStartupComponent; break; }
            case "TermsofUseSignatureComponent": { myResult = TermsofUseSignatureComponent; break; }
            case "SystemInfoComponent": { myResult = SystemInfoComponent; break; }
            case "MoveTypeGeneralTabComponent": { myResult = MoveTypeGeneralTabComponent; break; }
            case "ActivationWizardComponent": { myResult = ActivationWizardComponent; break; }
            case "DropBoxConnectionComponent": { myResult = DropBoxConnectionComponent; break; }
            case "DropBoxTestFileComponent": { myResult = DropBoxTestFileComponent; break; }
            case "EmailNotificationsSettingsComponent": { myResult = EmailNotificationsSettingsComponent; break; }
            case "CreateTenantPackageSelectionComponent": { myResult = CreateTenantPackageSelectionComponent; break; }
            case "CreateTenantValidationScreenComponent": { myResult = CreateTenantValidationScreenComponent; break; }
            case "TenantLoginPolicyComponent": { myResult = TenantLoginPolicyComponent; break; }                
            case "IntegrationSystemsSetting": { myResult = IntegrationSystemsSetting; break; }
            case "HybridTenantThresholdComponent": { myResult = HybridTenantThresholdComponent; break; }
            case "ErrorLogExceptionComponent": { myResult = ErrorLogExceptionComponent; break; }
            case "NewMoveTypeComponent": { myResult = NewMoveTypeComponent; break; }
            case "WebhookKeysComponent": { myResult = WebhookKeysComponent; break; }
            case "WebhookTesterComponent": { myResult = WebhookTesterComponent; break; }
            case "CreateTenantComponent": { myResult = CreateTenantComponent; break; }
            case "PriceStepsGeneralTabComponent": { myResult = PriceStepsGeneralTabComponent; break; }
            case "CustomEventTypeComponent": { myResult = CustomEventTypeComponent; break; }
            case "EventRemarksComponent": { myResult = EventRemarksComponent; break; }
            case "FeatureToggleGeneralTabComponent": { myResult = FeatureToggleGeneralTabComponent; break; }
            case "NewFeatureToggleComponent": { myResult = NewFeatureToggleComponent; break; }
            case "OceanInsightsSettingsComponent": { myResult = OceanInsightsSettingsComponent; break; }
            case "NewImageLibraryComponent": { myResult = NewImageLibraryComponent; break; }
            case "ImageLibraryGeneralTabComponent": { myResult = ImageLibraryGeneralTabComponent; break; }
            case "VizionAutomaticRequestComponent": { myResult = VizionAutomaticRequestComponent; break; }
            case "TermsofUseComponent": { myResult = TermsofUseComponent; break; }
            case "CustomsCloudComponent": { myResult = CustomsCloudComponent; break; }
            case "APISettingsComponent": { myResult = APISettingsComponent; break; }
            case "AmitalAPISettingsComponent": { myResult = AmitalAPISettingsComponent; break; }
            case "AmitalAPIAddApiWindowComponent": { myResult = AmitalAPIAddApiWindowComponent; break; }
            case "AmitalAPIAddClientWindowComponent": { myResult = AmitalAPIAddClientWindowComponent; break; }
            case "AmitalAPIAddSchemaWindowComponent": { myResult = AmitalAPIAddSchemaWindowComponent; break; }
            case "AmitalAPISchemaTable": { myResult = AmitalAPISchemaTable; break; }
            case "AmitalAPIRequestsComponent": { myResult = AmitalAPIRequestsComponent; break; }
            case "LogTexBoxFormComponent": { myResult = LogTexBoxFormComponent; break; }
            case "CloseSaveButtonsComponent": { myResult = CloseSaveButtonsComponent; break; }
            case "LogitudeGridSimpleComponent": { myResult = LogitudeGridSimpleComponent; break; }
        }

        return myResult;
    }
}
