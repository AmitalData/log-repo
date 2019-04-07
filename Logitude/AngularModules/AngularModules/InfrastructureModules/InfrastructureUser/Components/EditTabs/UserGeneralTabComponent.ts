import {Component, OnDestroy}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UserPM} from '../../../../Common/EntityPMs/UserPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {TenantLoginPolicyListService} from '../../../../Common/Services/StandardLists/TenantLoginPolicyListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TenantLoginPolicyList} from '../../../../Common/EntityLists/TenantLoginPolicyList';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {UserExtendedListService} from '../../../../Common/Services/ExtendedLists/UserExtendedListService';

@Component({
    moduleId: module.id,
    templateUrl: './UserGeneralTabComponent.html',
    providers: [TenantLoginPolicyListService],
})

export class UserGeneralTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: UserPM;
    public ObjectTableName: string = "User";
    public DataContext = this;
    public TechnologyList: CodeNameClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public TenantLoginPolicyListService: TenantLoginPolicyListService) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.BuildTechnologyList();
        this.SetUIProperties();
        this.Listen();
        this.CheckSecurityPolicySettingToShowPhone();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetUIProperties();
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetUIProperties();
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public SelectedTechnology: CodeNameClass;
    BuildTechnologyList() {
        this.TechnologyList = [];

        this.TechnologyList.push(new CodeNameClass("AG", "Angular"));
        this.TechnologyList.push(new CodeNameClass("SL", "SilverLight"));
        this.TechnologyList.push(new CodeNameClass("DE", "Default"));
        this.TechnologyList.push(new CodeNameClass("PR", "Prompt"));

        if (this.Technology) {
            this.SelectedTechnology = this.TechnologyList.filter(d => d.Code == this.EntityPM.Technology)[0];
        }

        else {
            this.SelectedTechnology = this.TechnologyList[0];
        }
    }
    TechnologyValueChanged(item: CodeNameClass) {
        this.SelectedTechnology = item;

        if (item == null) {
            this.Technology = null;
        }

        else {
            this.Technology = item.Code;
        }
    }
  
    IsShowFactorAuthenticationEnabled: boolean = false;
    CheckSecurityPolicySettingToShowPhone() {


        this.TenantLoginPolicyListService.getSingle(SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var result: TenantLoginPolicyList = pmResponse.Result;
                if (result.IsEnabledForSpecificUsers && result.LoginPolicyCode!="DISABLED" ) {
                    this.IsShowFactorAuthenticationEnabled = true;
                }
            
            }
        });


    }

    public IsEditingEnabled: boolean = false;
    public IsPersonalIdVisible: boolean = false;
    public IsExpirationDateVisible: boolean = false;
    public IsSalesmanVisible: boolean = false;
    public IsLicencedUserVisible: boolean = false;
    public IsShowContactInMobileVisiable: boolean = false;
    SetUIProperties() {

        if (FeatureLocator.HasFeaturePermession("User", "PERSONALID")) {
            this.IsPersonalIdVisible = true;
        }

        if (SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsExpirationDateVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("User", "ROLES")) {
            this.IsSalesmanVisible = true;
        }

        if (SessionLocator.TenantManagementJS.ManageLicencesPerUser) {
            this.IsLicencedUserVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "MOBILE")) {
            this.IsShowContactInMobileVisiable = true;
        }

        var isEditingEnabled = true;
        if (SessionLocator.Tenant == 65) {
            if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = false;
            }
        }

        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("Email", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PersonalId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("DepartmentId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("BusinessUnitId", this.ObjectTableName, isEditingEnabled);

        this.UIProperties.SetEnabled("ExpirationDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Technology", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("IsSalesman", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("InActive", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("LicencedUser", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("IsShowContactDetailsInTheMobileApp", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("ShowLocalNameInLOV", this.ObjectTableName, isEditingEnabled);
    }

    public get Email() { return this.EntityPM.Email; }
    public set Email(value: string) {
        if (this.EntityPM.Email != value) {
            this.EntityPM.Email = value;
        }
    }

    public get EnglishName() { return this.EntityPM.EnglishName; }
    public set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    public get PersonalId() { return this.EntityPM.PersonalId; }
    public set PersonalId(value: string) {
        if (this.EntityPM.PersonalId != value) {
            this.EntityPM.PersonalId = value;
        }
    }

    public get DepartmentId() { return this.EntityPM.DepartmentId; }
    public set DepartmentId(value: string) {
        if (this.EntityPM.DepartmentId != value) {
            this.EntityPM.DepartmentId = value;
        }
    }

    public get BranchId() { return this.EntityPM.BranchId; }
    public set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;
        }
    }

    public get BusinessUnitId() { return this.EntityPM.BusinessUnitId; }
    public set BusinessUnitId(value: string) {
        if (this.EntityPM.BusinessUnitId != value) {
            this.EntityPM.BusinessUnitId = value;
        }
    }

    public get ExpirationDate() { return this.EntityPM.ExpirationDate; }
    public set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;
        }
    }

    public get Notes() { return this.EntityPM.Notes; }
    public set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    public get Technology() { return this.EntityPM.Technology; }
    public set Technology(value: string) {
        if (this.EntityPM.Technology != value) {
            this.EntityPM.Technology = value;
        }
    }

    public get IsSalesman() { return this.EntityPM.IsSalesman; }
    public set IsSalesman(value: boolean) {
        if (this.EntityPM.IsSalesman != value) {
            this.EntityPM.IsSalesman = value;
        }
    }

    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {

            var isDirty: boolean = this.EntityPM.IsDirty;
            this.EntityPM.InActive = value;

            if (SessionLocator.TenantManagementJS.IsMultiPackage) {
                var service: UserExtendedListService = new UserExtendedListService();
                service.GetUserLicensesCountForUser(this.EntityPM.Id).subscribe(myResult => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {
                        var count: number = myResponse.Result;

                        if (count > 0) {
                            if (value) {
                                var confirmWindow = new ConfirmWindow();
                                confirmWindow.Show("Setting this user as inactive will disconnect its licensese");

                                confirmWindow.WindowClosed.subscribe((event: any) => {
                                    if (confirmWindow.Yes) {

                                    }

                                    else if (confirmWindow.No) {
                                        this.InActive = false;

                                        if (!isDirty) {
                                            this.EntityPM.IsDirty = false;
                                        }
                                    }
                                });
                            }
                        }
                    }
                });
            }
        }
    }

    public get LicencedUser() { return this.EntityPM.LicencedUser; }
    public set LicencedUser(value: boolean) {
        if (this.EntityPM.LicencedUser != value) {
            this.EntityPM.LicencedUser = value;
        }
    }

    public get IsShowContactDetailsInTheMobileApp() { return this.EntityPM.IsShowContactDetailsInTheMobileApp; }
    public set IsShowContactDetailsInTheMobileApp(value: boolean) {
        if (this.EntityPM.IsShowContactDetailsInTheMobileApp != value) {
            this.EntityPM.IsShowContactDetailsInTheMobileApp = value;
        }
    }

    public get Mobile() { return this.EntityPM.Mobile; }
    public set Mobile(value: string) {
        if (this.EntityPM.Mobile != value) {
            this.EntityPM.Mobile = value;
        }
    }


    public get IsTwoFactorAuthenticationEnabled() { return this.EntityPM.IsTwoFactorAuthenticationEnabled; }
    public set IsTwoFactorAuthenticationEnabled(value: boolean) {
        if (this.EntityPM.IsTwoFactorAuthenticationEnabled != value) {
            this.EntityPM.IsTwoFactorAuthenticationEnabled = value;
        }
    }
    
    public get ShowLocalNameInLOV () { return this.EntityPM.ShowLocalNameInLOV; }
    public set ShowLocalNameInLOV (value: boolean) {
        if (this.EntityPM.ShowLocalNameInLOV  != value) {
            this.EntityPM.ShowLocalNameInLOV  = value;
        }
    }

    public get ShowNewReleaseToolTip() { return this.EntityPM.ShowNewReleaseToolTip; }
    public set ShowNewReleaseToolTip(value: boolean) {
        if (this.EntityPM.ShowNewReleaseToolTip != value) {
            this.EntityPM.ShowNewReleaseToolTip = value;
        }
    }
}
