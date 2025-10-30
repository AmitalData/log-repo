import { EventEmitter, OnDestroy, Output } from '@angular/core';
import { Component } from '@angular/core';
import { ReportPM } from 'Common/EntityPMs/ReportPM';
import { ReportExecutionLogPMService } from 'Common/Services/StandardPMs/ReportExecutionLogPMService';
import { ReportPMService } from 'Common/Services/StandardPMs/ReportPMService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { BehaviorSubject, Subscription } from 'rxjs';
import { QueryFilterItem } from './Filters/QueryFilterItem';
import { ReportFliter } from './Filters/ReportFliter';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ReportsTemplateListExtendedService } from 'Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { ReportsPreviewComponent } from './ReportsPreviewComponent';
import { parseString } from 'xml2js';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ProcessMenuService } from 'Common/Services/ProcessMenuService';
import { ReportService } from 'Common/Services/ExtendedLists/ReportService';

@Component({
    selector: 'ProcessMenuComponent',
    templateUrl: './ProcessMenuComponent.html',
    inputs: [
        'CurrentProcessId',
        'IsProcessMenuVisible',
        'IsPinned',
        'CurrentSelectedTab',
    ],
})
export class ProcessMenuComponent implements OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    @Output() PinnedChanged = new EventEmitter<boolean>();
    @Output() NumberCompletedProcesses = new EventEmitter<number>();
    @Output() CloseMenu = new EventEmitter<MenuItemClass>();
    @Output() ViewedItem = new EventEmitter<string>();

    SelectedMenuItem: MenuItemClass;
    private menuItemsSubject = new BehaviorSubject<MenuItemClass[]>([]);
    MenuItems$ = this.menuItemsSubject.asObservable();
    showExceptionMessage = false;
    isProcessMenuVisible: boolean = false;
    public selectedTab: string = '0';
    public currentSelectedTab: string = '0';

    currentProcessId: string = '';
    public isPinned: boolean = false;
    public LayoutDirection: string = 'ltr';
    ReportExecutionLogPMService: ReportExecutionLogPMService =
        new ReportExecutionLogPMService();
    private reportService: ReportService = new ReportService();
    private subscription: Subscription | null = null;
    reportPMService: ReportPMService = new ReportPMService();
    reportsTemplateListExtendedService =
        new ReportsTemplateListExtendedService();
    ReportTemplates = null;

    public groupedMenuItems: { [key: string]: any[] } = {};
    public MenuTypeNames: { [key in MenuTypes]?: string } = {
        [MenuTypes.ReportExecutionLog]:
            TextCodeTranslator.Translate('General.MH.Reports'),
        [MenuTypes.BatchTaskExecution]: TextCodeTranslator.Translate(
            'Accounting.General.O.TaxReport'
        ),
    };
    private MenuGroupingMap: { [key in MenuTypes]: MenuTypes } = {
        [MenuTypes.ReportExecutionLog]: MenuTypes.ReportExecutionLog,
        [MenuTypes.BatchTaskExecution]: MenuTypes.BatchTaskExecution,
        [MenuTypes.ExcelExport]: MenuTypes.ReportExecutionLog,
    };
    constructor(private processMenuService: ProcessMenuService) {
        this.LayoutDirection =
            ObjectsLocator.GlobalSetting == undefined
                ? 'ltr'
                : ObjectsLocator.GlobalSetting.LayoutDirection;

        this.GroupMenuItemsByType();
    }
    GroupMenuItemsByType() {
        this.processMenuService.relatedProcessSubject.subscribe(
            (menuItemList) => {
                this.groupedMenuItems = menuItemList.reduce(
                    (
                        groups: { [key: string]: MenuItemClass[] },
                        menuItem: MenuItemClass
                    ) => {
                        const groupType =
                            this.MenuGroupingMap[menuItem.ItemType] ??
                            menuItem.ItemType; // ⬅️ שינוי כאן
                        if (!groups[groupType]) {
                            groups[groupType] = [];
                        }
                        groups[groupType].push(menuItem);
                        return groups;
                    },
                    {}
                );
            }
        );
    }
    selectTab(tab: string) {
        this.currentSelectedTab = tab;
    }
    ngOnDestroy(): void {
        this.subscription?.unsubscribe();
    }

    get CurrentProcessId() {
        return this.currentProcessId;
    }
    set CurrentProcessId(newValue: string) {
        if (this.currentProcessId != newValue) {
            this.currentProcessId = newValue;
        }
    }
    get CurrentSelectedTab() {
        return this.currentSelectedTab;
    }
    set CurrentSelectedTab(newValue: string) {
        if (this.currentSelectedTab != newValue) {
            this.currentSelectedTab = newValue;
        }
    }
    get IsPinned() {
        return this.isPinned;
    }
    set IsPinned(newValue: boolean) {
        if (this.isPinned != newValue) {
            this.isPinned = newValue;
        }
    }
    get IsProcessMenuVisible() {
        return this.isProcessMenuVisible;
    }
    set IsProcessMenuVisible(newValue: boolean) {
        if (this.isProcessMenuVisible != newValue) {
            this.isProcessMenuVisible = newValue;
        }
    }

    togglePin() {
        this.isPinned = !this.isPinned;
        this.PinnedChanged.emit(this.isPinned);
    }

    DeleteMenuItem(relatedRep: MenuItemClass) {
        this.processMenuService
            .DeleteFromMenu(relatedRep.Id, relatedRep.ItemType)
            .subscribe((res: any) => {
                this.ViewedItem.emit(relatedRep.Id);
            });
    }

    CancelMenuItem(relatedRep: MenuItemClass) {
        this.ReportExecutionLogPMService.Cancel(relatedRep.Id).subscribe(
            (res: any) => {
                if (!res.HasError) {
                    this.processMenuService.LoadMenuItems();
                }
                this.CurrentSession.StopBusyIndicator();

                this.ViewedItem.emit(relatedRep.Id);
            }
        );
    }

    ViewMenuItem(relatedRep: MenuItemClass) {
        switch (relatedRep.ItemType) {
            case MenuTypes.ReportExecutionLog:
                this.ViewReportExecutionLog(relatedRep);
                break;
            case MenuTypes.BatchTaskExecution:
                this.ViewBatchTaskExecution(relatedRep);
                break;
            case MenuTypes.ExcelExport:
                this.DownloadExcelReport(relatedRep);
                break;
            default:
                this.ShowNoSupportWindow();
                break;
        }

        // unpin and close
        this.PinnedChanged.emit(false);
        this.CloseMenu.emit();
        this.ViewedItem.emit(relatedRep.Id);
    }
    ShowNoSupportWindow() {
        var msg = new MessageWindow();
        msg.Width = 450;
        msg.Show(' This item is not supported yet');
    }
    ViewReportExecutionLog(item: MenuItemClass) {
        this.reportPMService
            .get(item.ItemId)
            .subscribe((response: ServiceResponse) => {
                if (response.Result) {
                    if (!response.HasError) {
                        this.reportsTemplateListExtendedService
                            .getReportsTemplateListsByReportId(item.ItemId)
                            .subscribe((myResponse: ServiceResponse) => {
                                if (myResponse.HasError) return;
                                this.CloseMenu.emit();

                                this.ReportTemplates = myResponse.Result;
                                this.LoadReportsPreviewComponent(
                                    item,
                                    response.Result
                                );

                                this.CurrentSession.StopBusyIndicator();
                            });
                    }
                }
            });
    }
    async DownloadExcelReport(item: MenuItemClass) {
        const filterXML = item.FilterXML ?? '';
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(filterXML, 'text/xml');

        const reportNameElement = xmlDoc.querySelector('ReportName');
        let reportName = '';

        if (reportNameElement && reportNameElement.textContent) {
            reportName = reportNameElement.textContent;
        }

        const res: Blob = await this.reportService.GetExcel(
            item.Id,
            reportName
        );
        const blobUrl: string = window.URL.createObjectURL(res);
        const link = document.createElement('a');
        link.href = blobUrl;
        link.download = item.Name + '.xlsx';
        link.click();
        link.remove();
    }

    ViewBatchTaskExecution(relatedRep: MenuItemClass) {
        SessionLocator.DynamicLoader.Load(
            './Infrastructure/Components/EditComponent/EditComponent',
            this.CurrentSession.SessionLocation.viewContainerRef
        ).then((cmpRef) => {
            cmpRef.instance.ComponentRef = cmpRef;
            const parser = new DOMParser();
            const xmlDoc = parser.parseFromString(
                relatedRep.FilterXML,
                'text/xml'
            );
            const reportId =
                xmlDoc.getElementsByTagName('ReportId')[0]?.textContent;

            cmpRef.instance.Run({
                EntityId: reportId,
                ObjectTableName: 'TaxReport',
            });
            this.CloseMenu.emit();
        });
    }
    private PageChild_PRREP: any = null;
    ReportsPreviewComponent: ReportsPreviewComponent;

    LoadReportsPreviewComponent(relatedRep: MenuItemClass, report: ReportPM) {
        this.CurrentSession.StartBusyIndicator('Preview...');
        SessionLocator.DynamicLoader.Load(
            './Report/Components/ReportsPreviewComponent',
            SessionLocator.SelectedSession.SessionLocation.viewContainerRef
        ).then((cmpRef: any) => {
            cmpRef.instance.ComponentRef = cmpRef;
            this.PageChild_PRREP = cmpRef.instance;
            this.SetReportDetails(relatedRep, report);
            this.CurrentSession.StopBusyIndicator();
        });
    }

    SetReportDetails(relatedRep: MenuItemClass, report: ReportPM) {
        let reportFilterItems = this.ConvertXmlToObject(relatedRep.FilterXML);
        this.PageChild_PRREP.SetReportFilterItems(
            reportFilterItems?.QueryFilterItemLists,
            false
        );

        this.PageChild_PRREP.SetReportTemplate(relatedRep.TemplateId, true);
        this.PageChild_PRREP.ReportsPreview(null, report, this.ReportTemplates);
        this.PageChild_PRREP.GenerateReportViewWorkerRole(reportFilterItems);
    }

    SetReportFilterItems(xml: any): Array<QueryFilterItem> {
        let reportFilterItems: Array<QueryFilterItem> = [];

        let filterItems = xml;
        if (!Array.isArray(filterItems)) {
            filterItems = [filterItems];
        }

        reportFilterItems = filterItems.map((item: any) => ({
            FieldName: item.FieldName,
            FieldValue: this.convertStringToType(item.FieldValue?._),
            FieldValue2: this.convertStringToType(item.FieldValue2?._),
            FieldValue3: this.convertStringToType(item.FieldValue3?._),
            Operator: item.Operator,
            IsCustom: item.IsCustom === 'true',
            DisplayInList: item.DisplayInList === 'true',
            IsCustomField: item.IsCustomField === 'true',
            FieldDataType: item.FieldDataType,
            IsListFilter: item.IsListFilter === 'true',
            IsAnalyticsMetadatas: item.IsAnalyticsMetadatas === 'true',
        }));

        return reportFilterItems;
    }
    convertStringToType(value: string): any {
        if (value === 'true') {
            return true;
        } else if (value === 'false') {
            return false;
        } else {
            return value;
        }
    }
    ConvertXmlToObject(xml: string): ReportFliter {
        let resultObject: any = {};
        parseString(
            xml,
            { explicitArray: false },
            (err: Error, result: any) => {
                if (err) {
                    console.error('Error parsing XML:', err);
                    return;
                }

                const reportFilter = result.ReportFliter;
                resultObject.CurrentCurrencyCodeType =
                    reportFilter.CurrentCurrencyCodeType;
                resultObject.DateType = reportFilter.DateType;
                resultObject.IncludeOperationalyClosed =
                    reportFilter.IncludeOperationalyClosed === 'true';
                resultObject.ReportCode = reportFilter.ReportCode;
                resultObject.FilterControlName = reportFilter.FilterControlName;
                resultObject.ReportDocumentId = reportFilter.ReportDocumentId;
                resultObject.CustomerId = reportFilter.CustomerId;
                resultObject.QuoteCustomerTypeCode =
                    reportFilter.QuoteCustomerTypeCode;
                resultObject.FieldDataType = reportFilter.FieldDataType;
                resultObject.Tenant = parseInt(reportFilter.Tenant, 10);
                resultObject.QueryFilterItemLists = this.SetReportFilterItems(
                    reportFilter.QueryFilterItemLists.QueryFilterItem
                );
                resultObject.NumberOfPage = parseInt(
                    reportFilter.NumberOfPage,
                    10
                );
                resultObject.ProcessType = 'ReportsRunUsingWR';
                resultObject.ReportKey = reportFilter.ReportKey;
                resultObject.ReportName = reportFilter.ReportName;
                resultObject.InvoiceType = reportFilter.InvoiceType;
                resultObject.DefaultTemplateId = reportFilter.DefaultTemplateId;
                resultObject.DefaultTemplateVsersion = parseInt(
                    reportFilter.DefaultTemplateVsersion,
                    10
                );
                resultObject.ReportsRunUsingWR =
                    reportFilter.ReportsRunUsingWR === 'true';
                resultObject.UserId = reportFilter.UserId;
                resultObject.ReportId = reportFilter.ReportId;
                resultObject.NumberOfRequests = parseInt(
                    reportFilter.NumberOfRequests,
                    10
                );
                resultObject.Level = reportFilter.Level;
                resultObject.DisablePreview =
                    reportFilter.DisablePreview === 'true';
            }
        );
        return resultObject;
    }
}

export class MenuItemClass {
    public StatusCode: string;
    public LocalName: string;
    public Name: string;
    public CreateDate: Date = new Date();
    public ExceptionMessage: string = '';
    public ItemId: string;
    public FilterXML: string;
    public TemplateId: string;
    public NotDisplayInMenu: boolean = false;
    public ItemType: number = 0;
    public Id: string;
}

export enum MenuTypes {
    ReportExecutionLog = 0,
    BatchTaskExecution = 1,
    ExcelExport = 2,
}
