import { DashboardGlobalFilterPM } from "DashboardModule/EntityPMs/DashboardGlobalFilterPM";
import { DashboardPM } from "DashboardModule/EntityPMs/DashboardPM";
import { DashboardSharedUserPM } from "DashboardModule/EntityPMs/DashboardSharedUserPM";
import { WidgetMeasurePM } from "DashboardModule/EntityPMs/WidgetMeasurePM";
import { WidgetPM } from "DashboardModule/EntityPMs/WidgetPM";
import { SessionInfo } from "Infrastructure/Utilities/SessionInfo";
import { DateTool } from '../../Infrastructure/Tools';
import { DashboardMapping } from "./DashboardMapping";

export class DashboardCopyService {
   
    public static CopyDashboard(sourceDashboard: DashboardPM): DashboardPM {
        var dashboard: DashboardPM = new DashboardPM();
        //dashboard = DashboardMapping.deepClone(sourceDashboard);
        // dashboard.Id = null;
        dashboard.Tenant = SessionInfo.LoggedUserTenant;
        dashboard.CreatedByUserId = SessionInfo.LoggedUserId;
        dashboard.UpdatedByUserId = SessionInfo.LoggedUserId;
        dashboard.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        dashboard.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        dashboard.Name = "Copy of " + sourceDashboard.Name;
        dashboard.Description = sourceDashboard.Description;
        this.CopyWidgets(sourceDashboard, dashboard);
        dashboard.PermissionLevelCode = sourceDashboard.PermissionLevelCode;
        this.CopyUsers(sourceDashboard, dashboard);
        this.CopyGlobalFiltyers(sourceDashboard, dashboard);
        dashboard.LoadedAutomatically = sourceDashboard.LoadedAutomatically;
        dashboard.PredefinedOrder = sourceDashboard.PredefinedOrder;
 
        return dashboard;

    }
    static CopyUsers( sourceDashboard: DashboardPM, dashboard: DashboardPM) {   
        // if(!sourceDashboard.DashboardSharedUsers || sourceDashboard.DashboardSharedUsers.length == 0) return;
        // dashboard.DashboardSharedUsers = [];
        // sourceDashboard.DashboardSharedUsers.forEach(sourceUser => {
        //     var user: DashboardSharedUserPM = new DashboardSharedUserPM(dashboard);
        //     user  = DashboardMapping.deepClone(sourceUser);
        //     user.Tenant = SessionInfo.LoggedUserTenant;
        //     user.DashboardId = dashboard.Id;
        //     user.Id = null;
        //     dashboard.DashboardSharedUsers.push(user)
        // });

        sourceDashboard.DashboardSharedUsers.forEach(sourceUser => {
                var user: DashboardSharedUserPM = new DashboardSharedUserPM(dashboard);
                user.Tenant = SessionInfo.LoggedUserTenant;
                user.DashboardId = dashboard.Id;
                user.UserId = sourceUser.UserId;
                user.UserName = sourceUser.UserName;
                dashboard.DashboardSharedUsers.push(user);
            });
    }

    static CopyWidgets(sourceDashboard: DashboardPM, dashboard: DashboardPM){
        // if(!sourceDashboard.Widgets || sourceDashboard.Widgets.length == 0) return;
        // dashboard.Widgets = [];
        // sourceDashboard.Widgets.forEach(sourceWidget => {
        //     var widget: WidgetPM = new WidgetPM(dashboard);
        //     widget  = DashboardMapping.deepClone(sourceWidget);
        //     widget.Tenant = SessionInfo.LoggedUserTenant;
        //     widget.DashboardId = dashboard.Id;
        //     widget.Id = null;
        //     this.CopyWidgetMeasures(sourceWidget, widget);
        //     dashboard.Widgets.push(widget)
        // });

        sourceDashboard.Widgets.forEach (sourceWidget => {
            var widget : WidgetPM = new WidgetPM(dashboard);
            
            widget.Tenant = SessionInfo.LoggedUserTenant;
            widget.DashboardId = dashboard.Id;
            widget.Title = sourceWidget.Title;
            widget.GroupById = sourceWidget.GroupById;
            widget.StartPotistion = sourceWidget.StartPotistion;
            widget.EndPosition = sourceWidget.EndPosition;
            widget.TypeCode = sourceWidget.TypeCode;
            widget.EntityId = sourceWidget.EntityId;
            this.CopyWidgetMeasures(sourceWidget, widget);
            widget.Filters = sourceWidget.Filters;
            widget.DateGroupCode = sourceWidget.DateGroupCode;
            widget.MaximumGrouping = sourceWidget.MaximumGrouping;
            widget.SortBy = sourceWidget.SortBy;
            widget.SortDirection = sourceWidget.SortDirection;
            widget.Key = sourceWidget.Key;
            widget.TimeOverTime = sourceWidget.TimeOverTime;
            widget.ComparisonPeriod = sourceWidget.ComparisonPeriod;
            widget.Increase = sourceWidget.Increase;
            widget.ComparisonOperator = sourceWidget.ComparisonOperator;
            widget.ComparisonDateGroup = sourceWidget.ComparisonDateGroup;
            widget.FromDate = sourceWidget.FromDate;
            widget.ToDate = sourceWidget.ToDate;
            widget.GlobalFilters = sourceWidget.GlobalFilters;
            widget.SecondaryGroupById = sourceWidget.SecondaryGroupById;
            widget.SecondaryDateGroupCode = sourceWidget.SecondaryDateGroupCode;
            dashboard.Widgets.push(widget);

        });

    }
    // static CopyWidgetMeasures(sourceWidget: WidgetPM, widget: WidgetPM) {
        // if(!sourceWidget.WidgetMeasures || sourceWidget.WidgetMeasures.length == 0) return;
        // widget.WidgetMeasures = [];
        // sourceWidget.WidgetMeasures.forEach(sourceMeasure => {
        //     var measure : WidgetMeasurePM = new WidgetMeasurePM(widget);
        //     measure = DashboardMapping.deepClone(sourceMeasure);
        //     measure.Tenant = SessionInfo.LoggedUserTenant;
        //     measure.WidgetId = widget.Id;
        //     measure.Id = null;
        //     widget.WidgetMeasures.push(measure)
        // });
    // }

    static CopyGlobalFiltyers( sourceDashboard: DashboardPM, dashboard: DashboardPM) {   
        // if(!sourceDashboard.DashboardGlobalFilters || sourceDashboard.DashboardGlobalFilters.length == 0) return;
        // dashboard.DashboardGlobalFilters = [];
        // sourceDashboard.DashboardGlobalFilters.forEach(sourceFilter => {
        //     var filter : DashboardGlobalFilterPM = new DashboardGlobalFilterPM(dashboard);
        //     filter = DashboardMapping.deepClone(sourceFilter);
        //     filter.Tenant = SessionInfo.LoggedUserTenant;
        //     filter.DashboardId = dashboard.Id;
        //     filter.Id = null;
        //     dashboard.DashboardGlobalFilters.push(filter)
        // });

        sourceDashboard.DashboardGlobalFilters.forEach(globalFilterItem => {
                var newGlobalFilterItem: DashboardGlobalFilterPM = new DashboardGlobalFilterPM(dashboard);
    
                newGlobalFilterItem.Tenant = SessionInfo.LoggedUserTenant;
                newGlobalFilterItem.DashboardId = dashboard.Id;
                newGlobalFilterItem.IsCommonFilter = globalFilterItem.IsCommonFilter;
                newGlobalFilterItem.CommonFilterField = globalFilterItem.CommonFilterField;
                newGlobalFilterItem.DataSetId = globalFilterItem.DataSetId;
                newGlobalFilterItem.DataSetFieldId = globalFilterItem.DataSetFieldId;
                newGlobalFilterItem.FilterOperator = globalFilterItem.FilterOperator;
                newGlobalFilterItem.DataTypeCode = globalFilterItem.DataTypeCode;
                newGlobalFilterItem.LineNumber = globalFilterItem.LineNumber;
                newGlobalFilterItem.JoinedTableName = globalFilterItem.JoinedTableName;
                newGlobalFilterItem.FieldCode = globalFilterItem.FieldCode;
                dashboard.DashboardGlobalFilters.push(newGlobalFilterItem);
            });
    }

    static CopyWidgetMeasures(sourecWidget: WidgetPM, widget: WidgetPM){
        sourecWidget.WidgetMeasures.forEach (itemMeasure => {
            var newWidgetMeasure: WidgetMeasurePM = new WidgetMeasurePM(widget);
            newWidgetMeasure.Tenant = SessionInfo.LoggedUserTenant;
            newWidgetMeasure.WidgetId = widget.Id;
            newWidgetMeasure.MeasureCode = itemMeasure.MeasureCode;
            newWidgetMeasure.MeasureFieldId = itemMeasure.MeasureFieldId;
            newWidgetMeasure.RenderAs = itemMeasure.RenderAs;
            widget.WidgetMeasures.push(newWidgetMeasure);
        });

    }

}