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
    public PerformanceChartIdExistance: Boolean = false;
    public PerformanceChartId: string;
    public LegendDiv: string;
    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;
    public SelectedCurrency: string = "1";

    constructor(private _entityResourceService: EntityResourceService) {
        this.PerformanceChartId = "PerformanceChartId_" + this.CurrentSession.GetNewId("PerformanceChartId");
        this.LegendDiv = "LegendDiv_" + this.CurrentSession.GetNewId("LegendDiv");
    }

    ngOnInit() {
        this.dashboardArgs = new QuoteDashboardArguments();
        this.dashboardService = new DashboardService();
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            this.FillChartArgs();
            this.LoadChartData();
        });
    }

    // Load chart Data
    public TopFiveSalesmanData: any[] = [];
    LoadChartData() {
        this.dashboardService.GetDashboardChartValues(this.dashboardArgs).subscribe((myResult: any) => {
            if (myResult != null && !myResult.HasError) {
                this.TopFiveSalesmanData = myResult;
                this.FillPerformanceChart();
            }
            else
                this.PerformanceChartIdExistance = false;
        });
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

    private Wizard: QuoteDashboardComponent;
    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
    }

    RefreshTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        this.FillChartArgs();
        this.LoadChartData();
    }

    public SalesmanNumber: number;

    FillPerformanceChart() {
        this.DeterminePeriod(this.TopFiveSalesmanData);

        var colors: string[] = ['#6AA4D9', '#ADADAD', '#EF8E4C', '#FFC208', '#84B761'];
        var graphs: any[] = [];
        var valueNumber: string;
        var idNumber: string;
        for (var i = 0; i < this.SalesmanNumber; i++) {
            valueNumber = "Value" + (i+1).toString();
            idNumber = "g" + (i+1).toString();
            graphs.push({
                id: idNumber,
                "useNegativeColorIfDown": false,
                "bullet": "round",
                "bulletBorderAlpha": 1,
                "bulletBorderColor": colors[i],
                "hideBulletsCount": 50,
                "lineThickness": 2,
                "lineColor": colors[i],
                "negativeLineColor": colors[i],
                "valueField": valueNumber,
                "title": this.SalesmanNames[i] != null ? this.SalesmanNames[i] : "",
            })
        }

        if (this.PerformanceChartIdExistance)
            makeAMLineChartMultiple(this.PerformanceChartId, this.dataProvider, null, graphs, true, this.LegendDiv, "Profit");

    }

    public dataProvider: any = [];

    DeterminePeriod(data) {

        if (data)
            if (this.TopFiveSalesmanData.length > 0)
                this.PerformanceChartIdExistance = true;
        
        if (this.PerformanceChartIdExistance) {
            this.dataProvider = [];
            this.SalesmanNames = [];
            switch (this.dashboardArgs.DatesCode) {
                case '0':
                case '-1': {
                    this.SalesmanNumber = this.TopFiveSalesmanData.length;
                    this.GroupingDataByOne(data);
                    break;
                }
                case '-7': {
                    this.SalesmanNumber = this.TopFiveSalesmanData.length / 7;
                    this.GroupingDataBySeven(data);
                    break;
                }
                case '-30': {
                    this.SalesmanNumber = this.TopFiveSalesmanData.length / 5;
                    this.GroupingDataByFive(data);
                    break;
                }
                case '-90': {
                    this.SalesmanNumber = this.TopFiveSalesmanData.length / 3;
                    this.GroupingDataByMonth(data);
                    break;
                }
                case "-365": {
                    this.SalesmanNumber = this.TopFiveSalesmanData.length / 4;
                    this.GroupingDataByQuarter(data);
                    break;
                }
                case "-2": {
                    this.SalesmanNumber = this.TopFiveSalesmanData.length / 4;
                    this.GroupingDataByQuarter(data);
                }
            }
        }
    }
  
    GroupingDataByFive(data) {
        var category1: any = [];
        var category2: any = [];
        var category3: any = [];
        var category4: any = [];
        var category5: any = [];
        var index = 0;
        data.forEach(item => {
            var value = index % 5;
            index++;
            switch (value) {
                case 0: {
                    category1.push(item);
                    break;
                }
                case 1: {
                    category2.push(item);
                    break;
                }
                case 2: {
                    category3.push(item);
                    break;
                }
                case 3: {
                    category4.push(item);
                    break;
                }
                case 4: {
                    category5.push(item);
                    break;
                }
            }

        });

        this.GetSalesmanNames(category1);
        this.MapData(category1, 0);
        this.MapData(category2, 1);
        this.MapData(category3, 2);
        this.MapData(category4, 3);
        this.MapData(category5, 4);
    }

    GroupingDataByQuarter(data) {
        var category1: any = [];
        var category2: any = [];
        var category3: any = [];
        var category4: any = [];
        var index = 0;
        data.forEach(item => {
            var value = index % 4;
            index++;
            switch (value) {
                case 0: {
                    category1.push(item);
                    break;
                }
                case 1: {
                    category2.push(item);
                    break;
                }
                case 2: {
                    category3.push(item);
                    break;
                }
                case 3: {
                    category4.push(item);
                    break;
                }
            }
        });

        this.GetSalesmanNames(category1);
        this.MapData(category1, 0);
        this.MapData(category2, 1);
        this.MapData(category3, 2);
        this.MapData(category4, 3);
    }

    GroupingDataByOne(data) {
        this.MapData(data, 0);
    }

    GroupingDataBySeven(data) {
        var category1: any = [];
        var category2: any = [];
        var category3: any = [];
        var category4: any = [];
        var category5: any = [];
        var category6: any = [];
        var category7: any = [];
        var index = 0;
        data.forEach(item => {
            var value = index % 7;
            index++;
            switch (value) {
                case 0: {
                    category1.push(item);
                    break;
                } 
                case 1: {
                    category2.push(item);
                    break;
                }
                case 2: {
                    category3.push(item);
                    break;
                }
                case 3: {
                    category4.push(item);
                    break;
                }
                case 4: {
                    category5.push(item);
                    break;
                }
                case 5: {
                    category6.push(item);
                    break;
                }
                case 6: {
                    category7.push(item);
                    break;
                }
            }

        });

        this.GetSalesmanNames(category1);
        this.MapData(category1, 0);
        this.MapData(category2, 1);
        this.MapData(category3, 2);
        this.MapData(category4, 3);
        this.MapData(category5, 4);
        this.MapData(category6, 5);
        this.MapData(category7, 6);
    }

    GroupingDataByMonth(data) {
        var category1: any = [];
        var category2: any = [];
        var category3: any = [];
        var index = 0;
        data.forEach(item => {
            var value = index % 3;
            index++;
            switch (value) {
                case 0: {
                    category1.push(item);
                    break;
                }
                case 1: {
                    category2.push(item);
                    break;
                }
                case 2: {
                    category3.push(item);
                    break;
                }
            }
        });
        this.GetSalesmanNames(category1);
        this.MapData(category1, 0);
        this.MapData(category2, 1);
        this.MapData(category3, 2);
    }

    MapData(data, i) {
        this.dataProvider[i] = {
            "Category": data[0] != null ? data[0].Label : null,
            "Value1": data[0] != null ? data[0].Value : 0,
            "Value2": data[1] != null ? data[1].Value : 0,
            "Value3": data[2] != null ? data[2].Value : 0,
            "Value4": data[3] != null ? data[3].Value : 0,
            "Value5": data[4] != null ? data[4].Value : 0
        };
    }
     
    public SalesmanNames: string[] = [];
     
    GetSalesmanNames(data) {
        var i = 0;
        data.forEach(item => {
            this.SalesmanNames[i] = item.SalesmanUserName; 
            i++
        });
    }

    ChangeCurrency(code: string) {

        if (code == this.LocalCurrencyCode)
            code = "1";
        else
            code = "2";

        if (code != this.dashboardArgs.SelectedCurrency) {
            this.dashboardArgs.SelectedCurrency = code;
            this.SelectedCurrency = code;
            this.LoadChartData();
        }
    }
    
}
