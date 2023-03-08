import {SharedLogisticsMainComponent} from './Components/SharedLogisticsMainComponent';
import {SharedLogisticsSettingComponent} from './Components/SharedLogisticsSettingComponent';
import {SharedLogisticsWizardComponent} from './Components/SharedLogisticsWizardComponent';
import {SharedLogisticsEventPermissiosComponent} from './Components/SharedLogisticsEventPermissiosComponent';
import {CargoTrackingMilestonesPermissiosComponent} from './Components/CargoTrackingMilestonesPermissiosComponent';
import {SharedLogisticsDocumentPermissiosComponent} from './Components/SharedLogisticsDocumentPermissiosComponent';
import {ActivityZoomComponent} from './Components/ActivityZoomComponent';
import {InviteCustomersComponent} from './Components/InviteCustomersComponent';
import {SharedMessageComponent} from './Components/SharedMessageComponent';
import {ViewBlocedCustomerComponent} from './Components/ViewBlocedCustomerComponent';
import {SharedLogisticMainMenuComponent} from './Components/SharedLogisticMainMenuComponent';
import {CutsomerTenantAccessManagementComponent} from './Components/CutsomerTenantAccessManagementComponent';
import {RelatedCustomerComponent} from './Components/RelatedCustomerComponent';
import {AddEditCustomerTenantAccessCardComponent} from './Components/AddEditCustomerTenantAccessCardComponent';
import {EditRelatedCustomerComponent} from './Components/EditRelatedCustomerComponent';
import {AddCustomerBatchComponent} from './Components/AddCustomerBatchComponent';
import {FieldTemplateComponent} from './Components/Templates/FieldTemplateComponent';
import {TenantAccessSettingsComponent} from './Components/TenantAccessSettingsComponent';
import { SharedLogisticsMoneyPermissiosComponent } from './Components/SharedLogisticsMoneyPermissiosComponent';
import { SharedLogisticsPartnersPermissiosComponent } from './Components/SharedLogisticsPartnersPermissiosComponent';
import { SharedInvoicesWorkspaceComponent } from './Components/Workspaces/SharedInvoicesWorkspaceComponent';
import { SharedShipmentsWorkspaceComponent } from './Components/Workspaces/SharedShipmentsWorkspaceComponent';
import { TemplateTypeComponent } from './Components/TemplateTypeComponent';
import { SharedLogisticsDigitalPortalComponent } from './Components/SharedLogisticsDigitalPortalComponent';
import { DigitalPortalCustomizationMainComponent } from './Components/DigitalPortal/DigitalPortalCustomizationMainComponent';
import { DigitalPortalLanguageSettingsComponent } from './Components/DigitalPortal/DigitalPortalLanguageSettingsComponent';
import { DigitalPortalCustomizationChageLabelsComponent } from './Components/DigitalPortal/DigitalPortalCustomizationChageLabelsComponent';
import { DigitalPortalCustomizationTranslateLabelsComponent } from './Components/DigitalPortal/DigitalPortalCustomizationTranslateLabelsComponent';
import { DigitalPortalCustomizationShowHideFieldsComponent } from './Components/DigitalPortal/DigitalPortalCustomizationShowHideFieldsComponent';
import { DigitalPortalCustomizationScreenLayoutComponent } from './Components/DigitalPortal/DigitalPortalCustomizationScreenLayoutComponent'; 
import { AddDigitalFieldCodeComponent } from './Components/DigitalPortal/AddDigitalFieldCodeComponent';
import { AddDigitalPredefinedComponent } from './Components/DigitalPortal/AddDigitalPredefinedComponent';
import { AddDigitalLogitudeFieldComponent } from './Components/DigitalPortal/AddDigitalLogitudeFieldComponent';
import { DigitalButtonComponent } from './Components/DigitalPortal/DigitalButtonComponent';
import { DigitalCheckBoxComponent } from './Components/DigitalPortal/DigitalCheckBoxComponent';
import { DigitalPortalCustomizationSubObjectsComponent } from './Components/DigitalPortal/DigitalPortalCustomizationSubObjectsComponent';


export const ControlsComponents =
    [
        SharedLogisticsSettingComponent,
        SharedLogisticsEventPermissiosComponent,
        CargoTrackingMilestonesPermissiosComponent,
        SharedLogisticsDocumentPermissiosComponent,
        SharedLogisticsMoneyPermissiosComponent,
        SharedLogisticsPartnersPermissiosComponent,
        SharedLogisticsDigitalPortalComponent,
    ];


export const Components =
    [
        SharedLogisticsMainComponent,
        SharedLogisticsSettingComponent,
        SharedLogisticsWizardComponent,
        SharedLogisticsEventPermissiosComponent,
        CargoTrackingMilestonesPermissiosComponent,
        SharedLogisticsDocumentPermissiosComponent,
        ActivityZoomComponent,
        InviteCustomersComponent,
        SharedMessageComponent,
        ViewBlocedCustomerComponent,
        SharedLogisticMainMenuComponent,
        CutsomerTenantAccessManagementComponent,
        RelatedCustomerComponent,
        AddEditCustomerTenantAccessCardComponent,
        EditRelatedCustomerComponent,
        AddCustomerBatchComponent,
        FieldTemplateComponent,
        TenantAccessSettingsComponent,
        SharedLogisticsMoneyPermissiosComponent,
        SharedLogisticsPartnersPermissiosComponent,
        SharedInvoicesWorkspaceComponent,
        SharedShipmentsWorkspaceComponent,
        TemplateTypeComponent,
        SharedLogisticsDigitalPortalComponent,
        DigitalPortalCustomizationMainComponent,
        DigitalPortalLanguageSettingsComponent,
        DigitalPortalCustomizationChageLabelsComponent,
        DigitalPortalCustomizationTranslateLabelsComponent,
        DigitalPortalCustomizationShowHideFieldsComponent,
        DigitalPortalCustomizationScreenLayoutComponent,
        AddDigitalFieldCodeComponent,
        AddDigitalPredefinedComponent,
        AddDigitalLogitudeFieldComponent,
        DigitalButtonComponent,
        DigitalCheckBoxComponent,
        DigitalPortalCustomizationSubObjectsComponent

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;
        TenantAccessSettingsComponent
        switch (name) {
            case "SharedLogisticsMainComponent": { myResult = SharedLogisticsMainComponent; break; }
            case "SharedLogisticsSettingComponent": { myResult = SharedLogisticsSettingComponent; break; }
            case "SharedLogisticsWizardComponent": { myResult = SharedLogisticsWizardComponent; break; }
            case "SharedLogisticsEventPermissiosComponent": { myResult = SharedLogisticsEventPermissiosComponent; break; }
            case "CargoTrackingMilestonesPermissiosComponent": { myResult = CargoTrackingMilestonesPermissiosComponent; break; } 
            case "SharedLogisticsDocumentPermissiosComponent": { myResult = SharedLogisticsDocumentPermissiosComponent; break; } 
            case "ActivityZoomComponent": { myResult = ActivityZoomComponent; break; }  
            case "InviteCustomersComponent": { myResult = InviteCustomersComponent; break; }  
            case "SharedMessageComponent": { myResult = SharedMessageComponent; break; } 
            case "ViewBlocedCustomerComponent": { myResult = ViewBlocedCustomerComponent; break; } 
            case "SharedLogisticMainMenuComponent": { myResult = SharedLogisticMainMenuComponent; break; } 
            case "CutsomerTenantAccessManagementComponent": { myResult = CutsomerTenantAccessManagementComponent; break; }
            case "RelatedCustomerComponent": { myResult = RelatedCustomerComponent; break; }
            case "AddEditCustomerTenantAccessCardComponent": { myResult = AddEditCustomerTenantAccessCardComponent; break; }
            case "EditRelatedCustomerComponent": { myResult = EditRelatedCustomerComponent; break; }      
            case "AddCustomerBatchComponent": { myResult = AddCustomerBatchComponent; break; }      
            case "FieldTemplateComponent": { myResult = FieldTemplateComponent; break; }      
            case "TenantAccessSettingsComponent": { myResult = TenantAccessSettingsComponent; break; } 
            case "SharedLogisticsMoneyPermissiosComponent": { myResult = SharedLogisticsMoneyPermissiosComponent; break; } 
            case "SharedLogisticsPartnersPermissiosComponent": { myResult = SharedLogisticsPartnersPermissiosComponent; break; }
            case "SharedInvoicesWorkspaceComponent": { myResult = SharedInvoicesWorkspaceComponent; break; }
            case "SharedShipmentsWorkspaceComponent": { myResult = SharedShipmentsWorkspaceComponent; break; } 
            case "TemplateTypeComponent": { myResult = TemplateTypeComponent; break; }
            case "SharedLogisticsDigitalPortalComponent": { myResult = SharedLogisticsDigitalPortalComponent; break; }
            case "DigitalPortalCustomizationMainComponent": { myResult = DigitalPortalCustomizationMainComponent; break; }
            case "DigitalPortalLanguageSettingsComponent": { myResult = DigitalPortalLanguageSettingsComponent; break; }
            case "DigitalPortalCustomizationChageLabelsComponent": { myResult = DigitalPortalCustomizationChageLabelsComponent; break; }
            case "DigitalPortalCustomizationTranslateLabelsComponent": { myResult = DigitalPortalCustomizationTranslateLabelsComponent; break; }
            case "DigitalPortalCustomizationShowHideFieldsComponent": { myResult = DigitalPortalCustomizationShowHideFieldsComponent; break; }
            case "DigitalPortalCustomizationScreenLayoutComponent": { myResult = DigitalPortalCustomizationScreenLayoutComponent; break; }
            case "AddDigitalFieldCodeComponent": { myResult = AddDigitalFieldCodeComponent; break; }
            case "AddDigitalPredefinedComponent": { myResult = AddDigitalPredefinedComponent; break; }
            case "AddDigitalLogitudeFieldComponent": { myResult = AddDigitalLogitudeFieldComponent; break; }
            case "DigitalButtonComponent": { myResult = DigitalButtonComponent; break; }
            case "DigitalCheckBoxComponent": { myResult = DigitalCheckBoxComponent; break; }
            case "DigitalPortalCustomizationSubObjectsComponent": { myResult = DigitalPortalCustomizationSubObjectsComponent; break; }
        }

        return myResult;
    }
}
