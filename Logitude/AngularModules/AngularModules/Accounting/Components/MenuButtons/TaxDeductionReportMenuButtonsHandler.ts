
declare var window: any;
import { TaxDeductionReportPM } from '../../EntityPMs/TaxDeductionReportPM';
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
//import { TaxDeductionReportExtendedPMService } from '../../Services/ExtendedPMs/TaxReportExtendedPMService';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';

export class TaxDeductionReportMenuButtonsHandler {

    public EntityPM: TaxDeductionReportPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "TaxDeductionReport"

   // _TaxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'TaxDeductionReport')[0];


                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    switch (button.EventCode) {
                        case "TDMR":
                            {
                               
                                    button.IsDisabled = false;
                                
                                break;
                            }
                    }
                }
            }
        }

        return menuButtons;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        switch (menuButton.EventCode) {
            case "DNBD": 
                {
                  
                 

                    break;
                }
            case "DNTX":
                {
                    var windowTitle = TextCodeTranslator.Translate("TaxReport.B.Download");

                    var windowArgs: any = {};
                    windowArgs.ObjectTableName = "TaxDeductionReport";
                    windowArgs.EntityPM = this.EntityPM;
                    windowArgs.StartDirectly = false; // start service after show window
                    windowArgs.TimerInterval = 1000; // wait time between requests

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 350;
                    logWindow.Height = 150;
                    logWindow.Title = windowTitle;
                    logWindow.ShowCloseButton = true;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(($event: any) => {
                        this.entityArgs.EditComponent.ReloadEntityPM();
                    });
                    logWindow.Show('./Accounting/Components/Others/AccountingFlatFileDownloadComponent');


                    break;
                }

        }



    }

    private StartBusyIndicator(message: string) {
        SessionLocator.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        SessionLocator.CurrentSession.StopBusyIndicator();
    }

}
