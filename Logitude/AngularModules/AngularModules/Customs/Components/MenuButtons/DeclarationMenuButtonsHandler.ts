declare var window: any;
import { Component, Output, EventEmitter, OnDestroy, ChangeDetectorRef } from '@angular/core';
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
import { UnifreightController, UnifreightInstructionController } from '../../Controller/UnifreightController';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { DeclarationPMService } from '../../Services/StandardPMs/DeclarationPMService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationCourierStatusPMService } from '../../Services/StandardPMs/DeclarationCourierStatusPMService';
import { DeclarationCourierStatusPM } from '../../../Customs/EntityPMs/DeclarationCourierStatusPM';
import { CustomsRequestMenuService } from '../../Services/Others/CustomsRequestMenuService';
import { IIGGeneralMessagesService } from '../../Services/WebServices/IIGGeneralMessagesService';
import { CustomFileCreditRequestParams } from '../../DataContract/RequestParams/CustomFileCreditRequestParams';

import { CustomMessageProgressHelper, CustomMessageProgressComponent, ShowProgressBarParams } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { DeclarationMessagesService } from '../../Services/WebServices/DeclarationMessagesService';
import { DeclarationWebService } from '../../Services/WebServices/DeclarationWebService';
import { CustomFileCreditResponseData } from '../../DataContract/ResponseData/CustomFileCreditResponseData';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../Utilities/DeclarationDisplayOnlyChecks';
import { VehicleReductionTypeListService } from '../../Services/StandardLists/VehicleReductionTypeListService';
import { MenuButtonsEvents, MenuButtonsStateChangedEventArgs } from '../../../Infrastructure/Utilities/events/MenuButtonsEvents';

import { PrintRequestRequestParams } from '../../DataContract/RequestParams/PrintRequestRequestParams';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { DeclarationEventManager } from '../../Utilities/DeclarationEventManager';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';
import { MenuButtonsComponent } from '../../../Infrastructure/Components/LogitudeComponents/MenuButtonsComponent/MenuButtonsComponent';
import { GenericRequestParams } from '../../DataContract/RequestParams/GenericRequestParams';
import { TestCase } from '../../DataContract/RequestParams/RequestParamsBase';
import { CustomsSettingExtendedListService } from '../../Services/ExtendedLists/CustomsSettingExtendedListService';
import { ExportStoragePM } from 'Customs/EntityPMs/ExportStoragePM';
import { ExportStoragePMService } from 'Customs/Services/StandardPMs/ExportStoragePMService';
import { NotificationPMService } from 'Customs/Services/StandardPMs/NotificationPMService';
import { NotificationPM } from 'Customs/EntityPMs/NotificationPM';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from 'Infrastructure/Args';
import { MainMenuItem } from 'Infrastructure/Components/MainMenuComponent/MainMenuComponent';
import { List } from 'Infrastructure/DataContracts/Dashboard/List';


export class DeclarationMenuButtonsHandler implements OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.EntityResourceService = new EntityResourceService();
    }
    ///aaaaa: string = 10;
    //-----------------properties---------------------------//
    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    public EntityPM: DeclarationPM;
    public ExportStoragePM: ExportStoragePM;
    public NotificationPM: NotificationPM;
    checkTransfer: string = ""; // moran 4.8.16 - AMI-56804
    MenuButtons: MenuButtonPM[];
    IdentityKey: string;
    IsDisplayOnly: boolean;
    IsDisplayOnlyCheckDone: boolean;
    MenuButtonsStateChangedEvent: any;
    private EntityResourceService: EntityResourceService;
    IsShowMenuBTN: boolean = false;
    containerizationIdList: string;
    //------------------------------------------------------//

    public CurrentEditComponentId: string;
    //Services
    private declarationPMService: DeclarationPMService = new DeclarationPMService();
    private declarationWebService: DeclarationWebService = new DeclarationWebService();
    private declarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
    private notificationPMService: NotificationPMService = new NotificationPMService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();
    declarationService: DeclarationPMService = new DeclarationPMService();


    public SetEntityPM(entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
        this.IdentityKey = AppTool.GetNewGuid();
    }
    //private _SubMenuButtonsStateChanged;
    //private _SubSaveCompleted;
    //private _SubLoadCompleted;
    //private _SubDisplayModeChanged;
    ngOnDestroy() {
        console.log("DeclarationMenuButtonsHandler:ngOnDestroy");
        //if (this._SubMenuButtonsStateChanged) {
        //    this._SubMenuButtonsStateChanged.unsubscribe();
        //    this._SubMenuButtonsStateChanged = null;
        //}
        //if (this._SubSaveCompleted) {
        //    this._SubSaveCompleted.unsubscribe();
        //    this._SubSaveCompleted = null;
        //}
        //if (this._SubLoadCompleted) {
        //    this._SubLoadCompleted.unsubscribe();
        //    this._SubLoadCompleted = null;
        //}
        //if (this._SubDisplayModeChanged) {
        //    this._SubDisplayModeChanged.unsubscribe();
        //    this._SubDisplayModeChanged = null;
        //}


    }
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            //this._SubMenuButtonsStateChanged =
            this.CurrentSession.SubscriptionAdd(
                this.MenuButtonsStateChangedEvent = MenuButtonsEvents.MenuButtonsStateChanged.subscribe((args: MenuButtonsStateChangedEventArgs) => {
                    if (!this.IsDisplayOnly) {
                        if (!AppTool.IsNullOrEmpty(this.CurrentSession.CurrentEditComponent.EditComponentController)) {
                            this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
                        }
                        this.ApplyCheckMenuButtonsState(this.MenuButtons);
                    }

                })
            );


            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(

                this.CurrentSession.CurrentEditComponent.TabChanged.subscribe((tabCode: string) => {
                    if (tabCode == "DCNT" && this.EntityPM.Direction == "E") {
                        this.IsShowMenuBTN = true;
                    }
                    else {
                        this.IsShowMenuBTN = false;
                    }
                    this.ApplyCheckMenuButtonsState(this.MenuButtons);
                })
            );

            //this._SubSaveCompleted =
            this.CurrentSession.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                        switch (this.MenuButtonCode) {

                        }
                    }
                })
            );
            //this._SubLoadCompleted =
            this.CurrentSession.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            //this._SubDisplayModeChanged =
            this.CurrentSession.SubscriptionAdd(
                DeclarationEventManager.DisplayModeChanged.subscribe((IsDisplayOnly: any) => {
                    this.CheckButtonState(this.MenuButtons);

                })
            );

        }
        //if (this.CurrentSession.CurrentEditComponent != null) {
        //    this.CurrentSession.CurrentEditComponent.MenuButtonsHandlerREF = this;
        //}
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        this.MenuButtons = menuButtons;
        if (SessionLocator.TenantPM.IsTestTenant) {


            let myMenuButtonDeclarationsStatusRequest = this.MenuButtons.filter(r => r.EventCode == "DeclarationsStatusRequest").slice(0)[0];
            //let myMenuButtonPM: MenuButtonPM= (JSON.parse(JSON.stringify(myMenuButtonDeclarationsStatusRequest))) ;
            let myMenuButtonPM = new MenuButtonPM(null);
            for (var attribut in myMenuButtonDeclarationsStatusRequest) {
                if (typeof this[attribut] === "object") {
                    //cloneObj[attribut] = this.clone();
                } else {
                    myMenuButtonPM[attribut] = myMenuButtonDeclarationsStatusRequest[attribut];
                }
            }
            //myMenuButtonPM.MenuButtonGroupId = myMenuButtonDeclarationsStatusRequest.
            myMenuButtonPM.Id = "SincroSendDeclarationDCA";
            myMenuButtonPM.LabelTextCodeCode = null;
            myMenuButtonPM.LabelTextCodeId = null;
            myMenuButtonPM.DisplayText = " DCA תרחיש";
            myMenuButtonPM.EventCode = "SincroSendDeclarationDCA";
            myMenuButtonPM.ShowMenuButton = true;
            myMenuButtonPM.IsHidden = false;

            myMenuButtonPM.Index = 1000
            menuButtons.push(myMenuButtonPM);
        }
        this.DisplayOnlyCheck();
    }

    ApplyCheckMenuButtonsState(menuButtons: MenuButtonPM[]) {
        let parentButton: MenuButtonPM;
        
        if (this.EntityPM != null) {
            if (this.CurrentSession.CurrentEditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

                var buttonEnabled: boolean = true;
                var eventsTabFeature = FeatureLocator.Features.filter(f => (f.Code == "UPDATE") && f.ObjectTableId == table.Id)[0];
                if (!eventsTabFeature) {
                    buttonEnabled = false;
                }

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    if (button.EventCode == "More") {
                        button.IsDisabled = true;
                        button.IsHidden = true;

                    }
                    if (button.EventCode == "CloseDeclaration") {
                        button.Width = 100;
                        if (this.EntityPM.Direction == "E" && this.EntityPM.DeclarationNumber != null) {
                            button.IsHidden = false;
                            if (this.EntityPM.IsSubmitDeclaration)
                                button.IsDisabled = false
                            else
                                button.IsDisabled = true
                        }
                        else if (this.EntityPM.Direction == "E" && this.EntityPM.AmendmentDontDisplayInList && this.EntityPM.IsExportClosed) button.IsHidden = false;

                        else {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "OpenNewContainerization") {
                        this.containerizationIdList = "";

                        button.Width = 100;
                        button.DisplayText = "המכלה";
                        if (this.EntityPM.Direction == "E" && this.EntityPM.ProcedureCurrentCode && this.EntityPM.ProcedureCurrentName && this.EntityPM.ProcedureCurrentName.includes("המכלה לפני התרה")) {
                            button.IsHidden = false;
                            this.EntityPM?.Consignments.forEach(c => {
                                this.containerizationIdList += (!AppTool.IsNullOrEmpty(c.ExportContainerizationID) ? (c.ExportContainerizationID + ",") : "")

                            });
                            if (this.containerizationIdList != "") {
                                button.DisplayText = "הומכל";
                                button.LabelTextCodeCode = ""
                            }
                        }
                        else {
                            button.IsHidden = true;
                        }



                    }
                    if (button.EventCode == "SendDeclaration") {
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                            button.IsHidden = false;
                        }
                        else {
                            button.IsDisabled = false;

                            button.IsHidden = false;

                        }




                    }


                    if (button.EventCode == "SendManifest") {
                        if (this.IsDisplayOnly) {
                            button.IsDisabled = true;
                            //  button.IsHidden = false;
                        }
                        else {
                            button.IsDisabled = false;
                            // button.IsHidden = false;
                        }

                        if (this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = false;
                            if (!this.IsDisplayOnly) {
                                if (this.EntityPM.IsAmendment == true && !AppTool.IsNullOrEmpty(this.EntityPM.AmendmentStatus)) {
                                    button.IsDisabled = true;
                                }
                                if (this.EntityPM.IsAmendment != true && !((this.EntityPM.CourierPaymentStatusCode != 'P' || this.EntityPM.CourierPaymentStatusCode == null) &&
                                    (this.EntityPM.CourierManifestStatusCode == 'V' || this.EntityPM.CourierManifestStatusCode == 'R' || this.EntityPM.CourierManifestStatusCode == 'X' || this.EntityPM.CourierManifestStatusCode == 'M')
                                )) {

                                    button.IsDisabled = true;
                                }
                            }

                        }
                        else {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "DeclarationPayment") {
                        if (!this.EntityPM.IsAmendment) {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                        button.IsHidden = false;
                        button.Width = 120;

                        if (this.EntityPM.Direction == "E") {
                            button.DisplayText = TextCodeTranslator.Translate("Customs.Declaration.TH.PaymentsExport");
                            button.LabelTextCodeCode = "";
                            if (this.EntityPM.DeclarationStatusTypeCode == "11") {
                                button.IsDisabled = true;
                            }

                        }
                    }
                    if (button.EventCode == "Forms") {
                        button.Width = 60;
                    }
                    if (button.EventCode == "PrintDeclarationForm") {
                        //if (!declaration.HasDocument)
                        //if (AppTool.IsNullOrEmpty(this.EntityPM.DocumentDeclarationId)) {
                        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

                        if (AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber) ||
                            (FeatureLocator.Features.filter(f => (f.Code == "DisabledPrintDecForm") && f.ObjectTableId == table.Id)[0] && this.EntityPM.Direction == "E" && !this.EntityPM.IsExportClosed)) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }
                    }

                    if (button.EventCode == "Actions") {

                        button.Width = 70;
                    }

                    if (button.EventCode == "PrintRelease") // moran 29.2.16 - Task 19807
                    {
                        //if (this.EntityPM.IsReleaseFile && !AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                        if (this.EntityPM.IsReleaseFile && AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "PrintTzrufa") // moran 2.3.16 - Task 19807
                    {
                        //if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {

                        if ((AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)
                            || (AmitalGatewayUtil.Instance.AmitalBrowserInUse && this.EntityPM.IsAmendment)) && !this.EntityPM.AmendmentDontDisplayInList) {
                            //if (!this.EntityPM.IsAccumulated) {
                            //    button.IsDisabled = false;
                            //} else {
                            //    button.IsDisabled = true;
                            //}
                            button.IsDisabled = false;
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "PrintAccumaltedTzrufa") {

                        if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {

                            if (this.EntityPM.IsAccumulated) {
                                button.IsDisabled = false;
                            } else {
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                        }
                    }

                    if (button.EventCode == "TransferToCollector") // moran 4.8.16 - AMI-56804
                    {
                        if (this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = true;
                        }
                        else {
                            if (this.checkTransfer == "1" || (this.EntityPM.AmendmentDontDisplayInList != true && this.EntityPM.IsAmendment)) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }
                    }
                    if (button.EventCode == "Copy") // moran 4.8.16 - AMI-56804
                    {

                        if (this.EntityPM.AmendmentDontDisplayInList != true && this.EntityPM.IsAmendment) {
                            button.IsDisabled = true;
                        }
                        else {
                            button.IsDisabled = false;
                        }

                    }
                    if (button.EventCode == "Vehicle Modifications") {
                        if (this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "ResetDeclarationNumber") {//Eitan H 26/11/17 34387
                        if (this.IsDisplayOnly || (this.EntityPM.Direction == "E" && this.EntityPM.IsSubmitDeclaration)) {
                            button.IsDisabled = true;
                            button.IsHidden = false;
                        }
                        else {
                            button.IsDisabled = false;
                            button.IsHidden = false;
                        }
                    }
                    if (button.EventCode == "CancelPayment") {
                        if (FeatureLocator.HasFeaturePermession("Customs.Declaration", "CancelPaymentFeature") && (this.EntityPM.Direction != "E")) {
                            button.IsHidden = false;
                            if(AppTool.IsNullOrEmpty(this.EntityPM.PaymentDate)){
                                button.IsDisabled = true;
                            }
                        }
                        else {
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "CourierPendingReason") {
                        if (!this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "CourierPendingReasonDel") {
                        if (!this.EntityPM.IsCourierDeclaration) {
                            button.IsHidden = true;
                        }
                    }

                    if (button.EventCode == "Declaration Closure") {
                        if (this.EntityPM.Direction != "E") {
                            if (this.EntityPM.IsClose) {
                                button.IsHidden = true;
                            }
                            else {
                                button.IsHidden = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                            button.IsHidden = true;
                        }

                    }
                    if (button.EventCode == "OperationalClosure") {
                        if (this.EntityPM.Direction == "E") {
                            if (this.EntityPM.IsClose) {
                                button.IsHidden = true;
                            }
                            else {
                                button.IsHidden = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                            button.IsHidden = true;
                        }


                    }


                    if (button.EventCode == "Cancel Declaration Closure") {
                        if (this.EntityPM.Direction != "E") {
                            if (!this.EntityPM.IsClose) {
                                button.IsHidden = true;
                            }
                            else {
                                button.IsHidden = false;
                            }
                        }
                        else {
                            button.IsHidden = true;
                            button.IsDisabled = true;
                        }

                    }
                    if (button.EventCode == "CancelOperationalClosure") {
                        if (this.EntityPM.Direction == "E") {
                            if (!this.EntityPM.IsClose) {
                                button.IsHidden = true;
                            }
                            else {
                                button.IsHidden = false;
                            }
                        }
                        else {
                            button.IsDisabled = true;
                            button.IsHidden = true;
                        }

                    }
                    if (button.EventCode == "Declaration Customs Requests") {
                        if (this.EntityPM.AmendmentDontDisplayInList == false && this.EntityPM.IsAmendment == true) {
                            button.IsDisabled = false;
                        }
                        else {
                            if (this.IsDisplayOnly) {
                                button.IsDisabled = true;
                            }
                            else {
                                button.IsDisabled = false;
                            }
                        }

                    }
                    if (this.EntityPM.AmendmentDontDisplayInList == true) {
                        parentButton = menuButtons.filter(x => x.EventCode == "Actions")[0];
                        if (parentButton.Id == button.ParentMenuButtonId && button.EventCode != "ExportStorageDecleration")
                            button.IsDisabled = true;
                    }
                    if (button.EventCode == "ExportStorageDecleration") {

                        if (this.EntityPM.TransportModeId == "O" && this.EntityPM.Direction == "E") {
                            if (this.IsDisplayOnly) {
                                button.IsDisabled = true;
                            }
                            else {

                                button.IsHidden = false;
                            }

                        }
                        else {
                            button.IsDisabled = true;
                            button.IsHidden = true;
                        }
                    }
                    if (button.EventCode == "Sending Initiated Message") {



                        if (this.IsShowMenuBTN) {
                            button.IsHidden = false;
                        }
                        else {
                            button.IsHidden = true;

                        }

                    }
                    if (button.EventCode == "UpdateMehesAutonmy") {
                        button.IsHidden = false;
                    }
                    if (button.EventCode == "DeclarationRestore") {
                        if (this.EntityPM.Direction == "E" && !this.EntityPM.IsSubmitDeclaration) {
                            button.IsDisabled = true;
                        }
                    }
                }

                this.IsDisplayOnlyCheckDone = true;
                return menuButtons;
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (true) {//!this.isButtonClicked) { this is temporary for testing.

            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;


            if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
                // save changes
                this.CurrentSession.StartBusyIndicatorSaving();
                this.declarationPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CurrentSession.StopBusyIndicator();
                    this.MenuButtonClickDo();
                });

            } else {
                this.MenuButtonClickDo();
            }
        }
    }

    public MenuButtonClickDo() {

        if (true) {//this.isValid) { this is also for testing temp of course
            switch (this.MenuButtonCode) {
                case "SincroSendDeclarationDCA":
                    {
                        this.SincroSendDeclarationDCA();
                        break;
                    }
                case "SendDeclaration":
                    {
                        ////SendDeclaration();
                        // SendDeclaration(declarationViewModel);
                        break;
                    }
                case "DeclarationReset":
                    {
                        //InvokeOperation < string > op = declarationViewModel.Context.ResetDeclarationNumber(declaration.Id, declaration.Tenant);
                        //op.Completed += op_ResetDeclarationNumberCompleted;
                    }
                    break;

                case "DeclarationPayment":
                    {
                        this.OpenPaymentOrderWindow();
                        break;
                    }
                case "DeclarationCancellation":
                    {
                        this.OpenDeclarationCancellationWindow();
                        break;
                    }
                case "PrintTzrufa":
                    {
                        this.PrintTzrufaMethod(false);
                        break;
                    }
                case "PrintAccumaltedTzrufa":
                    {
                        this.PrintTzrufaMethod(true);
                        break;
                    }

                case "PrintDeclarationForm":
                    {

                        this.OnPrintDeclarationFormClick();
                        //this.PrintDeclarationFormMethod();//declarationViewModel);
                        break;
                    }

                case "DeclarationsStatusRequest":
                    {
                        this.DeclarationsStatusRequestMethod();
                        //SaveDeclarationMethod("DeclarationsStatusRequest");
                        break;
                    }

                case "DeclarationRestore":
                    {
                        this.DeclarationRestoreMethod();//SaveDeclarationMethod("DeclarationRestore");
                        break;
                    }

                case "ResetDeclarationNumber":
                    {
                        this.ResetDeclarationNumberMethod();
                        let toDo = false;
                        if (toDo) {
                            this.CurrentSession.StartBusyIndicator("");
                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService();
                            myIIGGeneralMessagesService.GetResetDeclarationNumber(this.EntityPM.Id, this.EntityPM.Tenant)
                                .subscribe((myServiceResponse: ServiceResponse) => {
                                    let messageWindow = new MessageWindow();
                                    this.CurrentSession.StopBusyIndicator();
                                    if (myServiceResponse.HasError) {

                                        messageWindow.Show(myServiceResponse.ErrorsArray[0]);
                                    }
                                    else {
                                        if (myServiceResponse.Result != null) {
                                            messageWindow.Show(myServiceResponse.Result);
                                        }
                                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();


                                    }
                                });
                        }
                        break;
                    }
                case "CancelPayment":
                    {
                        this.CancelPaymentMethod();
                        break;
                    }

                case "Copy":
                    {
                        this.SaveDeclarationMethod("Copy");
                        break;
                    }

                case "PrintRelease": // moran 29.2.16 - Task 19807
                    {
                        this.PrintReleaseMethod();
                        break;
                    }
                case "OpenNewContainerization":
                    {/////
                        this.OpenNewContainerizationMethod();
                        break;
                    }
                case "CloseDeclaration":
                    {
                        this.CloseDeclarationMethod();
                        break;
                    }
                // moran 5.6.16 - AMI-56804 - add TransferToCollector
                case "TransferToCollector":
                    {
                        // SaveDeclarationMethod("TransferToCollector");
                        this.TransferToCollectorMethod();
                        break;
                    }

                case "Vehicle Modifications":
                    {
                        this.DisplayDeclarationVehicleModificationsMethod();
                        break;
                    }

                case "SpecialActionRequest":
                    {
                        this.SpecialActionRequestMethod();
                        break;
                    }
                case "CourierPendingReason":
                    {
                        this.CourierPendingReasonMethod();
                        break;
                    }
                case "CourierPendingReasonDel":
                    {
                        this.CourierPendingReasonDeleteMethod();
                        break;
                    }
                case "Declaration Closure":
                    {
                        this.DeclarationClosureMethod();
                        break;
                    }
                case "OperationalClosure":
                    {
                        this.DeclarationClosureMethod();
                        break;
                    }
                case "Cancel Declaration Closure":
                    {
                        this.CancelDeclarationClosureMethod();
                        break;
                    }
                case "CancelOperationalClosure":
                    {
                        this.CancelDeclarationClosureMethod();
                        break;
                    }
                case "Declaration Customs Requests":
                    {
                        this.DeclarationCustomsRequestsMethod();
                        break;
                    }
                case "LoadExcelSupplierInvoices":
                    {
                        this.OpenDeclarationLoadExcelSupplierInvoiceWindow();
                        break;
                    }
                case "ExportStorageDecleration":
                    {

                        this.OpenExportStorageDeclarationMethod()
                        break;
                    }
                case "Sending Initiated Message":
                    {

                        this.AddNotifications()
                        break;
                    }
                case "UpdateMehesAutonmy":
                    {
                        this.OpenMehesUpdateAutonmyWindow();
                        break;
                    }
            }

        }
    }

    OpenDeclarationLoadExcelSupplierInvoiceWindow() {
        var args: any = {
            DeclarationId: this.EntityPM.Id,
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 300;
        logWindow.Title = "הטענת חשבון ספק";
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/LoadExcelSupplierInvoice/LoadExcelSupplierInvoicesComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }

    OpenDeclarationCancellationWindow() {
        var args: any = {
            Declaration: this.EntityPM,
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.DeclarationCancellation");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationCancellation/DeclarationCancellationComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }

    OpenMehesUpdateAutonmyWindow() {
        var myDeclarationWebService = new DeclarationWebService();
        myDeclarationWebService
            .CheckIfError12195ExistInCustomfileno(this.EntityPM.Id, this.EntityPM.Tenant, this.EntityPM.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                if (myResponse.Result) {
                    let confirm = new ConfirmWindow();
                    confirm.WindowClosed.subscribe((event: any) => {
                        if (confirm.Yes) {
                            this.CurrentSession.StartBusyIndicatorCreating();
                            myDeclarationWebService
                                .UpdateSupplierInvoiceItemsWhoHasError12195(this.EntityPM.Id, this.EntityPM.Tenant, this.EntityPM.CustomFileNo)
                                .subscribe((myResponse: ServiceResponse) => {
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                    this.CurrentSession.StopBusyIndicator();
                                });
                        }
                    });
                    confirm.Show("האם לעדכן ספר מכס אוטונומיה לשורות עם שגיאה מס' 12195");

                }else{
                    let window = new MessageWindow();
                    window.Show("אין פרטי מכס לעדכון");
                }
            });
        /*var args: any = {
            Declaration: this.EntityPM,
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.DeclarationCancellation");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationCancellation/DeclarationCancellationComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });*/
    }
    OpenDeclarationCancellationWindow_() {
        var args: any = {
            Declaration: this.EntityPM,
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.DeclarationCancellation");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationCancellation/DeclarationCancellationComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }
    SincroSendDeclarationDCA(): any {

        let windowArgs = { "SincroScreen": "SincroSendDeclarationDCA" };

        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 600;
        logWindow.Title = "תרחשי הצהרה";
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(res => {
                if (!AppTool.IsNullOrEmpty(res) && res == "Ok") {


                    this.CurrentSession.StartBusyIndicatorCreating();
                    var searchParams: GenericRequestParams = new GenericRequestParams();
                    searchParams.Tenant = SessionLocator.Tenant;
                    searchParams.AppicationId = this.EntityPM.Id;
                    searchParams.LoggingEnabled = true;
                    searchParams.LoggingEntityId = this.EntityPM.Id;
                    searchParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
                    ///searchParams.LoggingObjectTableId = this.ObjectTable.Id;
                    searchParams.LoggingUserId = SessionLocator.LoggedUserId;
                    //searchParams.RequestName = "Declaration Request";
                    //searchParams.ResponseName = "Declaration Response";
                    //searchParams.RequestVIA = this.RequestVIA;
                    //searchParams.ForcePersonalSign = this.ForcePersonalSign;


                    searchParams.TestCase = new TestCase();
                    searchParams.TestCase.Code = comp._ScenarioCode;
                    searchParams.TestCase.Param1 = comp.Param1;
                    searchParams.TestCase.Param2 = comp.Param2;
                    let srv = new CustomsSettingExtendedListService();
                    srv.PostSincroOption(searchParams)
                        .subscribe((response: ServiceResponse) => {
                            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            this.CurrentSession.StopBusyIndicator();
                        });

                }
            });
        });

        logWindow.Show('./CustomsModules/CustomsControls/Components/TestCase/SendDeclarationTastCaseComponent');

    }

    DisplayDeclarationVehicleModificationsMethod() {
        this.CurrentSession.StartBusyIndicatorLoading();
        let myVehicleReductionTypeListService = new VehicleReductionTypeListService();
        myVehicleReductionTypeListService.getAllFromCache().
            subscribe((res: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder", 0).subscribe((response: any) => {
                    this.CurrentSession.StopBusyIndicator();


                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 500;
                    logWindow.Height = 600;
                    //logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.CopyDeclaration");
                    let windowArgs: any = {};
                    windowArgs.EntityPM = this.EntityPM;

                    logWindow.WindowArgs = windowArgs;
                    logWindow.ShowCloseButton = true;
                    logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/VehicleModificationsComponent');

                    logWindow.WindowClosed.subscribe(($event: any) => {

                    });


                });
            });


    }

    SaveDeclarationMethod(ActionName: string) {
        if (this.EntityPM.IsDirty) {
            let confirm = new ConfirmWindow();
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {

                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                    if (this.EntityPM.SupplierInvoices.length == 0) {
                        this.CopyMethod();
                    }
                    else {
                        let window = new MessageWindow();

                        window.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CantCopy"));
                    }

                }
            });
            confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.SaveDeclaration"));
        }
        else {
            if (this.EntityPM.SupplierInvoices.length == 0) {
                this.CopyMethod();
            }
            else {
                let window = new MessageWindow();

                window.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CantCopy"));
            }

        }
    }

    CopyMethod() {

        var windowArgs: any = {};
        windowArgs.DeclarationPM = this.EntityPM;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 900;
        logWindow.Height = 800;
        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.CopyDeclaration");
        logWindow.WindowArgs = windowArgs;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationQueryComponent');

        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ReloadEntity($event);
        });



    }

    ReloadEntity(message: string) {
        if (message != "cancel") {

            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

        }
    }

    
    CancelPaymentMethod() {
        const confirm = new ConfirmWindow();
        confirm.WindowClosed.subscribe((event) => {
            if (confirm.Yes) {
                // send massage 2755 like logic on DeclarationPaymentComponent component
                var params = new CustomFileCreditRequestParams();
                var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.Declaration")[0];

                
                params.Tenant = SessionLocator.Tenant;
                params.AppicationId =  this.EntityPM.Id;
                params.LoggingEnabled = true;
                params.LoggingEntityId = this.EntityPM.Id;
                params.LoggingObjectTableId = ObjectTable.Id;
                params.LoggingUserId = SessionLocator.LoggedUserId;
                params.RequestName = "send cancel payment request";
                params.ResponseName = "send cancel payment response";
                params.Mode = "Check";

                var messageWindow = new MessageWindow();
                messageWindow.Width = 400;
                messageWindow.Height = 150;
                messageWindow.Title = "ביטול הגשה";
                

                let myShowProgressBarParams = new ShowProgressBarParams();
                myShowProgressBarParams.OnCloseCustomMessageProgressComponentMethod =
                    (response: any) => {
                        let myPaymentResponseData: CustomFileCreditResponseData = response;
                      
                    };
                CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession, params.PBId, "ביטול הגשה", false, myShowProgressBarParams).then(res => {
                    var ResponseData = res; // this solution to fix the paid declaration not showing a yellow message.
                    if (ResponseData && ResponseData.ContinueProcessInBackground) {
                        SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController.IsInBatchRequest = true;
                    }
                    
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    let myPaymentResponseData: CustomFileCreditResponseData = res;
                    this.RefreshDeclaration();
                    SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
                    SessionLocator.SelectedSession.CloseCurrentWindow();
                    
                })
                .catch(err => {
                    err = err || "PostSendPaymentOnly return Error)";
                    let messWindow = new MessageWindow();
                    messWindow.Show(err);
                    messWindow.WindowClosed.subscribe(() => {
                        SessionLocator.SelectedSession.CloseCurrentWindow();
                    });
                });

                this.declarationMessagesService.PostSendPaymentOnly(params)
                    .subscribe( res => {
                        if(!res.Result.HasException){
                            let messWindow = new MessageWindow();
                            messWindow.Show("ביטול הגשה הסתיים בהצלחה");
                            messWindow.WindowClosed.subscribe(() => {
                                SessionLocator.SelectedSession.CloseCurrentWindow();
                            });
                        }
                    }
                );
            }
        });
        confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.CancelPayment")); 
    }

    RefreshDeclaration() {
        this.declarationService.get(this.EntityPM.Id).subscribe((res: ServiceResponse) => {
            this.EntityPM = res.Result;
        });
    }

    ResetDeclarationNumberMethod() {
        this._DeclarationNumberandVersionId = null;//itzik:clear onstart on the house !!!
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.GetAnyRequest("2755", this.EntityPM.CustomFileNo, this.EntityPM.Tenant)
            .subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var requestSheets = response.Result;
                    var haveRS2755: boolean = false;
                    if (requestSheets == null || requestSheets.length == 0) {
                    } else {
                        haveRS2755 = true;
                    }
                    if (haveRS2755) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 150;
                        messageWindow.Title = "םיפוס מספר הצהרה";
                        messageWindow.Show("לם ניתן לםפס מספר הצהרה ,קיימת בקשה מסוג הגשת תשלום ");
                        return;
                    }


                    let confirm = new ConfirmWindow();
                    confirm.WindowClosed.subscribe((event: any) => {
                        if (confirm.Yes) {
                            this.EntityPM.ResetDeclarationNumber = true;
                            this.EntityPM.DeclarationNumber = null;
                            this.EntityPM.VersionId = null;
                            this.EntityPM.IsSignedVersion = false;
                            this.EntityPM.DeclarationStatusTypeCode = null;
                            this.EntityPM.DeclarationNumberandVersionId = null;
                            if (this.EntityPM.IsCourierDeclaration) { //Reset Courier Fields
                                this.EntityPM.CourierCustomStatusCode = null;
                                this.EntityPM.CourierSuspentionReasonCode = null;
                                //this.EntityPM.AcceptanceStatusCode = null;
                            }
                            if (!AppTool.IsNullOrEmpty(this.EntityPM.ExternalDeclarationNumber)) {
                                if (this.EntityPM.ExternalDeclarationNumber.includes("-")) {
                                    let index = this.EntityPM.ExternalDeclarationNumber.indexOf("-");

                                    let var1 = this.EntityPM.ExternalDeclarationNumber.substring(index + 1);
                                    let var2 = //int.Parse(var1)
                                        Number(var1) + 1;

                                    this.EntityPM.ExternalDeclarationNumber = this.EntityPM.ExternalDeclarationNumber.substring(0, index + 1) + var2.toString();

                                } else {
                                    this.EntityPM.ExternalDeclarationNumber = this.EntityPM.ExternalDeclarationNumber + "-1";
                                }
                                let token = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(
                                    (isSave) => {
                                        token.unsubscribe()
                                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                        if (isSave) {
                                            this._DeclarationNumberandVersionId = null;
                                            let window = new MessageWindow();
                                            window.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationReset"));
                                        }
                                    });
                                this.CurrentSession.CurrentEditComponent.SaveChanges();
                            }
                        }
                    });
                    confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.ResetDeclaration"));
                }

            }
            );
    }
    private TransferToCollectorMethod() {
        let confirmWindow = new ConfirmWindow();
        //confirmWindow.Show("םשר העברה לגובה");
        //confirmWindow.Unloaded += confirmWindow_Unloaded;

        //confirmWindow.cancelButton.Visibility = Visibility.Collapsed;

        confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.TransferToCollector"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                //this.ActualSendToTransfer();
                let myUnifreightInstructionController = new UnifreightInstructionController(this.EntityPM, "COLLECT_TRANSFER");
                myUnifreightInstructionController
                    .ShowInstruction(
                        () => {
                            console.log("Instruction return - continue TransferToCollectorMethod");
                            this.ActualSendToTransfer();
                        },
                        () => { console.log("Instruction return - do not continue 2 TransferToCollectorMethod!!"); }
                    );

            }
        });
    }

    private ActualSendToTransfer() {
        let objecttable//: ObjectTablePM = //window.ObjectTable.Where(d => d.Name == "Customs.Declaration").FirstOrDefault();
            = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];
        let searchParams = new CustomFileCreditRequestParams();

        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.Id;
        searchParams.LoggingObjectTableId = objecttable.Id;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Send Transfer Request";
        searchParams.ResponseName = "Get Transfer Response";
        searchParams.Mode = "Transfer";
        //searchParams.RequestVIA = SendRequestVIA.WebServiceBatch;
        //ForcePersonalSign = _ForcePersonalSign,


        var myCustomMessageProgressHelper = new CustomMessageProgressHelper(this.CurrentSession);
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(searchParams.PBId, 5, true);
        let declarationMessagesService = new DeclarationMessagesService();
        declarationMessagesService.PostSendTransferRequest(searchParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                myCustomMessageProgressHelper.MessageArrived = true;
                this.CurrentSession.StopBusyIndicator();

                var responseData: CustomFileCreditResponseData = myServiceResponse.Result;
                if (!AppTool.IsNullOrEmpty(CustomMessageProgressComponent.CurrCustomMessageProgressHelper)) {
                    CustomMessageProgressComponent.CurrCustomMessageProgressHelper.MessageArrived = true;
                }
                this.CurrentSession.StopBusyIndicator();
                //this.AnalyzeActualSendToTransfer(result);
                if (responseData.CreditStatus == "1") // moran 16.8.16 - AMI-57900
                {


                    let confirmWindow = new ConfirmWindow();
                    //confirmWindow.cancelButton.Visibility = Visibility.Collapsed;
                    confirmWindow.Show(responseData.UserMessage);
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (!confirmWindow.Yes) {
                            this.ActualSendToReTransfer();
                        }
                    });
                    return;
                }
                if (!responseData.HasException && responseData.Succeeded) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    if (this.CurrentSession.CurrentWindow != null) {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                } else {
                    if (!responseData.Succeeded && !AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                        let messageWindow = new MessageWindow();
                        //confirmWindow.cancelButton.Visibility = Visibility.Collapsed;
                        messageWindow.Show(responseData.UserMessage);
                        return;
                    }
                }

            });


    }
    private ActualSendToReTransfer() {
        let objecttable//: ObjectTablePM = //window.ObjectTable.Where(d => d.Name == "Customs.Declaration").FirstOrDefault();
            = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

        let searchParams = new CustomFileCreditRequestParams();

        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this.EntityPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.EntityPM.Id;
        searchParams.LoggingObjectTableId = objecttable.Id;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Send ReTransfer Request";
        searchParams.ResponseName = "Get ReTransfer Response";
        searchParams.Mode = "ReTransfer";
        var myCustomMessageProgressHelper = new CustomMessageProgressHelper(this.CurrentSession);
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(searchParams.PBId, 5, true);
        let declarationMessagesService = new DeclarationMessagesService();
        declarationMessagesService.PostSendTransferRequest(searchParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                myCustomMessageProgressHelper.MessageArrived = true;
                this.CurrentSession.StopBusyIndicator();

                var responseData: CustomFileCreditResponseData = myServiceResponse.Result;
                //this.AnalyzeResponseMessageForsendToReTransfer(responseData);
                if (!responseData.HasException && responseData.Succeeded) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    if (this.CurrentSession.CurrentWindow != null) {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                    else {
                        //message = "Send ReTransfer Request Failed";
                    }
                }
            });

    }
    private AnalyzeResponseMessageForsendToReTransfer(responseData: CustomFileCreditResponseData) {
        let message = "";
        if (responseData != null) {
            //message = responseData.UserMessage;
            if (AppTool.IsNullOrEmpty(responseData.UserMessage)) {
                if (!responseData.HasException && responseData.Succeeded) {
                    message = "Send ReTransfer Request Succeeded";
                }
                else {
                    message = "Send ReTransfer Request Failed";
                }
            }
        }
        else {
            message = "Service returned a null response!";
        }

        return message;
    }

    DeclarationsStatusRequestMethod() {
        let customsRequestMenuService = new CustomsRequestMenuService();
        let my = {
            "DeclarationNumber": this.EntityPM.DeclarationNumber,
            "CustomsFile": this.EntityPM.CustomFileNo,
            "DeclarationId": this.EntityPM.Id,
        };
        customsRequestMenuService.WindowClosed.subscribe(
            (myarg) => { this.CurrentSession.CurrentEditComponent.ReloadEntityPM() }
        );
        customsRequestMenuService.ShowModalAsEditMenuAction("8250", my);

    }

    SpecialActionRequestMethod() {
        let customsRequestMenuService = new CustomsRequestMenuService();
        let my = {
            "DeclarationNumber": this.EntityPM.DeclarationNumber,
            "CustomFileNo": this.EntityPM.CustomFileNo,
            "DeclarationId": this.EntityPM.Id,
        };
        customsRequestMenuService.WindowClosed.subscribe(
            (myarg) => { this.CurrentSession.CurrentEditComponent.ReloadEntityPM() }
        );
        customsRequestMenuService.ShowModalAsEditMenuAction("40", my);

    }

    private DeclarationRestoreMethod() {
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.CheckIfRequestInProgress("2750", this.EntityPM.CustomFileNo, this.EntityPM.Tenant)
            .subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var requestSheets = response.Result;
                    var haveRS2750: boolean = false;
                    if ((requestSheets == null || requestSheets.length == 0)
                        || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                        haveRS2750 = false;
                    } else {
                        haveRS2750 = true;
                    }
                    if (haveRS2750) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 150;
                        messageWindow.Title = "שיחזור מספר הצהרה";
                        messageWindow.Show("לם ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הצהרה בתהליך ");
                        return;
                    }

                    var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
                    declarationDisplayOnlyChecks.CheckIfRequestInProgress("2755", this.EntityPM.CustomFileNo, this.EntityPM.Tenant)
                        .subscribe((response: ServiceResponse) => {
                            if (!response.HasError) {
                                var requestSheets = response.Result;
                                var haveRS2755: boolean = false;
                                if ((requestSheets == null || requestSheets.length == 0)
                                    || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                                    haveRS2755 = false;
                                } else {
                                    haveRS2755 = true;
                                }
                                if (haveRS2755) {
                                    var messageWindow = new MessageWindow();
                                    messageWindow.Width = 400;
                                    messageWindow.Height = 150;
                                    messageWindow.Title = "שיחזור מספר הצהרה";
                                    messageWindow.Show("לם ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הגשת תשלום ");
                                    return;
                                }

                                let customsRequestMenuService = new CustomsRequestMenuService();
                                let my = {
                                    "DeclarationNumber": this.EntityPM.DeclarationNumber,
                                    "CustomsFile": this.EntityPM.CustomFileNo,
                                    "DeclarationId": this.EntityPM.Id,
                                    "LoggingEntityReference": this.EntityPM.Direction,
                                };
                                customsRequestMenuService.WindowClosed.subscribe(
                                    (myarg) => { this.CurrentSession.CurrentEditComponent.ReloadEntityPM() }
                                );

                                customsRequestMenuService.ShowModalAsEditMenuAction('8373', my);

                            }
                        });
                }
            });
    }

    _DocumentDeclarationId: string = null;
    _DeclarationNumberandVersionId: string = null;


    private buttonDisabled = false;
    /*async */OnPrintDeclarationFormClick() {
        if (this.buttonDisabled) {
            console.log("OnPrintDeclarationFormClick:abort")
            return;
        }
        try {
            this.buttonDisabled = true;
            this.PrintDeclarationFormMethod();
        } finally {
            setTimeout(() => {
                this.buttonDisabled = false;
            }, 2000);
        }
    }
    private PrintDeclarationFormMethod()//DeclarationViewModel declarationViewModel)
    {
        console.log("PrintDeclarationFormMethod()");
        if (!AppTool.IsNullOrEmpty(this._DocumentDeclarationId) && this._DeclarationNumberandVersionId == this.EntityPM.DeclarationNumberandVersionId) {
            this.ShowDocumentDeclaration();
            return
        }

        var myDeclarationWebService = new DeclarationWebService();
        myDeclarationWebService
            .GetDocumentDeclarationId(this.EntityPM.Id)
            .subscribe((myResponse: ServiceResponse) => {
                var myRes = myResponse.Result;
                this._DocumentDeclarationId = myRes.DocumentDeclarationId;
                this._DeclarationNumberandVersionId = myRes.DeclarationVersion;
                if (AppTool.IsNullOrEmpty(this._DocumentDeclarationId)) {
                    if (AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                        var msg = new MessageWindow();
                        msg.Width = 350;
                        msg.Show("There are no DeclarationNumber & Declaration form  Document (button IsDisabled)");
                        return;
                    } else {

                        if (this._DeclarationNumberandVersionId != this.EntityPM.DeclarationNumberandVersionId) {
                            this.CheckBeforeSendPrintRequest();
                            return;
                        }
                    }

                }
                if (this._DeclarationNumberandVersionId != this.EntityPM.DeclarationNumberandVersionId) {
                    this.CheckBeforeSendPrintRequest();
                    return;
                }
                this.ShowDocumentDeclaration();
            });

    }
    _IsPrintGateIsClosed: boolean = false;
    private CheckBeforeSendPrintRequest() {
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.CheckIfGeneralRequestInProgress("8302", this.EntityPM.CustomFileNo, this.EntityPM.Tenant)
            .subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var requestSheets = response.Result;
                    var haveRS8302: boolean = false;
                    if ((requestSheets == null || requestSheets.length == 0)
                        || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                        haveRS8302 = false;
                    } else {
                        haveRS8302 = true;
                    }
                    if (haveRS8302) {
                        var messageWindow = new MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 150;
                        messageWindow.Title = "טופס הצהרה";
                        messageWindow.Show("לם ניתן להציג טופס הצהרה ,קיימת בקשה דומה בתהליך ");
                        return;
                    }
                    if (this._IsPrintGateIsClosed) {
                        console.log("_IsPrintGateIsClosed:abort()")
                        return;
                    }
                    try {
                        this._IsPrintGateIsClosed = true;

                        this.SendPrintRequest();
                    } finally {
                        setTimeout(() => { this._IsPrintGateIsClosed = false; }, 2000);
                    }


                }
            });
    }

    SendPrintRequest() {

        let objecttable//: ObjectTablePM = //window.ObjectTable.Where(d => d.Name == "Customs.Declaration").FirstOrDefault();
            = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];


        let declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();

        var currRequestParams = new PrintRequestRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;

        currRequestParams.LoggingEntityId = this.EntityPM.Id;
        currRequestParams.LoggingObjectTableId = objecttable.Id;

        //currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        //currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.IsSearchByDeclarationRadio = true;
        currRequestParams.IsSearchByCargoRadio = false;
        currRequestParams.DeclarationNumber = [];
        currRequestParams.DeclarationNumber.push(this.EntityPM.DeclarationNumber);

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession, currRequestParams.PBId,
                "שליחת שםילתם להדפסת הצהרה", true)
            .then((res) => {
                let sub =
                    this.CurrentSession.CurrentEditComponent.LoadCompleted
                        .subscribe(succ => {
                            sub.unsubscribe();

                            var myDeclarationWebService = new DeclarationWebService();
                            myDeclarationWebService
                                .GetDocumentDeclarationId(this.EntityPM.Id)
                                .subscribe((myResponse: ServiceResponse) => {
                                    var myRes = myResponse.Result;
                                    this._DocumentDeclarationId = myRes.DocumentDeclarationId;
                                    if (!AppTool.IsNullOrEmpty(this._DocumentDeclarationId)) {
                                        this.ShowDocumentDeclaration();
                                    }
                                });
                        });
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

            }
            ).catch((err) => {
                //?????
            });


        declarationMessagesService.PostPrintRequestRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {

            });

    }


    ShowDocumentDeclaration() {

        DownloadManager.DownloadPage(this._DocumentDeclarationId);
    }
    private PrintTzrufaMethod(IsAccumalated: boolean) {


        //<--- Yuval Chalup 29.07.2015 TASK-14849
        //if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
            //CustomDomainContext customDomainContext = new CustomDomainContext();

            ;

            var myUnifreightPrintStimulController = new UnifreightController(//customDomainContext,
                this.EntityPM, "Logitude.Customs.MenuButtonHandlers.DeclarationMenuButtonsHandler.MyUnifreightPrintStimulController");


            var TSRUFA = "TSRUFA"
            if (IsAccumalated) {
                TSRUFA = "AccumalatedTSRUFA"
            }
            myUnifreightPrintStimulController.SendRequestPrintStimulToUnifreightAsync(TSRUFA);
            myUnifreightPrintStimulController.GetPromise().
                then((e) => {
                    var UnifreightResponseStatus = e.UnifreightResponseStatus;
                    var UnifreightMessage = e.UnifreightMessage;

                });

        } else {
            var token = ServiceHelper.GetLDocumentDownloadToken();
            let uri = AppTool.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=PrintTzrufa&entityId=" + this.EntityPM.Id + "&tempId=" + token;
            var win = window.open(uri, '_blank');

            win.focus();
        }
        ///Yuval Chalup 29.07.2015 TASK-14849 --->

    }

    private CloseDeclarationMethod() {


        if (this.EntityPM.Direction != "E") {
            this.ShowCloseDeclationWindow();
            return;
        }



        if (this.EntityPM.IsAmendmentDisplayOnly && this.EntityPM.AmendmentMessage == 'קיים תיקון הצהרה בסטטוס ממתינה לטיפול') {
            var txtMsg = this.EntityPM.AmendmentMessage + ". לם ניתן לסגור הצהרה.";

            var msg = new MessageWindow();
            msg.Width = 400;
            msg.RTL = true;
            msg.ShowWarningIcon = true;
            msg.IsMessageMultiLine;
            msg.Show(txtMsg);
            return;
        }


        var res = this.declarationWebService.GetWaitingDeclarationAmendment(this.EntityPM.CustomFileNo)
            .subscribe((response: ServiceResponse) => {
                if (!response.HasError) {

                    if (response.Result == null) {
                        //no waiting amendnent
                        this.ShowCloseDeclationWindow();
                    }
                    else {
                        var msg = new MessageWindow();
                        msg.Width = 350;
                        msg.RTL = true;
                        msg.ShowWarningIcon = true;

                        msg.Show("קיים תיקון הצהרה בטיפול. לם ניתן לסגור הצהרה.");
                    }
                }
            });

    }

    private ShowCloseDeclationWindow() {
        var args: any = {
            EntityPM: this.EntityPM,
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 770;
        logWindow.Height = 650;
        logWindow.Title = "סגירת הצהרה";
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/CloseDeclaration/ExportDeclarationClosingDataComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }



    private OpenNewContainerizationMethod() {

        if (this.containerizationIdList == "") {
            var args: any = {
                EntityPM: this.EntityPM,
                EntityIsDeclarationPM: "true",
            };
            var logWindow = new LogitudeWindow();
            logWindow.Width = 1220;
            logWindow.Height = 550;
            logWindow.Title = ("המכלה חדשה");
            logWindow.WindowArgs = args;
            logWindow.ShowCloseButton = true;
            logWindow.Show('./CustomsModules/CustomsContainerization/Components/NewEntity/NewContainerizationComponent');
            logWindow.WindowClosed.subscribe(($event: any) => {

                if ($event != "0" && $event != "cancel" && $event != null) {
                    this.OpenScreenContainerizationByFilter($event);
                }
            });

        }
        else {

            var contIdList = this.containerizationIdList.split(',')
            if (contIdList.length - 1 == 1) {

                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({
                            EntityId: contIdList[0],
                            ObjectTableName: "Customs.Containerization"
                        });
                    });
            }
            else {

                this.OpenScreenContainerizationByFilter(this.containerizationIdList);
            }

        }
    }

    private OpenScreenContainerizationByFilter(Ids) {
        this.CurrentSession.CloseCurrentEditComponent();
        var mySelectedItem: MainMenuItem = this.CurrentSession.MainMenuComponent.MainMenuItems.filter(m => m.TextCode == "General.MH.Containerization")[0];
        this.CurrentSession.MainMenuComponent.ChangeMenu(mySelectedItem);


        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("Id", Ids, null, null, "InListExact", false, false, false, "string", false, true);

        var listArgs = new ListComponentArgs();
        listArgs.QueryCode = "Customs.Containerization.OpenContainerization";
        listArgs.Filters = filters;
        listArgs.ObjectTableName = "Customs.Containerization";
        listArgs.HideBackButton = true;

        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);

                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }

    ///new shoshana
    private OpenExportStorageDeclarationMethod() {


        var args: any = {
            DeclarationPM: this.EntityPM,
            EntityIsDeclarationPM: "true",
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1220;
        logWindow.Height = 550;
        logWindow.Title = ("קישור םחסנות להצהרה");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/ExportStorageDecleration/ExportStorageDeclerationComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.ExportStoragePM = this.CurrentSession.CurrentEditComponent.EntityPM;
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });

    }

    private AddNotifications() {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this.NotificationPM = new NotificationPM

        this.NotificationPM.Tenant = SessionLocator.Tenant;
        this.NotificationPM.NotificationDefinitionCode = "5101N";
        this.NotificationPM.CreateDate = new Date();
        this.NotificationPM.AssigneToId = null;
        this.NotificationPM.AssigneToNotificationTypeCode = "I";
        this.NotificationPM.DeclarationOfficeCode = this.EntityPM.DeclarationOfficeCode;
        this.NotificationPM.IsHandledByCustomOffice = true;
        this.NotificationPM.EntityId = this.EntityPM.Id;
        this.NotificationPM.ObjectTableId = "1-343";
        this.NotificationPM.Reference1Number = this.EntityPM.CustomFileNo;
        this.NotificationPM.Reference2Number = this.EntityPM.CustomFileNo;;
        this.NotificationPM.DepartmentId = null;
        this.NotificationPM.ClosedByAssignee = null;
        this.NotificationPM.ClosedByCustomOfficeUserId = null;
        this.NotificationPM.IsClosedBCustomOffice = false;
        this.NotificationPM.IsClosedByAssignee = false;
        this.NotificationPM.DueDate = new Date();
        this.NotificationPM.IsSeenByAssignee = false;
        this.NotificationPM.ResponseNotes = null;
        this.NotificationPM.CreatedByRequestID = null;
        this.NotificationPM.Description = "הודעה יזומה למכס בגין הצהרה מספר " + this.EntityPM.DeclarationNumber + " כללי";
        this.NotificationPM.ResponseToMessage = this.EntityPM.DeclarationNumber;
        this.NotificationPM.CustomerId = this.EntityPM.CustomerId;
        this.NotificationPM.SearchFields = this.NotificationPM.Reference1Number, this.NotificationPM.Reference2Number, this.NotificationPM.CustomerId, this.NotificationPM.Description, this.NotificationPM.NotificationDefinitionCode
        this.NotificationPM.BadjCount = null;



        this.notificationPMService.insert(this.NotificationPM).subscribe((myResult: any) => {


            this.CurrentSession.CurrentEditComponent.LoadCompleted.emit(null);
            SessionLocator.SelectedSession.StopBusyIndicator();

        });

    }
    private PrintReleaseMethod() // moran 29.2.16 - Task 19807
    {

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
            var myUnifreightPrintStimulController = new UnifreightController(this.EntityPM,
                "Logitude.Customs.MenuButtonHandlers.DeclarationMenuButtonsHandler.MyUnifreightPrintStimulController");
            myUnifreightPrintStimulController.SendRequestPrintStimulToUnifreightAsync("RELEASE");
            myUnifreightPrintStimulController.GetPromise().
                then((e) => {
                    var UnifreightResponseStatus = e.UnifreightResponseStatus;
                    var UnifreightMessage = e.UnifreightMessage;
                });

        }
        else {
            var token = ServiceHelper.GetLDocumentDownloadToken();
            let uri = AppTool.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=PrintRelease&entityId=" + this.EntityPM.Id + "&tempId=" + token;
            var win = window.open(uri, '_blank');
            win.focus();
        }
    }

    public OpenPaymentOrderWindow() {
        if (this.EntityPM) {


            this.ActivateUnifreightInstruction();
        } else {
            console.log("No entityPM in menu buttons!!!");
        }
    }

    private OpenDeclarationPaymentComponent() {
        var args: any = {
            EntityPM: this.EntityPM,
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        if (this.EntityPM.Direction == "E") {
            logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.PaymentsExport");
            logWindow.Height = 400;
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/DeclarationPaymentExportComponent');
        }
        else {
            logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.Payments");
            logWindow.Height = 700;
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/DeclarationPayment/DeclarationPaymentComponent');
        }
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }

    ActivateUnifreightInstruction() {
        if (!AmitalGatewayUtil.Instance.AmitalBrowserInUse || this.EntityPM.Direction == "E") {
            this.OpenDeclarationPaymentComponent();
            return;
        }
        if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM.CustomFileNo, this.EntityPM.IsConvertedDeclaration, this.EntityPM.IsConnectedToUnifreight)) {
            var myEnterViewUnifreightInstructionController = new UnifreightController(
                this.EntityPM,
                "Logitude.Customs.ViewModels.DeclarationPayment.DeclarationPaymentTabViewModel.MyEnterViewUnifreightInstructionController");

            myEnterViewUnifreightInstructionController
                .GetPromise().then((e) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (e.UnifreightResponseStatus) {

                        this.OpenDeclarationPaymentComponent();
                        return;
                    }
                    else {
                        ///this.CurrentSession.CloseCurrentWindow();
                    }


                });
            this.CurrentSession.StartBusyIndicatorLoading();
            myEnterViewUnifreightInstructionController.SendRequestInstructionToUnifreightAsync("PAYHAND_ENTER");
        }
    }

    DisplayOnlyCheck() {

        if (this.CurrentSession.CurrentEditComponent) {
            this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        }
        if (this.IsDisplayOnly) {

            this.ApplyCheckMenuButtonsState(this.MenuButtons);
            return;
        }
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
                this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
                this.ApplyCheckMenuButtonsState(this.MenuButtons);
            }

        });
    }

    CourierPendingReasonMethod() {

        var logitudeWindow = new LogitudeWindow();
        var windowArgs: any = {};
        this.declarationCourierStatusPMService.get(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                windowArgs.DeclarationCourierStatus = response.Result
                windowArgs.Mode = "FromDeclaration";
                windowArgs.DeclarationId = this.EntityPM.Id;
                windowArgs.CourierHawb = this.EntityPM.MAWBCourierMaster;

                logitudeWindow.Width = 500;
                logitudeWindow.Height = 300;
                logitudeWindow.IsShowCloseButton = true;
                logitudeWindow.Title = "Pending";//TextCodeTranslator.Translate("Customs.CourierMaster.O.MarkPending");
                logitudeWindow.WindowArgs = windowArgs;
                //logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/CourierPendingReasonGeneralComponent');
                logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/DeclarationPendingsGeneralComponent');
                logitudeWindow.WindowClosed.subscribe(($event: any) => {
                    //this._CourierWorksheetSharedDataService.SendNextMessage("DoRefresh");
                });
            }
        });
    }

    CourierPendingReasonDeleteMethod() {

        this.CurrentSession.StartBusyIndicatorLoading();
        this.declarationCourierStatusPMService.get(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            var declarationCourierStatusPM: DeclarationCourierStatusPM = response.Result;
            if (declarationCourierStatusPM != null && (!AppTool.IsNullOrEmpty(declarationCourierStatusPM.CourierPendingReasonList))) {
                var confirm = new ConfirmWindow();
                confirm.Width = 350;
                confirm.Height = 200;
                confirm.Title = "מחיקת Pending";
                confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
                confirm.ShowNoButton = true;
                confirm.Show("הםם למחוק Pending?");
                confirm.WindowClosed.subscribe((event: any) => {
                    if (confirm.Yes) {
                        this.CurrentSession.StartBusyIndicatorSaving();
                        declarationCourierStatusPM.CourierPendingReasonCode = null;
                        declarationCourierStatusPM.PendingRemarks = null;
                        this.declarationCourierStatusPMService.update(declarationCourierStatusPM).subscribe((response: ServiceResponse) => {
                            this.CurrentSession.StopBusyIndicator();
                        });
                    }
                    confirm.Close();
                });
            }
            else {
                let window = new MessageWindow();
                window.Show(" Pending לם ניתן לבצע מחיקה, לתיק לם מוגדר ");
            }
        });
    }

    DeclarationClosureMethod() {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Show("הםם ברצונך לסגור םת ההצהרה ?");//TextCodeTranslator.Translate("Customs.PhysicalCheck.O.IsClosePhysicalCheck"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.declarationWebService.DeclarationClosureMethod(this.EntityPM.Id, this.EntityPM.Tenant)
                    .subscribe((response: ServiceResponse) => {
                        console.log("[response] DeclarationClosureMethod: ", response);
                        if (!response.HasError) {
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            let messageWindow = new MessageWindow();
                            messageWindow.Width = 300;
                            messageWindow.Height = 180;
                            messageWindow.Show("ההצהרה נסגרה בהצלחה");//TextCodeTranslator.Translate("Customs.PhysicalCheck.O.ClosePhysicalCheck"));
                        }
                    });
            }
        });
    }

    CancelDeclarationClosureMethod() {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Show("הםם ברצונך לבטל סגירת ההצהרה ?");//TextCodeTranslator.Translate("Customs.PhysicalCheck.O.IsClosePhysicalCheck"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.declarationWebService.CancelDeclarationClosureMethod(this.EntityPM.Id, this.EntityPM.Tenant)
                    .subscribe((response: ServiceResponse) => {
                        console.log("[response] CancelDeclarationClosureMethod: ", response);
                        if (!response.HasError) {
                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            let messageWindow = new MessageWindow();
                            messageWindow.Width = 300;
                            messageWindow.Height = 180;
                            messageWindow.Show("ביטול סגירה בוצע בהצלחה");//TextCodeTranslator.Translate("Customs.PhysicalCheck.O.ClosePhysicalCheck"));
                        }
                    });
            }
        });
    }

    DeclarationCustomsRequestsMethod() {

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1500;
        logWindow.Height = 1000;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "בקשות מכס";
        logWindow.Show('./CustomsModules/CustomsRequests/Components/CustomsRequestsComponent');
        this.CurrentSession.StopBusyIndicator();
    }
}

