declare var window: any;
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { MenuButtonPM } from 'Infrastructure/EntityPMs/MenuButtonPM';
import { AppTool } from 'Infrastructure/Tools';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ContainerTrackingProviderPM } from 'Shipment/EntityPMs/ContainerTrackingProviderPM';
import { ContainerTrackingProviderExtendedPMService } from 'Shipment/Services/ExtendedPMs/ContainerTrackingProviderExtendedPMService';
import { ContainerTrackingProviderPMService } from '../../Services/StandardPMs/ContainerTrackingProviderPMService';

export class ContainerTrackingProviderMenuButtonsHandler {
    public EntityPM: ContainerTrackingProviderPM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'ContainerTrackingProvider')[0];

                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {

                    var button = menuButtons[i];

                    if (button.EventCode == "GetCarrier") {
                        button.IsDisabled = buttonEnabled ? (this.EntityPM.SourceCode != "VZN") : true;
                    }
                    if (button.EventCode == "ACTVRef") {
                        button.IsDisabled = buttonEnabled ? (this.EntityPM.SourceCode != "VZN") : true;
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


            switch (this.MenuButtonCode) {
                case "GetCarrier": {
                    this.GetCarrier();
                    break;
                }
                case "ACTVRef": {
                    this.GetActiveRequests();
                    break;
                }


                default: {
                    this.isButtonClicked = false;
                    break;
                }
            }

        }
        this.ResetButtonClicked();
    }
    GetActiveRequests() {
        var containerTrackingProviderPMService = new ContainerTrackingProviderExtendedPMService();
        containerTrackingProviderPMService.getActiveRequests(this.EntityPM.Id).subscribe(e => {
            if(e.HasError)
                return;
            var window = new MessageWindow();
            window.Width = 800;
            window.Height = 800;
            window.Show(JSON.stringify(e.Result));
        });
    }
    private ResetButtonClicked() {
        this.isButtonClicked = false;
    }
    GetCarrier() {
        var containerTrackingProviderPMService = new ContainerTrackingProviderExtendedPMService();
        containerTrackingProviderPMService.getSupportedCarrier(this.EntityPM.Id).subscribe(e => {
            if(e.HasError)
                return;
            var window = new MessageWindow();
            window.Width = 800;
            window.Height = 800;
            window.Show(JSON.stringify(e.Result));
        });

    }


    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent != null) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;




                    }

                    this.StopFlags();
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                        this.CurrentSession.SessionEvent.emit("ReloadHouses");
                    }

                    this.StopFlags();
                });
            }
        }
    }


    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    Reload: boolean = false;
    StopFlags() {
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
        this.Reload = false;

    }
    Validate() {


        // if (!this.isValid) {
        //     this.StopFlags();
        // }
    }


    private hasWarnings: boolean = false;
    public get HasWarnings() { return this.hasWarnings; }
    public set HasWarnings(value: boolean) { this.hasWarnings = value; }










}

