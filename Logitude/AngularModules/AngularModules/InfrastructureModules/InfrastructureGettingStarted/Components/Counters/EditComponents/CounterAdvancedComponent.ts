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
    moduleId: module.id,
    templateUrl: './CounterAdvancedComponent.html',
})

export class CounterAdvancedComponent extends BaseComponent {
    public CounterId: string;
    public CounterPM: CounterPM;
    public EntityPM: CounterDefinitionPM;
    public DataContext = this;
    public ObjectTableName = "CounterDefinition";
    public APIHelper: CounterAPIHelper;
    public IsCounterUsed: boolean = false;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: any[] = [];
    public HasAllTransportsFeature: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        if (FeatureLocator.HasFeaturePermession("Shipment", "ALLTRANSPORTMODES")) {
            this.HasAllTransportsFeature = true;
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
        this.EntityPM.Parameter1 = "E";
        this.EntityPM.Parameter2 = "A";

        if (this.APIHelper.CounterDefinitions.length == 0) {
            this.APIHelper.CounterDefinitions.push(this.EntityPM);
        }

        else {
            if (this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == this.EntityPM.Parameter1 && f.Parameter2 == this.EntityPM.Parameter2)[0] != null) {
                this.EntityPM = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == this.EntityPM.Parameter1 && f.Parameter2 == this.EntityPM.Parameter2)[0];
            }

            else {
                this.APIHelper.CounterDefinitions.push(this.EntityPM);
            }

            var myPipe = new GroupByPipe();
            var myGroupbyCount = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;

            if (myGroupbyCount > 4) {
                this.sameForAllDirectios = false;
                this.sameForAllTransports = false;
            }

            else {
                switch (myGroupbyCount) {
                    case 1: {
                        this.sameForAllDirectios = true;
                        this.sameForAllTransports = true;
                        break;
                    }

                    case 2:
                    case 3: {
                        this.sameForAllDirectios = true;
                        this.sameForAllTransports = false;
                        break;
                    }

                    case 4: {
                        this.sameForAllDirectios = false;
                        this.sameForAllTransports = true;
                        break;
                    }
                }
            }
        }

        this.BuildItemsSource();
    }
    BuildItemsSource() {
        this.ItemsSource = [];

        var itemsParams: any[] = [];
        itemsParams.push({ Parameter1: 'E', Parameter2: "A" });
        itemsParams.push({ Parameter1: 'I', Parameter2: "A" });
        itemsParams.push({ Parameter1: 'D', Parameter2: "A" });
        itemsParams.push({ Parameter1: 'R', Parameter2: "A" });

        itemsParams.forEach(item => {
            var itemPM: CounterDefinitionPM = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == item['Parameter1'] && f.Parameter2 == item['Parameter2'])[0];

            if (itemPM == null) {
                itemPM = new CounterDefinitionPM();
                itemPM.CounterId = this.CounterPM.Id;
                itemPM.Tenant = this.CounterPM.Tenant;
                itemPM.UniquePerPrefix = this.EntityPM.UniquePerPrefix;
                itemPM.StartNumber = this.EntityPM.StartNumber;
                itemPM.StartNumber_Old = this.EntityPM.StartNumber_Old;
                itemPM.Parameter1 = item['Parameter1'];
                itemPM.Parameter2 = item['Parameter2'];

                this.APIHelper.CounterDefinitions.push(itemPM);
            }

            if (itemPM.Parameter1 == this.EntityPM.Parameter1) {
                this.ItemsSource.push(new CounterAdvancedColumnItem(itemPM, this));
            }

            else {
                if (!this.SameForAllDirectios) {
                    this.ItemsSource.push(new CounterAdvancedColumnItem(itemPM, this));
                }
            }
        });
    }

    private sameForAllDirectios: boolean = true;
    public get SameForAllDirectios() { return this.sameForAllDirectios; }
    public set SameForAllDirectios(value: boolean) {
        if (this.sameForAllDirectios != value) {
            this.sameForAllDirectios = value;
            this.BuildItemsSource();
        }
    }

    private sameForAllTransports: boolean = true;
    public get SameForAllTransports() { return this.sameForAllTransports; }
    public set SameForAllTransports(value: boolean) {
        if (this.sameForAllTransports != value) {
            this.sameForAllTransports = value;
            this.BuildItemsSource();
        }
    }

    public get UniquePerPrefix() { return this.EntityPM.UniquePerPrefix; }
    public set UniquePerPrefix(value: boolean) {
        if (this.EntityPM.UniquePerPrefix != value) {
            this.EntityPM.UniquePerPrefix = value;

            this.EntityPM.Prefix = this.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == this.EntityPM.Parameter1 && f.Parameter2 == this.EntityPM.Parameter2)[0].Prefix;

            this.APIHelper.CounterDefinitions.forEach(item => {
                item.UniquePerPrefix = value;

                if (!value) {
                    item.Prefix = this.EntityPM.Prefix;
                }
            });

            this.ItemsSource.forEach(item => {
                item.SetUIProperties();
            });
        }
    }

    public get Prefix() { return this.EntityPM.Prefix; }
    public set Prefix(value: string) {
        if (this.EntityPM.Prefix != value) {
            this.EntityPM.Prefix = value;

            this.APIHelper.CounterDefinitions.forEach(item => {
                item.Prefix = value;
            });

            this.CalculateSampleValue();
        }
    }

    public get CounterSize() { return this.EntityPM.CounterSize; }
    public set CounterSize(value: number) {
        if (this.EntityPM.CounterSize != value) {
            this.EntityPM.CounterSize = value;

            this.APIHelper.CounterDefinitions.forEach(item => {
                item.CounterSize = value;
            });

            this.CalculateSampleValue();
        }
    }


    public get StartNumber() { return this.EntityPM.StartNumber; }
    public set StartNumber(value: number) {
        if (this.EntityPM.StartNumber != value) {
            this.EntityPM.StartNumber = value;

            this.APIHelper.CounterDefinitions.forEach(item => {
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

                isValidUniquePrefix = false;

                var myPipe = new GroupByPipe();
                var myGroupbyCount: number = myPipe.transform(this.APIHelper.CounterDefinitions, "Prefix").length;

                if (this.SameForAllDirectios == true && this.SameForAllTransports == true) {
                    if (myGroupbyCount == 1) {
                        isValidUniquePrefix = true;
                    }
                }

                if (this.SameForAllDirectios == false && this.SameForAllTransports == true) {
                    if (myGroupbyCount == 4) {
                        isValidUniquePrefix = true;
                    }
                }

                if (this.SameForAllDirectios == true && this.SameForAllTransports == false) {
                    if (myGroupbyCount == 3) {
                        isValidUniquePrefix = true;
                    }
                }

                if (this.SameForAllDirectios == false && this.SameForAllTransports == false) {
                    if (myGroupbyCount == 12) {
                        isValidUniquePrefix = true;
                    }
                }
            }
            else {
            }

            if (!isValidUniquePrefix) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("Some Prefix values are invalid (Prefix should be unique)");
            }

            else {
                var errors: string[] = [];
                if (this.CounterSize > 20) {
                    errors.push("Maximum size allowed for counter is 20");
                }
                if (this.UniquePerPrefix == true) {
                    this.APIHelper.CounterDefinitions.forEach(item => {
                        Validator.TryValidateObject(item, this.ObjectTableName, errors);

                        if (item.UniquePerPrefix && !AppTool.IsNullOrEmpty(item.Prefix) && !AppTool.IsNullOrEmpty(item.StartNumber)) {
                            if ((item.StartNumber).toString().length + AppTool.GetCounterPrefixLength(item.Prefix) > 20) {
                                errors.push("Maximum length allowed for [Prefix + StartNumber] is 20");
                            }
                        }
                    });
                }
                else {

                    //var m = AppTool.GetCounterPrefixLength(this.Prefix);
                    if ((this.StartNumber).toString().length + AppTool.GetCounterPrefixLength(this.Prefix) > 20) {
                        errors.push("Maximum length allowed for [Prefix + StartNumber] is 20");
                    }

                    this.APIHelper.CounterDefinitions.forEach(item => {
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
    CalculateSampleValue() {

        this.SampleValue = AppTool.GetCounterResolvedNumber(this.Prefix, this.StartNumber, "", this.CounterSize);
        
    }
}
export class CounterAdvancedColumnItem {
    public Name: string;
    public DataContext = this;
    public EntityPM: CounterDefinitionPM;
    public ObjectTableName = "CounterDefinition";
    public ItemsSource: CounterAdvancedDefinitionItem[] = [];
    constructor(itemPM: CounterDefinitionPM, public father: CounterAdvancedComponent) {
        this.EntityPM = itemPM;
        this.BuildItemsSource();

        switch (itemPM.Parameter1) {
            case "E": { this.Name = "Export"; break; }
            case "I": { this.Name = "Import"; break; }
            case "D": { this.Name = "Domestic"; break; }
            case "R": { this.Name = "Drop"; break; }
        }
    }

    BuildItemsSource() {
        this.ItemsSource = [];

        if (this.father.SameForAllTransports) {
            this.ItemsSource.push(new CounterAdvancedDefinitionItem(this.EntityPM, this));
        }

        else {
            var itemsParams: any[] = [];
            itemsParams.push({ Parameter1: this.EntityPM.Parameter1, Parameter2: "A" });
            itemsParams.push({ Parameter1: this.EntityPM.Parameter1, Parameter2: "O" });
            itemsParams.push({ Parameter1: this.EntityPM.Parameter1, Parameter2: "I" });

            itemsParams.forEach(item => {
                var itemPM: CounterDefinitionPM = this.father.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == item['Parameter1'] && f.Parameter2 == item['Parameter2'])[0];

                if (itemPM == null) {
                    itemPM = new CounterDefinitionPM();
                    itemPM.CounterId = this.father.CounterPM.Id;
                    itemPM.Tenant = this.father.CounterPM.Tenant;
                    itemPM.UniquePerPrefix = this.father.UniquePerPrefix;
                    itemPM.StartNumber = this.father.StartNumber;
                    itemPM.Parameter1 = item['Parameter1'];
                    itemPM.Parameter2 = item['Parameter2'];
                }

                this.ItemsSource.push(new CounterAdvancedDefinitionItem(itemPM, this));
            });
        }
    }

    SetUIProperties() {
        this.ItemsSource.forEach(item => {
            item.SetUIProperties();
        });
    }

    //public get Prefix() { return this.EntityPM.Prefix; }
    //public set Prefix(value: string) {
    //    if (this.EntityPM.Prefix != value) {
    //        this.EntityPM.Prefix = value;
    //    }

    //    this.ItemsSource.forEach(item => {
    //        item.Prefix = value;
    //    });
    //}

    //public get StartNumber() { return this.EntityPM.StartNumber; }
    //public set StartNumber(value: number) {
    //    if (this.EntityPM.StartNumber != value) {
    //        this.EntityPM.StartNumber = value;
    //    }

    //    this.ItemsSource.forEach(item => {
    //        item.StartNumber = value;
    //    });
    //}
}
export class CounterAdvancedDefinitionItem extends BaseComponent {
    public DataContext = this;
    public EntityPM: CounterDefinitionPM;
    public ObjectTableName = "CounterDefinition";
    public Parameter: string;
    constructor(itemPM: CounterDefinitionPM, private father: CounterAdvancedColumnItem) {
        super();
        this.EntityPM = itemPM;
        this.Parameter = itemPM.Parameter2;
        this.SetUIProperties();
    }

    SetUIProperties() {
        var isEnabled: boolean = true;
        var isEnabled_StartNumber: boolean = true;

        if (this.father.father.IsCounterUsed) {
            isEnabled = false;
            isEnabled_StartNumber = false;
        }

        else if (!this.father.father.UniquePerPrefix) {
            isEnabled_StartNumber = false;
        }

        this.UIProperties.SetEnabled("Prefix", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("StartNumber", this.ObjectTableName, isEnabled_StartNumber);
        this.UIProperties.SetEnabled("CounterSize", this.ObjectTableName, isEnabled);
    }

    public get Prefix() { return this.EntityPM.Prefix; }
    public set Prefix(value: string) {
        if (this.EntityPM.Prefix != value) {
            this.EntityPM.Prefix = value;

            if (this.father.father.SameForAllDirectios == false && this.father.father.SameForAllTransports == false) {
                // No need to apply for others
            }

            else {
                if (this.father.father.SameForAllTransports) {
                    // Get All Definitions with same Parameter1: E (Export)
                    this.father.father.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == this.EntityPM.Parameter1).forEach(item => {
                        item.Prefix = value;
                    });
                }

                if (this.father.father.SameForAllDirectios) {
                    // Get All Definitions with same Parameter2: A (Airline)
                    this.father.father.APIHelper.CounterDefinitions.filter(f => f.Parameter2 == this.EntityPM.Parameter2).forEach(item => {
                        item.Prefix = value;
                    });
                }
            }
        }
    }

    public get CounterSize() { return this.EntityPM.CounterSize; }
    public set CounterSize(value: number) {
        if (this.EntityPM.CounterSize != value) {
            this.EntityPM.CounterSize = value;

            if (this.father.father.SameForAllDirectios == false && this.father.father.SameForAllTransports == false) {
                // No need to apply for others
            }

            else {
                if (this.father.father.SameForAllTransports) {
                    // Get All Definitions with same Parameter1: E (Export)
                    this.father.father.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == this.EntityPM.Parameter1).forEach(item => {
                        item.CounterSize = value;
                    });
                }

                if (this.father.father.SameForAllDirectios) {
                    // Get All Definitions with same Parameter2: A (Airline)
                    this.father.father.APIHelper.CounterDefinitions.filter(f => f.Parameter2 == this.EntityPM.Parameter2).forEach(item => {
                        item.CounterSize = value;
                    });
                }
            }
        }
    }

    public get StartNumber() { return this.EntityPM.StartNumber; }
    public set StartNumber(value: number) {
        if (this.EntityPM.StartNumber != value) {
            this.EntityPM.StartNumber = value;

            if (this.father.father.SameForAllTransports) {
                this.father.father.APIHelper.CounterDefinitions.filter(f => f.Parameter1 == this.EntityPM.Parameter1).forEach(item => {
                    item.StartNumber = value;
                });
            }

            if (this.father.father.SameForAllDirectios) {
                this.father.father.APIHelper.CounterDefinitions.filter(f => f.Parameter2 == this.EntityPM.Parameter2).forEach(item => {
                    item.StartNumber = value;
                });
            }
        }
    }
}
