declare var System: any;
declare var window: any;

import {Component, OnInit, Output}  from '@angular/core';

import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ReportsTemplatePM} from '../../Common/EntityPMs/ReportsTemplatePM';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {ReportsTemplatePMService} from '../../Common/Services/StandardPMs/ReportsTemplatePMService';
import {ReportsTemplatesVersionListExtendedService} from '../../Common/Services/ExtendedLists/ReportsTemplatesVersionListExtendedService';
import {ReportsTemplatesVersionList} from '../../Common/EntityLists/ReportsTemplatesVersionList';
import {ReportsTemplatesVersionPMExtendedService} from '../../Common/Services/ExtendedPMs/ReportsTemplatesVersionPMExtendedService';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import { DownloadManager } from 'Infrastructure/Utilities/DownloadManager';
@Component({

    moduleId: './Report/Components/',
    selector: 'ReportsTemplateRestoreComponent',
    templateUrl: 'ReportsTemplateRestoreComponent.html',

})


export class ReportsTemplateRestoreComponent implements OnInit {
    CurrentReportsTemplatesVersionList: ReportsTemplateRestoreItem;
    reportsTemplatePMService: ReportsTemplatePMService;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private reportsTemplatesVersionListExtendedService: ReportsTemplatesVersionListExtendedService;
    private reportsTemplatesVersionPMExtendedService: ReportsTemplatesVersionPMExtendedService;
    
    ReportsTemplatesVersionLists: ReportsTemplateRestoreItem[] = [];
    ReportsTemplatePM: ReportsTemplatePM;
    RestoreButtonLable: string = "Restore";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.reportsTemplatePMService = new ReportsTemplatePMService();
        this.reportsTemplatesVersionListExtendedService = new ReportsTemplatesVersionListExtendedService();
        this.reportsTemplatesVersionPMExtendedService = new ReportsTemplatesVersionPMExtendedService();

        
    }
    IsVisibile: boolean = false;
    ngOnInit() {

    }

    DataViewModel: any;
    SetWindowArgs(args: any) {

        this._entityResourceService.getEntityResourceByTableName("ReportsTemplatesVersion", 0).subscribe((response:any) => {
            this.IsVisibile = true;
            if (args) {
                this.DataViewModel = args.DataViewModel;
                this.ReportsTemplatePM = args.ReportsTemplatePM;
                if (this.ReportsTemplatePM) {
                    this.LoadReportsTemplatesVersionLists();
              
                }

            }
        });


    }



    RestoreButtonClicked(item: ReportsTemplateRestoreItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Please notice that this will restore this version and set as the current one");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.Restore(item);
            }
        });

    }


    Restore(item: ReportsTemplateRestoreItem) {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.reportsTemplatesVersionPMExtendedService.GetRestoreReportsTemplatesVersion(item.Id, SessionLocator.LoggedUserId).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    if (this.DataViewModel) {
                        this.DataViewModel.IsChange = true;
                        this.DataViewModel.Refresh();
                    }
                    this.CurrentSession.CloseCurrentWindow();
                }
            }

        });
    }


    PreviewButtonClicked(item: ReportsTemplateRestoreItem) {
        if (item) {

            var windowArgs: any = {};
            windowArgs.DataViewModel = this;
            windowArgs.ProcessType = "ReportPreview";
            windowArgs.ReportTemplateId = item.ReportDocumentId;
            windowArgs.ReportsTemplateId = this.ReportsTemplatePM.Id;
            windowArgs.Tenant = item.Tenant;
            windowArgs.TemplateType = this.ReportsTemplatePM.TemplateType;

            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var logWindow = new LogitudeWindow();

            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = "Preview Report Template Version";

            logWindow.IsShowCloseButton = true;
            logWindow.WindowArgs = windowArgs;
            window.designerClosed = false;

            logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");

            logWindow.WindowClosed.subscribe(($event: any) => {

            });

        }
    }
  

    LoadReportsTemplatesVersionLists() {
        this.ReportsTemplatesVersionLists = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.reportsTemplatesVersionListExtendedService.getReportsTemplatesVersionListsByReportTemplateId(this.ReportsTemplatePM.Id).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {

                    result.sort((a, b) => { return (a.Version === b.Version) ? 0 : (a.Version < b.Version) ? 1 : -1 }).forEach((item) => {
                        this.ReportsTemplatesVersionLists.push(new ReportsTemplateRestoreItem(item));
                    });

                    if (this.ReportsTemplatesVersionLists[0]) {
                        this.ReportsTemplatesVersionLists[0].IsCurrentVersion = true;
                        this.ReportsTemplatesVersionLists[0].VisibleRestoredViewButton = true;
                    }

                }
            }

            this.CurrentSession.StopBusyIndicator();


        });


    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    //MouseEvent
    OnmMouseOver(reportsTemplatesVersionItem: ReportsTemplateRestoreItem) {

        this.ReportsTemplatesVersionLists.forEach((item) => {

            if (!item.IsCurrentVersion) {
                item.VisibleRestoredViewButton = false;
            }

        });
       
        reportsTemplatesVersionItem.VisibleRestoredViewButton = true;

    }
    OnmMouseleave(item: ReportsTemplateRestoreItem) {

        this.ReportsTemplatesVersionLists.forEach((item) => {
            item.VisibleRestoredViewButton = false;

            if (item.IsCurrentVersion) {
                item.VisibleRestoredViewButton = true;
            }
        });



    }

    DownloadButtonClicked(item: ReportsTemplateRestoreItem) {
        DownloadManager.DownloadPage(item.ReportDocumentId);

    }
}



export class ReportsTemplateRestoreItem  {
    Id: string;
    Version: number;
    IsRestored: boolean;
    UpdateDate: Date;
    UpdateByUserName: string;
    VisibleRestoredViewButton: boolean = false;
    ReportDocumentId: string;
    Tenant: number;
    EntityList: ReportsTemplatesVersionList;

    IsCurrentVersion: boolean = false;
    constructor(reportsTemplatesVersionList: ReportsTemplatesVersionList) {

        this.EntityList = reportsTemplatesVersionList;

        this.Id = reportsTemplatesVersionList.Id;
        this.Version = reportsTemplatesVersionList.Version;
        this.IsRestored = reportsTemplatesVersionList.IsRestored;
        this.UpdateDate = reportsTemplatesVersionList.UpdateDate;
        this.UpdateByUserName = reportsTemplatesVersionList.UpdateByUserName;
        this.ReportDocumentId = reportsTemplatesVersionList.ReportDocumentId;
        this.Tenant = reportsTemplatesVersionList.Tenant;
 
    }

}
