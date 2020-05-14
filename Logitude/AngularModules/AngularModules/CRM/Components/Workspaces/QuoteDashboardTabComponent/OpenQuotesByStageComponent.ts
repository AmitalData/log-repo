import { Component } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
declare var makeChart, FunnelClick, ResetItemFunnel;

@Component({
    selector: 'open-quote-by-stage',
    templateUrl: './OpenQuotesByStageComponent.html'
})

export class OpenQuotesByStageComponent {
    public ChartId: string = "SalesFunnelId_";
    private chartService: DashboardService;
    private chartArgs: QuoteDashboardArguments;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.chartService = new DashboardService();
        this.chartArgs = new QuoteDashboardArguments();
        this.chartArgs.ChartCode = "OQS";
        this.ChartId = "SalesFunnel_" + this.CurrentSession.GetNewId("SalesFunnel");
    }

    Update(comp: QuoteDashboardComponent) {
        this.chartArgs.OwnerId = comp.OwnerId;
        this.chartArgs.BusinessUnitId = comp.BusinessUnitId;
        this.chartArgs.FromDate = comp.FromDate;
        this.chartArgs.ToDate = comp.ToDate;
        this.LoadFunnelData();
    }

    public FunnelData: any;
    public FunnelDataFilterd = [];
    LoadFunnelData() {
        this.chartService.GetDashboardChartValues(this.chartArgs).subscribe((myResult: any) => {
            this.FunnelData = myResult;
            this.FillFunnelData();
        });
    }

    FillFunnelData() {
        try {
            var labelArr: Array<any> = [];
            var dataArr: Array<any> = [];
            this.FunnelDataFilterd = [];
            var i = 0;
            var sum = 0;

            this.FunnelData.forEach(p => {
                this.FunnelDataFilterd[i] = { title: p.LabelProperty, value: p.DecimalProperty };
                i++;
                sum += p.DecimalProperty;
            });

            makeChart(this.ChartId, this.FunnelDataFilterd, sum);
            var els = document.getElementsByTagName('a');
            els[1].remove();
        }

        catch (e) { }
    }

    FunnelClick() {
        var item = FunnelClick();
        ResetItemFunnel();

        if (item != null) {

            var objectTableName = "Quote";
            var queryCode = "Open Quotes";
            var displayTitle = this.FunnelData[item.index].LabelProperty + " Quotes";
            var backButtonTitle = TextCodeTranslator.Translate("General.MH.Quotes");

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
            var listArgs = new ListComponentArgs();

            filterAgrs.addAdditionalFilter("StageId", this.FunnelData[item.index].GroupedId, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("BusinessUnitId", this.chartArgs.BusinessUnitId, null, null, "Equals", true, false, false, "string");
            filterAgrs.addAdditionalFilter("SalesmanUserId", this.chartArgs.OwnerId, null, null, "Equal", false, false, false, "string");
            filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper.GetDateString(this.chartArgs.FromDate), ServiceHelper.GetDateString(this.chartArgs.ToDate), null, "Equals", true, false, false, "String");

            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe();
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }
    }
}
