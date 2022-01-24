import { Component } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Validator } from '../../../Infrastructure/Validators/Validator';

@Component({
    
    templateUrl: './TariffPriceStepsComponent.html',
})

export class TariffPriceStepsComponent extends BaseComponent {

    public DataContext = this;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: TariffSettingStep[] = [];
    public UnitOfMeasurementCode: string;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
    }

    SetWindowArgs(args) {
        this.DefaultPriceSteps = args;
        this.BuildItemsSource();
    }

    private defaultPriceSteps: string;
    get DefaultPriceSteps() { return this.defaultPriceSteps; }
    set DefaultPriceSteps(value: string) {
        if (this.defaultPriceSteps != value) {
            this.defaultPriceSteps = value;
        }
    }

    BuildItemsSource() {
        var Steps: string[] = [];
        if (this.DefaultPriceSteps != null) {
            Steps = this.DefaultPriceSteps.split(',');
        }

        var index: number = 0;
        Steps.forEach(item => {
            this.ItemsSource.push(new TariffSettingStep(item, index, this));
            this.BuildDefaultPriceSteps();
            index++;
        });

        if (Steps.length < 8) {
            for (var i = Steps.length; i < 8; i++) {
                this.ItemsSource.push(new TariffSettingStep(null, index, this));
                index++;
            }
        }
    }

    BuildDefaultPriceSteps() {

        var iDefaultPriceSteps: string = null;

        this.ItemsSource.filter(f => f.Step != null).sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 }).forEach(item => {
            if (AppTool.IsNullOrEmpty(iDefaultPriceSteps)) {
                iDefaultPriceSteps = "" + item.Step;
            }
            else {
                iDefaultPriceSteps += "," + item.Step;
            }
        });

        this.DefaultPriceSteps = iDefaultPriceSteps;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.DefaultPriceSteps)) {
            errors.push("Price steps Field is Required");
        }

        var isValidSort: boolean = true;
        var SortedItemStep: number = 0;

        this.ItemsSource.filter(f => !AppTool.IsNullOrEmpty(f.Step)).sort((a, b) => { return (a.Index === b.Index) ? 0 : (a.Index < b.Index) ? -1 : 1 }).forEach(item => {
            if (SortedItemStep == 0) {
                SortedItemStep = item.Step;
            }

            else {
                if (Number(item.Step) <= Number(SortedItemStep)) {
                    isValidSort = false;
                }

                else {
                    SortedItemStep = item.Step;
                }
            }
        });

        if (!isValidSort) {
            errors.push("Price steps must be sorted");
        }

        this.ValidationErrorsList = this.ValidationErrorsList.concat(errors);

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit(this.DefaultPriceSteps);
        }
    }
}

class TariffSettingStep extends BaseComponent {
    public Index: number;
    public UnitOfMeasurementCode: string;
    public DataContext = this;
    constructor(iStep: string, index: number, private father: TariffPriceStepsComponent) {
        super();

        this.Index = index;
        this.UnitOfMeasurementCode = this.father.UnitOfMeasurementCode;
        if (iStep) {
            this.Step = +iStep;
        }
    }

    private step: number = null;
    get Step() { return this.step; }
    set Step(value: number) {
        if (this.step != value) {
            this.step = value;
            this.father.BuildDefaultPriceSteps();
        }
    }


    DeleteClicked() {
        this.Step = null;
    }
}
