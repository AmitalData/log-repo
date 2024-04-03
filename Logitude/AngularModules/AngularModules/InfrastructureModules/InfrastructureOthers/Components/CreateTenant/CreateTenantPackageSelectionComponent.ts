import {Component, OnInit}  from '@angular/core';
declare var System: any;
declare var window: any;
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SignUpService} from '../../../../Infrastructure/Services/ExtendedPMs/SignUpService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    

    selector: 'CreateTenantPackageSelectionComponent',
    templateUrl: './CreateTenantPackageSelectionComponent.html',


})


export class CreateTenantPackageSelectionComponent extends BaseComponent implements OnInit {
    DataContext: CreateTenantPackageSelectionComponent = this;
    CustomerId: string;
    Email: string;
    ContactName: string;
    Phone: string;
    CustomerName: string;
    CountryName: string;
    CountryCode: string;
    ObjecttableName: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    signUpService: SignUpService;

    public IsStardLoadPage: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.signUpService = new SignUpService();
  
    }

    ngOnInit(


    ) {
        this._entityResourceService.getEntityResourceByTableName("Package").subscribe((response:any) => {
            this.IsStardLoadPage = true;
      
        });
    }


    SetWindowArgs(args: any) {
        this.CustomerId = args.CustomerId;
        this.Email = args.Email;
        this.ContactName = args.ContactName;
        this.Phone = args.Phone;
        this.CustomerName = args.CustomerName;
        this.CountryName = args.CountryName;
        this.CountryCode = args.CountryCode;
        this.ObjecttableName = args.ObjecttableName;
        

    }


    private packageCode: string = (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") ? "IMPO" : "BUSN";
    get PackageCode() { return this.packageCode; }
    set PackageCode(newValue: string) {
        if (this.packageCode != newValue) {
            this.packageCode = newValue;
        }
    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



    SaveButtonClicked() {
       this.Buildtenant();

   }

    Buildtenant() {

        this.CurrentSession.StartBusyIndicatorSaving();

        var SignUpInfo: SignUpInfoClass = new SignUpInfoClass();
        SignUpInfo.Email = this.Email;
        SignUpInfo.Phone = this.Phone; 
        SignUpInfo.Company = this.CustomerName; 
        SignUpInfo.Name = this.ContactName; 
        SignUpInfo.IsCrmTenant = true;
        SignUpInfo.CustomerId = this.CustomerId;
        SignUpInfo.Tenant = SessionLocator.Tenant;
        SignUpInfo.PackageCode = this.packageCode;
        SignUpInfo.CountryName = this.CountryName;
        SignUpInfo.CountryCode = this.CountryCode;
        SignUpInfo.ObjecttableName = this.ObjecttableName;
        this.signUpService.SendMessageToQueue(SignUpInfo).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
              
                this.CurrentSession.CurrentEditComponent.SaveChanges();
                this.CancelButtonClicked();
           }
           else this.CurrentSession.StopBusyIndicator();
       });

   }


}

export class SignUpInfoClass {
    public Email: string;
    public Company: string; 
    public Phone: string;
    public Name: string;
    public CustomerId: string;
    public IsCrmTenant: boolean;
    public Tenant: number;
    public PackageCode: string;
    public CountryName: string;
    public CountryCode: string;
    public ObjecttableName: string;
    public VatNumber: string;
    public TimeZoneOffset: number;
    public City: string;
    public IsCreateLogboxTenantFromCloud: boolean;
    public AdditionalEmail: string;
    
}
