declare var window: any;
import { Component, Output, EventEmitter, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ContainerizationPM } from '../../EntityPMs/ContainerizationPM';
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

import { CustomMessageProgressHelper, CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
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
import { ContainerizationMessagesService } from '../../Services/WebServices/ContainerizationMessagesService';
import { ContainerizationPMService } from '../../Services/StandardPMs/ContainerizationPMService';


export class ContainerizationMenuButtonsHandler implements OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.EntityResourceService = new EntityResourceService();
    }
    ///aaaaa: string = 10;
    //-----------------properties---------------------------//
    isValid: boolean = false;
    isButtonClicked: boolean = false;
    MenuButtonCode: string = null;
    public EntityPM: ContainerizationPM;
    checkTransfer: string = ""; // moran 4.8.16 - AMI-56804
    MenuButtons: MenuButtonPM[];
    IdentityKey: string;
    IsDisplayOnly: boolean;
    IsDisplayOnlyCheckDone: boolean;
    MenuButtonsStateChangedEvent: any;
    private EntityResourceService: EntityResourceService;
    //------------------------------------------------------//

    //Services
    private declarationPMService: DeclarationPMService = new DeclarationPMService();
    private declarationWebService: DeclarationWebService = new DeclarationWebService();
    private containerizationMessagesService: ContainerizationMessagesService = new ContainerizationMessagesService();
    private containerizationPMService: ContainerizationPMService = new ContainerizationPMService();

    private declarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();

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

            myMenuButtonPM.Index=1000
            menuButtons.push(myMenuButtonPM);
        }
        this.DisplayOnlyCheck();
    }

    ApplyCheckMenuButtonsState(menuButtons: MenuButtonPM[]) {
        let parentButton: MenuButtonPM;
          if (this.EntityPM != null) {
            if (this.CurrentSession.CurrentEditComponent != null) {

                var table = window.ObjectTables.filter(d => d.Name === 'Customs.Containerization')[0];

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
            this.MenuButtonClickDo();
        }
    }

    public MenuButtonClickDo() {
        if (true) {
            switch (this.MenuButtonCode) {
                case "AddDeclaration":
                    {
                        this.AddDeclarationMethod();//SaveDeclarationMethod("DeclarationRestore");
                        break;
                    }
                case "CancelContainerization":
                    {
                        this.CancelContainerizationMethod();//SaveDeclarationMethod("DeclarationRestore");
                        break;
                    }

            }
        }
    }
  
    AddDeclarationMethod() {
        var args: any = {
            EntityPM: this.EntityPM,
        }; 
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1200;
        logWindow.Height = 550;
        logWindow.Title = ("עדכון המכלה");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsContainerization/Components/NewEntity/NewContainerizationComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
        });
    }

    getParams(response: ServiceResponse, event: any) {
        var params: GenericRequestParams = new GenericRequestParams();
        params.Tenant = SessionLocator.Tenant;
        params.AppicationId = "12345";
        params.RequestVIA = event.RequestVIA;
        params.ForcePersonalSign = event.ForcePersonalSign;
        params.LoggingEnabled = true;
        params.LoggingEntityId = response.Result.Id;
        params.LoggingUserId = SessionLocator.LoggedUserId;
        return params
    }
    CancelContainerizationMethod() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Show("האם ברצונך לבטל את ההמכלה ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes)
            {
                this.EntityPM.OperationMode = "3";
                this.containerizationPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                    if (!response.HasError)
                    {
                        this.CurrentSession.StartBusyIndicator("Sending...");
                        this.containerizationMessagesService.SendContainerization(this.getParams(response, event)).subscribe((response: ServiceResponse) => {
                            console.log("[response] CancelContainerizationMethod: ", response);
                            this.CurrentSession.StopBusyIndicator();
                            if (!response.Result.HasException) {
                                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                this.CurrentSession.CurrentEditComponent.LoadCompleted.emit(true);
                                let messageWindow = new MessageWindow();
                                messageWindow.Width = 300;
                                messageWindow.Height = 180;
                                messageWindow.Show("ההמכלה בוטלה בהצלחה");
                            }
                            else {
                                let messageWindow = new MessageWindow();
                                messageWindow.Width = 300;
                                messageWindow.Height = 180;
                                messageWindow.Title = "שליחה נכשלה";
                                messageWindow.RTL = true;
                                messageWindow.ShowErrorIcon = true;
                                messageWindow.Show(response.Result.UserMessage);
                            }
                        });
                    }
                });
            }
        });
    }

    DisplayOnlyCheck() {
    }
}

