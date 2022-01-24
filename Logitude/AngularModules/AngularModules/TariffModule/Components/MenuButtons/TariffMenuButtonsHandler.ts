import { TariffPM } from '../../EntityPMs/TariffPM'
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TariffDomainService } from '../../Services/TariffDomainService';

export class TariffMenuButtonsHandler {
    public EntityPM: TariffPM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;
    MenuButtonCode: string = null;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(menuButton => {
                    menuButton.IsDisabled = false;
                    switch (menuButton.EventCode) {
                        case "Inactive": {
                            if (this.EntityPM.InActive)
                                menuButton.IsDisabled = true;
                            break;
                        }

                        case "Reactivate": {
                            if (!this.EntityPM.InActive) {
                                menuButton.IsDisabled = true;
                            }

                            break;
                        }

                        case "EditPriceSteps": {
                            if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFC" || this.EntityPM.TypeCode == "OFS"
                                || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
                                menuButton.IsHidden = true;
                            }
                            break;
                        }
                    }
                });
            }
        }

        return menuButtons;
    }

    private StopFlags() {
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
    }

    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;                   
                }
                if (this.MenuButtonCode == "EditPriceSteps") {
                    var service: TariffDomainService = new TariffDomainService();
                    service.GetAllVersionsWithLinesForTariff(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
                        if (!response.HasError) {
                            this.EditPriceStepsAction(response.Result);
                        }
                    });
                }
                this.StopFlags();
            });
        }
    }
  
    isButtonClicked: boolean = false;
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {
            this.StopFlags();
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            switch (menuButton.EventCode) {
                case "Inactive":
                    {
                        this.EntityPM.SetAsInActive = true;
                        this.EntityPM.InActive = true;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }

                case "Reactivate":
                    {
                        this.EntityPM.SetAsReActive = true;
                        this.EntityPM.InActive = false;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                case "EditPriceSteps": {
                    this.EditPriceStepsClicked();
                    break;
                }
                default: {
                    this.isButtonClicked = false;
                    break;
                }
            }
        }
    }

    EditPriceStepsClicked() {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    }

    EditPriceSteps() {
        
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Price Steps";
        logWindow.WindowArgs = this.EntityPM.PriceSteps;
        logWindow.Show("./TariffModule/Components/NewEntity/TariffPriceStepsComponent");
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d != "cancel") {
                    var steps = s.DefaultPriceSteps;
                    this.EntityPM.PriceSteps = steps;
                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                   // this.CurrentSession.SessionEvent.emit("PriceStepsModified");
                }
            });
        });
    }

    ShowMessageWindow(msg: string) {
        var messageWindow = new MessageWindow();
        messageWindow.Show(msg);
    }
    EditPriceStepsAction(versions : any) {
        if (versions != null) {
            if (versions != null && versions.length > 1) {
                this.ShowMessageWindow("Can't edit the price steps since the tariff has lines already.");
            }
            else {
                var version = versions[0];
                if (version == null || (version != null && version.IsDraft)) {
                    var hasTariffLines = false;
                    versions.forEach(item => {
                        if (item.TariffLines != null && item.TariffLines.length > 0) {
                            hasTariffLines = true;
                        }
                    });

                    if (hasTariffLines) {
                        this.ShowMessageWindow("Can't edit the price steps since the tariff has lines already.");
                    }
                    else {
                        this.EditPriceSteps();
                    }
                }
                else if (version != null && !version.IsDraft) {
                    this.ShowMessageWindow("Can't edit the price steps since the tariff has lines already.");
                }
                else {
                    this.EditPriceSteps();
                }
            }
        }
        else {
            this.EditPriceSteps();
        }
    }   
}
