import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ARInvoiceStockPM } from '../../../../Invoice/EntityPMs/ARInvoiceStockPM';
import { ARInvoiceStockLinePM } from '../../../../Invoice/EntityPMs/ARInvoiceStockLinePM';
import { AppTool, FormatTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';


@Component({
    moduleId: module.id,
    templateUrl: './NewARInvoiceStockLinesComponent.html',
})

export class NewARInvoiceStockLinesComponent extends BaseComponent {
    public DataContext: NewARInvoiceStockLinesComponent = this;
    public ObjectTableName: string = "ARInvoiceStockLine";
    public ValidationErrorsList: string[] = [];
    public Stock: ARInvoiceStockPM;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.Stock = args['Stock'];
            this.SetUIProperties();
        }
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("StartNumber", null, AppTool.IsNullOrEmpty(this.StartNumber) ? true : false);
        this.UIProperties.SetRequired("EndNumber", null, this.IsByNumber && AppTool.IsNullOrEmpty(this.EndNumber) ? true : false);
        this.UIProperties.SetRequired("Amount", null, !this.IsByNumber && AppTool.IsNullOrEmpty(this.Amount) ? true : false);
        this.UIProperties.SetEnabled("EndNumber", null, this.IsByNumber);
        this.UIProperties.SetEnabled("Amount", null, !this.IsByNumber);
    }

    private prefix: string = null;
    get Prefix() { return this.prefix; }
    set Prefix(newValue: string) {
        if (this.prefix != newValue) {
            this.prefix = newValue;
            this.Validate();
            this.RunGenerator();
        }
    }

    private suffix: string = null;
    get Suffix() { return this.suffix; }
    set Suffix(newValue: string) {
        if (this.suffix != newValue) {
            this.suffix = newValue;
            this.Validate();
            this.RunGenerator();
        }
    }

    SetIsByNumber(newValue: boolean) {
        this.IsByNumber = newValue;
    }
    private isByNumber: boolean = true;
    get IsByNumber() { return this.isByNumber; }
    set IsByNumber(newValue: boolean) {
        if (this.isByNumber != newValue) {
            this.isByNumber = newValue;
            this.Validate();
            this.RunGenerator();
            this.UIProperties.SetEnabled("EndNumber", null, this.IsByNumber);
            this.UIProperties.SetEnabled("Amount", null, !this.IsByNumber);
        }
    }

    private startNumber: string = null;
    get StartNumber() { return this.startNumber; }
    set StartNumber(newValue: string) {
        if (this.startNumber != newValue) {
            this.startNumber = newValue;
            this.Validate();
            this.RunGenerator();
        }
    }

    private endNumber: string = null;
    get EndNumber() { return this.endNumber; }
    set EndNumber(newValue: string) {
        if (this.endNumber != newValue) {
            this.endNumber = newValue;
            this.Validate();
            this.RunGenerator();
        }
    }

    private size: number = null;
    get Size() { return this.size; }
    set Size(newValue: number) {
        if (this.size != newValue) {
            this.size = newValue;
            this.Validate();
            this.RunGenerator();
        }
    }

    private amount: string = null;
    get Amount() { return this.amount; }
    set Amount(newValue: string) {
        if (this.amount != newValue) {
            this.amount = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.endNumber = null;
            }

            this.Validate();
            this.RunGenerator();
        }
    }

    Validate() {
        var errors: string[] = [];
        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.isOkButtonClicked) {
            if (AppTool.IsNullOrEmpty(this.StartNumber)) {
                errors.push(msg.replace("%FieldName", "Start Number"));
            }

            if (this.IsByNumber) {
                if (AppTool.IsNullOrEmpty(this.EndNumber)) {
                    errors.push(msg.replace("%FieldName", "End Number"));
                }
            }

            else {
                if (AppTool.IsNullOrEmpty(this.Amount)) {
                    errors.push(msg.replace("%FieldName", "Amount"));
                }
            }
        }

        this.UIProperties.SetRequired("StartNumber", null, false);
        this.UIProperties.SetRequired("EndNumber", null, false);
        this.UIProperties.SetRequired("Amount", null, false);
        this.UIProperties.SetValidity("StartNumber", null, true, "");
        this.UIProperties.SetValidity("EndNumber", null, true, "");
        this.UIProperties.SetValidity("Amount", null, true, "");

        // Start Number
        if (AppTool.IsNullOrEmpty(this.StartNumber)) {
            this.UIProperties.SetRequired("StartNumber", null, true);
        }
        else if (!this.StartNumber.match(/^[0-9]{7}$/)) {
            var msgStartNumbe = "Invalid " + TextCodeTranslator.Translate("MAWBStack.O.StartNumber") + ": (should be 7 digits)";
            errors.push(msgStartNumbe);
            this.UIProperties.SetValidity("StartNumber", null, false, msgStartNumbe);
        }

        // End Number
        if (this.IsByNumber) {
            if (AppTool.IsNullOrEmpty(this.EndNumber)) {
                this.UIProperties.SetRequired("EndNumber", null, true);
            }
            else if (!this.EndNumber.match(/^[0-9]{7}$/)) {
                var msgEndNumber = "Invalid " + TextCodeTranslator.Translate("MAWBStack.O.EndNumber") + ": (should be 7 digits)";
                errors.push(msgEndNumber);
                this.UIProperties.SetValidity("EndNumber", null, false, msgEndNumber);
            }
        }

        // Amount
        if (!this.IsByNumber) {
            if (AppTool.IsNullOrEmpty(this.Amount)) {
                this.UIProperties.SetRequired("Amount", null, true);
            }

            else if (!FormatTool.IsNumeric(this.Amount)) {
                var msgAmount = "Invalid Amount: should be digits";
                errors.push(msgAmount);
                this.UIProperties.SetValidity("Amount", null, false, msgAmount);
            }
        }

        this.ValidationErrorsList = errors;
    }

    private RunGenerator() {

    }

    CancelClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    private isOkButtonClicked: boolean = false;
    OkButtonClicked() {

        this.isOkButtonClicked = true;

        this.Validate();

        //    this.GenerateErrors.forEach(item => {
        //        this.ValidationErrorsList.push(item);
        //    });

        //    if (this.ValidationErrorsList.length == 0) {
        //        if (this.IsListGenerated) {

        //            SessionLocator.CurrentSession.StartBusyIndicatorSaving();

        //            this.StackDomainService.CreateMAWBStacksOperation(this.AirlineId, this.myStartNumber, this.myEndNumber, this.CustomerId).subscribe((myResponse: ServiceResponse) => {

        //                SessionLocator.CurrentSession.StopBusyIndicator();

        //                if (myResponse.HasError) {
        //                    this.ValidationErrorsList = myResponse.ErrorsArray;
        //                }

        //                else {
        //                    SessionLocator.CurrentSession.CloseCurrentWindowEmit("OK");
        //                }
        //            });
        //        }
        //    }
    }
}
