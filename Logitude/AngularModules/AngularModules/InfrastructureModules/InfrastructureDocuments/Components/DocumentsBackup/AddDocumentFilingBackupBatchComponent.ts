import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentFilingBackupBatchPM} from '../../../../Common/EntityPMs/DocumentFilingBackupBatchPM';
import {DocumentFilingBackupBatchPMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentFilingBackupBatchPMExtendedService';
import {DocumentFilingBackupBatchPMService} from '../../../../Common/Services/StandardPMs/DocumentFilingBackupBatchPMService'
import {DateTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    selector: 'AddDocumentFilingBackupBatchComponent',
    templateUrl: './AddDocumentFilingBackupBatchComponent.html',
})

export class AddDocumentFilingBackupBatchComponent extends BaseComponent implements OnInit {
    documentFilingBackupBatchPMExtendedService: DocumentFilingBackupBatchPMExtendedService;

    DataContext: any = this;
    IsNewDocumentFilingBackupSetting: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.documentFilingBackupBatchPMExtendedService = new DocumentFilingBackupBatchPMExtendedService();
        this.SetRequired();

    }

    ngOnInit() {

    }

    SetRequired() {
        var isRequiredFromDate: boolean = !this.FromDatetime ? true:false;
        this.UIProperties.SetRequired("FromDatetime", "DocumentFilingBackupBatch", isRequiredFromDate);


        var isRequiredToDate: boolean = !this.ToDatetime ? true : false;
        this.UIProperties.SetRequired("ToDatetime", "DocumentFilingBackupBatch", isRequiredToDate);
    }



    private includeBackedUp: boolean;
    public get IncludeBackedUp() {
       
        return this.includeBackedUp;
    }

    public set IncludeBackedUp(value: boolean) {
        if (this.includeBackedUp != value)
            this.includeBackedUp = value;
    }

    
    private fromDatetime: Date;
    public get FromDatetime() {
        return this.fromDatetime;
    }

    public set FromDatetime(value: Date) {
        if (this.fromDatetime != value) this.fromDatetime = value;
   
        this.SetRequired();
    }

    private toDatetime: Date;
    public get ToDatetime() {

        return this.toDatetime;
    }

    public set ToDatetime(value: Date) {
        if (this.toDatetime != value) this.toDatetime = value;
        this.SetRequired();
    }
 

   

    CancelButtonClicked() {



        this.CurrentSession.CloseCurrentWindow();
    }

    ValidationErrorsList: string[];
    OkButtonClicked() {
    
        this.ValidationErrorsList = [];

        if (!this.FromDatetime || !this.ToDatetime) {
            if (!this.FromDatetime) this.ValidationErrorsList.push("From Date field is required");
            if (!this.ToDatetime) this.ValidationErrorsList.push("To Date field is required");

        }

       else if (this.FromDatetime > this.ToDatetime) this.ValidationErrorsList.push("From date must be smaller\equal to To date");
        else {
            if (DateTool.GetDaysBetweenDates(this.FromDatetime, this.ToDatetime) > 360) {
                this.ValidationErrorsList.push("The backup period should be less or equal to one year");
            }
        }
        
        if (this.ValidationErrorsList.length == 0) {

            var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();

            //this.FromDatetime = DateTool.GetDate(this.FromDatetime.getUTCFullYear(), this.FromDatetime.getUTCMonth(), this.FromDatetime.getUTCDate() , todayDateTime.getUTCHours(), todayDateTime.getUTCMinutes(), todayDateTime.getUTCSeconds());

            //this.ToDatetime = DateTool.GetDate(this.ToDatetime.getUTCFullYear(), this.ToDatetime.getUTCMonth(), this.ToDatetime.getUTCDate(), todayDateTime.getUTCHours(), todayDateTime.getUTCMinutes(), todayDateTime.getUTCSeconds());

            this.InsertDocumentFilingBackupBatchPMService();
        }
  
    }


    InsertDocumentFilingBackupBatchPMService() {
        this.CurrentSession.StartBusyIndicatorSaving();

        var service: DocumentFilingBackupBatchPMService = new DocumentFilingBackupBatchPMService();
        var documentFilingBackupBatchPM: DocumentFilingBackupBatchPM = new DocumentFilingBackupBatchPM();
        documentFilingBackupBatchPM.ToDatetime = this.ToDatetime;
        documentFilingBackupBatchPM.FromDatetime = this.FromDatetime;
        documentFilingBackupBatchPM.IncludeBackedUp = this.IncludeBackedUp;
        
        documentFilingBackupBatchPM.Status = "Created";
        documentFilingBackupBatchPM.Tenant = SessionLocator.Tenant;
        documentFilingBackupBatchPM.TotalFailed = 0;
        documentFilingBackupBatchPM.TotalDocuments = 0;
        documentFilingBackupBatchPM.TotalSucceeded = 0;
        service.insert(documentFilingBackupBatchPM).subscribe(res => {
            this.CurrentSession.StopBusyIndicator();
            if (!res.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
            else {
                if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show(res.ErrorsArray[0]);
                }

            }
        });

    }







}
