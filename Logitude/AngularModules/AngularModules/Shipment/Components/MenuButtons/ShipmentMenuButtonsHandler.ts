declare var window: any;
import {OnDestroy} from '@angular/core';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {NewShipmentComponentArgs} from '../../Args';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {EntityWarningsValidator} from '../../../Infrastructure/Validators/EntityWarningsValidator';
import {RulesValidator} from '../../../Infrastructure/Validators/RulesValidator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ShipmentDomainService} from '../../Services/ShipmentDomainService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MenuButtonsTemplateArgs} from './MenuButtonsTemplateComponent';
import {ShipmentTool} from '../../Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentValidator} from '../../Validators/ShipmentValidator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTableRuleFieldPM} from '../../../Infrastructure/EntityPMs/ObjectTableRuleFieldPM';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';

export class ShipmentMenuButtonsHandler implements OnDestroy {
    public EntityPM: ShipmentPM;
    public entityArgs: EntityArgs
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Shipment')[0];

                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {

                    var button = menuButtons[i];

                    if (button.EventCode == "ShowAWB") {
                        button.IsDisabled = buttonEnabled ? (this.EntityPM.TransportModeId != "A") : true;
                    }
                    if (button.EventCode == "CopyShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "C") {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "OperationalCloseShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "AccountingCloseShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsAccountingClosed || !this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "OperationalReopenShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsOperationalClosed || this.EntityPM.IsAccountingClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "AccountedReopenShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsAccountingClosed || this.EntityPM.IsCancelled || this.EntityPM.ShipmentLevelCode == "H") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "CancelShipment") {
                        if (buttonEnabled) {
                            if (this.EntityPM.IsAccountingClosed || this.EntityPM.IsOperationalClosed || this.EntityPM.IsCancelled || (this.EntityPM.MasterShipmentDataId != null && this.EntityPM.MasterShipmentDataId != this.EntityPM.Id)) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ReactivateShipment") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.IsCancelled) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ExceptionResolved") {
                        if (buttonEnabled) {
                            if (!this.EntityPM.HasException) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentFromHouseToDirect") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "H" && this.EntityPM.MasterShipmentDataId == null) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentFromDirectToHouse") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode == "D" && !this.EntityPM.IsOperationalClosed) {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "SendRequest") {
                        if (buttonEnabled) {
                            if (this.EntityPM.TransportModeId == "A" && this.EntityPM.DirectionId == "E") {
                                button.IsDisabled = false;
                            }
                            else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "SendResponse") {
                        if (buttonEnabled) {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "ConvertToCustomFile") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentLevelCode != "D") {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }
                    if (button.EventCode == "SplitShipment") {

                        if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                            button.IsHidden = false;

                            button.IsDisabled = !buttonEnabled;
                        }

                        else {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentToLCL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentTypeId == "FCLD") {
                                button.IsHidden = false;
                            }
                            else {
                                button.IsHidden = true;
                            }
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "ConvertShipmentToFCL") {
                        if (buttonEnabled) {
                            if (this.EntityPM.ShipmentTypeId == "LCLD") {
                                button.IsHidden = false;
                            }
                            else {
                                button.IsHidden = true;
                            }
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }
                }

                return menuButtons;
            }
        }
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {

        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            this.Validate();

            if (this.isValid) {
                switch (this.MenuButtonCode) {
                    case "ShowEvents": {
                        this.ShowEvents();
                        break;
                    }
                    case "CopyShipment": {
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                    case "OperationalCloseShipment": {
                        this.OperationalCloseShipment();
                        break;
                    }
                    case "OperationalReopenShipment": {
                        this.OperationalReopenShipment();
                        break;
                    }
                    case "AccountingCloseShipment": {
                        this.AccountingCloseShipment();
                        break;
                    }
                    case "AccountedReopenShipment": {
                        this.AccountedReopenShipment();
                        break;
                    }
                    case "CancelShipment": {
                        this.CancelShipment();
                        break;
                    }
                    case "ReactivateShipment": {
                        this.ReactivateShipment();
                        break;
                    }
                    case "ExceptionResolved": {
                        this.ExceptionResolvedShipment();
                        break;
                    }
                    case "ConvertShipmentFromHouseToDirect": {
                        this.ConvertShipmentFromHouseToDirect();
                        break;
                    }
                    case "ConvertShipmentFromDirectToHouse": {
                        this.ConvertShipmentFromDirectToHouse();
                        break;
                    }
                    case "SendRequest": {
                        this.StopFlags();
                        break;
                    }
                    case "SendResponse": {
                        this.StopFlags();
                        break;
                    }
                    case "ConvertToCustomFile": {
                        this.ConvertShipmentToCustomFile();
                        break;
                    }
                    case "SplitShipment": {
                        this.SplitShipmentClicked();
                        break;
                    }
                    case "ConvertShipmentToLCL": {
                        this.ConvertShipmentToLCLClicked();
                        break;
                    }
                    case "ConvertShipmentToFCL": {
                        this.ConvertShipmentToFCLClicked();
                        break;
                    }

                    default: {
                        this.isButtonClicked = false;
                        break;
                    }
                }
            }
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                        switch (this.MenuButtonCode) {
                            case "CopyShipment": {
                                this.CopyShipment();
                                break;
                            }

                            case "SplitShipment": {
                                this.SplitShipmentApply();
                                break;
                            }
                        }

                        if (this.IsConvertToLCLClicked) {
                            this.DoConvertShipmentType("ToLCL");
                        }

                        if (this.IsConvertToFCLClicked) {
                            this.DoConvertShipmentType("ToFCL");
                        }

                        if (this.Reload) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    }

                    this.StopFlags();
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                        SessionLocator.CurrentSession.SessionEvent.emit("ReloadHouses");
                    }

                    this.StopFlags();
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    Reload: boolean = false;
    StopFlags() {
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.Reload = false;
        this.IsConvertToLCLClicked = false;
        this.IsConvertToFCLClicked = false;
    }
    Validate() {
        var validator = new ShipmentValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;

        this.entityArgs.EditComponent.ValidationErrorsList = errors;

        if (!this.isValid) {
            this.StopFlags();
        }
    }

    public ActionStepsStateList: Array<ActionsStepsState> = [];
    private shipmentService: ShipmentDomainService = new ShipmentDomainService();
    private warningsList: Array<ValidationErrorInfo>;
    public get WarningList() { if (this.warningsList == null) this.warningsList = new Array<ValidationErrorInfo>(); return this.warningsList; }
    public set WarningList(value: Array<ValidationErrorInfo>) { this.warningsList = value; }
    private hasWarnings: boolean = false;
    public get HasWarnings() { return this.hasWarnings; }
    public set HasWarnings(value: boolean) { this.hasWarnings = value; }

    private CopyShipment() {
        var args = new NewShipmentComponentArgs();
        args.IsCopyFromShipment = true;
        args.Shipment = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;

        if (this.EntityPM.ShipmentLevelCode == "C") {
            logWindow.Title = "Copy Master";
            logWindow.Show('./Shipment/Components/NewEntity/NewMasterComponent');
        }

        else {
            logWindow.Title = "Copy Shipment";
            logWindow.Show('./Shipment/Components/NewEntity/NewShipmentComponent');
        }

        logWindow.ComponentLoaded.subscribe(cmp => {
            this.StopFlags();

            cmp.MainCarriageFromPortId = this.EntityPM.MainCarriageFromPortId;
            cmp.MainCarriageToPortId = this.EntityPM.MainCarriageToPortId;
        });
    }
    private AccountedReopenShipment() {
        this.currentActionName = "AccountedReopen";
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var args = new MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = true;
        args.NotesHeader = "Shipment Accounting Reopen Notes";
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator.Translate("Shipment.M.ShipmentAccountingReopened");
        this.ActionStepsStateList.push(state);

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Accounted Shipment Reopen";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {
            cmp.ReopenDone.subscribe(p => {
                this.EntityPM.EventNote = p;
            });
        });
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ResetButtonClicked();
            if ($event == "confirm") {
                this.EntityPM.IsAccountingClosed = false;
                this.OkButton();

            }
            else {
            }

        });


    }
    private AccountingCloseShipment() {
        this.currentActionName = "AccountingClose";
        var hasOpenPayables: boolean = false;
        var hasOpenReceivables: boolean = false;
        if (this.EntityPM.ShipmentReceivables.length > 0) {
            this.EntityPM.ShipmentReceivables.forEach(p => {
                if (p.ShipmentReceivableLineStatusCode != "ACCT" && p.ShipmentReceivableLineStatusCode != "EMPT")
                    if (p.TotalAmount != null && p.TotalAmount != 0) hasOpenReceivables = true;

            });

        }
        if (!SessionLocator.AccountingSettingPM.AllowClosureWithoutPayables) {
            if (this.EntityPM.ShipmentPayables.length > 0)
                this.EntityPM.ShipmentPayables.forEach(p => {
                    if (p.ShipmentPayableLineStatusCode != "ACCT" && p.ShipmentPayableLineStatusCode != "EMPT")
                        if (p.ShipmentPayableAmountTypeCode == "NEXP") {
                            p.AccountedAmount != null && p.AccountedAmount != null ? hasOpenPayables = true : p.ExpectedAmount != null && p.ExpectedAmount != 0 ? hasOpenPayables = true : -1;

                        }
                        else {
                            if (p.ExpectedAmount != null && p.ExpectedAmount != 0) {
                                hasOpenPayables = true;
                            }
                        }
                });

        }

        if (this.EntityPM.ShipmentLevelCode == "C") {
            if (hasOpenPayables && hasOpenReceivables) {
                this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);

            }
            else {
                this.shipmentService.CheckHousesOpenAmounts(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (myResponse.Result != null && myResponse.Result != "") {
                            var Result: string = myResponse.Result;
                            if (Result.includes('R'))
                                hasOpenReceivables = true;
                            if (SessionLocator.AccountingSettingPM.AllowClosureWithoutPayables) {
                                if (Result.includes('P'))
                                    hasOpenPayables = true;
                            }
                        }
                    }
                });

                this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
            }
        }
        else {
            this.RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables);
        }
    }
    private RunAccountingCloseWindow(hasOpenPayables, hasOpenReceivables) {
        this.currentActionName = "OperationalReopen";
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var args = new MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = false;
        args.EventNote = null;
        var ErrorsList: Array<string> = [];
        var success: boolean = true;
        if (hasOpenPayables || hasOpenReceivables) {
            success = false;
            var error: string = "can’t close for accounting if there are any open payables/receivables";
            if (this.EntityPM.ShipmentLevelCode == "C") {
                error = "can’t close for accounting if there are any open payables/receivables in the Master or one \nof the connected shipments. Please check and fix this issue and try again";
            }
            ErrorsList.push(error);

        }

        else if (!this.EntityPM.IsOperationalClosed) {
            success = false;
        }
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator.Translate("Shipment.M.CheckingRequiredFields");
        success == true ? state.State = "Succeeded" : state.State = "Failed";
        this.ActionStepsStateList.push(state);

        var hasInvoice = null;
        hasInvoice = this.EntityPM.ShipmentARInvoices.filter(p => p.StatusCode != 'AC')[0];
        if (!hasInvoice) {
            state = new ActionsStepsState();
            state.Message = TextCodeTranslator.Translate("Shipment.M.DoesntContainInvoice");
            state.State = "Warning";
            this.ActionStepsStateList.push(state);

        }
        if (!success) args.EnabledOkButton = false;
        args.ValidationErrorsList = ErrorsList;
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Shipment Accounting Close";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ResetButtonClicked();
            if ($event == "confirm") {
                this.EntityPM.IsAccountingClosed = true;
                this.EntityPM.EventNote = null;
                this.EntityPM.AccountingCloseDate = DateTool.GetCurrentDateTimeAsUtc();
                this.OkButton();

            }
            else {
            }

        });






    }
    private OperationalReopenShipment() {
        this.currentActionName = "OperationalReopen";
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var args = new MenuButtonsTemplateArgs();
        args.IsNotesStackPanelVisible = true;
        args.NotesHeader = "Shipment Operational Reopen Notes";
        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator.Translate("Shipment.M.ShipmentOperationalReopened");
        this.ActionStepsStateList.push(state);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Shipment Operational Reopen";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {
            cmp.ReopenDone.subscribe(p => {
                this.EntityPM.EventNote = p;
                this.EntityPM.IsOperationalClosed = false;
                this.EntityPM.OperationalCloseDate = null;
                this.OkButton();
            });
        });

        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ResetButtonClicked();           
        });
    }
    private ShowEvents() {


    }
    private currentActionName: string = null;
    private ExceptionResolvedShipment() {
        this.currentActionName = "ExceptionResolved";

        var args = new MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Exception Resolved Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var state2: ActionsStepsState = new ActionsStepsState();
        state2.Message = TextCodeTranslator.Translate("Shipment.M.ExceptionResolved");
        this.ActionStepsStateList.push(state2);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = "Exception Resolved";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!AppTool.IsNullOrEmpty(notes))
                        this.EntityPM.EventNote = notes;
                    this.EntityPM.IsExceptionResolved = true;
                    this.EntityPM.HasException = false;
                    this.OkButton();
                }

                this.ResetButtonClicked();
            });
        });
    }
    private ConvertShipmentFromHouseToDirect() {
        this.currentActionName = "ConvertShipmentFromHouseToDirect";
        var args = new MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Convert Shipment From House To Direct...";
        args.IsNotesStackPanelVisible = false;
        args.EventNote = null;
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var state: ActionsStepsState = new ActionsStepsState();
        state.Message = TextCodeTranslator.Translate("Shipment.M.ShipmentConvertedHtoD");
        this.ActionStepsStateList.push(state);

        if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D" || this.EntityPM.DirectionId == "R") {
            var settingCode = "HAWBCounter" + this.EntityPM.TransportModeId + "_E_D";
            var tenantSettingPM = SessionLocator.TenantSettings.filter(p => p.SettingCode == settingCode)[0];
            if (tenantSettingPM != null) {
                if (tenantSettingPM.DontIncludeDirects) {
                    state = new ActionsStepsState();
                    state.Message = "The HAWB field will be removed";
                    state.State = "Warning";
                    this.ActionStepsStateList.push(state);
                }
            }
        }

        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 970;
        logWindow.Height = 570;
        logWindow.Title = "Convert Shipment From House To Direct";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    this.EntityPM.ConvertFromHouseToDirect = true;
                    this.EntityPM.ConvertFromDirectToHouse = false;
                    this.OkButton();
                }
                this.ResetButtonClicked();
            });
        });
    }
    private ConvertShipmentFromDirectToHouse() {
        if (this.EntityPM != null) {
            if (ShipmentTool.IsInlandDomestic(this.EntityPM)) {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Title = "Converting Shipment";
                messageWindow.Show("Converting inland domestic house to direct is not allowed");
            }

            else {
                this.currentActionName = "ConvertShipmentFromDirectToHouse";
                var args = new MenuButtonsTemplateArgs();
                args.ObjectTableName = "Shipment";
                args.EntityPM = this.EntityPM;
                args.NotesHeader = "Convert Shipment From Direct To House...";
                args.IsNotesStackPanelVisible = false;
                args.EventNote = null;
                this.ActionStepsStateList = new Array<ActionsStepsState>();
                var state: ActionsStepsState = new ActionsStepsState();
                state.Message = TextCodeTranslator.Translate("Shipment.M.ShipmentConvertedDToH");
                this.ActionStepsStateList.push(state);

                if (this.EntityPM.MainCarriageIsFromStack) {
                    state = new ActionsStepsState();
                    state.Message = TextCodeTranslator.Translate("Shipment.M.MasterAWBNumberTakenFromStack");
                    state.State = "Error";
                    this.ActionStepsStateList.push(state);
                    args.EnabledOkButton = false;
                }

                args.ActionStepsStateList = this.ActionStepsStateList;
                var logWindow = new LogitudeWindow();
                logWindow.WindowArgs = args;
                logWindow.Width = 935;
                logWindow.Height = 570;

                logWindow.Title = "Convert Shipment From Direct To House";
                logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
                logWindow.ComponentLoaded.subscribe(s => {
                    logWindow.WindowClosed.subscribe(d => {
                        if (d == "confirm") {
                            if (!this.EntityPM.MainCarriageIsFromStack) {
                                this.EntityPM.ConvertFromDirectToHouse = true;
                                this.EntityPM.ConvertFromHouseToDirect = false;
                            }

                            this.OkButton();
                        }

                        this.ResetButtonClicked();
                    });
                });
            }
        }
    }
    private ConvertShipmentToCustomFile() {
        this.currentActionName = "ConvertToCustomFile";
        this.EntityPM.ShipmentLevelCode = "A";
        this.EntityPM.DirectionId = "C";
        this.EntityPM.ConvertToCustomFile = true;
        this.entityArgs.EditComponent.SaveChanges();
        this.MenuButtonCode = null;

    }
    private ReactivateShipment() {

        this.currentActionName = "ReactivateShipment";

        var args = new MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Shipment Reactivation Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var state2: ActionsStepsState = new ActionsStepsState();
        state2.Message = TextCodeTranslator.Translate("Shipment.M.ShipmentReactivated");
        this.ActionStepsStateList.push(state2);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = "Reactivate Shipment";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!AppTool.IsNullOrEmpty(notes))
                        this.EntityPM.EventNote = notes;
                    this.EntityPM.IsCancelled = false;


                    this.OkButton();

                }

                this.ResetButtonClicked();


            });
        });






    }
    private CancelShipment() {


        this.currentActionName = "CancelShipment";

        if (this.EntityPM.ShipmentReceivables.filter(p => p.ARInvoiceId != null)[0]) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Title = "Cancelling Shipment";
            messageWindow.Show("This shipment can't be canceled because it has one or more invoices. all invoices must be disconnect to cancel this shipment");

        }

        else if (this.EntityPM.BookingId != null && this.EntityPM.BookingId != "") {

            var confirmWindow: ConfirmWindow = new ConfirmWindow();
            confirmWindow.Title = "Cancel Shipment";
            confirmWindow.Width = 400;
            confirmWindow.Show("Cancelling this shipment will disconnect it from the Booking , are you sure you want to cancel ?");
            confirmWindow.YesButtonText = "Yes";
            confirmWindow.NoButtonText = "No";


            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {

                    // console.log("Yes");
                    this.ConfirmCanceling();

                }

                else if (confirmWindow.No) {
                    // console.log("No");
                }

                this.ResetButtonClicked();

            });

        }
        else this.ConfirmCanceling();

    }
    private ResetButtonClicked() {
        this.isButtonClicked = false;
    }
    private ConfirmCanceling() {

        var args = new MenuButtonsTemplateArgs();
        args.ObjectTableName = "Shipment";
        args.EntityPM = this.EntityPM;
        args.NotesHeader = "Shipment Cancel Notes";
        args.IsNotesStackPanelVisible = true;
        args.EventNote = "";
        this.ActionStepsStateList = new Array<ActionsStepsState>();
        var state2: ActionsStepsState = new ActionsStepsState();
        state2.Message = TextCodeTranslator.Translate("Shipment.M.ShipmentCancelled");
        this.ActionStepsStateList.push(state2);
        args.ActionStepsStateList = this.ActionStepsStateList;
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = args;
        logWindow.Width = 935;
        logWindow.Height = 570;
        logWindow.Title = "Cancel Shipment";
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var notes = s.EventNotes;
                if (d == "confirm") {
                    if (!AppTool.IsNullOrEmpty(notes))
                        this.EntityPM.EventNote = notes;
                    this.EntityPM.IsCancelled = true;
                    if (this.EntityPM.MainCarriageIsFromStack || this.EntityPM.MAWBTakenFromStack) {
                        this.EntityPM.MAWBReturnedToStack = true;
                        this.EntityPM.MAWBReturnedToStackWithCancel = true;
                        this.EntityPM.MAWBStackNumber = this.EntityPM.Master;
                    }

                    this.OkButton();

                }
                this.ResetButtonClicked();
            });
        });


    }
    private OkButton() {
        if (this.currentActionName == "OperationalClose" || this.currentActionName == "AccountingClose") {
            ServiceLocator.SendTotangoUserActivity("Shipment", this.currentActionName);
        }

        else if (this.currentActionName == "ExceptionResolved") {
            this.EntityPM.ExceptionResolvedDescription = this.EntityPM.EventNote;
        }


        this.entityArgs.EditComponent.SaveChanges();
        this.MenuButtonCode = null;

        this.isButtonClicked = false;
    }
    private OperationalCloseShipment() {
        this.currentActionName = "OperationalClose";
        var WarningsList: Array<string> = [];
        var ErrorsList: Array<string> = [];

        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.myCloner = new Cloner(this.EntityPM);
            this.Clone(this.EntityPM);
            var args = this;
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Master Operational Close";
            logWindow.WindowArgs = args;
            logWindow.IsFillScreen = true;
            logWindow.Show('./Shipment/Components/MenuButtons/MasterActionConfirmationComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {
                this.ResetButtonClicked();
                if ($event == "confirm") {
                    this.EntityPM.OperationalCloseDate = DateTool.GetCurrentDateTimeAsUtc();
                    this.EntityPM.EventNote = null;
                    this.OkButton();
                }
                else {
                    this.RejectChanges();
                }
            });
        }

        else {
            this.myCloner = new Cloner(this.EntityPM);
            this.Clone(this.EntityPM);
            this.ActionStepsStateList = new Array<ActionsStepsState>();
            this.EntityPM.IsOperationalClosed = true;
            this.ValidateShipmentRules(WarningsList, ErrorsList, this.EntityPM);
            var tableId = window.ObjectTables.filter(t => t.Name == "Shipment")[0].Id;
            ServiceLocator.RulesValidator.ValidateAllRequiredFieldRules(this.EntityPM, tableId, ErrorsList);

            this.DisplayErrorsWindow(WarningsList, ErrorsList);
        }
    }
    private SplitShipmentClicked() {
        this.Validate();

        if (this.isValid) {
            if (this.EntityPM.IsOperationalClosed) {
                this.StopFlags();
                var messageWindow = new MessageWindow();
                messageWindow.Show("Can't split an operationally closed shipment");                
            }

            else {
                this.entityArgs.EditComponent.SaveChanges();
            }
        }
    }
    private SplitShipmentApply() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { EntityPM: this.EntityPM }
        logWindow.IsFillScreen = true;
        logWindow.Title = "Split Shipment";
        logWindow.Show('./Shipment/Components/SplitShipment/SplitShipmentComponent');
        logWindow.ComponentLoaded.subscribe(cmp => {
            this.StopFlags();

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
        });
    }

    private IsConvertToLCLClicked: boolean = false;
    private IsConvertToFCLClicked: boolean = false;
    private ConvertShipmentToLCLClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.IsConvertToLCLClicked = true;
            this.OkButton();
        }
    }
    private ConvertShipmentToFCLClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, "Shipment", errors);

        if (errors.length == 0) {
            this.IsConvertToFCLClicked = true;
            this.OkButton();
        }
    }
    private DoConvertShipmentType(type: string) {
        var errors: string[] = this.VaidateConvertShipmentType();
        SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var args = new MenuButtonsTemplateArgs();
            args.ObjectTableName = "Shipment";
            args.EntityPM = this.EntityPM;
            args.EventNote = null;
            args.IsNotesStackPanelVisible = true;
            var windowTitle: string;

            switch (type) {
                case "ToLCL":
                    {
                        this.currentActionName = "ConvertShipmentToLCL";
                        args.NotesHeader = "Convert Shipment From FCL To LCL...";
                        windowTitle = "Convert Shipment From FCL To LCL";
                        break;
                    }

                case "ToFCL":
                    {
                        this.currentActionName = "ConvertShipmentToFCL";
                        args.NotesHeader = "Convert Shipment From LCL To FCL...";
                        windowTitle = "Convert Shipment From LCL To FCL";
                        break;
                    }
            }
            
            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = args;
            logWindow.Width = 935;
            logWindow.Height = 570;

            logWindow.Title = windowTitle;
            logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    var notes = s.EventNotes;
                    if (d == "confirm") {
                        this.EntityPM.EventNote = notes;
                        this.ShowConfirmConvertShipmentType(type);                        
                    }                   
                });
            });
        }
    }
    private VaidateConvertShipmentType(): string[] {
        var errors: string[] = [];

        if (!AppTool.IsNullOrEmpty(this.EntityPM.QuoteId)) {
            errors.push("Cannot change shipment type when connected to a quote");
        }

        else if (this.EntityPM.ShipmentPackages.filter(d => !AppTool.IsNullOrEmpty(d.DeliveryId)).length > 0
            || this.EntityPM.ShipmentPackages.filter(d => !AppTool.IsNullOrEmpty(d.EmptyContainerReturnId)).length > 0) {
            errors.push("Cannot change shipment type when shipment packages are connected to a delivery or empty container return");
        }

        else if (this.EntityPM.ShipmentLevelCode == "H" && !AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
            errors.push("Cannot change shipment type when connected to a Master shipment");
        }

        else if (this.EntityPM.ShipmentLevelCode == "C" && this.EntityPM.ShipmentConsoleShipments.length > 0) {
            errors.push("Cannot change shipment type when connected to house shipments ");
        }
        
        return errors;
    }
    private ShowConfirmConvertShipmentType(type: string) {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Convert Shipment Type";
        confirmWindow.Width = 400;
        confirmWindow.Show("Changing the shipment type will result in deleting all the shipment packages , are you sure you want to change ?");
        confirmWindow.YesButtonText = "Yes";
        confirmWindow.NoButtonText = "No";

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                switch (type) {
                    case "ToLCL":
                        {
                            this.EntityPM.ConvertShipmentToLCL = true;
                            this.EntityPM.ConvertShipmentToFCL = false;
                            break;
                        }

                    case "ToFCL":
                        {
                            this.EntityPM.ConvertShipmentToLCL = false;
                            this.EntityPM.ConvertShipmentToFCL = true;
                            break;
                        }
                }

                this.Reload = true;
                this.OkButton();
            }

            this.ResetButtonClicked();
        });
    }

    private myCloner: Cloner;
    private Clone(EntityPM: ShipmentPM) {
        this.myCloner.AddField('IsOperationalClosed');
        this.myCloner.AddField('IsMaster');
        this.myCloner.AddEntity(EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
    private DisplayErrorsWindow(WarningsList: Array<string>, ErrorsList: Array<string>) {
        console.log(WarningsList);
        console.log(ErrorsList);



        var args = new MenuButtonsTemplateArgs();
        args.ValidationErrorsList = ErrorsList;
        args.ValidationWarningsList = WarningsList;
        args.IsNotesStackPanelVisible = false;
        args.ActionStepsStateList = this.ActionStepsStateList;

        var state = new ActionsStepsState();
        state.Message = TextCodeTranslator.Translate("Shipment.M.CheckingRequiredFields");
        if (ErrorsList.length != 0) {
            state.State = "Failed";
        }
        else {
            state.State = "Succeeded";
        }
        this.ActionStepsStateList.push(state);



        if (ErrorsList.length > 0)
            args.EnabledOkButton = false;
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Shipment Operational Close";
        logWindow.WindowArgs = args;
        logWindow.Show('./Shipment/Components/MenuButtons/MenuButtonsTemplateComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ResetButtonClicked();
            if ($event == "confirm") {
                this.EntityPM.IsOperationalClosed = true;
                this.EntityPM.OperationalCloseDate = DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.EventNote = null;
                this.OkButton();
            }
            else {

                this.EntityPM.RejectChanges();
                this.RejectChanges();

            }

        });
    }
    private ValidateShipmentRules(WarningsList: Array<string>, ErrorsList: Array<string>, entityPM: any) {

        var tableId = window.ObjectTables.filter(t => t.Name == "Shipment")[0].Id;

        var tableId = window.ObjectTables.filter(t => t.Name == "Shipment")[0].Id;

        var warningValidator: EntityWarningsValidator = new EntityWarningsValidator();
        var ruleValidator: RulesValidator = new RulesValidator();
        var requiredFields: Array<ObjectTableRuleFieldPM> = [];
        // ruleValidator.ExecuteRequierdFieldRule(entityPM,
        //ruleValidator.ValidateAllRequiredFieldRules(entityPM, tableId, ErrorsList);
        if (entityPM.ShipmentLevelCode == "H") {
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_AI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_OI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_IE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Shipment_OpClosed_Req_II", entityPM, requiredFields);

            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_AI", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_OI", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_IE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Shipment_OpClosed_Req_II", entityPM, WarningsList);
        }

        if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C") {

            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II", entityPM, requiredFields);

            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II_D", entityPM, requiredFields);

            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_AI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_OI_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_IE_D", entityPM, requiredFields);
            ruleValidator.ExecuteRequierdFieldRule("Master_OpClosed_Req_II_D", entityPM, requiredFields);

            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II", entityPM, WarningsList);

            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AE_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_AI_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OE_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_OI_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_IE_D", entityPM, WarningsList);
            warningValidator.ValidateRequiedFieldRule("Master_OpClosed_Req_II_D", entityPM, WarningsList);
        }

        var _tenantObjectFields = window.ObjectFields;
        if (requiredFields.length != 0) {

            for (var k in requiredFields) {
                var field = requiredFields[k];
                var obField = _tenantObjectFields.filter(x => x.Id === field.ObjectFieldId)[0];//ObjectFieldsCachedDataProvider.GetObjectFieldById(field.ObjectFieldId);
                var requiredError = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                var fieldTrans = TextCodeTranslator.Translate(obField.FullNameTextCodeCode);
                requiredError = requiredError.replace("%FieldName", fieldTrans);
                ErrorsList.push(requiredError);
            }

        }
    }    
}
export class ActionValidationArgs {
    public EnttiyPM: any;
    public WarningsList: Array<string> = [];
    public ErrorsList: Array<string> = [];
}
export class ActionsStepsState {
    constructor() { }
    private message: string;
    private state: string;
    public set Message(value: string) { this.message = value; }
    public get Message() { return this.message; }
    public set State(value: string) { this.state = value; }
    public get State() { return this.state; }
}
export class ValidationErrorInfo {
    private messageType: string;
    private errorCode: number;
    private errorMessage: string;
    public get MessageType() { return this.messageType; }
    public set MessageType(value: string) { this.messageType = value; }
    public set ErrorCode(value: number) { this.errorCode = value; }
    public get ErrorCode() { return this.errorCode; }
    public get ErrorMessage() { return this.errorMessage; }
    public set ErrorMessage(value: string) { this.errorMessage = value; }
    public ToString() {
        return this.ErrorMessage;
    }
}
