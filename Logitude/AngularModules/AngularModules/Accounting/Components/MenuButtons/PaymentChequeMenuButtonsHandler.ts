import {PaymentChequePM} from '../../EntityPMs/PaymentChequePM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
declare var window: any;
import {GLAccountPMService} from '../../Services/StandardPMs/GLAccountPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {BankAccountPMService} from '../../Services/StandardPMs/BankAccountPMService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { DocumentsPrintHelper } from '../../Utilities/DocumentsPrintHelper';

import {Validator} from '../../../Infrastructure/Validators/Validator';
import {GeneralPrintHelper} from '../../../Infrastructure/Helpers/GeneralPrintHelper';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';




export class PaymentChequeMenuButtonsHandler
{


    public EntityPM: PaymentChequePM;
    public entityArgs: EntityArgs
    public ObjectTableName: string = "PaymentCheque"
    gLAccountPMService: GLAccountPMService = new GLAccountPMService();
   FIELD_IS_REQUIERD :string = TextCodeTranslator.Translate("General.M.FieldIsRequired");

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        
    }
    bankAccountPMService: BankAccountPMService = new BankAccountPMService();
    private CurrentSession = SessionLocator.SelectedSession;

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'PaymentCheque')[0];
                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    switch (button.EventCode) {
                        case "SaveAsDraft":
                            {
                                if (this.EntityPM.PaymentChequeStatusCode == "2" || this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else { button.IsDisabled = false; }
                             
                                break;
                            }
                        case "Approve":
                            {
                              //  button.Width = 120;
                                if (this.EntityPM.PaymentChequeStatusCode == "2" || this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }
                                else { button.IsDisabled = false; }
                                                             break;
                            }
                        case "More":
                            {
                           //     button.Width = 500;
                               
                                break;
                            }
                        case "CancelCheque":
                            {
                                if (this.EntityPM.PaymentChequeStatusCode == "3" || this.EntityPM.IsCancelled ) {
                                    button.IsDisabled = true;
                                }
                                else
                                { button.IsDisabled = false; }
          
                                break;
                            }
                        case "PrintCheque":
                            {
                                if (this.EntityPM.PaymentChequeStatusCode != "2") { button.IsDisabled = true; }
                                else { button.IsDisabled = false; }
                                break;
                            }
                    }
                }
            }
        }

        return menuButtons;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

       
        var errors: string[] = [];
        switch (menuButton.EventCode) {

               
            case "SaveAsDraft":
                {
                    this.EntityPM.PaymentChequeStatusCode = "1";
                    this.CheckCurrency();
                    Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
                    for (let item of this.EntityPM.PaymentChequeLines) {

                        if (AppTool.IsNullOrEmpty(item.Amount)) {
                            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("PaymentChequeLine.F.Amount"));

                            errors.push(s + " " + TextCodeTranslator.Translate("Accounting.General.O.Line") + " " + item.SequenceNumeric);
                        }
                        if (AppTool.IsNullOrEmpty(item.Notes)) {
                            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("PaymentChequeLine.F.Notes"));

                            errors.push(s + " " + TextCodeTranslator.Translate("Accounting.General.O.Line") + " " + item.SequenceNumeric);
                                    }
                    }
                    this.entityArgs.EditComponent.ValidationErrorsList = errors;
                    this.SaveChanges();

                    break;
                }


            case "Approve":
                {
                   
                    Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
                    if (this.EntityPM.CurrencyId != this.EntityPM.BankGLAccountCurrencyId) {
                        errors.push(TextCodeTranslator.Translate("Accounting.General.O.BankAccountDifferentCurrencies"));
                    }
                    var totalAmount: number = 0;
                    for (let item of this.EntityPM.PaymentChequeLines) {

                        if (AppTool.IsNullOrEmpty(item.Amount)) {
                            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("PaymentChequeLine.F.Amount"));

                            errors.push(s + " " + TextCodeTranslator.Translate("Accounting.General.O.Line") + " " + item.SequenceNumeric);
                        }
                        if (AppTool.IsNullOrEmpty(item.Notes)) {
                            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("PaymentChequeLine.F.Notes"));

                            errors.push(s + " " + TextCodeTranslator.Translate("Accounting.General.O.Line") + " " + item.SequenceNumeric);
                        }
                        totalAmount = totalAmount + item.Amount;
                    }
                    if (totalAmount != this.EntityPM.LocalAmount) {
                        errors.push(TextCodeTranslator.Translate("Accounting.General.O.DifferentAmounts"));
                    }
                    this.entityArgs.EditComponent.ValidationErrorsList = errors;
                    if (this.entityArgs.EditComponent.ValidationErrorsList.length == 0) {
                        this.bankAccountPMService.get(this.EntityPM.BankAccountId).subscribe((res) => {
                            if (res) {
                                if (res.Result) {
                                    if (res.Result.ChequeCounter == null) {
                                        errors.push(TextCodeTranslator.Translate("Accounting.General.O.NoChequeCounter")
                                        );
                                        this.entityArgs.EditComponent.ValidationErrorsList = errors;


                                    }
                                    else {
                                        //   this.EntityPM.ChequeNumber = res.Result.ChequeCounter;
                                        this.EntityPM.PaymentChequeStatusCode = "2";
                                        this.EntityPM.ApproveDate = new Date();
                                        this.EntityPM.ApprovedByUserId = SessionLocator.LoggedUserId;
                                        //this.SaveChanges();
                                        this.entityArgs.EditComponent.SaveChanges();
                                        this.entityArgs.EditComponent.SaveCompleted.subscribe(($event) => {
                                            if ($event == true) {
                                                this.CurrentSession.DisableFieldsEvent.emit({});
                                                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();


                                            }
                                        });
                                    }

                                }
                            }
                        });
                    }
                    break;
                }

            case "CancelCheque": {
                var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.CancellationReason");

                var windowArgs: any = {};
                windowArgs.PaymentChequePM = this.EntityPM;

                var logWindow = new LogitudeWindow();
                logWindow.Width = 520;
                logWindow.Height = 200;
                logWindow.Title = windowTitle;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(($event: any) => this.ReloadEntityPM($event));
                logWindow.Show('./Accounting/Components/Others/CancelChequeComponent');
                break;
            }

            case "PrintCheque": {
                this.PrintPaymentCheque();
                break;
            }
        }

       

    }

    ReloadEntityPM(key: string) {
        if (key == "ok") {
            this.CurrentSession.DisableFieldsEvent.emit({});

            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
    }
    SaveChanges() {
        if (this.entityArgs.EditComponent.ValidationErrorsList.length == 0) {

            this.entityArgs.EditComponent.SaveChanges();

        }}
    CheckCurrency() {

        if (!this.EntityPM.IsGLAccountMultiCurrency) {
            if (this.EntityPM.CurrencyId != null) {
                if (this.EntityPM.GLAccountCurrencyId != this.EntityPM.CurrencyId) {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                    this.entityArgs.EditComponent.ValidationErrorsList.push("the payment currency does not match to the bill to GLAccount Currency!");

                } else {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                }
            }
        }

        if (!this.EntityPM.IsBankGlAccountMultiCur) {
            if (this.EntityPM.CurrencyId != null) {
                if (this.EntityPM.BankGLAccountCurrencyId != this.EntityPM.CurrencyId) {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                    this.entityArgs.EditComponent.ValidationErrorsList.push("the payment currency does not match to the bank  GLAccount Currency!");

                } else {
                    this.entityArgs.EditComponent.ValidationErrorsList = [];
                }
            }
        }



    }

    private PrintPaymentCheque() {
        var myPrintHelper = new GeneralPrintHelper("PaymentCheque", "PCDR", this.EntityPM.Id, null, this.EntityPM.ChequeNumber, null);
        if (myPrintHelper.IsLoadPrintControl) {
            ServiceLocator.SendTotangoUserActivity("PaymentCheque", "PrintCheque");
            myPrintHelper.ShowPrintControl();
        }


        //var printService = new DocumentsPrintHelper(this.ObjectTableName, this.EntityPM.Id, this.EntityPM.Tenant);
        //printService.BuildAndPrintDocument("PCDR");
    }

}
