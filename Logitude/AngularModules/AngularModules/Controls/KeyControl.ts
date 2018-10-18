import {Component, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'KeyControl',
    inputs: ['SelectedValue'],
    template:
    `
    <ul class="FiltersMenuImage">
        <li (click)="itemClicked('All')" (mouseover)="itemMouseOver('All')" [class.SelectedFilter]="SelectedValue === 'All'" >
            All
        </li>
        <li (click)="itemClicked('Open')" (mouseover)="itemMouseOver('Open')" (mouseleave)="itemMouseLeave('Open')" [class.SelectedFilter]="SelectedValue === 'Open'" title="Open"  >
            <img id="KeyControl_A" class="CenterCenter" src="./Images/Icons/IsOpened.png" style="top: 1px;height:17px"  />
        </li>
        <li (click)="itemClicked('Close')" (mouseover)="itemMouseOver('Close')" (mouseleave)="itemMouseLeave('Close')" [class.SelectedFilter]="SelectedValue === 'Close'" title="Close" >
            <img id="KeyControl_B" class="CenterCenter" src="./Images/Icons/IsClosed.png" style="top: 1px;height:17px" />
        </li>
       
    </ul>
    `
})

export class KeyControl {
    public SelectedValue: string = "All";
    @Output() SelectedValueChanged = new EventEmitter();

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
            var img_A = document.getElementById("KeyControl_A");
            var img_O = document.getElementById("KeyControl_B");
            img_A.setAttribute("src", "./Images/Icons/IsOpened.png");
            img_O.setAttribute("src", "./Images/Icons/IsClosed.png");

            switch (itemValue) {




                case "Open": {
                  //  img_A.setAttribute("src", "./Images/Icons/IsOpenedClicked.png");
                    break;
                }

                case "Close": {
               //     img_O.setAttribute("src", "./Images/Icons/IsClosedClicked.png");
                    break;
                }


            }
           
        }
    }
    itemMouseOver(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById("KeyControl_A");
            var img_O = document.getElementById("KeyControl_B");
            img_A.setAttribute("src", "./Images/Icons/IsOpened.png");
            img_O.setAttribute("src", "./Images/Icons/IsClosed.png");

            switch (itemValue) {




                case "Open": {
               //     img_A.setAttribute("src", "./Images/Icons/IsOpenedHoverd.png");
                    break;
                }

                case "Close": {
               //     img_O.setAttribute("src", "./Images/Icons/IsClosedHoverd.png");
                    break;
                }


            }

         
        }
    }
    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {

            var img_A = document.getElementById("KeyControl_A");
            var img_O = document.getElementById("KeyControl_B");
            img_A.setAttribute("src", "./Images/Icons/IsOpened.png");
            img_O.setAttribute("src", "./Images/Icons/IsClosed.png");


        }
    }
}