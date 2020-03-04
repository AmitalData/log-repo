import { Component, Input, OnInit } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { QuoteDomainService } from '../../../../Quote/Services/QuoteDomainService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
import { DashboardService } from '../../../../Quote/Services/QuoteDashboard/DashboardService';
import { QuoteDashboardArguments } from '../../../../Quote/DataContracts/QuoteDashboardArguments';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
declare var makeChart, FunnelClick, ResetItemFunnel;
@Component({
    selector: 'open-quote-by-stage',
    moduleId: module.id,
    templateUrl: './OpenQuotesByStageComponent.html'
})

export class OpenQuotesByStageComponent implements OnInit {

    public SalesFunnelId: string = "SalesFunnelId_";
    private CurrentSession = SessionLocator.SelectedSession;
    private dashboardService: DashboardService;
    private funnelArgs: QuoteDashboardArguments;
    
    constructor(private _entityResourceService: EntityResourceService) {
        this.SalesFunnelId = "SalesFunnel_" + this.CurrentSession.GetNewId("SalesFunnel");
    }

    ngOnInit() {
        this.funnelArgs = new QuoteDashboardArguments();
        this.dashboardService = new DashboardService();
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            this.FillFunnelArgs();
            this.LoadFunnelData();
        });
    }

    FillFunnelArgs() {
        this.funnelArgs.OwnerId = this.Wizard.OwnerId;
        this.funnelArgs.BusinessUnitId = this.Wizard.BusinessUnitId;
        this.funnelArgs.FromDate = this.Wizard.FromDate;
        this.funnelArgs.ToDate = this.Wizard.ToDate;
        this.funnelArgs.ChartCode = "OQS";
    }

    // Load Funnel Data
    public FunnelData: any; 
    public FunnelDataFilterd = [];
    LoadFunnelData() {
        this.dashboardService.GetDashboardChartValues(this.funnelArgs).subscribe((myResult: any) => {
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

            makeChart(this.SalesFunnelId, this.FunnelDataFilterd, sum);
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
            var queryCode = "All Quotes";
            var displayTitle = this.FunnelData[item.index].LabelProperty + " Quotes";
            var backButtonTitle = TextCodeTranslator.Translate("General.MH.Quotes");

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
            var listArgs = new ListComponentArgs();

            filterAgrs.addAdditionalFilter("StageId", this.FunnelData[item.index].GroupedId, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("BusinessUnitId", this.funnelArgs.BusinessUnitId, null, null, "Equals", true, false, false, "string");
            filterAgrs.addAdditionalFilter("CreatedByUserId", this.funnelArgs.OwnerId, null, null, "Equal", false, false, false, "string");
            filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper.GetDateString(this.Wizard.FromDate), ServiceHelper.GetDateString(this.Wizard.ToDate), null, "Equals", true, false, false, "String");

            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe();
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }

    private Wizard: QuoteDashboardComponent;
    InitTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
    }

    RefreshTab(wizard: QuoteDashboardComponent) {
        this.Wizard = wizard;
        this.FillFunnelArgs();
        this.LoadFunnelData();
    }
}
