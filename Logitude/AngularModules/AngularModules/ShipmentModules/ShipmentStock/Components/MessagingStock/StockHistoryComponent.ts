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
    private CurrentSession = SessionLocator.SelectedSession;
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
        this.CurrentSession.StartBusyIndicatorLoading();

        if (this.myDomainService == null) {
            this.myDomainService = new ShipmentDomainService();
        }

        this.myDomainService.GetLoggedTenantMessagingStockUsageHistoryLists(stockId).subscribe((myResult:any) => {
            this.ItemsSource = myResult;

            this.CurrentSession.StopBusyIndicator();
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
