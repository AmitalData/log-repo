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
    IsSelected: boolean;
    public ObjectTableName: string = "Customs.Containerization";
    public DataContext=this;

    remarks: any;
    SetWindowArgs(args: any) {

    }
    SendButtonClicked(event) {
        this.CurrentSession.CurrentWindow.Close(event);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}

