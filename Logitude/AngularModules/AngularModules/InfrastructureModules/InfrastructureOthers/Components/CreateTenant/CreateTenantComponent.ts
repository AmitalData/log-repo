import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SignUpService} from '../../../../Infrastructure/Services/ExtendedPMs/SignUpService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {SignUpInfoClass} from '../../../../InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantPackageSelectionComponent';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import { CountryListService } from '../../../../Common/Services/StandardLists/CountryListService';

@Component({
    selector: 'CreateTenantComponent',
    templateUrl: './CreateTenantComponent.html',
})

export class CreateTenantComponent extends BaseComponent implements OnInit {
    DataContext: CreateTenantComponent = this;
    Email: string;
    ContactName: string;
    Phone: string;
    CompanyName: string;
    City: string;
    VatNumber: string;
    TimeZoneOffset: number;
    IsCreateLogboxTenantFromCloud: boolean;
    public ValidationErrorsList: string[];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    signUpService: SignUpService;
    public IsStardLoadPage: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.signUpService = new SignUpService();
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Package").subscribe(response => {
            this.FillDetails();
        });
    }

    private FillDetails() {
        if (!this.IsCreateLogboxTenantFromCloud) {
            this.IsStardLoadPage = true;
            return;
        }
        this.FillLogboxTenantDetails();
    }

    private FillLogboxTenantDetails() {
        if (!this.IsCreateLogboxTenantFromCloud) return;
        var countryListService: CountryListService = new CountryListService();
        countryListService.getAllFromCache().subscribe((result: any) => {
            this.SetLogboxCountryDetails(result);
            this.IsStardLoadPage = true;
        });
        let logboxPackageCode = "IMPO";
        this.PackageCode = logboxPackageCode;
        let telivivCity = "Tel Aviv";
        this.City = telivivCity;
        this.TimeZoneOffset = SessionLocator.TenantPM?.TimeZoneOffset;
    }

    private SetLogboxCountryDetails(result: any) {
        let israelCountryCode = "IL";
        let israelCountry: CountryList = result.Result.filter(d => d.Tenant == SessionLocator.Tenant && d.Code == israelCountryCode)[0];
        this.Country = israelCountry != null ? israelCountry : this.Country;
    }

    SetWindowArgs(args: any) {
        this.IsCreateLogboxTenantFromCloud = args.IsCreateLogboxTenantFromCloud;
    }

    CountryId: string;
    CountryCode: string;
    CountryName: string;

    private packageCode: string = (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") ? "IMPO" : "BUSN";
    get PackageCode() { return this.packageCode; }
    set PackageCode(newValue: string) {
        if (this.packageCode != newValue) {
            this.packageCode = newValue;
        }
    }

    private country: CountryList = null;
    get Country() { return this.country; }
    set Country(newValue: CountryList) {
        if (this.country != newValue) {
            this.country = newValue;
            this.OnCountryChanged(newValue);
        }
    }

    private OnCountryChanged(list: CountryList) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryName = null;
            this.CountryId = null;
        }

        else {
            this.CountryCode = list.Code;
            this.CountryName = list.EnglishName;
            this.CountryId = list.Id;
        }

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];
        this.ValidationFields();

       if( this.ValidationErrorsList.length == 0) this.Buildtenant();
    }

    ValidationFields() {
        if (AppTool.IsNullOrEmpty(this.Email)) {
            this.ValidationErrorsList.push("Email field is required");
        } else if (!this.CheckIsValidEmail(this.Email)) {
            this.ValidationErrorsList.push("Invalid Email address");
        }

        if (AppTool.IsNullOrEmpty(this.ContactName)) {
            this.ValidationErrorsList.push("Name field is required");
        } else if (this.ContactName.length > 60) {
            this.ValidationErrorsList.push("Contact Name field max length is 60");
        }

        if (AppTool.IsNullOrEmpty(this.CompanyName)) {
            this.ValidationErrorsList.push("Company field is required");
        } else if (this.CompanyName.length > 60) {
            this.ValidationErrorsList.push("Company field max length is 60");
        }


        if (AppTool.IsNullOrEmpty(this.Phone)) {
            this.ValidationErrorsList.push("Phone field is required");
        }

        if (AppTool.IsNullOrEmpty(this.PackageCode)) {
            this.ValidationErrorsList.push("Package Code field is required");
        }

        if (this.IsCreateLogboxTenantFromCloud && AppTool.IsNullOrEmpty(this.VatNumber)) {
            this.ValidationErrorsList.push("Vat Number field is required");
        }

        if (this.IsCreateLogboxTenantFromCloud && AppTool.IsNullOrEmpty(this.CountryCode)) {
            this.ValidationErrorsList.push("Country field is required");
        }
    }

    CheckIsValidEmail(email: string) {

        var IsOk = true;

        var EMAIL_REGEXP1 = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        var EMAIL_REGEXP2 = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (email) {
            if (email) {
                if (!EMAIL_REGEXP1.test(email)) {
                    IsOk = false;
                    return;
                } else if (!EMAIL_REGEXP2.test(email)) {
                    IsOk = false;
                    return;
                }
            }
        }
        return IsOk;
    }

    Buildtenant() {
        this.CurrentSession.StartBusyIndicatorSaving();
        
        var SignUpInfo: SignUpInfoClass = new SignUpInfoClass();
        SignUpInfo.Email = this.Email;
        SignUpInfo.Phone = this.Phone;
        SignUpInfo.Company = this.CompanyName;
        SignUpInfo.Name = this.ContactName;
        SignUpInfo.PackageCode = this.packageCode;
        SignUpInfo.CountryName = this.CountryName;
        SignUpInfo.CountryCode = this.CountryCode;
        SignUpInfo.Tenant = SessionLocator.Tenant;
        SignUpInfo.VatNumber = this.VatNumber;
        SignUpInfo.TimeZoneOffset = this.TimeZoneOffset;
        SignUpInfo.City = this.City;
        SignUpInfo.IsCreateLogboxTenantFromCloud = this.IsCreateLogboxTenantFromCloud;
      
        this.signUpService.CreateTenant(SignUpInfo).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                if (myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Title = "Logitude Message";
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }
            }
        });
    }
}
