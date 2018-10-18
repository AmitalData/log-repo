import {Component, Input} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';

import {Modal} from '../../infrastructure/modal-libs/angular2-modal/providers/Modal';
import {ModalDialogInstance} from '../../infrastructure/modal-libs/angular2-modal/models/ModalDialogInstance';
import {ICustomModal, ICustomModalComponent} from '../../infrastructure/modal-libs/angular2-modal/models/ICustomModal';

export class AdditionCalculateWindowData {
    constructor(
        public num1: number,
        public num2: number
    ) { }
}

/**
 * A Sample of how simple it is to create a new window, with its own injects.
 */
@Component({
    selector: 'modal-content',
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES],
    styles: [`
        .custom-modal-container {
            padding: 15px;
        }
        .custom-modal-header {
            background-color: #219161;
            color: #fff;
            -webkit-box-shadow: 0px 3px 5px 0px rgba(0,0,0,0.75);
            -moz-box-shadow: 0px 3px 5px 0px rgba(0,0,0,0.75);
            box-shadow: 0px 3px 5px 0px rgba(0,0,0,0.75);
            margin-top: -15px;
            margin-bottom: 40px;
        }
    `],
    //TODO: [ngClass] here on purpose, no real use, just to show how to workaround ng2 issue #4330.
    // Remove when solved.

    templateUrl: './app/shipment/new-package-modal/new-package-modal.html'
})
export class AddNewPackageWindow implements ICustomModalComponent {
    dialog: ModalDialogInstance;
    context: AdditionCalculateWindowData;

    public wrongAnswer: boolean;

    constructor(dialog: ModalDialogInstance, modelContentData: ICustomModal) {
        this.dialog = dialog;
        this.context = <AdditionCalculateWindowData>modelContentData;
        //this.wrongAnswer = true;
    }

    //onKeyUp(value) {
    //    /* tslint:disable */ this.wrongAnswer = value != 5;
    //    this.dialog.close();
    //}

    CloseSuccess() {
        this.dialog.close("successfully closed");
    }

    CloseFail() {
        this.dialog.dismiss();
    }

    //beforeDismiss(): boolean {
    //    return true;
    //}

    //beforeClose(): boolean {
    //    return this.wrongAnswer;
    //}
}