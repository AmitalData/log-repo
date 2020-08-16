import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    selector: 'RemarksPopUp',
    templateUrl: './RemarksPopUp.html',
})
export class RemarksPopUp
    extends BaseComponent {
    constructor() {
        super();
    }
    remarks: any;
    SetWindowArgs(args: any) {
        this.remarks = args.remarks;
    }
}

