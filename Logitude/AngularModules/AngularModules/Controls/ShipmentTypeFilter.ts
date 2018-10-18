import {Component, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';

@Component({
    selector: 'ShipmentTypeFilter',
    inputs: ['SelectedValue'],
    changeDetection: ChangeDetectionStrategy.OnPush,

    template:
    `
    <ul class="FiltersMenu"> 
        <li (click)="itemClicked('O')" (mouseover)="itemMouseOver('O')" style="width:55px;" (mouseleave)="itemMouseLeave('O')" [class.SelectedFilter]="SelectedValue === 'O'">
           Open
        </li>
        <li (click)="itemClicked('A')" (mouseover)="itemMouseOver('A')" style="width:55px;" (mouseleave)="itemMouseLeave('A')" [class.SelectedFilter]="SelectedValue === 'A'">
          Archived
        </li> 
    </ul>
    `
})

export class ShipmentTypeFilter {
    public SelectedValue: string = "O";
    @Output() SelectedTypeValueChanged = new EventEmitter();

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedTypeValueChanged.emit(itemValue);

            //var img_A = document.getElementById("TransportFilter_A");
            //var img_O = document.getElementById("TransportFilter_O");
            //var img_I = document.getElementById("TransportFilter_I");
            //img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            //img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            //img_I.setAttribute("src", "./Images/TransportModes/I_G.png");

            //switch (itemValue) {
            //    case "A": {
            //        img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
            //        break;
            //    }

            //    case "O": {
            //        img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
            //        break;
            //    }

            //    case "I": {
            //        img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
            //        break;
            //    }
            //}
        }
    }
    itemMouseOver(itemValue: string) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");

        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A.png");
        //            break;
        //        }

        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O.png");
        //            break;
        //        }

        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I.png");
        //            //img_I.style.top = "1px";
        //            break;
        //        }
        //    }
        //}
    }
    itemMouseLeave(itemValue: string) {
        //if (this.SelectedValue != itemValue) {
        //    var img_A = document.getElementById("TransportFilter_A");
        //    var img_O = document.getElementById("TransportFilter_O");
        //    var img_I = document.getElementById("TransportFilter_I");

        //    switch (itemValue) {
        //        case "A": {
        //            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
        //            break;
        //        }

        //        case "O": {
        //            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
        //            break;
        //        }

        //        case "I": {
        //            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
        //            break;
        //        }
        //    }
        //}
    }
}