import { Component } from '@angular/core';
import { ARInvoiceStockPM } from '../../../../Invoice/EntityPMs/ARInvoiceStockPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ARInvoiceStockLinePM } from '../../../../Invoice/EntityPMs/ARInvoiceStockLinePM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { InvoiceDomainService } from '../../../../Invoice/Services/InvoiceDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceStockSelectionComponent.html',
})

export class ARInvoiceStockSelectionComponent {
   
    public ObjectTableName: string = "ARInvoiceStock";
    public StocksHeaderList: StockHeaderData [] = [];
    public StockLines: ARInvoiceStockLinePM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public StocksLineCount:number;

    constructor() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            this._entityResourceService.getEntityResourceByTableName("ARInvoiceStockLine", 0).subscribe(response => {
                this.InitializeServices();
                this.LoadData();
            });
        });
    }

    private InvoiceDomainService: InvoiceDomainService;
    InitializeServices() {
        this.InvoiceDomainService = new InvoiceDomainService();
    }

    private LoadData() {
        this.StocksHeaderList = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.InvoiceDomainService.GetListOfARInvoiceStockPM().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var stockList = myResponse.Result;

                stockList.forEach(item => {
                    this.StocksHeaderList.push(new StockHeaderData(item))
                });
                
                this.InvoiceStockSelectedItem = this.StocksHeaderList!= null? this.StocksHeaderList[0]: null;
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }

    private invoiceStockSelectedItem: StockHeaderData = null;
    public get InvoiceStockSelectedItem() {
        return this.invoiceStockSelectedItem;
    }
    public set InvoiceStockSelectedItem(value: StockHeaderData) {
        if (this.invoiceStockSelectedItem != value) {
            this.invoiceStockSelectedItem = value;
            this.FillStockLines(value);
        }
    }

    FillStockLines(stock: StockHeaderData): any {
        if (stock != null) {
            this.StockLines = [];
            this.StockLineSelectedItem = null;
            stock.StockHeader.ARInvoiceStockLines.filter(a => !a.IsUsed).forEach(item => {
                this.StockLines.push(item);
            });
            this.StocksLineCount = this.StockLines.length;
            stock.SetCount(this.StocksLineCount);
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

    EditStockClicked(item: ARInvoiceStockPM) {
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
export class StockHeaderData {

    StockHeader: ARInvoiceStockPM;
    LinesCount: string;

    constructor(entity: ARInvoiceStockPM, count: number = null) {
        this.StockHeader = entity;
        this.SetCount(count);
    }

    SetCount(count : number) {
        if (count == 0) {
            this.LinesCount = "";
        }
        else {
            this.LinesCount = count + "";
        }
    }
    get Name() { return this.StockHeader.Name; }
    set Name(newValue: string) {
        if (this.StockHeader.Name != newValue) {
            this.StockHeader.Name = newValue;
        }
    }

    get EndDate() { return this.StockHeader.EndDate; }
    set EndDate(newValue: Date) {
        if (this.StockHeader.EndDate != newValue) {
            this.StockHeader.EndDate = newValue;
        }
    }

    get Description() { return this.StockHeader.Description; }
    set Description(newValue: string) {
        if (this.StockHeader.Description != newValue) {
            this.StockHeader.Description = newValue;
        }
    }
}
