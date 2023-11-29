import { WidgetMeasurePM } from "DashboardModule/EntityPMs/WidgetMeasurePM";
import { WidgetPM } from "DashboardModule/EntityPMs/WidgetPM";
import { ReactWidgetMeasurePM } from "logitude-dashboard-library/dist/types/ReactWidgetMeasurePM";
import { ReactWidgetPM } from "logitude-dashboard-library/dist/types/widget";

export class DashboardMapping {
    public static GetReactWidget(widget: WidgetPM): ReactWidgetPM {
        var myWidget: ReactWidgetPM = {} as ReactWidgetPM;

        if (widget) {
            myWidget.Id = widget.Id;
            myWidget.key = widget.Id ?? widget.Key;
            myWidget.Tenant = widget.Tenant;
            myWidget.Title = widget.Title;
            myWidget.GroupById = widget.GroupById;
            myWidget.DashboardId = widget.DashboardId;
            myWidget.StartPotistion = widget.StartPotistion;
            myWidget.EndPosition = widget.EndPosition;
            myWidget.EntityId = widget.EntityId;
            myWidget.TypeCode = widget.TypeCode;
            myWidget.WidgetMeasures = [];
            myWidget.Filters = widget.Filters;
            myWidget.DateGroupCode = widget.DateGroupCode;
            myWidget.SortBy = widget.SortBy;
            myWidget.SortDirection = widget.SortDirection;
            myWidget.MaximumGrouping = widget.MaximumGrouping;
            myWidget.TimeOverTime = widget.TimeOverTime;
            myWidget.ComparisonOperator = widget.ComparisonOperator;
            myWidget.ComparisonPeriod = widget.ComparisonPeriod;
            myWidget.ComparisonDateGroup = widget.ComparisonDateGroup;
            myWidget.Increase = widget.Increase;
            myWidget.FromDate = widget.FromDate;
            myWidget.ToDate = widget.ToDate;
            myWidget.SecondaryGroupById = widget.SecondaryGroupById;
            myWidget.SecondaryDateGroupCode = widget.SecondaryDateGroupCode;
            myWidget.Alighnment = widget.Alignment;
            myWidget.ThousandSeparator = widget.ThousandSeparator;
            myWidget.UseNumberAbbreviation = widget.UseNumberAbbreviation;
            myWidget.UseAbbreviationAfter = widget.UseAbbreviationAfter;
            myWidget.DecimalPlaces = widget.DecimalPlaces;
            myWidget.LabelsPosition = widget.LabelsPosition;
            myWidget.Layout = {
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

        if (myWidget.TypeCode == "kpi") {
            myWidget.Layout.minH = 2;
            myWidget.Layout.minW = 2;
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

    public static GetWidgetPMFromReact(Widget: ReactWidgetPM) {
        return JSON.parse(JSON.stringify(Widget, function (key, val) {
            if (key !== "onChange")
                return val
        }))
    }

    public static deepClone(obj) {
        var clone = JSON.parse(JSON.stringify(obj,
            function (key, val) {
                if (key !== "UIProperties"
                    && key != 'entityParentPM'
                    && key != 'PropertyChanged'
                    && key != 'PropertyChanged'
                    && key != 'OldEntityPM'
                    ) return val
            }
        ));

        return clone;
    }




}
