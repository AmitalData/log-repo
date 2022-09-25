import { WidgetMeasurePM } from "DashboardModule/EntityPMs/WidgetMeasurePM";
import { WidgetPM } from "DashboardModule/EntityPMs/WidgetPM";
import { ReactWidgetMeasurePM } from "logitude-dashboard-library/dist/types/ReactWidgetMeasurePM";
import { ReactWidgetPM } from "logitude-dashboard-library/dist/types/widget";
import { BehaviorSubject, Subject } from 'rxjs';
export class DashboardMapping{
    public static GetReactWidget(widget: WidgetPM): ReactWidgetPM {
        var myWidget: ReactWidgetPM = {} as ReactWidgetPM;

        if (widget) {
            myWidget.Id = widget.Id;
            myWidget.key= widget.Id ?? widget.Key;
            myWidget.Tenant = widget.Tenant;
            myWidget.Title = widget.Title;
            myWidget.GroupById = widget.GroupById;
            myWidget.DashboardId = widget.DashboardId;
            myWidget.StartPotistion = widget.StartPotistion;
            myWidget.EndPosition = widget.EndPosition;
            myWidget.EntityId = widget.EntityId;
            myWidget.TypeCode = widget.TypeCode as "line" | "area" | "bar" | "histogram" | "pie" | "donut" | "radialBar" | "scatter" | "bubble" | "heatmap" | "treemap" | "boxPlot" | "candlestick" | "radar" | "polarArea" | "rangeBar";
            myWidget.WidgetMeasures = [];
            myWidget.Filters = widget.Filters;
            myWidget.DateGroupCode = widget.DateGroupCode;
            myWidget.SortBy = widget.SortBy;
            myWidget.SortDirection = widget.SortDirection;
            myWidget.MaximumGrouping = widget.MaximumGrouping;
            myWidget.Layout={
                minH: 5,
                minW: 3,
                i: myWidget.key,
                w: +widget.EndPosition.split(',')[0],
                h: +widget.EndPosition.split(',')[1],
                x: +widget.StartPotistion.split(',')[0],
                y: +widget.StartPotistion.split(',')[1],
              }
            widget.WidgetMeasures.forEach(item => {
                myWidget.WidgetMeasures.push(this.GetReactWidgetMeasure(item));
            });
        }

        return myWidget;
    }

    public static GetReactWidgetMeasure(widgetMeasure: WidgetMeasurePM): ReactWidgetMeasurePM {
        var myWidgetMeasuer: ReactWidgetMeasurePM = {} as ReactWidgetMeasurePM;

        if (widgetMeasure) {
            myWidgetMeasuer.Id = widgetMeasure.Id;
            myWidgetMeasuer.Tenant = widgetMeasure.Tenant;
            myWidgetMeasuer.WidgetId = widgetMeasure.WidgetId;
            myWidgetMeasuer.MeasureCode = widgetMeasure.MeasureCode;
            myWidgetMeasuer.MeasureFieldId = widgetMeasure.MeasureFieldId;
        }

        return myWidgetMeasuer;
    }

    public static GetWidgetPMFromReact(Widget:ReactWidgetPM){
        var w = {...Widget}
        w.onChange = undefined;
        return JSON.parse(JSON.stringify(w))
    }


    public static deepClone(obj){
        var clone = JSON.parse(JSON.stringify(obj,
            function (key, val) {
                if (key !== "UIProperties" 
                && key != 'entityParentPM'
                && key != 'PropertyChanged'
                && key != 'PropertyChanged'
                && key != 'OldEntityPM') return val
            }
            )) ;

        return clone;
    }
    public static deepCloneToType(obj,toObj:any){
        var clone = JSON.parse(JSON.stringify(obj,
            function (key, val) {
                if (key !== "UIProperties" 
                && key != 'entityParentPM'
                && key != 'PropertyChanged'
                && key != 'PropertyChanged'
                && key != 'OldEntityPM') return val
            }
            )) ;
            Object.assign(toObj,clone);
        return toObj;
    }



}
