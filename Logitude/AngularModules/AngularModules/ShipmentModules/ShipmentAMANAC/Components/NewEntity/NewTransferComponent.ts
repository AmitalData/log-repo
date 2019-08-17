import { Component } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { AccountingTransferHeaderPM } from '../../../../Invoice/EntityPMs/AccountingTransferHeaderPM';
import { AccountingTransferLinePM } from '../../../../Invoice/EntityPMs/AccountingTransferLinePM';

@Component({
    moduleId: module.id,
    templateUrl: './NewTransferComponent.html',
})

export class NewTransferComponent extends BaseComponent {
    public EntityPM: AccountingTransferHeaderPM = null;
    public ObjectTableName: string = "AccountingTransferHeader";
    public DataContext = this;
    public TransferTypeCode: string = null;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: NewTransferLine[] = [];
    public SelectedItem: NewTransferLine = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new AccountingTransferHeaderPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.UserId = SessionLocator.LoggedUserId;
        this.EntityPM.TransferDate = DateTool.GetCurrentDateTimeAsUtc();
        this.Listen();
    }

    private Listen() {
        this.CurrentSession.SessionEvent.subscribe(s => {


        });
    }

    SetWindowArgs(transferTypeCode: string) {
        this.TransferTypeCode = transferTypeCode;
        this.EntityPM.AccountingTransferTypeCode = transferTypeCode;

    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.LoadData();
        }
    }

    private toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
            this.LoadData();
        }
    }

    private searchText: string = null;
    get SearchText() { return this.searchText; }
    set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.LoadData();
        }
    }

    LoadData() {

    }

    public SelectedCount: number = 0;
    public ExportButtonIsEnabled: boolean = false;
    public IsFirstTimeLoading: boolean = true;
    OnLinesSelected() {
        this.SelectedCount = this.ItemsSource.filter(f => f.IsChecked == true).length;
        this.ExportButtonIsEnabled = this.SelectedCount > 0 ? true : false;
    }

    ExportButtonClicked() {
        var errors: string[] = [];
        

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
           
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class NewTransferLine {
    constructor(private fatherComponent: NewTransferComponent) {
        if (fatherComponent.IsFirstTimeLoading) {
            this.isChecked = true;
        }
    }

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
           
        }
    }
}
