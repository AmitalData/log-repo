declare var System: any;
declare var window: any;
declare var attachmentUploader, ResultAsArray: any;
import {Component, OnInit, ElementRef, Output, EventEmitter}  from '@angular/core';


import {EntityArgs} from '../../Infrastructure/DataContracts/EntityArgs';
import {ReportPM} from '../../Common/EntityPMs/ReportPM';
import {ImageParameter} from '../../Infrastructure/DataContracts/ImageParameter';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ReportsTemplatePM} from '../../Common/EntityPMs/ReportsTemplatePM';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {ReportsTemplatePMExtendedService} from '../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';

import {ReportsTemplatePMService} from '../../Common/Services/StandardPMs/ReportsTemplatePMService';

@Component({

    moduleId: './Report/Components/',
    selector: 'ReportTemplate',
    templateUrl: 'ReportTemplateComponent.html',

})


export class ReportTemplateComponent implements OnInit {

    public EntityPM: ReportPM;
    reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService;
    reportsTemplatePMService: ReportsTemplatePMService;

    IsChange: boolean = false;

    IsEnableEditUserReportTemplate: boolean = false;
    IsEnableEditAllReportTemplate: boolean = false;


    private _entityResourceService: EntityResourceService = new EntityResourceService();
    ReportsTemplatePMLists: ReportsTemplatePM[] = [];
    CurrentReportsTemplatePM: ReportsTemplatePM;



    MessageReportsTemplatePMLists : ReportsTemplatePM[] = [];
    CurrentMessageReportsTemplatePM:ReportsTemplatePM;





    private CurrentSession = SessionLocator.SelectedSession;
    constructor( public entityArgs: EntityArgs,public _elementRef: ElementRef) {
        this.reportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();
        this.reportsTemplatePMService = new ReportsTemplatePMService();
        this.ReportsTemplatePMLists = [];
       
        
    }
    IsVisibile: boolean = false;
    IsShowNewButton: boolean = false;
    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;

        if (FeatureLocator.HasFeaturePermession("ReportsTemplate", "NEW")) this.IsShowNewButton = true;

        if (FeatureLocator.HasFeaturePermession("ReportsTemplate", "SYSTEMTEMPLATEEDIT")) {
            this.IsEnableEditAllReportTemplate = true;
        }
        if (FeatureLocator.HasFeaturePermession("ReportsTemplate", "UPDATE")) {
            this.IsEnableEditUserReportTemplate = true;

        }

  
        this._entityResourceService.getEntityResourceByTableName("ReportsTemplate", 0).subscribe(response => {
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) {
                this.LoadReportsTemplatePMLists();
            }
        });


    }

    
    public ShowMessage(message: string) {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }



    LoadReportsTemplatePMLists() {
        this.ReportsTemplatePMLists = [];
        this.MessageReportsTemplatePMLists = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.reportsTemplatePMExtendedService.GetReportsTemplatePMsByReportId(this.EntityPM.Id).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {

                    result.forEach((item) => {
                        if (item.TemplateType == "R") {
                            this.ReportsTemplatePMLists.push(item);
                        } else if (item.TemplateType == "M") {
                            this.MessageReportsTemplatePMLists.push(item);
                        }

                    });

                    if (this.ReportsTemplatePMLists.length > 0) {
                        var tempate = this.ReportsTemplatePMLists.filter(d => d.Id == this.EntityPM.DefaultTemplateId)[0];
                        if (tempate) {
                            tempate.IsDefault = true;
                            tempate.IsDirty = false;
                        }
                    }

                    if (this.MessageReportsTemplatePMLists.length > 0) {
                        var tempate = this.MessageReportsTemplatePMLists.filter(d => d.Id == this.EntityPM.DefaultMessageTemplateId)[0];
                        if (tempate) {
                            tempate.IsDefault = true;
                            tempate.IsDirty = false;
                        }
                    }

                }
            }
           
            this.CurrentSession.StopBusyIndicator();


        });


    }


    //Description
    DescriptionKeyUpMethod(item: ReportsTemplatePM) {
        if (item != null && item.IsDirty) {
        
            this.UpdateReportsTemplatePM(item);
        }
    }

    //Inactive
    CheckInActiveclick(item: ReportsTemplatePM) {

        if (item.InActive) item.InActive = false;
        else item.InActive = true;

  
        this.UpdateReportsTemplatePM(item);

    }


    UpdateReportsTemplatePM(item: ReportsTemplatePM) {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.IsChange = true;
        this.reportsTemplatePMService.update(item).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    item = result;
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    this.ShowMessage(pmResponse.ErrorsArray[0]);
                }
            }
        });
    }



    Refresh() {
        this.LoadReportsTemplatePMLists();
    }


    EditReportsTemplate(item: ReportsTemplatePM) {
        if (item) {
            if (this.IsEnableEditAllReportTemplate || (!item.IsSystem && this.IsEnableEditUserReportTemplate)) {
                var windowArgs: any = {};
                windowArgs.DataViewModel = this;
                windowArgs.ReportTemplateId = item.Id;
                windowArgs.Tenant = item.Tenant;

                var widthwindow = window.innerWidth;
                var heighthwindow = window.innerHeight;
                var logWindow = new LogitudeWindow();

                logWindow.Width = widthwindow - 100;
                logWindow.Height = heighthwindow - 100;
                logWindow.Title = "Edit Report Template";

                logWindow.IsShowCloseButton = true;
                logWindow.WindowArgs = windowArgs;
                window.designerClosed = false;

                logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");

                logWindow.WindowClosed.subscribe(($event: any) => {

                    if ($event) {
                        this.IsChange = true;
                        this.Refresh();

                    }

                });
            }


        }
    }

    EditMessageReportsTemplate(item: ReportsTemplatePM, isNew: boolean = false) {
        if (item) {
            if (this.IsEnableEditAllReportTemplate || (!item.IsSystem && this.IsEnableEditUserReportTemplate)) {
                var windowArgs: any = {};
                windowArgs.DataViewModel = this;
                windowArgs.PageType = "ReportTemplate";
                windowArgs.TemplateId = item.Id;
                windowArgs.Tenant = item.Tenant;
                windowArgs.ObjectType = "ReportsTemplatePM";
                windowArgs.IsNewEntity = isNew;
                windowArgs.ReportTemplatePM = item;
                windowArgs.ReportComponentArea = "Maintenance";
                var widthwindow = window.innerWidth;
                var heighthwindow = window.innerHeight;

                var logWindow = new LogitudeWindow();
                logWindow.Width = widthwindow - 100;
                logWindow.Height = heighthwindow - 100;
                logWindow.Title = "Edit Message Template";

                logWindow.WindowArgs = windowArgs;
                logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
            }
        }
    }

    RestoreVersionButtonClicked(item: ReportsTemplatePM) {
        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.ReportsTemplatePM = item;
        var logWindow = new LogitudeWindow();
        logWindow.Width =668;
        logWindow.Height = 500;
        logWindow.Title = "Version History";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Report/Components/ReportsTemplateRestoreComponent"); 
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event) {
            }
        });
    }

    SetAsDefaultButtonClicked(type:string) {

        var currentTemplate: ReportsTemplatePM = type == "R" ? this.CurrentReportsTemplatePM : this.CurrentMessageReportsTemplatePM;

        if (currentTemplate) {
            if (!currentTemplate.InActive) {

                var tempate = type == "R" ? this.ReportsTemplatePMLists.filter(d => d.Id == this.EntityPM.DefaultTemplateId)[0] : this.MessageReportsTemplatePMLists.filter(d => d.Id == this.EntityPM.DefaultMessageTemplateId)[0];
                if (tempate) {
                    tempate.IsDefault = false;
                }

                this.IsChange = true;
                currentTemplate.IsDefault = true;
                if (type == "R") this.EntityPM.DefaultTemplateId = currentTemplate.Id;
                else if (type == "M") this.EntityPM.DefaultMessageTemplateId = currentTemplate.Id;
            }
            else this.ShowMessage("Please note that you can't set an inactive template as default");
        
        }

    }

    
    AddReportTemplateButtonClicked(type:string) {

     
            var windowArgs: any = {};
            windowArgs.DataViewModel = this;
            windowArgs.TemplateType = type;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 700;
            logWindow.Height = 500;
            logWindow.Title = type == "R" ? "New Report Template":"New Message Template";
            logWindow.WindowArgs = windowArgs;
  
            logWindow.Show("./Report/Components/NewReportsTemplateComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event) {
                }
            });
     
    }


    CopyButtonClicked(item: ReportsTemplatePM) {
        this.CurrentSession.StartBusyIndicator("copy template...");
        this.IsChange = true;
        this.reportsTemplatePMExtendedService.GetCopyReportsTemplate(item.Id, SessionLocator.LoggedUserId).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    if (!this.ReportsTemplatePMLists) {
                        this.ReportsTemplatePMLists = [];
                    }
                    this.ReportsTemplatePMLists.push(result);
                    this.CurrentReportsTemplatePM = result;
                }
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }



}


