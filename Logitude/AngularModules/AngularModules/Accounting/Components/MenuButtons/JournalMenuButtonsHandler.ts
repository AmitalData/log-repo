
declare var window: any;
import {JournalPM} from '../../EntityPMs/JournalPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {JournalPMService} from '../../Services/StandardPMs/JournalPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {JournalValidator} from '../../Validators/JournalValidator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentOutPMService} from '../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {JournalOpService} from '../../Services/Others/JournalOpService';
import { DocumentOutPM } from '../../../Common/EntityPMs/DocumentOutPM';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {DocumentTypePM} from '../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypePMExtendedService} from '../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {ExportDocumentService} from '../../../Common/Services/DocumentServices/ExportDocumentService';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import {DownloadManager} from '../../../Infrastructure/Utilities/DownloadManager';
import {GeneralPrintHelper} from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import {JournalExtendedPMService} from '../../Services/ExtendedPMs/JournalExtendedPMService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';

export class JournalMenuButtonsHandler {
    public EntityPM: JournalPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "Journal"
    private _documentOutPMService: DocumentOutPMService = new DocumentOutPMService();
    private _journalOpService: JournalOpService = new JournalOpService();
    private _documentTypePMService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();
    private _exportDocumentService: ExportDocumentService = new ExportDocumentService();
    private CurrentSession = SessionLocator.SelectedSession;
    private CancelledStatusCode: string = "5";

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Journal')[0];
                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    // Status codes:
                    //  0- Draft
                    //  1- Waiting for Approve
                    //  2- Approved
                    //  3- Voided

                    switch (button.EventCode) {
                   

                        case "JournalSave": // save and close
                            {
                                if (this.EntityPM.StatusCode == "2" || this.EntityPM.StatusCode == "3" || this.EntityPM.StatusCode == this.CancelledStatusCode ) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "JournalApprove":
                            {
                                if (this.EntityPM.StatusCode == "3" || this.EntityPM.StatusCode == "2" || this.EntityPM.StatusCode == this.CancelledStatusCode  ) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "JournalSaveAsDraft":
                            {
                                if (this.EntityPM.StatusCode == "2") {
                                    button.IsHidden = true;
                                }

                               else if (this.EntityPM.StatusCode == "3" || this.EntityPM.StatusCode == this.CancelledStatusCode ) {
                                    button.IsDisabled = true;
                                }

                               else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "JournalVoid":
                            {

                                // the VOID button is only available on this case:          BUG #44819
                                //    - Approved Journal, not storno

                                this.SetVoidButtonEnability(button);

                                // if (this.EntityPM.StatusCode == "3" || this.EntityPM.AccountingEntityCode != "1") { // 3- Voided | 1- Journal
                                //     button.IsDisabled = true;
                                // }
                                // else if( this.EntityPM.AccountingEntityCode == "1" && this.EntityPM.StatusCode == "2" && (this.EntityPM.OriginalJournalId != null)) // STORNO  1-Journal
                                // {
                                //     button.IsDisabled = true;
                                // }
                                // else if (this.EntityPM.StatusCode == "2" && this.EntityPM.AccountingEntityCode == "1" && this.EntityPM.OriginalJournalId == null) { // 2- Approved
                                //     button.IsDisabled = false;
                                // }

                                break;
                            }
                        case "JournalPrint":
                            {
                                button.IsDisabled = false;

                                if (!AppTool.IsNullOrEmpty(SessionLocator.LoggedUserPM.SecurityLevel) && SessionLocator.LoggedUserPM.SecurityLevel <= this.EntityPM.SecurityLevel)
                                    button.IsDisabled = true;
                                //    if (this.EntityPM.StatusCode == "2" && this.EntityPM.OriginalJournalId == null) {
                                //    button.IsDisabled = false;
                                //}
                                //else {
                                //    button.IsDisabled = true;
                                //}
                                break;
                            }
                        case "CopyJournal":
                            {
                                if (this.EntityPM.StatusCode == "2"   && this.EntityPM.AccountingEntityCode == "1") {
                                    button.IsDisabled = false;

                                }
                                else {
                                    button.IsDisabled = true;
                                }

                                if (!AppTool.IsNullOrEmpty(SessionLocator.LoggedUserPM.SecurityLevel) &&  SessionLocator.LoggedUserPM.SecurityLevel <= this.EntityPM.SecurityLevel)
                                    button.IsDisabled = true;
                                break;
                            }
                    }
                }
            }
        }

        return menuButtons;
    }

    private SetVoidButtonEnability(button: MenuButtonPM) {
        const JournalAccountingEntity = "1";
        const RevaluationAccountingEntity = "8";
        const AdjustmentAccountingEntity = "10";
        const ApprovedStatusCode = "2";
        const VoidedStatusCode = "3";


        let IsVoidButtonEnabled: Boolean = this.EntityPM.AccountingEntityCode == JournalAccountingEntity ||
            this.EntityPM.AccountingEntityCode == "12" ||
            this.EntityPM.AccountingEntityCode == RevaluationAccountingEntity ||
            this.EntityPM.AccountingEntityCode == AdjustmentAccountingEntity;

        let IsApprovedAndNotStorno: Boolean = this.EntityPM.StatusCode == ApprovedStatusCode
            && this.EntityPM.AccountingEntityCode == JournalAccountingEntity
            && this.EntityPM.OriginalJournalId == null; // Not Storno

        if (IsVoidButtonEnabled || IsApprovedAndNotStorno) {
            button.IsDisabled = false;
        }

        else {
            button.IsDisabled = true;
        }

        if (this.EntityPM.ExternalSystem)
            button.IsDisabled = true;

        if (this.EntityPM.StatusCode == VoidedStatusCode)
            button.IsDisabled = true;

        if (this.EntityPM.StatusCode == this.CancelledStatusCode)
            button.IsDisabled = true;

        if (!AppTool.IsNullOrEmpty(SessionLocator.LoggedUserPM.SecurityLevel) && SessionLocator.LoggedUserPM.SecurityLevel <= this.EntityPM.SecurityLevel)
            button.IsDisabled = true;


    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        //this.copyAccountingDates();

        switch (menuButton.EventCode) {
            case "JournalSave": // save and close
                {
                    this.EntityPM.StatusCode = "1"; // Waiting

                    this.SaveChenges();
                    break;
                }
            case "JournalApprove":
                {
                    this.EntityPM.StatusCode = "2"; // Approved

                    this.EntityPM.UIProperties.SetEnabled("AccountingDate", "Journal", false);
                    this.EntityPM.UIProperties.SetEnabled("Reference1", "Journal", false);
                    this.EntityPM.UIProperties.SetEnabled("Reference2", "Journal", false);
                    this.EntityPM.UIProperties.SetEnabled("Reference3", "Journal", false);
                    this.EntityPM.UIProperties.SetEnabled("Notes", "Journal", false);

                    this.SaveChenges();
                    break;
                }
            case "JournalSaveAsDraft":
                {
                    this.EntityPM.StatusCode = "0"; // Draft
                    this.EntityPM.IsDirty = true;
                    this.SaveChenges();
                    break;
                }
            case "JournalVoid":
                {
                    this.ShowConfirmMessageAndVoidJournal();
                    break;
                }
            case "JournalPrint":
                {
                    this.PrintJournal();

                    //if (this.EntityPM.StatusCode != "2") { // Approved
                    //    this.PrintJournal();
                    //} else {
                    //    this.entityArgs.EditComponent.SaveChanges();
                    //    this.entityArgs.EditComponent.SaveCompleted.subscribe(($event) => {
                    //        if ($event == true) {
                    //            this.entityArgs.EditComponent.ReloadEntityPM();
                    //            this.SetEntityPM(this.entityArgs);
                    //            this.PrintJournal();
                    //        }
                    //    });
                    //}
                    break;
                }
            case "CopyJournal":
                {
                    this.OpenCopyJournalScreen();
                    break;
                }
        }


    }

    private ShowConfirmMessageAndVoidJournal() {
        var msg = TextCodeTranslator.Translate("Journal.O.ConfirmVoidJournal");
        var confirmWindow = new ConfirmWindow();
        const widthOfWindow = 300;
        const heightOfWindow = 150;

        confirmWindow.Width = widthOfWindow;
        confirmWindow.Height = heightOfWindow;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Accounting.General.B.OK");
        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Accounting.General.B.Cancel");
        confirmWindow.Show(msg);

        confirmWindow.WindowClosed.subscribe((event: any) => {

            if (confirmWindow.Yes) {
                this.entityArgs.EditComponent.StartBusyIndicatorSaving();
                this.VoidJournal();
            }
            else {
                confirmWindow.Close();
            }
        });
    }

    private VoidJournal() {
        let myJournalExtendedPMService: JournalExtendedPMService = new JournalExtendedPMService();
        myJournalExtendedPMService
            .VoidJournal(this.EntityPM.Tenant, this.EntityPM.Id, "", "", "")
            .subscribe((res: ServiceResponse) => {
                this.entityArgs.EditComponent.StopBusyIndicator();
                if (res.HasError) {
                    this.entityArgs.EditComponent.ValidationErrorsList = res.ErrorsArray;
                } else {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
    }

    SaveChenges() {

        // the validation will be in PM Service (custom validator)
        this.entityArgs.EditComponent.SaveChanges();
        this.entityArgs.EditComponent.SaveCompleted.subscribe(($event) => {
            if ($event == true) {
                this.entityArgs.EditComponent.ReloadEntityPM();


            }
        });
    }
    OpenCopyJournalScreen() {
        var windowTitle = TextCodeTranslator.Translate("Journal.B.CopyJournal");
        var windowArgs: any = {};
        windowArgs.JournalPM = this.EntityPM;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 250;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
        logWindow.Show('./Accounting/Components/Others/CopyJournalComponent');
    }
    copyAccountingDates() {
        // Copy AccountingDate from journal to journal lines:
        for (let line of this.EntityPM.JournalLines) {
            if (line.AccountingDate != this.EntityPM.AccountingDate) {
                line.AccountingDate = this.EntityPM.AccountingDate;
            }
            //line.ActionTypeCode = line.ActionCode;
        }
    }

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }

    private PrintJournal() {
        this.BuildDocument();
    }

    documentOutPM: DocumentOutPM;
    BuildDocument() {
        this.CurrentSession.StartBusyIndicator("Building document....");
        var objectTable = window.ObjectTables.filter(d => d.Name === "Journal")[0];
        var objectTableId = objectTable.Id;


        //1
        //Get document type
        this._documentTypePMService.GetDocumentTypeByCode("JRPR", SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
            var documentType: DocumentTypePM = response.Result;
            console.log("_documentTypePMService.GetDocumentTypeByCode", response)
            if (documentType) {

                //2
                //Get document copy
                var documentTypeCopy = documentType.DocumentTypeCopies[0];




                //3
                //Get document out
                this._documentOutPMService.getCreateDocumentOut(documentType.Id, this.EntityPM.Id, null, null, objectTableId, SessionLocator.Tenant).subscribe((res:any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var documentout: DocumentOutPM = pmResponse.Result;
                        console.log("_documentOutPMService.getCreateDocumentOut", response)
                        if (documentout) {

                            var documentOutCopy = documentout.DocumentOutCopies[0];
                            this.documentOutPM = documentout;

                            //if (documentOutCopy) {
                                //4
                                //Export to pdf
                                this._exportDocumentService.getDocumentPdfFile(documentType.Id, this.EntityPM.Id, objectTableId, null, null, documentout.Id, documentout.Tenant, documentTypeCopy.Id, SessionLocator.LoggedUserId).subscribe((res:any) => {
                                    var pmResponse: ServiceResponse = res;
                                    if (!pmResponse.HasError) {
                                        console.log("_exportDocumentService.getDocumentPdfFile", pmResponse)
                                        var myResult = pmResponse.Result;

                                        if (myResult != null) {

                                            if (documentOutCopy) {
                                                //5
                                                //view page
                                                var documentName =  documentOutCopy.Id;

                                                this.ViewPage(documentName, documentOutCopy.DocoumentTypeCopyName, documentout);
                                            } else {
                                                this.BuildDocument(); // resend the request, the method [getCreateDocumentOut] does not create document out copy!!
                                                console.warn("Cannot find document out copy, resend request...");
                                            }



                                        }
                                        else
                                            this.StopBusyIndicator();

                                    } else {
                                        if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                            console.log(pmResponse.ErrorsArray[0]);
                                        }
                                        this.StopBusyIndicator();
                                    }

                                });
                            //} else {
                            //    console.warn("Cannot find document out copy, resend request...");
                            //    //this.CurrentSession.StopBusyIndicator();
                            //    this.BuildDocument(); // resend the request, the method [getCreateDocumentOut] does not create document out copy!!
                            //}




                        } else {
                            console.error("Cannot create document out!", res);
                            this.CurrentSession.StopBusyIndicator();
                        }
                    }


                });



            } else {
                var msg = new MessageWindow();
                this.CurrentSession.StopBusyIndicator();
                msg.Show("No document type found!");
            }

        });



    }

    ViewPage(documentName: string, docoumentTypeCopyName: string, documentOut: DocumentOutPM) {

        //ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, docoumentTypeCopyName + " Viewing");


        //DownloadManager.DownloadPage(documentName , documentOut.SecurityId);

        //this.StopBusyIndicator();

        var myPrintHelper = new GeneralPrintHelper(this.ObjectTableName, "JRPR", this.EntityPM.Id, null, this.EntityPM.AccountingEntityReference, null);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "Journal");
            myPrintHelper.ShowPrintControl();
        }

    }


}
