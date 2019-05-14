import { Component, OnDestroy } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ARInvoiceStockPM } from '../../../Invoice/EntityPMs/ARInvoiceStockPM';
import { ARInvoiceStockLinePM } from '../../../Invoice/EntityPMs/ARInvoiceStockLinePM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { InvoiceStockInputArgs } from '../../../Invoice/Args';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceStockInputTemplate.html',
})

export class ARInvoiceStockInputTemplate extends BaseComponent implements OnDestroy {
    public DataContext: ARInvoiceStockInputTemplate = this;
    public ObjectTableName: string = "ARInvoiceStock";
    public ValidationErrorsList: string[] = [];
    public EntityPM: ARInvoiceStockPM;
    public ItemsSource: ARInvoiceStockLinePM[] = [];
    public ItemsCount: number;
    public IsEditMode: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();

        this.Listen();
    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "RefreshARInvoiceStockScreen") {
                this.SetUIProperties();
            }
        });

        if (this.entityArgs != null && this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    this.SetUIProperties();
                    this.FillStockLines();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    this.SetUIProperties();
                    this.FillStockLines();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public InitTemplate(args: InvoiceStockInputArgs) {
        if (args != null) {
            this.EntityPM = args.Stock;
            this.IsEditMode = args.IsEditMode;
        }

        this.SetUIProperties();
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

    private IsEditingEnabled: boolean = true;
    SetUIProperties() {
        var isEditingEnabled = true;

        if (this.EntityPM.StatusCode == "C") {
            isEditingEnabled = false;
        }

        this.IsEditingEnabled = isEditingEnabled;

        this.UIProperties.SetEnabled("Name", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("StartDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("EndDate", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isEditingEnabled);
    }

    FillStockLines() {
        this.ItemsSource = [];
        this.SelectedItem = null;

        this.EntityPM.ARInvoiceStockLines.forEach(item => {
            this.ItemsSource.push(item);
        });

        this.ItemsCount = this.EntityPM.ARInvoiceStockLines.filter(d => !d.IsUsed).length;
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

    get IsAddEnabled() {
        var myResult = false;

        if (this.IsEditingEnabled) {
            myResult = true;
        }

        return myResult;
    }

    get IsRemoveEnabled() {
        var myResult = false;

        if (this.IsEditingEnabled) {
            if (this.SelectedItem != null && !this.SelectedItem.IsUsed) {
                myResult = true;
            }
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
                this.IsNew = false;
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
            var win = new MessageWindow();
            var usedSeries: ARInvoiceStockLinePM[] = this.EntityPM.ARInvoiceStockLines.filter(d => d.CreateDate == this.SelectedItem.CreateDate && d.IsUsed);

            if (isDeletingSeries && this.SelectedItem.CreateDate == null) {
                win.Show("This stock does not have a create date");
            }

            else if (isDeletingSeries && usedSeries.length > 0) {
                win.Show("You can't remove this series, since it contains at least one used number");
            }

            else {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.Title = "Removing Stock";
                confirmWindow.YesButtonText = "Delete";
                confirmWindow.NoButtonText = "Cancel";

                var message = "Are you sure you want to delete the stock number";
                var removeNotes = "";

                if (isDeletingSeries) {
                    var myGetDateFormats = DateTool.GetDateFormats(this.SelectedItem.CreateDate);
                    message = "Are you sure you want to delete the series inserted on";
                    message = message + "\n" + "[" + myGetDateFormats.ShortDateString + "] at [" + myGetDateFormats.ShortTimeString + "] ?";
                    removeNotes = "Invoice numbers inserted on [" + myGetDateFormats.ShortDateString + "] at [" + myGetDateFormats.ShortTimeString + "] removed";
                }

                else {
                    message = message + " [" + this.SelectedItem.Number + "] ?";
                    removeNotes = "Invoice  number [" + this.SelectedItem.Number + "] removed";
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

                                this.EntityPM.SeriesRemoved = true;
                                this.EntityPM.EventNotes = removeNotes;
                            }
                        }

                        else {
                            this.EntityPM.RemoveARInvoiceStockLinePM(this.SelectedItem);
                            this.EntityPM.NumberRemoved = true;
                            this.EntityPM.EventNotes = removeNotes;
                        }

                        this.FillStockLines();
                    }
                });
            }
        }
    }
}
