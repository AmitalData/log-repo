declare var window: any;
import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagementPM} from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import {AirlineList} from '../../../../Common/EntityLists/AirlineList';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {GlobalDomainService} from '../../../../Common/Services/GlobalDomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
declare var UploadPortsFile, ArrayBufferToBase64: any;

@Component({
    
    selector: 'CCSSettingsTabComponent',
    templateUrl: './CCSSettingsTabComponent.html',
})

export class CCSSettingsTabComponent extends BaseComponent {
    public DataContext: CCSSettingsTabComponent = this;
    public ObjectTableName: string = "TenantManagement";
    public EntityPM: TenantManagementPM;
    public AllowedAirlinesItemsSource: AllowedAirlineItem[];
    public ItemsSource: TenantManagementAirlineItem[];
    public QuickSearchItems: AirlineList[] = [];
    public IsResourcesReady: boolean = false;
    private myService: PartnersDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.myService = new PartnersDomainService();

        if (this.EntityPM != null) {
            this.entityResourceService.getEntityResourceByTableName("Airline", 0).subscribe((response1: any) => {
                this.IsResourcesReady = true;

                this.SetUIProperties();
                this.LoadAirlines();
            });
        }
    }   
    
    public IsEditingAllowed: boolean = false;
    public AllowAirlinesIsEnabled: boolean = true;
    public RestrictedLabel: string = "";
    private isTenantManagementEditable: boolean;
    public PortsFileHtmlId: string = Guid.NewRandomString();

    SetUIProperties() {
        this.isTenantManagementEditable = this.IsTenantManagementEditable();
        this.RestrictedLabel = TextCodeTranslator.Translate("TenantManagement.F.IsRestrictedByAirline");

        var allowAirlinesEnabled = true;
        if (!this.isTenantManagementEditable) {
            allowAirlinesEnabled = false;
        }

        else if (!this.IsRestrictedByAirline) {
            allowAirlinesEnabled = false;
        }

        this.IsEditingAllowed = this.isTenantManagementEditable;
        this.AllowAirlinesIsEnabled = allowAirlinesEnabled;

        this.UIProperties.SetEnabled("TTY", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("PIMA", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("AWBMessagesCCSTypeCode", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("IsCargonautEnabled", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("IsDEXXConnectionEnabled", this.ObjectTableName, this.isTenantManagementEditable);
        this.UIProperties.SetEnabled("ScheduledTasksLimitPerReport", this.ObjectTableName, this.isTenantManagementEditable);

        this.UIProperties.SetVisibility("IsEAWBOnlyDemo", this.ObjectTableName, (SessionLocator.Tenant == 0 || SessionLocator.Tenant == 341) ? true : false);
        this.UIProperties.SetVisibility("IsINTTRAOnlyDemo", this.ObjectTableName, (SessionLocator.Tenant == 0) ? true : false);

        this.SetUIProperties_SetRequires();
    }

    private SetUIProperties_SetRequires() {
        //this.UIProperties.SetRequires("TTY", TargetEntityName, entityPM, false);
        this.UIProperties.SetRequired("PIMA", this.ObjectTableName, false);

        if (AppTool.IsNullOrEmpty(this.ScheduledTasksLimitPerReport)) {
            this.UIProperties.SetRequired("ScheduledTasksLimitPerReport", this.ObjectTableName, true);
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push("Scheduled Tasks Limit Per Report is required");;
        } else {
            this.UIProperties.SetRequired("ScheduledTasksLimitPerReport", this.ObjectTableName, false);
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = []
        };
         


        if (this.AWBMessagesCCSTypeCode == "CHAMP") {
            //if (string.IsNullOrEmpty(TTY))
            //{
            //    this.UIProperties.SetRequires("TTY", TargetEntityName, entityPM, true);
            //}
        }

        else if (this.AWBMessagesCCSTypeCode == "GLSHK") {
            if (AppTool.IsNullOrEmpty(this.PIMA)) {
                this.UIProperties.SetRequired("PIMA", this.ObjectTableName, true);
            }
        }
    }

    private IsTenantManagementEditable() {
        var myResult = false;

        if (FeatureLocator.HasFeaturePermession("TenantManagement", "EnableTenantManagementEdit")) {
            myResult = true;
        }

        return myResult;
    }

    get AWBMessagesCCSTypeCode() { return this.EntityPM.AWBMessagesCCSTypeCode; }
    set AWBMessagesCCSTypeCode(newValue: string) {
        if (this.EntityPM.AWBMessagesCCSTypeCode != newValue) {
            this.EntityPM.AWBMessagesCCSTypeCode = newValue;

            this.SetUIProperties();
            this.LoadAirlines();
        }
    }

    get TTY() { return this.EntityPM.TTY; }
    set TTY(newValue: string) {
        if (this.EntityPM.TTY != newValue) {
            this.EntityPM.TTY = newValue;

            this.SetUIProperties_SetRequires();
        }
    }

    get PIMA() { return this.EntityPM.PIMA; }
    set PIMA(newValue: string) {
        if (this.EntityPM.PIMA != newValue) {
            this.EntityPM.PIMA = newValue;

            this.SetUIProperties_SetRequires();
        }
    }

    get ScheduledTasksLimitPerReport() { return this.EntityPM.ScheduledTasksLimitPerReport; }
    set ScheduledTasksLimitPerReport(newValue) {
        if (this.EntityPM.ScheduledTasksLimitPerReport != newValue) {
            this.EntityPM.ScheduledTasksLimitPerReport = newValue;

            this.SetUIProperties_SetRequires();
        }
    }

    get IsEAWBOnlyDemo() { return this.EntityPM.IsEAWBOnlyDemo; }
    set IsEAWBOnlyDemo(newValue: boolean) {
        if (this.EntityPM.IsEAWBOnlyDemo != newValue) {
            this.EntityPM.IsEAWBOnlyDemo = newValue;
        }
    }

    get IsINTTRAOnlyDemo() { return this.EntityPM.IsINTTRAOnlyDemo; }
    set IsINTTRAOnlyDemo(newValue: boolean) {
        if (this.EntityPM.IsINTTRAOnlyDemo != newValue) {
            this.EntityPM.IsINTTRAOnlyDemo = newValue;
        }
    }

    get IsCargonautEnabled() { return this.EntityPM.IsCargonautEnabled; }
    set IsCargonautEnabled(newValue: boolean) {
        if (this.EntityPM.IsCargonautEnabled != newValue) {
            this.EntityPM.IsCargonautEnabled = newValue;
        }
    }

    get IsDEXXConnectionEnabled() { return this.EntityPM.IsDEXXConnectionEnabled; }
    set IsDEXXConnectionEnabled(newValue: boolean) {
        if (this.EntityPM.IsDEXXConnectionEnabled != newValue) {
            this.EntityPM.IsDEXXConnectionEnabled = newValue;
        }
    }

    get IsRestrictedByAirline() { return this.EntityPM.IsRestrictedByAirline; }
    set IsRestrictedByAirline(newValue: boolean) {
        if (this.EntityPM.IsRestrictedByAirline != newValue) {
            this.EntityPM.IsRestrictedByAirline = newValue;

            this.SetUIProperties();
        }
    }


    QuickSearchItemClicked(entity: any) {
        //RefreshScreenEvent myEvent = SessionLocator.CurrentAssemblyLocator.EventAggregator.GetEvent<RefreshScreenEvent>();
        //myEvent.Publish(new RefreshScreenEventArgs("AirlinesPopupClose"));

        this.AllowedAirline(entity, true);
    }
    
    LoadPortsClicked() {
        document.getElementById(this.PortsFileHtmlId).click();
    }

    UploadFile(event: any) {
        //var file: any = UploadPortsFile(this.PortsFileHtmlId);
        //if (file && file.name && file.name.toLowerCase().indexOf("csv") != -1) {
        //    this.ArrayBufferToBase64(file, this);
        //}
    }

    ArrayBufferToBase64(file: any, viewmode: any) {
        if (file) {
            var reader: FileReader = new FileReader();

            var reader = new FileReader();
            reader.onload = function (e) {
                //this.text = reader.result;
                var binary = '';
                var result = ArrayBufferToBase64(e);
                var bytes = new Uint8Array(result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }

                viewmode.ImportPorts(window.btoa(binary));
            };

            reader.onerror = function (e) {
                console.log(e);
            };

            reader.readAsArrayBuffer(file);
        }
    }

    ImportPorts(data: any) {
        

    }

    private LoadAirlines() {
        this.ItemsSource = [];

        this.CurrentSession.StartBusyIndicator("Loading Airlines...");
        this.LoadZeroTenantAirlines();
    }

    private tenantZeroSearchText: string = null;
    get TenantZeroSearchText() { return this.tenantZeroSearchText; }
    set TenantZeroSearchText(newValue: string) {
        if (this.tenantZeroSearchText != newValue) {
            this.tenantZeroSearchText = newValue;

            this.LoadZeroTenantAirlines();
        }
    }
    
    private myZeroTenantAirlines: AirlineList[];
    private LoadZeroTenantAirlines() {
        this.myZeroTenantAirlines = [];

        var filters: ApiQueryFilters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 500;

        filters.addAdditionalFilter("TTYPIMA", this.AWBMessagesCCSTypeCode, null, null, "Equals", true, false, false, "string");

        if (!AppTool.IsNullOrEmpty(this.TenantZeroSearchText)) {
            filters.addAdditionalFilter("SearchFields", this.TenantZeroSearchText, null, null, "Contains", true, true, false, "string");
        }
        
        this.myService.GetAirlinesByFiltersAndTenant(filters, 0).subscribe((myResponse: ServiceResponse) => {
               if (!myResponse.HasError) {
                this.myZeroTenantAirlines = myResponse.Result;

                if (this.myZeroTenantAirlines.length > 0) {
                    this.LoadCurrentTenantAirlines();
                }

                else {
                    this.BuildItemsSource();
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

    private myCurrentTenantAirlines: AirlineList[];
    public LoadCurrentTenantAirlines() {
        this.myCurrentTenantAirlines = [];

        this.myService.GetAirlinesForRequestedTenant(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.myCurrentTenantAirlines = myResponse.Result;

                this.BuildItemsSource();
                this.BuildAllowedAirlinesItemsSource();
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    
    private BuildItemsSource() {
        this.ItemsSource = [];

        var list: TenantManagementAirlineItem[] = [];

        this.myZeroTenantAirlines.forEach(tenantZeroItem => {
            var myTenantItem: AirlineList = this.myCurrentTenantAirlines.filter(d => d.Code.toLowerCase() == tenantZeroItem.Code.toLowerCase())[0];
            list.push(new TenantManagementAirlineItem(myTenantItem, tenantZeroItem, this.EntityPM, this));
        });

        list.sort((a, b) => { return (a.IsRegistered === b.IsRegistered) ? 0 : (a.IsRegistered > b.IsRegistered) ? -1 : 1 }).forEach(item => {
            this.ItemsSource.push(item);
        });
    }

    private BuildAllowedAirlinesItemsSource() {
        this.AllowedAirlinesItemsSource = [];

        var list: AirlineList[] = this.myCurrentTenantAirlines.filter(d => d.IsAllowedInAirlinesRestriction);

        list.forEach(item => {
            this.AllowedAirlinesItemsSource.push(new AllowedAirlineItem(item, this));
        });
    }

    DeleteAllowedAirline(item: AllowedAirlineItem) {
        this.AllowedAirline(item.myAirline, false);
    }

    public AllowedAirline(item: AirlineList, isAllowed: boolean) {
        if (isAllowed) {
            if (!item.IsAllowedInAirlinesRestriction) {
                this.AllowedAirlinesItemsSource.push(new AllowedAirlineItem(item, this));
            }
        }

        else {
            var itemViewModel = this.AllowedAirlinesItemsSource.filter(d => d.Id == item.Id)[0];
            if (itemViewModel != null) {
                var index = this.AllowedAirlinesItemsSource.indexOf(itemViewModel);
                if (index > -1) {
                    this.AllowedAirlinesItemsSource.splice(index, 1);
                }
            }
        }

        this.myService.AllowAirline(isAllowed, item.Code, this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                
            }            
        });
    }
}

export class AllowedAirlineItem {
    public myAirline: AirlineList;
    constructor(entityList: AirlineList, public fatherComponent: CCSSettingsTabComponent) {
        this.myAirline = entityList;
    }

    get Id() { return this.myAirline.Id; }
    get Code() { return this.myAirline.Code; }
    get EnglishName() { return this.myAirline.EnglishName; }
    get Prefix() { return this.myAirline.Prefix; }

}

export class TenantManagementAirlineItem extends BaseComponent {
    private currenctAirline: AirlineList;
    private zeroAirline: AirlineList;
    private entityPM: TenantManagementPM;
    private isGLSHK: boolean;
    private tenantAirlineId: string = null;
    private partnersService: PartnersDomainService;
    public DataContext: TenantManagementAirlineItem = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(myTenantItem: AirlineList, tenantZeroItem: AirlineList, entityPM: TenantManagementPM, public fatherComponent: CCSSettingsTabComponent) {
        super();
        this.currenctAirline = myTenantItem;
        this.zeroAirline = tenantZeroItem;
        this.entityPM = entityPM;
        this.partnersService = new PartnersDomainService()

        if (entityPM.AWBMessagesCCSTypeCode == "GLSHK") {
            this.isGLSHK = true;
        }

        if (myTenantItem != null) {
            this.tenantAirlineId = myTenantItem.Id;

            var isRequestedField: boolean = false;
            var isRegisteredField: boolean = false;

            if (this.isGLSHK) {
                isRegisteredField = myTenantItem.IsGLSHKRegistered;
                isRequestedField = myTenantItem.GLSHKRegistrationRequested;
            }

            else {
                isRegisteredField = myTenantItem.IsChampRegistered;
                isRequestedField = myTenantItem.ChampRegistrationRequested;
            }

            this.isRequested = isRequestedField;
            this.isRegistered = isRegisteredField;
            this.isDeclined = myTenantItem.IsDeclined;
            this.declineNotes = myTenantItem.DeclineNotes;
        }

        this.SetUIProperties();
        this.LoadAirlineTenant();
    }

    public IsRegistrationNeeded: boolean = false;
    public IsRequestedEnabled: boolean = false;
    public IsDeclinedEnabled: boolean = false;
    private SetUIProperties() {
        var isRegistrationNeeded: boolean = false;
        var isFieldEnabled: boolean = false;

        if (this.isGLSHK) {
            isRegistrationNeeded = this.zeroAirline.GLSHKNeedsRegistration;
        }

        else {
            isRegistrationNeeded = this.zeroAirline.ChampNeedsRegistration;
        }        

        if (isRegistrationNeeded) {
            if (!this.IsRegistered) {
                isFieldEnabled = true;
            }
        }

        this.IsRegistrationNeeded = isRegistrationNeeded;
        this.IsRequestedEnabled = isFieldEnabled;
        this.IsDeclinedEnabled = isFieldEnabled;

        this.UIProperties.SetEnabled("DeclineNotes", "Airline", this.IsDeclined);
    }

    get Code() { return this.zeroAirline.Code; }
    get EnglishName() { return this.zeroAirline.EnglishName; }
    get Prefix() { return this.zeroAirline.Prefix; }
    get RegistrationNotes() { return this.zeroAirline.RegistrationNotes; }
    
    private isRegistered: boolean = false;
    get IsRegistered() { return this.isRegistered; }
    set IsRegistered(newValue: boolean) {
        if (this.isRegistered != newValue) {
            this.isRegistered = newValue;

            this.SetUIProperties();

            var message: string = newValue ? "Registering Airline..." : "UnRegistering Airline...";
            var loggedContactName: string = SessionLocator.LoggedUserPM.EnglishName;

            this.CurrentSession.StartBusyIndicator(message);

            this.partnersService.RegisteringAirline(newValue, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode, loggedContactName).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }

    private isRequested: boolean = false;
    get IsRequested() { return this.isRequested; }
    set IsRequested(newValue: boolean) {
        if (this.isRequested != newValue) {
            this.isRequested = newValue;

            this.CurrentSession.StartBusyIndicator("Request Airline...");
            this.partnersService.RegistrationRequested(newValue, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.fatherComponent.LoadCurrentTenantAirlines();
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }

    private isDeclined: boolean = false;
    get IsDeclined() { return this.isDeclined; }
    set IsDeclined(newValue: boolean) {
        if (this.isDeclined != newValue) {
            this.isDeclined = newValue;

            this.SetUIProperties();

            this.CurrentSession.StartBusyIndicator("Decline Airline...");
            this.partnersService.SetIsDeclined(newValue, this.DeclineNotes, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.SetUIProperties();
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }
    
    private declineNotes: string;
    get DeclineNotes() { return this.declineNotes; }
    set DeclineNotes(newValue: string) {
        if (this.declineNotes != newValue) {
            this.declineNotes = newValue;
        }
    }

    DeclineNotesLostFocusMethod(notes: string) {
        var myOriginNotes: string = null;
        if (this.currenctAirline != null) {
            myOriginNotes = this.currenctAirline.DeclineNotes;
        }

        if (notes != myOriginNotes) {
            this.DeclineNotes = notes;

            this.CurrentSession.StartBusyIndicator("Decline Airline...");
            this.partnersService.SetIsDeclined(this.IsDeclined, this.DeclineNotes, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }

    private isDirect: boolean = false;
    get IsDirect() { return this.isDirect; }
    set IsDirect(newValue: boolean) {
        if (this.isDirect != newValue) {
            this.isDirect = newValue;

            this.CurrentSession.StartBusyIndicator("Updating Participant...");
            this.partnersService.SetIsDirect(newValue, this.tenantAirlineId, this.zeroAirline.Id, this.entityPM.Id, this.entityPM.AWBMessagesCCSTypeCode).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }

    private airlineTenant: TenantManagementPM;
    public IsDirectEnabled: boolean = false;
    private LoadAirlineTenant() {
        var service: GlobalDomainService = new GlobalDomainService();

        service.GetAirlineTenantExistsForAirline(this.zeroAirline.Code).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.airlineTenant = myResponse.Result;

                if (this.airlineTenant != null) {
                    this.IsDirectEnabled = true;

                    this.GetParticipant();
                }
            }
        });
    }

    private GetParticipant() {
        this.partnersService.GetIsDirect(this.entityPM.Id, this.airlineTenant.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.isDirect = myResponse.Result;
            }
        });
    }
}
