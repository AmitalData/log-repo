import { ReportsTemplateList } from "../../../../Common/EntityLists/ReportsTemplateList";
import { ReportsTemplatePM } from "../../../../Common/EntityPMs/ReportsTemplatePM";
import { LogitudeWindow } from "../../../../Controls/Windows/LogitudeWindow";
import { StimulsoftViewerComponent } from "../StimulsoftViewerComponent";

export class SchedulerReportMessageTemplateService {
    private objectTableId: string;
    private entityId: string;
    private parentEntityId: string;
    private dataViewModel: StimulsoftViewerComponent;

    public SchedulerReportMessageTemplateService() {

    }

    SetArgs(args: any) {
        this.objectTableId = args.ObjectTableId;
        this.entityId = args.EntityId;
        this.dataViewModel = args.DataViewModel;
        this.parentEntityId = args.ParentEntityId;
    }

    Edit(selectedMessageTemplateList) {

        if (!selectedMessageTemplateList) return;
        var windowArgs: any = this.GetEditWindowArgs(selectedMessageTemplateList);
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;

        var logWindow = new LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = "Edit Message Template";

        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
        logWindow.WindowClosed.subscribe(($templateId: any) => {
            if ($templateId) {
                this.CheckAndAddToSchedulerTemplatesList($templateId);
            }
        });
    }

    CheckAndAddToSchedulerTemplatesList(templateId: string) {
        if (this.dataViewModel.StimulsoftArgData.ReportsPreviewComponent.MessageTemplateIds.indexOf(templateId) > -1) return;
        this.dataViewModel.StimulsoftArgData.ReportsPreviewComponent.MessageTemplateIds.push(templateId);
    }

    Add() {
        var windowArgs: any = this.GetAddWindowArgs();
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.Title = "New Message Template";
        logWindow.WindowArgs = windowArgs;

        logWindow.Show("./Report/Components/NewReportsTemplateComponent");
        logWindow.WindowClosed.subscribe(($templateId: any) => {
            if ($templateId) {
                if (!this.dataViewModel.StimulsoftArgData.ReportsPreviewComponent.MessageTemplateIds) {
                    this.dataViewModel.StimulsoftArgData.ReportsPreviewComponent.MessageTemplateIds = [];
                }
                this.dataViewModel.StimulsoftArgData.ReportsPreviewComponent.MessageTemplateIds.push($templateId);
            }
        });
    }

    GetEditWindowArgs(selectedMessageTemplateList: ReportsTemplateList) {
        var windowArgs: any = {};
        windowArgs.DataViewModel = this.dataViewModel;
        windowArgs.PageType = "ReportTemplate";
        windowArgs.TemplateId = selectedMessageTemplateList.Id;
        windowArgs.Tenant = selectedMessageTemplateList.Tenant;
        windowArgs.ObjectType = "ReportsTemplatePM";
        windowArgs.IsNewEntity = false;
        windowArgs.ReportTemplatePM = this.MapMessageTemplateListToPM(selectedMessageTemplateList);
        windowArgs.ReportComponentArea = "Scheduler";
        windowArgs.RequsetPageName = "Scheduler";
        windowArgs.DontShowToField = true;
        windowArgs.DontShowBCCField = true;
        windowArgs.IsFromScheduler = true;
        windowArgs.EntityId = this.entityId ? this.entityId : this.parentEntityId;
        windowArgs.ObjectTableId = this.objectTableId ? this.objectTableId : null;
        return windowArgs;
    }


    GetAddWindowArgs() {
        let windowArgs: any = {};
        windowArgs.DataViewModel = this.dataViewModel;
        windowArgs.ReportEntityId = this.entityId ? this.entityId : this.parentEntityId;
        windowArgs.ObjectTableId = this.objectTableId ? this.objectTableId : null;
        windowArgs.PageType = "ReportTemplate";
        windowArgs.TypeTab = "RichText";
        windowArgs.TemplateType = "M";
        windowArgs.RequsetPageName = "Report";
        windowArgs.IsFromScheduler = true;
        return windowArgs;
    }

    MapMessageTemplatePMToList(reportTemplatePM: any) {
        let reportTemplateList = new ReportsTemplateList();

        reportTemplateList.Id = reportTemplatePM.Id;
        reportTemplateList.Tenant = reportTemplatePM.Tenant;
        reportTemplateList.Description = reportTemplatePM.Description;
        reportTemplateList.CurrentVersion = reportTemplatePM.CurrentVersion;
        reportTemplateList.ReportId = reportTemplatePM.ReportId;
        reportTemplateList.TemplateType = reportTemplatePM.TemplateType;
        reportTemplateList.ObjectTableId = reportTemplatePM.ObjectTableId;
        reportTemplateList.EntityId = reportTemplatePM.EntityId;
        reportTemplateList.CreateDate = reportTemplatePM.CreateDate;
        reportTemplateList.UpdateDate = reportTemplatePM.UpdateDate;
        reportTemplateList.UpdatedByUserId = reportTemplatePM.UpdatedByUserId;
        reportTemplateList.CreatedByUserId = reportTemplatePM.CreatedByUserId;
        reportTemplateList.InActive = reportTemplatePM.InActive;
        reportTemplateList.IsSystem = reportTemplatePM.IsSystem;
        reportTemplateList.CC = reportTemplatePM.CC;
        reportTemplateList.From = reportTemplatePM.From;
        reportTemplateList.Subject = reportTemplatePM.Subject;
        reportTemplateList.ReplyTo = reportTemplatePM.ReplyTo;

        return reportTemplateList;
    }

    MapMessageTemplateListToPM(reportTemplateList: any) {
        let reportTemplatePM = new ReportsTemplatePM();
        reportTemplatePM.Id = reportTemplateList.Id;
        reportTemplatePM.Tenant = reportTemplateList.Tenant;
        reportTemplatePM.Description = reportTemplateList.Description;
        reportTemplatePM.ReportId = reportTemplateList.ReportId;
        return reportTemplatePM;
    }
}
