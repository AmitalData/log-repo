import { Component} from '@angular/core';
import { ARInvoiceStockList } from '../../../Invoice/EntityLists/ARInvoiceStockList';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ARInvoiceStockListService } from '../../../Invoice/Services/StandardLists/ARInvoiceStockListService';

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
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            this._entityResourceService.getEntityResourceByTableName("ARInvoiceStockLine", 0).subscribe(response => {
            this.IsVisibile = true;
            this.ARInvoiceStockListService = new ARInvoiceStockListService();
            this.LoadData();
            });
        });
    }
    
    private LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        
        this.ARInvoiceStockListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.BuildItemsSource(myResponse.Result);
            }

            this.CurrentSession.StopBusyIndicator();
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
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit AR Invoice Stock";
        logWindow.IsFillScreen = true;
        logWindow.ShowEditComponent(item.Id, "ARInvoiceStock");

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                this.LoadData();
            });
        });       
    }

    NewStockClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "New Invoice Stock";
        logWindow.Width = 900;
        logWindow.Height = 600;
        logWindow.WindowArgs = { IsNew: true, EntityPM: null };
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewEntity/NewARInvoiceStockComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
            }
        });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
