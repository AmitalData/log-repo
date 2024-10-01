declare var window: any;
import { TaxReportPM } from '../../EntityPMs/TaxReportPM';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TaxReportExtendedPMService } from '../../Services/ExtendedPMs/TaxReportExtendedPMService';
import { BatchTaskExecutionListService } from 'Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from 'Infrastructure/EntityLists/BatchTaskExecutionList';
import { TaxReportPMService } from 'Accounting/Services/StandardPMs/TaxReportPMService';
import { EntityPMService } from 'Infrastructure/Services/EntityPMService';


export class TaxReportMenuButtonsHandler {
    public EntityPM: TaxReportPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "TaxReport"
    taxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
    _BatchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    btePM: any;
    bteList: BatchTaskExecutionList;
    TaxReportPMService: TaxReportPMService = new TaxReportPMService();
    EntityPMService: EntityPMService = new EntityPMService();

    private CurrentSession = SessionLocator.SelectedSession;
    private BlockReportCancel: boolean = false;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'TaxReport')[0];


                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    switch (button.EventCode) {
                        case "TRCN":
                            {
                                if (this.EntityPM.IsCancelled || this.EntityPM.StatusCode == TaxReportStatus.TransmittedAndClosingJournal) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }

                        case "TRDL": {
                            if (this.EntityPM.IsCancelled) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                            break;
                        }
                        case "UPLD": {
                            this.SetUploadButtonEnabilityAccordingToConsolidationVAT(button);

                            break;
                        }
                        case MenuButton.ReturnToDraft: {

                            this.SetReturnToDraftButtonStatus(button);
                        }
                        case MenuButton.ClosingJournal: {
                            button.IsDisabled = this.EntityPM.StatusCode != TaxReportStatus.Transmitted;
                            break;
                        }
                        case MenuButton.CancelClosingJournal: {
                            button.IsDisabled = this.EntityPM.StatusCode != TaxReportStatus.TransmittedAndClosingJournal;
                            break;
                        }
                        case "REBB": { // Rebuild
                            if (this.EntityPM.CanRecalculate) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                    }

                    this.SetMenuButtonEnabilityAccordingToCancelationProgress(button);
                }
            }
        }

        return menuButtons;
    }




    private SetReturnToDraftButtonStatus(button: MenuButtonPM) {
        this.taxReportExtendedPMService.GetReturnToDraftButtonStatus(this.EntityPM.CreateDate).subscribe((response: any) => {
            this.CurrentSession.StopBusyIndicator();
            if (response != null) {
                if (!response.Result.Result && this.EntityPM.StatusCode == TaxReportStatus.Transmitted) {
                    button.IsDisabled = false;
                }
                else {
                    button.IsDisabled = true;
                }
            }
        });
    }
    private SetUploadButtonEnabilityAccordingToConsolidationVAT(button: MenuButtonPM) {
        if (this.EntityPM.StatusCode == "D" || this.EntityPM.StatusCode == "E") {
            button.IsDisabled = false;
        }
        else {
            button.IsDisabled = true;
        }
    }


    private SetMenuButtonEnabilityAccordingToCancelationProgress(button: MenuButtonPM) {
        if (this.EntityPM.StatusCode == TaxReportStatus.CancelationInProgress)
            button.IsDisabled = true;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        switch (menuButton.EventCode) {
            case "TRCN": // Cancel
                {
                    this.CancelTaxReport();
                    break;
                }
            case "TRDL": // Download
                {
                    var windowTitle = TextCodeTranslator.Translate("TaxReport.B.Download");

                    var windowArgs: any = {};
                    windowArgs.ObjectTableName = "TaxReport";
                    windowArgs.EntityPM = this.EntityPM;
                    windowArgs.StartDirectly = false; // start service after show window
                    windowArgs.TimerInterval = 1000; // wait time between requests

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 500;
                    logWindow.Height = 220;
                    logWindow.Title = windowTitle;
                    logWindow.ShowCloseButton = true;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(($event: any) => {
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    });
                    logWindow.Show('./Accounting/Components/Others/AccountingFlatFileDownloadComponent');
                    break;
                }
            case MenuButton.ReturnToDraft: // Cancel
                {
                    this.EntityPM.StatusCode = TaxReportStatus.Darft;
                    this.entityArgs.EditComponent.SaveChanges();
                    this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        } else {
                            this.EntityPM.StatusCode = TaxReportStatus.Transmitted;
                        }
                    });

                    break;
                }
            case MenuButton.Upload: // upload
                {
                    //this._entityResourceService.getEntityResourceByTableName("GLAccount", 0).subscribe((response: any) => {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Width = 780;
                    logitudeWindow.Height = 350;
                    var windowArgs: any = {};
                    windowArgs.ObjectTableName = "TaxReport";
                    windowArgs.EntityPM = this.EntityPM;
                    logitudeWindow.WindowArgs = windowArgs;
                    logitudeWindow.Title = TextCodeTranslator.Translate("TaxReport.B.UploadRows");
                    logitudeWindow.WindowClosed.subscribe(($event: any) => {
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    });
                    logitudeWindow.Show('./Accounting/Components/Others/TaxReportUploadLinesComponent');
                    //});
                    break;
                }
            case MenuButton.ClosingJournal: {
                this.CreateClosingJournalButtonClicked();
                break;
            }
            case MenuButton.CancelClosingJournal: {
                this.CancelClosingJournal();
                break;
            }
            case "REBB": {
                this.ConfirmRecalculatingReport();
            }

        }



    }

    private CancelTaxReportInBatch() {
        this.taxReportExtendedPMService.CancelTaxReportInBatch(this.EntityPM).subscribe((myResult: ServiceResponse) => {
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.entityArgs.EditComponent.ReloadEntityPM();
            }
            else {
                this.EntityPM.IsCancelled = false;
                this.EntityPM.StatusCode = TaxReportStatus.CancelationFailed;
                this.entityArgs.EditComponent.SaveChanges();
                this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                });
            }
        });
    }

    timer: any;
    timerInterval: number = 1000;
    ConfirmRecalculatingReport() {
        let confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate('InterestReport.O.Approve');
        confirmWindow.NoButtonText = TextCodeTranslator.Translate('InterestReport.O.Cancel');

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator("Refreshing ...");
                this.EntityPM.RecalculateData = true;
                //
                //this.entityArgs.EditComponent.SaveChanges();

                //this.EntityPMService.update(this.ObjectTableName, this.EntityPM).then((res: any) => {

                this.TaxReportPMService.update(this.EntityPM).subscribe((myResult: any) => {
                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        var entity = mm.Result;
                        this.taxReportExtendedPMService.PostCreateTaxReportInBatch(entity).subscribe((myResult: any) => {
                            var mm: ServiceResponse = myResult;
                            var entity = mm.Result;
                            this.btePM = entity;

                            //  this.ChangeStatus("inprogress");

                            this.timer = setInterval(() => {
                                this.GetBTE();
                            }, this.timerInterval);

                        });
                    }
                });
            }
        });

        confirmWindow.Show(TextCodeTranslator.Translate('TaxReport.O.ConfirmRecalculateReport'));
    }

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    GetBTE() {
        this._BatchTaskExecutionListService.getSingle(this.btePM.Id).subscribe((myResult: any) => {
            console.log("[_BatchTaskExecutionListService.getSingle]", myResult);
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.bteList = mm.Result;
                if (this.bteList.StatusCode == "D") // D- Done
                {

                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                    if (!this.EntityPM.RecalculateData) {
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                            this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({ EntityId: this.EntityPM.Id, ObjectTableName: this.ObjectTableName });
                                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                                    //this.CancelButtonClicked();
                                });
                            });
                    }else{
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    }
                    this.CurrentSession.StopBusyIndicator();
                    //stop timer
                    if (this.timer) {
                        clearInterval(this.timer);
                    }


                }
                else if (this.bteList.StatusCode == "F") // F- Failed
                {
                    //stop timer

                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                    if (this.timer) {
                        clearInterval(this.timer);
                    }

                    //update status
                    //   this.ChangeStatus("failed");

                }
            }
            else {
            }
        });

    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
    CreateClosingJournalButtonClicked() {
        var taxReportMonthDate = new Date(this.EntityPM.TaxReportMonth)
        if (taxReportMonthDate.getFullYear() < 2022 || (taxReportMonthDate.getFullYear() === 2022 && taxReportMonthDate.getMonth() + 1 < 5)) {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 500;
            messageWindow.IsMessageMultiLine = true;
            let message: string = TextCodeTranslator.Translate(TextCode.TaxReportCloseJournalNotSupported);
            messageWindow.Show(message);
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();

        this.taxReportExtendedPMService.GetTaxReportReconciledLines(this.EntityPM.Id)
            .subscribe((response: ServiceResponse) => {
                this.StopBusyIndicator();
                let reconciledTaxReportLines: any = response?.Result;
                if (!reconciledTaxReportLines) {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Show(TextCodeTranslator.Translate(TextCode.TaxReportClosingJournalConfirmationMessage))
                    confirmWindow.Width = 400;
                    confirmWindow.WindowClosed.subscribe(event => {
                        if (confirmWindow.Yes) {
                            this.CreateClosingJournal();
                        }
                    });
                } else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 500;
                    messageWindow.IsMessageMultiLine = true;
                    let message: string = TextCodeTranslator.Translate(TextCode.TaxReportCantBeClosedValidationMessage);
                    message += '\n(' + reconciledTaxReportLines + ')'
                    messageWindow.Show(message);
                }
            });
    }
    CreateClosingJournal() {
        this.CurrentSession.StartBusyIndicatorCreating();

        this.taxReportExtendedPMService.CloseTaxReport(this.EntityPM.Id)
            .subscribe((response: ServiceResponse) => {
                this.StopBusyIndicator();
                if (response.HasError) {
                    const message = new MessageWindow();
                    message.ShowErrorIcon = true;
                    message.Width = 400;
                    message.Show(response.ErrorsArray.join('\n'));
                } else {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                    const message = new MessageWindow();
                    message.ShowSuccessIcon = true;
                    message.Width = 400;
                    message.Show(TextCodeTranslator.Translate(TextCode.TaxReportCloseJournalRunInBackground));
                }
            }, (error) => {
                new MessageWindow().Show(error || 'Somthing wrong happend!');
            });
    }


    CancelTaxReport() {
        this.CurrentSession.StartBusyIndicatorCreating();

        this.taxReportExtendedPMService.GetActiveFutureReportsExist(this.EntityPM.CreateDate)
            .subscribe((response: any) => {
                this.StopBusyIndicator();
                if (response != null) {
                    if (!response.Result.Result) {
                        this.CancelTaxReportInner();
                    }
                    else {
                            const message = new MessageWindow();
                            message.ShowErrorIcon = true;
                            message.Width = 400;
                            let text : string = TextCodeTranslator.Translate(TextCode.TaxReportOCancelLaterReports);
                            message.Show(text + '\n');
                    }
                } 
            }, (error) => {
                new MessageWindow().Show(error || 'Something wrong happened!');
            });
    }

    CancelTaxReportInner() {
        var confirmWindow = new ConfirmWindow();
        var msg = TextCodeTranslator.Translate("Accounting.General.O.WantToCancelTaxReport");
        confirmWindow.Show(msg);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.StatusCode = TaxReportStatus.CancelationInProgress;
                this.entityArgs.EditComponent.SaveChanges();
                this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.entityArgs.EditComponent.ReloadEntityPM();
                        this.CancelTaxReportInBatch();
                    } else {
                        this.EntityPM.IsCancelled = false;
                    }
                });
            }
        });
    }


    CancelClosingJournal() {
        this.CurrentSession.StartBusyIndicatorCreating();

        this.taxReportExtendedPMService.CancelClosingJournal(this.EntityPM.Id)
            .subscribe((response: ServiceResponse) => {
                this.StopBusyIndicator();
                if (response.HasError) {
                    const message = new MessageWindow();
                    message.ShowErrorIcon = true;
                    message.Width = 400;
                    message.Show(response.ErrorsArray.join('\n'));
                } else {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            }, (error) => {
                new MessageWindow().Show(error || 'Somthing wrong happend!');
            });
    }
}

enum MenuButton {

    Upload = "UPLD",
    ReturnToDraft = "RTDR",
    ClosingJournal = 'TRCJ',
    CancelClosingJournal = 'TRCCJ'

}

enum TaxReportStatus {

    Darft = "D",
    Transmitted = "T",
    TransmittedAndClosingJournal = "J",
    Error = "E",
    CancelationInProgress = "CP",
    CancelationFailed = "CF"
}

enum TextCode {
    TaxReportCantBeClosedValidationMessage = "TaxReport.O.ClosingJournalValidationMessage",
    TaxReportClosingJournalConfirmationMessage = "TaxReport.O.ClosingJournalConfirmationMessage",
    TaxReportCloseJournalNotSupported = "TaxReport.O.CloseJournalNotSupported",
    TaxReportCloseJournalRunInBackground = "TaxReport.O.CloseJournalRunInBackground",
    TaxReportOCancelLaterReports = "TaxReport.O.CancelLaterReports",
}
function CloneDeep(EntityPM: TaxReportPM) {
    throw new Error('Function not implemented.');
}

