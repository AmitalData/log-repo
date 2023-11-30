declare var window: any;
import {Component} from '@angular/core';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ImportEntityArgs} from '../../../../Common/Components/Maintenance/TenantImportComponent';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {CommonDomainService, HelpResource} from'../../../../Common/Services/CommonDomainService'; 
import {GlobalDomainService} from'../../../../Common/Services/GlobalDomainService'; 
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse'; 
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {ContactPMService} from '../../../../Common/Services/StandardPMs/ContactPMService';
import {ContactPM} from '../../../../Common/EntityPMs/ContactPM';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import { UserArgs} from '../../../../Infrastructure/Args';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './GettingStartedComponent.html',
})

export class GettingStartedComponent extends BaseComponent {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public DataContext: GettingStartedComponent = this;
    public VideosObslist: HelpResourceArgs[] = [];
    public HowToObslist: HelpResourceArgs[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public ReleaseDateString: string;
    constructor() {
        super();

        this.ReleaseDateString = ObjectsLocator.GlobalSetting.ReleaseDateString;

        this.LoadData();
        this.CheckFeatures1();
        this.CheckFeatures2();
    }

    //Help Resources
    public HasVedios: boolean = false;
    public HasHowTos: boolean = false;
    LoadData() {
        this.LoadHelpResources();
        this.LoadGetStartedData();
    }
    LoadHelpResources() {
        var service = new GlobalDomainService();
        service.GetAllHelpResources().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var allItems: HelpResource[] = response.Result;
                var videoList: HelpResource[] = allItems.filter(d => d.Type == "VID");
                var nonVideoList: HelpResource[] = allItems.filter(d => d.Type != "VID");

                this.BuildVideoist(videoList);
                this.BuildHowToList(nonVideoList);
            }
        });
    }
    BuildVideoist(myList: HelpResource[]) {

        this.VideosObslist = [];

        myList.sort((a, b) => { return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate > b.UpdateDate) ? -1 : 1 }).forEach(item => {
            if (FeatureLocator.HasFeaturePermession("HelpResource", item.FeatureCode)) {
                this.VideosObslist.push(new HelpResourceArgs(item));
            }
        });

        this.HasVedios = this.VideosObslist.length > 0 ? true : false;
    }
    BuildHowToList(myList: HelpResource[]) {
        this.HowToObslist = [];

        var releaseNotes: HelpResource[] = myList.filter(d => d.Type == "REL");
        var nonReleaseNotes: HelpResource[] = myList.filter(d => d.Type != "REL");

        if (releaseNotes.length > 0) {
            var releaseNote: HelpResource = releaseNotes.filter(d => d.Type == "REL").sort((a, b) => {
                return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate > b.UpdateDate) ? -1 : 1
            })[0];

            this.HowToObslist.push(new HelpResourceArgs(releaseNote));
        }

        nonReleaseNotes.sort((a, b) => { return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate > b.UpdateDate) ? -1 : 1 }).forEach(item => {
            if (this.HowToObslist.length < 9) {
                this.HowToObslist.push(new HelpResourceArgs(item));
            }
        });

        this.HasHowTos = this.HowToObslist.length > 0 ? true : false;
    }

    // CheckFeatures 1
    public CustomerInfoVisibility: boolean = false;
    public AgentInfoVisibility: boolean = false;
    public UserInfoVisibility: boolean = false;
    public PortInfoVisibility: boolean = false;
    public AirlineInfoVisibility: boolean = false;
    public ShippingLineInfoVisibility: boolean = false;
    public AddCustomerVisibility: boolean = false;
    public AddAgentVisibility: boolean = false;
    public AddUserVisibility: boolean = false;
    public AddPortVisibility: boolean = false;
    public AddAirlineVisibility: boolean = false;
    public AddShippingLineVisibility: boolean = false;
    CheckFeatures1() {
        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERS")) {
            this.CustomerInfoVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "AGENTS")) {
            this.AgentInfoVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "USERS")) {
            this.UserInfoVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "PORTS")) {
            this.PortInfoVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "AIRLINES")) {
            this.AirlineInfoVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "SHIPPINGLINES")) {
            this.ShippingLineInfoVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERS")) {
            if (FeatureLocator.HasFeaturePermession("Customer", "NEW") && FeatureLocator.HasFeaturePermession("Customer", "NEWCUSTOMER")) {
                this.AddCustomerVisibility = true;
            }
        }
        if (FeatureLocator.HasFeaturePermession("General", "AGENTS")) {
            if (FeatureLocator.HasFeaturePermession("Agent", "NEW") && FeatureLocator.HasFeaturePermession("Agent", "NEWAGENT")) {
                this.AddAgentVisibility = true;
            }
        }
        if (FeatureLocator.HasFeaturePermession("General", "USERS")) {
            if (FeatureLocator.HasFeaturePermession("User", "NEW") && FeatureLocator.HasFeaturePermession("User", "NEWUSER")) {
                this.AddUserVisibility = true;
            }
        }
        if (FeatureLocator.HasFeaturePermession("General", "PORTS")) {
            if (FeatureLocator.HasFeaturePermession("Port", "NEW") && FeatureLocator.HasFeaturePermession("Port", "NEWPORT")) {
                this.AddPortVisibility = true;
            }
        }
        if (FeatureLocator.HasFeaturePermession("General", "AIRLINES")) {
            if (FeatureLocator.HasFeaturePermession("Airline", "NEW") && FeatureLocator.HasFeaturePermession("Airline", "NEWAIRLINE")) {
                this.AddAirlineVisibility = true;
            }
        }
        if (FeatureLocator.HasFeaturePermession("General", "SHIPPINGLINES")) {
            if (FeatureLocator.HasFeaturePermession("ShippingLine", "NEW") && FeatureLocator.HasFeaturePermession("ShippingLine", "NEWSHIPPINGLINE")) {
                this.AddShippingLineVisibility = true;
            }
        }
    }

    public SystemSettingsVisibility: boolean = false;
    public CompanyAddressVisibility: boolean = false;
    public SystemDefaultsVisibility: boolean = false;
    public CountersVisibility: boolean = false;
    public CompanyLogoVisibility: boolean = false;
    public SystemCurrenciesVisibility: boolean = false;
    public AccountingSettingsVisibility: boolean = false;
    public LocalSettingsVisibility: boolean = false;
    public InvoiceSettingsVisibility: boolean = false;
    public AirlineSettingsVisibility: boolean = false;
    public PersonalSettingsVisibility: boolean = false;
    public SignatureVisibility: boolean = false;
    public ChangePasswordVisibility: boolean = false;
    CheckFeatures2() {
        if (FeatureLocator.HasFeaturePermession("General", "SYSTEMSETTINGS")) {
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyAddress")) {
                this.SystemSettingsVisibility = true;
            }

            else if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemDefaults")) {
                this.SystemSettingsVisibility = true;
            }

            else if (FeatureLocator.HasFeaturePermession("General", "COUNTERS")) {
                this.SystemSettingsVisibility = true;
            }

            else if (FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyLogo")) {
                this.SystemSettingsVisibility = true;
            }

            else if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
                this.SystemSettingsVisibility = true;
            }

            else if (FeatureLocator.HasFeaturePermession("General", "HOWTOACCOUNTINGSETTINGS")) {
                this.SystemSettingsVisibility = true;
            }

            else if (FeatureLocator.HasFeaturePermession("General", "General.Features.LocalSettings")) {
                this.SystemSettingsVisibility = true;
            }

            else if (FeatureLocator.HasFeaturePermession("General", "General.Features.InvoiceSettings")) {
                this.SystemSettingsVisibility = true;
            }
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyAddress")) {
            this.CompanyAddressVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemDefaults")) {
            this.SystemDefaultsVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "COUNTERS")) {
            this.CountersVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyLogo")) {
            this.CompanyLogoVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.SystemCurrenciesVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "HOWTOACCOUNTINGSETTINGS")) {
            this.AccountingSettingsVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.LocalSettings")) {
            this.LocalSettingsVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.InvoiceSettings")) {
            this.InvoiceSettingsVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.AirlineSettings")) {
            this.AirlineSettingsVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "PERSONALSETTINGS")) {
            if (FeatureLocator.HasFeaturePermession("General", "General.Features.Signature")) {
                this.PersonalSettingsVisibility = true;
            }

            else if (FeatureLocator.HasFeaturePermession("General", "General.Features.ChangePassword")) {
                this.PersonalSettingsVisibility = true;
            }
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.Signature")) {
            this.SignatureVisibility = true;
        }
        if (FeatureLocator.HasFeaturePermession("General", "General.Features.ChangePassword")) {
            this.ChangePasswordVisibility = true;
        }
    }

    // Data Management 
    public New(entity: string) {
        switch (entity) {
            case "User": { this.RunNewEntity(entity, "./InfrastructureModules/InfrastructureUser/Components/NewUserComponent"); break; }
            case "Agent": { this.RunNewEntity(entity, "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent"); break; }
            case "Customer": { this.RunNewEntity(entity, "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent"); break; }
            case "Port": { this.RunImportEntity(entity); break; }
            case "Airline": { this.RunImportEntity(entity); break; }
            case "ShippingLine": { this.RunImportEntity(entity); break; }
            default: { break; }
        }
    }
    private RunNewEntity(objectTableName: string, path: string) {
        this._entityResourceService.getEntityResourceByTableName(objectTableName).subscribe((response:any) => {
            var str = TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator.TranslateTable(objectTableName));
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = str;
            logWindow.Show(path);
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.LoadGetStartedData();
            });
        });
    }
    private RunImportEntity(objectTableName: string) {
        this._entityResourceService.getEntityResourceByTableName(objectTableName, 0).subscribe((response:any) => {
            var windowTitle = "Add " + objectTableName;
            var logWindow = new LogitudeWindow();
            var args: ImportEntityArgs = new ImportEntityArgs();
            var objectTable = window.ObjectTables.filter(x => x.Name === objectTableName)[0];
            if (objectTable) {
                args.ObjectTableId = objectTable.Id;
            }
            args.ObjectTableName = objectTableName;
            logWindow.WindowArgs = args;
            logWindow.Width = 1000;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.LoadGetStartedData();
            });
        });
    }
    ViewList(entity: string) {
        if (entity == "User") {
            this._entityResourceService.getEntityResourceByTableName("User", 0).subscribe((resp: any) => {
                SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureUser/Components/UserWorkspaceComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        var args = new UserArgs();
                        args.BackButtonText = TextCodeTranslator.Translate("General.MH.GettingStarted");
                        cmpRef.instance.Run(args);
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
        else {
            var objectTabelId = "";
            var objectTablePM = window.ObjectTables.filter(x => x.Name === entity)[0]; 
            if (objectTablePM != null) {
                objectTabelId = objectTablePM.Id;
            }
            if (!AppTool.IsNullOrEmpty(objectTabelId)) {
                this._entityResourceService.getEntityResourceByTableName(entity).subscribe((response:any) => {
                    this.ViewQuery(objectTabelId, entity);
                });
            }
        }
    }
    ViewQuery(objectTableId: string, objectTableName: string) {
        var filterAgrs = new ApiQueryFilters();
        var queryCode = "";
        switch (objectTableName.toLocaleLowerCase()) {
            case "customer":
                {
                    queryCode = "Customers";
                    break;
                }
            case "agent":
                {
                    queryCode = "Agents";
                    break;
                }
            case "user":
                {
                    queryCode = "AllUsers";
                    break;
                } 
            case "port":
                {
                    queryCode = "Ports";
                    break;
                }
            case "airline":
                {
                    queryCode = "Airlines";
                    break;
                }
            case "shippingline":
                {
                    queryCode = "Shipping lines";
                    break;
                }
        }
        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.BackButtonTitle = TextCodeTranslator.Translate("General.MH.GettingStarted");
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });
    }

    // LoadGetStartedData
    LoadGetStartedData() {
        var service = new CommonDomainService();
        service.GetGettingStartedData().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var result = response.Result;
                if (result != null) {
                    this.PortsCount = result.PortsCount;
                    this.AgentsCount = result.AgentsCount;
                    this.CustomersCount = result.CustomersCount;
                    this.UsersCount = result.UsersCount;
                    this.QuotesCount = result.QuotesCount;
                    this.ShipmentsCount = result.ShipmentsCount;
                    this.MastersCount = result.MastersCount;
                    this.AirlinesCount = result.AirlinesCount;
                    this.ShippinglinesCount = result.ShippinglinesCount;
                }
            }
        });
    }
    public PortsCount: number = 0;
    public AgentsCount: number = 0;
    public CustomersCount: number = 0;
    public UsersCount: number = 0;
    public QuotesCount: number = 0;
    public ShipmentsCount: number = 0;
    public MastersCount: number = 0;
    public AirlinesCount: number = 0;
    public ShippinglinesCount: number = 0;

    // System Info
    SystemInfoClicked() {
        this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(tenantResp => {
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Width = 800;
                logitudeWindow.Height = 600;
                logitudeWindow.DataContext = "SystemInfo";
                logitudeWindow.Title = "System Info";
                logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CustomizeLogitude/SystemInfoComponent');
            });
        });
    }
    CompanyAddressClick() {
        var windowTitle = "Company Address Settings";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/CompanyAddress/CompanyAddressSettingsComponent');
    }
    SystemDefaultsClick() {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
            var windowTitle = "System Defaults ";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 630;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemDefaults/SystemDefaultsComponent');
        });
    }
    CompanyCountersClick() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Counters";
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/CountersComponent');
    }
    CompanyLogoClick() {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 740;
        logitudeWindow.Height = 585;
        logitudeWindow.DataContext = this;
        logitudeWindow.Title = "Logo Definition";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/UploadImage/UploadLogoComponent');
    });
}
    SystemCurrenciesClick() {
        var windowTitle = "System Currencies";
        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemCurrencies/SystemCurrenciesComponent');
    }
    AccountingSettingsClick() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 820;
        logitudeWindow.Height = 570;
        logitudeWindow.Title = "Accounting Settings";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AccountingSettings/AccountingSettingsComponent');
    }
    LocalSettingsClick() {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
            var windowTitle = "Local Settings";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 750;
            logWindow.Height = 500;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/LocalSettings/LocalSettingsComponent');
        });
    }
    InvoiceSettingsClick() {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
            var windowTitle = "Invoice Settings";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 750;
            logWindow.Height = 500;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/InvoiceSettings/InvoiceSettingsComponent');
        });
    }
    AirlineSettingsClick() {
        this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe((response:any) => {
            var windowTitle = "Airline Settings";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 650;
            logWindow.Height = 350;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AirlineSettings/AirlineSettingsComponent');
        });
    }
    SignatureClick() {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe((response:any) => {
            var windowArgs: any = {};
            windowArgs.DataViewModel = this;
            windowArgs.PageType = "Signature";
            windowArgs.TemplateId = SessionLocator.LoggedUserId;
            windowArgs.Tenant = SessionLocator.Tenant;
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var logWindow = new LogitudeWindow();
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = "Edit Html Template";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
        });
    }
    ChangePasswordClick() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 600;
        logitudeWindow.Height = 400;
        logitudeWindow.Title = "Change User Password";
        this._entityResourceService.getEntityResourceByTableName("User").subscribe((response:any) => {
            logitudeWindow.DataContext = this;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/PersonalSettings/ChangePasswordComponent');
        });
    }

    // Check Box 
    private checkBoxIsEnabled = true;
    get CheckBoxIsEnabled() { return this.checkBoxIsEnabled; }
    set CheckBoxIsEnabled(value: boolean) {
        this.checkBoxIsEnabled = value;
    }
  
    get IsDisplayGetStartedChecked() {
        if (SessionLocator.LoggedUserPM != null) {
            return SessionLocator.LoggedUserPM.DisplayGettingStarted;
        }
        return null;
    }
    set IsDisplayGetStartedChecked(value: boolean) {
        if (SessionLocator.LoggedUserPM.DisplayGettingStarted != value) {
            SessionLocator.LoggedUserPM.DisplayGettingStarted = value;
            this.CheckBoxIsEnabled = false;
            this.SubmitOnCheckBox();
        }
    }

    SubmitOnCheckBox() {
        var service = new CommonDomainService();
        service.UpdateUserData(this.IsDisplayGetStartedChecked).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var result = response.Result;
                this.CheckBoxIsEnabled = true;
            }
        });
    }

    ViewAllResources() {
        ServiceLocator.SendTotangoUserActivity("Help Center", "View All");
        var url = ServiceHelper.GetLogitudeURL() + 'TrainingResourcesHTML/TrainingResourcesMainPage.aspx';
        var params: any[] = [{ name: "Token", value: SessionInfo.DocumentDownloadToken }]
        ServiceHelper.OpenWindowWithParams(url, params);

    }
}

export class HelpResourceArgs {
    private entity: HelpResource;
    constructor(help: HelpResource) {
        this.entity = help;
    }

    get VideoContent() { return this.entity.Name + " (" + this.entity.Duration + ")"; }
    get Uri() { return this.entity.VideoURL; }
    get HowToContent() { return this.entity.Name; }
    get Code() { return this.entity.Code; }
    get IsNew() { return this.entity.IsNew; }

    private HowToMethod() {
        if (this.entity.Type == "REL")
            ServiceLocator.SendTotangoUserActivity("How-To", "View Release Notes");

        ServiceLocator.SendTotangoUserActivity("Help Center", "How-To");
        var url = ServiceHelper.GetLogitudeURL() + 'WebPages/HowToDownloadPage.aspx?id=' + this.entity.Code;
        var params: any[] = [{ name: "Token", value: SessionInfo.DocumentDownloadToken }, { name: "Code", value: this.Code } ]
        ServiceHelper.OpenWindowWithParams(url, params);

    }
    private NafigateToURL() {
        window.open(this.Uri);
    }
}
