import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ARInvoiceStockPM } from '../../../../Invoice/EntityPMs/ARInvoiceStockPM';
import { ARInvoiceStockLinePM } from '../../../../Invoice/EntityPMs/ARInvoiceStockLinePM';
import { AppTool, FormatTool, DateTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ARInvoiceStockPMService } from '../../../../Invoice/Services/StandardPMs/ARInvoiceStockPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './NewARInvoiceStockLinesComponent.html',
})

export class NewARInvoiceStockLinesComponent extends BaseComponent {
    public DataContext: NewARInvoiceStockLinesComponent = this;
    public ObjectTableName: string = "ARInvoiceStockLine";
    public ValidationErrorsList: string[] = [];
    public Stock: ARInvoiceStockPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public ItemsSource: ARInvoiceStockLinePM[] = [];
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
        else if (!FormatTool.IsNumeric(this.StartNumber)) {
            var msgStartNumbe = "Invalid StartNumber: should be digits";
            errors.push(msgStartNumbe);
            this.UIProperties.SetValidity("StartNumber", null, false, msgStartNumbe);
        }

        // End Number
        if (this.IsByNumber) {
            if (AppTool.IsNullOrEmpty(this.EndNumber)) {
                this.UIProperties.SetRequired("EndNumber", null, true);
            }
            else if (!FormatTool.IsNumeric(this.EndNumber)) {
                var msgEndNumber = "Invalid End Number: should be digits";
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

    public SelectedItem: any = null;
    private RunGenerator() {
        this.ItemsSource = [];
        this.GenerateErrors = [];
        this.SelectedItem = null;

        if (this.IsByNumber) {
            if (!AppTool.IsNullOrEmpty(this.StartNumber) && !AppTool.IsNullOrEmpty(this.EndNumber)) {
                if (FormatTool.IsNumeric(this.StartNumber) && FormatTool.IsNumeric(this.EndNumber)) {
                    var NumericStartNumber = +this.StartNumber;
                    var NumericEndNumber = +this.EndNumber;
                    var NumericAmount = NumericEndNumber - NumericStartNumber + 1;
                    this.amount = NumericAmount.toString();
                    this.GenerateList(NumericStartNumber, NumericEndNumber, NumericAmount);
                }
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.StartNumber) && !AppTool.IsNullOrEmpty(this.Amount)) {
                if (FormatTool.IsNumeric(this.StartNumber) && FormatTool.IsNumeric(this.Amount)) {
                    var NumericStartNumber = +this.StartNumber;
                    var NumericAmount = +this.Amount;
                    var NumericEndNumber = NumericStartNumber + NumericAmount - 1;
                    this.endNumber = NumericEndNumber.toString();
                    this.GenerateList(NumericStartNumber, NumericEndNumber, NumericAmount);
                }
            }
        }
    }

    public IsListGenerated: boolean = false;
    private GenerateErrors: string[] = [];
    GenerateList(myStartNumber: number, myEndNumber: number, myAmount) {
        this.IsListGenerated = false;
        var StocksListCount = this.Stock.ARInvoiceStockLines == null ? 0 : this.Stock.ARInvoiceStockLines.length;

        if (myEndNumber < myStartNumber) {
            this.GenerateErrors.push("End number must be greater than start number");
        }

        else if (myAmount > 1000) {
            this.GenerateErrors.push("Maximum number of added stacks in one transaction is 1000");
        }

        else if ((myAmount > 10000) || ((StocksListCount + myAmount) > 10000)) {
            this.GenerateErrors.push("Maximum number of generated stacks is 10000");
        }

        else if (!AppTool.IsNullOrZero(this.Size)) {
            if (myStartNumber.toString().length > this.Size) {
                this.GenerateErrors.push("Start Number should be " + this.Size + " digits maximum");
            }

            else if (myEndNumber.toString().length > this.Size) {
                this.GenerateErrors.push("End Number should be " + this.Size + " digits maximum");
            }

            else {
                this.StartGenerating(myStartNumber, myEndNumber);
            }
        }

        else {
            this.StartGenerating(myStartNumber, myEndNumber);
        }

        this.GenerateErrors.forEach(item => {
            this.ValidationErrorsList.push(item);
        });
    }

    private StartGenerating(myStartNumber: number, myEndNumber: number) {
        var todayDate = DateTool.GetCurrentDateAsUtc();

        while (myStartNumber <= myEndNumber) {
            this.IsListGenerated = true;
            var paddingStartNumber: string = myStartNumber.toString();

            if (!AppTool.IsNullOrZero(this.Size)) {
                if (myStartNumber.toString().length < this.Size) {
                    paddingStartNumber = myStartNumber.toString().padStart(this.Size, "0");
                }
            }

            var invoiceNumber: string = paddingStartNumber.toString();

            if (!AppTool.IsNullOrEmpty(this.Prefix) && !AppTool.IsNullOrEmpty(this.Suffix)) {
                invoiceNumber = this.Prefix + paddingStartNumber.toString() + this.Suffix;
            }

            else if (!AppTool.IsNullOrEmpty(this.Prefix) && AppTool.IsNullOrEmpty(this.Suffix)) {
                invoiceNumber = this.Prefix + paddingStartNumber.toString();
            }

            else if (AppTool.IsNullOrEmpty(this.Prefix) && !AppTool.IsNullOrEmpty(this.Suffix)) {
                invoiceNumber = paddingStartNumber.toString() + this.Suffix;
            }

            var newEntityPM = new ARInvoiceStockLinePM(this.Stock);
            newEntityPM.Tenant = SessionLocator.Tenant;
            newEntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            newEntityPM.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
            newEntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            newEntityPM.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
            newEntityPM.CreateDate = todayDate;
            newEntityPM.UpdateDate = todayDate;
            newEntityPM.Number = invoiceNumber;

            if (this.Stock.ARInvoiceStockLines.filter(f => f.Number == invoiceNumber)[0] == null) {
                this.ItemsSource.push(newEntityPM);
            }

            else {
                this.GenerateErrors.push("Invoice Number: [" + invoiceNumber + "] already exists in the stock!");
                myStartNumber = myEndNumber + 1;
            }

            myStartNumber++;
        }
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private isOkButtonClicked: boolean = false;
    OkButtonClicked() {

        this.isOkButtonClicked = true;

        this.Validate();

        this.GenerateErrors.forEach(item => {
            this.ValidationErrorsList.push(item);
        });

        if (this.ValidationErrorsList.length == 0) {
            if (this.IsListGenerated) {
                this.CurrentSession.StartBusyIndicatorSaving();
                
                this.ItemsSource.forEach(item => {
                    this.Stock.AddARInvoiceStockLinePM(item);
                });

                this.Stock.NumbersAdded = true;
                this.Stock.Amount = this.Stock.ARInvoiceStockLines.length;
                this.Stock.EventNotes = "Invoice numbers from [" + this.StartNumber + "] to [" + this.EndNumber + "] added";

                var stockPMService: ARInvoiceStockPMService = new ARInvoiceStockPMService();
                if (AppTool.IsNullOrEmpty(this.Stock.Id)) {
                    stockPMService.insert(this.Stock).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (!myResponse.HasError) {
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }

                        else {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                    });
                }

                else {
                    stockPMService.update(this.Stock).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (!myResponse.HasError) {
                            if (this.CurrentSession.CurrentEditComponent != null) {
                                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            }

                            this.CurrentSession.CloseCurrentWindow();
                        }

                        else {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                    });
                }
            }
        }
    }
}
