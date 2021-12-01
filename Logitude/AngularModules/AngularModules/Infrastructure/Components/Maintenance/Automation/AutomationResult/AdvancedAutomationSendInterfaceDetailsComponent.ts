import { Component} from '@angular/core';
import { BaseComponent } from '../../../LogitudeComponents/BaseComponent';
import { AdvancedAutomationSendInterfaceDetails } from '../../../../DataContracts/AutomationSendInterface';
import { SessionLocator } from '../../../../Utilities/SessionLocator';

@Component({
    templateUrl: './AdvancedAutomationSendInterfaceDetailsComponent.html',
})

export class AdvancedAutomationSendInterfaceDetailsComponent extends BaseComponent {
    public DataContext: any;
    public AdvancedAutomationSendInterfaceDetails: AdvancedAutomationSendInterfaceDetails;
    public ObjectTableName: string = "AdvancedAutomationSendInterfaceDetails";
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
        this.DataContext = this;
    }

    SetWindowArgs(args: any) {
        this.AdvancedAutomationSendInterfaceDetails = args.AdvancedAutomationSendInterfaceDetails;
        if (this.AdvancedAutomationSendInterfaceDetails) {
            this.IncludeEvents = this.AdvancedAutomationSendInterfaceDetails.IncludeEvents;
        }
    }

    SaveButtonClicked() {
        this.AdvancedAutomationSendInterfaceDetails.IncludeEvents = this.IncludeEvents;
        this.CurrentSession.CurrentWindow.Close("Changed");
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
}
