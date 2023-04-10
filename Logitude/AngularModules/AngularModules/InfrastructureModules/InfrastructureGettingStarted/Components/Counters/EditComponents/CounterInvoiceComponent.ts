import { Component, ViewChild, ViewContainerRef} from '@angular/core';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {CounterPM} from '../../../../../Common/EntityPMs/CounterPM';
import {CounterDefinitionPM} from '../../../../../Common/EntityPMs/CounterDefinitionPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {CountersDomainService, CounterAPIHelper} from '../../../../../Common/Services/CountersDomainService';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {GroupByPipe} from '../../../../../Infrastructure/Pipes/GroupByPipe';
import { Observable } from 'rxjs';
import { CustomizedARInvoiceCounterComponent } from './CustomizedARInvoiceCounterComponent';

@Component({
    
    templateUrl: './CounterInvoiceComponent.html',
})

export class CounterInvoiceComponent extends BaseComponent {
    public CounterId: string;
    public CounterPM: CounterPM;
    public EntityPM: CounterDefinitionPM;
    public DataContext = this;
    public ObjectTableName = "CounterDefinition";
    public APIHelper: CounterAPIHelper;
    public IsCounterUsed: boolean = false;
    public IsResourcesReady: boolean = false;
    public HasConsolidationFeature: boolean = false;
    public ValidationErrorsList: string[] = [];
    public SameRadioButtonLabel: string;
    public DiffRadioButtonLabel: string;
    public HasInterestFeature: boolean = false;
    public HasBranchCounterCodeFeature: boolean = false;
    public HasSeparatePerBranchCounterCodeFeature: boolean = false;
    public ItemsSource: CounterInvoiceDefinitionItem[] = [];
    private CurrentSession = SessionLocator.SelectedSession;

    public CustomizedRadioButtonLabel: string;
    public HasCustomizedCounterFeature: boolean = false;
    @ViewChild('CustomizedCounterComponent', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

    constructor() {
        super();

        this.SetFeatures();
        this.SetRadioButtonsLabels();
    }

    private SetFeatures() {
        this.HasConsolidationFeature = FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent");
        this.HasInterestFeature = FeatureLocator.HasFeaturePermession("InterestReport", "Module");
        this.HasBranchCounterCodeFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "BCC")[0] ? true : false;
        this.HasSeparatePerBranchCounterCodeFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "SPB")[0] ? true : false;
        this.HasCustomizedCounterFeature = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "ICC")[0] ? true : false;
    }

    private SetRadioButtonsLabels() {
        if (this.HasConsolidationFeature) {
            this.SameRadioButtonLabel = "Same for Invoice, Credit, Manifest, Customs, Customs Credit, Consolidation and Credit Consolidation.";
            this.DiffRadioButtonLabel = "Different for Invoice, Credit, Manifest, Customs, Customs Credit, Consolidation and Credit Consolidation.";
        }

        else {
            this.SameRadioButtonLabel = "Same for Invoice, Credit and Manifest.";
            this.DiffRadioButtonLabel = "Different for Invoice, Credit and Manifest.";
        }
        this.CustomizedRadioButtonLabel = "Customized Counter";
    }

    SetWindowArgs(args: any) {
        this.CounterId = args["CounterId"];

        if (this.CounterId) {
            this.CurrentSession.StartBusyIndicatorLoading();

            var myService = new CountersDomainService();
            myService.GetCounterAPIHelper(this.CounterId).subscribe((myResponse: ServiceResponse) => {

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.APIHelper = myResponse.Result;

                    if (this.APIHelper) {
                        this.APIHelper.CounterDefinitions = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 != "TX");

                        this.CounterPM = this.APIHelper.CounterPM;
                        this.IsCounterUsed = this.APIHelper.IsCounterUsed;
                        this.InitializeDefinitions();
                        this.SetUIProperties();                        
                    }

                    this.CalculateSampleValue();
                }

                this.IsResourcesReady = true;
                this.CurrentSession.StopBusyIndicator();
            });
        }
    }
    SetUIProperties() {
        this.UIProperties.SetEnabled("Prefix", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("Suffix", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("StartNumber", this.ObjectTableName, !this.IsCounterUsed);
        this.UIProperties.SetEnabled("CounterSize", this.ObjectTableName, !this.IsCounterUsed);
    }
    InitializeDefinitions() {

        // Dummy:Init
        this.EntityPM = new CounterDefinitionPM();
        this.EntityPM.CounterId = this.CounterPM.Id;
        this.EntityPM.Tenant = this.CounterPM.Tenant;
        this.EntityPM.UniquePerPrefix = false;
        this.EntityPM.StartNumber = 1000;
        this.EntityPM.StartNumber_Old = 0;
        this.EntityPM.Parameter1 = "IN";
        this.EntityPM.Parameter2 = null;

        if (this.APIHelper.CounterDefinitions.length == 0) {
            this.APIHelper.CounterDefinitions.push(this.EntityPM);
        }

        else {
            if (this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == this.EntityPM.Parameter1)[0] != null) {
                this.EntityPM = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == this.EntityPM.Parameter1)[0];
            }

            else {
                this.APIHelper.CounterDefinitions.push(this.EntityPM);
            }

            var myPipe = new GroupByPipe();
            var myGroupbyCount: number = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;

            if (myGroupbyCount == 1) {
                this.sameForAllTypes = true;
            }

            else {
                this.sameForAllTypes = false;
            }
        }

        this.CheckIsCustomizedCounter();
        this.BuildItemsSource();
    }
    private activateConsolidationCreditNoteCounter: boolean = false;

    public get ActivateConsolidationCreditNoteCounter() {
        this.activateConsolidationCreditNoteCounter = this.ItemsSource.filter(i => i.EntityPM.Parameter1 == "COD").length > 0 &&
            !this.ItemsSource.filter(i => i.EntityPM.Parameter1 == "COD")[0].EntityPM.InActive;
        return this.activateConsolidationCreditNoteCounter;
    }
    public set ActivateConsolidationCreditNoteCounter(value: boolean) {
        if (this.activateConsolidationCreditNoteCounter == value) return;
        this.activateConsolidationCreditNoteCounter = value;
        this.ItemsSource.filter(i => i.EntityPM.Parameter1 == "COD")[0].EntityPM.InActive = !value;
    }
    BuildItemsSource() {
        this.ItemsSource = [];

        var itemsParams: any[] = [];
        itemsParams.push({ Code: 'IN', Name: "Invoice" });
        itemsParams.push({ Code: 'CD', Name: "Credit" });
        itemsParams.push({ Code: 'MN', Name: "Manifest" });
        itemsParams.push({ Code: 'CI', Name: "Customs Invoice" });
        itemsParams.push({ Code: 'CC', Name: "Customs Credit" });
        if (this.HasConsolidationFeature) {
            itemsParams.push({ Code: 'CON', Name: "Consolidation" });
            itemsParams.push({ Code: 'COD', Name: "Consolidation Credit" });
        }
        if (SessionLocator.TenantPM.AccountingActivated) {
            itemsParams.push({ Code: 'IT', Name: "Interest Invoice" });
            itemsParams.push({ Code: 'IC', Name: "Interest Credit" });
        }

        itemsParams.forEach(item => {
            var itemPM: CounterDefinitionPM = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == item['Code'])[0];

            if (itemPM == null) {
                itemPM = new CounterDefinitionPM();
                itemPM.CounterId = this.EntityPM.CounterId;
                itemPM.Tenant = this.EntityPM.Tenant;
                itemPM.UniquePerPrefix = this.EntityPM.UniquePerPrefix;
                itemPM.StartNumber = this.EntityPM.StartNumber;
                itemPM.StartNumber_Old = this.EntityPM.StartNumber_Old;
                itemPM.Parameter1 = item['Code'];
                itemPM.Parameter1 = null;
                itemPM.InActive = false;
                itemPM.UsePerBranch = false;
                this.APIHelper.CounterDefinitions.push(itemPM);
            }

            this.ItemsSource.push(new CounterInvoiceDefinitionItem(itemPM, item['Name'], this));
        });
    }

    //private seperatePerBranchChanged: boolean = false;
    private seperatePerBranch: boolean = false;
    public get SeperatePerBranch() {
        this.seperatePerBranch = this.ItemsSource[0].EntityPM.UsePerBranch;
        return this.seperatePerBranch;
    }
    public set SeperatePerBranch(value: boolean) {
        if (this.seperatePerBranch == value) return;
        this.seperatePerBranch = value;
        //this.seperatePerBranchChanged = true;
        this.ItemsSource.forEach(item => {
            item.EntityPM.UsePerBranch = value;
        });
    }

    private sameForAllTypes: boolean = true;
    public get SameForAllTypes() { return this.sameForAllTypes; }
    public set SameForAllTypes(value: boolean) {
        if (this.sameForAllTypes != value) {
            this.sameForAllTypes = value;

            this.EntityPM.UniquePerPrefix = false;
            this.EntityPM.Prefix = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == "IN")[0].Prefix;
            this.EntityPM.StartNumber = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == "IN")[0].StartNumber;
            this.ItemsSource.forEach(item => {
                item.UniquePerPrefix = this.EntityPM.UniquePerPrefix;
                item.Prefix = this.EntityPM.Prefix;
                item.StartNumber = this.EntityPM.StartNumber;
            });
        }
    }

    public get UniquePerPrefix() { return this.EntityPM.UniquePerPrefix; }
    public set UniquePerPrefix(value: boolean) {
        if (this.EntityPM.UniquePerPrefix != value) {
            this.EntityPM.UniquePerPrefix = value;

            this.EntityPM.Prefix = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == "IN")[0].Prefix;

            this.ItemsSource.forEach(item => {
                item.UniquePerPrefix = value;

                if (!value) {
                    item.Prefix = this.EntityPM.Prefix;
                }

                item.SetUIProperties();
            });

            if (this.customizedARInvoiceCounterComponent) this.customizedARInvoiceCounterComponent.SetUniquePerPrefix(value);
        }
    }
    public get Prefix() {
        return this.EntityPM.Prefix;
    }
    public set Prefix(value: string) {
        if (this.EntityPM.Prefix != value) {
            this.EntityPM.Prefix = value;

            this.ItemsSource.forEach(item => {
                item.Prefix = value;
            });

            this.CalculateSampleValue();
        }
    }

    public get Suffix() { return this.EntityPM.Suffix; }
    public set Suffix(value: string) {
        if (this.EntityPM.Suffix != value) {
            this.EntityPM.Suffix = value;

            this.ItemsSource.forEach(item => {
                item.Suffix = value;
            });

            this.CalculateSampleValue();
        }
    }

    public get CounterSize() { return this.EntityPM.CounterSize; }
    public set CounterSize(value: number) {
        if (this.EntityPM.CounterSize != value) {
            this.EntityPM.CounterSize = value;

            this.ItemsSource.forEach(item => {
                item.CounterSize = value;
            });
            this.CalculateSampleValue();
        }
    }

    public get StartNumber() { return this.EntityPM.StartNumber; }
    public set StartNumber(value: number) {
        if (this.EntityPM.StartNumber != value) {
            this.EntityPM.StartNumber = value;

            this.ItemsSource.forEach(item => {
                item.StartNumber = value;
            });

            this.CalculateSampleValue();
        }
    }

    private CheckIsCustomizedCounter() {
        if (!this.HasCustomizedCounterFeature) return;
        let customizedCounters = this.APIHelper.CounterDefinitions.filter(c => c.IsCustomized == true);
        this.IsCustomizedCounter = customizedCounters != null && customizedCounters.length > 0;
        if (this.IsCustomizedCounter) {
            this.UniquePerPrefix = customizedCounters[0]?.UniquePerPrefix;
        }        
        this.APIHelper.CounterDefinitions = this.APIHelper.CounterDefinitions.filter(c => c.IsCustomized == false);
    }

    private isCustomizedCounter: boolean = false;
    public get IsCustomizedCounter() { return this.isCustomizedCounter; }
    public set IsCustomizedCounter(value: boolean) {
        if (this.isCustomizedCounter == value) return;
        this.isCustomizedCounter = value;
        this.RunComponent();
    }
    RunComponent() {
        if (!this.IsCustomizedCounter) return;

        if (this.viewContainerRef) {
            this.LoadCustomizedCounterComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private customizedARInvoiceCounterComponent: CustomizedARInvoiceCounterComponent;
    private LoadCustomizedCounterComponent() {
        SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CustomizedARInvoiceCounterComponent', this.viewContainerRef)
            .then(cmpRef => {
                this.customizedARInvoiceCounterComponent = cmpRef.instance;
                this.customizedARInvoiceCounterComponent.CounterInvoiceComponent = this;
                this.customizedARInvoiceCounterComponent.SetUniquePerPrefix(this.UniquePerPrefix);
                this.customizedARInvoiceCounterComponent.Run();
            });
    }

    SameForAllTypesChecked() {
        this.SameForAllTypes = true;
        this.IsCustomizedCounter = false;
        this.ValidationErrorsList = [];
    }
    DiffForEachTypesChecked() {
        this.SameForAllTypes = false;
        this.IsCustomizedCounter = false;
        this.ValidationErrorsList = [];
    }
    IsCustomizedCounterChecked() {
        this.SameForAllTypes = false;
        this.IsCustomizedCounter = true;
        this.ValidationErrorsList = [];
    }
    //private largestLastValueOfCounterStat: number = 0;

   

    //ValidateStartNumber() {
    //    new CountersDomainService().GetLastValueCounterStatByCounterId(this.CounterId).subscribe((myResponse: ServiceResponse) => {

    //        if (myResponse.HasError) {
    //            this.ShowLastValueCounterMessageWindow(myResponse.ErrorsArray[0]);
    //            return;
    //        }

    //        this.HandelLastValueCounterStatResponse(myResponse.Result);

    //    });
        
    //}


    //HandelLastValueCounterStatResponse(response) {

    //    let largestLastValueOfCounterStat = response;
    //    if (largestLastValueOfCounterStat == 0 || !largestLastValueOfCounterStat) {
    //        this.Save();
    //        return;
    //    };

    //    let message = "";
    //    this.APIHelper.CounterDefinitions.forEach(item => {
    //        if (item.StartNumber <= largestLastValueOfCounterStat) {
    //            message = "The start number must be greater than the Last Value Counter " + largestLastValueOfCounterStat + " !";
    //        }
    //    });


    //    if (!AppTool.IsNullOrEmpty(message)) {
    //        this.ShowLastValueCounterMessageWindow(message);
    //        return;
    //    }

    //    this.Save();
    //}



    //ShowLastValueCounterMessageWindow(msgValue:string) {
    //    var messageWindow = new MessageWindow();
    //    messageWindow.Show(msgValue);
    //}
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



    OkButtonClicked() {
        {
            if (this.IsCustomizedCounter) {
                this.customizedARInvoiceCounterComponent.OkButtonClicked();
                return;
            }
            var isValidGreaterStartNumber: boolean = true;

            this.APIHelper.CounterDefinitions.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.StartNumber)) {
                    if (item.StartNumber < item.StartNumber_Old) {
                        isValidGreaterStartNumber = false;
                    }
                }
            });

            if (!isValidGreaterStartNumber) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("The new start number must be greater than current start number!");
            }

            else {
                var isValidUniquePrefix: boolean = true;

                if (this.UniquePerPrefix) {
                    var myPipe = new GroupByPipe();
                    var myGroupbyCount: number = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;
                    if (myGroupbyCount != this.APIHelper.CounterDefinitions.length) {
                        isValidUniquePrefix = false;
                    }
                }

                if (!isValidUniquePrefix) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("Some Prefix values are invalid (Prefix should be unique)");
                }

                else {
                    var errors: string[] = [];

                    if (this.HasEmptyCounterSize()) {
                        errors.push("Size field is mandatory!");
                    }

                    if (this.UniquePerPrefix == true) {
                        this.APIHelper.CounterDefinitions.forEach(item => {
                            if (item.CounterSize > 20) {
                                errors.push("Maximum size allowed for counter is 20");
                            }
                            Validator.TryValidateObject(item, this.ObjectTableName, errors);

                            if (item.UniquePerPrefix && !AppTool.IsNullOrEmpty(item.Prefix) && !AppTool.IsNullOrEmpty(item.StartNumber)) {
                                if ((item.StartNumber).toString().length + AppTool.GetCounterPrefixLength(item.Prefix) + AppTool.GetCounterPrefixLength(item.Suffix) > 20) {
                                    errors.push("Maximum length allowed for [Prefix + StartNumber + Suffix] is 20");
                                }
                            }
                        });
                    }
                    else {

                        if ((this.StartNumber).toString().length + AppTool.GetCounterPrefixLength(this.Prefix) + AppTool.GetCounterPrefixLength(this.Suffix) > 20) {
                            errors.push("Maximum length allowed for [Prefix + StartNumber + Suffix] is 20");
                        }

                        this.APIHelper.CounterDefinitions.forEach(item => {
                            if (item.CounterSize > 20) {
                                errors.push("Maximum size allowed for counter is 20");
                            }
                            Validator.TryValidateObject(item, this.ObjectTableName, errors);
                        });
                    }

                    this.ValidationErrorsList = errors;

                    if (errors.length == 0) {

                        this.CurrentSession.StartBusyIndicatorSaving();

                        var myService = new CountersDomainService();
                        myService.Post(this.APIHelper).subscribe((myResponse: ServiceResponse) => {

                            this.CurrentSession.StopBusyIndicator();

                            if (myResponse.HasError) {
                                this.ValidationErrorsList = myResponse.ErrorsArray;
                            }

                            else {
                                this.CurrentSession.CloseCurrentWindowEmit("Ok");
                            }
                        });
                    }
                }
            }
        }

    }

    //Save() {
    //    var isValidGreaterStartNumber: boolean = true;

    //    this.APIHelper.CounterDefinitions.forEach(item => {
    //        if (!AppTool.IsNullOrEmpty(item.StartNumber)) {
    //            if (item.StartNumber < item.StartNumber_Old) {
    //                isValidGreaterStartNumber = false;
    //            }
    //        }
    //    });

    //    if (!isValidGreaterStartNumber) {
    //        var messageWindow = new MessageWindow();
    //        messageWindow.Show("The new start number must be greater than current start number!");
    //    }

    //    else {
    //        var isValidUniquePrefix: boolean = true;

    //        if (this.UniquePerPrefix) {
    //            var myPipe = new GroupByPipe();
    //            var myGroupbyCount: number = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;
    //            if (myGroupbyCount != this.APIHelper.CounterDefinitions.length) {
    //                isValidUniquePrefix = false;
    //            }
    //        }

    //        if (!isValidUniquePrefix) {
    //            var messageWindow = new MessageWindow();
    //            messageWindow.Show("Some Prefix values are invalid (Prefix should be unique)");
    //        }

    //        else {
    //            var errors: string[] = [];

    //            if (this.HasEmptyCounterSize()) {
    //                errors.push("Size field is mandatory!");
    //            }

    //            if (this.UniquePerPrefix == true) {
    //                this.APIHelper.CounterDefinitions.forEach(item => {
    //                    if (item.CounterSize > 20) {
    //                        errors.push("Maximum size allowed for counter is 20");
    //                    }
    //                    Validator.TryValidateObject(item, this.ObjectTableName, errors);

    //                    if (item.UniquePerPrefix && !AppTool.IsNullOrEmpty(item.Prefix) && !AppTool.IsNullOrEmpty(item.StartNumber)) {
    //                        if ((item.StartNumber).toString().length + AppTool.GetCounterPrefixLength(item.Prefix) + AppTool.GetCounterPrefixLength(item.Suffix) > 20) {
    //                            errors.push("Maximum length allowed for [Prefix + StartNumber + Suffix] is 20");
    //                        }
    //                    }
    //                });
    //            }
    //            else {

    //                if ((this.StartNumber).toString().length + AppTool.GetCounterPrefixLength(this.Prefix) + AppTool.GetCounterPrefixLength(this.Suffix) > 20) {
    //                    errors.push("Maximum length allowed for [Prefix + StartNumber + Suffix] is 20");
    //                }

    //                this.APIHelper.CounterDefinitions.forEach(item => {
    //                    if (item.CounterSize > 20) {
    //                        errors.push("Maximum size allowed for counter is 20");
    //                    }
    //                    Validator.TryValidateObject(item, this.ObjectTableName, errors);
    //                });
    //            }

    //            this.ValidationErrorsList = errors;

    //            if (errors.length == 0) {

    //                this.CurrentSession.StartBusyIndicatorSaving();

    //                var myService = new CountersDomainService();
    //                myService.Post(this.APIHelper).subscribe((myResponse: ServiceResponse) => {

    //                    this.CurrentSession.StopBusyIndicator();

    //                    if (myResponse.HasError) {
    //                        this.ValidationErrorsList = myResponse.ErrorsArray;
    //                    }

    //                    else {
    //                        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    //                    }
    //                });
    //            }
    //        }
    //    }
    //}



    public SampleValue: string;

    private HasEmptyCounterSize() {
        return this.ItemsSource.some(item => AppTool.IsNullOrEmpty(item.CounterSize) && !item.EntityPM.InActive);
    }

    CalculateSampleValue() {

        this.SampleValue = AppTool.GetCounterResolvedNumber(this.Prefix, this.StartNumber, this.Suffix, this.CounterSize);

    }

}
export class CounterInvoiceDefinitionItem extends BaseComponent {
    public Name: string;
    public DataContext = this;
    public EntityPM: CounterDefinitionPM;
    public ObjectTableName = "CounterDefinition";
    constructor(itemPM: CounterDefinitionPM, name: string, private father: CounterInvoiceComponent) {
        super();
        this.Name = name;
        this.EntityPM = itemPM;
        this.SetUIProperties();
    }

    SetUIProperties() {
        var isEnabled: boolean = true;
        var isEnabled_StartNumber: boolean = true;

        if (this.father.IsCounterUsed) {
            isEnabled = false;
            isEnabled_StartNumber = false;
        }

        else if (!this.UniquePerPrefix) {
            isEnabled_StartNumber = false;
        }

        this.UIProperties.SetEnabled("Prefix", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("Suffix", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("CounterSize", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("StartNumber", this.ObjectTableName, isEnabled_StartNumber);
    }

    public get UniquePerPrefix() { return this.EntityPM.UniquePerPrefix; }
    public set UniquePerPrefix(value: boolean) {
        if (this.EntityPM.UniquePerPrefix != value) {
            this.EntityPM.UniquePerPrefix = value;
            this.SetUIProperties();
        }
    }

    public get Prefix() { return this.EntityPM.Prefix; }
    public set Prefix(value: string) {
        if (this.EntityPM.Prefix != value) {
            this.EntityPM.Prefix = value;
        }
    }

    public get Suffix() { return this.EntityPM.Suffix; }
    public set Suffix(value: string) {
        if (this.EntityPM.Suffix != value) {
            this.EntityPM.Suffix = value;
        }
    }

    public get CounterSize() { return this.EntityPM.CounterSize; }
    public set CounterSize(value: number) {
        if (this.EntityPM.CounterSize != value) {
            this.EntityPM.CounterSize = value;
        }
    }

    public get StartNumber() { return this.EntityPM.StartNumber; }
    public set StartNumber(value: number) {
        if (this.EntityPM.StartNumber != value) {
            this.EntityPM.StartNumber = value;
        }
    }
}
