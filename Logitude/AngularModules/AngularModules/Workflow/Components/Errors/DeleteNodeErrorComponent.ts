import { Component } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './DeleteNodeErrorComponent.html',
})

export class DeleteNodeErrorComponent {

    public NodeNameToDelete: string;
    public UsedInNodes: string[];

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.NodeNameToDelete = args.NodeNameToDelete ? args.NodeNameToDelete : "";
        this.UsedInNodes = args.UsedInNodes ? args.UsedInNodes : [];
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}