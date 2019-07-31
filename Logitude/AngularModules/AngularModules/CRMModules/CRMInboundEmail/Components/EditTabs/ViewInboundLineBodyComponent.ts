import {Component}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'TicketDocsOutTabComponent',
    moduleId: module.id,
    templateUrl: './ViewInboundLineBodyComponent.html',
})

export class ViewInboundLineBodyComponent {
    public DataContext = this;
    public Description: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    SetWindowArgs(args: string) {
        this.Description = args;
    }
    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }
}
