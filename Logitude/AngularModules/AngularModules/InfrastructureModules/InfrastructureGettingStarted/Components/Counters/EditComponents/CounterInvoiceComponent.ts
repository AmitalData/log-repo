import {Component} from '@angular/core';
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
    public HasInterestFeature: boolean=false;
    public ItemsSource: CounterInvoiceDefinitionItem[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.HasConsolidationFeature = FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent");
        this.HasInterestFeature = FeatureLocator.HasFeaturePermession("InterestReport", "Module");

        if (this.HasConsolidationFeature) {
            this.SameRadioButtonLabel = "Same for Invoice, Credit, Manifest and Consolidation.";
            this.DiffRadioButtonLabel = "Different for Invoice, Credit, Manifest and Consolidation.";
        }

        else {
            this.SameRadioButtonLabel = "Same for Invoice, Credit and Manifest.";
            this.DiffRadioButtonLabel = "Different for Invoice, Credit and Manifest.";
        }
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

        this.BuildItemsSource();
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

                this.APIHelper.CounterDefinitions.push(itemPM);
            }

            this.ItemsSource.push(new CounterInvoiceDefinitionItem(itemPM, item['Name'], this));
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
        }
    }

    public get Prefix() { return this.EntityPM.Prefix; }
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

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

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

                if (this.HasEmptyCounterSize())
                {
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

                    if ((this.StartNumber).toString().length + AppTool.GetCounterPrefixLength(this.Prefix) + AppTool.GetCounterPrefixLength(this.Suffix)> 20) {
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

    public SampleValue: string;

    private HasEmptyCounterSize() {
        return this.ItemsSource.some(item => AppTool.IsNullOrEmpty(item.CounterSize));
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
