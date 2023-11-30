import { Component} from '@angular/core';
import { BaseComponent } from '../../../LogitudeComponents/BaseComponent';
import { AdvancedAutomationSendInterfaceDetails, AutomationSendInterface, ARInvoiceDetails } from '../../../../DataContracts/AutomationSendInterface';
import { SessionLocator } from '../../../../Utilities/SessionLocator';

@Component({
    templateUrl: './AdvancedAutomationSendInterfaceDetailsComponent.html',
})

export class AdvancedAutomationSendInterfaceDetailsComponent extends BaseComponent {
    public DataContext: any;
    public AutomationSendInterface: AutomationSendInterface;
    public AdvancedAutomationSendInterfaceDetails: AdvancedAutomationSendInterfaceDetails;
    public ARInvoiceDetails: ARInvoiceDetails;
    public ObjectTableName: string = "AdvancedAutomationSendInterfaceDetails";
    public SelectedInterfaceCode: string;
    public ShipmentInterfaceCode: string = "ShipmentAPI";
    public ARInvoiceInterfaceCode: string = "ARInvoiceAPI";
    private CurrentSession = SessionLocator.SelectedSession;
    
    constructor() {
        super();
        this.DataContext = this;
    }

    SetWindowArgs(args: any) {
        this.AutomationSendInterface = args.AutomationSendInterface;
        if (this.AutomationSendInterface) {
            this.FillData();
        }
    }

    FillData() {
        this.AdvancedAutomationSendInterfaceDetails = this.AutomationSendInterface.AdvancedAutomationSendInterfaceDetails;

        if (!this.AdvancedAutomationSendInterfaceDetails) {
            return;
        }

        this.SelectedInterfaceCode = this.AutomationSendInterface.InterfaceName;
        this.ARInvoiceDetails = this.AdvancedAutomationSendInterfaceDetails.ARInvoiceDetails;

        if (this.SelectedInterfaceCode == this.ShipmentInterfaceCode) {
            this.FillShipmentDetails();
        }
        else if (this.SelectedInterfaceCode == this.ARInvoiceInterfaceCode) {
            this.FillARInvoiceDetails();
        }
    }

    FillShipmentDetails() {
        this.IncludeEvents = this.AdvancedAutomationSendInterfaceDetails.IncludeEvents;
    }

    FillARInvoiceDetails() {
        if (!this.ARInvoiceDetails) {
            this.ARInvoiceDetails = new ARInvoiceDetails();
        }
        this.IncludeShipmentDetails = this.ARInvoiceDetails.IncludeShipmentDetails;
    }

    SaveButtonClicked() {
        if (this.SelectedInterfaceCode == this.ShipmentInterfaceCode) {
            this.SaveShipmentDetails();
        }
        else if (this.SelectedInterfaceCode == this.ARInvoiceInterfaceCode) {
            this.SaveARInvoiceDetails();
        }
        
        this.CurrentSession.CurrentWindow.Close("Changed");
    }

    SaveShipmentDetails() {
        this.AdvancedAutomationSendInterfaceDetails.IncludeEvents = this.IncludeEvents;
    }

    SaveARInvoiceDetails() {
        this.ARInvoiceDetails.IncludeShipmentDetails = this.IncludeShipmentDetails;
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private includeEvents: boolean;
    get IncludeEvents() { return this.includeEvents; }
    set IncludeEvents(value: boolean) {
        if (this.includeEvents != value) {
            this.includeEvents= value;
        }
    }

    private includeShipmentDetails: boolean;
    get IncludeShipmentDetails() { return this.includeShipmentDetails; }
    set IncludeShipmentDetails(value: boolean) {
        if (this.includeShipmentDetails != value) {
            this.includeShipmentDetails = value;
        }
    }
}
