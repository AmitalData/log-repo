declare var window: any;
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {GLAccountPMService} from '../../Services/StandardPMs/GLAccountPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityPMService} from '../../../Infrastructure/Services/EntityPMService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountList} from '../../EntityLists/GLAccountList';
import {GLAccountListService} from '../../Services/StandardLists/GLAccountListService';
import {GLAccountExtendedListService} from '../../Services/ExtendedLists/GLAccountExtendedListService';
import {LedgerTransactionExtendedListService} from '../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import {ReconcileEventManager} from '../../Utilities/ReconcileEventManager';

export class GLAccountMenuButtonsHandler {
    public EntityPM: GLAccountPM;
    public entityArgs: EntityArgs
    public TenantPM: TenantPM;
    public ObjectTableName: string = "GLAccount"
    TotalSum: number = 0;
    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    private glAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    public SetEntityPM(entityArgs: EntityArgs) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;


        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private TabSelectedEvent: any = null;
    Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }


        }
    }



    public CheckButtonState(menuButtons: MenuButtonPM[]) {


        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'GLAccount')[0];
                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "GLAccountInactive":
                            {
                                if (this.EntityPM.Inactive == true) {
                                    button.IsDisabled = true;
                                } else if (this.EntityPM.AccountTypeCode == '4' || this.EntityPM.AccountTypeCode == '5') { // Job / File
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }

                        case "GLAccountPrintCardIndex":
                            {
//                                if (this.EntityPM.Inactive == true) {
//                                    button.IsDisabled = true;
//                               }

//                                else {
                                    button.IsDisabled = false;
//                                }
                                    break;
                            }

                        case "Reconcile":
                            {
                                button.DisplayText = TextCodeTranslator.Translate("GLAccount.B.Reconcile") + " (" + this.EntityPM.ReconcilationCount + ")";


                                       if (this.EntityPM.IsControlAccount == true || this.EntityPM.ReconcilationCount==0 ) {
                                           button.IsDisabled = true;



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

                case "GLAccountInactive":
                    {
                        var myGLAccountListService: GLAccountListService = new GLAccountListService();
                        myGLAccountListService.getSingle(this.EntityPM.Id)
                            .subscribe((myResponse: ServiceResponse) => {
                                var myGLAccountList: GLAccountList = myResponse.Result as GLAccountList;

                                if (!myGLAccountList.BalanceInLocalCurrency || myGLAccountList.BalanceInLocalCurrency == 0) {
                                    this.EntityPM.Inactive = true;
                                    this.EntityPM.ActiveStatusName = TextCodeTranslator.Translate("GLAccounts.Q.Inactive");
                                    this.entityArgs.EditComponent.SaveChanges();
                                } else {
                                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                                    this.entityArgs.EditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.GLABalanceNotEqual0"));
                                }
                            });
                        break;
                    }
                case "GLAccountPrintCardIndex":
                    {
                        this.entityArgs.EditComponent.SaveChanges();
                        // Here to start the report filter screen
                        break;
                    }
                case "Reconcile":
                    {
                        this.ReconcileButtonClicked();

                        break;
                    }
            }
        } else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    }

    private StartBusyIndicator(message: string) {
        SessionLocator.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        SessionLocator.CurrentSession.StopBusyIndicator();
    }

    ReconcileButtonClicked() {
        var screenWidth = this.getScreenWidth();
        var screenHeight = this.getScreenHeight();
        this._LedgerTransactionExtendedListService.GetFirstLedgerTransaction(this.EntityPM.Id).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.Result) {
                var result = serviceResponse.Result;
                var transaction = result.Result; // get the data
                var openAmountCurrency = transaction.OpenAmountCurrencySign;

                // original amount currency
                var originalAmountCurrency;
                if (this.EntityPM.ReconcileMethodCode == "0") originalAmountCurrency = SessionLocator.TenantPM.CurrencySign;
                else if (this.EntityPM.ReconcileMethodCode == "1") originalAmountCurrency = transaction.CurrencySign;


                var windowArgs: any = {};
                windowArgs.GLAccountPM = this.EntityPM;
                windowArgs.openAmountCurrency = openAmountCurrency;
                windowArgs.originalAmountCurrency = originalAmountCurrency;
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Width = (screenWidth > 1024) ? (screenWidth > 1200 ? 1500 : screenWidth - 20) : 900;
                logitudeWindow.Height = (screenHeight > 768) ? (screenHeight > 800 ? 700 : screenHeight - 70) : screenHeight - 70;

                logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.General.O.Reconcile"); //"Reconcile";

                logitudeWindow.WindowArgs = windowArgs;
                logitudeWindow.Show('./Accounting/Components/Others/ReconcileComponent');
                logitudeWindow.WindowClosed.subscribe(($event: any) => {
                    if ($event == 'ok') {
                        // show alert
                    }
                    this.GetNonReconciledTransactionsCount();


                });

            }
        });
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


    GetNonReconciledTransactionsCount() {
        this.glAccountExtendedListService.GetAccountReconcilesCount(this.EntityPM.Id).subscribe(myResult => {


            if (!AppTool.IsNullOrEmpty(myResult)) {

                this.EntityPM.ReconcilationCount = myResult;

                this.SaveChenges();
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


}
