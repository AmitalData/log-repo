
import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren } from '@angular/core';
import { AutomationPM } from '../../../../../Common/EntityPMs/AutomationPMExtended';
import { AppTool, DateTool } from '../../../../../Infrastructure/Tools';


import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Operator } from '../ViewModel/AutomationConditionViewModel';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { AutomationSendInterface } from '../../../../DataContracts/AutomationSendInterface';
import { SessionLocator } from '../../../../Utilities/SessionLocator';

@Component({
    selector: 'SendInterfaceResult',
    templateUrl: './SendInterfaceResultComponent.html',
    inputs: [''],

})
export class SendInterfaceResultComponent extends BaseComponent implements OnInit {

    SendInterfaceClassLists: Operator[] = [];
    SendFormatLists: Operator[] = [];
    SendViaClassLists: Operator[] = [];
    FTPFolderLists: Operator[] = [];
    IsCustomerCare: boolean = false;

    DataContext: any;
    IsRefreshComputingPartner: boolean = false;
    constructor() {
        super();
        this.DataContext = this;
        if (FeatureLocator.HasFeaturePermession("Automation", "SENDINTERFACERESULT") && SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsCustomerCare = true;
        }

        this.InitializeSendInterfaceResultComponent();

    }
    item: any;
    ngOnInit() {

        this.UIProperties.SetEnabled("ComputingPartnerId", "SendInterface", this.IsCustomerCare);

    }

    InitializeSendInterfaceResultComponent() {
        this.SendInterfaceClassLists = [];
        this.SendInterfaceClassLists.push(new Operator("Shipment API", "ShipmentAPI"));




        this.SendFormatLists = [];
        this.SendFormatLists.push(new Operator("XML", "XML"));
        this.SendFormatLists.push(new Operator("JSON", "JSON"));


        this.SendViaClassLists = [];
        this.SendViaClassLists.push(new Operator("FTP", "FTP"));
        this.SendViaClassLists.push(new Operator("Email", "EMAIL"));

    }



    private sendInterfaceSelected: Operator;
    get SendInterfaceSelected() { return this.sendInterfaceSelected; }
    set SendInterfaceSelected(newValue: Operator) {
        if (newValue != this.sendInterfaceSelected) {
            this.sendInterfaceSelected = newValue;
            if (this.automationSendInterface.InterfaceName != newValue.Code) {
                this.automationSendInterface.InterfaceName = newValue.Code;
                this.automationSendInterface.IsChanged = true;
            }
        }
    }






    private sendViaSelected: Operator;
    get SendViaSelected() { return this.sendViaSelected; }
    set SendViaSelected(newValue: Operator) {
        if (newValue != this.sendViaSelected) {
            this.sendViaSelected = newValue;
            if (this.automationSendInterface.SendVia != newValue.Code) {
                this.automationSendInterface.SendVia = newValue.Code;
                this.automationSendInterface.IsChanged = true;
            }

        }
    }



    private fTPFolderSelected: Operator;
    get FTPFolderSelected() { return this.fTPFolderSelected; }
    set FTPFolderSelected(newValue: Operator) {
        if (newValue != this.fTPFolderSelected) {
            this.fTPFolderSelected = newValue;
            if (this.automationSendInterface.SendVia != newValue.Code) {
                this.automationSendInterface.SendVia = newValue.Code;
                this.automationSendInterface.IsChanged = true;
            }
        }
    }


    private sendFormatSelected: Operator;
    get SendFormatSelected() { return this.sendFormatSelected; }
    set SendFormatSelected(newValue: Operator) {
        if (newValue != this.sendFormatSelected) {
            this.sendFormatSelected = newValue;
            if (this.automationSendInterface.Format != newValue.Code) {
                this.automationSendInterface.Format = newValue.Code;
                this.automationSendInterface.IsChanged = true;
            }

        }
    }



    private computingPartnerId: string;
    get ComputingPartnerId() { return this.computingPartnerId; }
    set ComputingPartnerId(newValue: string) {
        if (newValue != this.computingPartnerId) {
            this.computingPartnerId = newValue;
            if (this.automationSendInterface.ComputingPartnerId != newValue) {
                this.automationSendInterface.ComputingPartnerId = newValue;
                this.automationSendInterface.IsChanged = true;
            }

        }
    }

    SelectedComputedPartnerChange(value) {
        let newComputedPartnerValue;
        if (value) newComputedPartnerValue = value.Id;
        this.ComputingPartnerId = newComputedPartnerValue;

    }

    SetSelectedDelfultData() {

        if (this.automationSendInterface) {
            if (this.automationSendInterface.InterfaceName) {
                this.SendInterfaceSelected = this.SendInterfaceClassLists.filter(d => d.Code == this.automationSendInterface.InterfaceName)[0];
            } else this.SendInterfaceSelected = this.SendInterfaceClassLists[0];


            if (this.automationSendInterface.Format) {
                this.SendFormatSelected = this.SendFormatLists.filter(d => d.Code == this.automationSendInterface.Format)[0];
            } else this.SendFormatSelected = this.SendFormatLists[0];

            if (this.automationSendInterface.SendVia) {
                this.SendViaSelected = this.SendViaClassLists.filter(d => d.Code == this.automationSendInterface.SendVia)[0];
            } else this.SendViaSelected = this.SendViaClassLists[0];

            this.ComputingPartnerId = this.automationSendInterface.ComputingPartnerId;

        }

    }





    AddFTPFolder() {


    }


    ShowFTPDetails() {
        var windowArgs: any = {};
        windowArgs.FTPAutomationDetails = this.automationSendInterface.FTPDetails;

        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 450;
        logWindow.Title = "FTP Details";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/AutomationResult/FTPAutomationDetailsComponent");
        logWindow.WindowClosed.subscribe((message: any) => {
            if (message == "Changed") this.automationSendInterface.IsChanged = true
        });
    }


    AddComputingPartner() {
        var windowArgs: any = {};

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 550;
        logitudeWindow.Title = "New Computing Partner";

        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/DocumentType/NewComputingPartnerConmponent');


        logitudeWindow.WindowClosed.subscribe((computingPartner: any) => {
            if (computingPartner) {
                this.ComputingPartnerId = computingPartner;
                this.IsRefreshComputingPartner = !this.IsRefreshComputingPartner;

            }

        });
    }
    public automationSendInterface: AutomationSendInterface;
    Run(automationSendInterface: AutomationSendInterface) {
        this.automationSendInterface = automationSendInterface;

        this.SetSelectedDelfultData();
    }
}

