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


export class TaxReportMenuButtonsHandler {
    public EntityPM: TaxReportPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "TaxReport"
    taxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();


    private CurrentSession = SessionLocator.SelectedSession;

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
                                if (this.EntityPM.IsCancelled) {
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
                            button.IsDisabled = this.EntityPM.StatusCode == TaxReportStatus.Transmitted;
                            break;
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
                else{
                    button.IsDisabled = true;
                }
                }
        });
    }
    private SetUploadButtonEnabilityAccordingToConsolidationVAT(button: MenuButtonPM) {
        if (this.EntityPM.StatusCode == "D" || this.EntityPM.StatusCode=="E") {
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

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
    CreateClosingJournalButtonClicked(){
        this.taxReportExtendedPMService.CheckIfTaxReportCanHaveClosingJournal(this.EntityPM.Id)
            .subscribe((response: ServiceResponse) =>
            {
                let canHaveClosingJournal: boolean = response.Result;
                if (canHaveClosingJournal) {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Show(TextCodeTranslator.Translate(TextCode.TaxReportClosingJournalConfirmationMessage))
                    confirmWindow.Width = 400;
                    confirmWindow.WindowClosed.subscribe(event =>
                    {
                        if (confirmWindow.Yes) {
                            this.CreateClosingJournal();
                        }
                    });
                } else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Show(TextCodeTranslator.Translate(TextCode.TaxReportCantBeClosedValidationMessage));
                }
            });
    }
    CreateClosingJournal(){
        this.CurrentSession.StartBusyIndicatorCreating();

        this.taxReportExtendedPMService.CloseTaxReport(this.EntityPM.Id)
            .subscribe((response: ServiceResponse) =>
            {
                this.StopBusyIndicator();
                if(response.HasError){
                    const message = new MessageWindow();
                    message.ShowErrorIcon = true;
                    message.Width = 400;
                    message.Show(response.ErrorsArray.join('\n'));
                }else{
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            }, (error) =>
            {
                new MessageWindow().Show(error || 'Somthing wrong happend!');
            });
    }
}

enum MenuButton {

    Upload = "UPLD",
    ReturnToDraft ="RTDR",
    ClosingJournal = 'TRCJ'

}

enum TaxReportStatus {

    Darft = "D",
    Transmitted = "T",
    Error = "E",
    CancelationInProgress = "CP",
    CancelationFailed = "CF"
}

enum TextCode {
    TaxReportCantBeClosedValidationMessage = "TaxReport.O.ClosingJournalValidationMessage",
    TaxReportClosingJournalConfirmationMessage = "TaxReport.O.ClosingJournalConfirmationMessage"
}
