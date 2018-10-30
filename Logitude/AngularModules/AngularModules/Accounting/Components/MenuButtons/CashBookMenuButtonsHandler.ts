declare var window: any;
import {CashBookPM} from '../../EntityPMs/CashBookPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CashBookPMService} from '../../Services/StandardPMs/CashBookPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

export class CashBookMenuButtonsHandler {
    public EntityPM: CashBookPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "CashBook"
    TotalSum: number = 0;
    public isRTL: boolean = false;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;

        if(ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'CashBook')[0];
                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "CashBookInactive":
                            {
                                if (this.EntityPM.Inactive == true) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "CashBookDeposite":
                            {
                                if (this.EntityPM.Inactive == true) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                    }
                }
            }
        }

        return menuButtons;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        var errors = [];

        if (errors.length == 0) {
            switch (menuButton.EventCode) {

                case "CashBookInactive":
                    {
                        this.CalculateTotals();
                        //if (this.TotalSum != 0) {
                        if (this.EntityPM.TotalAmount != 0) {
                            this.entityArgs.EditComponent.ValidationErrorsList = [];
                            this.entityArgs.EditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.BalanceOfCashbookUnequalZeroCantBlocked"));//"The balance of the cashbook is unequal to zero, can’t be blocked");
                        } else {
                            this.EntityPM.Inactive = true;
                            this.entityArgs.EditComponent.SaveChanges();
                        }
                        break;
                    }
                case "CashBookDeposite":
                    {
                        this.RunNewDepositWizard();
                        break;
                    }

            }
        } else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    }

    CalculateTotals() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CashBookLines)) {
            for (let line of this.EntityPM.CashBookLines) {
                this.TotalSum += line.LocalAmount;
            }
        }
    }

    RunNewDepositWizard() {

        if (this.EntityPM.CashBookTypeCode == "2") { // 2-Cheque
            var exist = this.EntityPM.CashBookLines.find(d => d.IsDeposited == false);
            if (!exist) {
                var msg = new MessageWindow();
                msg.Width = 350;
                msg.RTL = this.isRTL;
                msg.Show(TextCodeTranslator.Translate("Accounting.O.NoChequesInCashbook"));//"There are no Cheques in the Cashbook");

                return;
            }
        } else if (this.EntityPM.CashBookTypeCode == "1") { // 1-Cash
            if (AppTool.IsNullOrZero(this.EntityPM.TotalAmount)){
                var msg = new MessageWindow();
                msg.Width = 350;
                msg.RTL = this.isRTL;
                msg.Show(TextCodeTranslator.Translate("Accounting.O.NoCashInCashbook"));//"There are no Cash in the Cashbook");

                return;
            }
        }

        var windowTitle = "New Deposit";
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewDeposit");

        var windowArgs = new Args;
        windowArgs.CashBookId = this.EntityPM.Id;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 520;
        logWindow.Height = 240;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
        logWindow.Show('./Accounting/Components/NewEntity/NewBankDepositComponent');
    }


    private StartBusyIndicator(message: string) {
        SessionLocator.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        SessionLocator.CurrentSession.StopBusyIndicator();
    }
}

export class Args {
    CashBookId: string;
}
