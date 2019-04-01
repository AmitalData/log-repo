import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ARInvoiceStockPM } from '../../../Invoice/EntityPMs/ARInvoiceStockPM';
import { ARInvoiceStockLinePM } from '../../../Invoice/EntityPMs/ARInvoiceStockLinePM';
import { ARInvoiceStockPMService } from '../../../Invoice/Services/StandardPMs/ARInvoiceStockPMService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { InvoiceStockInputArgs } from '../../../Invoice/Args';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceStockInputTemplate.html',
})

export class ARInvoiceStockInputTemplate extends BaseComponent {
    public DataContext: ARInvoiceStockInputTemplate = this;
    public ObjectTableName: string = "ARInvoiceStock";
    public ValidationErrorsList: string[] = [];
    public EntityPM: ARInvoiceStockPM;
    private stockPMService: ARInvoiceStockPMService;
    public ItemsSource: ARInvoiceStockLinePM[] = [];
    public ItemsCount: number;
    public IsEditMode: boolean = false;
    constructor() {
        super();

        this.stockPMService = new ARInvoiceStockPMService();
    }

    public InitTemplate(args: InvoiceStockInputArgs) {
        if (args != null) {
            this.EntityPM = args.Stock;            
            this.IsEditMode = args.IsEditMode;
        }

        this.InitializeData();
    }

    public IsNew: boolean;
    SetWindowArgs(args: InvoiceStockInputArgs) {
        if (args != null) {
            this.EntityPM = args.Stock;
            this.IsEditMode = args.IsEditMode;
        }

        this.InitializeData();
    }

    InitializeData() {
       
    }

    get Name() { return this.EntityPM.Name; }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get StartDate() { return this.EntityPM.StartDate; }
    set StartDate(newValue: Date) {
        if (this.EntityPM.StartDate != newValue) {
            this.EntityPM.StartDate = newValue;
        }
    }

    get EndDate() { return this.EntityPM.EndDate; }
    set EndDate(newValue: Date) {
        if (this.EntityPM.EndDate != newValue) {
            this.EntityPM.EndDate = newValue;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(newValue: string) {
        if (this.EntityPM.Notes != newValue) {
            this.EntityPM.Notes = newValue;
        }
    }

    public SelectedItem: ARInvoiceStockLinePM = null;

    get IsRemoveEnabled() {
        var myResult = false;

        if (this.SelectedItem != null) {
            myResult = true;
        }

        return myResult;
    }

    AddStockLineClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Stock Numbers";
        logWindow.WindowArgs = { FBLStocksList: this.ItemsSource };
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewArInvoiceStockLinesComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {

            }
        });
    }

    RemoveStockLineClicked() {

    }

    RemoveSeriesClicked() {

    }

    CancelClicked() {
        this.RejectChanges();
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('StartDate');
        this.myCloner.AddField('EndDate');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    OkClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.IsNew) {
                SessionLocator.CurrentSession.StartBusyIndicatorSaving();

                this.stockPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                    SessionLocator.CurrentSession.StopBusyIndicator();

                    if (!myResponse.HasError) {
                        SessionLocator.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }
        }
    }
}
