import { Component} from '@angular/core';
import { ARInvoiceStockList } from '../../../Invoice/EntityLists/ARInvoiceStockList';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ARInvoiceStockListService } from '../../../Invoice/Services/StandardLists/ARInvoiceStockListService';
import { ARInvoiceStockPMService } from '../../../Invoice/Services/StandardPMs/ARInvoiceStockPMService';

@Component({
    moduleId: module.id,
    templateUrl: './ManageStocksComponent.html',
})

export class ManageStocksComponent {
    public ObjectTableName: string = "ARInvoiceStock";    
    public ItemsSource: ARInvoiceStockList[] = [];
    public IsVisibile: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private ARInvoiceStockListService: ARInvoiceStockListService;
    constructor() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            this.IsVisibile = true;
            this.ARInvoiceStockListService = new ARInvoiceStockListService();
            this.LoadData();
        });
    }
    
    private LoadData() {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();
        
        this.ARInvoiceStockListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.BuildItemsSource(myResponse.Result);
            }

            SessionLocator.CurrentSession.StopBusyIndicator();
        });
    }
    private BuildItemsSource(items: ARInvoiceStockList[]) {
        var itemsSource: ARInvoiceStockList[] = [];

        if (items != null) {
            itemsSource = items;
        }
        
        this.ItemsSource = itemsSource;
    }

    EditStock(item: ARInvoiceStockList) {
        SessionLocator.CurrentSession.StartBusyIndicatorLoading();

        var stockPMService: ARInvoiceStockPMService = new ARInvoiceStockPMService();
        stockPMService.get(item.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Edit Invoice Stock";
                logWindow.Height = 600;
                logWindow.WindowArgs = { IsNew: false, EntityPM: myResponse.Result };
                logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewARInvoiceStockComponent');
                logWindow.WindowClosed.subscribe(s => {
                    if (s == "OK") {
                        this.LoadData();
                    }
                });
            }

            SessionLocator.CurrentSession.StopBusyIndicator();
        });       
    }

    NewStockClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Invoice Stock";
        logWindow.Height = 600;
        logWindow.WindowArgs = { IsNew: true, EntityPM: null };
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewARInvoiceStockComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
            }
        });
    }

    CloseClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}
