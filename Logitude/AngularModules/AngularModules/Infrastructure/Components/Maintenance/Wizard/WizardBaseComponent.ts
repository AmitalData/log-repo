import {Component, ViewChildren, QueryList, Output, EventEmitter, ComponentRef}  from '@angular/core';
import {BaseComponent} from '../../LogitudeComponents/BaseComponent';
import {Validator} from '../../../Validators/Validator';
import {InfraSettings} from '../../../Utilities/InfraSettings';
import {SessionLocator} from '../../../Utilities/SessionLocator';
import {ServiceResponse} from '../../../DataContracts/ServiceResponse';
import {LocationDirective} from '../../../Utilities/LocationDirective';
import {AppTool, DateTool, ArrayTool} from '../../../Tools';
import {AgentPM} from '../../../../Common/EntityPMs/AgentPM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {VatTypePM} from '../../../../Common/EntityPMs/VatTypePM';
import {VatTypePercentagePM} from '../../../../Common/EntityPMs/VatTypePercentagePM';
import {AgentPMService} from '../../../../Common/Services/StandardPMs/AgentPMService';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {AddressPMService} from '../../../../Common/Services/StandardPMs/AddressPMService';
import {VatTypePMService} from '../../../../Common/Services/StandardPMs/VatTypePMService';
import {CommonDomainService} from '../../../../Common/Services/CommonDomainService';
import {EntityResourceService} from '../../../Services/EntityResourceService';
import {RatesTablePM} from '../../../EntityPMs/RatesTablePM';
import {RatesTablePMService} from '../../../Services/StandardPMs/RatesTablePMService';
import {CachedDataManager} from '../../../Utilities/CachedDataManager';
import { ObjectsLocator } from '../../../Locators/ObjectsLocator';
import { ObjectsUpdater } from '../../../Locators/ObjectsUpdater';

@Component({
    moduleId: module.id,
    templateUrl: './WizardBaseComponent.html',
})

export class WizardBaseComponent extends BaseComponent {
    public AgentPM: AgentPM = null;
    public TenantPM: TenantPM = null;
    public AddressPM: AddressPM = null;
    public VatTypePM: VatTypePM = null;
    public PercentagePM: VatTypePercentagePM = null;
    public IsResourcesReady: boolean = false;
    ValidationErrorsList: string[] = [];
    @Output() SaveCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() SignOutCompleted: EventEmitter<boolean> = new EventEmitter<boolean>(); 
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.InitServices();
    }

    private myAgentPMService: AgentPMService;
    private myTenantPMService: TenantPMService;
    private myAddressPMService: AddressPMService;
    private myVatTypePMService: VatTypePMService;
    private myCommonDomainService: CommonDomainService;
    InitServices() {
        this.myAgentPMService = new AgentPMService();
        this.myTenantPMService = new TenantPMService();
        this.myAddressPMService = new AddressPMService();
        this.myVatTypePMService = new VatTypePMService();
        this.myCommonDomainService = new CommonDomainService();
    }

    private Retries: number = 0;
    private timerToken: any;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(resp1 => {
                    this.entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(resp2 => {
                        this.InitObjectsData();                        
                    });
                });
            }
        }

        else {
            this.RunComponentTimer();
        }
    }
    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    InitObjectsData() {
        this.myTenantPMService.get(SessionLocator.TenantPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                this.TenantPM = myResponse.Result;
                this.TenantPM.ProfitCurrencyRate = null;

                if (AppTool.IsNullOrEmpty(this.TenantPM.ProfitCurrencyRate)) {
                    if (this.TenantPM.PackageCode == "IMPO") {
                        this.TenantPM.ProfitCurrencyRate = 4;
                    }
                }

                this.InitSTDVat();
            }
        });
    }
    InitSTDVat() {
        this.myCommonDomainService.GetSingleVatTypeByCode("STD").subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.VatTypePM = myResponse.Result;

                if (this.VatTypePM) {

                    var myPercentagePM: VatTypePercentagePM = null;

                    if (this.VatTypePM.VatTypePercentages.length == 0) {
                        myPercentagePM = new VatTypePercentagePM(this.VatTypePM);
                        myPercentagePM.Tenant = this.VatTypePM.Tenant;
                        myPercentagePM.VatTypeId = this.VatTypePM.Id;
                        myPercentagePM.FromDate = DateTool.GetCurrentDateAsUtc();
                        this.VatTypePM.AddVatTypePercentagePM(myPercentagePM);
                    }

                    else {
                        myPercentagePM = ArrayTool.SortByDate(this.VatTypePM.VatTypePercentages, "FromDate")[0];
                    }

                    if (this.TenantPM.STDVatPercentage != myPercentagePM.Percentage) {
                        this.TenantPM.STDVatPercentage = myPercentagePM.Percentage;
                    }

                    this.PercentagePM = myPercentagePM;
                   
                }
                this.InitAgentObject();
            }
        });
    }
    InitAgentObject() {
        if (AppTool.IsNullOrEmpty(SessionLocator.TenantPM.AgentId)) {
            this.AgentPM = new AgentPM();
            this.AgentPM.Code = "new";
            this.AgentPM.PartnerTypeId = "AG";
            this.AgentPM.EnglishName = SessionLocator.TenantPM.Company;
            this.AgentPM.Tenant = SessionLocator.TenantPM.Id;
            this.InitAddressObject();
        }

        else {
            this.myAgentPMService.get(SessionLocator.TenantPM.AgentId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.AgentPM = myResponse.Result;
                    this.InitAddressObject();
                }
            });
        }
    }
    InitAddressObject() {
        if (AppTool.IsNullOrEmpty(SessionLocator.TenantPM.AddressId)) {
            this.AddressPM = new AddressPM();
            this.AddressPM.Tenant = SessionLocator.TenantPM.Id;
            this.AddressPM.AddressTypeId = "M";
            this.AddressPM.Name = SessionLocator.TenantPM.Company;
            this.AddressPM.Description = SessionLocator.TenantPM.Company;
            this.AddressPM.PhoneNumber = SessionLocator.LoggedUserPM.BusinessPhone;
            this.InitScreenView();
        }

        else {
            this.myAddressPMService.get(SessionLocator.TenantPM.AddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.AddressPM = myResponse.Result;
                    this.InitScreenView();
                }
            });
        }
    }
    InitScreenView() {
        this.IsResourcesReady = true;
        this.SelectedTabCode = "ADD";
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(value: string) {
        if (this.selectedTabCode != value) {
            this.selectedTabCode = value;
            this.SelectionChanged();
        }
    }

    private PageChild_ADD: any = null;
    private PageChild_LOG: any = null;
    private PageChild_ACC: any = null;
    SelectionChanged() {
        let location: LocationDirective = this.AllLocations.toArray().filter(f => f.Code == this.SelectedTabCode)[0];
        if (location) {
            switch (location.Code) {
                case "ADD": {
                    if (this.PageChild_ADD == null) {
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/Maintenance/Wizard/WizardAddressCompnent', location.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_ADD = cmpRef.instance;
                                this.PageChild_ADD.InitializeComponent(this.TenantPM, this.AddressPM, this.AgentPM);
                            });
                    }

                    break;
                }

                case "LOG": {
                    if (this.PageChild_LOG == null) {
                        SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureGettingStarted/Components/UploadImage/UploadLogoComponent', location.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_LOG = cmpRef.instance;
                                this.PageChild_LOG.IsHideAreaCloseButton = true;
                            });
                    }

                    break;
                }

                case "ACC": {
                    if (this.PageChild_ACC == null) {
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/Maintenance/Wizard/WizardAccountingComponent', location.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_ACC = cmpRef.instance;
                                this.PageChild_ACC.InitializeComponent(this.TenantPM);
                            });
                    }

                    break;
                }
            }
        }
    }

    LogoTabIndexColor: string = "rgba(110, 113, 114, 0.37)";
    LogoTabTextColor: string = "rgba(110, 113, 114, 0.37)";
    LogoTabBackground: string = "rgba(110, 113, 114, 0.12)";
    FinishTabIndexColor: string = "rgba(110, 113, 114, 0.37)";
    FinishTabTextColor: string = "rgba(110, 113, 114, 0.37)";
    FinishTabBackground: string = "rgba(110, 113, 114, 0.12)";
    AccountingSettingsTabIndexColor: string = "rgba(110, 113, 114, 0.37)";
    AccountingSettingsTabTextColor: string = "rgba(110, 113, 114, 0.37)";
    AccountingSettingsTabBackground: string = "rgba(110, 113, 114, 0.12)"; 
    NextButtonClicked() {
        var errors: string[] = [];
       
        switch (this.SelectedTabCode) {
            case "ADD": {

                if (this.PageChild_ADD) {
                    this.PageChild_ADD.Validate(errors);
                }
                
                break;
            }

            case "ACC": {

                if (this.PageChild_ACC) {
                    this.PageChild_ACC.Validate(errors);
                }

                break;
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            if (SessionLocator.TenantPM.PackageCode == "IMPO" && this.SelectedTabCode == "ADD") {
                this.SelectedTabCode = "ACC";
            }
            else {

                if (this.SelectedTabCode == "ADD") {
                    this.SelectedTabCode = "LOG";
                    this.LogoTabBackground = "#1B90CB";
                    this.LogoTabIndexColor = "#FFFFFF";
                    this.LogoTabTextColor = "#1B90CB";

                }

                else if (this.SelectedTabCode == "LOG") {
                    this.SelectedTabCode = "ACC";
                    this.AccountingSettingsTabBackground = "#1B90CB";
                    this.AccountingSettingsTabIndexColor = "#FFFFFF";
                    this.AccountingSettingsTabTextColor = "#1B90CB";
                }

                else if (this.SelectedTabCode == "ACC") {
                    this.SelectedTabCode = "FIN";
                    this.FinishTabBackground = "#1B90CB";
                    this.FinishTabIndexColor = "#FFFFFF";
                    this.FinishTabTextColor = "#1B90CB";

                }

            }
        }
    }
    BackButtonClicked() {

        if (SessionLocator.TenantPM.PackageCode == "IMPO" && this.SelectedTabCode == "ACC") {
            this.SelectedTabCode = "ADD";
        }
        else {
            if (this.SelectedTabCode == "LOG") this.SelectedTabCode = "ADD";
            else if (this.SelectedTabCode == "ACC") this.SelectedTabCode = "LOG";
            else if (this.SelectedTabCode == "FIN") this.SelectedTabCode = "ACC";

        }
    

       
    } 
     
    SignoutClicked() {
        this.SignOutCompleted.emit(true);
        
    }
    FinishButtonClicked() {
        var errors: string[] = [];
        this.PageChild_ADD.Validate(errors);
        this.PageChild_ACC.Validate(errors);

        Validator.TryValidateObject(this.AgentPM, "Agent", errors);
        Validator.TryValidateObject(this.TenantPM, "Tenant", errors);
        Validator.TryValidateObject(this.AddressPM, "Address", errors);
        
        if (this.TenantPM.DayLightOffset != 0) {
            if (this.TenantPM.DayLightEndDate == null || this.TenantPM.DayLightStartDate == null) {
                errors.push("DayLightStartDate and DayLightEndDate should have values");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            var isSavingVAT: boolean = false;
            if (this.PercentagePM) {

                if (AppTool.IsNullOrEmpty(this.PercentagePM.Id)) {
                    if (!AppTool.IsNullOrEmpty(this.TenantPM.STDVatPercentage)) {
                        this.PercentagePM.Percentage = this.TenantPM.STDVatPercentage;
                        isSavingVAT = true;
                    }
                }

                else {
                    if (this.PercentagePM.Percentage != this.TenantPM.STDVatPercentage) {
                        this.PercentagePM.Percentage = this.TenantPM.STDVatPercentage;
                        isSavingVAT = true;
                    }
                }
            }

            if (isSavingVAT) {
                this.SaveVatType();
            }

            else {
                this.SaveAgent();
            }
        }
    }    
    SaveVatType() {
        this.myVatTypePMService.update(this.VatTypePM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ShowServiceErrors(myResponse);
            }

            else {
                this.SaveAgent();                
            }
        });
    }
    SaveAgent() {
        if (AppTool.IsNullOrEmpty(this.AgentPM.Id)) {
            this.myAgentPMService.insert(this.AgentPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ShowServiceErrors(myResponse);
                }

                else {
                    this.TenantPM.AgentId = this.AgentPM.Id;
                    this.AddressPM.CardId = this.AgentPM.Id;
                    this.SaveAddress();
                }
            });
        }

        else {
            this.myAgentPMService.update(this.AgentPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ShowServiceErrors(myResponse);
                }

                else {
                    this.TenantPM.AgentId = this.AgentPM.Id;
                    this.AddressPM.CardId = this.AgentPM.Id;
                    this.SaveAddress();
                }
            });
        }
    }
    SaveAddress() {
        if (AppTool.IsNullOrEmpty(this.AddressPM.Id)) {
            this.myAddressPMService.insert(this.AddressPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ShowServiceErrors(myResponse);
                }

                else {
                    this.TenantPM.AddressId = this.AddressPM.Id;
                    this.SaveTenant();
                }
            });
        }

        else {
            this.myAddressPMService.update(this.AddressPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    this.ShowServiceErrors(myResponse);
                }

                else {
                    this.TenantPM.AddressId = this.AddressPM.Id;
                    this.SaveTenant();
                }
            });
        }
    }
    SaveTenant() {
        this.myTenantPMService.update(this.TenantPM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ShowServiceErrors(myResponse);
            }

            else {

                this.SaveCurrencyRate();
            }
        });        
    }
    SaveCurrencyRate() {
        var newRatesTablePM = new RatesTablePM();
        newRatesTablePM.Tenant = this.TenantPM.Id;
        newRatesTablePM.BaseCurrencyId = this.TenantPM.CurrencyId;
        newRatesTablePM.ForeignCurrencyId = this.TenantPM.ProfitCurrencyId;
        newRatesTablePM.Rate = this.TenantPM.ProfitCurrencyRate;
        newRatesTablePM.LogDateTime = DateTool.GetCurrentDateTimeAsUtc();
        newRatesTablePM.ValueDate = DateTool.GetCurrentDateAsUtc();

        var myService = new RatesTablePMService();
        myService.insert(newRatesTablePM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ShowServiceErrors(myResponse);
            }

            else {
                CachedDataManager.RefreshTableData("Currency", true);

                this.myTenantPMService.get(this.TenantPM.Id).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ShowServiceErrors(myResponse);
                    }

                    else {

                        InfraSettings.TenantPM = this.TenantPM = myResponse.Result;

                        if (this.TenantPM.CountryCode == "MX") {
                            this.OnCreatingMexicanTenant();
                        }

                        else if (this.TenantPM.CountryCode == "US") {
                            this.OnCreatingUSTenant();
                        }

                        else if (this.TenantPM.CountryCode == "MA") {
                            this.OnCreatingMoroccoTenant();
                        }

                        else if (this.TenantPM.CountryCode == "IL") {
                            this.OnCreatingIsraelTenant();
                        }

                        else {
                            this.CurrentSession.StopBusyIndicator();
                            this.SaveCompleted.emit(true);
                        }
                    }
                });
            }
        });        
    }
    OnCreatingMexicanTenant() {
        this.myCommonDomainService.GetOnCreatingMexicanTenant().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ShowServiceErrors(myResponse);
            }

            else {

                CachedDataManager.RefreshTableData("VatType", true);

                if (SessionLocator.AccountingSettingPM) {
                    SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes = true;
                }

                this.myCommonDomainService.GetAllVatTypesGroups().subscribe((myResponse2: ServiceResponse) => {
                    if (!myResponse2.HasError) {
                        SessionLocator.AllVatTypesGroups = myResponse2.Result;
                    }

                    this.CurrentSession.StopBusyIndicator();
                    this.SaveCompleted.emit(true);
                });
            }
        });
    }
    OnCreatingUSTenant() {
        this.myCommonDomainService.GetOnCreatingUSTenant().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ShowServiceErrors(myResponse);
            }

            else {

                if (ObjectsLocator.CustomsInterfaceSettingPM) {
                    ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments = true;
                }


                this.CurrentSession.StopBusyIndicator();
                this.SaveCompleted.emit(true);
            }
        });
    }
    OnCreatingMoroccoTenant() {
        this.myCommonDomainService.OnCreatingMoroccoTenant().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ShowServiceErrors(myResponse);
            }

            else {
                if (ObjectsLocator.CustomsInterfaceSettingPM) {
                    ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments = true;
                }

                this.CurrentSession.StopBusyIndicator();
                this.SaveCompleted.emit(true);
            }
        });
    }
    OnCreatingIsraelTenant() {
        this.myCommonDomainService.OnCreatingIsraelTenant().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ShowServiceErrors(myResponse);
            }

            else {
                ObjectsUpdater.UpdateAccountingSettingPM(myResponse.Result);             

                this.CurrentSession.StopBusyIndicator();
                this.SaveCompleted.emit(true);
            }
        });
    }

    ShowServiceErrors(myResponse: ServiceResponse) {
        if (myResponse) {
            this.ValidationErrorsList = myResponse.ErrorsArray;
            this.CurrentSession.StopBusyIndicator();
        }
    }
}
