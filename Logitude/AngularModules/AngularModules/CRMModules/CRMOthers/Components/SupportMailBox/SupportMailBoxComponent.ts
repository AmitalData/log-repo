import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './SupportMailBoxComponent.html',
})

export class SupportMailBoxComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
    }



    AddMailBox() {

    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
