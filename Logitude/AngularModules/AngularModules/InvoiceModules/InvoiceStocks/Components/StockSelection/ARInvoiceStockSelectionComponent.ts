import { Component } from '@angular/core';
import { ARInvoiceStockList } from '../../../../Invoice/EntityLists/ARInvoiceStockList';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ARInvoiceStockListService } from '../../../../Invoice/Services/StandardLists/ARInvoiceStockListService';
import { ARInvoiceStockLinePM } from '../../../../Invoice/EntityPMs/ARInvoiceStockLinePM';
import { ARInvoiceStockPMService } from '../../../../Invoice/Services/StandardPMs/ARInvoiceStockPMService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceStockSelectionComponent.html',
})

export class ARInvoiceStockSelectionComponent {
    public ObjectTableName: string = "ARInvoiceStock";
    public StocksHeaderList: ARInvoiceStockList[] = [];
    public StockLines: ARInvoiceStockLinePM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    constructor() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            this._entityResourceService.getEntityResourceByTableName("ARInvoiceStockLine", 0).subscribe(response => {
                this.InitializeServices();
                this.LoadData();
            });
        });
    }

    private ARInvoiceStockListService: ARInvoiceStockListService;
    InitializeServices() {
        this.ARInvoiceStockListService = new ARInvoiceStockListService();
    }

    private LoadData() {
        this.StocksHeaderList = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ARInvoiceStockListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.StocksHeaderList = myResponse.Result;
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    private invoiceStockSelectedItem: ARInvoiceStockList = null;
    public get InvoiceStockSelectedItem() {
        return this.invoiceStockSelectedItem;
    }
    public set InvoiceStockSelectedItem(value: ARInvoiceStockList) {
        if (this.invoiceStockSelectedItem != value) {
            this.invoiceStockSelectedItem = value;
        }
    }

    private stockLineSelectedItem: ARInvoiceStockLinePM = null;
    public get StockLineSelectedItem() {
        return this.stockLineSelectedItem;
    }
    public set StockLineSelectedItem(value: ARInvoiceStockLinePM) {
        if (this.stockLineSelectedItem != value) {
            this.stockLineSelectedItem = value;
        }
    }

    EditStockClicked(item: ARInvoiceStockList) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit ";
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
        logWindow.WindowArgs = { IsNew: true, EntityPM: null };
        logWindow.Show('./InvoiceModules/InvoiceStocks/Components/NewEntity/NewARInvoiceStockComponent');
        logWindow.WindowClosed.subscribe(s => {
            if (s == "OK") {
                this.LoadData();
            }
        });
    }

    CancelClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    OkClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }
}
