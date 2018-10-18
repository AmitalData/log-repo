import {Component} from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ContainerFollowupActionsComponent.html',
})

export class ContainerFollowupActionsComponent {
    public Code: string;
    public RoutingLinkText: string;
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.Code = args['Code'];

        switch (this.Code) {
            case "D": {
                this.RoutingLinkText = "Add Container Delivery";
                break;
            }

            case "R": {
                this.RoutingLinkText = "Add Empty Container Return";
                break;
            }
        }
    }

    SelectAction(typeCode: string) {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit(typeCode);
    }

    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}