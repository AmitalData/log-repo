import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {MAWBStackPM} from '../../../EntityPMs/MAWBStackPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AWBStackDomainService, StockSeriesListClass, StockSeries} from '../../../Services/AWBStackDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './NewStackComponent.html',
})

export class NewStackComponent extends BaseComponent {
    public IsCustomerMode: boolean = false;
    public DataContext: NewStackComponent = this;
    public ObjectTableName: string = "MAWBStack";
    public ValidationErrorsList: string[] = [];
    public ItemsSource: MAWBStackPM[] = [];
    public AirlineStacksList: MAWBStackPM[] = [];    
    private StackDomainService: AWBStackDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.StackDomainService = new AWBStackDomainService();
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.AirlineId = args['AirlineId'];
            this.CustomerId = args['CustomerId'];
            this.IsCustomerMode = args['IsCustomerMode'];
            this.AirlineStacksList = args['AirlineStacksList'];
            this.SetUIProperties();
        }
    }

    private SetUIProperties() {
        this.UIProperties.SetRequired("AirlineId", null, AppTool.IsNullOrEmpty(this.AirlineId) ? true : false);
        this.UIProperties.SetRequired("StartNumber", null, AppTool.IsNullOrEmpty(this.StartNumber) ? true : false);
        this.UIProperties.SetRequired("EndNumber", null, this.IsByNumber && AppTool.IsNullOrEmpty(this.EndNumber) ? true : false);
        this.UIProperties.SetRequired("Amount", null, !this.IsByNumber && AppTool.IsNullOrEmpty(this.Amount) ? true : false);
        this.UIProperties.SetEnabled("EndNumber", null, this.IsByNumber);
        this.UIProperties.SetEnabled("Amount", null, !this.IsByNumber);
    }

    private airlineId: string = null;
    get AirlineId() { return this.airlineId; }
    set AirlineId(newValue: string) {
        if (this.airlineId != newValue) {
            this.airlineId = newValue;
            this.UIProperties.SetRequired("AirlineId", null, AppTool.IsNullOrEmpty(newValue) ? true : false);
        }
    }

    private customerId: string = null;
    get CustomerId() { return this.customerId; }
    set CustomerId(newValue: string) {
        if (this.customerId != newValue) {
            this.customerId = newValue;
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

                this.StackDomainService.CreateMAWBStacksOperation(this.AirlineId, this.myStartNumber, this.myEndNumber, this.CustomerId).subscribe((myResponse: ServiceResponse) => {

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
    }

    Validate() {

        var errors: string[] = [];
        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.isOkButtonClicked) {
            if (AppTool.IsNullOrEmpty(this.AirlineId)) {
                errors.push(msg.replace("%FieldName", "Airline"));
            }

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

    public SelectedItem: any = null;
    RunGenerator() {
        this.ItemsSource = [];
        this.GenerateErrors = [];
        this.SelectedItem = null;

        if (this.IsByNumber) {
            if (!AppTool.IsNullOrEmpty(this.StartNumber) && !AppTool.IsNullOrEmpty(this.EndNumber)) {
                if (FormatTool.IsNumeric(this.StartNumber) && FormatTool.IsNumeric(this.EndNumber)) {
                    if (this.StartNumber.length == 7 && this.EndNumber.length == 7) {
                        var NumericStartNumber = +this.StartNumber;
                        var NumericEndNumber = +this.EndNumber;
                        var NumericAmount = NumericEndNumber - NumericStartNumber + 1;
                        this.amount = NumericAmount.toString();
                        this.GenerateList(NumericStartNumber, NumericEndNumber, NumericAmount);
                    }
                }
            }
        }

        else {
            if (!AppTool.IsNullOrEmpty(this.StartNumber) && !AppTool.IsNullOrEmpty(this.Amount)) {
                if (FormatTool.IsNumeric(this.StartNumber) && FormatTool.IsNumeric(this.Amount)) {
                    if (this.StartNumber.length == 7) {
                        var NumericStartNumber = +this.StartNumber;
                        var NumericAmount = +this.Amount;
                        var NumericEndNumber = NumericStartNumber + NumericAmount - 1;
                        this.endNumber = NumericEndNumber.toString();
                        this.GenerateList(NumericStartNumber, NumericEndNumber, NumericAmount);
                    }
                }
            }
        }
    }

    public IsListGenerated: boolean = false;
    private GenerateErrors: string[] = [];
    private myStartNumber: number = null;
    private myEndNumber: number = null;
    GenerateList(myStartNumber: number, myEndNumber: number, myAmount) {

        this.IsListGenerated = false;
        this.myStartNumber = myStartNumber;
        this.myEndNumber = myEndNumber;

        var AirlineStacksListCount = this.AirlineStacksList == null ? 0 : this.AirlineStacksList.length;

        if (myEndNumber < myStartNumber) {
            this.GenerateErrors.push("End number must be greater than start number");
        }

        else if (myAmount > 1000) {
            this.GenerateErrors.push("Maximum number of added stacks in one transaction is 1000");
        }

        else if ((myAmount > 10000) || ((AirlineStacksListCount + myAmount) > 10000)) {
            this.GenerateErrors.push("Maximum number of generated stacks is 10000");
        }

        else {
            while (myStartNumber <= myEndNumber) {
                this.IsListGenerated = true;
                var chk = myStartNumber % 7;

                var newNumberStr: string = "" + myStartNumber.toString() + chk;
                var newNumber: number = +newNumberStr;

                var newEntityPM = new MAWBStackPM();
                newEntityPM.AirlineId = this.AirlineId;
                newEntityPM.Tenant = SessionLocator.Tenant;
                newEntityPM.Number = newNumber;
                newEntityPM.AssignedToId = this.CustomerId;

                if (this.ItemsSource.filter(f => f.Number == newNumber)[0] == null) {
                    this.ItemsSource.push(newEntityPM);
                }

                else {
                    this.GenerateErrors.push("Air WayBill Number: " + newNumber + " already exists in the stack!");
                    myStartNumber = myEndNumber + 1;
                }

                myStartNumber++;
            }
        }

        this.GenerateErrors.forEach(item => {
            this.ValidationErrorsList.push(item);
        });
    }
}
