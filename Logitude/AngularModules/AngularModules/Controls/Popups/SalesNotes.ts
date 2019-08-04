import {Component, OnInit, Output, EventEmitter, OnDestroy} from '@angular/core';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {EntityArgs} from '../../Infrastructure/DataContracts/EntityArgs';
import {CustomerPM} from '../../Common/EntityPMs/CustomerPM';
import {CustomerSalesNotePM} from '../../Common/EntityPMs/CustomerSalesNotePM';
import {ConfirmWindow} from '../Windows/ConfirmWindow';

@Component({
    selector: "SalesNotes",
    moduleId: module.id,
    templateUrl: './SalesNotes.html',
    inputs: ['Title', 'IconCode', 'IsEnabled', 'EntityPM', 'CustomerPM', 'IsFromQuote'],
})

export class SalesNotes implements OnInit, OnDestroy {
    public ComponentId: string = null;
    public ComponentButtonId: string = null;
    public ComponentContentId: string = null;
    public Width: number = 250;
    public Height: number = 130;
    public MinHeight: number = 130;
    public MaxHeight: number = 350;
    public Title: string = "Notes";
    public EntityPM: CustomerPM;
    public customerPM: CustomerPM;

    get CustomerPM() {
        return this.customerPM;
    }
    set CustomerPM(value: CustomerPM) {
        if (this.customerPM != value) {
            this.customerPM = value;
            this.UpdateComponent();
        }
    }

    public IsEnabled: boolean = true;
    public IsFromQuote: boolean = false;
    public IconCode: string;
    public IconPath: string;
    public IconOpacity: number = 1;
    public IsEditingEnabled: boolean = true;
    public NotesList: SalesNoteItem[] = [];
    @Output() OnAddButtonClicked: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() OnEditButtonClicked: EventEmitter<CustomerSalesNotePM> = new EventEmitter<CustomerSalesNotePM>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        var idIndex = this.CurrentSession.GetNewId("SalesNotes");
        this.ComponentId = "SalesNotes_" + idIndex;
        this.ComponentButtonId = "SalesNotesButton_" + idIndex;
        this.ComponentContentId = "SalesNotesContent_" + idIndex;
    }

    ngOnInit() {
        this.UpdateComponent();
        this.Listen();
    }

    private SalesNotesChangedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent) {
            if (!this.SalesNotesChangedEvent) {
                this.SalesNotesChangedEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                    switch (s) {
                        case "CustomerSalesNotesChanged":
                            {
                                this.UpdateComponent();
                                break;
                            }
                    }
                });
            }

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                        this.UpdateComponent();
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                        this.UpdateComponent();
                    }
                });
            }
        }
    }
    UpdateComponent() {
        this.BuildNotesList();
        this.SetPopupHeight();
        this.SetIconPath();
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SalesNotesChangedEvent);
        this.SalesNotesChangedEvent = null;
        this.StopPositionTimer();
    }

    SetIconPath() {
        var file: string = "Gray.png";

        switch (this.IconCode) {
            case "G": {
                file = "Green.png";
                this.IconOpacity = this.NotesList.length == 0 ? 0.5 : 1;
                break;
            }

            case "B": {
                file = "Blue.png";
                this.IconOpacity = this.NotesList.length == 0 ? 0.5 : 1;
                break;
            }

            default: {
                file = this.NotesList.length == 0 ? "Gray.png" : "Orange.png";
                this.IconOpacity = 1;
                break;
            }
        }

        var iconPath = "./_Resources/Images/Icons/Notes/" + file;
        this.IconPath = "url(" + iconPath + ")";
    }
    SetPopupHeight() {
        var myResult: number = 100;

        if (this.NotesList) {
            if (this.NotesList.length > 0) {
                var itemHeight = 80;
                var itemsCount = this.NotesList.length;

                if (itemsCount <= 3) {
                    myResult = itemsCount * itemHeight;
                    myResult += 10;
                    myResult += 25;
                }

                else {
                    myResult = 360;
                }
            }
        }

        this.Height = myResult;
    }

    private isOpened: boolean = false;
    get IsOpened() { return this.isOpened; }
    set IsOpened(value: boolean) {
        if (value != undefined) {
            if (this.isOpened != value) {
                this.isOpened = value;

                if (value) {
                    this.SetPopupHeight();
                    this.RunPositionTimer();

                    if (document.getElementById(this.ComponentContentId).style.visibility != "visible") {
                        document.getElementById(this.ComponentContentId).style.visibility = "visible";
                    }
                }

                else {
                    this.StopPositionTimer();

                    if (document.getElementById(this.ComponentContentId).style.visibility != "hidden") {
                        document.getElementById(this.ComponentContentId).style.visibility = "hidden";
                    }
                }
            }
        }
    }

    private timerToken: any;
    private StopPositionTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }
    private RunPositionTimer() {
        this.StopPositionTimer();
        this.timerToken = setInterval(() => this.CalculateFixedPosition(), 0);
    }
    private CalculateFixedPosition() {
        var item = document.getElementById(this.ComponentId);
        if (item) {
            var itemRect = item.getBoundingClientRect();

            document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 24) + 'px';
            document.getElementById(this.ComponentContentId).style.left = (itemRect.left + 24 - this.Width) + 'px';
        }
    }

    public IsMouseOver: boolean = false;
    public IsMouseOverButton: boolean = false;
    OnButtonClicked() {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }

        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
        }
    }
    OnButtonLostFocus() {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver) {
                document.getElementById(this.ComponentButtonId).focus();
            }

            else {
                this.IsOpened = false;
            }
        }
    }
    BuildNotesList() {
        this.NotesList = [];

        var list: SalesNoteItem[] = [];

        if (this.EntityPM && this.EntityPM.SalesNotes != null) {
            this.EntityPM.SalesNotes.forEach((item: CustomerSalesNotePM) => {
                list.push(new SalesNoteItem(item));
            });
        }

        if (this.CustomerPM && this.CustomerPM.SalesNotes != null) {
            this.CustomerPM.SalesNotes.forEach((item: CustomerSalesNotePM) => {
                list.push(new SalesNoteItem(item));
            });
        }

        list.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? 1 : -1 }).forEach(item => {
            this.NotesList.push(item);
        });
    }

    AddButtonClicked() {
        this.IsOpened = false;
        this.OnAddButtonClicked.emit(true);
    }
    EditButtonClicked(itemClass: SalesNoteItem) {
        if (itemClass) {
            if (itemClass.EntityPM) {
                this.IsOpened = false;
                this.OnEditButtonClicked.emit(itemClass.EntityPM);
            }
        }
    }
    DeleteButtonClicked(itemClass: SalesNoteItem) {
        if (itemClass) {
            if (itemClass.EntityPM) {

                this.IsOpened = false;

                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("Delete this note?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        this.EntityPM.RemoveCustomerSalesNotePM(itemClass.EntityPM);
                        this.UpdateComponent();
                    }
                });
            }
        }
    }
}
export class SalesNoteItem {
    public EntityPM: CustomerSalesNotePM;
    public SortingValue: number = 0;
    public Notes: string;
    public UpdateLabel: string;
    public UpdateByUser: string;
    public UpdateDateString: string;
    public UpdateLabelWidth: number;
    public UpdateByUserWidth: number;
    public UpdateDateStringWidth: number;
    public IsButtonsHidden: boolean = true;
    constructor(item: CustomerSalesNotePM) {
        this.EntityPM = item;
        this.SortingValue = DateTool.GetDateParts(item.UpdateDate).DateTicks;
        this.Notes = item.Notes;

        this.UpdateLabel = item.CreateDate == item.UpdateDate ? "Created by" : "Modified by";
        this.UpdateByUser = item.UpdatedByUserName;
        this.UpdateDateString = DateTool.GetDateFormats(item.UpdateDate).DateString;

        var width: number = 250 - 10;
        this.UpdateLabelWidth = AppTool.GetTextWidth(this.UpdateLabel, 10) + 5;
        this.UpdateDateStringWidth = AppTool.GetTextWidth(this.UpdateDateString, 10) + 10;

        var updateByUserWidth: number = AppTool.GetTextWidth(this.UpdateByUser, 10) + 5;
        var emptySpaceWidth: number = width - (this.UpdateLabelWidth + this.UpdateDateStringWidth);

        this.UpdateByUserWidth = emptySpaceWidth < updateByUserWidth ? emptySpaceWidth : updateByUserWidth;

        //if (emptySpaceWidth < updatedByUserWidth) {
        //    this.UpdateByUserWidth = emptySpaceWidth;
        //}

        //else {
        //    this.UpdateByUserWidth = updatedByUserWidth;
        //}
    }
}
