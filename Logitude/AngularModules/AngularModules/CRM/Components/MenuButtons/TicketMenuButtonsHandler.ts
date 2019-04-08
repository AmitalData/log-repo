declare var window: any;
import {TicketPM} from '../../EntityPMs/TicketPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {CashBookLinePM} from '../../../Accounting/EntityPMs/CashBookLinePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {DocumentOutPM}  from '../../../Common/EntityPMs/DocumentOutPM';
import {DocumentTypePM} from '../../../Common/EntityPMs/DocumentTypePM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {TicketClosureArgs} from '../../Args';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {TicketValidator} from '../../Validators/TicketValidator';
import {CRMDomainService} from '../../Services/CRMDomainService';

export class TicketMenuButtonsHandler {
    public EntityPM: TicketPM;
    public entityArgs: EntityArgs
    private status: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private isCancelled: boolean;
    private isReactivate: boolean;
    private isClosuerWindow: boolean;
    private ResetAllFlags() {
        this.isCancelled = false;
        this.isReactivate = false;
        this.isClosuerWindow = false;
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                        if (this.isCancelled) {
                            this.isCancelled = false;
                            this.CancelActivateTicket();
                        }
                        if (this.isReactivate) {
                            this.isReactivate = false;
                            this.CancelActivateTicket();
                        }
                        if (this.isClosuerWindow) {
                            this.isClosuerWindow = false;
                            var isValid = true;
                            var validator: TicketValidator = new TicketValidator();
                            var errors = validator.ValidateCurrenctEntity(this.EntityPM);
                            if (errors != null && errors.length > 0) {
                                isValid = false;
                                if (this.CurrentSession.CurrentEditComponent != null) {
                                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
                                }
                            }
                            if (isValid) {
                                this.CheckTicketOwnerPermission();
                            }
                        }
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    }
                });
            }
        }
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'Ticket')[0];

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "Cancel":
                            {
                                if (FeatureLocator.HasFeaturePermession("Ticket", "TicketMore")) {
                                    if (this.EntityPM.IsCancelled) {
                                        button.IsDisabled = true;
                                    }

                                    else {
                                        button.IsDisabled = false;
                                    }
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }

                        case "Reactivate":
                            {
                                if (FeatureLocator.HasFeaturePermession("Ticket", "TicketMore")) {
                                    if (this.EntityPM.IsCancelled) {
                                        button.IsDisabled = false;
                                    }
                                    else {
                                        button.IsDisabled = true;
                                    }
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }

                        case "ClosewithoutNotifying":
                            {
                                if (FeatureLocator.HasFeaturePermession("Ticket", "TicketMore")) {
                                    if (!FeatureLocator.HasFeaturePermession("Ticket", "TicketClosure")) {
                                        button.IsDisabled = true;
                                    }
                                }
                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }
                    }
                }
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "Cancel":
                {
                    this.status = true;
                    this.ResetAllFlags();
                    this.isCancelled = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }

            case "Reactivate":
                {
                    this.status = false;
                    this.ResetAllFlags();
                    this.isReactivate = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }

            case "ClosewithoutNotifying":
                {
                    this.ResetAllFlags();
                    this.isClosuerWindow = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
        }
    }

    // [Cancel Ticket]
    CancelActivateTicket() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        var title = "";
        if (this.status) {
            title = "Are you sure you want to cancel this Ticket ?";
        }
        else {
            title = "Are you sure you want to Re-active this Ticket ?";

        }
        confirmWindow.Show(title);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                if (this.entityArgs.EditComponent.ValidationErrorsList!= null && this.entityArgs.EditComponent.ValidationErrorsList.length == 0) {
                    if (this.status) {
                        this.EntityPM.IsCancelled = true;
                    }
                    else {
                        this.EntityPM.IsCancelled = false;
                    }
                    this.entityArgs.EditComponent.SaveChanges();
                }
            }
        });
    }

    // [Close Ticket]
    ClosuerWindow() {
        var windowTitle = "Ticket Closure";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        var args = new TicketClosureArgs();
        args.Ticket = this.EntityPM;
        args.StageCode = "CS";
        logWindow.WindowArgs = args;
        logWindow.Show('./CRMModules/CRMTickets/Components/EditTabs/Others/TicketClosureComponent');
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (comp.IsOkClosed == true) {
                        this.entityArgs.EditComponent.SaveChanges();
                    }
                }
            });
        });
    }

    CheckTicketOwnerPermission() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetTicketOwnerPermission(this.EntityPM.OwnerId, this.EntityPM.OwnerName).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ClosuerWindow();
            }
            else {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
}
