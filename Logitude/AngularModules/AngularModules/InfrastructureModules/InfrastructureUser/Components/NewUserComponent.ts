import {UserRolesItemClass} from './EditTabs/UserRolesTabComponent';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {UserPM} from '../../../Common/EntityPMs/UserPM';
import {RolePM} from '../../../Common/EntityPMs/RolePM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ClassLevelValidator} from '../../../Infrastructure/Validators/ClassLevelValidator';
import {AppTool, DateTool,FormatTool} from '../../../Infrastructure/Tools';
import {PasswordChangeService} from '../../../Common/Services/Others/PasswordChangeService';
import {UserPMService} from '../../../Common/Services/StandardPMs/UserPMService';
import {RoleExtendedPMService} from '../../../Common/Services/ExtendedPMs/RoleExtendedPMService';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { FeatureToggleList } from '../../../Infrastructure/EntityLists/FeatureToggleList';
import { UserLicenseArgs } from '../../../Infrastructure/Args';
import { PackageListService } from '../../../Common/Services/StandardLists/PackageListService';
import { PackageList } from '../../../Common/EntityLists/PackageList';
import { UserLicensePM } from '../../../Common/EntityPMs/UserLicensePM';
import { UserExtendedPMService } from '../../../Common/Services/ExtendedPMs/UserExtendedPMService';

@Component({
    
    selector: 'NewUser',
    templateUrl: './NewUserComponent.html',
    providers: [PasswordChangeService, UserPMService, RoleExtendedPMService]
})

export class NewUserComponent extends BaseComponent implements OnInit {
  public AdditionalPackagesOnly: boolean = false;

    ReTypePassword: string = "";
    UserId: string;
    validator: ClassLevelValidator;
    Info1Text: string;
    IsPasswordDisable: boolean;
    Info1Visibility: boolean;
    ObjectFieldHelp: string = "";
    LicencedUserVisible: boolean;
    ExpirationDateVisible: boolean;
    IsDistributorVisible: boolean;
    IsSalesmanVisible: boolean;
    PersonalIdVisible: boolean;
    public ObsList: UserRolesItemClass[];
    public allRoles: RolePM[];
    DistributorCodeVisible: boolean;
    DemoTenantMessageVisibility: boolean;
    IsScreenEnabled: boolean;
    public IsAdditionalPackagesOnlyVisible: boolean = false;
    //IsFreelancerVisible: boolean = false;
    IsCurrentUserFreelancer: boolean = false;
    SelectedUserRolesItemClass: UserRolesItemClass;
    CanSave: boolean = true;
    NewUserPM: UserPM = new UserPM();
    private userPMService: UserPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _passwordChangeService: PasswordChangeService, public _roleExtendedPMService: RoleExtendedPMService) {
        super();

        if (this.userPMService == null) {
            this.userPMService = new UserPMService();
        }

        this.validator = new ClassLevelValidator();
        this.NewUserPM.Tenant = SessionLocator.Tenant;
        this.NewUserPM.BusinessUnitId = SessionLocator.Tenant.toString();

        this.IsCurrentUserFreelancer = SessionLocator.LoggedUserPM.IsFreelancer;

        //if (ObjectsLocator.GlobalSetting?.WorkEnvironment == "customs") {
        if (ObjectsLocator != null && ObjectsLocator.GlobalSetting != null && ObjectsLocator.GlobalSetting?.WorkEnvironment == "customs") {
            if (this.IsCurrentUserFreelancer) {
                //this.NewUserPM.IsFreelancer = true;
                //this.IsFreelancerVisible = false;
            } else {
                //this.IsFreelancerVisible = true;
            }
        }


    }

    ngOnInit() {
        this.Run();
    }

    RolesAreaVisibility: boolean;

    Run() {
        if (FeatureLocator.HasFeaturePermession("User", "ROLES")) this.RolesAreaVisibility = true;


        this.IsScreenEnabled = true;

        if (ObjectsLocator.IsDemoTenant(SessionInfo.LoggedUserTenant.toString())) {

            this.DemoTenantMessageVisibility = true;
            this.IsScreenEnabled = false;

            if (SessionInfo.LoggedUserPM.IsCustomerCare) {
                this.IsScreenEnabled = true;
                this.IsPasswordDisable = false;
                this.DemoTenantMessageVisibility = false;

            }

            var date: Date = DateTool.GetCurrentDateAsUtc();
            var month = date.getUTCMonth();
            var year = date.getUTCFullYear();
            var day = date.getUTCDate();




            var result = new Date();
            result.setUTCFullYear(year);
            result.setUTCMonth(month);
            result.setUTCDate(day + 7);

            this.NewUserPM.ExpirationDate = result;
        }


        this.SetUiProperties_Visibility();
        this.SetUiProperties_IsEnabled();

        this.LoadUserRolesMethod();

        var feature = FeatureLocator.Features.filter(x => x.Code === "PERSONALID")[0];

        if (feature == null) this.PersonalIdVisible = false;
        else this.PersonalIdVisible = true;

    }

    LoadUserRolesMethod() {
        this._roleExtendedPMService.GetRolesForUser(null, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.allRoles = myResult;
                    this.BuildObsList();
                }
            }
        });
    }

    public ValidationErrorsList: string[];
    SaveButtonClicked() {
        var Password = "";
        if (this.CanSave) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = this.validator.Validate("User", this.NewUserPM);

            if (this.NewUserPM.Password) {
                Password = this.NewUserPM.Password.trim();
            }

            else {
                this.ValidationErrorsList.push("Please fill the password field!");
            }

            if (this.ReTypePassword) {
                this.ReTypePassword = this.ReTypePassword.trim();
            }

            if (this.ObsList.filter(d => d.IsActive).length == 0) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.AddRoleToUser"));
            }

            if (Password != this.ReTypePassword) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.CurrentPasswordDoesntMatchYourInput"));
            }

            if (!FormatTool.IsEmail(this.NewUserPM.Email)) {
                this.ValidationErrorsList.push("Invalid email format!");
            }

            if (this.ValidationErrorsList.length == 0) {
                this.ValidationErrorsList = [];
                this.NewUserPM.Tenant = SessionInfo.LoggedUserTenant;
                this.NewUserPM.Technology = "AG";
                this.NewUserPM.LayoutDirection = "rtl";

                if (SessionLocator.TenantPM.HebrewTenant) {
                    this.NewUserPM.ShowLocalNameInLOV = true;
                    this.NewUserPM.DontShowLocalLabels = false;
                    this.NewUserPM.LayoutDirection = "rtl";
                }

                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

                this.userPMService.insert(this.NewUserPM).subscribe((myResult: any) => {
                    if (myResult) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (myResult.HasError) {
                            myResult.ErrorsArray.forEach((item) => {
                                this.ValidationErrorsList.push(item);
                            });
                        }

                        else {
                            var newUserPM: UserPM = myResult.Result;
                            this.CurrentSession.CloseCurrentWindowEmit(newUserPM.Id);

                            this.OpenLicenseManagementScreen();
                        }
                    }
                }, error => {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    var dd: any = error;
                    console.log(dd.text);
                });
            }
        }
    }

    private OpenLicenseManagementScreen() {
        if (SessionLocator.TenantManagementJS.IsMultiPackage && FeatureLocator.HasFeaturePermession("User", "User.Feature.LicensesManagment")) {
            var service: PackageListService = new PackageListService();
            service.getAllFromCache().subscribe((result: any) => {
                var allPackages: PackageList[] = result.Result;

                var userExtendedPMService: UserExtendedPMService = new UserExtendedPMService();
                userExtendedPMService.GetUserLicenses().subscribe((myResult: any) => {
                    if (myResult) {
                        var myResponse: ServiceResponse = myResult;
                        if (!myResponse.HasError) {
                            var allUserLicenses: UserLicensePM[] = myResponse.Result;

                            userExtendedPMService.GetUsersWorkspaceSummary(SessionInfo.LoggedUserTenant).subscribe((res: any) => {
                                var pmResponse: ServiceResponse = res;
                                if (!pmResponse.HasError) {
                                    var myResult = pmResponse.Result;
                                    if (myResult) {
                                        var args: UserLicenseArgs = new UserLicenseArgs();
                                        args.AllPackages = allPackages;
                                        args.AllUserLicenses = allUserLicenses;
                                        args.ActiveNotAdditionalUsersCount = myResult.ActiveNotAdditionalUsersCount;
                                        args.SearchField = this.NewUserPM.Email;

                                        var logitudeWindow = new LogitudeWindow();
                                        logitudeWindow.Width = 960;
                                        logitudeWindow.Height = 570;
                                        logitudeWindow.Title = "Licenses Management";
                                        logitudeWindow.WindowArgs = args;
                                        logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/LicensesManagementComponent');
                                        logitudeWindow.WindowClosed.subscribe(($event: any) => {
                                            this.CurrentSession.FireEvent("RefreshUserWorkspace");
                                        });
                                    }
                                }
                            });
                        }
                    }
                });
            });
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    EmailTextChangedMethod(s: string) {
        if (this.ValidationErrorsList != null) {
            var err = this.ValidationErrorsList.filter(e => e == TextCodeTranslator.Translate("User.M.ContactExistInCurrentTenant"))[0];
            if (err != null) {
                this.ValidationErrorsList = [];
            }
        }
    }

    EmailKeyUpMethod(email: string) {
        this.IsPasswordDisable = false;
        this.Info1Text = "";
        this.Info1Visibility = false;
        this.CanSave = true;
        this.NewUserPM.Password = this.ReTypePassword = "";
        if (!AppTool.IsNullOrEmpty(email)) {
            email = email.trim();

            if (email.indexOf('.') > 0 && email.indexOf('@') > 0) {
                this._passwordChangeService.CheckIfUserIsExists(email, SessionLocator.Tenant, false, false).subscribe((res:any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {

                            var result = myResult.split('@');

                            if (result[0] == "True") {
                                this.ValidationErrorsList = [];

                                this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.ContactExistInCurrentTenant"));

                                this.CanSave = false;
                                this.IsPasswordDisable = false;
                            }

                            else {
                                if (this.ValidationErrorsList) {
                                    var index = this.ValidationErrorsList.indexOf(TextCodeTranslator.Translate("User.M.ContactExistInCurrentTenant"));
                                    if (index != -1) this.ValidationErrorsList.splice(index, 1);
                                }

                                if (result[1] == "True") {
                                    this.IsPasswordDisable = true;
                                    this.NewUserPM.Password = this.ReTypePassword = "123";

                                    this.Info1Text = "This email already has a password";
                                    this.Info1Visibility = true;
                                }
                                else {
                                    this.IsPasswordDisable = false;
                                    this.Info1Text = "";
                                    this.Info1Visibility = false;
                                    this.CanSave = true;
                                }
                            }

                        }
                    }






                });

            }
            else {
                this.CanSave = true;
            }
        }
        else {
            this.CanSave = true;
        }


    }

    SetUiProperties_Visibility() {
        if (SessionLocator.TenantManagementJS.ManageLicencesPerUser) this.LicencedUserVisible = true;
        else this.LicencedUserVisible = false;

        if (ObjectsLocator.IsDemoTenant(SessionInfo.LoggedUserTenant.toString()) && SessionInfo.LoggedUserPM.IsCustomerCare) this.ExpirationDateVisible = true;
        else this.ExpirationDateVisible = false;

        if (SessionInfo.LoggedUserTenant != 0) {
            this.IsDistributorVisible = false;
            this.DistributorCodeVisible = false;
        }

        if (FeatureLocator.HasFeaturePermession("User", "ROLES")) this.IsSalesmanVisible = true;
        else this.IsSalesmanVisible = false;

        if (SessionLocator.TenantManagementJS.MainAdditionalPackageApplied && SessionLocator.TenantManagementJS.IsMultiPackage) {
            this.IsAdditionalPackagesOnlyVisible = true;
        }
    }

    SetUiProperties_IsEnabled() {
        this.NewUserPM.UIProperties.SetEnabled("Email", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("EnglishName", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("LocalName", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("PersonalId", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("ExpirationDate", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("IsSalesman", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("LicencedUser", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("IsDistributor", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("DistributorCode", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("DepartmentId", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("BranchId", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("BusinessUnitId", "User", this.IsScreenEnabled);
        this.NewUserPM.UIProperties.SetEnabled("Notes", "User", this.IsScreenEnabled);
    }

    EditRole(myRole: RolePM) {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.Title = "Edit " + myRole.Name + " Role";
        logWindow.WindowArgs = { RolePM: myRole };
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/EditRoleFeaturesComponent');
    }

    SelectedUserRolesItem: UserRolesItemClass;
    BuildObsList() {
        this.ObsList = new Array<UserRolesItemClass>();

        if (FeatureLocator.HasFeaturePermession("User", "ROLES")) {
            var isEditingAllowed = this.IsScreenEnabled;


            this.allRoles.forEach((item) => {

                switch (item.Code) {
                    case "DIST":
                    case "CUCA":
                        {
                            if (SessionInfo.LoggedUserTenant == 0) {
                                this.ObsList.push(new UserRolesItemClass(item, this.NewUserPM, this));
                            }

                            break;
                        }

                    case "HRAD":
                        {
                            if (SessionInfo.LoggedUserTenant == 0 || SessionInfo.LoggedUserTenant == 1489 || FeatureLocator.IsPackage_DVMT()) {
                                if (SessionLocator.LoggedUserPM.IsCustomerCare) {
                                    this.ObsList.push(new UserRolesItemClass(item, this.NewUserPM, this));
                                }
                            }

                            break;
                        }

                    default:
                        {
                            if (item.IsCustomRole) {
                                if (item.Tenant == SessionInfo.LoggedUserTenant) {
                                    this.ObsList.push(new UserRolesItemClass(item, this.NewUserPM, this));
                                }
                            }

                            else {
                                this.ObsList.push(new UserRolesItemClass(item, this.NewUserPM, this));
                            }

                            break;
                        }
                }
            });


        }

        else {
            var adminRole = this.allRoles.filter(d => d.Code == "ADMN")[0];
            if (adminRole) {
                var adminItem = new UserRolesItemClass(adminRole, this.NewUserPM, this);
                adminItem.IsActive = true;
                this.ObsList.push(adminItem);
            }



        }

    }
}
