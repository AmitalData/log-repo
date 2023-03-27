import { Component, ViewChild, ViewContainerRef} from '@angular/core';
import { forEach } from 'cypress/types/lodash';
import { CounterDefinitionPM } from '../../../../../Common/EntityPMs/CounterDefinitionPM';
import { CounterPM } from '../../../../../Common/EntityPMs/CounterPM';
import { CounterAPIHelper, CountersDomainService } from '../../../../../Common/Services/CountersDomainService';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { GroupByPipe } from '../../../../../Infrastructure/Pipes/GroupByPipe';
import { CounterDefinitionPMExtendedService } from '../../../../../Infrastructure/Services/ExtendedPMs/CounterDefinitionPMExtendedService';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { CustomizedARInvoiceCounterValidatingService } from '../../../Services/CustomizedARInvoiceCounterValidatingService';
import { CounterInvoiceComponent } from './CounterInvoiceComponent';


@Component({
    
    templateUrl: './CustomizedARInvoiceCounterComponent.html',
})

export class CustomizedARInvoiceCounterComponent extends BaseComponent {

    public DataContext: this;
    public CounterInvoiceComponent: CounterInvoiceComponent;
    private counterId: string;
    private currentSession = SessionLocator.SelectedSession;
    private counterDefinitionPMExtendedService: CounterDefinitionPMExtendedService;
    public NumberOfSerieses: number;
    public CustomizedCounterItems: Array<CustomizedCounterItem> = [];
    public InvoicesCodes: Array<string> = [];
    public UniquePerPrefix: boolean = false;
    public ObjectTableName = "CounterDefinition";
    public APIHelper: CounterAPIHelper;
    private customizedARInvoiceCounterValidatingService: CustomizedARInvoiceCounterValidatingService;
    public MainCounterDefinitionsPMs: CounterDefinitionPM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;


    constructor() {
        super();
        this.counterDefinitionPMExtendedService = new CounterDefinitionPMExtendedService();
        this.customizedARInvoiceCounterValidatingService = new CustomizedARInvoiceCounterValidatingService();
        this.BuildInvoicesCodes();
    }
    private BuildInvoicesCodes() {
        this.InvoicesCodes.push("AR Invoice");
        this.InvoicesCodes.push("Credit Note");
        this.InvoicesCodes.push("Customs");
        this.InvoicesCodes.push("Credit Customs");
        this.InvoicesCodes.push("Consolidation");
        this.InvoicesCodes.push("Credit Consolidation");
        this.InvoicesCodes.push("Manifest");
    }
    Run() {
        this.currentSession.StartBusyIndicatorLoading();
        this.counterId = this.CounterInvoiceComponent.CounterId;
        this.MainCounterDefinitionsPMs = [];

        this.counterDefinitionPMExtendedService.GetCustomizedCounterDefinitionsByCounterId(this.counterId).subscribe((response: ServiceResponse) => {
            if (response.HasError || !response.Result) {
                this.currentSession.StopBusyIndicator();
                return;
            }
            this.BuildCounterDefinitionsList(response.Result);
            this.UpdateSeriesUniquePerPrefix();
            this.currentSession.StopBusyIndicator();
        });
    }
    BuildCounterDefinitionsList(counterDefinitions: Array<CounterDefinitionPM>) {
        if (counterDefinitions.length == 0) {
            this.NumberOfSerieses = 1;
            this.SetDefaultCounterDefinitionSeries();
            return;
        }
        this.NumberOfSerieses = counterDefinitions.length + 1;
        let myPipe = new GroupByPipe();
        let myGroup = myPipe.transform(counterDefinitions, "Parameter1");
        myGroup.forEach((value, key) => {
            this.PushCustomizedCounterItem(value[key].value, value[key].Key);
        });

    }
    private PushCustomizedCounterItem(counterdefinitions: any[], key: any) {
        let customizedCounterItem = new CustomizedCounterItem(key, this.counterId);
        counterdefinitions.forEach(def => {
            customizedCounterItem.CounterDefinitions.push(def);
            this.MainCounterDefinitionsPMs.push(def);
            customizedCounterItem.MainCounterDefinitionsPMs = this.MainCounterDefinitionsPMs;
        });
        customizedCounterItem.MapItemFields();
        this.CustomizedCounterItems.push(customizedCounterItem);
    }

    SetDefaultCounterDefinitionSeries() {
        this.CustomizedCounterItems.push(new CustomizedCounterItem("Serie " + this.NumberOfSerieses, this.counterId));
        this.NumberOfSerieses = this.NumberOfSerieses + 1;
    }
    public EnabledInvoiceType(invoiceCode: string, serieCode: string): boolean {

        let otherCustomizedCounterItems = this.CustomizedCounterItems.filter(item => item.SeriesCode != serieCode);
        if (otherCustomizedCounterItems == null || otherCustomizedCounterItems.length == 0) return true;

        let isInvoiceCheckedInCurrentSerie = this.CustomizedCounterItems.filter(item => item.SeriesCode == serieCode)[0]?.IsInvoiceChecked(invoiceCode);
        if (isInvoiceCheckedInCurrentSerie) return true;
        let isInvoiceCheckedInOtherSerie = false
        for (let i = 0; i < otherCustomizedCounterItems.length; i++){
            isInvoiceCheckedInOtherSerie = otherCustomizedCounterItems[i].CounterDefinitions.filter(def => def.Parameter2 == invoiceCode && !def.InActive)[0] != null;
            if (isInvoiceCheckedInOtherSerie) return false;
        }
        return true;
    }
    public SetUniquePerPrefix(value: boolean) {
        if (this.UniquePerPrefix == value) return;
        this.UniquePerPrefix = value;
        this.UpdateSeriesUniquePerPrefix();
    }
    UpdateSeriesUniquePerPrefix() {
        this.CustomizedCounterItems.forEach(item => {
            item.UniquePerPrefix = this.UniquePerPrefix;
        });
    }

    private startNumber: number;
    public get StartNumber() {
        return this.startNumber;
    }
    public set StartNumber(value: number) {
        if (this.startNumber == value) return;
        this.startNumber = value;
        this.UpdateSeriesesStartNumber(value);
    }
    UpdateSeriesesStartNumber(value: number) {
        this.CustomizedCounterItems.forEach(item => {
            item.StartNumber = value;
        });
    }
    AddCustomizedCounterItem() {
        let customizedCounterItem = new CustomizedCounterItem("Serie " + this.NumberOfSerieses, this.counterId);
        customizedCounterItem.StartNumber = this.UniquePerPrefix ? null : this.StartNumber;
        customizedCounterItem.UniquePerPrefix = this.UniquePerPrefix;
        this.CustomizedCounterItems.push(customizedCounterItem);
        this.NumberOfSerieses = this.NumberOfSerieses + 1;
    }




    OkButtonClicked() {
        this.BuildAPIHelperCounterDefinitions();
        this.CounterInvoiceComponent.ValidationErrorsList = this.ValidateCounterDefinitions();
    }
    BuildAPIHelperCounterDefinitions() {
        this.APIHelper = new CounterAPIHelper();
        this.APIHelper.CounterDefinitions = [];
        this.APIHelper.CounterId = this.counterId;
        this.APIHelper.CounterPM = new CounterPM();
        this.APIHelper.IsCustomized = true;
        this.CustomizedCounterItems.forEach(item => {
            this.PushCustomizedCounterDefinitionsToAPIHelper(item.CounterDefinitions);
        });

    }
    PushCustomizedCounterDefinitionsToAPIHelper(counterDefinitions: CounterDefinitionPM[]) {
        counterDefinitions.forEach(def => {
            this.APIHelper.CounterDefinitions.push(def);
        });
    }

    private ValidateCounterDefinitions() {
        let validationErrorsList = this.customizedARInvoiceCounterValidatingService.Validate(this.APIHelper.CounterDefinitions);
        if (validationErrorsList && validationErrorsList.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();
            var myService = new CountersDomainService();
            myService.Post(this.APIHelper).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    validationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.CurrentSession.CloseCurrentWindowEmit("Ok");
                }
            });
        }

        return validationErrorsList;
    }
}






export class CustomizedCounterItem extends BaseComponent {

    constructor(seriesCode: string, counterId: string) {
        super();
        this.SeriesCode = seriesCode;
        this.CounterId = counterId;
        this.CounterDefinitions = [];
    }

    MapItemFields() {
        if (this.CounterDefinitions.length == 0) return;
        this.Prefix = this.CounterDefinitions[0].Prefix;
        this.CounterSize = this.CounterDefinitions[0].CounterSize;
        this.StartNumber = this.CounterDefinitions[0].StartNumber;
        this.CounterDefinitions.forEach(counterDefinitionPM => {
            counterDefinitionPM.StartNumber_Old = this.StartNumber;
        });
        this.Suffix = this.CounterDefinitions[0].Suffix;
        this.setFormat();
    }

    public IsDirty: boolean = false;
    public SeriesCode: string;
    public CounterId: string;
    public DataContext = this;
    public ObjectTableName = "CounterDefinition";
    public MainCounterDefinitionsPMs: CounterDefinitionPM[] = [];

    private prefix: string;
    public get Prefix() {
        return this.prefix;
    }
    public set Prefix(value: string) {
        if (this.prefix == value) return;
        this.IsDirty = true;
        this.prefix = value;
        this.CounterDefinitions.forEach(counterDefinitionPM => {
            counterDefinitionPM.Prefix = this.prefix;
        });
        this.setFormat();
    }

    private suffix: string;
    public get Suffix() {
        return this.suffix;
    }
    public set Suffix(value: string) {
        if (this.suffix == value) return;
        this.IsDirty = true;
        this.suffix = value;
        this.CounterDefinitions.forEach(counterDefinitionPM => {
            counterDefinitionPM.Suffix = this.suffix;
        });
        this.setFormat();
    }

    private counterSize: number;
    public get CounterSize() {
        return this.counterSize;
    }
    public set CounterSize(value: number) {
        if (this.counterSize == value) return;
        this.IsDirty = true;
        this.counterSize = value;
        this.CounterDefinitions.forEach(counterDefinitionPM => {
            counterDefinitionPM.CounterSize = this.counterSize;
        });
        this.setFormat();
    }

    private startNumber: number;
    public get StartNumber() {
        return this.startNumber;
    }
    public set StartNumber(value: number) {
        if (this.startNumber == value) return;
        this.IsDirty = true;
        this.startNumber = value;
        this.CounterDefinitions.forEach(counterDefinitionPM => {
            counterDefinitionPM.StartNumber = this.startNumber;
        });
        this.setFormat();
    }

    public Format: string;
    public setFormat() {
        this.Format = AppTool.GetCounterResolvedNumber(this.Prefix, this.StartNumber, this.Suffix, this.CounterSize);
    }

    private uniquePerPrefix: boolean = false;
    public get UniquePerPrefix() { return this.uniquePerPrefix; }
    public set UniquePerPrefix(value: boolean) {
        if (this.uniquePerPrefix == value) return;
        this.IsDirty = true;
        this.uniquePerPrefix = value;
        this.CounterDefinitions.forEach(counterDefinitionPM => {
            counterDefinitionPM.UniquePerPrefix = this.uniquePerPrefix;
        });
    }

    public CounterDefinitions: Array<CounterDefinitionPM> = [];

    public AddRemoveCounterDefinition(invoiceCode: string, checked: boolean) {
        this.IsDirty = true;
        if (checked) {
            this.AddCounterDefinition(invoiceCode);
            return;
        }
        this.RemoveCounterDefinition(invoiceCode);
    }
    
    private AddCounterDefinition(invoiceCode: string) {
        if (AppTool.IsNullOrEmpty(invoiceCode)) return;
        let counterDefinitionPM = this.CounterDefinitions.filter(c => c.Parameter2 == invoiceCode && !c.InActive)[0];
        if (counterDefinitionPM != null) return;
        counterDefinitionPM = this.GetInstanceOfCounterDefinitionPM(invoiceCode);
        this.UpdateData(counterDefinitionPM, false);
        this.CounterDefinitions.push(counterDefinitionPM);
    }
    UpdateData(counterDefinitionPM: CounterDefinitionPM, isDelete: boolean) {
        if (isDelete) {
            counterDefinitionPM.InActive = true;
        }
        else   {
            let item = this.MainCounterDefinitionsPMs.filter(d => d.Parameter2 == counterDefinitionPM.Parameter2)[0];
            if (item) counterDefinitionPM.Id = item.Id;
            this.MainCounterDefinitionsPMs = this.MainCounterDefinitionsPMs.filter(d => d.Parameter2 != item.Parameter2);
            this.MainCounterDefinitionsPMs.push(counterDefinitionPM);
        }
       
    }
    GetInstanceOfCounterDefinitionPM(invoiceCode: string): CounterDefinitionPM {
        let counterDefinitionPM = new CounterDefinitionPM();
        counterDefinitionPM.Tenant = SessionLocator.Tenant;
        counterDefinitionPM.Parameter1 = this.SeriesCode;
        counterDefinitionPM.Parameter2 = invoiceCode;
        counterDefinitionPM.Prefix = this.Prefix;
        counterDefinitionPM.StartNumber = this.StartNumber;
        counterDefinitionPM.CounterId = this.CounterId;
        counterDefinitionPM.CounterSize = this.CounterSize;
        counterDefinitionPM.Suffix = this.Suffix;
        counterDefinitionPM.IsCustomized = true;

        counterDefinitionPM.UniquePerPrefix = this.UniquePerPrefix;
        //counterDefinitionPM.UsePerBranch =

        counterDefinitionPM.StartNumber_Old = 0;
        return counterDefinitionPM;
    }

    private RemoveCounterDefinition(invoiceCode: string) {
        if (AppTool.IsNullOrEmpty(invoiceCode)) return;
        let counterDefinitionPM = this.CounterDefinitions.filter(c => c.Parameter2 == invoiceCode && !c.InActive)[0];
        this.UpdateData(counterDefinitionPM, true);
    }

    public IsInvoiceChecked(invoiceCode: string): boolean {
        return this.CounterDefinitions.filter(c => c.Parameter2 == invoiceCode && !c.InActive)[0] != null;
    }
}
