declare var window: any;
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { DeclarationPM } from '../../EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { AppTool, DateTool, FormatTool } from '../../../Infrastructure/Tools';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../Controller/UnifreightController';

import { DeclarationPMService } from '../../Services/StandardPMs/DeclarationPMService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {MenuButtonsEvents, MenuButtonsStateChangedEventArgs} from '../../../Infrastructure/Utilities/events/MenuButtonsEvents';

export class VehicleMenuButtonsHandler {
    //-----------------properties---------------------------//
    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    public EntityPM: DeclarationPM;
    checkTransfer: string = ""; // moran 4.8.16 - AMI-56804
    MenuButtons: MenuButtonPM[];
    IdentityKey: string;
    //------------------------------------------------------//
    private CurrentSession = SessionLocator.SelectedSession;
    IsDisplayOnly: boolean;
    IsDisplayOnlyCheckDone: boolean;
    MenuButtonsStateChangedEvent: any;
    //Services
    private declarationPMService: DeclarationPMService = new DeclarationPMService();

    public SetEntityPM(entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
        this.IdentityKey = AppTool.GetNewGuid();
    }

    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                    switch (this.MenuButtonCode) {

                    }
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }

        this.CurrentSession.SubscriptionAdd(
            this.MenuButtonsStateChangedEvent = MenuButtonsEvents.MenuButtonsStateChanged.subscribe((args: MenuButtonsStateChangedEventArgs) => {
                if (!this.IsDisplayOnly) {
                    if (!AppTool.IsNullOrEmpty(this.CurrentSession.CurrentEditComponent.EditComponentController)) {
                        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
                    }
                    this.CheckButtonState(this.MenuButtons);
                }

            })
        );
    }
    
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        this.MenuButtons = menuButtons;
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.EntityPM != null) {
            if (this.CurrentSession.CurrentEditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Customs.Vehicle')[0];

                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];


                    if (button.EventCode == "SendVehicle") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "DeleteVehicle") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                        }
                    }



                }
                return menuButtons;
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (true) {//!this.isButtonClicked) { this is temporary for testing.

            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;

            if (true) {//this.isValid) { this is also for testing temp of course
                switch (this.MenuButtonCode) {
                    case "SendVehicle":
                        {
                            ////SendDeclaration();
                            // SendDeclaration(declarationViewModel);
                            break;
                        }

                    case "DeleteVehicle":
                        {
                            ////SendDeclaration();
                            // SendDeclaration(declarationViewModel);
                            break;
                        }
                }
            }
        }
    }



}
