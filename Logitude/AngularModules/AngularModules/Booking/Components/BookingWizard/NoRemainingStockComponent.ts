import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './NoRemainingStockComponent.html',
})

export class NoRemainingStockComponent {
    public PurchaseStockUri: string;
    public StockErrorMessage: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.PurchaseStockUri = "https://www.plimus.com/jsp/buynow.jsp?contractId=3233898&language=ENGLISH&currency=USD&custom1=" + SessionLocator.Tenant + "&quantity=1";
        this.StockErrorMessage = "Your remaining stock is (0) which is insufficient for this operation. Please purchase another messaging stock via the link";
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
