import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentDomainService, AWBStockUsageHistoryList} from '../../../../Shipment/Services/ShipmentDomainService';

@Component({
    moduleId: module.id,

    templateUrl: './StockHistoryComponent.html',
})

export class StockHistoryComponent {
    public ItemsSource: AWBStockUsageHistoryList[];
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

        this.myDomainService.GetLoggedTenantAWBStockUsageHistoryLists(stockId).subscribe((myResult:any) => {
            this.ItemsSource = myResult;

            SessionLocator.CurrentSession.StopBusyIndicator();
        });
    }

    CloseButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}