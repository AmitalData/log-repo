declare var window: any;
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { PhysicalCheckPM } from '../../EntityPMs/PhysicalCheckPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import { PhysicalCheckWebService } from '../../../Customs/Services/WebServices/PhysicalCheckWebService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

export class PhysicalCheckMenuButtonsHandler {
    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    public EntityPM: PhysicalCheckPM;
    private CurrentSession = SessionLocator.SelectedSession;
    private _PhysicalCheckWebService: PhysicalCheckWebService = new PhysicalCheckWebService;

    public SetEntityPM(entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });
        }
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.CurrentSession.CurrentEditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Shipment')[0];

                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];

                    if (button.EventCode == "Actions") {
                        button.Width = 70;
                    }

                    if (button.EventCode == "ClosePhysicalCheck" ) {
                        if (this.EntityPM.IsClosed || this.EntityPM.BringQueueForwardIndicatorS == "4") {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }

                }
                return menuButtons;
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (true){//if (!this.isButtonClicked) {

            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;

            switch (this.MenuButtonCode) {
                case "ClosePhysicalCheck":
                    {
                        this.ClosePhysicalCheckMethod();
                        break;
                    }
            }
        }
    }

    private ClosePhysicalCheckMethod() {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Show("האם ברצונך לסגור את הבדיקה ?");//TextCodeTranslator.Translate("Customs.PhysicalCheck.O.IsClosePhysicalCheck"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this._PhysicalCheckWebService.PostClosePhysicalCheck(this.EntityPM.Id, this.EntityPM.Tenant)
                    .subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            let messageWindow = new MessageWindow();
                            messageWindow.Width = 300;
                            messageWindow.Height = 180;
                            messageWindow.Show("הבדיקה נסגרה בהצלחה");//TextCodeTranslator.Translate("Customs.PhysicalCheck.O.ClosePhysicalCheck"));
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                        }
                    });
            }
        });
    }

}
