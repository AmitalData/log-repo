import {UserWorkspaceComponent} from './Components/UserWorkspaceComponent';
import {LicensesManagementComponent} from './Components/LicensesManagementComponent';
import {ColumnCheckBoxComponent} from './Components/ColumnCheckBoxComponent';
import {NewUserComponent} from './Components/NewUserComponent';
import {ResetUserPasswordComponent} from './Components/ResetUserPasswordComponent';
import {SupportManagementComponent} from './Components/SupportManagementComponent';
import {UserLoginHistoryComponent} from './Components//UserLoginHistoryComponent';
import {UserPermissionsTabComponent} from './Components/EditTabs/UserPermissionsTabComponent';
import {UserGeneralTabComponent} from './Components/EditTabs/UserGeneralTabComponent';
import {UserRolesTabComponent} from './Components/EditTabs/UserRolesTabComponent';
import {NewRoleComponent} from './Components/Roles/NewRoleComponent';
import {EditRoleFeaturesComponent} from './Components/Roles/EditRoleFeaturesComponent';
import {EditFeaturesRoleLinkComponent} from './Components/Roles/EditFeaturesRoleLinkComponent';
import {ShowNewFeaturesComponent} from './Components/Roles/ShowNewFeaturesComponent';
import {FeaturesEventChangesComponent} from './Components/Roles/FeaturesEventChangesComponent';
import {RoleFeaturesEventsComponent} from './Components/Roles/RoleFeaturesEventsComponent';
import {PackageFeaturesEventsComponent} from './Components/Packages/PackageFeaturesEventsComponent';
import {UserPackagesComponent} from './Components/Packages/UserPackagesComponent';
import {AddEditUserPackageComponent} from './Components/Packages/AddEditUserPackageComponent';
import {EditPackageFeaturesComponent} from './Components/Packages/EditPackageFeaturesComponent';
import {EditFeaturesPackageLinkComponent} from './Components/Packages/EditFeaturesPackageLinkComponent';
import {DocumentFilingInboxTabComponent} from './Components/EditTabs/DocumentFilingInboxTabComponent';
import {DevicesTabComponent} from './Components/EditTabs/DevicesTabComponent';
import {UserUnlockComponent} from './Components/UserUnlockComponent/UserUnlockComponent';
import {ChangePasswordComponent} from './Components/PersonalSettings/ChangePasswordComponent';
import {UserDistributorComponent} from './Components/EditTabs/UserDistributorComponent';
import {UserSearchComponent} from './Components/UsersSearch/UserSearchComponent';

export const Components =
    [
        UserWorkspaceComponent,
        LicensesManagementComponent,
        ColumnCheckBoxComponent,
        NewUserComponent,
        ResetUserPasswordComponent,
        SupportManagementComponent,
        UserLoginHistoryComponent,
        UserGeneralTabComponent,
        UserRolesTabComponent,
        UserPermissionsTabComponent,
        NewRoleComponent,
        EditRoleFeaturesComponent,
        EditFeaturesRoleLinkComponent,
        ShowNewFeaturesComponent,
        UserPackagesComponent,
        AddEditUserPackageComponent,
        EditPackageFeaturesComponent,
        EditFeaturesPackageLinkComponent,
        DevicesTabComponent,
        RoleFeaturesEventsComponent,
        PackageFeaturesEventsComponent,
        FeaturesEventChangesComponent,
        DocumentFilingInboxTabComponent,
        UserUnlockComponent,
        ChangePasswordComponent,
        UserDistributorComponent,
        UserSearchComponent,

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "UserWorkspaceComponent": { myResult = UserWorkspaceComponent; break; }
            case "LicensesManagementComponent": { myResult = LicensesManagementComponent; break; }
            case "ColumnCheckBoxComponent": { myResult = ColumnCheckBoxComponent; break; }
            case "NewUserComponent": { myResult = NewUserComponent; break; }
            case "ResetUserPasswordComponent": { myResult = ResetUserPasswordComponent; break; }
            case "SupportManagementComponent": { myResult = SupportManagementComponent; break; }
            case "UserLoginHistoryComponent": { myResult = UserLoginHistoryComponent; break; }
            case "UserGeneralTabComponent": { myResult = UserGeneralTabComponent; break; }
            case "UserRolesTabComponent": { myResult = UserRolesTabComponent; break; }
            case "UserPermissionsTabComponent": { myResult = UserPermissionsTabComponent; break; }
            case "NewRoleComponent": { myResult = NewRoleComponent; break; }
            case "EditRoleFeaturesComponent": { myResult = EditRoleFeaturesComponent; break; }
            case "EditFeaturesRoleLinkComponent": { myResult = EditFeaturesRoleLinkComponent; break; }
            case "ShowNewFeaturesComponent": { myResult = ShowNewFeaturesComponent; break; }
            case "UserPackagesComponent": { myResult = UserPackagesComponent; break; }
            case "AddEditUserPackageComponent": { myResult = AddEditUserPackageComponent; break; }
            case "EditPackageFeaturesComponent": { myResult = EditPackageFeaturesComponent; break; }
            case "EditFeaturesPackageLinkComponent": { myResult = EditFeaturesPackageLinkComponent; break; }
            case "DevicesTabComponent": { myResult = DevicesTabComponent; break; }
            case "SupportManagementComponent": { myResult = SupportManagementComponent; break; }    
            case "RoleFeaturesEventsComponent": { myResult = RoleFeaturesEventsComponent; break; }
            case "PackageFeaturesEventsComponent": { myResult = PackageFeaturesEventsComponent; break; }
            case "FeaturesEventChangesComponent": { myResult = FeaturesEventChangesComponent; break; } 
            case "DocumentFilingInboxTabComponent": { myResult = DocumentFilingInboxTabComponent; break; }  
            case "UserUnlockComponent": { myResult = UserUnlockComponent; break; }
            case "ChangePasswordComponent": { myResult = ChangePasswordComponent; break; }
            case "UserDistributorComponent": { myResult = UserDistributorComponent; break; }
            case "UserSearchComponent": { myResult = UserSearchComponent; break; }


        }

        return myResult;
    }
}