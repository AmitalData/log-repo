import { Component, OnInit } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';


declare var makeAMLineChartMultiple: any;

@Component({
    selector: 'top-five-salesman-profit',
    moduleId: module.id,
    templateUrl: './TopFiveSalesmanProfitComponent.html',
})

export class TopFiveSalesmanProfitComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    private dashboardService: DashboardService;
    private dashboardArgs: QuoteDashboardArguments;
    public PerformanceChartId: string;
    public LegendDiv: string;
    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;
    public SelectedCurrency: string = "1";   
    public IsNoDataVisible: boolean = false;
    constructor(private _entityResourceService: EntityResourceService) {
        this.dashboardService = new DashboardService();
        this.dashboardArgs = new QuoteDashboardArguments();
        this.PerformanceChartId = "PerformanceChartId_" + this.CurrentSession.GetNewId("PerformanceChartId");
        this.LegendDiv = "LegendDiv_" + this.CurrentSession.GetNewId("LegendDiv");
    }

    ngOnInit() {                
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe((response:any) => {
            this.FillChartArgs();
            this.LoadChartData();
        });
    }
   
    private Wizard: QuoteDashboardComponent;
    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
    }

    FillChartArgs() {
        this.dashboardArgs.OwnerId = this.Wizard.OwnerId;
        this.dashboardArgs.BusinessUnitId = this.Wizard.BusinessUnitId;
        this.dashboardArgs.FromDate = this.Wizard.FromDate;
        this.dashboardArgs.ToDate = this.Wizard.ToDate;
        this.dashboardArgs.DatesCode = this.Wizard.DatesCode;
        this.dashboardArgs.ChartCode = "TFS";
        this.dashboardArgs.SelectedCurrency = "1";
    }

    RefreshTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        this.FillChartArgs();
        this.LoadChartData();
    }

    ChangeCurrency(code: string) {

        if (code == this.LocalCurrencyCode)
            code = "1";
        else
            code = "2";

        if (code != this.dashboardArgs.SelectedCurrency) {
            this.dashboardArgs.SelectedCurrency = code;
            this.SelectedCurrency = code;
            this.FillChartData();
        }
    }

    LoadChartData() {
        
        this.DataSource = [];
        this.IsNoDataVisible = false;

        this.dashboardService.GetDashboardChartValues(this.dashboardArgs).subscribe((myResult: any) => {

            this.IsNoDataVisible = false;
            this.DataSource = [];

            if (myResult != null && !myResult.HasError) {
                var data: any[] = myResult;

                if (data.length == 0) {
                    this.IsNoDataVisible = true;
                }

                this.ExctractData(data);
                this.FillChartData();
            }
        });
    }

    public DataSource: ChartItem[] = [];
    ExctractData(data:any[]) {

        this.DataSource = [];
        var index: number = 0;
        
        data.forEach(item => {

            var chartItem: ChartItem = this.DataSource.filter(f => f.Id == item.Key)[0];
            if (chartItem) {
                chartItem.Values.push(new ChartItemValue(item.Label, item.ProfitInLocal, item.ProfitInProfit));
            }

            else {
                chartItem = new ChartItem(item.Key, item.SalesmanUserName);
                chartItem.Index = index;
                chartItem.Values.push(new ChartItemValue(item.Label, item.ProfitInLocal, item.ProfitInProfit));
                this.DataSource.push(chartItem);
                index++;
            }
        });
    }

    FillChartData() {

        var graphs: any[] = [];
        var dataProvider: any[] = [];

        if (this.DataSource.length > 0) {

            var colors: string[] = ['#6AA4D9', '#ADADAD', '#EF8E4C', '#FFC208', '#84B761'];

            this.DataSource.forEach(item => {

                var itemColor = colors[item.Index];
                var itemUniqueKey = item.Id;

                graphs.push({
                    id: item.Index.toString(),
                    "useNegativeColorIfDown": false,
                    "bullet": "round",
                    "bulletBorderAlpha": 1,
                    "bulletBorderColor": itemColor,
                    "hideBulletsCount": 50,
                    "lineThickness": 2,
                    "lineColor": itemColor,
                    "negativeLineColor": itemColor,
                    "valueField": itemUniqueKey,
                    "title": item.Name,
                });

                item.Values.forEach(itemValue => {
                    var itemProvider = dataProvider.filter(f => f.Category == itemValue.Label)[0];
                    if (itemProvider) {
                        itemProvider[itemUniqueKey] = this.SelectedCurrency == "1" ? itemValue.ValueInLocal : itemValue.ValueInProfit;
                    }

                    else {
                        itemProvider = {};
                        itemProvider["Category"] = itemValue.Label;
                        itemProvider[itemUniqueKey] = this.SelectedCurrency == "1" ? itemValue.ValueInLocal : itemValue.ValueInProfit;
                        dataProvider.push(itemProvider);
                    }
                });

            });
        }

        document.getElementById(this.LegendDiv).innerHTML = "";

        makeAMLineChartMultiple(this.PerformanceChartId, dataProvider, null, graphs, true, this.LegendDiv, "Profit");
    }   
}

class ChartItem {
    public Id: string;
    public Name: string;
    public Index: number;
    public Values: ChartItemValue[];
    constructor(id: string, name: string) {
        this.Id = id;
        this.Name = name;
        this.Index = 0;
        this.Values = [];
    }
}

class ChartItemValue {
    public Label: string;
    public ValueInLocal: number;
    public ValueInProfit: number;
    constructor(label: string, valueInLocal: number, valueInProfit: number) {
        this.Label = label;
        this.ValueInLocal = valueInLocal;
        this.ValueInProfit = valueInProfit;
    }
}


