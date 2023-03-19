import { ReportsTemplateList } from "../../../../Common/EntityLists/ReportsTemplateList";
import { ReportsTemplatePM } from "../../../../Common/EntityPMs/ReportsTemplatePM";
import { LogitudeWindow } from "../../../../Controls/Windows/LogitudeWindow";
import { StimulsoftViewerComponent } from "../StimulsoftViewerComponent";

export class StimulsoftViewerService {
    private objectTableId: string;
    private entityId: string;
    private dataViewModel: StimulsoftViewerComponent;
    public StimulsoftViewerService() {

    }

    SetArgs(args: any) {
        this.objectTableId = args.ObjectTableId;
        this.entityId = args.EntityId;
        this.dataViewModel = args.DataViewModel;
    }

    EditMessageTemplate(selectedMessageTemplateList) {

        if (!selectedMessageTemplateList) return;
        let selectedMessageTemplatePM = this.MapMessageTemplateListToPM(selectedMessageTemplateList);
        var windowArgs: any = this.GetEditMessageTemplateWindowArgs(selectedMessageTemplatePM);
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;

        var logWindow = new LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = "Edit Message Template";

        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
    }


    AddMessageTemplate() {
        var windowArgs: any = this.GetAddMessageTemplateWindowArgs();
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.Title = "New Message Template";
        logWindow.WindowArgs = windowArgs;

        logWindow.Show("./Report/Components/NewReportsTemplateComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {
            }
        });
    }

    GetEditMessageTemplateWindowArgs(selectedMessageTemplatePM: ReportsTemplatePM) {
        var windowArgs: any = {};
        windowArgs.DataViewModel = this.dataViewModel;
        windowArgs.PageType = "ReportTemplate";
        windowArgs.TemplateId = selectedMessageTemplatePM.Id;
        windowArgs.Tenant = selectedMessageTemplatePM.Tenant;
        windowArgs.ObjectType = "ReportsTemplatePM";
        windowArgs.IsNewEntity = false;
        windowArgs.ReportTemplatePM = selectedMessageTemplatePM;
        windowArgs.ReportComponentArea = "Scheduler";
        windowArgs.RequsetPageName = "Scheduler";
        windowArgs.DontShowToField = true;
        windowArgs.DontShowBCCField = true;
        windowArgs.IsFromScheduler = true;
        windowArgs.EntityId = this.entityId ? this.entityId : null;
        windowArgs.ObjectTableId = this.objectTableId ? this.objectTableId : null;
        return windowArgs;
    }


    GetAddMessageTemplateWindowArgs() {
        let windowArgs: any = {};
        windowArgs.DataViewModel = this.dataViewModel;
        windowArgs.ReportEntityId = this.entityId ? this.entityId : null;
        windowArgs.ObjectTableId = this.objectTableId ? this.objectTableId : null;
        windowArgs.PageType = "ReportTemplate";
        windowArgs.TypeTab = "RichText";
        windowArgs.TemplateType = "M";
        windowArgs.RequsetPageName = "Report";
        windowArgs.IsFromScheduler = true;
        return windowArgs;
    }

    MapMessageTemplateListToPM(reportTemplateList: any) {
        let reportTemplatePM = new ReportsTemplatePM();

        reportTemplatePM.Id = reportTemplateList.Id;
        reportTemplatePM.Tenant = reportTemplateList.Tenant;
        reportTemplatePM.Description = reportTemplateList.Description;
        reportTemplatePM.CurrentVersion = reportTemplateList.CurrentVersion;
        reportTemplatePM.ReportId = reportTemplateList.ReportId;
        reportTemplatePM.TemplateType = reportTemplateList.TemplateType;
        reportTemplatePM.ObjectTableId = reportTemplateList.ObjectTableId;
        reportTemplatePM.EntityId = reportTemplateList.EntityId;
        reportTemplatePM.CreateDate = reportTemplateList.CreateDate;
        reportTemplatePM.UpdateDate = reportTemplateList.UpdateDate;
        reportTemplatePM.UpdatedByUserId = reportTemplateList.UpdatedByUserId;
        reportTemplatePM.CreatedByUserId = reportTemplateList.CreatedByUserId;
        reportTemplatePM.InActive = reportTemplateList.InActive;
        reportTemplatePM.IsSystem = reportTemplateList.IsSystem;
        reportTemplatePM.CC = reportTemplateList.CC;
        reportTemplatePM.From = reportTemplateList.From;
        reportTemplatePM.Subject = reportTemplateList.Subject;
        reportTemplatePM.ReplyTo = reportTemplateList.ReplyTo;

        return reportTemplatePM;
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
}
