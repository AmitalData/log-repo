import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../../../Infrastructure/Tools';
import {PaddingPipe} from '../../../../Infrastructure/Pipes/PaddingPipe';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AWBStackDomainService, StockSeriesListClass, StockSeries} from '../../../Services/AWBStackDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './AssignToShipperComponent.html',
})

export class AssignToShipperComponent extends BaseComponent {
    public Entity: StockSeries;
    public IsCustomerMode: boolean = false;
    public ValidationErrorsList: string[] = [];
    public DataContext: AssignToShipperComponent = this;
    private StackDomainService: AWBStackDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.StackDomainService = new AWBStackDomainService();
    }

    SetWindowArgs(args: any) {
        this.ShipperId = args['ShipperId'];
        this.IsCustomerMode = args['IsCustomerMode'];
        this.Entity = args['StockSeriesItem'];
        
        this.SetUIProperties();

        var pipe = new PaddingPipe();
        this.from = pipe.transform(this.Entity.From, "L", 8, "0");
        this.to = pipe.transform(this.Entity.To, "L", 8, "0");
        this.total = this.Entity.Total;
    }

    private SetUIProperties() {
        this.UIProperties.SetEnabled("ShipperId", null, !this.IsCustomerMode);
        this.UIProperties.SetEnabled("Total", null, this.IsPartialSelection);
        this.UIProperties.SetEnabled("From", null, this.IsPartialSelection);
        this.UIProperties.SetEnabled("To", null, false);        
    }

    private isPartialSelection: boolean = false;
    get IsPartialSelection() { return this.isPartialSelection }
    set IsPartialSelection(newValue: boolean) {
        if (this.isPartialSelection != newValue) {
            this.isPartialSelection = newValue;
            this.SetUIProperties();
        }
    }

    private shipperId: string = null;
    get ShipperId() { return this.shipperId }
    set ShipperId(newValue: string) {
        if (this.shipperId != newValue) {
            this.shipperId = newValue;
        }
    }

    private total: number = null;
    get Total() { return this.total }
    set Total(newValue: number) {
        if (this.total != newValue) {
            this.total = newValue;
            this.Validate();
        }
    }

    private from: string = null;
    get From() { return this.from }
    set From(newValue: string) {
        if (this.from != newValue) {
            this.from = newValue;
            this.Validate();
        }
    }

    private to: string = null;
    get To() { return this.to }
    set To(newValue: string) {
        if (this.to != newValue) {
            this.to = newValue;
        }
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        this.CurrentSession.StartBusyIndicatorSaving();

        this.Validate();

        if (this.ValidationErrorsList.length == 0) {
            var errors: string[] = [];
            var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

            if (this.ShipperId == null) {
                errors.push(msg.replace("%FieldName", "Shipper"));
            }

            this.ValidationErrorsList = errors;
        }

        if (this.ValidationErrorsList.length > 0) {
            this.CurrentSession.StopBusyIndicator();
        }

        else {

            this.StackDomainService.AssignStockSeriesToCustomer(+this.From, +this.To, this.Entity.AirlineId, this.ShipperId).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            });
        }
    }

    Validate() {
        var errors: string[] = [];
        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.From)) {
            errors.push(msg.replace("%FieldName", "From"));
        }

        else if (this.From.length != 8) {
            errors.push("From Field length should be 8 digits");
        }

        else {
            var pipe = new PaddingPipe();

            // Origin Data
            var fromString = pipe.transform(this.Entity.From, "L", 8, "0");
            var toString = pipe.transform(this.Entity.To, "L", 8, "0");
            var start: number = +fromString.substr(0, fromString.length - 1);
            var end: number = +toString.substr(0, toString.length - 1);
            var fr: number = +this.From.substr(0, this.From.length - 1);
            var to: number = +this.To.substr(0, this.To.length - 1);

            var myTotal: number = +this.Total;

            if (myTotal != 0) {

                if ((myTotal > this.Entity.Total) || (fr + myTotal - 1) > end) {
                    errors.push("Selected series is out of available range");
                }

                else {
                    this.To = ((fr + myTotal - 1).toString() + ((fr + myTotal - 1) % 7).toString());
                }
            }

            else {
                errors.push("Total Field can't be zero");
            }

            if (this.From.length == 8) {
                var checkDigit1: number = +this.From.substr(7, 1);
                var ck: number = fr % 7;

                if (checkDigit1 != ck) {
                    errors.push("From Field check digit is invalid it should be: " + ck);
                }

                else {
                    if ((fr < start) || (fr + myTotal - 1) > end) {
                        errors.push("Selected series is out of available range");
                    }

                    else {
                        this.To = ((fr + myTotal - 1).toString() + ((fr + myTotal - 1) % 7).toString());
                    }
                }
            }
        }

        this.ValidationErrorsList = errors;
    }
}
