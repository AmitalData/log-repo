
import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {AppTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {CRMControlsService, MeetingSummary} from '../Services/CRMControlsService';

@Component({
    selector: "MettingSummary",
    moduleId: module.id,
     templateUrl: './MettingSummary.html',
     inputs: ['IsChecked', 'Text', 'EntityId', 'IsToRight'],
})

export class MettingSummary {
    public ComponentId: string = null;
    public ComponentButtonId: string = null;
    public ComponentContentId: string = null;
    public ComponentTemplateId: string = null;
    public Width: number = 380;
    public Height: number = 280;

    @Output() Updated: EventEmitter<boolean> = new EventEmitter<boolean>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        var idIndex = this.CurrentSession.GetNewId("MettingSummary");
        this.ComponentId = "MettingSummary_" + idIndex;
        this.ComponentButtonId = "MettingSummaryButton_" + idIndex;
        this.ComponentContentId = "MettingSummaryContent_" + idIndex;
        this.ComponentTemplateId = "MettingSummaryTemplate_" + idIndex;
    }

    private text: string = null;
    get Text() { return this.text; }
    set Text(value: string) {
        if (this.text != value) {
            this.text = value;
        }
    }

    private isToRight: boolean = false;
    get IsToRight() { return this.isToRight; }
    set IsToRight(value: boolean) {
        if (this.isToRight != value) {
            this.isToRight = value;
        }
    }

    private entityId: string = null;
    get EntityId() { return this.entityId; }
    set EntityId(value: string) {
        if (this.entityId != value) {
            this.entityId = value;
        }
    }

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
        }
    }
    
    private isOpened: boolean = false;
    get IsOpened() { return this.isOpened; }
    set IsOpened(value: boolean) {
        if (value != undefined) {
            if (this.isOpened != value) {
                this.isOpened = value;

                if (value) {
                    if (document.getElementById(this.ComponentContentId).style.visibility != "visible") {
                        document.getElementById(this.ComponentContentId).style.visibility = "visible";
                    }
                }

                else {
                    if (document.getElementById(this.ComponentContentId).style.visibility != "hidden") {
                        document.getElementById(this.ComponentContentId).style.visibility = "hidden";
                    }
                }
            }
        }
    }

    public IsMouseOver: boolean = false;
    public IsMouseOverButton: boolean = false;
    public IsMouseOverInput: boolean = false;
    OnButtonClicked() {
        if (this.IsOpened) {
            this.IsOpened = false;
        }

        else {
            var item = document.getElementById(this.ComponentId);
            var itemRect = item.getBoundingClientRect();

            if (this.IsToRight) {
                document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 26) + 'px';
                document.getElementById(this.ComponentContentId).style.left = (itemRect.left) + 'px';
            }
            else {
                document.getElementById(this.ComponentContentId).style.top = (itemRect.top + 26) + 'px';
                document.getElementById(this.ComponentContentId).style.left = (itemRect.left +75  - this.Width) + 'px';
            }

            this.IsOpened = true;
        }
    }
    OnButtonLostFocus() {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver || this.IsMouseOverInput) {
                if (!this.IsMouseOverInput) {
                    document.getElementById(this.ComponentButtonId).focus();
                }
            }

            else {
                this.IsOpened = false;
            }
        }
    }
    OnTextBoxLostFocus() {
        if (!this.IsMouseOverButton) {
            if (this.IsMouseOver) {
                document.getElementById(this.ComponentButtonId).focus();
            }
            else {
                this.IsOpened = false;
            }
        }
    }
    OnTextBoxKeydown(e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode == 9) {
            e.preventDefault();
        }
    }

    // Commands
    CancelButtonClicked() {
        this.ValidationErrorsList = []; 
        this.Text = null; 
        this.IsOpened = false;
    }

    public ValidationErrorsList = [];
    OkButtonClicked() {
        var errors = [];
        if (!AppTool.IsNullOrEmpty(this.Text)) {
            if (this.Text.length > 5000) {
                errors.push("Meeting Summary must be less\nthan 5000 char");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.UpdatingActivityMettingSummary();
        }
    }

    UpdatingActivityMettingSummary() {
        var service = new CRMControlsService();
        var summary = new MeetingSummary();
        summary.ActivityId = this.EntityId;
        summary.Summary = this.Text;
        summary.Post = this.IsChecked;
        service.PutCompleteActivity(summary).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.Updated.emit(true);
            }
        });
    }
}
