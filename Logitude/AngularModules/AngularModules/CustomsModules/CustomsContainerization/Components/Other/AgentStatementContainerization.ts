import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'AgentStatementContainerization',
    templateUrl: './AgentStatementContainerization.html',
})
export class AgentStatementContainerization
    extends BaseComponent {
    constructor() {
        super();
    }

    private CurrentSession = SessionLocator.SelectedSession;
    IsSelected: boolean = false;
    IsDirectCharging: boolean = false;
    public ObjectTableName: string = "Customs.Containerization";
    public DataContext=this;

    remarks: any;
    SetWindowArgs(args: any) {
        if (args.IsDirectCharging != null) {
            this.IsDirectCharging=true
        }
    }
    SendButtonClicked(event) {
        if (this.IsSelected) {
            this.CurrentSession.CurrentWindow.Close("true");
        } else {
            this.CurrentSession.CurrentWindow.Close("false");
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}

