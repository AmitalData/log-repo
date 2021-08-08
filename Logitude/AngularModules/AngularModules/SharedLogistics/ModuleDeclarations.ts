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

export const ControlsComponents =
    [
        SharedLogisticsSettingComponent,
        SharedLogisticsEventPermissiosComponent,
        CargoTrackingMilestonesPermissiosComponent,
        SharedLogisticsDocumentPermissiosComponent,
        SharedLogisticsMoneyPermissiosComponent,
        SharedLogisticsPartnersPermissiosComponent,        
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
        }

        return myResult;
    }
}
