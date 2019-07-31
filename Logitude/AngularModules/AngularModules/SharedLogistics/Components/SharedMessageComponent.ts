
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
@Component({

    moduleId: module.id,
    selector: 'SharedMessageComponent',
    templateUrl: './SharedMessageComponent.html',

})
export class SharedMessageComponent implements OnInit {
    Message: string;
    TextColor: string;
    IsShowOkButton: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {


    }

    ngOnInit() {

    }


    SetWindowArgs(args: any) {
        this.Message = args.Message;
        this.TextColor = args.TextColor;
        this.IsShowOkButton = args.IsShowOkButton;
    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }

}
