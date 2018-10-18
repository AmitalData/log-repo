declare var window: any;
import {BankDepositPM} from '../../EntityPMs/BankDepositPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {BankDepositPMService} from '../../Services/StandardPMs/BankDepositPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
//import {BankDepositValidator} from '../../Validators/BankDepositValidator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {DocumentTypePM} from '../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypePMExtendedService} from '../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {ExportDocumentService} from '../../../Common/Services/DocumentServices/ExportDocumentService';
import {DocumentOutPMService} from '../../../Common/Services/ExtendedPMs/DocumentOutPMService';
import { DocumentOutPM } from '../../../Common/EntityPMs/DocumentOutPM';
import { DocumentsPrintHelper } from '../../Utilities/DocumentsPrintHelper';
import {GeneralPrintHelper} from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

export class BankDepositMenuButtonsHandler {
    public EntityPM: BankDepositPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "BankDeposit"
    private _documentOutPMService: DocumentOutPMService = new DocumentOutPMService();
    private _documentTypePMService: DocumentTypePMExtendedService = new DocumentTypePMExtendedService();
    private _exportDocumentService: ExportDocumentService = new ExportDocumentService();


    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'BankDeposit')[0];
                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    switch (button.EventCode) {
                        case "BankDepositApprove":
                            {
                                if (this.EntityPM.Id ) {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "BankDepositPrint":
                            {
                                if (!this.EntityPM.Id)
                                    button.IsDisabled = true;
                                else
                                    button.IsDisabled = false;
                                break;
                            }
                        case "CancelDeposit":
                            {
                                if (!this.EntityPM.Id)
                                    button.IsDisabled = true;
                                else
                                    button.IsDisabled = false;


                                if (this.EntityPM.IsCanceled)
                                    button.IsDisabled = true;

                                break;
                            }
                    }
                }
            }
        }

        return menuButtons;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        if (!this.EntityPM.CreateDate) {
            // will override in server, its required even on client!!
            this.EntityPM.CreateDate = new Date(); 
            this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId; 
            this.EntityPM.UpdateDate = new Date();
            this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        }



        switch (menuButton.EventCode) {
            
            case "BankDepositApprove":
                {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];


                    if (this.EntityPM.IsCashDeposit)
                    {
                        if (this.EntityPM.LocalDepositAmount == 0)
                        {
                            var msg = TextCodeTranslator.Translate("Accounting.General.O.ZeroDeposit");
                            this.entityArgs.EditComponent.ValidationErrorsList = [];
                            this.entityArgs.EditComponent.ValidationErrorsList.push(msg);
                            return;
                        }
                    }
                    else
                    {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.BankDepositLines)) {
                            this.entityArgs.EditComponent.ValidationErrorsList = [];
                            this.entityArgs.EditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.selectAtLeast1Linetodeposit"));
                            return;
                        } else {
                            if (this.EntityPM.BankDepositLines.length == 0) {
                                this.entityArgs.EditComponent.ValidationErrorsList = [];
                                this.entityArgs.EditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.selectAtLeast1Linetodeposit"));
                                return;
                            }
                        }

                    }
                    break;
                }
            case "CancelDeposit":
                {
                    this.EntityPM.IsCanceled = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {

                        } else {
                            this.EntityPM.IsCanceled = false;
                        }
                    });
                    return;
                }

            case "BankDepositPrint":
                {
                    this.PrintDeposit();
                    return; // no save on print
                }

        }

        if (this.entityArgs.EditComponent.ValidationErrorsList.length == 0)
        {

            this.entityArgs.EditComponent.SaveChanges();
        }
  
    }

    private StartBusyIndicator(message: string) {
        SessionLocator.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        SessionLocator.CurrentSession.StopBusyIndicator();
    }

    private PrintDeposit() {
        var myPrintHelper = new GeneralPrintHelper(this.ObjectTableName, "BDPR", this.EntityPM.Id, null, this.EntityPM.BankAccountNumber, null);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, "BankDepositPrint");
            myPrintHelper.ShowPrintControl();
        }
    }
}