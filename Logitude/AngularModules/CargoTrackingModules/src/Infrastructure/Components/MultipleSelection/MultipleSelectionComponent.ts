import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'multiple-selection',
    templateUrl: './MultipleSelectionComponent.html',
    styleUrls: ['./MultipleSelectionComponent.css']
})
export class MultipleSelectionComponent {

    MultipleSelection = new FormControl();
    @Input() MultipleSelectionList: any;
    @Input() Title: string;
    constructor() {}
}
