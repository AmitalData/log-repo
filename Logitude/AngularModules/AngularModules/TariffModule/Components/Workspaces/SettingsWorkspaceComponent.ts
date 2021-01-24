import { Component, OnInit, ViewChildren, QueryList, OnDestroy } from '@angular/core';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { AppTool } from '../../../Infrastructure/Tools';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { TariffDomainService, TariffFilterParameter } from '../../Services/TariffDomainService';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { BatchTaskExecutionListService } from '../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from '../../../Infrastructure/EntityLists/BatchTaskExecutionList';
import { BatchTaskExecutionPM } from '../../../Infrastructure/EntityPMs/BatchTaskExecutionPM';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
declare var ResultAsArray: any;

@Component({
    selector: 'SettingsComponent',
    
    templateUrl: './SettingsWorkspaceComponent.html',
    providers: [EntityResourceService, TariffDomainService],
})

export class SettingsWorkspaceComponent implements OnInit, OnDestroy {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private CurrentSession = SessionLocator.SelectedSession;
    private DocumentExtendedService: DocumentsFilingExtendedPMService;
    public IsTariffGenerateVisible: boolean = false;
    public IsTariffSettingsVisible: boolean = false;
    constructor(private _entityResourceService: EntityResourceService, private tariffDomainService: TariffDomainService) {
        //SSthis.RunComponent();
    }
    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe((response:any) => {
            if (this.CurrentSession == null)
                this.CurrentSession = SessionLocator.SelectedSession;
        });
        this.InitComponent();
        this.SetQueriesVisibility();
    }

    ngOnDestroy() {
        this.StopTimer();
    }

    private InitComponent() {
        this.DocumentExtendedService = new DocumentsFilingExtendedPMService();
        this.SetQueriesVisibility();
    }

    SetQueriesVisibility() {
        if (FeatureLocator.HasFeaturePermession("Tariff", "TARIFFGENERATE")) {
            this.IsTariffGenerateVisible = true;
        }

        if (FeatureLocator.HasFeaturePermession("TariffSetting", "READ")) {
            this.IsTariffSettingsVisible = true;
        }
    }

    TariffSettingsClicked() {
        this._entityResourceService.getEntityResourceByTableName("TariffSetting", 0).subscribe((response:any) => {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = "Tariff Settings";
            logWindow.Show('./TariffModule/Components/Workspaces/TariffSettingComponent');
        });
    }

    private timer: any;
    private timerInterval: number = 5000;
    private IsLoading: boolean = false;
    private batchEntity: BatchTaskExecutionPM;
    GenerateTariffsClicked() {
        this.CurrentSession.StartBusyIndicator("Generating...");

        this.tariffDomainService.GenerateTariffs().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                this.batchEntity = myResponse.Result;

                if (this.batchEntity != null) {
                    this.timer = setInterval(() => { this.GetBTE(); }, this.timerInterval);
                }
            }

            else {
                this.CurrentSession.StopBusyIndicator();
                var window = new MessageWindow();
                window.Show(myResponse.ErrorsArray[0]);
            }
        });
    }

    // Generate from Excel 
    private FileName: string;
    GenerateExcelTariffsClicked(fileEvent) {
        var file = fileEvent.target.files[0];

        if (file) {
            var extension: string = file.name.split('.')[1];

            if (extension.includes("xls")) {
                var file = fileEvent.target.files[0];
                this.UploadExcel(file);
            }

            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("You have to upload excel files only");
            }
        }
    }
    UploadExcel(file: any) {
        this.FileName = null;
        if (!AppTool.IsNullOrEmpty(file.name)) {
            var name = file.name.split('.');
            if (name.length == 2) {
                this.FileName = name[0];
            }
        }
        if (file && file.size > 0) {
            this.DocumentExtendedService.GetFileSizeAndUnit(file.size).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        this.StartUploadingExcelFile(file);
                    }
                }
            });
        }
    }
    StartUploadingExcelFile(file: any) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    }
    ConvertArrayBufferToBase64(file: any, context: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            var filter = new TariffFilterParameter();
            filter.FileData = window.btoa(binary);
            filter.FileName = context.FileName;
            context.SendExcelToServer(filter);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
    SendExcelToServer(filter: any) {
        this.CurrentSession.StartBusyIndicator("Generating...");
        this.tariffDomainService.GenerateTariffsFromExcel(filter).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.batchEntity = response.Result;

                if (this.batchEntity != null) {
                    this.CurrentSession.StartBusyIndicator("Uploading File...");
                    this.CheckBatchTaskExecution(this.batchEntity.Id);
                }

                //this.CurrentSession.StopBusyIndicator();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                var window = new MessageWindow();
                window.Show(response.ErrorsArray[0]);
            }
        });
    }

    CheckBatchTaskExecution(BatchTaskExecutionId: string) {
        var iBatchService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
        iBatchService.getSingle(BatchTaskExecutionId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BatchTaskExecutionList = myResponse.Result;

                if (list.StatusCode == "D") {
                    this.CurrentSession.StopBusyIndicator();

                    var window = new MessageWindow();
                    window.Show("File uploaded successfully");
                }

                else if (list.StatusCode == "F") {
                    this.CurrentSession.StopBusyIndicator();
                    var window = new MessageWindow();
                    window.Show("There was an error uploading excel file. Please try again later");
                }

                else {
                    this.CheckBatchTaskExecution(BatchTaskExecutionId);
                }
            }

            else {
                this.CurrentSession.StopBusyIndicator();
                var window = new MessageWindow();
                window.Show(myResponse.ErrorsArray[0]);
            }
        });
    }

    GetBTE() {
        if (!this.IsLoading) {
            this.IsLoading = true;

            var bteList: BatchTaskExecutionList;
            var myService: BatchTaskExecutionListService = new BatchTaskExecutionListService();

            myService.getSingle(this.batchEntity.Id).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    bteList = myResponse.Result;

                    if (bteList.StatusCode == "D") {
                        this.CurrentSession.StopBusyIndicator();
                    }

                    else if (bteList.StatusCode == "F") {
                        this.CurrentSession.StopBusyIndicator();
                        this.StopTimer();
                    }
                }

                else {
                    this.CurrentSession.StopBusyIndicator();
                    this.StopTimer();

                    var window = new MessageWindow();
                    window.Show(myResponse.ErrorsArray[0]);

                }

                this.IsLoading = false;
            });
        }
    }
   
    StopTimer() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    }
}
