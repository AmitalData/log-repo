import {BrandingTabComponent} from './Components/TenantManagement/BrandingTabComponent';
import {TenantManagementGeneralTabComponent} from './Components/TenantManagement/TenantManagementGeneralTabComponent';
import {TenantManagementStatisticsTabComponent} from './Components/TenantManagement/TenantManagementStatisticsTabComponent';
import {CCSSettingsTabComponent} from './Components/TenantManagement/CCSSettingsTabComponent';
import {SupportTabComponent} from './Components/TenantManagement/SupportTabComponent';
import {AddEditAddOnComponent} from './Components/TenantManagement/AddEditAddOnComponent';
import {AddEditLicenceComponent} from './Components/TenantManagement/AddEditLicenceComponent';
import {AddEditPrivateLabelsComponent} from './Components/TenantManagement/AddEditPrivateLabelsComponent';
import {PrivateLabelLoadComponent} from './Components/TenantManagement/PrivateLabelLoadComponent';
import { CargoTrackingBrandingComponent } from './Components/TenantManagement/CargoTrackingBrandingComponent'; 

export const Components =
    [
        BrandingTabComponent,
        TenantManagementGeneralTabComponent,
        TenantManagementStatisticsTabComponent,
        CCSSettingsTabComponent,
        SupportTabComponent,
        AddEditAddOnComponent,
        AddEditLicenceComponent,
        AddEditPrivateLabelsComponent,
        PrivateLabelLoadComponent,
        CargoTrackingBrandingComponent, 
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "BrandingTabComponent": { myResult = BrandingTabComponent; break; }
            case "TenantManagementGeneralTabComponent": { myResult = TenantManagementGeneralTabComponent; break; }
            case "TenantManagementStatisticsTabComponent": { myResult = TenantManagementStatisticsTabComponent; break; }
            case "CCSSettingsTabComponent": { myResult = CCSSettingsTabComponent; break; }
            case "SupportTabComponent": { myResult = SupportTabComponent; break; }
            case "AddEditAddOnComponent": { myResult = AddEditAddOnComponent; break; }
            case "AddEditLicenceComponent": { myResult = AddEditLicenceComponent; break; }
            case "AddEditPrivateLabelsComponent": { myResult = AddEditPrivateLabelsComponent; break; }
            case "PrivateLabelLoadComponent": { myResult = PrivateLabelLoadComponent; break; }
            case "CargoTrackingBrandingComponent": { myResult = CargoTrackingBrandingComponent; break; } 

        }

        return myResult;
    }
}
