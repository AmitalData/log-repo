import { Component, ViewChild, ViewContainerRef} from '@angular/core';
import { forEach } from 'cypress/types/lodash';
import { CounterDefinitionPM } from '../../../../../Common/EntityPMs/CounterDefinitionPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { GroupByPipe } from '../../../../../Infrastructure/Pipes/GroupByPipe';
import { CounterDefinitionPMExtendedService } from '../../../../../Infrastructure/Services/ExtendedPMs/CounterDefinitionPMExtendedService';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
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
    public HasNoCounterDefinitions: boolean = false;
    public NumberOfSerieses: number;
    public CustomizedCounterItems: Array<CustomizedCounterItem> = [];
    public InvoicesCodes: Array<string> = [];
    public UniquePerPrefix: boolean = false;
    public ObjectTableName = "CounterDefinition";

    constructor() {
        super();
        this.counterDefinitionPMExtendedService = new CounterDefinitionPMExtendedService();
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
        this.counterDefinitionPMExtendedService.GetCustomizedCounterDefinitionsByCounterId(this.counterId).subscribe((response: ServiceResponse) => {
            if (response.HasError || !response.Result) {
                this.currentSession.StopBusyIndicator();
                return;
            }
            this.BuildCounterDefinitionsList(response.Result);
            this.currentSession.StopBusyIndicator();
        });
    }
    BuildCounterDefinitionsList(counterDefinitions: Array<CounterDefinitionPM>) {
        if (counterDefinitions.length == 0) {
            this.NumberOfSerieses = 1;
            this.HasNoCounterDefinitions = true;
            this.SetDefaultCounterDefinitionSeries();
            return;
        }
        this.NumberOfSerieses = counterDefinitions.length + 1;
        let myPipe = new GroupByPipe();
        let myGroup = myPipe.transform(counterDefinitions, "Parameter1");
        myGroup.forEach((key, value) => {
        });

    }
    SetDefaultCounterDefinitionSeries() {
        this.CustomizedCounterItems.push(new CustomizedCounterItem("Serie " + this.NumberOfSerieses, this.counterId));
        this.NumberOfSerieses = this.NumberOfSerieses + 1;
    }
    public EnabledInvoiceType(invoiceCode: string, serieCode: string): boolean {
        //will check for other customized items, if the invoice included in them
        //check for current item if the invoice is checked inside it
        let otherCustomizedCounterItems = this.CustomizedCounterItems.filter(item => item.SeriesCode != serieCode);
        if (otherCustomizedCounterItems == null || otherCustomizedCounterItems.length == 0) return true;

        let isInvoiceCheckedInCurrentSerie = this.CustomizedCounterItems.filter(item => item.SeriesCode == serieCode)[0]?.IsInvoiceChecked(invoiceCode);
        if (isInvoiceCheckedInCurrentSerie) return true;
        let isInvoiceCheckedInOtherSerie = false
        for (let i = 0; i < otherCustomizedCounterItems.length; i++){
            isInvoiceCheckedInOtherSerie = otherCustomizedCounterItems[i].CounterDefinitions.filter(def => def.Parameter2 == invoiceCode)[0] != null;
            if (isInvoiceCheckedInOtherSerie) return false;
        }
        return true;
    }
    public SetUniquePerPrefix(value: boolean) {
        if (this.UniquePerPrefix == value) return;
        this.UniquePerPrefix = value;
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
        this.CustomizedCounterItems.push(customizedCounterItem);
        this.NumberOfSerieses = this.NumberOfSerieses + 1;
    }

}






export class CustomizedCounterItem extends BaseComponent {


    constructor(seriesCode: string, counterId: string) {
        super();
        this.SeriesCode = seriesCode;
        this.CounterId = counterId;
    }

    public IsDirty: boolean = false;
    public SeriesCode: string;
    public CounterId: string;
    public DataContext = this;
    public ObjectTableName = "CounterDefinition";

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
        let counterDefinitionPM = this.CounterDefinitions.filter(c => c.Parameter2 == invoiceCode)[0];
        if (counterDefinitionPM != null) return;
        counterDefinitionPM = this.GetInstanceOfCounterDefinitionPM(invoiceCode);
        this.CounterDefinitions.push(counterDefinitionPM);
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

        //counterDefinitionPM.UniquePerPrefix = 
        //counterDefinitionPM.UsePerBranch =

        return counterDefinitionPM;
    }

    private RemoveCounterDefinition(invoiceCode: string) {
        if (AppTool.IsNullOrEmpty(invoiceCode)) return;
        this.CounterDefinitions = this.CounterDefinitions.filter(c => c.Parameter2 != invoiceCode);
    }

    public IsInvoiceChecked(invoiceCode: string): boolean {
        return this.CounterDefinitions.filter(c => c.Parameter2 == invoiceCode)[0] != null;
    }
}
