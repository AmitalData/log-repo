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
import {CreateTenantComponent} from './Components/CreateTenant/CreateTenantComponent';


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

        }

        return myResult;
    }
}
