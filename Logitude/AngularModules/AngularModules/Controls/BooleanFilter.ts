import {Component, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'BooleanFilter',
    inputs: ['SelectedValue'],
    template:
    `
    <ul class="FiltersMenu"> 
        <li (click)="itemClicked('True')" (mouseover)="itemMouseOver('True')" style="width:35px;" (mouseleave)="itemMouseLeave('True')" [class.SelectedFilter]="SelectedValue === 'True'">
           Yes
        </li>
        <li (click)="itemClicked('False')" (mouseover)="itemMouseOver('False')" style="width:35px;" (mouseleave)="itemMouseLeave('False')" [class.SelectedFilter]="SelectedValue === 'False'">
           No
        </li> 
    </ul>
    `
})

export class BooleanFilter {
    public SelectedValue: string = "True";
    @Output() SelectedTypeValueChanged = new EventEmitter();

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedTypeValueChanged.emit(itemValue);
        }
    }
    itemMouseOver(itemValue: string) {
       
    }
    itemMouseLeave(itemValue: string) {
        
    }
}