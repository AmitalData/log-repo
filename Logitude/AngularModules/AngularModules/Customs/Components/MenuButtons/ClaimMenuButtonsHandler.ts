declare var window: any;
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { ClaimPM } from '../../EntityPMs/ClaimPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {AppTool, DateTool, FormatTool} from '../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController } from '../../Controller/UnifreightController';
import { CustomDocumentTypeListService } from '../../Services/StandardLists/CustomDocumentTypeListService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { DocumentsFilingPM } from '../../../Common/EntityPMs/DocumentsFilingPM';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';


export class ClaimMenuButtonsHandler {
    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    public EntityPM: ClaimPM;
    private CurrentSession = SessionLocator.SelectedSession;

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

                    if (button.EventCode == "SendClaim") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "CloseClaim") {
                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "CancelCloseClaim") {
                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
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

            if (true) {//if (this.isValid) {
                switch (this.MenuButtonCode) {
                    case "CloseClaim":
                        {
                            this.CloseClaimMethod();
                            break;
                        }
                    case "CancelCloseClaim":
                        {
                            this.CancelCloseClaimMethod();
                            break;
                        }
                }
            }
        }
    }

    private CloseClaimMethod() {
        var isCloseClaimMethod: boolean = false;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.Claim.O.IsCloseClaim"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.IsClosed = true;
                isCloseClaimMethod = true;
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(
                    (isSave) => {
                        if (isCloseClaimMethod) {
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            let messageWindow = new MessageWindow();
                            messageWindow.Width = 300;
                            messageWindow.Height = 180;
                            messageWindow.Show(TextCodeTranslator.Translate("Customs.Claim.O.CloseClaim"));
                            isCloseClaimMethod = false;
                        }
                    });
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    }

    private CancelCloseClaimMethod() {
        var isCancelCloseClaimMethod: boolean = false;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.Claim.O.IsCancelCloseClaim"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.IsClosed = false;
                isCancelCloseClaimMethod = true;
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(
                    (isSave) => {
                        if (isCancelCloseClaimMethod) {
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            let messageWindow = new MessageWindow();
                            messageWindow.Width = 300;
                            messageWindow.Height = 180;
                            messageWindow.Show(TextCodeTranslator.Translate("Customs.Claim.O.CancelCloseClaim"));
                            isCancelCloseClaimMethod = false;
                        }
                    });
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
        
    }

}
