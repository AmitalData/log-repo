import { Component, OnInit, HostListener } from '@angular/core';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';

@Component({
    templateUrl: "WorkFlowHelperComponent.html",
})

export class WorkFlowHelperComponent implements OnInit {

    public SetWarningMessagesEventCode: string = "SetWorkflowWarningMessages";
    public SetErrorMessagesEventCode: string = "SetWorkflowErrorMessages";
    public WarningMessagesTitle: string = "Review these warnings";
    public ErrorMessagesTitle: string = "Fix these errors";
    public WarningMessages: string[] = [];
    public ErrorMessages: string[] = [];
    public IsWarningMessagesOpened: boolean = false;
    public IsErrorMessagesOpened: boolean = false;
    public IsWarningMessagesDisplayed: boolean = false;
    public IsErrorMessagesDisplayed: boolean = false;
    public IsClickInside: boolean = false;

    constructor(public entityArguments: EntityArgs) { }

    @HostListener("click")
    clickInside() {
        this.IsClickInside = true;
    }

    @HostListener("document:click")
    clickOutside() {
        if (!this.IsClickInside) {
            this.IsWarningMessagesOpened = false;
            this.IsErrorMessagesOpened = false;
        }
        this.IsClickInside = false;
    }

    ngOnInit() {
        this.initializeEventEmitterHandler();
    }

    initializeEventEmitterHandler() {
        this.entityArguments.EntityArgEventEmitter.subscribe((event: any) => {
            this.handleSetWarningMessagesEvent(event);
            this.handleSetErrorMessagesEvent(event);
        });
    }

    handleSetWarningMessagesEvent(event: any) {
        if (this.isSetWarningMessagesEvent(event)) {
            let warningMessages = this.getMessages(event);
            this.IsWarningMessagesDisplayed = warningMessages && warningMessages.length > 0;
            this.WarningMessages = warningMessages;
        }
    }

    handleSetErrorMessagesEvent(event: any) {
        if (this.isSetErrorMessagesEvent(event)) {
            let errorMessages = this.getMessages(event);
            this.IsErrorMessagesDisplayed = errorMessages && errorMessages.length > 0;
            this.ErrorMessages = errorMessages;
        }
    }

    isSetWarningMessagesEvent(event: any) {
        return event && event.Code && event.Code === this.SetWarningMessagesEventCode;
    }

    isSetErrorMessagesEvent(event: any) {
        return event && event.Code && event.Code === this.SetErrorMessagesEventCode;
    }

    getMessages(event: any) {
        if (event && event.Messages) {
            return event.Messages.filter((message: any) => message && message !== "") as string[];
        }
        return [];
    }

    toggleWarningMessages() {
        this.IsWarningMessagesOpened = !this.IsWarningMessagesOpened;
        if (this.IsWarningMessagesOpened) {
            this.IsErrorMessagesOpened = false;
        }
    }

    toggleErrorMessages() {
        this.IsErrorMessagesOpened = !this.IsErrorMessagesOpened;
        if (this.IsErrorMessagesOpened) {
            this.IsWarningMessagesOpened = false;
        }
    }
}