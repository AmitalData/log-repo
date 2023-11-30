import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Operator } from '../ViewModel/AutomationConditionViewModel';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { AutomationOnUpdateDocument } from '../../../../DataContracts/AutomationOnUpdateDocument';
import { SessionLocator } from '../../../../Utilities/SessionLocator';

@Component({
    selector: 'OnUpdateDocumentResult',
    templateUrl: './OnUpdateDocumentResultComponent.html',
    inputs: [''],
})
export class OnUpdateDocumentResultComponent extends BaseComponent implements OnInit {
    SendViaClassLists: Operator[] = [];
    FTPFolderLists: Operator[] = [];
    IsCustomerCare: boolean = false;
    IsRefreshComputingPartner: boolean = false;
    CountDocumentSelection: string = "no documents selected";

    DataContext: any;
    constructor() {
        super();
        this.DataContext = this;
        if (SessionLocator.LoggedUserPM.IsCustomerCare) this.IsCustomerCare = true;
        this.InitializeOnUpdateDocumentResultComponent();
    }

    ngOnInit() {
        this.UIProperties.SetEnabled("ComputingPartnerId", "OnUpdateDocument", this.IsCustomerCare);
    }

    InitializeOnUpdateDocumentResultComponent() {
        this.SendViaClassLists = [];
        this.SendViaClassLists.push(new Operator("FTP", "FTP"));
    }

    private sendViaSelected: Operator;
    get SendViaSelected() { return this.sendViaSelected; }
    set SendViaSelected(newValue: Operator) {
        if (newValue == this.sendViaSelected) return;
        this.sendViaSelected = newValue;
        if (this.AutomationOnUpdateDocument.SendVia == newValue.Code) return;
        this.AutomationOnUpdateDocument.SendVia = newValue.Code;
        this.AutomationOnUpdateDocument.IsChanged = true;
    }

    private fTPFolderSelected: Operator;
    get FTPFolderSelected() { return this.fTPFolderSelected; }
    set FTPFolderSelected(newValue: Operator) {
        if (newValue == this.fTPFolderSelected) return;
        this.fTPFolderSelected = newValue;
        if (this.AutomationOnUpdateDocument.SendVia == newValue.Code) return;
        this.AutomationOnUpdateDocument.SendVia = newValue.Code;
        this.AutomationOnUpdateDocument.IsChanged = true;
    }

    SetSelectedDelfultData() {
        if (!this.AutomationOnUpdateDocument) return;
        if (this.AutomationOnUpdateDocument.SendVia) {
            this.SendViaSelected = this.SendViaClassLists.filter(d => d.Code == this.AutomationOnUpdateDocument.SendVia)[0];
        }
        else this.SendViaSelected = this.SendViaClassLists[0];

        this.ComputingPartnerId = this.AutomationOnUpdateDocument.ComputingPartnerId;
        this.FillDocumentsSelectedCount();
    }

    FillDocumentsSelectedCount() {
        if (this.AutomationOnUpdateDocument.DocumentTypeLists && this.AutomationOnUpdateDocument.DocumentTypeLists.length > 0) {
            this.CountDocumentSelection = this.AutomationOnUpdateDocument.DocumentTypeLists.length + " documents selected";
        }
        else {
            this.CountDocumentSelection = "no documents selected";
        }
    }

    AddFTPFolder() {
    }

    ShowFTPDetails() {
        let windowArgs: any = {};
        windowArgs.FTPAutomationDetails = this.AutomationOnUpdateDocument.FTPDetails;
        let logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 450;
        logWindow.Title = "FTP Details";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/AutomationResult/FTPAutomationDetailsComponent");
        logWindow.WindowClosed.subscribe((message: any) => { this.FTPAutomationDetailsComponentWindowClosed(message); });
    }

    FTPAutomationDetailsComponentWindowClosed(message: any) {
        if (message == "Changed") this.AutomationOnUpdateDocument.IsChanged = true;
    }

    public AutomationOnUpdateDocument: AutomationOnUpdateDocument;
    public ObjectTableId: string;
    Run(automationOnUpdateDocument: AutomationOnUpdateDocument, objectTableId: string) {
        this.AutomationOnUpdateDocument = automationOnUpdateDocument;
        this.ObjectTableId = objectTableId;
        this.SetSelectedDelfultData();
    }

    private computingPartnerId: string;
    get ComputingPartnerId() { return this.computingPartnerId; }
    set ComputingPartnerId(newValue: string) {
        if (newValue == this.computingPartnerId) return;
        this.computingPartnerId = newValue;
        if (this.AutomationOnUpdateDocument.ComputingPartnerId == newValue) return;
        this.AutomationOnUpdateDocument.ComputingPartnerId = newValue;
        this.AutomationOnUpdateDocument.IsChanged = true;
    }

    SelectedComputedPartnerChange(value) {
        let newComputedPartnerValue;
        if (value) newComputedPartnerValue = value.Id;
        this.ComputingPartnerId = newComputedPartnerValue;
    }

    AddComputingPartner() {
        let windowArgs: any = {};
        let logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 550;
        logitudeWindow.Title = "New Computing Partner";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/DocumentType/NewComputingPartnerConmponent');
        logitudeWindow.WindowClosed.subscribe((computingPartner: any) => { this.NewComputingPartnerConmponentWindowClosed(computingPartner); });
    }

    NewComputingPartnerConmponentWindowClosed(computingPartner: any) {
        if (!computingPartner) return;
        this.ComputingPartnerId = computingPartner;
        this.IsRefreshComputingPartner = !this.IsRefreshComputingPartner;
    }

    SelectDocumentTypes() {
        let logWindow = new LogitudeWindow();
        logWindow.Width = 760;
        logWindow.Height = 560;
        let windowArgs: any = {};
        windowArgs.AutomationOnUpdateDocument = this.AutomationOnUpdateDocument;
        windowArgs.ObjectTableId = this.ObjectTableId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "Document Types"
        logWindow.DataContext = this;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/DocumentAttachmentsComponent");
        logWindow.WindowClosed.subscribe(($event: any) => { this.DocumentAttachmentsComponentWindowClosed($event); });
    }

    DocumentAttachmentsComponentWindowClosed($event: any) {
        if ($event == "OK") this.FillDocumentsSelectedCount();
    }
}
