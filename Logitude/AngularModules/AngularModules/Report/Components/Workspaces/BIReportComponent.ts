import { Component, OnInit, ElementRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BIReportList } from '../../../Infrastructure/EntityLists/BIReportList';
import { BIReportListService } from '../../../Infrastructure/Services/StandardLists/BIReportListService';
import { InfrastructureDomainService } from '../../../Infrastructure/Services/InfrastructureDomainService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: './Report/Components/Workspaces/',
    templateUrl: 'BIReportComponent.html',
})

export class BIReportComponent {
  
    public ItemsSource: BIReportList[] = [];
    private BIReportListService: BIReportListService;
    private InfrastructureDomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        this.InfrastructureDomainService = new InfrastructureDomainService();
        this.LoadData();
        this.Listen();
    }

    private Listen() {
        this.CurrentSession.SessionEvent.subscribe(s => {
            if (s == "BIRefresh") {
                this.LoadData();
            }
        });
    }

    InitComponent() {

    }
    LoadData() {
        this.ItemsSource = [];
        this.BIReportListService = new BIReportListService();
        this.BIReportListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var myResult: BIReportList[] = myResponse.Result;
                if (AppTool.IsNullOrEmpty(this.mySearchText)) {
                    this.ItemsSource = myResult;
                }
                else {
                    myResult.forEach((item) => {
                        if (!AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(this.mySearchText.toUpperCase()) > -1
                            ||
                            !AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(this.mySearchText.toUpperCase()) > -1) {
                            this.ItemsSource.push(item);
                        }
                    });
                }
            }
        });
    }
    public NewBIReportButtonClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1200;
        logWindow.Height = 820;
        var windowArgs: any = {};
        windowArgs.IsBIReportWorkspace = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (s != null && d != "cancel") {
                    SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ DWQueryId: s.QID, ObjectTableName: 'BIReport', EntityId: null });
                        });
                }
            });
        });
    }
    OnNewBIReportWindowClosed(arg: any) {
        if (arg != 'cancel') {
            this.LoadData();
        }
    }

    EditBIReportClicked(report: BIReportList) {
        this.entityResourceService.getEntityResourceByTableName("BIReport", 0).subscribe(response => {
            if (!AppTool.IsNullOrEmpty(report.Id)) {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: report.Id, ObjectTableName: 'BIReport'});
                        cmpRef.instance.BackCompleted.subscribe(bk => {
                        });
                    });
            }
        });
    }

    ViewBIReportClicked(report: BIReportList) {
        SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ DWQueryId: report.DWQueryId, ObjectTableName: 'BIReport', EntityList: report, EntityId: report.Id });
            });
    }

    ExportToExcelClicked(report: BIReportList) {
        var windowArgs: any = {};
        windowArgs.queryId = report.DWQueryId;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/ExportBI2ExcelControl/ExportBI2ExcelControl');
    }

    public mySearchText: string = null;
    SearchTextChanged(text: string) {
        this.mySearchText = text;
        this.LoadData();
    }
}
