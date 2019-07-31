import {Component}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './PurchaseStockComponent.html',   
})

export class PurchaseStockComponent   {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    PurchaseNewStock() {
        window.open("https://www.plimus.com/jsp/buynow.jsp?contractId=3233898&language=ENGLISH&currency=USD&custom1=" + SessionLocator.Tenant + "&quantity=1" );
    }
}
