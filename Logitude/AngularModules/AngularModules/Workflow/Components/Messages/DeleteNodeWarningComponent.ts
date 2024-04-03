import { Component } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './DeleteNodeWarningComponent.html',
})

export class DeleteNodeWarningComponent {

    public NodeNameToDelete: string;
    public UsedInNodes: string[] | null;

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.NodeNameToDelete = args.NodeNameToDelete ? args.NodeNameToDelete : "";
        this.UsedInNodes = args.UsedInNodes;
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}