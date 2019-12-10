declare var window: any;
import {BankAccountPM} from '../../EntityPMs/BankAccountPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BankAccountPMService} from '../../Services/StandardPMs/BankAccountPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {LedgerTransactionExtendedListService} from '../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import {CurrencyPMService} from '../../../Common/Services/StandardPMs/CurrencyPMService';

export class BankAccountMenuButtonsHandler {
    public EntityPM: BankAccountPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "BankAccount"
    TotalSum: number = 0;
    private CurrentSession = SessionLocator.SelectedSession;
    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    _CurrencyPMService: CurrencyPMService = new CurrencyPMService();


    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'BankAccount')[0];

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "BankAccountReconcile":
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

                case "BankAccountReconcile":
                    {
                        this.CurrentSession.StartBusyIndicatorLoading();
                        if (!this.EntityPM.GLAccountCurrencyId || this.EntityPM.GLAccountCurrencyId == "multi")
                        {
                            this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.GLAccountId).subscribe((serviceResponse: ServiceResponse) => {
                                if (serviceResponse.Result) {
                                    var result = serviceResponse.Result;
                                    var transaction = result.Result; // get the data
                                    var openAmountCurrency = transaction ? transaction.OpenAmountCurrencySign : "";
                                    this.showReconcileWindow(openAmountCurrency);

                                }
                                this.CurrentSession.StopBusyIndicator();
                            });
                        }
                        else
                        {
                            this._CurrencyPMService.get(this.EntityPM.GLAccountCurrencyId).subscribe((myResult) => {
                                var currency = myResult.Result;
                                var openAmountCurrency = currency ? currency.Sign : "";
                                this.showReconcileWindow(openAmountCurrency);
                                this.CurrentSession.StopBusyIndicator();
                            });
                        }

                        break;
                    }

            }
        } else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    }

    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }
    getScreenWidth() {
        if (self.innerWidth) {
            return self.innerWidth;
        }

        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }

        if (document.body) {
            return document.body.clientWidth;
        }
    }

    showReconcileWindow(currency: any) {


        var screenWidth = this.getScreenWidth();
        var screenHeight = this.getScreenHeight();

        var windowArgs: any = {};
        windowArgs.BankAccountPM = this.EntityPM;
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.openAmountCurrency = currency; // CurrencySign
        windowArgs.ObjectTableName = "BankAccount";

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = (screenWidth > 1024) ? (screenWidth > 1200 ? 1500 : screenWidth - 20) : 900;
        logitudeWindow.Height = (screenHeight > 768) ? (screenHeight > 850 ? 700 : screenHeight - 70) : screenHeight - 70;

        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.ExternalReconcile");

        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Accounting/Components/Others/ExternalReconcileComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            // show alert
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }

}
