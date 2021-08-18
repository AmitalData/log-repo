declare var window: any;
import { TaxReportPM } from '../../EntityPMs/TaxReportPM';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { TaxReportPMService } from '../../Services/StandardPMs/TaxReportPMService';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { AppTool } from '../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityPMService } from '../../../Infrastructure/Services/EntityPMService';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TaxReportExtendedPMService } from '../../Services/ExtendedPMs/TaxReportExtendedPMService';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';
import { FullAccountingSettingListService } from '../../Services/StandardLists/FullAccountingSettingListService';


export class TaxReportMenuButtonsHandler {
    public EntityPM: TaxReportPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "TaxReport"
    private fullAccountingSettingListService: FullAccountingSettingListService;
    _TaxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
    private CurrentSession = SessionLocator.SelectedSession;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.fullAccountingSettingListService = new FullAccountingSettingListService();
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
                    }
                }
            }
        }

        return menuButtons;
    }
    private SetUploadButtonEnabilityAccordingToConsolidationVAT(button: MenuButtonPM) {
        this.fullAccountingSettingListService.getSingle(this.TenantPM.Id.toString()).subscribe((res: any) => {
            this.CurrentSession.StopBusyIndicator();         
                if (res != null) {
                    var res = res.Result;
                    if (res.ConsolidationVAT == this.TenantPM.VatNumber && (this.EntityPM.StatusCode == "D" || this.EntityPM.StatusCode=="E")) {
                        button.IsDisabled = false;
                    }
                    else {
                        button.IsDisabled = true;
                    }
                }
        });
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
                            this.EntityPM.IsCancelled = true;
                            this.entityArgs.EditComponent.SaveChanges();
                            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                if (isSaveSuccess) {
                                    this.entityArgs.EditComponent.ReloadEntityPM();
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

            case "UPLD": // upload 
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
                    logitudeWindow.Show('./Accounting/Components/Others/TaxReportUploadLinesComponent');
                    //});
                    break;
                }
        }



    }

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
}
