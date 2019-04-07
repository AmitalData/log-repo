import {Component, OnInit, AfterViewInit} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {TenantPM} from '../../EntityPMs/TenantPM';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPMService} from '../../Services/StandardPMs/TenantPMService';
import {PaymentTermList} from '../../EntityLists/PaymentTermList';
import {PaymentTermListService} from '../../Services/StandardLists/PaymentTermListService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {InfraSettings} from '../../../Infrastructure/Utilities/InfraSettings';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'LogBoxSettings',
    moduleId: module.id,
    templateUrl: './LogBoxSettings.html',
})

export class LogBoxSettings extends BaseComponent implements OnInit, AfterViewInit {

    public DataContext: LogBoxSettings = this;
    public ObjectTableName: string = "Tenant";
    public TenantPm: TenantPM = new TenantPM();
    public IsVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this.LoadTenantPMMethod();
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(response=> {
        });
    }

    ngOnInit() {
        this.StockTypes = this.GetStockTypes();
    }

    ngAfterViewInit() {
     
    }

    // Load Tenant 
    public IsTenantUS: boolean = false;
    private LoadTenantPMMethod() {
        var myService: TenantPMService = new TenantPMService();
        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            this.TenantPm = response.Result;
            this.IsVisibile = true;

            if (this.TenantPm.CountryCode.toUpperCase() == "US") {
                this.IsTenantUS = true;
            }
        });
    }
     Agent: StockTypesDetails = new StockTypesDetails("A", "Agent"); 
     Customer: StockTypesDetails = new StockTypesDetails("C", "Customer");
   

    GetStockTypes() { 
        var list = []; 
        list.push(this.Agent); 
        list.push(this.Customer);
        return list; 
    }

    StockTypeValueChanged(event) {
        this.StockTypeCode = event.Code;
        this.StockType = event;
    }

    private stockType: StockTypesDetails;
    get StockType() {
        if (this.TenantPm.StockTypeCode == "A") {
            return this.Agent;
        }
        else if (this.TenantPm.StockTypeCode == "C") {
            return this.Customer;
        }
        return this.stockType;
    }
    set StockType(value: StockTypesDetails) {
        if (this.stockType != value) {
            this.stockType = value;
        }
    }

    private stockTypes: StockTypesDetails[];
    public get StockTypes() { return this.stockTypes;}
    public set StockTypes(newValue: StockTypesDetails[]) {
        this.stockTypes = newValue;
    }

    get CustomerId() { return this.TenantPm.CustomerId; }
    set CustomerId(value: string) {
        if (this.TenantPm.CustomerId != value) {
            this.TenantPm.CustomerId = value;
        }
    }

    get StockTypeCode() { return this.TenantPm.StockTypeCode; }
    set StockTypeCode(value: string) {
        if (this.TenantPm.StockTypeCode != value) {
            this.TenantPm.StockTypeCode = value;
        }
    }

    get DocumentShareAsDefault() { return this.TenantPm.DocumentShareAsDefault; }
    set DocumentShareAsDefault(newValue: boolean) {
        if (this.TenantPm.DocumentShareAsDefault != newValue) {
            this.TenantPm.DocumentShareAsDefault = newValue;
        }
    }



    get IsCustomerTenantShare() { return this.TenantPm.IsCustomerTenantShare; }
    set IsCustomerTenantShare(value: boolean) {
        if (this.TenantPm.IsCustomerTenantShare != value) {
            this.TenantPm.IsCustomerTenantShare = value;
        }
    }

    get CustomerTenantShareImportFile() { return this.TenantPm.CustomerTenantShareImportFile; }
    set CustomerTenantShareImportFile(value: boolean) {
        if (this.TenantPm.CustomerTenantShareImportFile != value) {
            this.TenantPm.CustomerTenantShareImportFile = value;
        }
    }
    
    get CustomerTenantShareImportFileVisible() {
        var result = false;
        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
            result = true;
        }

        return result;
    }

    get IsCustomerTenantShareVisible() {
        var result = false;
        if (FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
            result = true;
        }

        return result;
    }

    //Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.TenantPm, this.DataContext.ObjectTableName, errors);


        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            
            this.SubmitTenantChanges();
        }
    }

    SubmitTenantChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");

        var myService: TenantPMService = new TenantPMService();
        myService.update(this.TenantPm).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    InfraSettings.TenantPM = this.TenantPm;
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}

export class StockTypesDetails {

    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }

    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }

}
