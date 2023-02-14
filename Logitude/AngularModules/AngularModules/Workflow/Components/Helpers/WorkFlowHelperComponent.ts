import { Component, OnInit, HostListener } from '@angular/core';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';

@Component({
    templateUrl: "WorkFlowHelperComponent.html",
})

export class WorkFlowHelperComponent implements OnInit {

    public SetWarningMessagesEventCode: string = "SetWorkflowWarningMessages";
    public WarningMessagesTitle: string = "Review these warnings";
    public WarningMessages: string[] = [];
    public IsWarningMessagesOpened: boolean = false;
    public ShowWarningMessages: boolean = false;

    public SetErrorMessagesEventCode: string = "SetWorkflowErrorMessages";
    public ErrorMessagesTitle: string = "Fix these errors";
    public ErrorMessages: string[] = [];
    public IsErrorMessagesOpened: boolean = false;
    public ShowErrorMessages: boolean = false;

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
        if (event && event.Code && event.Code === this.SetWarningMessagesEventCode) {
            this.WarningMessages = event.Messages ? event.Messages.filter((message: any) => message && message !== "") : [];
            this.ShowWarningMessages = this.WarningMessages && this.WarningMessages.length > 0;
        }
    }

    handleSetErrorMessagesEvent(event: any) {
        if (event && event.Code && event.Code === this.SetErrorMessagesEventCode) {
            this.ErrorMessages = event.Messages ? event.Messages.filter((message: any) => message && message !== "") : [];
            this.ShowErrorMessages = this.ErrorMessages && this.ErrorMessages.length > 0;
        }
    }

    toggleWarningMessages() {
        this.IsWarningMessagesOpened = !this.IsWarningMessagesOpened;
        this.IsErrorMessagesOpened = this.IsWarningMessagesOpened ? false : this.IsErrorMessagesOpened;
    }

    toggleErrorMessages() {
        this.IsErrorMessagesOpened = !this.IsErrorMessagesOpened;
        this.IsWarningMessagesOpened = this.IsErrorMessagesOpened ? false : this.IsWarningMessagesOpened;
    }
}