import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentFilingBackupBatchPM} from '../../../../Common/EntityPMs/DocumentFilingBackupBatchPM';
import {DocumentFilingBackupBatchPMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentFilingBackupBatchPMExtendedService';

@Component({
    moduleId: module.id,
    selector: 'DocumentFilingBackupBatchesComponent',
    templateUrl: './DocumentFilingBackupBatchesComponent.html',
})

export class DocumentFilingBackupBatchesComponent extends BaseComponent implements OnInit {
    documentFilingBackupBatchPMExtendedService: DocumentFilingBackupBatchPMExtendedService;
    public BatchObsList: Array<DocumentFilingBackupBatchDataViewModel> = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.documentFilingBackupBatchPMExtendedService = new DocumentFilingBackupBatchPMExtendedService();
    }

    ngOnInit() {

        this.LoadData();
    }
    


    private selectedItemBatch: DocumentFilingBackupBatchDataViewModel;
    public get SelectedItemBatch() { return this.selectedItemBatch; }
    public set SelectedItemBatch(value: DocumentFilingBackupBatchDataViewModel) { if (this.selectedItemBatch != value) this.selectedItemBatch = value; }




    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.BatchObsList = [];
        this.documentFilingBackupBatchPMExtendedService.GetDocumentFilingBackupBatchPMs().subscribe(res => {
            if (!res.HasError) {
                var documentFilingBackupBatchPMs: Array<DocumentFilingBackupBatchPM> = res.Result;
                documentFilingBackupBatchPMs.forEach(item => {
                    this.BatchObsList.push(new DocumentFilingBackupBatchDataViewModel(item));
                });

                this.BatchObsList = this.BatchObsList.sort((a, b) => (a.CreateDateTime > b.CreateDateTime) ? -1 : ((a.CreateDateTime < b.CreateDateTime) ? 1 : 0));


            } else {
                if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show(res.ErrorsArray[0]);
                }

            }
            this.CurrentSession.StopBusyIndicator();
        });
    }




    RefreshBatch() {
        this.LoadData();
    }

    AddBatch() {

            var logitudeWindow: LogitudeWindow = new LogitudeWindow();
            logitudeWindow.WindowArgs = {Parent: this };
            logitudeWindow.Title = "Add New Batch";
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 300;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentsBackup/AddDocumentFilingBackupBatchComponent');
            logitudeWindow.WindowClosed.subscribe(p => {
                if (p == "OK") {
                    this.LoadData();
                }
            });
        
    }


    SettingButtonClicked() {

            var logWindow = new LogitudeWindow();
            logWindow.Title = "Documents Backup Setting";
            logWindow.Width = 500;
            logWindow.Height = 300;
            logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentsBackup/DocumentFilingBackupSettingComponent');

    }




    CloseButtonClicked() {



        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {



        this.CurrentSession.CloseCurrentWindow();
    }

}

export class DocumentFilingBackupBatchDataViewModel {

    public EntityPM: DocumentFilingBackupBatchPM;

    constructor(entityPM: DocumentFilingBackupBatchPM) {
        this.EntityPM = entityPM;
    }

    public get BatchNumber() { return this.EntityPM.BatchNumber; }
    public set BatchNumber(value: string) { if (this.EntityPM.BatchNumber != value) this.EntityPM.BatchNumber = value; }

    public get DoneDate() { return this.EntityPM.DoneDate; }
    public set DoneDate(value: Date) { if (this.EntityPM.DoneDate != value) this.EntityPM.DoneDate = value; }


    public get CreateDateTime() { return this.EntityPM.CreateDateTime; }
    public set CreateDateTime(value: Date) { if (this.EntityPM.CreateDateTime != value) this.EntityPM.CreateDateTime = value; }



    public get FromDatetime() { return this.EntityPM.FromDatetime; }
    public set FromDatetime(value: Date) { if (this.EntityPM.FromDatetime != value) this.EntityPM.FromDatetime = value; }


    public get ToDatetime() { return this.EntityPM.ToDatetime; }
    public set ToDatetime(value: Date) { if (this.EntityPM.ToDatetime != value) this.EntityPM.ToDatetime = value; }



    public get Status() { return this.EntityPM.Status; }
    public set Status(value: string) { if (this.EntityPM.Status != value) this.EntityPM.Status = value; }

    public get TotalFailed() { return this.EntityPM.TotalFailed; }
    public set TotalFailed(value: number) { if (this.EntityPM.TotalFailed != value) this.EntityPM.TotalFailed = value; }


    public get TotalDocuments() { return this.EntityPM.TotalDocuments; }
    public set TotalDocuments(value: number) { if (this.EntityPM.TotalDocuments != value) this.EntityPM.TotalDocuments = value; }


    public get TotalSucceeded() { return this.EntityPM.TotalSucceeded; }
    public set TotalSucceeded(value: number) { if (this.EntityPM.TotalSucceeded != value) this.EntityPM.TotalSucceeded = value; }




}
