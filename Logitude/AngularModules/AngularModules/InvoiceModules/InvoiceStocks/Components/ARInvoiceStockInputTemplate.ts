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
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { InvoiceStockInputArgs } from '../../../Invoice/Args';
import { DateTool } from '../../../Infrastructure/Tools';
import { InvoiceDomainService } from '../../../Invoice/Services/InvoiceDomainService';

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
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.stockPMService = new ARInvoiceStockPMService();
    }

    public InitTemplate(args: InvoiceStockInputArgs) {
        if (args != null) {
            this.EntityPM = args.Stock;            
            this.IsEditMode = args.IsEditMode;
        }

        this.FillStockLines();
    }

    public IsNew: boolean;
    SetWindowArgs(args: InvoiceStockInputArgs) {
        if (args != null) {
            this.EntityPM = args.Stock;
            this.IsEditMode = args.IsEditMode;
        }

        this.FillStockLines();
    }

    FillStockLines() {
        this.ItemsSource = [];
        this.SelectedItem = null;

        this.EntityPM.ARInvoiceStockLines.forEach(item => {
            this.ItemsSource.push(item);
        });

        this.ItemsCount = this.ItemsSource.length;        
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
        logWindow.WindowArgs = { Stock: this.EntityPM };
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewARInvoiceStockLinesComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.FillStockLines();
            }
        });
    }

    RemoveStockLineClicked() {
        this.Remove(false);
    }

    RemoveSeriesClicked() {
        this.Remove(true);
    }

    Remove(isDeletingSeries: boolean) {
        if (this.SelectedItem != null) {

            if (isDeletingSeries == true && this.SelectedItem.CreateDate == null) {
                var win = new MessageWindow();
                win.Show("This stock does not have a create date");
            }

            else {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.Title = "Removing Stock";
                confirmWindow.YesButtonText = "Delete";
                confirmWindow.NoButtonText = "Cancel";

                var message = "Are you sure you want to delete the stock number";

                if (isDeletingSeries) {
                    var myGetDateFormats = DateTool.GetDateFormats(this.SelectedItem.CreateDate);
                    message = "Are you sure you want to delete the series inserted on";
                    message = message + "\n" + "[" + myGetDateFormats.ShortDateString + "] at [" + myGetDateFormats.ShortTimeString + "] ?";
                }

                else {
                    message = message + " [" + this.SelectedItem.Number + "] ?";
                }

                confirmWindow.Show(message);

                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        if (isDeletingSeries) {
                            var deletedSeries: ARInvoiceStockLinePM[] = this.EntityPM.ARInvoiceStockLines.filter(d => d.CreateDate == this.SelectedItem.CreateDate);
                            if (deletedSeries.length > 0) {
                                deletedSeries.forEach(item => {
                                    this.EntityPM.RemoveARInvoiceStockLinePM(item);
                                });
                            }
                        }

                        else {
                            this.EntityPM.RemoveARInvoiceStockLinePM(this.SelectedItem);
                        }

                        this.FillStockLines();

                        this.EntityPM.Amount = this.ItemsCount;
                        this.EntityPM.Remaining = this.ItemsCount;
                    }
                });
            }
        }
    }

    CancelClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
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
            this.EntityPM.Amount = this.ItemsCount;
            this.EntityPM.Remaining = this.ItemsCount;

            if (this.IsNew) {
                this.CurrentSession.StartBusyIndicatorSaving();

                this.stockPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (!myResponse.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }
        }
    }
}
