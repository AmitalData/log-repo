import {Component, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'CurrencyFilter',
    inputs: ['CurrencyFilter'],
    template:
    `
    <ul class="FiltersMenu">
       
       
        <li id="USD,profit" (click)="itemClicked('USD,profit')" (mouseover)="itemMouseOver('USD,profit')" (mouseleave)="itemMouseLeave('USD,profit')" [class.SelectedFilter]="SelectedValue === 'USD,profit'" title="USD">
            USD
        </li>

 <li id="NIS,local" (click)="itemClicked('NIS,local')" (mouseover)="itemMouseOver('NIS,local')" (mouseleave)="itemMouseLeave('NIS,local')" [class.SelectedFilter]="SelectedValue === 'NIS,local'" title="NIS">
          NIS
        </li>
        
    </ul>
    `
})

export class CurrencyFilter {
    public SelectedValue: string = "NIS,local";
    @Output() SelectedValueChanged = new EventEmitter();

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);

      
        }
    }
    itemMouseOver(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var att = document.getElementById(itemValue);
            att.style.backgroundColor = 'white';
        }
    }
    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {
       
        }
    }
}