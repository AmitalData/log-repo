"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var UserWorkspaceComponent_1 = require("./Components/UserWorkspaceComponent");
var LicensesManagementComponent_1 = require("./Components/LicensesManagementComponent");
var ColumnCheckBoxComponent_1 = require("./Components/ColumnCheckBoxComponent");
var NewUserComponent_1 = require("./Components/NewUserComponent");
var ResetUserPasswordComponent_1 = require("./Components/ResetUserPasswordComponent");
var SupportManagementComponent_1 = require("./Components/SupportManagementComponent");
var UserLoginHistoryComponent_1 = require("./Components//UserLoginHistoryComponent");
var UserPermissionsTabComponent_1 = require("./Components/EditTabs/UserPermissionsTabComponent");
var UserGeneralTabComponent_1 = require("./Components/EditTabs/UserGeneralTabComponent");
var UserRolesTabComponent_1 = require("./Components/EditTabs/UserRolesTabComponent");
var NewRoleComponent_1 = require("./Components/Roles/NewRoleComponent");
var EditRoleFeaturesComponent_1 = require("./Components/Roles/EditRoleFeaturesComponent");
var EditFeaturesRoleLinkComponent_1 = require("./Components/Roles/EditFeaturesRoleLinkComponent");
var ShowNewFeaturesComponent_1 = require("./Components/Roles/ShowNewFeaturesComponent");
var FeaturesEventChangesComponent_1 = require("./Components/Roles/FeaturesEventChangesComponent");
var RoleFeaturesEventsComponent_1 = require("./Components/Roles/RoleFeaturesEventsComponent");
var PackageFeaturesEventsComponent_1 = require("./Components/Packages/PackageFeaturesEventsComponent");
var UserPackagesComponent_1 = require("./Components/Packages/UserPackagesComponent");
var AddEditUserPackageComponent_1 = require("./Components/Packages/AddEditUserPackageComponent");
var EditPackageFeaturesComponent_1 = require("./Components/Packages/EditPackageFeaturesComponent");
var EditFeaturesPackageLinkComponent_1 = require("./Components/Packages/EditFeaturesPackageLinkComponent");
var DocumentFilingInboxTabComponent_1 = require("./Components/EditTabs/DocumentFilingInboxTabComponent");
var DevicesTabComponent_1 = require("./Components/EditTabs/DevicesTabComponent");
var UserUnlockComponent_1 = require("./Components/UserUnlockComponent/UserUnlockComponent");
var ChangePasswordComponent_1 = require("./Components/PersonalSettings/ChangePasswordComponent");
var UserDistributorComponent_1 = require("./Components/EditTabs/UserDistributorComponent");
var UserSearchComponent_1 = require("./Components/UsersSearch/UserSearchComponent");
exports.Components = [
    UserWorkspaceComponent_1.UserWorkspaceComponent,
    LicensesManagementComponent_1.LicensesManagementComponent,
    ColumnCheckBoxComponent_1.ColumnCheckBoxComponent,
    NewUserComponent_1.NewUserComponent,
    ResetUserPasswordComponent_1.ResetUserPasswordComponent,
    SupportManagementComponent_1.SupportManagementComponent,
    UserLoginHistoryComponent_1.UserLoginHistoryComponent,
    UserGeneralTabComponent_1.UserGeneralTabComponent,
    UserRolesTabComponent_1.UserRolesTabComponent,
    UserPermissionsTabComponent_1.UserPermissionsTabComponent,
    NewRoleComponent_1.NewRoleComponent,
    EditRoleFeaturesComponent_1.EditRoleFeaturesComponent,
    EditFeaturesRoleLinkComponent_1.EditFeaturesRoleLinkComponent,
    ShowNewFeaturesComponent_1.ShowNewFeaturesComponent,
    UserPackagesComponent_1.UserPackagesComponent,
    AddEditUserPackageComponent_1.AddEditUserPackageComponent,
    EditPackageFeaturesComponent_1.EditPackageFeaturesComponent,
    EditFeaturesPackageLinkComponent_1.EditFeaturesPackageLinkComponent,
    DevicesTabComponent_1.DevicesTabComponent,
    RoleFeaturesEventsComponent_1.RoleFeaturesEventsComponent,
    PackageFeaturesEventsComponent_1.PackageFeaturesEventsComponent,
    FeaturesEventChangesComponent_1.FeaturesEventChangesComponent,
    DocumentFilingInboxTabComponent_1.DocumentFilingInboxTabComponent,
    UserUnlockComponent_1.UserUnlockComponent,
    ChangePasswordComponent_1.ChangePasswordComponent,
    UserDistributorComponent_1.UserDistributorComponent,
    UserSearchComponent_1.UserSearchComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "UserWorkspaceComponent": {
                myResult = UserWorkspaceComponent_1.UserWorkspaceComponent;
                break;
            }
            case "LicensesManagementComponent": {
                myResult = LicensesManagementComponent_1.LicensesManagementComponent;
                break;
            }
            case "ColumnCheckBoxComponent": {
                myResult = ColumnCheckBoxComponent_1.ColumnCheckBoxComponent;
                break;
            }
            case "NewUserComponent": {
                myResult = NewUserComponent_1.NewUserComponent;
                break;
            }
            case "ResetUserPasswordComponent": {
                myResult = ResetUserPasswordComponent_1.ResetUserPasswordComponent;
                break;
            }
            case "SupportManagementComponent": {
                myResult = SupportManagementComponent_1.SupportManagementComponent;
                break;
            }
            case "UserLoginHistoryComponent": {
                myResult = UserLoginHistoryComponent_1.UserLoginHistoryComponent;
                break;
            }
            case "UserGeneralTabComponent": {
                myResult = UserGeneralTabComponent_1.UserGeneralTabComponent;
                break;
            }
            case "UserRolesTabComponent": {
                myResult = UserRolesTabComponent_1.UserRolesTabComponent;
                break;
            }
            case "UserPermissionsTabComponent": {
                myResult = UserPermissionsTabComponent_1.UserPermissionsTabComponent;
                break;
            }
            case "NewRoleComponent": {
                myResult = NewRoleComponent_1.NewRoleComponent;
                break;
            }
            case "EditRoleFeaturesComponent": {
                myResult = EditRoleFeaturesComponent_1.EditRoleFeaturesComponent;
                break;
            }
            case "EditFeaturesRoleLinkComponent": {
                myResult = EditFeaturesRoleLinkComponent_1.EditFeaturesRoleLinkComponent;
                break;
            }
            case "ShowNewFeaturesComponent": {
                myResult = ShowNewFeaturesComponent_1.ShowNewFeaturesComponent;
                break;
            }
            case "UserPackagesComponent": {
                myResult = UserPackagesComponent_1.UserPackagesComponent;
                break;
            }
            case "AddEditUserPackageComponent": {
                myResult = AddEditUserPackageComponent_1.AddEditUserPackageComponent;
                break;
            }
            case "EditPackageFeaturesComponent": {
                myResult = EditPackageFeaturesComponent_1.EditPackageFeaturesComponent;
                break;
            }
            case "EditFeaturesPackageLinkComponent": {
                myResult = EditFeaturesPackageLinkComponent_1.EditFeaturesPackageLinkComponent;
                break;
            }
            case "DevicesTabComponent": {
                myResult = DevicesTabComponent_1.DevicesTabComponent;
                break;
            }
            case "SupportManagementComponent": {
                myResult = SupportManagementComponent_1.SupportManagementComponent;
                break;
            }
            case "RoleFeaturesEventsComponent": {
                myResult = RoleFeaturesEventsComponent_1.RoleFeaturesEventsComponent;
                break;
            }
            case "PackageFeaturesEventsComponent": {
                myResult = PackageFeaturesEventsComponent_1.PackageFeaturesEventsComponent;
                break;
            }
            case "FeaturesEventChangesComponent": {
                myResult = FeaturesEventChangesComponent_1.FeaturesEventChangesComponent;
                break;
            }
            case "DocumentFilingInboxTabComponent": {
                myResult = DocumentFilingInboxTabComponent_1.DocumentFilingInboxTabComponent;
                break;
            }
            case "UserUnlockComponent": {
                myResult = UserUnlockComponent_1.UserUnlockComponent;
                break;
            }
            case "ChangePasswordComponent": {
                myResult = ChangePasswordComponent_1.ChangePasswordComponent;
                break;
            }
            case "UserDistributorComponent": {
                myResult = UserDistributorComponent_1.UserDistributorComponent;
                break;
            }
            case "UserSearchComponent": {
                myResult = UserSearchComponent_1.UserSearchComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map