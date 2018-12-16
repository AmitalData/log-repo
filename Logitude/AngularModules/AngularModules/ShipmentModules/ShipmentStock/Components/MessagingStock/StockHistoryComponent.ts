import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { ShipmentDomainService} from '../../../../Shipment/Services/ShipmentDomainService';
import { MessagingStockUsageHistoryList } from '../../../../Shipment/EntityLists/MessagingStockUsageHistoryList';

@Component({
    moduleId: module.id,

    templateUrl: './StockHistoryComponent.html',
})

export class StockHistoryComponent {
    public ItemsSource: MessagingStockUsageHistoryList[];
    constructor() {
        this.ItemsSource = [];        
    }

    SetWindowArgs(stockId: string) {
        if (stockId != null) {
            this.LoadData(stockId);
        }
    }

    private myDomainService: ShipmentDomainService;
    private LoadData(stockId: string) {
        
        this.ItemsSource = [];
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        if (this.myDomainService == null) {
            this.myDomainService = new ShipmentDomainService();
        }

        this.myDomainService.GetLoggedTenantMessagingStockUsageHistoryLists(stockId).subscribe((myResult:any) => {
            this.ItemsSource = myResult;

            SessionLocator.CurrentSession.StopBusyIndicator();
        });
    }

    CloseButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}
