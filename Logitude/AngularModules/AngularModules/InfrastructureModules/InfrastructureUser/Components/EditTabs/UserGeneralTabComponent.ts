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
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { FullAccountingSettingListService } from 'Accounting/Services/StandardLists/FullAccountingSettingListService';

@Component({
    
    templateUrl: './UserGeneralTabComponent.html',
    providers: [TenantLoginPolicyListService],
})

export class UserGeneralTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: UserPM;
    public ObjectTableName: string = "User";
    public DataContext = this;
    public TechnologyList: CodeNameClass[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public SignatureImageId: string;
    public EntityId: string; 
    private fullAccountingSettingListService: FullAccountingSettingListService;

    constructor(public entityArgs: EntityArgs, public TenantLoginPolicyListService: TenantLoginPolicyListService) {
        super();
        this.fullAccountingSettingListService = new FullAccountingSettingListService();
        this.EntityPM = entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.BuildTechnologyList();
        this.SetUIProperties(); 
        this.Listen();
        this.CheckSecurityPolicySettingToShowPhone();

        this.BuildLayoutDirectionList();
        this.SetSelectedDirection();

        this.InitializeImageIds();
        this.getAccountingSettingSecurityLevelField();
       // this.SelectedDirection = this.EntityPM.LayoutDirection == 'ltr' ? this.LayoutDirections[0] : this.LayoutDirections[1];
    }

    private InitializeImageIds() {
        this.SignatureImageId = this.EntityPM.SignatureImageId;
    }

    SignatureUploadedCompleted(imageId) {
        this.SignatureImageId = imageId;
        this.EntityPM.SignatureImageId = imageId;

    }

    RemoveImage(name) {

        switch (name) {
            case "SignatureImageId": {
                this.EntityPM.SignatureImageId = null;
                this.SignatureImageId = null;
                break;
            }
            default: {
                //statements; 
                break;
            }
        }
    } 

    private SetSelectedDirection() {
        if (this.EntityPM.LayoutDirection == 'ltr') {
            this.SelectedDirection = this.LayoutDirections[0];
        }
        else if (this.EntityPM.LayoutDirection == 'rtl') {
            this.SelectedDirection = this.LayoutDirections[1];
        }
        else this.SelectedDirection = this.LayoutDirections[2];
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
    public LayoutDirections: CodeNameClass[];
   

    private selectedDirection: CodeNameClass;
    get SelectedDirection() { return this.selectedDirection; }
    set SelectedDirection(value: CodeNameClass) {
        if (this.selectedDirection != value) {
            this.selectedDirection = value;
            if (value.Code == "3") {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.LayoutDirection)) {
                    this.EntityPM.LayoutDirection = null;
                }
            } else if ( this.EntityPM.LayoutDirection != value.Name.toLowerCase()) {
                this.EntityPM.LayoutDirection = value.Name.toLowerCase();
            }

        }
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


        this.TenantLoginPolicyListService.getSingle(SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var result: TenantLoginPolicyList = pmResponse.Result;
                if (result.IsEnabledForSpecificUsers && result.LoginPolicyCode!="DISABLED" ) {
                    this.IsShowFactorAuthenticationEnabled = true;
                }
            
            }
        });
    }

    getAccountingSettingSecurityLevelField() {
        this.fullAccountingSettingListService.getSingle(SessionLocator.Tenant.toString()).subscribe((response: any) => {
            this.CurrentSession.StopBusyIndicator();         
            if (response != null) {
                var response = response.Result;
                if (response.IsSecurityLevelActivated) {
                        this.IsSecurityLevelVisibile = true;
                    }
                    else {
                        this.IsSecurityLevelVisibile= false;
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
    public IsAdditionalPackagesOnlyVisible: boolean = false;
    public IsLayoutDirectionVisibile: boolean = false;
    public IsDontShowLocalLabelsVisibile: boolean = false;
    public IsSecurityLevelVisibile: boolean = false;

    SetUIProperties() {
        if (FeatureLocator.HasFeaturePermession("User", "PERSONALID")) {
            this.IsPersonalIdVisible = true;
        }
        if (FeatureLocator.HasFeaturePermession("User", "LYDR")) {
            this.IsLayoutDirectionVisibile = true;
        }
        if (FeatureLocator.HasFeaturePermession("User", "DontShowLocalLabels")) {
            this.IsDontShowLocalLabelsVisibile = true;
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

        if (SessionLocator.TenantManagementJS.MainAdditionalPackageApplied && SessionLocator.TenantManagementJS.IsMultiPackage) {
            this.IsAdditionalPackagesOnlyVisible = true;
        }

        var isEditingEnabled = true;
        if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString())) {
            if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = false;
            }
        }

        this.IsEditingEnabled = isEditingEnabled;
        this.UIProperties.SetEnabled("Email", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("EnglishName", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("LocalName", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("PersonalId", this.ObjectTableName, false);
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
        this.UIProperties.SetEnabled("DontShowLocalLabels", this.ObjectTableName, isEditingEnabled);

    }
   
    BuildLayoutDirectionList() {

        this.LayoutDirections = [];
        this.LayoutDirections.push(new CodeNameClass("1", "LTR"));
        this.LayoutDirections.push(new CodeNameClass("2", "RTL"));
        this.LayoutDirections.push(new CodeNameClass("3", "Not set"));
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

            if (SessionLocator.TenantManagementJS.IsMultiPackage || SessionLocator.TenantManagementJS.MainAdditionalPackageApplied) {
                var service: UserExtendedListService = new UserExtendedListService();
                service.GetUserLicensesCountForUser(this.EntityPM.Id).subscribe((myResult:any) => {
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

    public get DontShowLocalLabels () { return this.EntityPM.DontShowLocalLabels; }
    public set DontShowLocalLabels (value: boolean) {
        if (this.EntityPM.DontShowLocalLabels  != value) {
            this.EntityPM.DontShowLocalLabels  = value;
        }
    }

    public get AdditionalPackagesOnly() { return this.EntityPM.AdditionalPackagesOnly; }
    public set AdditionalPackagesOnly(value: boolean) {
        if (this.EntityPM.AdditionalPackagesOnly != value) {
            this.EntityPM.AdditionalPackagesOnly = value;
        }
    }

    public get SecurityLevel() { return this.EntityPM.SecurityLevel; }
    public set SecurityLevel(value: number) {
        if (this.EntityPM.SecurityLevel != value) {
            this.EntityPM.SecurityLevel = value;
        }
    }
}
