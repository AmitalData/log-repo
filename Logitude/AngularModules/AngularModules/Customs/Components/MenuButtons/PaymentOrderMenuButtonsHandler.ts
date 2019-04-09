declare var window: any;
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { PaymentOrderPM } from '../../EntityPMs/PaymentOrderPM';
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

import { DocumentTypeListService } from '../../../Common/Services/StandardLists/DocumentTypeListService';

import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {DownloadManager} from '../../../Infrastructure/Utilities/DownloadManager';
export class PaymentOrderMenuButtonsHandler {
    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    public EntityPM: PaymentOrderPM;
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

                    if (button.EventCode == "More") {
                        button.IsDisabled = false;
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "PrintPaymentOrder") {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.DocumentPaymentId)) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                    }
                    if (button.EventCode == "SendPaymentOrder") {

                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "ClosePaymentOrder") {
                        if (this.EntityPM.IsClosed) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                        button.IsHidden = false;
                    }

                    if (button.EventCode == "UnClosePaymentOrder") {
                        if (!this.EntityPM.IsClosed || (this.EntityPM.IsClosed && this.EntityPM.ActualPayDate != null)) {
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

            if (true) {//if (this.isValid) {
                switch (this.MenuButtonCode) {
                    case "SendPaymentOrder":
                        {
                            //SendPaymentOrder(paymentOrderViewModel);
                            break;
                        }
                    case "ClosePaymentOrder":
                        {
                            this.ClosePaymentOrderMethod();
                            break;
                        }
                    case "UnClosePaymentOrder":
                        {
                            this.UnClosePaymentOrderMethod();
                            break;
                        }
                    case "PrintPaymentOrder":
                        {
                            this.PrintDeclarationFormMethod();
                            break;
                        }
                    case "PrintDeficit":
                        {
                            this.PrintDeficitFormMethod();
                            break;
                        }
                }
            }
        }
    }

    private ClosePaymentOrderMethod() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.PaymentOrder.O.ClosePaymentOrder"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.IsClosed = true;
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(
                    (isSave) => {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM(); let window = new MessageWindow();
                    });
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    }

    private UnClosePaymentOrderMethod() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.PaymentOrder.O.ReOpenPaymentOrder"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.EntityPM.IsClosed = false;
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(
                    (isSave) => {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM(); let window = new MessageWindow();
                    });
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    }

    private PrintDeclarationFormMethod() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.DocumentPaymentId)) {
            var msg = new MessageWindow();
            msg.Width = 350;
            msg.Show("There are no Declaration form  Document");
            return;
        }
        let documentName = this.EntityPM.Tenant + "_" + this.EntityPM.DocumentPaymentId;
        this.ShowDocument(documentName);
    }

    private PrintDeficitFormMethod() {
        let documentTypeId: string = "";
        var documentTypeListService: DocumentTypeListService = new DocumentTypeListService();
        var table = window.ObjectTables.filter(d => d.Name == 'Customs.Declaration')[0];
        var filters = new ApiQueryFilters();
        //filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        //filters.addAdditionalFilter("Code", "DEF", null, null, "Equals", false, false, false, "string");
        filters.GetAll = true;
        filters.Tenant = SessionLocator.Tenant        
        documentTypeListService.getAllFromCache(filters).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var myResult = response.Result;
                if (AppTool.IsNullOrEmpty(myResult)) {
                    var msg = new MessageWindow();
                    msg.Width = 350;
                    msg.Show("Declaration Document is missing");
                    return;
                }
                var item = myResult.filter(d => d.Code == "DEF")[0];
                if (!AppTool.IsNullOrEmpty(item)) {
                    documentTypeId = item.Id;
                }

                this.GetDocumentByPaymentNumber(documentTypeId);
            }
        });
    }

    GetDocumentByPaymentNumber(documentTypeId : string){

        if (AppTool.IsNullOrEmpty(documentTypeId)) {
            var msg = new MessageWindow();
            msg.Width = 350;
            msg.Show("The value 'DEF' does not exist in Document Type");
            return;
        }

        var documentFiling: DocumentsFilingPM;
        var documentsFilingExtendedPMService: DocumentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        documentsFilingExtendedPMService.GetSingleDocumentsFilingByChild(documentTypeId, this.EntityPM.PaymentNumber, this.EntityPM.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && !AppTool.IsNullOrEmpty(pmResponse.Result)) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    documentFiling = myResult;

                    if (AppTool.IsNullOrEmpty(documentFiling)) {
                        var msg = new MessageWindow();
                        msg.Width = 350;
                        msg.Show("Declaration Document is missing");
                        return;
                    }
                    else {
                        let documentName = this.EntityPM.Tenant + "_" + documentFiling.DocumentId;
                        this.ShowDocument(documentName);
                    }
                }
            }
            else {
                var msg = new MessageWindow();
                msg.Width = 350;
                msg.Show("לא נמצאה הודעת חיוב ");
                return;
            }
        });

    }

    ShowDocument(documentName: string){

        DownloadManager.DownloadPage(documentName);
        //if(AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(uri);
        //    return;
        //}
  
    }
}
