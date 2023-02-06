import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { FeatureLocator } from "Infrastructure/Utilities/FeatureLocator";
import { ContainerPMExtendedService } from "Shipment/Services/ExtendedPMs/ContainerPMExtendedService";

@Component({
    templateUrl: './ContainerViewsGraphComponent.html',
    selector: 'ContainerViewsGraph',
})

export class ContainerViewsGraphComponent extends BaseComponent implements OnInit {
    @Output() OnViewClick = new EventEmitter<string>();
    private ContainerPMExtendedService: ContainerPMExtendedService;
    public DataSource: any = {};
    public IsViewsGraphActive: boolean;

    constructor() {
        super()
        this.ContainerPMExtendedService = new ContainerPMExtendedService();
        this.BuildChartInfo();
        this.IsViewsGraphActive = FeatureLocator.HasFeaturePermession("Container", "VIEWSGRAPH");
    }

    BuildChartInfo() {
        var chartInfo = {} as any;
        chartInfo.theme = "fusion";
        chartInfo.enableSlicing = "0";
        chartInfo.enableRotation = "0";
        chartInfo.chartTopMargin = "10";
        chartInfo.chartBottomMargin = "10";
        chartInfo.chartLeftMargin = "40";
        chartInfo.chartRightMargin = "40";
        chartInfo.toolTipBgColor = "#FFFFFF";
        chartInfo.showToolTipShadow = "1";
        chartInfo.scrollHeight = "6";
        chartInfo.scrollWidth = "6";
        chartInfo.scrollPadding = "8";
        chartInfo.flatScrollBars = "1";
        chartInfo.showPercentValues = "1";
        chartInfo.showLegend = "1";
        chartInfo.divLineDashed = "1";
        chartInfo.divLineDashLen = "5";
        chartInfo.divLineDashGap = "5";
        chartInfo.divLineColor = "#E7E7E7";
        chartInfo.divLineAlpha = "100";
        chartInfo.divLineThickness = "1";
        chartInfo.chartTopMargin = "20";
        chartInfo.chartBottomMargin = "10";
        chartInfo.chartLeftMargin = "20";
        chartInfo.chartRightMargin = "50";
        chartInfo.canvasBottomPadding = "10";
        this.DataSource.chart = chartInfo;
    }

    ngOnInit(): void {
        this.GetData();
    }

    public GetData() {
        if (!this.IsViewsGraphActive) return;

        this.DataSource.data = null;
        this.ContainerPMExtendedService.GetViewsGraphData().subscribe((myResult: any) => {
            var pmResponse: ServiceResponse = myResult;
            if (pmResponse.HasError) return;
            this.BuildSimpleDataSource(pmResponse.Result?.Datas);
        });
    }

    private BuildSimpleDataSource(seriesMeasures: ContainerViewsGraphDataItem[]): any {
        this.DataSource.data = this.BuildDataSource(seriesMeasures);
    }

    private BuildDataSource(seriesMeasures: ContainerViewsGraphDataItem[]) {
        let data: any[] = [];
        if (!seriesMeasures || !seriesMeasures[0]) return data;

        data = seriesMeasures.map((e, index) => {
            let item = {
                value: e.Value,
                label: e.Label,
                id: e.QueryCode,
                tooltext: e.Value + " " + e.ToolTip,
                color: this.GetSeriesPositionColor(index + 1)
            }
            return item;
        });
        return data;
    }

    private GetSeriesColor(position: number): string | null {
        if (position % 2 == 0) return mainBlueColor;
        return mainGreenColor;
    }


    private GetSeriesPositionColor(position: number): string | null {
        switch (position) {
            case 1: return mainBlueColor;
            case 2: return mainGreenColor;
            case 3: return mainYellowColor;
            case 4: return mainTurquoiseColor;
            case 5: return mainPurpleColor;
            case 6: return mainOrangeColor;
            case 7: return mainRedColor;
            default: return null;
        }
    }

    public DataplotClick(event: any) {
        var queryCode = event?.dataObj?.id;
        if (queryCode) this.OnViewClick.emit(queryCode);
    }
}

class ContainerViewsGraphDataItem {
    public QueryCode: string;
    public Value: number;
    public Label: string;
    public ToolTip: string;
}

export const mainYellowColor = "#F6CF33"
export const mainRedColor = "#EE5F77"
export const mainGreenColor = "#4AC76F"
export const mainTurquoiseColor = "#32C7C7"
export const mainBlueColor = "#369CFB"
export const mainPurpleColor = "#935BE0"
export const mainOrangeColor = "#FAAD14"