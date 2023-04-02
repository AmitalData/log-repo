import { Component, ViewChild, ViewContainerRef} from '@angular/core';
import { forEach } from 'cypress/types/lodash';
import { CounterDefinitionPM } from '../../../../../Common/EntityPMs/CounterDefinitionPM';
import { CounterPM } from '../../../../../Common/EntityPMs/CounterPM';
import { CounterAPIHelper, CountersDomainService } from '../../../../../Common/Services/CountersDomainService';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
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
    public NumberOfSeries: number;
    public CustomizedCounterItems: Array<CustomizedCounterItem> = [];
    public InvoicesTypes: Array<any> = [];
    public UniquePerPrefix: boolean = false;
    public ObjectTableName = "CounterDefinition";
    public APIHelper: CounterAPIHelper;
    private customizedARInvoiceCounterValidatingService: CustomizedARInvoiceCounterValidatingService;
    public MainCounterDefinitionsPMs: CounterDefinitionPM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public IsEnabled: boolean = true;
    constructor() {
        super();
        this.counterDefinitionPMExtendedService = new CounterDefinitionPMExtendedService();
        this.customizedARInvoiceCounterValidatingService = new CustomizedARInvoiceCounterValidatingService();
        this.BuildInvoicesTypes();
    }
    private BuildInvoicesTypes() {
        this.InvoicesTypes.push({
            Code: "IN",
            DisplayText: "AR Invoice"
        });
        this.InvoicesTypes.push({
            Code: "CD",
            DisplayText: "Credit Note"
        });
        this.InvoicesTypes.push({
            Code: "CI",
            DisplayText: "Customs"
        });
        this.InvoicesTypes.push({
            Code: "CC",
            DisplayText: "Credit Customs"
        });
        this.InvoicesTypes.push({
            Code: "CON",
            DisplayText: "Consolidation"
        });
        this.InvoicesTypes.push({
            Code: "COD",
            DisplayText: "Credit Consolidation"
        });
        this.InvoicesTypes.push({
            Code: "MN",
            DisplayText: "Manifest"
        });

    }
    Run() {
        this.currentSession.StartBusyIndicatorLoading();
        this.counterId = this.CounterInvoiceComponent.CounterId;
        this.MainCounterDefinitionsPMs = [];
        this.IsEnabled = !this.CounterInvoiceComponent.IsCounterUsed;
        this.counterDefinitionPMExtendedService.GetCustomizedCounterDefinitionsByCounterId(this.counterId).subscribe((response: ServiceResponse) => {
            if (response.HasError || !response.Result) {
                this.currentSession.StopBusyIndicator();
                return;
            }


            this.MainCounterDefinitionsPMs = response.Result;
            this.BuildCustomizedCounterItems(response.Result);
            if (this.MainCounterDefinitionsPMs && this.MainCounterDefinitionsPMs.length > 0 && !this.MainCounterDefinitionsPMs[0].UniquePerPrefix) this.StartNumber = this.MainCounterDefinitionsPMs[0].StartNumber;
            this.UpdateSeriesUniquePerPrefix();
            this.currentSession.StopBusyIndicator();
        });
    }

    BuildCustomizedCounterItems(counterDefinitions: Array<CounterDefinitionPM>) {
        if (counterDefinitions.length == 0) {
            this.NumberOfSeries = 1;
            this.SetDefaultCustomizedCounterItem();
            return;
        }
        let myPipe = new GroupByPipe();
        let myGroup = myPipe.transform(counterDefinitions, "Parameter1");
        this.NumberOfSeries = myGroup.length + 1;
        myGroup.forEach((value) => {
            this.PushCustomizedCounterItem(value.value, value.key);
        });

        this.SortCustomizedCounterItems();
    }
    private SortCustomizedCounterItems() {
        this.CustomizedCounterItems = this.CustomizedCounterItems.sort((a, b) => {
            if (a.SeriesCode < b.SeriesCode) {
                return -1;
            }
            if (a.SeriesCode > b.SeriesCode) {
                return 1;
            }
            return 0;
        });
    }
    private PushCustomizedCounterItem(counterdefinitions: any[], key: any) {
        let customizedCounterItem = new CustomizedCounterItem(key, this.counterId, this);
        counterdefinitions.forEach(def => {
            customizedCounterItem.CounterDefinitions.push(def);
        });

        customizedCounterItem.MapItemFields();
        this.CustomizedCounterItems.push(customizedCounterItem);
    }

    SetDefaultCustomizedCounterItem() {
        this.CustomizedCounterItems.push(new CustomizedCounterItem("Serie " + this.NumberOfSeries, this.counterId, this));
        this.NumberOfSeries = this.NumberOfSeries + 1;
        this.UpdateSeriesesStartNumber();
    }
    public EnabledInvoiceType(invoiceCode: string, serieCode: string): boolean {
        if (!this.IsEnabled) return false;
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
            if (this.UniquePerPrefix) return;
            item.StartNumber = this.CustomizedCounterItems[0].CounterDefinitions[0]?.StartNumber;
        });
        if (!this.UniquePerPrefix) {
            this.StartNumber = this.CustomizedCounterItems[0].CounterDefinitions[0]?.StartNumber;
        }
    }

    private startNumber: number;
    public get StartNumber() {
        return this.startNumber;
    }
    public set StartNumber(value: number) {
        if (this.startNumber == value) return;
        this.startNumber = value;
        this.UpdateSeriesesStartNumber();
    }
    UpdateSeriesesStartNumber() {
        if (this.UniquePerPrefix) return;
        this.CustomizedCounterItems.forEach(item => {
            item.StartNumber = this.StartNumber;
        });
    }

    DisableAddCustomizedCounterItems(): boolean {
        let numberOfCountrDefinitions = 0;
        this.CustomizedCounterItems.forEach(item => {
            numberOfCountrDefinitions += item.CounterDefinitions.length;
        });
        return numberOfCountrDefinitions == 7;
    }
    AddCustomizedCounterItem() {
        if (this.DisableAddCustomizedCounterItems()) return;
        let customizedCounterItem = new CustomizedCounterItem("Serie " + this.NumberOfSeries, this.counterId, this);
        customizedCounterItem.StartNumber = this.UniquePerPrefix ? null : this.StartNumber;
        customizedCounterItem.UniquePerPrefix = this.UniquePerPrefix;
        this.CustomizedCounterItems.push(customizedCounterItem);
        this.NumberOfSeries = this.NumberOfSeries + 1;
    }

    DeleteCustomizedCounterItem(customizedCounterItem: CustomizedCounterItem, isFromConfirmWindow: boolean = false) {
        if (!customizedCounterItem || !this.IsEnabled) return;
        this.CustomizedCounterItems = this.CustomizedCounterItems.filter(item => item.SeriesCode != customizedCounterItem.SeriesCode);
        if (isFromConfirmWindow) return;
        this.RefreshCustomizedCounterItems();
        this.UpdateSeriesUniquePerPrefix();
        this.UpdateSeriesesStartNumber();
    }
 
    RefreshCustomizedCounterItems() {
        this.NumberOfSeries = 1;
        if (!this.CustomizedCounterItems || this.CustomizedCounterItems.length == 0) {
            this.SetDefaultCustomizedCounterItem();
            return;
        }
        this.CustomizedCounterItems.forEach(item => {
            item.SeriesCode = "Serie " + this.NumberOfSeries;
            this.UpdateCounterDefinitionsPartameter1(item.SeriesCode, item.CounterDefinitions);
            this.NumberOfSeries = this.NumberOfSeries + 1;
        })
    }
    UpdateCounterDefinitionsPartameter1(seriesCode: string, counterDefinitions: CounterDefinitionPM[]) {
        if (AppTool.IsNullOrEmpty(seriesCode) || !counterDefinitions || counterDefinitions.length == 0) return;
        counterDefinitions.forEach(def => {
            def.Parameter1 = seriesCode;
        });
    }


    OkButtonClicked() {
        let seriesWithNoInvoices = this.CustomizedCounterItems.filter(item => item.CounterDefinitions?.length == 0);
        if (seriesWithNoInvoices != null && seriesWithNoInvoices.length > 0) {
            this.ShowConfirmationWindow(seriesWithNoInvoices);
            return;
        }
        this.BuildAPIHelperCounterDefinitions();
        this.ValidateCounterDefinitions();
    }
    ShowConfirmationWindow(customizedCounterItems: CustomizedCounterItem[]) {
        let seriesCodes = this.GetSeriesCodes(customizedCounterItems);
        let confirmationMessage = seriesCodes + (customizedCounterItems.length == 1 ? " is" : " are") + " not related to any invoice type, so "+(customizedCounterItems.length == 1 ? "it" : "they")+" will not be taken in consideration";
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(confirmationMessage);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.DeleteEmptyCustomizedCounterItems(customizedCounterItems);
                this.BuildAPIHelperCounterDefinitions();
                this.ValidateCounterDefinitions();
            }
        });
    }
    GetSeriesCodes(customizedCounterItems: CustomizedCounterItem[]) {
        let seriesCodes = "";
        customizedCounterItems.forEach(item => {
            seriesCodes = seriesCodes + (AppTool.IsNullOrEmpty(seriesCodes) ? "" : ", ") + item.SeriesCode;
        });
        return seriesCodes;
    }
    DeleteEmptyCustomizedCounterItems(customizedCounterItems: CustomizedCounterItem[]) {
        customizedCounterItems.forEach(item => {
            this.DeleteCustomizedCounterItem(item, true);
        });
        this.RefreshCustomizedCounterItems();
        this.UpdateSeriesUniquePerPrefix();
        this.UpdateSeriesesStartNumber();
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
        this.CounterInvoiceComponent.ValidationErrorsList = this.customizedARInvoiceCounterValidatingService.Validate(this.APIHelper.CounterDefinitions);
        if (this.CounterInvoiceComponent.ValidationErrorsList && this.CounterInvoiceComponent.ValidationErrorsList.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();
            var myService = new CountersDomainService();
            myService.Post(this.APIHelper).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    return;
                }
                this.CounterInvoiceComponent.ValidationErrorsList = myResponse.ErrorsArray;

            });
        }

    }
}






export class CustomizedCounterItem extends BaseComponent {


    public MainComponent: CustomizedARInvoiceCounterComponent;
    constructor(seriesCode: string, counterId: string, mainComponent: CustomizedARInvoiceCounterComponent ) {
        super();
        this.SeriesCode = seriesCode;
        this.CounterId = counterId;
        this.MainComponent = mainComponent;
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

    public IsDeleted: boolean = false;
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
        this.uniquePerPrefix = value;
        this.CounterDefinitions.forEach(counterDefinitionPM => {
            counterDefinitionPM.UniquePerPrefix = this.uniquePerPrefix;
        });
    }

    public CounterDefinitions: Array<CounterDefinitionPM> = [];

    public AddRemoveCounterDefinition(invoiceCode: string, checked: boolean) {
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
        this.UpdateCounterDefinitionPM(counterDefinitionPM);
        this.CounterDefinitions.push(counterDefinitionPM);
    }
    UpdateCounterDefinitionPM(counterDefinitionPM: CounterDefinitionPM) {
        let item = this.MainComponent.MainCounterDefinitionsPMs.filter(d => d.Parameter2 == counterDefinitionPM.Parameter2)[0];
        if (item) counterDefinitionPM.Id = item.Id;
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
        this.CounterDefinitions = this.CounterDefinitions.filter(c => c.Parameter2 != invoiceCode && !c.InActive);
    }

    public IsInvoiceChecked(invoiceCode: string): boolean {
        return this.CounterDefinitions.filter(c => c.Parameter2 == invoiceCode && !c.InActive)[0] != null;
    }
}
