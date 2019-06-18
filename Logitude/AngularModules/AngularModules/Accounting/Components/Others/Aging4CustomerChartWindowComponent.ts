declare var makeAmBarChart;
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {ReconciliationPM} from '../../EntityPMs/ReconciliationPM';
import {ReconciliationLinePM} from '../../EntityPMs/ReconciliationLinePM';
import {GLAccountTotalByMonthList} from '../../EntityLists/GLAccountTotalByMonthList';
import {LedgerTransactionPM} from '../../EntityPMs/LedgerTransactionPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters, FilterItem} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../Infrastructure/Tools';
import {GLAccountExtendedListService} from '../../Services/ExtendedLists/GLAccountExtendedListService';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { AgingReportParameters } from '../../DataContracts/AgingReportParameters';
import { PeriodM } from '../../DataContracts/PeriodM';

@Component({
    selector: 'Aging4CustomerChartWindowComponent',
    moduleId: module.id,
    templateUrl: './Aging4CustomerChartWindowComponent.html',
})

export class Aging4CustomerChartWindowComponent extends BaseComponent implements AfterViewInit {
    public DataContext: Aging4CustomerChartWindowComponent = this;
    _GLAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();

    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "GLAccountTotalByMonth";
    public EntityList: GLAccountTotalByMonthList;
    AccountId: string;
    CardId: string;
    noCard: boolean = false;

    constructor() {
        super();

    }

    ngAfterViewInit() {
        this.LoadChartData();
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.AccountId = args.accountId;
            this.CardId = args.cardId;
            this.EntityList = args.entity;

            if (AppTool.IsNullOrEmpty(this.CardId))
                this.noCard = true;
            else
                this.noCard = false;

        }
    }

    OkButtonClicked() {
        SessionLocator.CurrentSession.CurrentWindow.Close("ok");
    }

    //#region Chart Code

    public barChartLabels: string[] = [];
    public barChartData: any[] = [{
        data: [], label: '', scaleShowVerticalLines: false,
    }];


    LoadChartData() {
        if (AppTool.IsNullOrEmpty(this.AccountId))
            return;

        var numberOfmonthsbackwards = 3;

        var args = new AgingReportParameters();

        args.Tenant = SessionLocator.Tenant,
            args.AgingForDate = new Date();
        args.NumberOfmonthsbackwards = numberOfmonthsbackwards;
        args.VendorCustomerId = this.AccountId;
        //args.Category1Id = "";
        //args.Category2Id = "";
        //args.Category3Id = "";
        //args.Category4Id = "";
        //args.Category5Id = "";
        //args.CollectorId = SessionLocator.LoggedUserId;
        //args.SalesmanId = "";


        this._GLAccountExtendedListService.GetAgingReport(args).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var periods: PeriodM[];
                    periods = res;
                    console.log("periods: ", periods);
                    this.LoadChart(periods);
                }
            }
        });

    }

    LoadChart(data: PeriodM[]) {

        //#region Graph metadata
        var max = 0;
        var DataProvider = [];
        var i = 0;
        var index = 0;
        this.barChartData[0].data = [];
        this.barChartLabels = [];
        this.barChartData = [{
            data: [], label: '', scaleShowVerticalLines: false,
        }];

        //Graph Properties
        var Graphs = Graphs = [{
            "balloonText": "[[value]]",
            "fillAlphas": 1,
            "id": "AmGraph-1" + i,
            "title": "Aging",
            "type": "column",
            "valueField": "dataCol",
            "fillColors": ["#FFAD49", "#FFC47C", "#FFC47C", "#FFAD49",],
            "gradientOrientation": "horizontal",
            "borderAlpha": 0,
            "lineColor": "#fff"

        }]
        //#endregion

        data.forEach(element => {
            //if (element.Total <= 0) return;
            this.barChartData[0].label = "Amount";

            // Amount
            var value = element.Total;// + (Math.floor((Math.random() * 2500) + 1));
            this.barChartData[0].data[i] = value.toString();

            // Labels
            var label = element.PeriodName.replace("b4", "Before"); // replace 'b4' with 'Before'
            label = label.startsWith("Before") ? label.replace("/20", "/") : label; // minimize year in 'Before' Column
            this.barChartLabels[i] = label;

            // Data
            DataProvider[i] = { "category": this.barChartLabels[i], "dataCol": this.barChartData[0].data[i] };

            if (value > max)
                max = value;


            i++;
        });

        makeAmBarChart("GLAccountAgingChartChart2", Graphs, DataProvider, max);

    }

    //#endregion




}
