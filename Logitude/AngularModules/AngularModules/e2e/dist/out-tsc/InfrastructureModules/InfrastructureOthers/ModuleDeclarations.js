"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ApiCredintialsComponent_1 = require("./Components/ApiCredintials/ApiCredintialsComponent");
var DatabaseBackupComponent_1 = require("./Components/CustomizeLogitude/DatabaseBackupComponent");
var TermsOfUseStartupComponent_1 = require("./Components/TermsOfUse/TermsOfUseStartupComponent");
var TermsofUseSignatureComponent_1 = require("./Components/TermsOfUse/TermsofUseSignatureComponent");
var DSVTermsOfUseStartupComponent_1 = require("./Components/TermsOfUse/CustomTermsOfUse/DSVTermsOfUseStartupComponent");
var SystemInfoComponent_1 = require("./Components/CustomizeLogitude/SystemInfoComponent");
var ActivationWizardComponent_1 = require("./Components/ActivationWizard/ActivationWizardComponent");
var HybridTenantThresholdComponent_1 = require("./Components/CustomizeLogitude/HybridTenantThresholdComponent");
var DropBoxConnectionComponent_1 = require("./Components/DropBox/DropBoxConnectionComponent");
var DropBoxTestFileComponent_1 = require("./Components/DropBox/DropBoxTestFileComponent");
var EmailNotificationsSettingsComponent_1 = require("./Components/EmailNotifications/EmailNotificationsSettingsComponent");
var IntegrationSystemsSetting_1 = require("./Components/IntegrationSystemsSetting/IntegrationSystemsSetting");
var CreateTenantPackageSelectionComponent_1 = require("./Components/CreateTenant/CreateTenantPackageSelectionComponent");
var CreateTenantValidationScreenComponent_1 = require("./Components/CreateTenant/CreateTenantValidationScreenComponent");
var TenantLoginPolicyComponent_1 = require("./Components/TenantSecurityPolicy/TenantLoginPolicyComponent");
var ErrorLogExceptionComponent_1 = require("./Components/ErrorLog/ErrorLogExceptionComponent");
var MoveTypeGeneralTabComponent_1 = require("./Components/MoveType/MoveTypeGeneralTabComponent");
var NewMoveTypeComponent_1 = require("./Components/MoveType/NewMoveTypeComponent");
var WebhookKeysComponent_1 = require("./Components/WebhookKeys/WebhookKeysComponent");
var WebhookTesterComponent_1 = require("./Components/WebhookKeys/WebhookTesterComponent");
var CreateTenantComponent_1 = require("./Components/CreateTenant/CreateTenantComponent");
exports.Components = [
    ApiCredintialsComponent_1.ApiCredintialsComponent,
    DatabaseBackupComponent_1.DatabaseBackupComponent,
    TermsOfUseStartupComponent_1.TermsOfUseStartupComponent,
    DSVTermsOfUseStartupComponent_1.DSVTermsOfUseStartupComponent,
    TermsofUseSignatureComponent_1.TermsofUseSignatureComponent,
    SystemInfoComponent_1.SystemInfoComponent,
    ActivationWizardComponent_1.ActivationWizardComponent,
    MoveTypeGeneralTabComponent_1.MoveTypeGeneralTabComponent,
    DropBoxConnectionComponent_1.DropBoxConnectionComponent,
    DropBoxTestFileComponent_1.DropBoxTestFileComponent,
    EmailNotificationsSettingsComponent_1.EmailNotificationsSettingsComponent,
    IntegrationSystemsSetting_1.IntegrationSystemsSetting,
    CreateTenantPackageSelectionComponent_1.CreateTenantPackageSelectionComponent,
    CreateTenantValidationScreenComponent_1.CreateTenantValidationScreenComponent,
    TenantLoginPolicyComponent_1.TenantLoginPolicyComponent,
    HybridTenantThresholdComponent_1.HybridTenantThresholdComponent,
    ErrorLogExceptionComponent_1.ErrorLogExceptionComponent,
    NewMoveTypeComponent_1.NewMoveTypeComponent,
    WebhookKeysComponent_1.WebhookKeysComponent,
    WebhookTesterComponent_1.WebhookTesterComponent,
    CreateTenantComponent_1.CreateTenantComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "ApiCredintialsComponent": {
                myResult = ApiCredintialsComponent_1.ApiCredintialsComponent;
                break;
            }
            case "DatabaseBackupComponent": {
                myResult = DatabaseBackupComponent_1.DatabaseBackupComponent;
                break;
            }
            case "TermsOfUseStartupComponent": {
                myResult = TermsOfUseStartupComponent_1.TermsOfUseStartupComponent;
                break;
            }
            case "DSVTermsOfUseStartupComponent": {
                myResult = DSVTermsOfUseStartupComponent_1.DSVTermsOfUseStartupComponent;
                break;
            }
            case "TermsofUseSignatureComponent": {
                myResult = TermsofUseSignatureComponent_1.TermsofUseSignatureComponent;
                break;
            }
            case "SystemInfoComponent": {
                myResult = SystemInfoComponent_1.SystemInfoComponent;
                break;
            }
            case "MoveTypeGeneralTabComponent": {
                myResult = MoveTypeGeneralTabComponent_1.MoveTypeGeneralTabComponent;
                break;
            }
            case "ActivationWizardComponent": {
                myResult = ActivationWizardComponent_1.ActivationWizardComponent;
                break;
            }
            case "DropBoxConnectionComponent": {
                myResult = DropBoxConnectionComponent_1.DropBoxConnectionComponent;
                break;
            }
            case "DropBoxTestFileComponent": {
                myResult = DropBoxTestFileComponent_1.DropBoxTestFileComponent;
                break;
            }
            case "EmailNotificationsSettingsComponent": {
                myResult = EmailNotificationsSettingsComponent_1.EmailNotificationsSettingsComponent;
                break;
            }
            case "CreateTenantPackageSelectionComponent": {
                myResult = CreateTenantPackageSelectionComponent_1.CreateTenantPackageSelectionComponent;
                break;
            }
            case "CreateTenantValidationScreenComponent": {
                myResult = CreateTenantValidationScreenComponent_1.CreateTenantValidationScreenComponent;
                break;
            }
            case "TenantLoginPolicyComponent": {
                myResult = TenantLoginPolicyComponent_1.TenantLoginPolicyComponent;
                break;
            }
            case "IntegrationSystemsSetting": {
                myResult = IntegrationSystemsSetting_1.IntegrationSystemsSetting;
                break;
            }
            case "HybridTenantThresholdComponent": {
                myResult = HybridTenantThresholdComponent_1.HybridTenantThresholdComponent;
                break;
            }
            case "ErrorLogExceptionComponent": {
                myResult = ErrorLogExceptionComponent_1.ErrorLogExceptionComponent;
                break;
            }
            case "NewMoveTypeComponent": {
                myResult = NewMoveTypeComponent_1.NewMoveTypeComponent;
                break;
            }
            case "WebhookKeysComponent": {
                myResult = WebhookKeysComponent_1.WebhookKeysComponent;
                break;
            }
            case "WebhookTesterComponent": {
                myResult = WebhookTesterComponent_1.WebhookTesterComponent;
                break;
            }
            case "CreateTenantComponent": {
                myResult = CreateTenantComponent_1.CreateTenantComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map