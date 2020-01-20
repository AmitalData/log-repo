import { Component, Input, OnInit } from '@angular/core';
import { QuoteDashboardComponent } from './QuoteDashboardComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { QuoteDomainService } from '../../../../Quote/Services/QuoteDomainService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ListComponentArgs } from '../../../../Infrastructure/Args';
declare var makeChart, FunnelClick, ResetItemFunnel;
@Component({
    selector: 'open-quote-by-stage',
    moduleId: module.id,
    templateUrl: './OpenQuotesByStageComponent.html'
})

export class OpenQuotesByStageComponent implements OnInit {

    public SalesFunnelId: string = "SalesFunnelId_";
    private CurrentSession = SessionLocator.SelectedSession;
    private quouteDomainService: QuoteDomainService;
 
    constructor(private _entityResourceService: EntityResourceService) {
        this.SalesFunnelId = "SalesFunnel_" + this.CurrentSession.GetNewId("SalesFunnel");
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            this.LoadFunnelData();
        });
    }

    ngOnInit() {
        this.quouteDomainService = new QuoteDomainService(); 
    }

    // Load Funnel Data
    public FunnelData: any;
    public FunnelDataFilterd = [];
    LoadFunnelData() {
        this.quouteDomainService.GetStageFunnelData(this.Wizard.OwnerId, this.Wizard.BusinessUnitId, null).subscribe((myResult: any) => {
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
            var queryCode = "Open Quotes";
            var displayTitle = this.FunnelData[item.index].LabelProperty + " Quotes";
            var backButtonTitle = TextCodeTranslator.Translate("General.MH.Quotes");

            var filterAgrs: ApiQueryFilters = new ApiQueryFilters();
            var listArgs = new ListComponentArgs();
            var myOwnerId = null;
            var myBusinessUnitId = null;
            var myFilterCode = null;

            if (this.FunnelData[item.index].OwnerId != null && this.FunnelData[item.index].OwnerId != "") {
                myOwnerId = this.FunnelData[item.index].OwnerId;
            }

            if (this.FunnelData[item.index].BusinessUnitId != null && this.FunnelData[item.index].BusinessUnitId != "") {
                myBusinessUnitId = this.FunnelData[item.index].BusinessUnitId;
            }

            if (this.FunnelData[item.index].DataTypeCode != null && this.FunnelData[item.index].DataTypeCode != "") {
                myFilterCode = this.FunnelData[item.index].DataTypeCode;
            }

            filterAgrs.addAdditionalFilter("StageId", this.FunnelData[item.index].GroupedId, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", true, false, false, "string");
          

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
        this.LoadFunnelData();
        console.log("Init Tab");
    }

    RefreshTab() {
        console.log("Refresh Tab");
    }
}
