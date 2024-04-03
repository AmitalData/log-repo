import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Operator } from '../ViewModel/AutomationConditionViewModel';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { AutomationSendDocument } from '../../../../DataContracts/AutomationSendDocument';
import { SessionLocator } from '../../../../Utilities/SessionLocator';

@Component({
    selector: 'SendDocumentResult',
    templateUrl: './SendDocumentResultComponent.html',
    inputs: [''],
})
export class SendDocumentResultComponent extends BaseComponent implements OnInit {
    SendViaClassLists: Operator[] = [];
    FTPFolderLists: Operator[] = [];
    IsCustomerCare: boolean = false;
    public ObjectTableName: string;
    DataContext: any;
    constructor() {
        super();
        this.DataContext = this;
        if (SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsCustomerCare = true;
        }
        this.InitializeSendDocumentResultComponent();
    }

    ngOnInit() {
    }

    InitializeSendDocumentResultComponent() {
        this.SendViaClassLists = [];
        this.SendViaClassLists.push(new Operator("FTP", "FTP"));
        this.SendViaClassLists.push(new Operator("Email", "EMAIL"));       
    }

    private sendViaSelected: Operator;
    get SendViaSelected() { return this.sendViaSelected; }
    set SendViaSelected(newValue: Operator) {
        if (newValue != this.sendViaSelected) {
            this.sendViaSelected = newValue;
            if (this.automationSendDocument.SendVia != newValue.Code) {
                this.automationSendDocument.SendVia = newValue.Code;
                this.automationSendDocument.IsChanged = true;
            }
        }
    }

    private fTPFolderSelected: Operator;
    get FTPFolderSelected() { return this.fTPFolderSelected; }
    set FTPFolderSelected(newValue: Operator) {
        if (newValue != this.fTPFolderSelected) {
            this.fTPFolderSelected = newValue;
            if (this.automationSendDocument.SendVia != newValue.Code) {
                this.automationSendDocument.SendVia = newValue.Code;
                this.automationSendDocument.IsChanged = true;
            }
        }
    }

    SetSelectedDelfultData() {
        if (this.automationSendDocument) {
            if (this.automationSendDocument.SendVia) {
                this.SendViaSelected = this.SendViaClassLists.filter(d => d.Code == this.automationSendDocument.SendVia)[0];
            }
            else if (this.ObjectTableName == "Shipment") {
                this.SendViaClassLists = this.SendViaClassLists.filter(d => d.Code !== "FTP");
                this.SendViaSelected = this.SendViaClassLists.filter(d => d.Code == "EMAIL")[0];
            }
            else
                this.SendViaSelected = this.SendViaClassLists[0];
        }
    }

    AddFTPFolder() {
    }

    ShowFTPDetails() {
        var windowArgs: any = {};
        windowArgs.FTPAutomationDetails = this.automationSendDocument.FTPDetails;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 450;
        logWindow.Title = "FTP Details";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/AutomationResult/FTPAutomationDetailsComponent");
        logWindow.WindowClosed.subscribe((message: any) => {
            if (message == "Changed") this.automationSendDocument.IsChanged = true
        });
    }

    public automationSendDocument: AutomationSendDocument;
    Run(automationSendDocument: AutomationSendDocument) {
        this.automationSendDocument = automationSendDocument;
        this.SetSelectedDelfultData();
    }
}
