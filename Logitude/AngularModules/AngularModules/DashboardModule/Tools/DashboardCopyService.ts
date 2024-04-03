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
        dashboard.PinnedByDefault = sourceDashboard.PinnedByDefault;
        dashboard.PredefinedOrder = sourceDashboard.PredefinedOrder;

        return dashboard;

    }


    static CopyWidgets(sourceDashboard: DashboardPM, dashboard: DashboardPM) {
        sourceDashboard.Widgets.forEach(sourceWidget => {
            var widget: WidgetPM = new WidgetPM(dashboard);

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
            widget.Alignment = sourceWidget.Alignment;
            widget.ThousandSeparator = sourceWidget.ThousandSeparator;
            widget.UseNumberAbbreviation = sourceWidget.UseNumberAbbreviation;
            widget.UseAbbreviationAfter = sourceWidget.UseAbbreviationAfter;
            widget.DecimalPlaces = sourceWidget.DecimalPlaces;
            widget.LabelsPosition = sourceWidget.LabelsPosition;
            dashboard.Widgets.push(widget);

        });

    }

    static CopyWidgetMeasures(sourceWidget: WidgetPM, widget: WidgetPM) {
        sourceWidget.WidgetMeasures.forEach(itemMeasure => {
            var newWidgetMeasure: WidgetMeasurePM = new WidgetMeasurePM(widget);
            newWidgetMeasure.Tenant = SessionInfo.LoggedUserTenant;
            newWidgetMeasure.WidgetId = widget.Id;
            newWidgetMeasure.MeasureCode = itemMeasure.MeasureCode;
            newWidgetMeasure.MeasureFieldId = itemMeasure.MeasureFieldId;
            newWidgetMeasure.RenderAs = itemMeasure.RenderAs;
            newWidgetMeasure.YAxisType = itemMeasure.YAxisType;
            widget.WidgetMeasures.push(newWidgetMeasure);
        });

    }
    static CopyUsers(sourceDashboard: DashboardPM, dashboard: DashboardPM) {
        sourceDashboard.DashboardSharedUsers.forEach(sourceUser => {
            var user: DashboardSharedUserPM = new DashboardSharedUserPM(dashboard);
            user.Tenant = SessionInfo.LoggedUserTenant;
            user.DashboardId = dashboard.Id;
            user.UserId = sourceUser.UserId;
            user.UserName = sourceUser.UserName;
            dashboard.DashboardSharedUsers.push(user);
        });
    }

    static CopyGlobalFiltyers(sourceDashboard: DashboardPM, dashboard: DashboardPM) {
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


}