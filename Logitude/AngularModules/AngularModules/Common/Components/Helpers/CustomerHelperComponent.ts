import {Component, OnDestroy} from '@angular/core';
import {CustomerPM} from '../../EntityPMs/CustomerPM';
import {CustomerSalesNotePM} from '../../EntityPMs/CustomerSalesNotePM';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: "./CustomerHelperComponent.html",
})

export class CustomerHelperComponent implements OnDestroy {
    public EntityPM: CustomerPM;
    public IsEditingEnabled: boolean = false;
    public IsSalesNotesVisible: boolean = false;
    public IsSupportNotesVisible: boolean = false;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM) {
            this.Listen();
            this.BuildComponent();
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private BuildComponent() {
        var isSalesNotesVisible = false;
        var isSupportNotesVisible = false;

        if (FeatureLocator.HasFeaturePermession("Customer", "SALES")) {
            isSalesNotesVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "TICKET")) {
            isSupportNotesVisible = true;
        }

        this.IsSalesNotesVisible = isSalesNotesVisible;
        this.IsSupportNotesVisible = isSupportNotesVisible;
    }

    get SupportNotes() { return this.EntityPM.SupportNotes; }
    set SupportNotes(value: string) {
        if (this.EntityPM.SupportNotes != value) {
            this.EntityPM.SupportNotes = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    AddSalesNoteClicked() {
        var itemPM: CustomerSalesNotePM = new CustomerSalesNotePM(null);

        var maxDate: Date = null;
        var nowDate: Date = DateTool.GetCurrentDateTimeAsUtc();

        var maxDateTicks: number = 0;
        var nowDateTicks: number = DateTool.GetDateParts(nowDate).DateTicks;

        if (this.EntityPM.SalesNotes.length > 0) {
            this.EntityPM.SalesNotes.forEach(item => {
                var itemDateTicks: number = DateTool.GetDateParts(item.UpdateDate).DateTicks;

                if (itemDateTicks > maxDateTicks) {
                    maxDateTicks = itemDateTicks;
                    maxDate = item.UpdateDate;
                }
            });
        }

        if (maxDate == null) {
            maxDate = nowDate;
        }

        else if (maxDateTicks < nowDateTicks) {
            maxDate = nowDate;
        }

        else {
            maxDate = DateTool.AddHours(maxDate, 1);
        }

        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.CustomerId = this.EntityPM.Id;
        itemPM.CreateDate = maxDate;
        itemPM.UpdateDate = maxDate;
        itemPM.CreatedByUserId = SessionLocator.LoggedUserId;
        itemPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        itemPM.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        itemPM.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;

        this.RunAddEditSalesNoteWindow(itemPM, true);
    }
    EditSalesNoteClicked(itemPM: CustomerSalesNotePM) {
        if (itemPM) {
            this.RunAddEditSalesNoteWindow(itemPM, false);
        }
    }
    RunAddEditSalesNoteWindow(itemPM: CustomerSalesNotePM, isNewEntity: boolean) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Height = 320;
        logitudeWindow.Width = 450;
        logitudeWindow.Title = isNewEntity ? "New Sales Note" : "Edit Sales Note";
        logitudeWindow.HelpText = "The maximum number of characters allowed in this field is 500.";
        logitudeWindow.ShowHelpIcon = true;
        logitudeWindow.WindowArgs = { CustomerPM: this.EntityPM, EntityPM: itemPM, IsNewEntity: isNewEntity };
        logitudeWindow.Show('./Common/Components/Helpers/AddEditCustomerSalesNoteComponent');
    }
}