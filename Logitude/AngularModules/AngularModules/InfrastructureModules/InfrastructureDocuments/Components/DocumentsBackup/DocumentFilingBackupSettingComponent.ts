import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentFilingBackupSettingPM} from '../../../../Common/EntityPMs/DocumentFilingBackupSettingPM';
import {FTPDetailPMService} from '../../../../Common/Services/StandardPMs/FTPDetailPMService';
import {DocumentFilingBackupSettingPMService} from '../../../../Common/Services/StandardPMs/DocumentFilingBackupSettingPMService'
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    selector: 'DocumentFilingBackupSettingComponent',
    templateUrl: './DocumentFilingBackupSettingComponent.html',
})

export class DocumentFilingBackupSettingComponent extends BaseComponent implements OnInit {
    IsLoad: boolean = false;
    documentFilingBackupSettingPMService: DocumentFilingBackupSettingPMService;
    DataContext: any = this;
    IsNewDocumentFilingBackupSetting: boolean = false;
    documentFilingBackupSettingPM: DocumentFilingBackupSettingPM;
    private myFTPService: FTPDetailPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.documentFilingBackupSettingPMService = new DocumentFilingBackupSettingPMService();
        this.myFTPService = new FTPDetailPMService();
    }

    ngOnInit() {

        this.LoadData();
    }
    FTPDetailHost: string;
    FTPDetailId: string;
    LoadData() {
        this.documentFilingBackupSettingPMService.get(SessionLocator.Tenant).subscribe(res => {
            if (!res.HasError) {
                this.documentFilingBackupSettingPM = res.Result;
                if (!this.documentFilingBackupSettingPM) {
                    this.documentFilingBackupSettingPM = new DocumentFilingBackupSettingPM();
                    this.documentFilingBackupSettingPM.Tenant = SessionLocator.Tenant;
                    this.documentFilingBackupSettingPM.IsActive = false;
                    this.IsNewDocumentFilingBackupSetting = true;
                }
            } else {
                if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show(res.ErrorsArray[0]);
                }
            }


            this.FTPDetailId = this.documentFilingBackupSettingPM.FTPDetailId;
            if (!AppTool.IsNullOrEmpty(this.FTPDetailId)) {
                this.myFTPService.get(this.FTPDetailId).subscribe(myResult => {
                    var myResponse: ServiceResponse = myResult;
                    if (!myResponse.HasError) {
                        var myEntity: any = myResponse.Result;
                        this.FTPDetailHost = myEntity.Host;

                    }
                });
            }

            this.IsLoad = true;
            this.CurrentSession.StopBusyIndicator();
        });

    }





    AddEditFTPDetails(type:string) {
        if (type == "Edit" && AppTool.IsNullOrEmpty(this.FTPDetailId)) return;

        var logWindow = new LogitudeWindow();

        if (type == "Edit") {
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { IsNew: false, EntityId: this.FTPDetailId };
 
        }
        else {
            logWindow.Title = "Add FTP Detail";
            logWindow.WindowArgs = { IsNew: true };
        }

        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.FTPDetailId = this.documentFilingBackupSettingPM.FTPDetailId = comp.EntityPM.Id;
                    this.FTPDetailHost = comp.EntityPM.Host;
                }
            });
        });
    }





    private isActive: boolean;
    public get IsActive() {
        if (this.documentFilingBackupSettingPM) {
            this.isActive = this.documentFilingBackupSettingPM.IsActive;
        }

        return this.isActive;
    }

    public set IsActive(value: boolean) {
        if (this.isActive != value)
            if (this.documentFilingBackupSettingPM) {
                this.documentFilingBackupSettingPM.IsActive = value;
            }
    }



    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    ValidationErrorsList: string[];
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.documentFilingBackupSettingPM) {
            if (this.ValidationErrorsList.length == 0) {
                this.CurrentSession.StartBusyIndicatorSaving();
                if (this.IsNewDocumentFilingBackupSetting) {
                    this.documentFilingBackupSettingPMService.insert(this.documentFilingBackupSettingPM).subscribe(res => {
                        this.CurrentSession.StopBusyIndicator();
                        if (!res.HasError) {
                            this.IsNewDocumentFilingBackupSetting = false;
                            this.CurrentSession.CloseCurrentWindow();
       
                        } else if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                            var messageWindow: MessageWindow = new MessageWindow();
                            messageWindow.Show(res.ErrorsArray[0]);
                        
                        }
                    });


                } else {
                    this.documentFilingBackupSettingPMService.update(this.documentFilingBackupSettingPM).subscribe(res => {
                        this.CurrentSession.StopBusyIndicator();
                        if (!res.HasError) {
                            this.CurrentSession.CloseCurrentWindow();
                        } else if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                            var messageWindow: MessageWindow = new MessageWindow();
                            messageWindow.Show(res.ErrorsArray[0]);
                   
                        }
                    });
                }


            }
        }



    }


 







}
