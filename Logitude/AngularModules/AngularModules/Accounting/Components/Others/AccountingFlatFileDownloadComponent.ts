import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnDestroy}  from '@angular/core';
import {TaxReportPM} from '../../EntityPMs/TaxReportPM';
import { DocumentOutPM } from '../../../Common/EntityPMs/DocumentOutPM';
import { BatchTaskExecutionList } from '../../../Infrastructure/EntityLists/BatchTaskExecutionList';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { BatchTaskExecutionListService } from '../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { DocumentsFilingViewsExtService } from '../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService';
import {TaxReportExtendedPMService} from '../../Services/ExtendedPMs/TaxReportExtendedPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { TaxDeductionReportExtendedPMService } from '../../Services/ExtendedPMs/TaxDeductionReportExtendedPMService';
import { TaxDeductionReportPM } from '../../EntityPMs/TaxDeductionReportPM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

declare var window;

@Component({
    selector: 'AccountingFlatFileDownloadComponent',
    
    templateUrl: './AccountingFlatFileDownloadComponent.html',
})
export class AccountingFlatFileDownloadComponent extends BaseComponent implements OnDestroy {
    ObjectTableName: string = "TaxReport";
    DataContext: any = this;
    reportPM: TaxReportPM;
    taxDeductionPM: TaxDeductionReportPM;
    public ValidationErrorsList: string[] = [];
    timerInterval:number = 1000;
    Loading: boolean = false;
    Success: boolean = false;
    Failed: boolean = false;
    LabelText: string = "";
    docFilingPM: any;
    btePM: any;
    bteList: BatchTaskExecutionList;
    timer: any;
    public isRTL: boolean = false;

    _DocumentsFilingViewsExtService: DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService();
    _BatchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    _TaxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
    taxDeductionReportExtendedPMService: TaxDeductionReportExtendedPMService = new TaxDeductionReportExtendedPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

    }
    ngOnDestroy() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    }
    SetWindowArgs(args: any) {
        if (args != null) {
            this.ChangeStatus();

            this.ObjectTableName = args.ObjectTableName;

            // Tax Report
            if (this.ObjectTableName == "TaxReport") {
                this.reportPM = args.EntityPM;
                // if the file does not need rebuild, show download button
                if (!this.reportPM.NeedsRebulid) {
                    //update status
                    this.ChangeStatus("inprogress");

                    //get documentid
                    this.GetDocument();
                }

            }
            else if (this.ObjectTableName == "TaxDeductionReport") {
                this.taxDeductionPM = args.EntityPM;
            }


            if (args.StartDirectly)
                this.RunService(false);

            if (args.TimerInterval)
                this.timerInterval = args.TimerInterval;





        }
    }


    RunService(byButton: boolean = false) {

        this.ChangeStatus("creating");

        switch (this.ObjectTableName) {

            // Tax Report
            case "TaxReport":
                {
                    if (byButton || this.reportPM.NeedsRebulid) {
                        this._TaxReportExtendedPMService.DownloadPNC874FileInBatch(this.reportPM).subscribe((myResult:ServiceResponse) => {
                            var mm: ServiceResponse = myResult;
                            if (!myResult.HasError) {
                                var entity = mm.Result;
                                this.btePM = entity;

                                this.ChangeStatus("inprogress");

                                this.timer = setInterval(() => {
                                    this.GetBTE();
                                }, this.timerInterval);
                            }
                            else {
                                this.Loading = false;
                                this.Success = false;
                                this.Failed = true;
                               this.ShowError(myResult.ErrorsArray);
                                this.CurrentSession.CloseCurrentWindow();
                            }
                        });
                    } else
                    {
                        //update status
                        this.ChangeStatus("ready");

                        //get documentid
                        this.GetDocument();
                    }
                    break;
                }

            case "TaxDeductionReport":
                {
                    //if (byButton || this.reportPM.NeedsRebulid) {
                        //this.taxDeductionReportExtendedPMService.DownloadTaxDeduction856FileInBatch(this.taxDeductionPM).subscribe(myResult => {
                        //    var mm: ServiceResponse = myResult;
                        //    var entity = mm.Result;
                        //    this.btePM = entity;

                        //    this.ChangeStatus("inprogress");

                        //    this.timer = setInterval(() => {
                        //        this.GetBTE();
                        //    }, this.timerInterval);

                        //});
                    //} else {
                    //    //update status
                    //    this.ChangeStatus("ready");

                    //    //get documentid
                    //    this.GetDocument();
                    //}
                    break;
                }

            default:
                {
                    // ...
                    break;
                }
        }
    }
    GetBTE() {
        this._BatchTaskExecutionListService.getSingle(this.btePM.Id).subscribe((myResult:any) => {
            console.log("[_BatchTaskExecutionListService.getSingle]", myResult);
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.bteList = mm.Result;
                if (this.bteList.StatusCode == "D") // D- Done
                {

                    // ...
                    //get documentid
                    this.GetDocument();

                    //stop timer
                    if (this.timer) {
                        clearInterval(this.timer);
                    }


                }
                else if (this.bteList.StatusCode == "F") // F- Failed
                {
                    //stop timer
                    if (this.timer) {
                        clearInterval(this.timer);
                    }

                    //update status
                    this.ChangeStatus("failed");

                }
            }
            else {
            }
        });

    }
    GetDocument() {

        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];
        if (this.ObjectTableName == "TaxReport") {


            this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.reportPM.Id, objectTable.Id).subscribe((myResult:any) => {
                console.log("[GetLastDocumentsFilingPM]", myResult);
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.docFilingPM = mm.Result;

                    if (!this.reportPM.NeedsRebulid)
                        this.ChangeStatus("ready");
                    else
                        this.ChangeStatus("done");

                }
                else {
                    console.error("GetLastDocumentsFilingPM ERROR", mm);
                }
            });
        }
        else if (this.ObjectTableName == "TaxDeductionReport") {
            this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.taxDeductionPM.Id, objectTable.Id).subscribe((myResult:any) => {
                console.log("[GetLastDocumentsFilingPM]", myResult);
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.docFilingPM = mm.Result;


                        this.ChangeStatus("ready");


                }
                else {
                    console.error("GetLastDocumentsFilingPM ERROR", mm);
                }
            });
        }
    }

    //#region Buttons
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

    }
    DownloadButtonClicked() {
        DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);
    }
    ShowError(error=null) {
        var msg = this.bteList? this.bteList.ErrorLog : error;
        var msgbox = new MessageWindow();
        msgbox.Width = 500;
        // msgbox.Height = 400;
        msgbox.RTL = this.isRTL;
        msgbox.Show(msg);
    }
    //#endregion

    GetLabelColor() {
        if (this.Success) return "green";
        else if (this.Failed) return "red";
        else if (this.Loading) return "blue";
        else return "black";
    }
    ChangeStatus(status: string="") {
        switch (status) {
            case "ready": { // file didn't needs rebuild
                this.Loading = false;
                this.Success = true;
                this.Failed = false;
                this.LabelText = TextCodeTranslator.Translate("General.O.FileIsReady");
                //this.LabelText = "Creating file ...";
                break;
            }
            case "creating": {
                this.Loading = true;
                this.Success = false;
                this.Failed = false;
                this.LabelText = TextCodeTranslator.Translate("General.O.CreatingFile");
                //this.LabelText = "Creating file ...";
                break;
            }
            case "inprogress": {
                this.Loading = true;
                this.Success = false;
                this.Failed = false;
                this.LabelText = TextCodeTranslator.Translate("General.O.PleaseWaitCreatingFile");
                //this.LabelText = "Please wait while creating file ...";
                break;
            }
            case "done": {
                this.Loading = false;
                this.Success = true;
                this.Failed = false;
                this.LabelText = TextCodeTranslator.Translate("General.O.FileCreated");
                //this.LabelText = "File created";
                break;
            }
            case "failed": {
                this.Loading = false;
                this.Success = false;
                this.Failed = true;
                this.LabelText = TextCodeTranslator.Translate("General.O.ErrorwhileCreating");
                this.ShowError();
                //this.LabelText = "Error while creating!";
                break;
            }
            default: {
                this.Loading = false;
                this.Success = false;
                this.Failed = false;
                this.LabelText = TextCodeTranslator.Translate("General.O.clicktoStartCreatingFile");
                //this.LabelText = "Please click create to start creating file";
                break;
            }
        }
    }




}
