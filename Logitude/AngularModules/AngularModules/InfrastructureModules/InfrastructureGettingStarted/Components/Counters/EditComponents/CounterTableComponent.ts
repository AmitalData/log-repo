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

@Component({
    moduleId: module.id,
    templateUrl: './CounterTableComponent.html',
})

export class CounterTableComponent extends BaseComponent {
    public CounterId: string;
    public CounterPM: CounterPM;
    public EntityPM: CounterDefinitionPM;
    public DataContext = this;
    public ObjectTableName = "CounterDefinition";
    public APIHelper: CounterAPIHelper;
    public IsCounterUsed: boolean = false;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
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

        if (this.APIHelper.CounterDefinitions.length == 0) {
            this.APIHelper.CounterDefinitions.push(this.EntityPM);
        }

        else {
            this.EntityPM = this.APIHelper.CounterDefinitions[0];
        }        
    }

    public get Prefix() { return this.EntityPM.Prefix; }
    public set Prefix(value: string) {
        if (this.EntityPM.Prefix != value) {
            this.EntityPM.Prefix = value;
            this.CalculateSampleValue();
        }
    }

    public get CounterSize() { return this.EntityPM.CounterSize; }
    public set CounterSize(value: number) {
        if (this.EntityPM.CounterSize != value) {
            this.EntityPM.CounterSize = value;
            this.CalculateSampleValue();
        }
    }

    public get StartNumber() { return this.EntityPM.StartNumber; }
    public set StartNumber(value: number) {
        if (this.EntityPM.StartNumber != value) {
            this.EntityPM.StartNumber = value;

            this.CalculateSampleValue();
        }
    }

    public get UniquePerPrefix() { return this.EntityPM.UniquePerPrefix; }
    public set UniquePerPrefix(value: boolean) {
        if (this.EntityPM.UniquePerPrefix != value) {
            this.EntityPM.UniquePerPrefix = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        let counterLength: number = 15;
        if (this.ObjectTableName == "Shipment" || this.ObjectTableName == "Quote") {
            counterLength = 20;
        }
        var isValidGreaterStartNumber: boolean = true;

        if (!AppTool.IsNullOrEmpty(this.StartNumber)) {
            if (this.StartNumber < this.EntityPM.StartNumber_Old) {
                isValidGreaterStartNumber = false;
            }
        }

        if (!isValidGreaterStartNumber) {
            var messageWindow = new MessageWindow();
            messageWindow.Show("The new start number must be greater than current start number!");
        }

        else {
            var errors: string[] = [];
            if (this.CounterSize > counterLength) {
                errors.push("Maximum size allowed for counter is " + counterLength);
                }
            if (this.UniquePerPrefix == true) {
                Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

                if (this.UniquePerPrefix && !AppTool.IsNullOrEmpty(this.Prefix) && !AppTool.IsNullOrEmpty(this.StartNumber)) {
                    if ((this.StartNumber).toString().length + AppTool.GetCounterPrefixLength(this.Prefix) > counterLength) {
                        errors.push("Maximum length allowed for [Startnumber + Prefix] is " + counterLength);
                    }
                }
            }
            else if ((this.StartNumber).toString().length + AppTool.GetCounterPrefixLength(this.Prefix) > counterLength) {
                errors.push("Maximum length allowed for [Prefix + StartNumber] is " + counterLength);
            }
        

            this.ValidationErrorsList = errors;

            if (errors.length == 0) {
                var isDirty: boolean = false;

                if (this.EntityPM.IsDirty) {
                    isDirty = true;
                }

                if (!isDirty) {
                    this.CurrentSession.CloseCurrentWindow();
                }

                else {
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
